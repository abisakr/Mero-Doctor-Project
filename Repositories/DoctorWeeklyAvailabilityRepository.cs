using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Mero_Doctor_Project.DTOs.DoctorDto;

namespace Mero_Doctor_Project.Repositories
{
    public class DoctorWeeklyAvailabilityRepository : IDoctorWeeklyAvailabilityRepository
    {
        private readonly ApplicationDbContext _context;
        public DoctorWeeklyAvailabilityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<string>> SetDoctorAvailabilityAsync(SetDoctorAvailabilityDto dto, string userId)
        {
            try
            {
                foreach (var availability in dto.Availabilities)
                {
                    var existingDay = await _context.DoctorWeeklyAvailabilities
                        .Include(d => d.TimeRanges)
                        .Include(d => d.Doctor)
                        .FirstOrDefaultAsync(d => d.Doctor.UserId == userId && d.AvailableDate == availability.AvailableDate);

                    if (existingDay != null)
                    {
                        foreach (var time in availability.TimeRanges)
                        {
                            bool isDuplicate = existingDay.TimeRanges.Any(tr =>
                                tr.AvailableTime == time.AvailableTime);

                            if (!isDuplicate)
                            {
                                var newRange = new DoctorWeeklyTimeRange
                                {
                                    AvailableTime = time.AvailableTime,
                                    IsAvailable = true,
                                    DoctorWeeklyAvailabilityId = existingDay.DoctorWeeklyAvailabilityId
                                };
                                await _context.DoctorWeeklyTimeRanges.AddAsync(newRange);
                            }
                        }
                    }
                    else
                    {
                        var validTimeRanges = new List<DoctorWeeklyTimeRange>();

                        foreach (var time in availability.TimeRanges)
                        {
                            if (!validTimeRanges.Any(tr => tr.AvailableTime == time.AvailableTime))
                            {
                                validTimeRanges.Add(new DoctorWeeklyTimeRange
                                {
                                    AvailableTime = time.AvailableTime,
                                    IsAvailable = true
                                });
                            }
                        }

                        var doctorId = await _context.Doctors
                            .Where(d => d.UserId == userId)
                            .Select(d => d.DoctorId)
                            .FirstOrDefaultAsync();

                        var newDay = new DoctorWeeklyAvailability
                        {
                            DoctorId = doctorId,
                            AvailableDate = availability.AvailableDate,
                            TimeRanges = validTimeRanges
                        };

                        _context.DoctorWeeklyAvailabilities.Add(newDay);
                    }
                }

                await _context.SaveChangesAsync();

                return new ResponseModel<string>
                {
                    Success = true,
                    Message = "Doctor availability updated successfully.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<string>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ResponseModel<GetDoctorAvailabilityDto>> GetDoctorAvailabilityAsync(string doctorId)
        {
            try
            {
                var availabilities = await _context.DoctorWeeklyAvailabilities
                    .Where(a => a.Doctor.UserId == doctorId)
                    .Include(a => a.Doctor)
                    .Include(a => a.TimeRanges)
                    .ToListAsync();

                if (!availabilities.Any())
                {
                    return new ResponseModel<GetDoctorAvailabilityDto>
                    {
                        Success = false,
                        Message = "No availability found for this doctor.",
                        Data = null
                    };
                }

                var dto = new GetDoctorAvailabilityDto
                {
                    DoctorUserId = doctorId,
                    Availabilities = availabilities.Select(a => new GetDayAvailabilityDto
                    {
                        DoctorWeeklyAvailabilityId=a.DoctorWeeklyAvailabilityId,   
                        AvailableDate = a.AvailableDate.ToString("yyyy-MM-dd"),
                        DayOfWeek = a.AvailableDate.DayOfWeek.ToString(),   
                        TimeRanges = a.TimeRanges.Select(tr => new GetTimeRangeDto
                        {   TimeRangeId=tr.DoctorWeeklyTimeRangeId,
                            AvailableTime = tr.AvailableTime.ToString("hh:mm tt"),
                            IsAvailable = tr.IsAvailable ? "Yes" : "No" 
                        }).ToList()
                    }).ToList()
                };

                return new ResponseModel<GetDoctorAvailabilityDto>
                {
                    Success = true,
                    Message = "Doctor availability fetched successfully.",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<GetDoctorAvailabilityDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ResponseModel<string>> DeleteDoctorDayAvailabililtyAsync(DeleteWeekdayDto dto, string userId)
        {
            try
            {
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId ==userId );
                if (doctor == null)
                {
                    return new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Unauthorized or doctor not found.",
                        Data = null
                    };
                }

                var availability = await _context.DoctorWeeklyAvailabilities
                    .Include(d => d.TimeRanges)
                    .FirstOrDefaultAsync(d => d.DoctorId == doctor.DoctorId && d.DoctorWeeklyAvailabilityId == dto.DoctorWeeklyAvailabilityId);

                if (availability == null)
                {
                    return new ResponseModel<string>
                    {
                        Success = false,
                        Message = "No availability found for the given day.",
                        Data = null
                    };
                }

                _context.DoctorWeeklyTimeRanges.RemoveRange(availability.TimeRanges);
                _context.DoctorWeeklyAvailabilities.Remove(availability);
                await _context.SaveChangesAsync();

                return new ResponseModel<string>
                {
                    Success = true,
                    Message = "Day availability deleted successfully.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<string>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ResponseModel<string>> DeleteDoctorTimeRangeAsync(DeleteTimeRangeDto dto, string userId)
        {
            try
            {
                var timeRange = await _context.DoctorWeeklyTimeRanges
                    .Include(tr => tr.WeeklyAvailability)
                    .ThenInclude(wa => wa.Doctor)
                    .FirstOrDefaultAsync(tr => tr.DoctorWeeklyTimeRangeId == dto.TimeRangeId);

                if (timeRange == null)
                {
                    return new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Time range not found.",
                        Data = null
                    };
                }

                if (timeRange.WeeklyAvailability.Doctor.UserId != userId)
                {
                    return new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Unauthorized: You can only delete your own time range.",
                        Data = null
                    };
                }

                _context.DoctorWeeklyTimeRanges.Remove(timeRange);
                await _context.SaveChangesAsync();

                return new ResponseModel<string>
                {
                    Success = true,
                    Message = "Time range deleted successfully.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<string>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
