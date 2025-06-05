namespace Mero_Doctor_Project.DTOs.AppointmentDto
{
    public class BookAppointmentDto
    {
        
            public int DoctorId { get; set; }
            public DayOfWeek DayOfWeek { get; set; } // Comes directly from UI selection
            public DateTime AppointmentDate { get; set; } // Actual booking date
            public TimeOnly StartTime { get; set; }
            public TimeOnly EndTime { get; set; }
        

    }
}
