using AutoMapper;
using MediatR;
using Shift_System.Application.Common.Mappings;
using Shift_System.Application.Features.Teams.Events;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities.Tables;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Teams.Commands
{
    public record CreateTeamCommand : IRequest<Result<Guid>>, IMapFrom<Team>
    {
        public string TeamName { get; set; } = string.Empty;
        public string? FileName { get; set; }
    }

    internal class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateTeamCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(CreateTeamCommand command, CancellationToken cancellationToken)
        {
            var _team = new Team()
            {
                Id = new Guid(),
                TeamName = command.TeamName,
                FileName = command.FileName, // Dosya adı burada kaydediliyor
                CreatedDate = DateTime.Now,
            };
            await _unitOfWork.Repository<Team>().AddAsync(_team);
            _team.AddDomainEvent(new TeamCreatedEvent(_team));
            await _unitOfWork.Save(cancellationToken);
            return await Result<Guid>.SuccessAsync(_team.Id, "Team_Created");
        }

    }
}

