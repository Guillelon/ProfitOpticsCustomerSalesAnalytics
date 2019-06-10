using DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CustomerRepository
    {
        private AnalyticsDbContext _context;
        public CustomerRepository()
        {
            _context = new AnalyticsDbContext();
        }

        public int GetCount()
        {
            var query = "SELECT count(*) FROM [SalesAnalytics].[dbo].[customerItemsHistory]";
            var result = _context.Database.SqlQuery<int>(query).FirstOrDefault();
            return result;
        }
    }
}
