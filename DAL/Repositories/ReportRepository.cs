using DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ReportRepository
    {
        private AnalyticsDbContext _context;

        public ReportRepository()
        {
            _context = new AnalyticsDbContext();
        }

        public IList<SalesByMonth> SalesByMonthAndYear(int year)
        {
            var query = "SELECT Month([InvoiceDate]) as 'Month', " +
                        "COUNT(*) as 'SalesCount', " +
                        "CONVERT(DECIMAL(10,2), ROUND(SUM([Sales]),2)) as 'SalesAmount' " +
                        "FROM[dbo].[CustomerItemsHistory] " +
                        "WHERE Year([InvoiceDate]) = " + year + " " +
                        "GROUP BY Month([InvoiceDate]) " +
                        "ORDER BY Month([InvoiceDate])";
            var result = _context.Database.SqlQuery<SalesByMonth>(query).ToList();
            return result;
        }

        public IList<CustomerItemDTO> HighestCustomerGPByYear(int year)
        {
            var query = "SELECT TOP 10 CU.Number, CU.Name, CONVERT(DECIMAL(10, 2), ROUND(SUM(CIH.[GP]),2)) AS AccumulatedGP " +
                        " FROM [dbo].[Customers] CU, [dbo].[CustomerItemsHistory] CIH " +
                        " WHERE CU.Number = CIH.[CustomerNumber] " +
                        " AND YEAR(CIH.[InvoiceDate]) = " + year +
                        " GROUP BY CU.Number, CU.Name" +
                        " ORDER BY AccumulatedGP DESC";
            var result = _context.Database.SqlQuery<CustomerItemDTO>(query).ToList();
            return result;
        }

        public IList<CustomerItemDTO> HighestItemGPByYear(int year)
        {
            var query = "SELECT TOP 10 IT.Number, IT.Description as 'Name', CONVERT(DECIMAL(10, 2), ROUND(SUM(CIH.[GP]),2)) AS AccumulatedGP " +
                        " FROM [dbo].[Items] IT, [dbo].[CustomerItemsHistory] CIH " +
                        " WHERE IT.Number = CIH.[ItemNumber] " +
                        " AND YEAR(CIH.[InvoiceDate]) = " + year +
                        " GROUP BY IT.Number, IT.Description" +
                        " ORDER BY AccumulatedGP DESC";
            var result = _context.Database.SqlQuery<CustomerItemDTO>(query).ToList();
            return result;
        }

        public IList<CustomerItemGrowthDTO> TopGrowingCustomerInSaleByYear(int year)
        {
            var query = "DECLARE @year numeric " +
                        "DECLARE @previousYear numeric " +
                        "SET @year = " + year + " " +
                        "SET @previousYear = @year - 1; " +
                        "WITH cte AS(SELECT CIH.CustomerNumber, CU.Name,YEAR(CIH.InvoiceDate) AS YEAR, SUM(CIH.Sales) AS SALES " +
                            "FROM [CustomerItemsHistory] CIH, [dbo].[Customers] CU where sales != 0 AND CU.Number = CIH.CustomerNumber " +
                            "GROUP BY CIH.CustomerNumber, CU.Name, YEAR(CIH.InvoiceDate)) " +
                        "SELECT top 10 c1.CustomerNumber as 'Number', c1.Name, CONVERT(DECIMAL(10,2),c1.SALES) as 'SalesFirstYear', CONVERT(DECIMAL(10,2),c2.SALES) as 'SalesSecondYear', " +
                        "CONVERT(DECIMAL(10, 2), (CONVERT(DECIMAL(10, 2), (c2.SALES - c1.SALES)) / CONVERT(DECIMAL(10, 2), c1.SALES)) * 100)  AS 'Growth' " +
                        "FROM cte c1, cte c2 " +
                        "WHERE c1.CustomerNumber = c2.CustomerNumber " +
                        "  AND c2.YEAR = @year " +
                        "  AND c1.YEAR = @previousYear " +
                        "  AND c2.sales != 0 " +
                        "  AND c1.SALES != 0 " +
                        "ORDER BY  'Growth' DESC";
            var result = _context.Database.SqlQuery<CustomerItemGrowthDTO>(query).ToList();
            return result;
        }

        public IList<CustomerItemGrowthDTO> TopGrowingItemInSaleByYear(int year)
        {
            var query = "DECLARE @year numeric " +
                        "DECLARE @previousYear numeric " +
                        "SET @year = " + year + " " +
                        "SET @previousYear = @year - 1; " +
                        "WITH cte AS(SELECT CIH.ItemNumber, IT.Description, YEAR(CIH.InvoiceDate) AS YEAR, SUM(CIH.Sales) AS SALES " +
                            "FROM [CustomerItemsHistory] CIH, [Items] IT where CIH.sales != 0 AND IT.Number = CIH.ItemNumber " +
                            "GROUP BY CIH.ItemNumber, IT.Description, YEAR(CIH.InvoiceDate)) " +
                            "SELECT top 10 c1.ItemNumber as 'Number',C1.Description AS 'Name', C1.Description AS 'Name', CONVERT(DECIMAL(10,2),c1.SALES) as 'SalesFirstYear', CONVERT(DECIMAL(10,2),c2.SALES) as 'SalesSecondYear', " +
                            "CONVERT(DECIMAL(10, 2), (CONVERT(DECIMAL(10, 2), (c2.SALES - c1.SALES)) / CONVERT(DECIMAL(10, 2), c1.SALES)) * 100)  AS 'Growth' " +
                        "FROM cte c1, cte c2 " +
                        "WHERE c1.ItemNumber = c2.ItemNumber " +
                        "  AND c2.YEAR = @year " +
                        "  AND c1.YEAR = @previousYear " +
                        "  AND c2.sales != 0 " +
                        "  AND c1.SALES != 0 " +
                        "ORDER BY  'Growth' DESC";
            var result = _context.Database.SqlQuery<CustomerItemGrowthDTO>(query).ToList();
            return result;
        }

        public IList<SalesByParameter> SalesBySources()
        {
            var query = "SELECT TOP 3 OrderSource as 'Name', COUNT(*) as 'SalesCount', " +
                        "CONVERT(DECIMAL(10,2), ROUND(SUM([Sales]),2)) as 'SalesAmount' " +
                        "FROM [dbo].[CustomerItemsHistory] " +
                        "WHERE OrderSource != '' " +
                        "GROUP BY OrderSource " +
                        "ORDER BY 'SalesAmount' DESC ";
            var result = _context.Database.SqlQuery<SalesByParameter>(query).ToList();
            return result;
        }

        public IList<SalesByParameter> SalesByCategory()
        {
            var query = "SELECT TOP 3 IT.[CategoryOne] as 'Name', COUNT(*) as 'SalesCount', " +
                        "CONVERT(DECIMAL(10,2), ROUND(SUM(CIH.[Sales]),2)) as 'SalesAmount' " +
                        "FROM [dbo].[CustomerItemsHistory] CIH, [dbo].[Items] IT " +
                        "WHERE CIH.[ItemNumber] = IT.[Number] " +
                        "GROUP BY IT.[CategoryOne] " +
                        "ORDER BY 'SalesAmount' DESC ";
            var result = _context.Database.SqlQuery<SalesByParameter>(query).ToList();
            return result;
        }

        public IList<SalesByParameter> SalesByState()
        {
            var query = "SELECT TOP 3 CU.[State] as 'Name', COUNT(*) as 'SalesCount', " +
                        "CONVERT(DECIMAL(10,2), ROUND(SUM(CIH.[Sales]),2)) as 'SalesAmount' " +
                        "FROM [dbo].[CustomerItemsHistory] CIH, [dbo].[Customers] CU " +
                        "WHERE CIH.[CustomerNumber] = CU.[Number] " +
                        "GROUP BY CU.[State] " +
                        "ORDER BY 'SalesAmount' DESC ";
            var result = _context.Database.SqlQuery<SalesByParameter>(query).ToList();
            return result;
        }

        public IList<CustomerDTO> GetUserByNameContains(string name)
        {
            var query = "SELECT NUMBER, NAME, STATE FROM [dbo].[Customers] WHERE NAME LIKE '%"+name+"%' ";
            var result = _context.Database.SqlQuery<CustomerDTO>(query).ToList();
            return result;
        }

        public IList<ItemsSalesByCustomer> GetTopItemsByUser(string customerNumber)
        {
            try
            {
                var query = "SELECT TOP 10 IT.Description AS 'Name', IT.Number AS 'Number', COUNT(*) AS 'SalesCount', " +
                            "CONVERT(DECIMAL(10, 2), ROUND(SUM(CIH.Sales),2)) AS 'Amount', CONVERT(DECIMAL(10, 2), ROUND(SUM(CIH.InvoiceQuantity),2)) AS 'ItemQuantity' " +
                            "FROM [dbo].[CustomerItemsHistory] CIH, [dbo].[Items] IT " +
                            "WHERE CIH.CustomerNumber = '" + customerNumber + "' AND CIH.ItemNumber = IT.Number" +
                            "  AND CIH.TransactionType = 'Invoice'" +
                            "GROUP BY IT.Description, IT.Number " +
                            "ORDER BY 'SalesCount' DESC ";

                var result = _context.Database.SqlQuery<ItemsSalesByCustomer>(query).ToList();
                return result;

            }
            catch(Exception e)
            {
                var hola = e;
                return null;
            }
        }

        public IList<ItemsSalesByCustomer> GetHistoryByCustomerAndItem(string customerNumber, string itemNumber)
        {
            var query = "SELECT CIH.InvoiceDate AS 'Date',CIH.InvoiceQuantity AS 'ItemQuantity', " +
                       " CONVERT(DECIMAL(10, 2), ROUND(CIH.Sales, 2)) AS 'Amount', " +
                       " CONVERT(DECIMAL(10, 2), ROUND(CIH.InvoiceCost * CIH.InvoiceQuantity, 2)) AS 'Cost' " +
                       " FROM CustomerItemsHistory CIH, Items IT " +
                       " WHERE CIH.ItemNumber = '"+itemNumber+"'" +
                       " AND CIH.CustomerNumber = '"+ customerNumber+"'" +
                       " AND CIH.ItemNumber = IT.Number" +
                       " AND CIH.TransactionType = 'Invoice'";

            var result = _context.Database.SqlQuery<ItemsSalesByCustomer>(query).ToList();
            return result;
        }
    }
}
