using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserForm.BLL.DTOs
{
    public class FormUpdateRequest
    {
        public Guid FormId { get; set; }
        public string? CourseCode { get; set; }
        public string? SupportCode { get; set; }
        public DateTime? ExamDate { get; set; }
    }
}
