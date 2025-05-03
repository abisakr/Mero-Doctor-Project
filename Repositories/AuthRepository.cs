using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.DTOs.AuthDto;
using Mero_Doctor_Project.Helper;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Mero_Doctor_Project.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenGenerator _tokenGenerator;
        public AuthRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager, TokenGenerator tokenGenerator)
        {
            _context = context;
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }
        // Implement methods for authentication here
        public async Task<ResponseModel<string>> LoginAsync(LoginDto loginDto)
        {
            var response = new ResponseModel<string>();

            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                {
                    response.Success = false;
                    response.Message = "Invalid Email or password.";
                    response.Data = null;
                    return response;
                }

                var roles = await _userManager.GetRolesAsync(user);

                // Generate token using your token generator (e.g., JWT token)
                var token = _tokenGenerator.GenerateToken(user.Id, user.FullName, roles);

                response.Success = true;
                response.Message = "Login successful.";
                response.Data = token;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Login failed: {ex.Message}";
                response.Data = null;
                return response;
            }
        }
    }
}