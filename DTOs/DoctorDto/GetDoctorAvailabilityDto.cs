namespace Mero_Doctor_Project.DTOs.DoctorDto
{
    public class GetDoctorAvailabilityDto
    {
        public string DoctorUserId { get; set; }
        public string ProfilePictureUrl { get; set; }
        public List<GetDayAvailabilityDto> Availabilities { get; set; }
    }
}
