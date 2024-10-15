using AutoMapper;
using MediatR;
using Shift_System.Application.Common.Mappings;
using Shift_System.Application.Features.Teams.Events;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities.Tables;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Teams.Commands
{
    public record DeleteTeamsCommand : IRequest<Result<Guid>>, IMapFrom<Team>
    {
        public Guid Id { get; set; }
        public DeleteTeamsCommand()
        {
        }
        public DeleteTeamsCommand(Guid id)
        {
            Id = id;
        }
    }
    internal class DeleteTeamsCommandHandler : IRequestHandler<DeleteTeamsCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteTeamsCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<Guid>> Handle(DeleteTeamsCommand command, CancellationToken cancellationToken)
        {
            var _Team = await _unitOfWork.Repository<Team>().GetByIdAsync(command.Id);
            if (_Team != null)
            {
                await _unitOfWork.Repository<Team>().DeleteAsync(_Team);
                _Team.AddDomainEvent(new TeamsDeletedEvent(_Team));

                await _unitOfWork.Save(cancellationToken);

                return await Result<Guid>.SuccessAsync(_Team.Id, "Team_Deleted");
            }
            else
            {
                return await Result<Guid>.FailureAsync("Team_Not_Found");
            }
        }
    }
}
