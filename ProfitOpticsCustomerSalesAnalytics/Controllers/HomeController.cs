using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProfitOpticsCustomerSalesAnalytics.Controllers
{
    public class HomeController : Controller
    {
        private ReportRepository _repo;

        public HomeController()
        {
            _repo = new ReportRepository();
        }

        public ActionResult Index()
        {
            return View();
        }

       
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}