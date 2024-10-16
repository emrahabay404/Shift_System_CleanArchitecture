﻿using AutoMapper;
using MediatR;
using Shift_System.Application.Features.Teams.Events;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities.Tables;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Teams.Commands
{
    public record UpdateTeamsCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
        public string TeamName { get; set; }
    }
    internal class UpdateTeamsCommandHandler : IRequestHandler<UpdateTeamsCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateTeamsCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<Guid>> Handle(UpdateTeamsCommand command, CancellationToken cancellationToken)
        {
            var _team = await _unitOfWork.Repository<Team>().GetByIdAsync(command.Id);
            if (_team != null)
            {
                _team.TeamName = command.TeamName;
                _team.UpdatedDate = DateTime.Now;

                await _unitOfWork.Repository<Team>().UpdateAsync(_team);
                _team.AddDomainEvent(new TeamUpdatedEvent(_team));

                await _unitOfWork.Save(cancellationToken);

                return await Result<Guid>.SuccessAsync(_team.Id, "Team_Updated");
            }
            else
            {
                return await Result<Guid>.FailureAsync("Team_Not_Found");
            }
        }
    }
}