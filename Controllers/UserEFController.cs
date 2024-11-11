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
        IUserRepository _userRepository;
        IMapper _mapper;

        public UserEFController(IConfiguration config, IUserRepository userRepository
        )
        {
            _userRepository = userRepository;
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserToAddDto, User>();
            }));
        }

        // ----- user controllers -----
        [HttpGet("users")]
        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = _userRepository.GetUsers();

            return users;
        }

        [HttpGet("users/{userId}")]
        public User GetSingleUser(string userId)
        {
            return _userRepository.GetSingleUser(userId);
        }

        [HttpPut("updateUser")]
        public IActionResult UpdateUser(User user)
        {
            User? userDB = _userRepository.GetSingleUser(user.UserId.ToString());

            if (userDB != null)
            {
                userDB.FirstName = user.FirstName;
                userDB.LastName = user.LastName;
                userDB.Email = user.Email;
                userDB.Gender = user.Gender;
                userDB.Active = user.Active;

                if (_userRepository.SaveChanges())
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
            User newUser = _mapper.Map<User>(user);

            _userRepository.AddEntity(newUser);
            if (_userRepository.SaveChanges())
            {
                return Ok("User created successfully");
            }

            return BadRequest("User creation failed");
        }

        [HttpDelete("deleteUser/{userId}")]
        public IActionResult DeleteUser(string userId)
        {
            User? userDB = _userRepository.GetSingleUser(userId);

            if (userDB != null)
            {
                _userRepository.RemoveEntity(userDB);
                if (_userRepository.SaveChanges())
                {
                    return Ok("User deleted successfully");
                }

                throw new Exception("User not found");
            }

            throw new Exception("User not found");
        }

        // ----- user salary controllers -----
        [HttpGet("userSalary")]
        public IEnumerable<UserSalary> GetUserSalaries()
        {
            return _userRepository.GetUserSalaries();
        }

        [HttpGet("userSalary/{userId}")]
        public UserSalary GetSingleUserSalary(string userId)
        {
            return _userRepository.GetSingleUserSalary(userId);
        }

        [HttpPut("updateUserSalary")]
        public IActionResult UpdateUserSalary(UserSalary userSalary)
        {
            UserSalary? userSalaryDB = _userRepository.GetSingleUserSalary(userSalary.UserId.ToString());

            if (userSalaryDB != null)
            {
                userSalaryDB.Salary = userSalary.Salary;

                if (_userRepository.SaveChanges())
                {
                    return Ok("User salary updated successfully");
                }

                throw new Exception("User salary not found");
            }

            throw new Exception("User salary not found");
        }

        [HttpPost("createUserSalary")]
        public IActionResult CreateUserSalary(UserSalary userSalary)
        {
            _userRepository.AddEntity<UserSalary>(userSalary);
            if (_userRepository.SaveChanges())
            {
                return Ok("User salary created successfully");
            }

            return BadRequest("User salary creation failed");
        }

        [HttpDelete("deleteUserSalary/{userId}")]
        public IActionResult DeleteUserSalary(string userId)
        {
            UserSalary? userSalaryDB = _userRepository.GetSingleUserSalary(userId);

            if (userSalaryDB != null)
            {
                _userRepository.RemoveEntity(userSalaryDB);
                if (_userRepository.SaveChanges())
                {
                    return Ok("User salary deleted successfully");
                }

                throw new Exception("User salary not found");
            }

            throw new Exception("User salary not found");
        }

        // ----- user job info controllers -----
        [HttpGet("userJobInfo")]
        public IEnumerable<UserJobInfo> GetUserJobInfos()
        {
            return _userRepository.GetUserJobInfos();
        }

        [HttpGet("userJobInfo/{userId}")]
        public UserJobInfo GetSingleUserJobInfo(string userId)
        {
            return _userRepository.GetSingleUserJobInfo(userId);
        }

        [HttpPut("updateUserJobInfo")]
        public IActionResult UpdateUserJobInfo(UserJobInfo userJobInfo)
        {
            UserJobInfo? userJobInfoDB = _userRepository.GetSingleUserJobInfo(userJobInfo.UserId.ToString());

            if (userJobInfoDB != null)
            {
                userJobInfoDB.JobTitle = userJobInfo.JobTitle;
                userJobInfoDB.Department = userJobInfo.Department;

                if (_userRepository.SaveChanges())
                {
                    return Ok("User job info updated successfully");
                }

                throw new Exception("User job info not found");
            }

            throw new Exception("User job info not found");
        }

        [HttpPost("createUserJobInfo")]
        public IActionResult CreateUserJobInfo(UserJobInfo userJobInfo)
        {
            UserJobInfo? userJobInfoDB = _userRepository.GetSingleUserJobInfo(userJobInfo.UserId.ToString());

            if (userJobInfoDB != null)
            {
                return BadRequest("User job info already exists");
            }

            _userRepository.AddEntity<UserJobInfo>(userJobInfo);
            if (_userRepository.SaveChanges())
            {
                return Ok("User job info created successfully");
            }

            return BadRequest("User job info creation failed");
        }

        [HttpDelete("deleteUserJobInfo/{userId}")]
        public IActionResult DeleteUserJobInfo(string userId)
        {
            UserJobInfo? userJobInfoDB = _userRepository.GetSingleUserJobInfo(userId);

            if (userJobInfoDB != null)
            {
                _userRepository.RemoveEntity(userJobInfoDB);
                if (_userRepository.SaveChanges())
                {
                    return Ok("User job info deleted successfully");
                }

                throw new Exception("User job info not found");
            }

            throw new Exception("User job info not found");
        }
    }
}

