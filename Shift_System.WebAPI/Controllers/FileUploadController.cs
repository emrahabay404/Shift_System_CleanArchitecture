using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Interfaces;

namespace Shift_System.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUploadService _fileUploadService;

        public FileUploadController(IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        // Tek dosya yükleme işlemi
        [HttpPost("upload-single")]
        public async Task<IActionResult> UploadSingleFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("Dosya seçilmedi.");

                // Dosya yükleme servisini kullanıyoruz
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                var filePath = await _fileUploadService.UploadFileAsync(file, folderPath);

                if (string.IsNullOrEmpty(filePath))
                    return BadRequest("Dosya yüklenemedi.");

                return Ok(new { success = true, message = "Dosya yüklendi.", filePath });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Çoklu dosya yükleme işlemi
        [HttpPost("upload-multiple")]
        public async Task<IActionResult> UploadMultipleFiles(List<IFormFile> files)
        {
            try
            {
                if (files == null || files.Count == 0)
                    return BadRequest("Dosya seçilmedi.");

                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                var filePaths = new List<string>();

                foreach (var file in files)
                {
                    var filePath = await _fileUploadService.UploadFileAsync(file, folderPath);
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        filePaths.Add(filePath);
                    }
                }

                if (filePaths.Count == 0)
                    return BadRequest("Dosyalar yüklenemedi.");

                return Ok(new { success = true, message = "Dosyalar yüklendi.", filePaths });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpDelete("delete-file/{fileName}")]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            // Uygulamanın çalışma dizinini kontrol edelim
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            // Dosya uzantısı belirtilmediyse otomatik olarak tespit edelim
            var filesInDirectory = Directory.GetFiles(folderPath);
            string fullFilePath = null;

            // Uzantısız dosya adını bul ve eşleşen dosyayı tespit et
            foreach (var file in filesInDirectory)
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                if (fileNameWithoutExtension.Equals(fileName, StringComparison.OrdinalIgnoreCase))
                {
                    fullFilePath = file;  // Eşleşen dosya bulundu
                    break;
                }
            }

            if (fullFilePath == null)
            {
                Console.WriteLine("Dosya bulunamadı, dosya adını ve uzantısını kontrol edin.");
                return NotFound(new { success = false, message = $"Dosya bulunamadı: {fileName}" });
            }

            // Dosya bulundu ve silme işlemi yapılacak
            if (System.IO.File.Exists(fullFilePath))
            {
                System.IO.File.Delete(fullFilePath);
                return Ok(new { success = true, message = "Dosya başarıyla silindi!" });
            }

            return NotFound(new { success = false, message = $"Dosya bulunamadı: {fileName}" });
        }



    }
}
