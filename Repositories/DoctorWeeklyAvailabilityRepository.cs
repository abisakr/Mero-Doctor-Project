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
        public async Task<ResponseModel<string>> SetDoctorAvailabilityAsync(DoctorAvailabilityDto dto)
        {
            try
            {
                foreach (var dayAvailability in dto.Availabilities)
                {
                    var existingDay = await _context.DoctorWeeklyAvailabilities
                        .Include(d => d.TimeRanges)
                        .FirstOrDefaultAsync(d =>
                            d.DoctorId == dto.DoctorId && d.DayOfWeek == dayAvailability.DayOfWeek);

                    if (existingDay != null)
                    {
                        foreach (var time in dayAvailability.TimeRanges)
                        {
                            bool isDuplicate = existingDay.TimeRanges.Any(tr =>
                                tr.StartTime == time.StartTime && tr.EndTime == time.EndTime);

                            bool isOverlapping = existingDay.TimeRanges.Any(tr =>
                                time.StartTime < tr.EndTime && time.EndTime > tr.StartTime);

                            if (!isDuplicate && !isOverlapping)
                            {
                                var newRange = new DoctorWeeklyTimeRange
                                {
                                    StartTime = time.StartTime,
                                    EndTime = time.EndTime,
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

                        foreach (var time in dayAvailability.TimeRanges)
                        {
                            // Avoid self-overlapping within new time ranges
                            if (validTimeRanges.Any(tr =>
                                time.StartTime < tr.EndTime && time.EndTime > tr.StartTime))
                            {
                                continue; // Skip overlapping slot
                            }

                            validTimeRanges.Add(new DoctorWeeklyTimeRange
                            {
                                StartTime = time.StartTime,
                                EndTime = time.EndTime,
                                IsAvailable = true
                            });
                        }

                        var newDay = new DoctorWeeklyAvailability
                        {
                            DoctorId = dto.DoctorId,
                            DayOfWeek = dayAvailability.DayOfWeek,
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



        public async Task<ResponseModel<DoctorAvailabilityDto>> GetDoctorAvailabilityAsync(int doctorId)
        {
            try
            {
                var availabilities = await _context.DoctorWeeklyAvailabilities
                    .Where(a => a.DoctorId == doctorId)
                    .Include(a => a.TimeRanges)
                    .ToListAsync();

                if (!availabilities.Any())
                {
                    return new ResponseModel<DoctorAvailabilityDto>
                    {
                        Success = false,
                        Message = "No availability found for this doctor.",
                        Data = null
                    };
                }

                var dto = new DoctorAvailabilityDto
                {
                    DoctorId = doctorId,
                    Availabilities = availabilities.Select(a => new DayAvailabilityDto
                    {
                        DayOfWeek = a.DayOfWeek,
                        TimeRanges = a.TimeRanges.Select(tr => new TimeRangeDto
                        {
                            StartTime = tr.StartTime,
                            EndTime = tr.EndTime,
                            IsAvailable = tr.IsAvailable
                        }).ToList()
                    }).ToList()
                };

                return new ResponseModel<DoctorAvailabilityDto>
                {
                    Success = true,
                    Message = "Doctor availability fetched successfully.",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<DoctorAvailabilityDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }


        public async Task<ResponseModel<string>> DeleteDoctorWeekdayAsync(DeleteWeekdayDto dto, string userId)
        {
            try
            {
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.DoctorId == dto.DoctorId && d.UserId == userId);
                if (doctor == null)
                {
                    return new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Unauthorized or doctor not found.",
                        Data = null
                    };
                }

                var day = await _context.DoctorWeeklyAvailabilities
                    .Include(d => d.TimeRanges)
                    .FirstOrDefaultAsync(d => d.DoctorId == dto.DoctorId && d.DayOfWeek == dto.DayOfWeek);

                if (day == null)
                {
                    return new ResponseModel<string>
                    {
                        Success = false,
                        Message = "No availability found for the given day.",
                        Data = null
                    };
                }

                _context.DoctorWeeklyTimeRanges.RemoveRange(day.TimeRanges);
                _context.DoctorWeeklyAvailabilities.Remove(day);
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

