using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models.ViewModels
{
    public class ReportViewModel
    {
        public string TypeOfReport { get; set; }
        public DateTime Date { get; set; }
        public decimal Balance { get; set; }
      
    }
}
