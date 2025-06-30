namespace Mero_Doctor_Project.DTOs.NotificationDto
{
    public class NotificationViewDto
    {
        public int NotificationId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; }
    }
}
