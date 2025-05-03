using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.DTOs.AuthDto;
using Mero_Doctor_Project.Helper;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models.Enums;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Mero_Doctor_Project.Repositories
{
    public class AuthPatientRegistrationRepository : IAuthPatientRegistrationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthPatientRegistrationRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Method for Patient registration 
        public async Task<ResponseModel<Patient>> PatientRegisterAsync(PatientRegistrationDto dto)
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
                    return new ResponseModel<Patient>
                    {
                        Success = false,
                        Message = "User registration failed.",
                        Data = null // No patient data to return
                    };
                }

                var roleResult = await _userManager.AddToRoleAsync(user, "Patient");
                if (!roleResult.Succeeded)
                {
                    return new ResponseModel<Patient>
                    {
                        Success = false,
                        Message = "Failed to assign Patient role.",
                        Data = null // No patient data to return
                    };
                }

                var patient = new Patient
                {
                    UserId = user.Id,
                    DateOfBirth = dto.DateOfBirth,
                    Gender = dto.Gender,
                    Address = dto.Address,
                    Latitude = dto.Latitude,
                    Longitude = dto.Longitude
                };

                await _context.Patients.AddAsync(patient);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ResponseModel<Patient>
                {
                    Success = true,
                    Message = "Registration successful.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<Patient>
                {
                    Success = false,
                    Message = $"Patient registration failed: {ex.Message}",
                    Data = null // No patient data to return
                };
            }
        }



    }
}
