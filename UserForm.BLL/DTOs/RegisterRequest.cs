using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserForm.BLL.DTOs
{
    public class RegisterRequest
    {
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
        public Guid CampusId { get; set; }
    }
}
