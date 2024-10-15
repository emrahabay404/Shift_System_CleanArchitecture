using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shift_System.Application.Common.Mappings;
using Shift_System.Application.Features.Documents.Events;
using Shift_System.Application.Interfaces;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities.Tables;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Documents.Commands
{
    public record CreateDocumentCommand : IRequest<Result<List<FileUploadResult>>>, IMapFrom<DocumentInfo>
    {
        public Guid? DataId { get; set; }           // İlgili veri ID'si
        public string? TableName { get; set; }       // Dosyanın hangi tabloya ait olduğunu belirten alan
        public List<IFormFile>? Files { get; set; }  // Dosya listesi
        public string? FileName { get; set; }        // Dosyanın adı
        public string? FileType { get; set; }        // Dosyanın türü (örneğin, pdf, jpg)
        public long? FileSize { get; set; }          // Dosyanın boyutu (byte cinsinden)
        public string? FilePath { get; set; }        // Dosyanın sunucuda kaydedileceği yol
        public string? FileBase64Data { get; set; }  // Dosyanın Base64 formatında veri olarak tutulması
        public Guid? CreatedBy { get; set; }         // Dosyayı oluşturan/kaydeden kullanıcının ID'si
        public DateTime? CreatedDate { get; set; }   // Dosyanın oluşturulduğu tarih
        public bool? IsDeleted { get; set; }         // Silinmiş olup olmadığını kontrol eden alan
        public bool? Status { get; set; }            // Dosyanın durumunu gösteren alan (aktif/pasif)
    }

    internal class CreateDocumentCommandHandler : IRequestHandler<CreateDocumentCommand, Result<List<FileUploadResult>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileUploadService _fileUploadService;

        public CreateDocumentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileUploadService fileUploadService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileUploadService = fileUploadService;
        }

        public async Task<Result<List<FileUploadResult>>> Handle(CreateDocumentCommand command, CancellationToken cancellationToken)
        {
            var uploadResults = new List<FileUploadResult>();
            var baseDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            var uploadFolderPath = Path.Combine(baseDirectory, "Shift_System_UI", "wwwroot", "Uploads");

            if (command.Files == null || !command.Files.Any())
            {
                return await Result<List<FileUploadResult>>.FailureAsync("Dosya bulunamadı.");
            }

            foreach (var file in command.Files)
            {
                var fileResult = new FileUploadResult { FileName = file.FileName };

                // Geçerli dosya kontrolü
                if (!FileConverter.IsValidFile(file))
                {
                    fileResult.Message = "Geçersiz dosya türü veya boyutu.";
                    uploadResults.Add(fileResult);
                    continue;
                }

                // Dosya yolu oluşturma
                var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadFolderPath, uniqueFileName);
                fileResult.FilePath = filePath;

                try
                {
                    // Dosyayı kaydetme
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream, cancellationToken);
                    }

                    // Dosyanın başarıyla kaydedildiğini kontrol et
                    if (!File.Exists(filePath))
                    {
                        fileResult.Message = "Dosya kaydedilemedi.";
                        uploadResults.Add(fileResult);
                        continue;
                    }

                    // Dosyanın Base64'e dönüştürülmesi
                    string base64Data;
                    try
                    {
                        base64Data = FileConverter.ConvertFileToBase64(filePath);
                    }
                    catch (Exception ex)
                    {
                        fileResult.Message = $"Dosya okuma hatası: {ex.Message}";
                        uploadResults.Add(fileResult);
                        continue;
                    }

                    // Veritabanına kaydetme
                    var document = new DocumentInfo
                    {
                        Id = Guid.NewGuid(),
                        DataId = command.DataId ?? Guid.NewGuid(),
                        TableName = command.TableName ?? "",
                        CreatedDate = DateTime.Now,
                        CreatedBy = command.CreatedBy,
                        FileName = uniqueFileName,
                        FileType = FileConverter.GetFileType(file.FileName),
                        FileSize = file.Length,
                        FilePath = filePath,
                        FileBase64Data = base64Data,
                        IsDeleted = false,
                        Status = true
                    };

                    await _unitOfWork.Repository<DocumentInfo>().AddAsync(document);
                    document.AddDomainEvent(new DocumentCreatedEvent(document));

                    fileResult.Message = "Dosya başarıyla yüklendi.";
                }
                catch (Exception ex)
                {
                    fileResult.Message = $"Dosya işlenirken hata oluştu: {ex.Message}";
                }

                uploadResults.Add(fileResult);
            }

            await _unitOfWork.Save(cancellationToken);

            return Result<List<FileUploadResult>>.Success(uploadResults, "Dosya yükleme işlemi tamamlandı.");
        }
    }

    public class FileUploadResult
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Message { get; set; }
    }
}