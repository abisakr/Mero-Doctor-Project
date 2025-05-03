using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.DTOs.AuthDto;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models.Enums;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Mero_Doctor_Project.Repositories
{
    public class AuthDoctorRegistrationRepository : IAuthDoctorRegistrationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthDoctorRegistrationRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Method for doctor registration 
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
                    foreach (var error in userResult.Errors)
                    {
                        Console.WriteLine($"User creation error: {error.Description}");
                    }
                    return new ResponseModel<Doctor>
                    {
                        Success = false,
                        Message = "User registration failed.",
                        Data = null // No doctor data to return
                    };
                }

                var roleResult = await _userManager.AddToRoleAsync(user, "Doctor");
                if (!roleResult.Succeeded)
                {
                    Console.WriteLine("Failed to assign Doctor role.");
                    return new ResponseModel<Doctor>
                    {
                        Success = false,
                        Message = "Failed to assign Doctor role.",
                        Data = null // No doctor data to return
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

                await _context.Doctors.AddAsync(doctor);
                await _context.SaveChangesAsync();
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
                return new ResponseModel<Doctor>
                {
                    Success = false,
                    Message = $"Doctor registration failed: {ex.Message}",
                    Data = null // No doctor data to return
                };
            }
        }




    }


}
