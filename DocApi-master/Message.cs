using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocApi
{
    public class Message
    {
        public int MessageId { get; set; }
        public string SenderId { get; set; }
        public string ImageUrl { get; set; }
        public string ImageType { get; set; }   
        public int TreatmentId { get; set; }
        public string PatientName { get; set; }
        public string PatientPhoneNumber { get; set; }
        public DateTime SentAt { get; set; }
        
    }                      
}
