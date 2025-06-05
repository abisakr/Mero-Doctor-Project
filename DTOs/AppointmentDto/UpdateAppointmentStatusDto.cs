using Mero_Doctor_Project.Models.Enums;

namespace Mero_Doctor_Project.DTOs.AppointmentDto
{
    public class UpdateAppointmentStatusDto
    {
        public int AppointmentId { get; set; }
        public AppointmentStatus Status { get; set; } // Only Accepted or Rejected expected
    }
}
