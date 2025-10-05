using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserForm.BLL.DTOs
{
    public class FormCreateRequest
    {
        public Guid UserId { get; set; }
        public Guid CampusId { get; set; }
        public Guid ServiceId { get; set; }
        public string CourseCode { get; set; }
        public string SupportCode { get; set; }
        public DateTime ExamDate { get; set; }
    }

}
