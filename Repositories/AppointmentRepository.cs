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
        public async Task<ResponseModel<string>> BookAppointmentAsync(BookAppointmentDto dto, string userId)
        {
            try
            {
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
                if (patient == null) return new ResponseModel<string> { Success = true, Message = "Patient not found.", Data = null };

                var availability = await _context.DoctorWeeklyAvailabilities
                    .Include(a => a.TimeRanges)
                    .FirstOrDefaultAsync(a => a.DoctorId == dto.DoctorId && a.DayOfWeek == dto.DayOfWeek);

                if (availability == null) return new ResponseModel<string> { Success = true, Message = "Doctor not available on selected day.", Data = null };

                var matchingTimeRange = availability.TimeRanges.FirstOrDefault(tr =>
                    tr.IsAvailable &&
                    dto.StartTime >= tr.StartTime &&
                    dto.EndTime <= tr.EndTime);

                if (matchingTimeRange == null) return new ResponseModel<string>
                {
                    Success = true,
                    Message = "Time slot not available.",
                    Data = null
                };

                bool hasConflict = await _context.Appointments.AnyAsync(a =>
                    a.DoctorId == dto.DoctorId &&
                    a.DateTime.Date == dto.AppointmentDate.Date &&
                    ((dto.StartTime >= a.StartTime && dto.StartTime < a.EndTime) ||
                     (dto.EndTime > a.StartTime && dto.EndTime <= a.EndTime)));

                if (hasConflict) return new ResponseModel<string>
                {
                    Success = true,
                    Message = "Slot already booked.",
                    Data = null
                };

                string transactionId = $"tx-{Guid.NewGuid().ToString("N").Substring(0, 8)}";
                decimal price = 1000; 

                var appointment = new Appointment
                {
                    DoctorId = dto.DoctorId,
                    PatientId = patient.PatientId,
                    DayOfWeek = dto.DayOfWeek,
                    DateTime = DateTime.UtcNow,
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime,
                    Status = AppointmentStatus.Pending,
                    Price = price,
                    TransactionId = transactionId,
                    TransactionStatus = "Pending"
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                return new ResponseModel<string>
                {
                    Success = true,
                    Message = "Appointment created. Proceed with payment.",
                    Data = transactionId
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<string>
                {
                    Success = true,
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
        public async Task<ResponseModel<List<AppointmentDto>>> GetAppointmentsByPatientAsync(string userId)
        {
            try
            {
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
                if (patient == null)
                {
                    return new ResponseModel<List<AppointmentDto>>
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
                    .OrderByDescending(a => a.DateTime)
                    .ToListAsync();

                var dtoList = appointments.Select(a => new AppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    DoctorId = a.DoctorId,
                    PatientId = a.PatientId,
                    Status = a.Status,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    AppointmentDate = a.DateTime,
                    DoctorName = a.Doctor.User.FullName 
                }).ToList();

                return new ResponseModel<List<AppointmentDto>>
                {
                    Success = true,
                    Message = "Appointments fetched successfully.",
                    Data = dtoList
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<AppointmentDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }
        public async Task<ResponseModel<List<AppointmentDto>>> GetAppointmentsByDoctorAsync(int doctorId)
        {
            try
            {
                var appointments = await _context.Appointments
                       .Where(a => a.DoctorId == doctorId)
                       .Include(a => a.Patient)
                       .ThenInclude(p => p.User) 
                       .OrderByDescending(a => a.DateTime)
                       .ToListAsync();


                var dtoList = appointments.Select(a => new AppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    DoctorId = a.DoctorId,
                    PatientId = a.PatientId,
                    Status = a.Status,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    AppointmentDate = a.DateTime,
                    PatientName = a.Patient.User.FullName 
                }).ToList();

                return new ResponseModel<List<AppointmentDto>>
                {
                    Success = true,
                    Message = "Appointments fetched successfully.",
                    Data = dtoList
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<AppointmentDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

    }

}
