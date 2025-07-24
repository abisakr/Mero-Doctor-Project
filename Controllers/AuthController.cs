using System.Data;
using System.Net;
using System.Net.Mail;
using System.Reflection.Metadata;
using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.DTOs.AuthDto;
using Mero_Doctor_Project.Helper;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mero_Doctor_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController( IAuthRepository authRepository,UserManager<ApplicationUser> userManager)
        {
            _authRepository = authRepository;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseModel<string>>> LoginAsync([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
               return BadRequest(ModelState); 
            }

            var result = await _authRepository.LoginAsync(loginDto);

            if (result.Success)
                return Ok(result);

            return Unauthorized(result);
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid email.",
                    Data = null
                }); 
            var roles= await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Sorry, cannot proceed with the request.",
                    Data = null
                });
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"merodoctor://reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email)}";

            // Send Email
            string fromMail = "abiskar.ag@gmail.com";
            string fromPassword = "ejoooddorojxudoe";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = "Reset Your Password";
            message.To.Add(new MailAddress(user.Email));
            message.Body = $"<html><body>Click <a href='{resetLink}'>here</a> to reset your password.</body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            await smtpClient.SendMailAsync(message);
            return  Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Reset link is sent if Email is correct.",
                    Data = null
            });  
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid request.",
                    Data = null
                });

            var result = await _userManager.ResetPasswordAsync(user, WebUtility.UrlDecode(model.Token), model.NewPassword);

            if (!result.Succeeded) 
                 return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Error on resetting the Password.",
                    Data = null
                });

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Password reset successfully.",
                Data = null
            });
        }

    }


}

