using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.DTOs.DoctorDto;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Mero_Doctor_Project.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ApplicationDbContext _context;
        public DoctorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<GetDoctorDto>> GetDoctorByIdAsync(string userId)
        {
            try
            {
                var doctor = await _context.Doctors
       .Include(d => d.User)
       .Include(d => d.Specialization)
       .FirstOrDefaultAsync(d => d.UserId == userId);

                var today = DateOnly.FromDateTime(DateTime.Today);

                var todaysUnvisitedAppointmentsCount = await _context.Appointments
                    .Where(a => a.DoctorId == doctor.DoctorId
                                && a.AvailableDate == today
                                && !a.Visited)  // Only unvisited appointments
                    .CountAsync();



                if (doctor == null)
                {
                    return new ResponseModel<GetDoctorDto>
                    {
                        Success = false,
                        Message = "Doctor not found."
                    };
                }
                var dto = new GetDoctorDto
                {
                    UserId = doctor.UserId,
                    DoctorId = doctor.DoctorId,
                    FullName = doctor.User.FullName,
                    Email = doctor.User.Email,
                    PhoneNumber = doctor.User.PhoneNumber,
                    ProfilePictureUrl = doctor.User.ProfilePictureUrl,
                    RegistrationId = doctor.RegistrationId,
                    Status = doctor.Status.ToString(),
                    Degree = doctor.Degree,
                    Experience = doctor.Experience,
                    ClinicAddress = doctor.ClinicAddress,
                    SpecializationName = doctor.Specialization.Name,
                    Latitude = doctor.User.Latitude,
                    Longitude = doctor.User.Longitude,
                    TodaysAppointments = todaysAppointmentsCount // <-- Assign the count here
                };


                return new ResponseModel<GetDoctorDto>
                {
                    Success = true,
                    Message = "Doctor Fetched.",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<GetDoctorDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }
      
        public async Task<ResponseModel<List<GetDoctorDto>>> GetDoctorsByFilterAsync(int? specializationId, string? doctorName)
        {
            try
            {
                var query = _context.Doctors
                    .Include(d => d.User)
                    .Include(d => d.Specialization)
                    .AsQueryable();

                if (specializationId.HasValue)
                {
                    query = query.Where(d => d.SpecializationId == specializationId.Value);
                }

                if (!string.IsNullOrEmpty(doctorName))
                {
                    // Assuming FullName is in User entity
                    query = query.Where(d => d.User.FullName.Contains(doctorName));
                }

                var doctors = await query.ToListAsync();

                var dtoList = doctors.Select(doctor => new GetDoctorDto
                {
                    UserId=doctor.UserId,
                    DoctorId=doctor.DoctorId,
                    FullName = doctor.User.FullName,
                    Email = doctor.User.Email,
                    PhoneNumber = doctor.User.PhoneNumber,
                    ProfilePictureUrl = doctor.User.ProfilePictureUrl,
                    RegistrationId = doctor.RegistrationId,
                    Status = doctor.Status.ToString(),
                    Degree = doctor.Degree,
                    Experience = doctor.Experience,
                    ClinicAddress = doctor.ClinicAddress,
                    SpecializationName = doctor.Specialization.Name,
                    Latitude = doctor.User.Latitude,
                    Longitude = doctor.User.Longitude
                }).ToList();

                return new ResponseModel<List<GetDoctorDto>>
                {
                    Success = true,
                    Message = dtoList.Count > 0 ? "Doctors fetched." : "No doctors found.",
                    Data = dtoList
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<GetDoctorDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }
        public async Task<ResponseModel<List<GetDoctorDto>>> GetAllDoctorsAsync()
        {
            try
            {
                var doctors = await _context.Doctors
                    .Include(d => d.User)
                    .Include(d => d.Specialization)
                    .ToListAsync();

                var dtoList = doctors.Select(doctor => new GetDoctorDto
                {
                    UserId=doctor.UserId,
                    DoctorId=doctor.DoctorId,
                    FullName = doctor.User.FullName,
                    Email = doctor.User.Email,
                    PhoneNumber = doctor.User.PhoneNumber,
                    ProfilePictureUrl = doctor.User.ProfilePictureUrl,
                    RegistrationId = doctor.RegistrationId,
                    Status = doctor.Status.ToString(),
                    Degree = doctor.Degree,
                    Experience = doctor.Experience,
                    ClinicAddress = doctor.ClinicAddress,
                    SpecializationName = doctor.Specialization?.Name,
                    Latitude = doctor.User.Latitude,
                    Longitude = doctor.User.Longitude
                }).ToList();

                return new ResponseModel<List<GetDoctorDto>>
                {
                    Success = true,
                    Message = "Doctors fetched successfully.",
                    Data = dtoList
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<GetDoctorDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }
        public async Task<ResponseModel<List<GetDoctorDto>>> GetAllTopDoctorsAsync()
        {
            try
            {
                var topDoctors = await _context.Doctors
                    .Include(d => d.User)
                    .Include(d => d.Specialization)
                    .Include(d => d.Ratings)
                    .Select(d => new
                    {
                        Doctor = d,
                        AverageRating = d.Ratings.Any() ? d.Ratings.Average(r => r.Rating) : 0
                    })
                    .OrderByDescending(x => x.AverageRating)
                    .ThenBy(x => x.Doctor.User.FullName)
                    .Take(10)
                    .ToListAsync();

                var dtoList = topDoctors.Select(x => new GetDoctorDto
                {
                    UserId = x.Doctor.UserId,
                    DoctorId = x.Doctor.DoctorId,
                    FullName = x.Doctor.User.FullName,
                    Email = x.Doctor.User.Email,
                    PhoneNumber = x.Doctor.User.PhoneNumber,
                    ProfilePictureUrl = x.Doctor.User.ProfilePictureUrl,
                    RegistrationId = x.Doctor.RegistrationId,
                    Status = x.Doctor.Status.ToString(),
                    Degree = x.Doctor.Degree,
                    Experience = x.Doctor.Experience,
                    ClinicAddress = x.Doctor.ClinicAddress,
                    SpecializationName = x.Doctor.Specialization?.Name,
                    Latitude = x.Doctor.User.Latitude,
                    Longitude = x.Doctor.User.Longitude,
                    AverageRating = (int)Math.Round(x.AverageRating) // 👈 Rounded to nearest integer
                }).ToList();

                return new ResponseModel<List<GetDoctorDto>>
                {
                    Success = true,
                    Message = "Top 10 doctors fetched successfully.",
                    Data = dtoList
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<GetDoctorDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }



    }
}
