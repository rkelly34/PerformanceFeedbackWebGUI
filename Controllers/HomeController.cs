using System;
using PerformanceFeedbackWebGUI.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace PerformanceFeedbackWebGUI.Controllers
{
    public class HomeController : Controller
    {
        
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Message = Session["testRedisSession"];
            var e = new Employee();
            return View(e);
        }

        [HttpPost]
        public ActionResult Index(Employee employee)
        {
            var id = employee.Authenticate(employee.Username, employee.Password);
            if (id > 0)
            {
                Session["EmployeeId"] = id;
                return Redirect("/Home/Main");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Main()
        {
            ViewBag.Message = Session["testRedisSession"];
            ViewBag.EmployeeList = GetEmployees();
            ViewBag.ReviewList = GetReviews();
            var r = new Review();
            return View(r);
        }

        [HttpPost]
        public ActionResult Main(Review r)
        {
            r.Action();

            ViewBag.Message = string.Format("ReviewId {0}", r.ReviewId);
            return Redirect("/Home/Main");
        }

        [HttpGet]
        public ActionResult Admin()
        {
            ViewBag.Message = "EmployeeId only to be filled in when updating or deleting.";
            var e = new AdminEmployee();
            return View(e);
        }

        [HttpPost]
        public ActionResult Admin(AdminEmployee employee)
        {
            var result = employee.Action();

            ViewBag.Message = string.Format("EmployeeId {0}", employee.EmployeeId);
            return Redirect("/Home/Admin");
        }

        private IList<SelectListItem> GetEmployees()
        {
            IList<SelectListItem> result = new List<SelectListItem>();
            var employees = Employee.GetEmployeeList();
            foreach (var e in employees)
            {
                result.Add(new SelectListItem { Value = e.EmployeeId.ToString(), Text = e.Name });
            }
            return result;
        }

        private IList<Review> GetReviews()
        {
            var id = Convert.ToInt64(Session["EmployeeId"]);
            var e = new Employee(id);

            if (e.Reviews == null)
                return new List<Review>();
            return e.Reviews.ToList();
        }
    }
}