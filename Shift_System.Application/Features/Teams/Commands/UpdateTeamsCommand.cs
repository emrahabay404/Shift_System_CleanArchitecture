using AutoMapper;
using MediatR;
using Shift_System.Application.Features.Teams.Events;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities;
using Shift_System.Shared;

namespace Shift_System.Application.Features.Teams.Commands
{
   public record UpdateTeamsCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
    }
    internal class UpdateTeamsCommandHandler : IRequestHandler<UpdateTeamsCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateTeamsCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(UpdateTeamsCommand command, CancellationToken cancellationToken)
        {
            var _team = await _unitOfWork.Repository<Team>().GetByIdAsync(command.Id);
            if (_team != null)
            {
                _team.TeamName = command.TeamName;
                _team.UpdatedDate = DateTime.Now;

                await _unitOfWork.Repository<Team>().UpdateAsync(_team);
                _team.AddDomainEvent(new TeamUpdatedEvent(_team));

                await _unitOfWork.Save(cancellationToken);

                return await Result<int>.SuccessAsync(_team.Id, "Team Updated.");
            }
            else
            {
                return await Result<int>.FailureAsync("Team Not Found.");
            }
        }
    }
}