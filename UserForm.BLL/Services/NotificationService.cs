using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserForm.DAL.Models;

namespace UserForm.BLL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AssignmentSupportDBContext _db;

        public NotificationService(AssignmentSupportDBContext db)
        {
            _db = db;
        }

        public async Task SendAsync(Guid toUserId, string subject, string body, Guid? formId = null)
        {
            var notif = new Notification
            {
                NotificationId = Guid.NewGuid(),
                ToUserId = toUserId,
                FormId = formId,
                Subject = subject,
                Body = body,
                SentAt = DateTime.UtcNow,
                DeliveryChannel = "IN_APP"
            };

            _db.Notifications.Add(notif);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid userId)
        {
            return await _db.Notifications
                .Include(n => n.Form)
                .Where(n => n.ToUserId == userId)
                .OrderByDescending(n => n.SentAt)
                .ToListAsync();
        }
    }
}
