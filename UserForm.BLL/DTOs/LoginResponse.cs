using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserForm.BLL.DTOs
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string StudentName { get; set; }
    }
}
