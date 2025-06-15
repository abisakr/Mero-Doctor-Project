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

        public async Task<ResponseModel<List<GetSpecializationDto>>> GetAllAsync()
        {
            try
            {
                var specializations = await _dbSet.ToListAsync();
                var dtoList = _mapper.Map<List<GetSpecializationDto>>(specializations);

                return new ResponseModel<List<GetSpecializationDto>>
                {
                    Success = true,
                    Message = "Fetched all",
                    Data = dtoList
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<GetSpecializationDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ResponseModel<GetSpecializationDto>> GetByIdAsync(int id)
        {
            try
            {
                var specialization = await _dbSet.FindAsync(id);
                if (specialization == null)
                {
                    return new ResponseModel<GetSpecializationDto>
                    {
                        Success = false,
                        Message = "Not found",
                        Data = null
                    };
                }

                var dto = _mapper.Map<GetSpecializationDto>(specialization);
                return new ResponseModel<GetSpecializationDto>
                {
                    Success = true,
                    Message = "Found",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<GetSpecializationDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ResponseModel<SetSpecializationDto>> AddAsync(SetSpecializationDto dto)
        {
            try
            {
                var specialization = _mapper.Map<Specialization>(dto);
                await AddAsync(specialization);
                await SaveChangesAsync();

                var resultDto = _mapper.Map<SetSpecializationDto>(specialization);
                return new ResponseModel<SetSpecializationDto>
                {
                    Success = true,
                    Message = "Created",
                    Data = resultDto
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<SetSpecializationDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ResponseModel<SetSpecializationDto>> UpdateAsync(int id, SetSpecializationDto dto)
        {
            try
            {
                var specialization = await _dbSet.FindAsync(id);
                if (specialization == null)
                {
                    return new ResponseModel<SetSpecializationDto>
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

                var resultDto = _mapper.Map<SetSpecializationDto>(specialization);
                return new ResponseModel<SetSpecializationDto>
                {
                    Success = true,
                    Message = "Updated",
                    Data = resultDto
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<SetSpecializationDto>
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
