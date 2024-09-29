using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Interfaces;
using Shift_System.Shared.Helpers;

namespace Shift_System.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                return Ok(new { success = false, message = Messages.File_Upload_Failed_TR });
            }

            if (file.Length > maxFileSize)
            {
                return Ok(new { success = false, message = string.Format(Messages.File_Size_Exceeded_TR, maxFileSize / (1024 * 1024)) });
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

                return Ok(new { success = true, message = Messages.File_Upload_Success_TR, filePath });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = $"{Messages.File_Upload_Failed_TR}: {ex.Message}" });
            }
        }

        // Çoklu dosya yükleme işlemi
        [HttpPost("upload-multiple")]
        public async Task<IActionResult> UploadMultipleFiles(List<IFormFile> files)
        {
            const long maxFileSize = 5 * 1024 * 1024; // 5 MB

            var successfullyUploaded = new List<object>();
            var failedUploads = new List<object>();

            try
            {
                if (files == null || files.Count == 0)
                {
                    return Ok(new { success = false, message = Messages.File_Upload_Failed_TR });
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
                                Status = false,
                                Description = string.Format(Messages.File_Size_Exceeded_TR, maxFileSize / (1024 * 1024))
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
                            Status = true,
                            Description = Messages.File_Upload_Success_TR
                        });
                    }
                    catch (Exception ex)
                    {
                        failedUploads.Add(new
                        {
                            FileName = file.FileName,
                            Status = false,
                            Description = $"{Messages.File_Upload_Failed_TR}: {ex.Message}"
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
                        message = Messages.No_Files_Uploaded_TR,
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
                        message = Messages.All_Files_Uploaded_Success_TR,
                        totalUploaded,
                        totalFailed,
                        successfullyUploaded
                    });
                }

                return Ok(new
                {
                    success = false,
                    message = Messages.Some_Files_Failed_TR,
                    totalUploaded,
                    totalFailed,
                    successfullyUploaded,
                    failedUploads
                });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = $"{Messages.File_Upload_Failed_TR}: {ex.Message}" });
            }
        }

        [HttpDelete("delete-file/{fileName}")]
        public IActionResult DeleteFile(string fileName)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            var filesInDirectory = Directory.GetFiles(folderPath);
            string fullFilePath = null;

            foreach (var file in filesInDirectory)
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                if (fileNameWithoutExtension.Equals(fileName, StringComparison.OrdinalIgnoreCase))
                {
                    fullFilePath = file;
                    break;
                }
            }

            if (fullFilePath == null)
            {
                return NotFound(new { success = false, message = $"{Messages.File_Not_Found_TR}: {fileName}" });
            }

            if (System.IO.File.Exists(fullFilePath))
            {
                System.IO.File.Delete(fullFilePath);
                return Ok(new { success = true, message = Messages.File_Deleted_Success_TR });
            }

            return NotFound(new { success = false, message = $"{Messages.File_Not_Found_TR}: {fileName}" });
        }
    }
}
