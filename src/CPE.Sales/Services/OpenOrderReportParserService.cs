using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CPE.Sales.Models;
using CPE.Sales.Properties;
using Excel;

namespace CPE.Sales.Services
{
    public sealed class OpenOrderReportParserService
    {
        private DataSet _dataSet;

        public async Task<RescheduleResult> CheckIfHasBeenRescheduled(string drawingNumber, string orderNumber)
        {
            const int orderNumberColumnIndex = 7;
            const int drawingNumberColumnIndex = 2;
            const int rescheduleDateColumnIndex = 10;

            var result = new RescheduleResult {HasBeenRescheduled = false, RescheduledDate = null};

            await ReadExcelFileIntoDataSet();

            // convert supplied order number to a double to match the excel data type
            var doubleOrderNumber = double.Parse(orderNumber);

            foreach (DataRow row in _dataSet.Tables[0].Rows)
            {
                // retrieve the order number and drawing number for the current row
                var rawOrderNumber = row.ItemArray[orderNumberColumnIndex];

                if (rawOrderNumber is DBNull || !(rawOrderNumber is double))
                {
                    continue;
                }

                var retrievedOrderNumber = (double) rawOrderNumber;
                var retrievedDrawingNumber = (string) row.ItemArray[drawingNumberColumnIndex];

                // check if the values match the supplied one and continue loop if they don't
                if (!retrievedOrderNumber.Equals(doubleOrderNumber))
                {
                    continue;
                }

                if (!retrievedDrawingNumber.Equals(drawingNumber))
                {
                    continue;
                }

                // retrieve the reschedule date for the current row
                var rawRescheduleDate = row.ItemArray[rescheduleDateColumnIndex];

                // value will be DBNull if the date hasn't changed
                if (rawRescheduleDate is DBNull)
                {
                    break;
                }

                // convert the value to a DateTime object as it is a double in the excel file
                var oaDate = (double) rawRescheduleDate;

                result.RescheduledDate = DateTime.FromOADate(oaDate);
                result.HasBeenRescheduled = true;

                break;
            }

            return result;
        }

        private async Task ReadExcelFileIntoDataSet()
        {
            if (_dataSet != null)
            {
                return;
            }

            await Task.Factory.StartNew(() =>
            {
                if (_dataSet == null)
                {
                    var folder = Settings.Default.OpenOrderReportsFolderName;

                    var lastReport = MSOutlookService.GetMostRecentOpenOrderReport(folder);

                    using (var fs = new FileStream(lastReport.Attachments.First(), FileMode.Open))
                    {
                        var reader = ExcelReaderFactory.CreateBinaryReader(fs);
                        _dataSet = reader.AsDataSet();
                    }
                }
            });
        }
    }
}