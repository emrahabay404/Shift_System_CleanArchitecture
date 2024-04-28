using AutoMapper;
using MediatR;
using Shift_System.Application.Common.Mappings;
using Shift_System.Application.Features.Shifts.Events;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities;
using Shift_System.Shared;

namespace Shift_System.Application.Features.Shifts.Commands
{
   public record DeleteShiftCommand : IRequest<Result<int>>, IMapFrom<ShiftList>
    {
        public int Id { get; set; }
        public DeleteShiftCommand()
        {
        }
        public DeleteShiftCommand(int id)
        {
            Id = id;
        }
    }
    internal class DeleteShiftCommandHandler : IRequestHandler<DeleteShiftCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteShiftCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(DeleteShiftCommand command, CancellationToken cancellationToken)
        {
            var _shiftList = await _unitOfWork.Repository<ShiftList>().GetByIdAsync(command.Id);
            if (_shiftList != null)
            {
                await _unitOfWork.Repository<ShiftList>().DeleteAsync(_shiftList);
                _shiftList.AddDomainEvent(new ShiftListDeletedEvent(_shiftList));

                await _unitOfWork.Save(cancellationToken);

                return await Result<int>.SuccessAsync(_shiftList.Id, "ShiftList_Deleted");
            }
            else
            {
                return await Result<int>.FailureAsync("ShiftList_Not_Found");
            }
        }
    }
}

