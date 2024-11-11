using DotnetAPI.Models;

namespace DotnetAPI.Data
{
    public interface IUserRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToAdd);
        public IEnumerable<User> GetUsers();
        public User GetSingleUser(string userId);
        public IEnumerable<UserSalary> GetUserSalaries();
        public UserSalary GetSingleUserSalary(string userId);
        public IEnumerable<UserJobInfo> GetUserJobInfos();
        public UserJobInfo GetSingleUserJobInfo(string userId);
    }
}