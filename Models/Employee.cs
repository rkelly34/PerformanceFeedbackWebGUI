using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerformanceFeedbackWebGUI.Models
{
    public class Employee
    {
        protected static string host = "localhost";
        public long EmployeeId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        
        public ICollection<Review> Reviews { get; set; }
        public Employee()
        {
        }

        public Employee(long EmployeeId)
        {
            using (var redis = new RedisClient(host))
            {
                var e = redis.GetAll<Employee>().FirstOrDefault(x => x.EmployeeId == EmployeeId);
                EmployeeId = e.EmployeeId;
                Name = e.Name;
                Username = e.Username;
                Password = e.Password;
                IsAdmin = e.IsAdmin;
                Reviews = e.Reviews;
            }
        }

        public long Authenticate(string username, string password)
        {
            using (var redis = new RedisClient(host))
            {
                var all = redis.GetAll<Employee>();
                var e = all.FirstOrDefault(x => x.Username.ToLower() == username.ToLower() 
                    && x.Password.ToLower() == password.ToLower());
                if (e == null)
                    return 0;
                return e.EmployeeId;
            }
        }

        public static List<Employee> GetEmployeeList()
        {
            using (var redis = new RedisClient(host))
            {
                return redis.GetAll<Employee>().ToList();
            }
        }

    }
}