using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.DTOs.AuthDto;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models.Enums;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Mero_Doctor_Project.Repositories
{
    public class AuthDoctorRegistrationRepository : Repository<Doctor>, IAuthDoctorRegistrationRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthDoctorRegistrationRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
            : base(context)
        {
            _userManager = userManager;
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
