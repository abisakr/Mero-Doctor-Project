using System.Collections.Generic;
using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.DTOs.DoctorDto;
using Mero_Doctor_Project.DTOs.PatientDto;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Mero_Doctor_Project.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;
        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<GetPatientDto>> GetPatientByIdAsync(string userId)
        {
            try
            {
                var patient = await _context.Patients
                    .Include(d => d.User)
                    .FirstOrDefaultAsync(d => d.UserId == userId);

                if (patient == null)
                {
                    return new ResponseModel<GetPatientDto>
                    {
                        Success = false,
                        Message = "Patient not found."
                    };
                }

                var dto = new GetPatientDto
                {
                    UserId = patient.UserId,
                    FullName = patient.User.FullName,
                    Email = patient.User.Email,
                    PhoneNumber = patient.User.PhoneNumber,
                    ProfilePictureUrl = patient.User.ProfilePictureUrl,
                    Address = patient.Address,
                    Gender=patient.Gender.ToString(),
                    Latitude = patient  .User.Latitude,
                    Longitude = patient.User.Longitude
                };

                return new ResponseModel<GetPatientDto>
                {
                    Success = true,
                    Message = "Patient Fetched.",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<GetPatientDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }
        public async Task<ResponseModel<List<GetPatientDto>>> GetAllPatientsAsync()
        {
            try
            {
                var patient= await _context.Patients
                    .Include(d => d.User)
                    .ToListAsync();

                var dtoList = patient.Select(patient => new  GetPatientDto 
                {
                    UserId = patient.UserId,
                    FullName = patient.User.FullName,
                    Email = patient.User.Email,
                    PhoneNumber = patient.User.PhoneNumber,
                    ProfilePictureUrl = patient.User.ProfilePictureUrl,
                    Address = patient.Address,
                    Gender=patient.Gender.ToString(),
                    Latitude = patient  .User.Latitude,
                    Longitude = patient.User.Longitude
                }).ToList();

                return new ResponseModel<List<GetPatientDto>>
                {
                    Success = true,
                    Message = "Patients fetched successfully.",
                    Data = dtoList
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<GetPatientDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }
    }
    
}
