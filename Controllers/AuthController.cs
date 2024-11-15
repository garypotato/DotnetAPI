using System.Data;
using System.Security.Cryptography;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using DotnetAPI.Helpers;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DotnetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        private readonly IConfiguration _config;
        private readonly AuthHelper _authHelper;
        public AuthController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
            _config = config;
            _authHelper = new AuthHelper(config);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration.Password == userForRegistration.PasswordConfirm)
            {
                string sqlCheckUserExists = "select * from TutorialAppSchema.Users where Email = '"
                    + userForRegistration.Email
                    + "'";
                IEnumerable<User> users = _dapper.LoadData<User>(sqlCheckUserExists);

                if (users.ToList().Count == 0)
                {
                    byte[] passwordSalt = new byte[128 / 8];

                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(passwordSalt);
                    }

                    byte[] passwordHash = _authHelper.GetPasswordHash(userForRegistration.Password, passwordSalt);

                    string sqlInsertUserToAuth = "insert into TutorialAppSchema.Auth (Email, PasswordHash, PasswordSalt) values ('"
                        + userForRegistration.Email
                        + "', @passwordHash, @passwordSalt)";

                    List<SqlParameter> SqlParameter = new List<SqlParameter>();

                    SqlParameter passwordSaltParameter = new SqlParameter("@passwordSalt", SqlDbType.VarBinary);
                    passwordSaltParameter.Value = passwordSalt;
                    SqlParameter passwordHashParameter = new SqlParameter("@passwordHash", SqlDbType.VarBinary);
                    passwordHashParameter.Value = passwordHash;

                    SqlParameter.Add(passwordSaltParameter);
                    SqlParameter.Add(passwordHashParameter);

                    if (_dapper.ExecuteSqlWithParameter(sqlInsertUserToAuth, SqlParameter))
                    {
                        string sqlInsertUserToUsers = "insert into TutorialAppSchema.Users (FirstName , LastName, Email, Gender, Active) values ('"
                            + userForRegistration.FirstName + "', '"
                            + userForRegistration.LastName + "', '"
                            + userForRegistration.Email + "', '"
                            + userForRegistration.Gender + "', 1)";

                        if (_dapper.ExecuteData(sqlInsertUserToUsers))
                        {
                            return Ok();
                        }

                        throw new Exception("Failed to add user in Users table");
                    }

                    throw new Exception("Failed to register user");
                }

                throw new Exception("User already exists");
            }

            throw new Exception("Passwords do not match");
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLogin)
        {
            string sqlForHashAndSalt = "select PasswordHash, PasswordSalt from TutorialAppSchema.Auth where Email = '"
                + userForLogin.Email
                + "'";

            UserForLogConfirmationDto? userForLogConfirmation = _dapper
                .LoadDataSingle<UserForLogConfirmationDto>(sqlForHashAndSalt);

            byte[] passwordHash = _authHelper.GetPasswordHash(userForLogin.Password, userForLogConfirmation.PasswordSalt);

            for (int i = 0; i < passwordHash.Length; i++)
            {
                if (passwordHash[i] != userForLogConfirmation.PasswordHash[i])
                {
                    return StatusCode(401, "Invalid password");
                }
            }

            int userId = _dapper.LoadDataSingle<int>("select UserId from TutorialAppSchema.Users where Email = '"
                + userForLogin.Email
                + "'");

            return Ok(new Dictionary<string, string>
            {
                {
                    "token", _authHelper.CreateToken(userId)
                }
            });
        }

        [HttpGet("RefreshToken")]
        public IActionResult RefreshToken()
        {
            string userIdString = User.FindFirst("userId")?.Value + "";

            string userIdSql = "select UserId from TutorialAppSchema.Users where UserId = "
                + userIdString;

            int userIdFromDB = _dapper.LoadDataSingle<int>(userIdSql);

            return Ok(new Dictionary<string, string>
            {
                {"token", _authHelper.CreateToken(userIdFromDB)}
            }
            );
        }
    }
}