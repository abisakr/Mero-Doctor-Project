namespace Mero_Doctor_Project.DTOs.DoctorDto
{
    public class GetDoctorAvailabilityDto
    {
        public string DoctorId { get; set; }
        public List<GetDayAvailabilityDto> Availabilities { get; set; }
    }
}
