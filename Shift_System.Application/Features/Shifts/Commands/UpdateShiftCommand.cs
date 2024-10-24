﻿using AutoMapper;
using MediatR;
using Shift_System.Application.Features.Shifts.Events;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities.Tables;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Shifts.Commands
{
    public record UpdateShiftCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
        public string Shift_Name { get; set; }
    }


    internal class UpdateShiftCommandHandler : IRequestHandler<UpdateShiftCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateShiftCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<Guid>> Handle(UpdateShiftCommand command, CancellationToken cancellationToken)
        {
            var _shift = await _unitOfWork.Repository<ShiftList>().GetByIdAsync(command.Id);
            if (_shift != null)
            {
                _shift.Shift_Name = command.Shift_Name;
                _shift.UpdatedDate = DateTime.Now;

                await _unitOfWork.Repository<ShiftList>().UpdateAsync(_shift);
                _shift.AddDomainEvent(new ShiftUpdatedEvent(_shift));

                await _unitOfWork.Save(cancellationToken);

                return await Result<Guid>.SuccessAsync(_shift.Id, "Shift_Updated");
            }
            else
            {
                return await Result<Guid>.FailureAsync("Shift_Not_Found");
            }
        }
    }
}
