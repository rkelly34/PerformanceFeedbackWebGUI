using PerformanceFeedbackWebGUI.Models;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PerformanceFeedbackWebGUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start()
        {
            Session["testRedisSession"] = "Message from the redis ression";

            using (var redis = new RedisClient("localhost"))
            {
                var employees = redis.As<Employee>();
                employees.DeleteById(1);
                var reviews = redis.As<Review>();
                var a = new Employee() { EmployeeId = 1, Name = "Admin", Username = "admin", Password = "admin", IsAdmin = true };
                var review1 = new Review() { ReviewId = reviews.GetNextSequence(), EmployeeId = 1, ReviewText = "A good review", Rating = ReviewScore.Good };
                var review2 = new Review() { ReviewId = reviews.GetNextSequence(), EmployeeId = 1, ReviewText = "A bad review", Rating = ReviewScore.Poor };
                employees.Store(a);
                reviews.Store(review1);
                reviews.Store(review2);
            }
        }
    }
}
