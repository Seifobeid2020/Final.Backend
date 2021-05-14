using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models.ViewModels
{
    public class ExpenseViewModel
    {
        public int ExpenseId { get; set; }
        public decimal ExpenseValue { get; set; }
        public string ExpenseDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
        public string ExpenseTypeName { get; set; }
        public int ExpenseTypeId { get; set; }

    }
}
