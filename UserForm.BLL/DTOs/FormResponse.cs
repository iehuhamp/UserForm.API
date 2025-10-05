using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserForm.BLL.DTOs
{
    public class FormResponse
    {
        public Guid FormId { get; set; }
        public string StudentName { get; set; }
        public string Email { get; set; }
        public string ServiceName { get; set; }
        public string CampusName { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string Status { get; set; }
    }
}
