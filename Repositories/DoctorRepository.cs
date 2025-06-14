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
                    UserId=doctor.UserId,
                    FullName = doctor.User.FullName,
                    Email = doctor.User.Email,
                    PhoneNumber = doctor.User.PhoneNumber,
                    ProfilePictureUrl = doctor.User.ProfilePictureUrl,
                    RegistrationId = doctor.RegistrationId,
                    Status = doctor.Status,
                    Degree = doctor.Degree,
                    Experience = doctor.Experience,
                    ClinicAddress = doctor.ClinicAddress,
                    SpecializationName = doctor.Specialization.Name,
                    Latitude = doctor.User.Latitude,
                    Longitude = doctor.User.Longitude
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
                    FullName = doctor.User.FullName,
                    Email = doctor.User.Email,
                    PhoneNumber = doctor.User.PhoneNumber,
                    ProfilePictureUrl = doctor.User.ProfilePictureUrl,
                    RegistrationId = doctor.RegistrationId,
                    Status = doctor.Status,
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
                    FullName = doctor.User.FullName,
                    Email = doctor.User.Email,
                    PhoneNumber = doctor.User.PhoneNumber,
                    ProfilePictureUrl = doctor.User.ProfilePictureUrl,
                    RegistrationId = doctor.RegistrationId,
                    Status = doctor.Status,
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



    }
}
