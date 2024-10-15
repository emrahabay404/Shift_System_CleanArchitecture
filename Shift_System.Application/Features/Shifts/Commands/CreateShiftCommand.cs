using AutoMapper;
using MediatR;
using Shift_System.Application.Common.Mappings;
using Shift_System.Application.Features.Shifts.Events;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities.Tables;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Shifts.Commands
{
    public record CreateShiftCommand : IRequest<Result<Guid>>, IMapFrom<ShiftList>
   {
      public string Shift_Name { get; set; }
   }

   internal class CreateShiftCommandHandler : IRequestHandler<CreateShiftCommand, Result<Guid>>
   {
      private readonly IUnitOfWork _unitOfWork;
      private readonly IMapper _mapper;

      public CreateShiftCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
      {
         _unitOfWork = unitOfWork;
         _mapper = mapper;
      }

      public async Task<Result<Guid>> Handle(CreateShiftCommand command, CancellationToken cancellationToken)
      {
         var _shift = new ShiftList()
         {
            Shift_Name = command.Shift_Name,
            CreatedDate = DateTime.Now,
         };

         await _unitOfWork.Repository<ShiftList>().AddAsync(_shift);
         _shift.AddDomainEvent(new ShiftCreatedEvent(_shift));

         await _unitOfWork.Save(cancellationToken);

         return await Result<Guid>.SuccessAsync(_shift.Id, "Shift_Created");
      }
   }
}
