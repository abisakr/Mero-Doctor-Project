using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.DTOs.AuthDto;
using Mero_Doctor_Project.Helper;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mero_Doctor_Project.Repositories
{
    public class AuthPatientRegistrationRepository : Repository<Patient>, IAuthPatientRegistrationRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenGenerator _tokenGenerator;

        public AuthPatientRegistrationRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager,TokenGenerator tokenGenerator)
            : base(context)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<ResponseModel<Patient>> PatientRegisterAsync(PatientRegistrationDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Check if Email already exists (in ApplicationUser)
                var emailExists = await _userManager.FindByEmailAsync(dto.Email);
                if (emailExists != null)
                {
                    return new ResponseModel<Patient>
                    {
                        Success = false,
                        Message = "Email is already registered.",
                        Data = null
                    };
                }

                // 2. Check if PhoneNumber already exists (in ApplicationUser)
                var phoneExists = await _context.Users.AnyAsync(u => u.PhoneNumber == dto.PhoneNumber);
                if (phoneExists)
                {
                    return new ResponseModel<Patient>
                    {
                        Success = false,
                        Message = "Phone number is already registered.",
                        Data = null
                    };
                }

                // Proceed with creating user and patient as before
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
                    return new ResponseModel<Patient>
                    {
                        Success = false,
                        Message = $"User registration failed: {errors}",
                        Data = null
                    };
                }

                var roleResult = await _userManager.AddToRoleAsync(user, "Patient");
                if (!roleResult.Succeeded)
                {
                    return new ResponseModel<Patient>
                    {
                        Success = false,
                        Message = "Failed to assign Patient role.",
                        Data = null
                    };
                }

                var patient = new Patient
                {
                    UserId = user.Id,
                    DateOfBirth = dto.DateOfBirth,
                    Gender = dto.Gender,
                    Address = dto.Address,
                };

                await AddAsync(patient);
                await SaveChangesAsync();
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
                await transaction.RollbackAsync();

                return new ResponseModel<Patient>
                {
                    Success = false,
                    Message = $"Patient registration failed: {ex.Message}",
                    Data = null
                };
            }
        }

    }
}
