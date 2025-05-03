using Mero_Doctor_Project.DTOs.AuthDto;
using Mero_Doctor_Project.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace Mero_Doctor_Project.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        public Task<ResponseModel<string>> LoginAsync(LoginDto loginDto);
    }
}
