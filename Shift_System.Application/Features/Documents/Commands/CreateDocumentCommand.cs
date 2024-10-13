using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shift_System.Application.Common.Mappings;
using Shift_System.Application.Features.Documents.Events;
using Shift_System.Application.Interfaces;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Documents.Commands
{
    public record CreateDocumentCommand : IRequest<Result<Guid>>, IMapFrom<DocumentInfo>
    {
        public Guid? DataId { get; set; }
        public string? TableName { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public long? FileSize { get; set; }
        public string? FilePath { get; set; }
        public string? FileBase64Data { get; set; }
        public Guid? CreatedBy { get; set; }
        public List<IFormFile>? Files { get; set; } // Dosya listesi
    }

    internal class CreateDocumentCommandHandler : IRequestHandler<CreateDocumentCommand, Result<Guid>>
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
        public async Task<Result<Guid>> Handle(CreateDocumentCommand command, CancellationToken cancellationToken)
        {
            const long maxFileSize = 5 * 1024 * 1024; // 5 MB
            var baseDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            var uploadFolderPath = Path.Combine(baseDirectory, "Shift_System_UI", "wwwroot", "Uploads");

            if (command.Files == null || !command.Files.Any())
            {
                return await Result<Guid>.FailureAsync(Messages.File_Not_Found_TR);
            }

            foreach (var file in command.Files)
            {
                if (file.Length > maxFileSize)
                {
                    return await Result<Guid>.FailureAsync(Messages.File_Size_Exceeded_TR);
                }

                if (!FileConverter.IsValidFile(file))
                {
                    return await Result<Guid>.FailureAsync(Messages.Invalid_File_Type_TR);
                }

                // Dosya adı oluştur ve kaydet
                var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadFolderPath, uniqueFileName);

                try
                {
                    // Dosyayı kaydet
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Dosyanın başarıyla kaydedildiğini kontrol et
                    if (!File.Exists(filePath))
                    {
                        return await Result<Guid>.FailureAsync(Messages.File_Not_Save_TR);
                    }
                    Console.WriteLine($"Dosya başarıyla kaydedildi: {filePath}");

                    // Dosyayı oku ve Base64'e dönüştür
                    byte[] fileBytes;
                    string base64Data = null;
                    try
                    {
                        fileBytes = await File.ReadAllBytesAsync(filePath);
                        Console.WriteLine($"Base64'e dönüştürülmeden önce dosya boyutu: {fileBytes.Length}");

                        // Base64 dönüşüm
                        base64Data = Convert.ToBase64String(fileBytes);
                        Console.WriteLine($"Base64 veri uzunluğu: {base64Data.Length}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Dosya okuma hatası: {ex.Message}");
                        return await Result<Guid>.FailureAsync($"Dosya okuma hatası (jpg/png/etc): {ex.Message}");
                    }

                    if (base64Data == null)
                    {
                        return await Result<Guid>.FailureAsync("Base64 dönüşüm işlemi başarısız oldu.");
                    }

                    // Base64 verisi loglama
                    Console.WriteLine($"Veritabanına kaydedilmeden önce Base64 veri uzunluğu: {base64Data.Length}");

                    var _doc = new DocumentInfo
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
                        FileBase64Data = base64Data, // Base64 verisi burada set ediliyor
                        IsDeleted = false,
                        Status = true
                    };

                    // Veritabanına kaydet
                    await _unitOfWork.Repository<DocumentInfo>().AddAsync(_doc);
                    _doc.AddDomainEvent(new DocumentCreatedEvent(_doc));
                }
                catch (Exception ex)
                {
                    return await Result<Guid>.FailureAsync($"Dosya işlenirken hata oluştu: {ex.Message}");
                }
            }

            await _unitOfWork.Save(cancellationToken);
            return await Result<Guid>.SuccessAsync(Messages.File_Upload_Success_TR);
        }

    }

}