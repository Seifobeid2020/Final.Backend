using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models.ViewModels
{
    public class FiveExpenseViewModel
    {
        public string ExpenseTypeName { get; set; }
        public decimal ExpenseValue { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
