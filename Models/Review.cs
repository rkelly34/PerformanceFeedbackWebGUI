using ServiceStack.Redis;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PerformanceFeedbackWebGUI.Models
{
    public class Review
    {
        protected static string host = "localhost";
        public static ICollection<Employee> EmployeeList { get; set; }

        public long ReviewId { get; set; }
        public long EmployeeId { get; set; }
        public string ReviewText { get; set; }
        public ReviewScore Rating { get; set; }

        public Review()
        {

        }

        public bool Action()
        {
            using (var redis = new RedisClient(host))
            {
                if (EmployeeId == default(long) || this == new Review())
                    return false;

                ReviewId = redis.As<Review>().GetNextSequence();
                redis.As<Review>().Store(this);
                return true;
            }
        }
    }

    public enum ReviewScore
    {
        Poor = 0,
        Improvement = 1,
        Good = 2,
        Great = 3,
        Exceptional = 4
    }

}