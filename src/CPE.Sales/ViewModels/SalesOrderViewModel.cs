using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPE.Domain.Services;
using CPE.Sales.Services;

namespace CPE.Sales.ViewModels
{
    public class SalesOrderViewModel : ViewModelBase
    {
        private readonly SalesOrderParserService _parserService;
        private readonly OpenOrderReportParserService _openOrders;
        private readonly IPartService _parts;
        private bool _isMultiline;
        private string _drawingNumber;
        private string _partName;
        private DateTime _originalDate;
        private DateTime? _rescheduledDate;
        private byte[] _photo;

        public ObservableCollection<LineDetail> OrderItems { get; set; } = new ObservableCollection<LineDetail>();

        public string DrawingNumber
        {
            get { return _drawingNumber; }
            set
            {
                _drawingNumber = value;
                OnPropertyChanged();
            }
        }

        public string PartName
        {
            get { return _partName; }
            set
            {
                _partName = value;
                OnPropertyChanged();
            }
        }

        public DateTime OriginalDate { get; set; }

        public DateTime? RescheduleDate { get; set; }

        public byte[] Photo { get; set; }

        public bool IsMultiline
        {
            get { return _isMultiline; }
            set
            {
                _isMultiline = value;
                OnPropertyChanged();
            }
        }
        public SalesOrderViewModel(SalesOrderParserService parserService, OpenOrderReportParserService openOrders, IPartService parts)
        {
            _parserService = parserService;
            _openOrders = openOrders;
            _parts = parts;
        }

        public void ShowItem(LineDetail lineDetail)
        {
            DrawingNumber = lineDetail.DrawingNumber;
        }

        public async Task RetrieveLinesAsync(SalesOrderListViewModel.SalesOrder salesOrder)
        {
            OrderItems.Clear();

            if (salesOrder.IsMultiline)
            {
                IsMultiline = true;

                var lines = await _parserService.GetSalesOrderLinesAsync(salesOrder.Mail);

                foreach (var item in lines)
                {
                    var line = new LineDetail();

                    line.DrawingNumber = item.DrawingNumber;
                    line.OriginalDate = item.DeliveryDate;

                    var rescheduleResult = await _openOrders.CheckIfHasBeenRescheduled(line.DrawingNumber, salesOrder.OrderNumber);

                    if (rescheduleResult.HasBeenRescheduled)
                    {
                        line.RescheduledDate = rescheduleResult.RescheduledDate;
                    }

                    var part = _parts.GetWhereDrawingNumberEquals(line.DrawingNumber);

                    line.Name = part == null ? "N/A" : part.Name;

                    OrderItems.Add(line);
                }
            }
            else
            {
                IsMultiline = false;

                var line = new LineDetail();

                line.DrawingNumber = await _parserService.ParseSingleLineDrawingNumberAsync(salesOrder.Mail);
                line.OriginalDate= await _parserService.ParseSingleLineDeliveryDateAsync(salesOrder.Mail);

                var rescheduleResult = await _openOrders.CheckIfHasBeenRescheduled(line.DrawingNumber, salesOrder.OrderNumber);

                if (rescheduleResult.HasBeenRescheduled)
                {
                    line.RescheduledDate = rescheduleResult.RescheduledDate;
                }

                var part = _parts.GetWhereDrawingNumberEquals(line.DrawingNumber);

                line.Name = part == null ? "N/A" : part.Name;

                ShowItem(line);
            }
        }

        public class LineDetail
        {
            public string DrawingNumber { get; set; }

            public string Name { get; set; }

            public DateTime OriginalDate { get; set; }

            public DateTime? RescheduledDate { get; set; }

            public byte[] Photo { get; set; }
        }
    }
}