using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models.Enums;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Mero_Doctor_Project.DTOs.AppointmentDto;
using payment_gateway_nepal;

namespace Mero_Doctor_Project.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDbContext _context;
        public AppointmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<AppointmentBookingResponseDto>> BookAppointmentAsync(BookAppointmentDto dto, string patientUserId)
        {
            try
            {
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == patientUserId);
                if (patient == null)
                    return new ResponseModel<AppointmentBookingResponseDto> { Success = false, Message = "Patient not found.", Data = null };

                var availability = await _context.DoctorWeeklyAvailabilities
                    .Include(a => a.TimeRanges).Include(a => a.Doctor)
                    .FirstOrDefaultAsync(a => a.DoctorId == dto.DoctorId && a.AvailableDate == dto.AvailableDate);

                if (availability == null)
                    return new ResponseModel<AppointmentBookingResponseDto> { Success = false, Message = "Doctor not available on selected date.", Data = null };

                var matchingTimeRange = availability.TimeRanges.FirstOrDefault(tr =>
                    tr.IsAvailable && tr.AvailableTime == dto.AvailableTime);

                if (matchingTimeRange == null)
                    return new ResponseModel<AppointmentBookingResponseDto>
                    {
                        Success = false,
                        Message = "Time slot not available.",
                        Data = null
                    };

                bool hasConflict = await _context.Appointments.AnyAsync(a =>
                    a.DoctorId == dto.DoctorId &&
                    a.AvailableDate == dto.AvailableDate &&
                    a.AvailableTime == dto.AvailableTime);

                if (hasConflict)
                    return new ResponseModel<AppointmentBookingResponseDto>
                    {
                        Success = false,
                        Message = "Slot already booked.",
                        Data = null
                    };

                string transactionId = $"tx-{Guid.NewGuid().ToString("N").Substring(0, 8)}";

                var appointment = new Appointment
                {
                    DoctorId = dto.DoctorId,
                    PatientId = patient.PatientId,
                    AvailableDate = dto.AvailableDate,
                    AvailableTime = dto.AvailableTime,
                    BookingDateTime = DateTime.UtcNow,
                    Status = AppointmentStatus.Pending,
                    Price = dto.Price,
                    TransactionId = transactionId,
                    TransactionStatus = "Pending",
                    Visited = false
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();
                return new ResponseModel<AppointmentBookingResponseDto>
                {
                    Success = true,
                    Message = "Appointment created. Proceed with payment.",
                    Data = new AppointmentBookingResponseDto { TransactionId=transactionId,DoctorUserId= availability.Doctor.UserId }
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<AppointmentBookingResponseDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }


        public async Task<ResponseModel<string>> UpdateAppointmentVisitedAsync(int appointmentId, string doctorUserId)
        {
            try
            {
                var appointment = await _context.Appointments
                    .Include(a => a.Doctor)
                    .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

                if (appointment == null)
                {
                    return new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Appointment not found.",
                        Data = null
                    };
                }

                // Check if the doctor is authorized
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.DoctorId == appointment.DoctorId && d.UserId == doctorUserId);
                if (doctor == null)
                {
                    return new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Unauthorized. You do not have permission to update this appointment.",
                        Data = null
                    };
                }

                appointment.Visited = true;
                await _context.SaveChangesAsync();

                return new ResponseModel<string>
                {
                    Success = true,
                    Message = "Appointment marked as visited.",
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

        public async Task<ResponseModel<string>> UpdateAppointmentStatusAsync(UpdateAppointmentStatusDto dto, string doctorUserId)
        {
            try
            {
                var appointment = await _context.Appointments
                    .Include(a => a.Doctor)
                    .FirstOrDefaultAsync(a => a.AppointmentId == dto.AppointmentId);

                if (appointment == null)
                {
                    return new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Appointment not found.",
                        Data = null
                    };
                }

                // Ensure the logged-in doctor owns this appointment
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.DoctorId == appointment.DoctorId && d.UserId == doctorUserId);
                if (doctor == null)
                {
                    return new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Unauthorized. You do not have permission to update this appointment.",
                        Data = null
                    };
                }

                // Update status (only if it was pending)
                if (appointment.Status != AppointmentStatus.Pending)
                {
                    return new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Only pending appointments can be updated.",
                        Data = null
                    };
                }

                appointment.Status = dto.Status;
                await _context.SaveChangesAsync();

                return new ResponseModel<string>
                {
                    Success = true,
                    Message = $"Appointment {dto.Status.ToString().ToLower()} successfully.",
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
        public async Task<ResponseModel<List<GetAppointmentDto>>> GetAppointmentsByPatientAsync(string patientUserId)
        {
            try
            {
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == patientUserId);
                if (patient == null)
                {
                    return new ResponseModel<List<GetAppointmentDto>>
                    {
                        Success = false,
                        Message = "Patient not found.",
                        Data = null
                    };
                }

                var appointments = await _context.Appointments
                    .Where(a => a.PatientId == patient.PatientId)
                    .Include(a => a.Doctor)
                    .ThenInclude(d => d.User)
                    .OrderByDescending(a => a.BookingDateTime)
                    .ToListAsync();

                var dtoList = appointments.Select(a => new GetAppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    DoctorId = a.DoctorId,
                    PatientId = a.PatientId,
                    Status = a.Status.ToString(),
                    AvailableDate = a.AvailableDate.ToString("yyyy-MM-dd"),  // e.g. 2025-06-18
                    AvailableTime = a.AvailableTime.ToString("hh:mm tt"),     // e.g. 02:45 PM
                    BookingDateTime = a.BookingDateTime.ToString("yyyy-MM-dd hh:mm:ss tt"), // e.g. 2025-06-18 02:45:30 PM
                    DoctorName = a.Doctor.User.FullName,
                    PatientName = null // You can add patient name if needed here
                }).ToList();

                return new ResponseModel<List<GetAppointmentDto>>
                {
                    Success = true,
                    Message = "Appointments fetched successfully.",
                    Data = dtoList
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<GetAppointmentDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ResponseModel<List<GetAppointmentDto>>> GetAppointmentsByDoctorAsync(string doctorUserId)
        {
            try
            {
                var doctor = await _context.Doctors.FirstOrDefaultAsync(p => p.UserId == doctorUserId);
                if (doctor == null)
                {
                    return new ResponseModel<List<GetAppointmentDto>>
                    {
                        Success = false,
                        Message = "Doctor not found.",
                        Data = null
                    };
                }
                var appointments = await _context.Appointments
                    .Where(a => a.DoctorId == doctor.DoctorId)
                    .Include(a => a.Patient)
                        .ThenInclude(p => p.User)
                    .OrderByDescending(a => a.BookingDateTime) // Changed to BookingDateTime from DateTime
                    .ToListAsync();

                var dtoList = appointments.Select(a => new GetAppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    DoctorId = a.DoctorId,
                    PatientId = a.DoctorId,
                    Status = a.Status.ToString(), // convert enum to string
                    AvailableDate = a.AvailableDate.ToString("yyyy-MM-dd"), // format DateOnly as string
                    AvailableTime = a.AvailableTime.ToString("hh:mm tt"),                    // format TimeOnly as string
                    BookingDateTime = a.BookingDateTime.ToString("yyyy-MM-dd hh:mm:ss tt"), // format DateTime as string
                    PatientName = a.Patient.User.FullName,
                    DoctorName = null // You can fill this if you include Doctor and Doctor.User as well
                }).ToList();

                return new ResponseModel<List<GetAppointmentDto>>
                {
                    Success = true,
                    Message = "Appointments fetched successfully.",
                    Data = dtoList
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<GetAppointmentDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }
        public async Task<ResponseModel<List<GetAppointmentDto>>> GetTodaysDoctorAppontmentsAsync(string doctorUserId)
        {
            try
            {
                var doctor = await _context.Doctors.FirstOrDefaultAsync(p => p.UserId == doctorUserId);
                if (doctor == null)
                {
                    return new ResponseModel<List<GetAppointmentDto>>
                    {
                        Success = false,
                        Message = "Doctor not found.",
                        Data = null
                    };
                }

                var today = DateOnly.FromDateTime(DateTime.Today);

                var appointments = await _context.Appointments
                    .Where(a =>
                        a.DoctorId == doctor.DoctorId &&
                        a.AvailableDate >= today &&
                        !a.Visited)
                    .Include(a => a.Patient)
                        .ThenInclude(p => p.User)
                    .OrderBy(a => a.AvailableDate)
                    .ThenBy(a => a.AvailableTime)
                    .ToListAsync();

                var dtoList = appointments.Select(a => new GetAppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    DoctorId = a.DoctorId,
                    PatientId = a.PatientId,
                    Status = a.Status.ToString(),
                    AvailableDate = a.AvailableDate.ToString("yyyy-MM-dd"),
                    AvailableTime = a.AvailableTime.ToString("hh:mm tt"),
                    BookingDateTime = a.BookingDateTime.ToString("yyyy-MM-dd hh:mm:ss tt"),
                    PatientName = a.Patient.User.FullName,
                    ProfilePictureUrl = a.Patient.User.ProfilePictureUrl, // ✅ Added
                    DoctorName = null
                }).ToList();

                return new ResponseModel<List<GetAppointmentDto>>
                {
                    Success = true,
                    Message = "Upcoming unvisited appointments fetched successfully.",
                    Data = dtoList
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<GetAppointmentDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ResponseModel<List<GetAppointmentDto>>> GetTodaysPatientAppontmentsAsync(string patientUserId)
        {
            try
            {
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == patientUserId);
                if (patient == null)
                {
                    return new ResponseModel<List<GetAppointmentDto>>
                    {
                        Success = false,
                        Message = "Patient not found.",
                        Data = null
                    };
                }

                var today = DateOnly.FromDateTime(DateTime.Today);

                var appointments = await _context.Appointments
                    .Where(a => a.PatientId == patient.PatientId && a.AvailableDate >= today)
                    .Include(a => a.Doctor)
                        .ThenInclude(d => d.User)
                    .OrderBy(a => a.AvailableDate)
                    .ThenBy(a => a.AvailableTime)
                    .ToListAsync();

                var dtoList = appointments.Select(a => new GetAppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    DoctorId = a.DoctorId,
                    PatientId = a.PatientId,
                    Status = a.Status.ToString(),
                    AvailableDate = a.AvailableDate.ToString("yyyy-MM-dd"),
                    AvailableTime = a.AvailableTime.ToString("hh:mm tt"),
                    BookingDateTime = a.BookingDateTime.ToString("yyyy-MM-dd hh:mm:ss tt"),
                    DoctorName = a.Doctor.User.FullName,
                    ProfilePictureUrl = a.Doctor.User.ProfilePictureUrl, // ✅ Added
                    PatientName = null
                }).ToList();

                return new ResponseModel<List<GetAppointmentDto>>
                {
                    Success = true,
                    Message = "Upcoming appointments fetched successfully.",
                    Data = dtoList
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<GetAppointmentDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ResponseModel<List<GetAppointmentDto>>> GetAllUpcomingAppointmentsAsync()
        {
            try
            {
                var today = DateOnly.FromDateTime(DateTime.Today);

                var appointments = await _context.Appointments
                    .Where(a => a.AvailableDate >= today)
                    .Include(a => a.Doctor)
                        .ThenInclude(d => d.User)
                    .OrderBy(a => a.AvailableDate)
                    .ThenBy(a => a.AvailableTime)
                    .ToListAsync();

                var dtoList = appointments.Select(a => new GetAppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    DoctorId = a.DoctorId,              
                    Status = a.Status.ToString(),
                    AvailableDate = a.AvailableDate.ToString("yyyy-MM-dd"),
                    AvailableTime = a.AvailableTime.ToString("hh:mm tt"),
                    BookingDateTime = a.BookingDateTime.ToString("yyyy-MM-dd hh:mm:ss tt"),
                    DoctorName = a.Doctor.User.FullName,
                }).ToList();

                return new ResponseModel<List<GetAppointmentDto>>
                {
                    Success = true,
                    Message = "Upcoming appointments for all doctors fetched successfully.",
                    Data = dtoList
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<GetAppointmentDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }


    }

}
