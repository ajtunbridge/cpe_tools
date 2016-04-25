using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPE.Data.EntityFramework.Model.Tricorn;
using CPE.Domain.Model;
using CPE.Domain.Model.Tricorn;
using CPE.Domain.Services;

namespace CPE.Data.EntityFramework.Repositories.Tricorn
{
    public class TricornRepository : ITricornService
    {
        private readonly TricornEntities _entities;

        public TricornRepository(TricornEntities entities)
        {
            _entities = entities;
        }

        public async Task<string> GetNameByDrawingNumberAsync(string drawingNumber)
        {
            var lastQuote = await _entities.Quotes.Where(q => q.Drawing_Number == drawingNumber)
                .OrderByDescending(q => q.Quote_Reference)
                .FirstOrDefaultAsync();

            if (lastQuote != null)
            {
                return lastQuote.Description;
            }

            var lastWorksOrder = await _entities.WOrders.Where(wo => wo.Drawing_Number == drawingNumber)
                .OrderByDescending(wo => wo.User_Reference)
                .FirstOrDefaultAsync();

            if (lastWorksOrder == null)
            {
                return "ERROR! SEE ADAM!!";
            }

            return lastWorksOrder.Description;
        }

        public async Task<decimal> GetTotalValueOfJobsForCurrentMonthAsync()
        {
            var year = DateTime.Today.Year;
            var month = DateTime.Today.Month;

            var startOfMonth = new DateTime(year, month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var orderOnSystem = await _entities.WOrders.Where(wo => wo.Delivery >= startOfMonth && wo.Delivery <= endOfMonth).ToListAsync();

            return orderOnSystem.Where(wo => wo.Total_Cost.HasValue).Sum(wo => wo.Total_Cost.Value);
        }

        public async Task<WorksOrder> GetWorksOrderByUserReferenceAsync(string userReference)
        {
            var wo = await _entities.WOrders.Where(w => w.User_Reference == userReference).FirstOrDefaultAsync();

            return new WorksOrder
            {
                CustomerReference = wo.Customer_Reference.Value,
                DrawingNumber = wo.Drawing_Number,
                Version = wo.Drawing_Issue,
                Name = wo.Description,
                UserReference = userReference
            };
        }
    }
}
