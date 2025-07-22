namespace Mero_Doctor_Project.DTOs.DoctorDto
{
    public class GetDoctorAvailabilityDto
    {
        public string DoctorUserId { get; set; }
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string SpecialzationName { get; set; }
        public List<GetDayAvailabilityDto> Availabilities { get; set; }
    }
}
