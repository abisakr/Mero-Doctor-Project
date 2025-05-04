using AutoMapper;
using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.DTOs.Specilization;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Mero_Doctor_Project.Repositories
{
    public class SpecializationRepository : Repository<Specialization>, ISpecializationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SpecializationRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseModel<List<SpecializationDto>>> GetAllAsync()
        {
            try
            {
                var specializations = await _dbSet.ToListAsync();
                var dtoList = _mapper.Map<List<SpecializationDto>>(specializations);

                return new ResponseModel<List<SpecializationDto>>
                {
                    Success = true,
                    Message = "Fetched all",
                    Data = dtoList
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<SpecializationDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ResponseModel<SpecializationDto>> GetByIdAsync(int id)
        {
            try
            {
                var specialization = await _dbSet.FindAsync(id);
                if (specialization == null)
                {
                    return new ResponseModel<SpecializationDto>
                    {
                        Success = false,
                        Message = "Not found",
                        Data = null
                    };
                }

                var dto = _mapper.Map<SpecializationDto>(specialization);
                return new ResponseModel<SpecializationDto>
                {
                    Success = true,
                    Message = "Found",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<SpecializationDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ResponseModel<SpecializationDto>> AddAsync(SpecializationDto dto)
        {
            try
            {
                var specialization = _mapper.Map<Specialization>(dto);
                await AddAsync(specialization);
                await SaveChangesAsync();

                var resultDto = _mapper.Map<SpecializationDto>(specialization);
                return new ResponseModel<SpecializationDto>
                {
                    Success = true,
                    Message = "Created",
                    Data = resultDto
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<SpecializationDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ResponseModel<SpecializationDto>> UpdateAsync(int id, SpecializationDto dto)
        {
            try
            {
                var specialization = await _dbSet.FindAsync(id);
                if (specialization == null)
                {
                    return new ResponseModel<SpecializationDto>
                    {
                        Success = false,
                        Message = "Not found",
                        Data = null
                    };
                }

                // Map updated properties from DTO
                _mapper.Map(dto, specialization);
                Update(specialization);
                await SaveChangesAsync();

                var resultDto = _mapper.Map<SpecializationDto>(specialization);
                return new ResponseModel<SpecializationDto>
                {
                    Success = true,
                    Message = "Updated",
                    Data = resultDto
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<SpecializationDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ResponseModel<bool>> DeleteAsync(int id)
        {
            try
            {
                var specialization = await _dbSet.FindAsync(id);
                if (specialization == null)
                {
                    return new ResponseModel<bool>
                    {
                        Success = false,
                        Message = "Not found",
                        Data = false
                    };
                }

                Delete(specialization);
                await SaveChangesAsync();

                return new ResponseModel<bool>
                {
                    Success = true,
                    Message = "Deleted",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = false
                };
            }
        }
    }
}
