using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models.ViewModels
{
    public class FivePatientViewModel
    {
        public string FullName { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
