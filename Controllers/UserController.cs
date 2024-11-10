using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        DataContextDapper _dapper;

        public UserController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        // ----- user controller methods -----
        [HttpGet("users")]
        public IEnumerable<User> GetUsers()
        {
            string sql = "select * from TutorialAppSchema.Users";
            IEnumerable<User> users = _dapper.LoadData<User>(sql);

            return users;
        }

        [HttpGet("users/{userId}")]
        public User GetSingleUser(string userId)
        {
            string sql = @"select * from TutorialAppSchema.Users 
                        where userId = " + userId;
            User user = _dapper.LoadDataSingle<User>(sql);

            return user;
        }

        [HttpPut("updateUser")]
        public IActionResult UpdateUser(User user)
        {
            string sql = "update TutorialAppSchema.Users set FirstName = '"
            + user.FirstName +
            "', LastName = '" + user.LastName +
            "', Email = '" + user.Email +
            "', Gender = '" + user.Gender +
            "', Active = '" + user.Active
            + "' where UserId = " + user.UserId;

            if (_dapper.ExecuteData(sql))
            {
                return Ok("User updated successfully");
            }
            else
            {
                return BadRequest("User update failed");
            }
        }

        [HttpPost("createUser")]
        public IActionResult CreateUser(UserToAddDto user)
        {
            string sql = "insert into TutorialAppSchema.Users (FirstName , LastName, Email, Gender, Active) values ('"
            + user.FirstName + "', '" + user.LastName + "', '" + user.Email + "', '" + user.Gender + "', '" + user.Active + "')";
            Console.WriteLine(sql);
            if (_dapper.ExecuteData(sql))
            {
                return Ok("User created successfully");
            }
            else
            {
                return BadRequest("User creation failed");
            }
        }

        [HttpDelete("deleteUser/{userId}")]
        public IActionResult DeleteUser(string userId)
        {
            string sql = "delete from TutorialAppSchema.Users where UserId = " + userId;
            if (_dapper.ExecuteData(sql))
            {
                return Ok("User deleted successfully");
            }
            else
            {
                return BadRequest("User deletion failed");
            }
        }

        // ----- user salary controller methods -----
        [HttpGet("userSalaries")]
        public IEnumerable<UserSalary> GetUserSalaries()
        {
            string sql = "select * from TutorialAppSchema.UserSalaries";
            IEnumerable<UserSalary> userSalaries = _dapper.LoadData<UserSalary>(sql);

            return userSalaries;
        }

        [HttpGet("userSalaries/{userId}")]
        public UserSalary GetSingleUserSalary(string userId)
        {

            string sql = @"select * from TutorialAppSchema.UserSalaries 
                        where userId = " + userId;
            UserSalary userSalary = _dapper.LoadDataSingle<UserSalary>(sql);

            return userSalary;
        }

        [HttpPut("updateUserSalary")]
        public IActionResult UpdateUserSalary(UserSalary userSalary)
        {
            string sql = "update TutorialAppSchema.UserSalaries set Salary = '"
            + userSalary.Salary
            + "' where UserId = " + userSalary.UserId;

            if (_dapper.ExecuteData(sql))
            {
                return Ok("User salary updated successfully");
            }
            else
            {
                return BadRequest("User salary update failed");
            }
        }

        [HttpPost("createUserSalary")]
        public IActionResult CreateUserSalary(UserSalaryToAddDto userSalary)
        {
            string sql = "insert into TutorialAppSchema.UserSalaries (Salary) values ('" + userSalary.Salary + "')";

            if (_dapper.ExecuteData(sql))
            {
                return Ok("User salary created successfully");
            }
            else
            {
                return BadRequest("User salary creation failed");
            }
        }

        [HttpDelete("deleteUserSalary/{userId}")]
        public IActionResult DeleteUserSalary(string userId)
        {
            string sql = "delete from TutorialAppSchema.UserSalaries where UserId = " + userId;
            if (_dapper.ExecuteData(sql))
            {
                return Ok("User salary deleted successfully");
            }
            else
            {
                return BadRequest("User salary deletion failed");
            }
        }

        // ----- user job info controller methods -----
        [HttpGet("userJobInfos")]
        public IEnumerable<UserJobInfo> GetUserJobInfos()
        {
            string sql = "select * from TutorialAppSchema.UserJobInfos";
            IEnumerable<UserJobInfo> userJobInfos = _dapper.LoadData<UserJobInfo>(sql);

            return userJobInfos;
        }

        [HttpGet("userJobInfos/{userId}")]
        public UserJobInfo GetSingleUserJobInfo(string userId)
        {
            string sql = @"select * from TutorialAppSchema.UserJobInfos 
                        where userId = " + userId;
            UserJobInfo userJobInfo = _dapper.LoadDataSingle<UserJobInfo>(sql);

            return userJobInfo;
        }

        [HttpPut("updateUserJobInfo")]
        public IActionResult UpdateUserJobInfo(UserJobInfo userJobInfo)
        {
            string sql = "update TutorialAppSchema.UserJobInfos set JobTitle = '"
            + userJobInfo.JobTitle +
            "', Department = '" + userJobInfo.Department
            + "' where UserId = " + userJobInfo.UserId;

            if (_dapper.ExecuteData(sql))
            {
                return Ok("User job info updated successfully");
            }
            else
            {
                return BadRequest("User job info update failed");
            }
        }

        [HttpPost("createUserJobInfo")]
        public IActionResult CreateUserJobInfo(UserJobInfoToAddDto userJobInfo)
        {
            string sql = "insert into TutorialAppSchema.UserJobInfos (JobTitle, Department) values ('"
            + userJobInfo.JobTitle + "', '" + userJobInfo.Department + "')";

            if (_dapper.ExecuteData(sql))
            {
                return Ok("User job info created successfully");
            }
            else
            {
                return BadRequest("User job info creation failed");
            }
        }

        [HttpDelete("deleteUserJobInfo/{userId}")]
        public IActionResult DeleteUserJobInfo(string userId)
        {
            string sql = "delete from TutorialAppSchema.UserJobInfos where UserId = " + userId;
            if (_dapper.ExecuteData(sql))
            {
                return Ok("User job info deleted successfully");
            }
            else
            {
                return BadRequest("User job info deletion failed");
            }
        }

    }
}

