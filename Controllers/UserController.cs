using System.Data;
using Dapper;
using DotnetAPI.Data;
using DotnetAPI.Helpers;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        DataContextDapper _dapper;
        ReusableSql _reusableSql;

        public UserController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
            _reusableSql = new ReusableSql(config);
        }

        // ----- user controller methods -----
        [HttpGet("users/{userId}/{Active}")]
        public IEnumerable<User> GetUsers(int userId, bool Active)
        {
            string sql = @"EXEC TutorialAppSchema.spUsers_Get";
            string stringParameters = "";
            DynamicParameters sqlParameters = new DynamicParameters();

            if (userId != 0)
            {
                stringParameters += ", @UserId = @UserIdParameter";
                sqlParameters.Add("@UserIdParameter", userId, DbType.Int32);
            }
            if (Active)
            {
                stringParameters += ", @Active = @ActiveParameter";
                sqlParameters.Add("@ActiveParameter", Active, DbType.Boolean);
            }

            if (stringParameters.Length > 0)
            {
                sql += stringParameters.Substring(1);
            }

            IEnumerable<User> users = _dapper.LoadDataWithParameters<User>(sql, sqlParameters);

            return users;
        }

        [HttpPut("upsertUser")]
        public IActionResult UpsertUser(User user)
        {
            if (_reusableSql.UpsertUser(user))
            {
                return Ok();
            }

            throw new Exception("Failed to Update User");
        }

        [HttpDelete("deleteUser/{userId}")]
        public IActionResult DeleteUser(string userId)
        {
            string sql = "EXEC TutorialAppSchema.spUser_Delete @UserId = " + userId.ToString();
            if (_dapper.ExecuteData(sql))
            {
                return Ok("User deleted successfully");
            }
            else
            {
                return BadRequest("User deletion failed");
            }
        }
    }
}

