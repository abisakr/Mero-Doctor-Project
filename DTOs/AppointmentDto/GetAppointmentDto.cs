using Mero_Doctor_Project.Models.Enums;

namespace Mero_Doctor_Project.DTOs.AppointmentDto
{
    public class GetAppointmentDto
    {
       
            public int AppointmentId { get; set; }
            public int DoctorId { get; set; }
            public int PatientId { get; set; }
            public string Status { get; set; }
            public string AvailableDate { get; set; }
            public string AvailableTime { get; set; }
            public string BookingDateTime { get; set; }
            public string DoctorName { get; set; }
            public string PatientName { get; set; }

    }
}
