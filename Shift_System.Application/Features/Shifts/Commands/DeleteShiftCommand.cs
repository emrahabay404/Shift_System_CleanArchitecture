using AutoMapper;
using MediatR;
using Shift_System.Application.Common.Mappings;
using Shift_System.Application.Features.Shifts.Events;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Shifts.Commands
{
   public record DeleteShiftCommand : IRequest<Result<Guid>>, IMapFrom<ShiftList>
    {
        public Guid Id { get; set; }
        public DeleteShiftCommand()
        {
        }
        public DeleteShiftCommand(Guid id)
        {
            Id = id;
        }
    }
    internal class DeleteShiftCommandHandler : IRequestHandler<DeleteShiftCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteShiftCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<Guid>> Handle(DeleteShiftCommand command, CancellationToken cancellationToken)
        {
            var _shiftList = await _unitOfWork.Repository<ShiftList>().GetByIdAsync(command.Id);
            if (_shiftList != null)
            {
                await _unitOfWork.Repository<ShiftList>().DeleteAsync(_shiftList);
                _shiftList.AddDomainEvent(new ShiftListDeletedEvent(_shiftList));

                await _unitOfWork.Save(cancellationToken);

                return await Result<Guid>.SuccessAsync(_shiftList.Id, "ShiftList_Deleted");
            }
            else
            {
                return await Result<Guid>.FailureAsync("ShiftList_Not_Found");
            }
        }
    }
}

