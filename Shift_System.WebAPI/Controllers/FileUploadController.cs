using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Interfaces;

namespace Shift_System.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            // Maksimum izin verilen dosya boyutu (örneğin, 5 MB)
            const long maxFileSize = 5 * 1024 * 1024; // 5 MB

            if (file == null || file.Length == 0)
            {
                return BadRequest("Dosya seçilmedi.");
            }

            // Dosya boyutu kontrolü
            if (file.Length > maxFileSize)
            {
                return BadRequest($"Dosya boyutu {maxFileSize / (1024 * 1024)} MB'ı aşıyor.");
            }

            try
            {
                // API projesindeki dosya yolunu alıyoruz
                var currentDirectory = Directory.GetCurrentDirectory(); // API'nin bulunduğu dizin
                var folderPathAfterWwwroot = "Uploads"; // wwwroot sonrasındaki dizin

                // Dosya kaydedilecek dizini UI projesindeki wwwroot yoluna çeviriyoruz
                var uiProjectDirectory = currentDirectory.Replace(
                    "Shift_System.WebAPI",
                    $@"Shift_System_UI\wwwroot\{folderPathAfterWwwroot}\"
                );

                // Eğer dizin mevcut değilse oluştur
                if (!Directory.Exists(uiProjectDirectory))
                {
                    Directory.CreateDirectory(uiProjectDirectory);
                }

                // Dosyanın kaydedileceği tam yol
                var filePath = Path.Combine(uiProjectDirectory, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok(new { success = true, message = "Dosya UI projesine yüklendi.", filePath });
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
            // Maksimum izin verilen dosya boyutu (örneğin, 5 MB)
            const long maxFileSize = 5 * 1024 * 1024; // 5 MB

            try
            {
                if (files == null || files.Count == 0)
                    return BadRequest("Dosya seçilmedi.");

                // Boyut sınırını aşan dosyalar için bir liste oluştur
                var oversizedFiles = new List<string>();

                // Dosya boyutu kontrolleri
                foreach (var file in files)
                {
                    if (file.Length > maxFileSize)
                    {
                        // Eğer dosya boyutu sınırı aşarsa listeye ekle
                        oversizedFiles.Add(file.FileName);
                    }
                }

                // Eğer boyut sınırını aşan dosya/dosyalar varsa hata mesajı döndür
                if (oversizedFiles.Any())
                {
                    return BadRequest($"Aşağıdaki dosyalar {maxFileSize / (1024 * 1024)} MB sınırını aşıyor: {string.Join(", ", oversizedFiles)}");
                }

                // API projesindeki dosya yolunu alıyoruz
                var currentDirectory = Directory.GetCurrentDirectory(); // Temsa_Api'nin bulunduğu dizin
                var folderPathAfterWwwroot = "Uploads"; // wwwroot sonrasındaki dizin

                // Dosya kaydedilecek dizini UI projesindeki wwwroot yoluna çeviriyoruz
                var uiProjectDirectory = currentDirectory.Replace(
                    "Shift_System.WebAPI",
                    $@"Shift_System_UI\wwwroot\{folderPathAfterWwwroot}\"
                );

                // Eğer dizin mevcut değilse oluştur
                if (!Directory.Exists(uiProjectDirectory))
                {
                    Directory.CreateDirectory(uiProjectDirectory);
                }

                var filePaths = new List<string>();

                foreach (var file in files)
                {
                    var filePath = Path.Combine(uiProjectDirectory, Path.GetFileName(file.FileName));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        filePaths.Add(filePath);
                    }
                }

                if (filePaths.Count == 0)
                    return BadRequest("Dosyalar yüklenemedi.");

                return Ok(new { success = true, message = "Dosyalar UI projesine yüklendi.", filePaths });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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

        //[HttpPost("upload-single")]
        //public async Task<IActionResult> UploadSingleFile(IFormFile file)
        //{
        //    try
        //    {
        //        if (file == null || file.Length == 0)
        //            return BadRequest("Dosya seçilmedi.");

        //        // Dosya yükleme servisini kullanıyoruz
        //        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        //        var filePath = await _fileUploadService.UploadFileAsync(file, folderPath);

        //        if (string.IsNullOrEmpty(filePath))
        //            return BadRequest("Dosya yüklenemedi.");

        //        return Ok(new { success = true, message = "Dosya yüklendi.", filePath });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
