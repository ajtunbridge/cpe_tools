using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CPE.Domain.Model;
using CPE.Domain.Model.Tricorn;

namespace CPE.Domain.Services
{
    public interface ITricornService
    {
        Task<string> GetNameByDrawingNumberAsync(string drawingNumber);

        Task<decimal> GetTotalValueOfJobsForCurrentMonthAsync();

        Task<WorksOrder> GetWorksOrderByUserReferenceAsync(string userReference);
    }
}