using System.ComponentModel.DataAnnotations;

namespace Mero_Doctor_Project.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ApplicationUser User { get; set; } // Navigation property

    }
}
