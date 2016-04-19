using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPE.Data.EntityFramework.Model.Tricorn;
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
    }
}
