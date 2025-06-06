using Mero_Doctor_Project.Models.Enums;

namespace Mero_Doctor_Project.DTOs.AppointmentDto
{
    public class AppointmentDto
    {
       
            public int AppointmentId { get; set; }
            public int DoctorId { get; set; }
            public int PatientId { get; set; }
            public AppointmentStatus Status { get; set; }
            public TimeOnly StartTime { get; set; }
            public TimeOnly EndTime { get; set; }
            public DateTime AppointmentDate { get; set; }

            // Optional extras
            public string DoctorName { get; set; }
            public string PatientName { get; set; }
       

    }
}
