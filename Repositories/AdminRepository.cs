using AutoMapper;
using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.DTOs.Admin;
using Mero_Doctor_Project.DTOs.Specilization;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models.Enums;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Mero_Doctor_Project.Repositories
{
    public class AdminRepository : Repository<Doctor>, IAdminRepository
    {
        private readonly IMapper _mapper;
        public AdminRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<ResponseModel<UpdateDoctorInfoDto>> VerifyDoctorAsync(int id, DoctorStatus status)
        {
            try
            {
                var doctor = await _dbSet.FindAsync(id);
                if (doctor == null)
                {
                    return new ResponseModel<UpdateDoctorInfoDto>
                    {
                        Success = false,
                        Message = "Doctor not found",
                        Data = null
                    };
                }

                if (doctor.Status == DoctorStatus.Verified)
                {
                    return new ResponseModel<UpdateDoctorInfoDto>
                    {
                        Success = false,
                        Message = "Doctor already verified",
                        Data = null
                    };
                }

                if (doctor.Status == DoctorStatus.Rejected)
                {
                    return new ResponseModel<UpdateDoctorInfoDto>
                    {
                        Success = false,
                        Message = "Doctor is rejected",
                        Data =null
                    };
                }

                doctor.Status = status;
                Update(doctor);
                await SaveChangesAsync();

                return new ResponseModel<UpdateDoctorInfoDto>
                {
                    Success = true,
                    Message = $"Doctor status updated to: {status}",
                    Data = new UpdateDoctorInfoDto { UserId = doctor.UserId }
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<UpdateDoctorInfoDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ResponseModel<List<GetDoctorInfoDto>>> GetVerifiedDoctorsAsync()
        {
            try
            {
                var verifiedDoctors = await _dbSet
                    .Include(d => d.Specialization)
                    .Include(d => d.User)
                    .Where(d => d.Status == DoctorStatus.Verified)
                    .Select(d => new GetDoctorInfoDto
                    {
                        DoctorId=d.DoctorId,
                        FullName = d.User.FullName,
                        Email = d.User.Email,
                        PhoneNumber = d.User.PhoneNumber,
                        Degree = d.Degree,
                        Experience = d.Experience,
                        RegistrationId = d.RegistrationId,
                        ClinicAddress = d.ClinicAddress,
                        Specialization = d.Specialization.Name,
                        Status=d.Status.ToString()
                    })
                    .ToListAsync();

                if (!verifiedDoctors.Any())
                {
                    return new ResponseModel<List<GetDoctorInfoDto>>
                    {
                        Success = false,
                        Message = "No verified doctors found.",
                        Data = null
                    };
                }

                return new ResponseModel<List<GetDoctorInfoDto>>
                {
                    Success = true,
                    Message = $"{verifiedDoctors.Count} verified doctor(s) found",
                    Data = verifiedDoctors
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<GetDoctorInfoDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ResponseModel<List<GetDoctorInfoDto>>> GetPendingDoctorsAsync()
        {
            try
            {
                var pendingDoctors = await _dbSet
                    .Include(d => d.Specialization)
                    .Include(d => d.User)
                    .Where(d => d.Status == DoctorStatus.Pending)
                    .Select(d => new GetDoctorInfoDto
                    {
                        DoctorId = d.DoctorId,
                        FullName = d.User.FullName,
                        Email = d.User.Email,
                        PhoneNumber = d.User.PhoneNumber,
                        Degree = d.Degree,
                        Experience = d.Experience,
                        RegistrationId = d.RegistrationId,
                        ClinicAddress = d.ClinicAddress,
                        Specialization = d.Specialization.Name,
                        Status = d.Status.ToString()
                    })
                    .ToListAsync();

                if (!pendingDoctors.Any())
                {
                    return new ResponseModel<List<GetDoctorInfoDto>>
                    {
                        Success = false,
                        Message = "No pending doctors found",
                        Data = null
                    };
                }

                return new ResponseModel<List<GetDoctorInfoDto>>
                {
                    Success = true,
                    Message = $"{pendingDoctors.Count} pending doctor(s) found",
                    Data = pendingDoctors
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<GetDoctorInfoDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }


        public async Task<ResponseModel<List<GetDoctorInfoDto>>> GetRejectedDoctorsAsync()
        {
            try
            {
                var rejectedDoctors = await _dbSet
                    .Include(d => d.Specialization)
                    .Include(d => d.User)
                    .Where(d => d.Status == DoctorStatus.Rejected)
                    .Select(d => new GetDoctorInfoDto
                    {
                        DoctorId = d.DoctorId,
                        FullName = d.User.FullName,
                        Email = d.User.Email,
                        PhoneNumber = d.User.PhoneNumber,
                        Degree = d.Degree,
                        Experience = d.Experience,
                        RegistrationId = d.RegistrationId,
                        ClinicAddress = d.ClinicAddress,
                        Specialization = d.Specialization.Name,
                        Status = d.Status.ToString()
                    })
                    .ToListAsync();

                if (!rejectedDoctors.Any())
                {
                    return new ResponseModel<List<GetDoctorInfoDto>>
                    {
                        Success = false,
                        Message = "No rejected doctors found",
                        Data = null
                    };
                }

                return new ResponseModel<List<GetDoctorInfoDto>>
                {
                    Success = true,
                    Message = $"{rejectedDoctors.Count} rejected doctor(s) found",
                    Data = rejectedDoctors
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<GetDoctorInfoDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

    }
}


    
