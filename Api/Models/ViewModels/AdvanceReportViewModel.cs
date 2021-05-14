using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models.ViewModels
{
    public class AdvanceReportViewModel
    {
        public string ReportKind { get; set; }
        public string TypeOfReportKind { get; set; }
        public DateTime Date { get; set; }
        public decimal Balance { get; set; }
    }
}
