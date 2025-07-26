namespace Mero_Doctor_Project.DTOs.AppointmentDto
{
    public class GetCompletedAppointmentDto
    {
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public string Status { get; set; }
        public string AvailableDate { get; set; }
        public string AvailableTime { get; set; }
        public string BookingDateTime { get; set; }
        public string? TransactionId { get; set; }
        public string? TransactionStatus { get; set; }
        public string? PaymentDate { get; set; }
        public bool Visited { get; set; }

        // Doctor Info
        public string DoctorName { get; set; }

        // Patient Info
        public string PatientName { get; set; }
        public string PatientEmail { get; set; }
        public string PatientPhone { get; set; }
        public double? PatientLatitude { get; set; }
        public double? PatientLongitude { get; set; }
        public string? PatientProfilePicture { get; set; }
    }
}
