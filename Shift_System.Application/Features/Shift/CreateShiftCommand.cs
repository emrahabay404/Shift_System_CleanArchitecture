using AutoMapper;
using MediatR;
using Shift_System.Application.Common.Mappings;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities;
using Shift_System.Shared;

namespace Shift_System.Application.Features.Shift
{
   public record CreateShiftCommand : IRequest<Result<int>>, IMapFrom<ShiftList>
   {
      public string Shift_Name { get; set; }
   }

   internal class CreateShiftCommandHandler : IRequestHandler<CreateShiftCommand, Result<int>>
   {
      private readonly IUnitOfWork _unitOfWork;
      private readonly IMapper _mapper;

      public CreateShiftCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
      {
         _unitOfWork = unitOfWork;
         _mapper = mapper;
      }

      public async Task<Result<int>> Handle(CreateShiftCommand command, CancellationToken cancellationToken)
      {
         var _shift = new ShiftList()
         {
            Shift_Name = command.Shift_Name,
              CreatedDate = DateTime.Now,
         };

         await _unitOfWork.Repository<ShiftList>().AddAsync(_shift);
         _shift.AddDomainEvent(new ShiftCreatedEvent(_shift));

         await _unitOfWork.Save(cancellationToken);

         return await Result<int>.SuccessAsync(_shift.Id, "Shift Created.");
      }
   }
}
