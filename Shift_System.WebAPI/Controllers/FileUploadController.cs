using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Teams.Commands;
using Shift_System.Application.Interfaces;
using Shift_System.Shared.Helpers;

namespace Shift_System.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUploadService _fileUploadService;
        private readonly IMediator _mediator;

        public FileUploadController(IFileUploadService fileUploadService, IMediator mediator)
        {
            _fileUploadService = fileUploadService;
            _mediator = mediator;
        }

        // Tek dosya yükleme işlemi
        [HttpPost("upload-single")]
        public async Task<IActionResult> UploadSingleFile(IFormFile file)
        {
            const long maxFileSize = 5 * 1024 * 1024; // 5 MB

            if (file == null || file.Length == 0)
            {
                return Ok(new { success = false, message = "Dosya seçilmedi." });
            }

            if (file.Length > maxFileSize)
            {
                return Ok(new { success = false, message = $"Dosya boyutu {maxFileSize / (1024 * 1024)} MB'ı aşıyor." });
            }

            try
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                var folderPathAfterWwwroot = "Uploads";

                var uiProjectDirectory = currentDirectory.Replace(
                    "Shift_System.WebAPI",
                    $@"Shift_System_UI\wwwroot\{folderPathAfterWwwroot}\"
                );

                if (!Directory.Exists(uiProjectDirectory))
                {
                    Directory.CreateDirectory(uiProjectDirectory);
                }

                var newFileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uiProjectDirectory, newFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok(new { success = true, message = "Dosya başarıyla yüklendi.", filePath });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = $"Dosya yüklenirken hata oluştu: {ex.Message}" });
            }
        }

        // Çoklu dosya yükleme işlemi
        [HttpPost("upload-multiple")]
        public async Task<IActionResult> UploadMultipleFiles(List<IFormFile> files)
        {
            const long maxFileSize = 5 * 1024 * 1024; // 5 MB

            var successfullyUploaded = new List<object>(); // Detaylı bilgi için object kullanıyoruz
            var failedUploads = new List<object>(); // Detaylı hata bilgisi için object kullanıyoruz

            try
            {
                if (files == null || files.Count == 0)
                {
                    return Ok(new { success = false, message = "Dosya seçilmedi." });
                }

                var currentDirectory = Directory.GetCurrentDirectory();
                var folderPathAfterWwwroot = "Uploads";

                var uiProjectDirectory = currentDirectory.Replace(
                    "Shift_System.WebAPI",
                    $@"Shift_System_UI\wwwroot\{folderPathAfterWwwroot}\"
                );

                if (!Directory.Exists(uiProjectDirectory))
                {
                    Directory.CreateDirectory(uiProjectDirectory);
                }

                foreach (var file in files)
                {
                    try
                    {
                        if (file.Length > maxFileSize)
                        {
                            failedUploads.Add(new
                            {
                                FileName = file.FileName,
                                Status = false, // Başarısız durumu false olarak döndürülüyor
                                Description = $"Dosya boyutu {maxFileSize / (1024 * 1024)} MB'ı aşıyor."
                            });
                            continue;
                        }

                        var newFileName = $"{Guid.NewGuid()}_{file.FileName}";
                        var filePath = Path.Combine(uiProjectDirectory, newFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        successfullyUploaded.Add(new
                        {
                            FileName = newFileName,
                            Status = true, // Başarılı durumu true olarak döndürülüyor
                            Description = "Dosya başarıyla yüklendi."
                        });
                    }
                    catch (Exception ex)
                    {
                        failedUploads.Add(new
                        {
                            FileName = file.FileName,
                            Status = false, // Hata durumunda false döndürülüyor
                            Description = $"Yükleme hatası: {ex.Message}"
                        });
                    }
                }

                var totalUploaded = successfullyUploaded.Count;
                var totalFailed = failedUploads.Count;

                if (totalUploaded == 0)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "Hiçbir dosya yüklenemedi.",
                        totalUploaded,
                        totalFailed,
                        failedUploads
                    });
                }

                if (totalFailed == 0)
                {
                    return Ok(new
                    {
                        success = true,
                        message = "Tüm dosyalar başarıyla yüklendi.",
                        totalUploaded,
                        totalFailed,
                        successfullyUploaded
                    });
                }

                return Ok(new
                {
                    success = false,
                    message = "Bazı dosyalar yüklenemedi.",
                    totalUploaded,
                    totalFailed,
                    successfullyUploaded,
                    failedUploads
                });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = $"Dosyalar yüklenirken hata oluştu: {ex.Message}" });
            }
        }

        [HttpDelete("delete-file/{fileName}")]
        public IActionResult DeleteFile(string fileName)
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
