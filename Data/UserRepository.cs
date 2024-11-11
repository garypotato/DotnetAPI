using DotnetAPI.Models;

namespace DotnetAPI.Data
{
    public class UserRepository : IUserRepository
    {
        DataContextEF _entityFramework;

        public UserRepository(IConfiguration config)
        {
            _entityFramework = new DataContextEF(config);
        }

        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() >= 0;
        }

        public void AddEntity<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _entityFramework.Add(entityToAdd);
            }
        }

        public void RemoveEntity<T>(T entityToRemove)
        {
            if (entityToRemove != null)
            {
                _entityFramework.Remove(entityToRemove);
            }
        }

        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = _entityFramework.Users.ToList<User>();

            return users;
        }

        public User GetSingleUser(string userId)
        {
            User? user = _entityFramework.Users.Where(u => u.UserId == Convert.ToInt32(userId)).FirstOrDefault();
            if (user != null)
            {
                return user;
            }

            throw new Exception("User not found");
        }

        public IEnumerable<UserSalary> GetUserSalaries()
        {
            IEnumerable<UserSalary> userSalaries = _entityFramework.UserSalary.ToList<UserSalary>();

            return userSalaries;
        }

        public UserSalary GetSingleUserSalary(string userId)
        {
            UserSalary? userSalary = _entityFramework.UserSalary.Where(u => u.UserId == Convert.ToInt32(userId)).FirstOrDefault();
            if (userSalary != null)
            {
                return userSalary;
            }

            throw new Exception("User salary not found");
        }

        public IEnumerable<UserJobInfo> GetUserJobInfos()
        {
            IEnumerable<UserJobInfo> userJobInfos = _entityFramework.UserJobInfo.ToList<UserJobInfo>();

            return userJobInfos;
        }

        public UserJobInfo GetSingleUserJobInfo(string userId)
        {
            UserJobInfo? userJobInfo = _entityFramework.UserJobInfo.Where(u => u.UserId == Convert.ToInt32(userId)).FirstOrDefault();
            if (userJobInfo != null)
            {
                return userJobInfo;
            }

            throw new Exception("User job info not found");
        }
    }
}