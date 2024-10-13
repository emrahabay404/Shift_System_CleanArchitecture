using AutoMapper;
using MediatR;
using Shift_System.Application.Common.Mappings;
using Shift_System.Application.Features.Documents.Events;
using Shift_System.Application.Interfaces;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Documents.Commands
{
    public record class DeleteDocumentsCommand : IRequest<Result<Guid>>, IMapFrom<DocumentInfo>
    {
        public Guid Id { get; set; }
        public DeleteDocumentsCommand()
        {
        }
        public DeleteDocumentsCommand(Guid id)
        {
            Id = id;
        }
    }

    internal class DeleteDocumentsCommandHandler : IRequestHandler<DeleteDocumentsCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileUploadService _fileUploadService; // FileUploadService'i ekliyoruz

        public DeleteDocumentsCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileUploadService fileUploadService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileUploadService = fileUploadService; // Inject edilen servisi kullanacağız
        }

        public async Task<Result<Guid>> Handle(DeleteDocumentsCommand command, CancellationToken cancellationToken)
        {
            // Veritabanındaki belgeyi getir
            var _document = await _unitOfWork.Repository<DocumentInfo>().GetByIdAsync(command.Id);
            if (_document != null)
            {
                // Dosya yolunu kontrol et ve fiziksel dosyayı sil
                var folderPath = Path.GetDirectoryName(_document.FilePath); // Klasör yolu
                var fileName = Path.GetFileName(_document.FilePath); // Dosya adı

                // FileUploadService kullanarak fiziksel dosyayı sil
                var fileDeleted = _fileUploadService.DeleteFile(folderPath, fileName);
                if (!fileDeleted)
                {
                    // Fiziksel dosya silinemedi ama veritabanındaki kaydın silinmiş olduğunu işaretleyeceğiz
                    _document.IsDeleted = true;
                    await _unitOfWork.Repository<DocumentInfo>().UpdateAsync(_document);
                    await _unitOfWork.Save(cancellationToken);

                    return await Result<Guid>.FailureAsync(Messages.File_Not_Found_TR);
                }

                // Veritabanındaki kaydı silme yerine "IsDeleted" olarak işaretle
                _document.IsDeleted = true;
                await _unitOfWork.Repository<DocumentInfo>().UpdateAsync(_document);

                _document.AddDomainEvent(new DocumentDeletedEvent(_document));

                // Veritabanında değişiklikleri kaydet
                await _unitOfWork.Save(cancellationToken);

                return await Result<Guid>.SuccessAsync(_document.Id, Messages.File_Deleted_Success_TR);
            }
            else
            {
                return await Result<Guid>.FailureAsync(Messages.File_Not_Found_TR);
            }
        }
    }
}