using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    DataContextDapper _dapper;

    public UserController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

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
}

