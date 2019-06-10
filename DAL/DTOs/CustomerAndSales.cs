using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs
{
    public class ReportSalesDTO
    {
        public IList<SalesByMonth> FirstYear { get; set; }
        public IList<SalesByMonth> SecondYear { get; set; }
        public IList<SalesByParameter> SalesBySource { get; set; }
        public IList<SalesByParameter> SalesByCategory { get; set; }
        public IList<SalesByParameter> SalesByState { get; set; }

        public ReportSalesDTO()
        {
            FirstYear = new List<SalesByMonth>();
            SecondYear = new List<SalesByMonth>();
            SalesBySource = new List<SalesByParameter>();
            SalesByCategory = new List<SalesByParameter>();
            SalesByState = new List<SalesByParameter>();
        }
    }

    public class ProfitReportDTO
    {
        public IList<CustomerItemDTO> ProfitableCustomers { get; set; }
        public IList<CustomerItemDTO> ProfitableItems { get; set; }

        public ProfitReportDTO()
        {
            ProfitableCustomers = new List<CustomerItemDTO>();
            ProfitableItems = new List<CustomerItemDTO>();
        }
    }

    public class SalesByMonth
    {
        public int Month { get; set; }
        public int SalesCount { get; set; }
        public Decimal SalesAmount { get; set; }
    }

    public class SalesByParameter
    {
        public string Name { get; set; }
        public int SalesCount { get; set; }
        public Decimal SalesAmount { get; set; }
    }

    public class CustomerItemDTO
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public Decimal AccumulatedGP { get; set; }
    }

    public class ReportGrowthDTO
    {
        public IList<CustomerItemGrowthDTO> Costumers { get; set; }
        public IList<CustomerItemGrowthDTO> Items { get; set; }

        public ReportGrowthDTO()
        {
            Costumers = new List<CustomerItemGrowthDTO>();
            Items = new List<CustomerItemGrowthDTO>();
        }
    }

    public class CustomerItemGrowthDTO
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public Decimal SalesFirstYear { get; set; }
        public Decimal SalesSecondYear { get; set; }
        public Decimal Growth { get; set; }
    }
}
