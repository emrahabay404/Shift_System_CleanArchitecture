using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities.Tables;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Teams.Queries
{
    public record GetAllTeamsQuery : IRequest<Result<List<GetAllTeamsDto>>>;

    internal class GetAllTeamsQueryHandler : IRequestHandler<GetAllTeamsQuery, Result<List<GetAllTeamsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllTeamsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllTeamsDto>>> Handle(GetAllTeamsQuery query, CancellationToken cancellationToken)
        {
            var _teams = await _unitOfWork.Repository<Team>().Entities
                   .ProjectTo<GetAllTeamsDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);

            //return await Result<List<GetAllTeamsDto>>.SuccessAsync(_teams, "Teams_Listed");
            return await Result<List<GetAllTeamsDto>>.SuccessAsync(_teams, Messages.Teams_Listed_TR);
        }

    }
}