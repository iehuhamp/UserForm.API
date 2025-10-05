using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserForm.DAL.Models;

namespace UserForm.BLL.Services
{
    public interface INotificationService
    {
        Task SendAsync(Guid toUserId, string subject, string body, Guid? formId = null);
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid userId);
    }
}
