using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace PerformanceFeedbackWebGUI.Models
{
    public class AdminEmployee : Employee
    {

        public enum AdminType
        {
            Add,
            Update,
            Delete
        }

        public AdminType adminType { get; set; }
            
        public AdminEmployee() : base()
        {
        }

        public bool Action()
        {
            if (adminType == AdminType.Add)
                return Add();
            if (adminType == AdminType.Delete)
                return Delete();
            return Update();
        }

        private bool Add()
        {
            using (var redis = new RedisClient(host))
            {
                if (EmployeeId != default(long))
                    return false;

                EmployeeId = redis.As<Employee>().GetNextSequence();

                redis.As<Employee>().Store(this);
                return true;
            }
        }

        private bool Update()
        {
            using (var redis = new RedisClient(host))
            {

                var employees = redis.As<Employee>();

                var update = employees.GetById(EmployeeId);
                Thread.Sleep(500);
                var all = redis.GetAll<Employee>();
                if (update == null)
                    return false;
                if (Username != default(string))
                    update.Username = Username;
                if (Name != default(string))
                    update.Name = Name;
                if (Password != default(string))
                    update.Password = Password;
                employees.Store(update);
                return true;
            }
        }

        private bool Delete()
        {
            using (var redis = new RedisClient(host))
            {

                var employees = redis.As<Employee>();
                var all = redis.GetAll<Employee>();
                if (!all.Any(x => x.EmployeeId == EmployeeId))
                    return false;
                employees.DeleteRelatedEntities<Review>(EmployeeId);
                var e = all.FirstOrDefault(x => x.EmployeeId == EmployeeId);

                employees.DeleteById(EmployeeId);
                var s = redis.GetAll<Employee>().ToList();
                
                return true;
            }
        }
    }

}