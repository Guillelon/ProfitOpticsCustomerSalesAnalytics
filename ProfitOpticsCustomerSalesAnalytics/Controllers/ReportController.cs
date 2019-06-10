using DAL.DTOs;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProfitOpticsCustomerSalesAnalytics.Controllers
{
    public class ReportController : Controller
    {
        private ReportRepository _repo;

        public ReportController()
        {
            _repo = new ReportRepository();
        }

        public ActionResult Growth()
        {
            var dto = new ReportGrowthDTO();
            dto.Costumers = _repo.TopGrowingCustomerInSaleByYear(2018);
            dto.Items = _repo.TopGrowingItemInSaleByYear(2018);
            return View(dto);
        }

        public ActionResult Profit()
        {
            var dto = new ProfitReportDTO();
            dto.ProfitableCustomers = _repo.HighestCustomerGPByYear(2018);
            dto.ProfitableItems = _repo.HighestItemGPByYear(2018);
            return View(dto);
        }

        public ActionResult Sales()
        {
            var dto = new ReportSalesDTO();
            dto.FirstYear = _repo.SalesByMonthAndYear(2017);
            dto.SecondYear = _repo.SalesByMonthAndYear(2018);
            dto.SalesBySource = _repo.SalesBySources();
            dto.SalesByCategory = _repo.SalesByCategory();
            dto.SalesByState = _repo.SalesByState();
            return View(dto);
        }

        
    }
}