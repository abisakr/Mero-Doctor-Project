using Mero_Doctor_Project.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mero_Doctor_Project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestingController : ControllerBase
    {
            
        private readonly IWebHostEnvironment _environment;
        private readonly UploadImageHelper _uploadImageHelper;

        public TestingController(IWebHostEnvironment environment,UploadImageHelper uploadImageHelper)
        {
           _environment = environment;
            _uploadImageHelper = uploadImageHelper;
        }

       

        [HttpPost("upload")]      
        public async Task<IActionResult> UploadImage(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            // Ensure the file is an image
            if (!file.ContentType.StartsWith("image/"))
                return BadRequest("File is not an image.");

         string  filePath = await _uploadImageHelper.UploadImageAsync(file, folderName);   

            return Ok(new { FilePath = filePath });
        }



        [HttpPost("replace/{folderName}/{fileName}")]
        public async Task<IActionResult> ReplaceImage(string folderName, string fileName, IFormFile newFile)
        {
            if (newFile == null || newFile.Length == 0)
                return BadRequest("No new file uploaded.");

            if (!newFile.ContentType.StartsWith("image/"))
                return BadRequest("File is not an image.");

            var filePath = await _uploadImageHelper.ReplaceImageAsync(folderName, fileName, newFile);
            if (filePath == null)
                return NotFound("Image not found.");
            return Ok(new { FilePath = filePath });
        }
    }
}