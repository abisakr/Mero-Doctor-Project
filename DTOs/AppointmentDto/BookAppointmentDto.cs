namespace Mero_Doctor_Project.DTOs.AppointmentDto
{
    public class BookAppointmentDto
    {
        
        public int DoctorId { get; set; }
        public TimeOnly AvailableTime { get; set; }
        public DateOnly AvailableDate { get; set; }
        public decimal Price { get; set; }
    }
}
