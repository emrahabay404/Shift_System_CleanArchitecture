using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Shift_System.Application.Extensions;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities.Tables;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Shifts.Queries
{
    public record GetShiftsWithPaginationQuery : IRequest<PaginatedResult<GetAllShiftsDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetShiftsWithPaginationQuery() { }

        public GetShiftsWithPaginationQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    internal class GetShiftsWithPaginationQueryHandler : IRequestHandler<GetShiftsWithPaginationQuery, PaginatedResult<GetAllShiftsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetShiftsWithPaginationQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<GetAllShiftsDto>> Handle(GetShiftsWithPaginationQuery query, CancellationToken cancellationToken)
        {
            var _shifts = await _unitOfWork.Repository<ShiftList>().Entities
        .OrderBy(x => x.Shift_Name)
        .ProjectTo<GetAllShiftsDto>(_mapper.ConfigurationProvider)
        .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);

            return PaginatedResult<GetAllShiftsDto>.SuccessAsync(
                _shifts.Data,
                _shifts.TotalCount,
                _shifts.CurrentPage,
                _shifts.PageSize,
                Messages.Teams_Listed_TR
            );
        }

    }
}
