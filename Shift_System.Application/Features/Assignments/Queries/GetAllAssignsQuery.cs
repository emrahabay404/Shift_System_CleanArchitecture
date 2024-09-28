using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Assignments.Queries
{

    public record GetAllAssignsQuery : IRequest<Result<List<GetAllAssignsDto>>>;

    internal class GetAllAssignsQueryHandler : IRequestHandler<GetAllAssignsQuery, Result<List<GetAllAssignsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllAssignsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllAssignsDto>>> Handle(GetAllAssignsQuery query, CancellationToken cancellationToken)
        {
            var _Assigns = await _unitOfWork.Repository<AssignList>().Entities
                   .ProjectTo<GetAllAssignsDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);

            return await Result<List<GetAllAssignsDto>>.SuccessAsync(_Assigns, "AssignList_Listed");
      }
    }
}
