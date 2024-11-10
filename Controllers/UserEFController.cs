using AutoMapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserEFController : ControllerBase
    {
        DataContextEF _EF;
        IMapper _mapper;

        public UserEFController(IConfiguration config)
        {
            _EF = new DataContextEF(config);
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserToAddDto, User>();
            }));
        }

        [HttpGet("users")]
        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = _EF.Users.ToList<User>();

            return users;
        }

        [HttpGet("users/{userId}")]
        public User GetSingleUser(string userId)
        {
            User? user = _EF.Users.Where(u => u.UserId == Convert.ToInt32(userId)).FirstOrDefault();
            if (user != null)
            {
                return user;
            }

            throw new Exception("User not found");
        }

        [HttpPut("updateUser")]
        public IActionResult UpdateUser(User user)
        {
            User? userDB = _EF.Users.Where(u => u.UserId == Convert.ToInt32(user.UserId)).FirstOrDefault();

            if (userDB != null)
            {
                userDB.FirstName = user.FirstName;
                userDB.LastName = user.LastName;
                userDB.Email = user.Email;
                userDB.Gender = user.Gender;
                userDB.Active = user.Active;

                if (_EF.SaveChanges() > 0)
                {
                    return Ok("User updated successfully");
                }

                throw new Exception("User not found");
            }

            throw new Exception("User not found");
        }

        [HttpPost("createUser")]
        public IActionResult CreateUser(UserToAddDto user)
        {
            User? userDB = _EF.Users.Where(u => u.Email == user.Email).FirstOrDefault<User>();

            if (userDB != null)
            {
                return BadRequest("User already exists");
            }

            User newUser = _mapper.Map<User>(user);

            _EF.Users.Add(newUser);
            if (_EF.SaveChanges() > 0)
            {
                return Ok("User created successfully");
            }

            return BadRequest("User creation failed");
        }

        [HttpDelete("deleteUser/{userId}")]
        public IActionResult DeleteUser(string userId)
        {
            User? userDB = _EF.Users.Where(u => u.UserId == Convert.ToInt32(userId)).FirstOrDefault<User>();

            if (userDB != null)
            {
                _EF.Users.Remove(userDB);
                if (_EF.SaveChanges() > 0)
                {
                    return Ok("User deleted successfully");
                }

                throw new Exception("User not found");
            }

            throw new Exception("User not found");
        }
    }
}

