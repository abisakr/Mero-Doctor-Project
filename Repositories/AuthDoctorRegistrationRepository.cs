using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.DTOs.AuthDto;
using Mero_Doctor_Project.Helper;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models.Enums;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mero_Doctor_Project.Repositories
{
    public class AuthDoctorRegistrationRepository : Repository<Doctor>, IAuthDoctorRegistrationRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenGenerator _tokenGenerator;

        public AuthDoctorRegistrationRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager,TokenGenerator tokenGenerator)
            : base(context)
        {
            _userManager = userManager;
           _tokenGenerator = tokenGenerator;
        }

        public async Task<ResponseModel<string>> DoctorLoginAsync(DoctorLoginDto model)
        {
            var response = new ResponseModel<string>();

            try
            {
                // Step 1: Find the doctor by RegistrationId
                var doctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.RegistrationId == model.RegistrationId);

                if (doctor == null)
                {
                    response.Success = false;
                    response.Message = "Invalid RegistrationId.";
                    return response;
                }

                // Step 2: Find the corresponding ApplicationUser
                var user = await _userManager.FindByIdAsync(doctor.UserId);
                if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    response.Success = false;
                    response.Message = "Invalid credentials.";
                    return response;
                }

                var roles = await _userManager.GetRolesAsync(user);
                // Step 3: Check if user has 'Doctor' role
                if (!roles.Contains("Doctor"))
                {
                    response.Success = false;
                    response.Message = "Access denied. User is not a doctor.";
                    return response;
                }
                // Step 3: Generate token
                var token = _tokenGenerator.GenerateToken(user.Id, user.FullName, roles);

                response.Success = true;
                response.Message = "Login successful.";
                response.Data = token;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Login failed: {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<ResponseModel<Doctor>> DoctorRegisterAsync(DoctorRegistrationDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = new ApplicationUser
                {
                    UserName = dto.Email,
                    FullName = dto.FullName,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    Latitude = dto.Latitude,
                    Longitude = dto.Longitude
                };

                var userResult = await _userManager.CreateAsync(user, dto.Password);
                if (!userResult.Succeeded)
                {
                    var errors = string.Join("; ", userResult.Errors.Select(e => e.Description));
                    return new ResponseModel<Doctor>
                    {
                        Success = false,
                        Message = $"User creation failed: {errors}",
                        Data = null
                    };
                }

                var roleResult = await _userManager.AddToRoleAsync(user, "Doctor");
                if (!roleResult.Succeeded)
                {
                    var errors = string.Join("; ", roleResult.Errors.Select(e => e.Description));
                    return new ResponseModel<Doctor>
                    {
                        Success = false,
                        Message = $"Failed to assign Doctor role: {errors}",
                        Data = null
                    };
                }

                var doctor = new Doctor
                {
                    UserId = user.Id,
                    Degree = dto.Degree,
                    Experience = dto.Experience,
                    RegistrationId = dto.RegistrationId,
                    ClinicAddress = dto.ClinicAddress,
                    Status = DoctorStatus.Pending,
                    SpecializationId = dto.SpecializationId
                };

                await AddAsync(doctor);         // From base Repository
                await SaveChangesAsync();       // From base Repository
                await transaction.CommitAsync();

                return new ResponseModel<Doctor>
                {
                    Success = true,
                    Message = "Doctor registration successful.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ResponseModel<Doctor>
                {
                    Success = false,
                    Message = $"Doctor registration failed: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
