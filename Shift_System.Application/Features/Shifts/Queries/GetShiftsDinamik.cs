using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shift_System.Application.Extensions;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities.Tables;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Shifts.Queries
{
    // GetShiftsDinamik sorgusu
    public record GetShiftsDinamik(DynamicQuery Query) : IRequest<PaginatedResult<GetAllShiftsDto>>;

    internal class GetShiftsDinamikHandler : IRequestHandler<GetShiftsDinamik, PaginatedResult<GetAllShiftsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetShiftsDinamikHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<GetAllShiftsDto>> Handle(GetShiftsDinamik request, CancellationToken cancellationToken)
        {
            var query = BuildFilteredQuery(request);
            var paginatedList = await ApplyPagingAndProjection(query, request.Query, cancellationToken);

            return CreatePaginatedResult(paginatedList);
        }

        // Filtreli ve sıralı sorgu oluştur
        private IQueryable<ShiftList> BuildFilteredQuery(GetShiftsDinamik request)
        {
            var query = _unitOfWork.Repository<ShiftList>().Entities.AsQueryable();

            if (request.Query.Filter?.Any() == true)
                query = QueryFilterHelper.ApplyFilter(query, request.Query.Filter);

            if (request.Query.Sort?.Any() == true)
                query = request.Query.Sort.Aggregate(query, (current, sort) =>
                    sort.Dir.ToLower() == "desc"
                        ? current.OrderByDescending(e => EF.Property<object>(e, sort.Field))
                        : current.OrderBy(e => EF.Property<object>(e, sort.Field)));

            return query;
        }

        // Sayfalama ve dönüşüm işlemi
        private async Task<PaginatedResult<GetAllShiftsDto>> ApplyPagingAndProjection(
            IQueryable<ShiftList> query, DynamicQuery dynamicQuery, CancellationToken cancellationToken)
        {
            return await query
                .ProjectTo<GetAllShiftsDto>(_mapper.ConfigurationProvider)
                .ToPaginatedListAsync(dynamicQuery.PageIndex, dynamicQuery.PageSize, cancellationToken);
        }

        // PaginatedResult oluştur
        private PaginatedResult<GetAllShiftsDto> CreatePaginatedResult(PaginatedResult<GetAllShiftsDto> paginatedList)
        {
            return PaginatedResult<GetAllShiftsDto>.SuccessAsync(
                paginatedList.Data,
                paginatedList.TotalCount,
                paginatedList.CurrentPage,
                paginatedList.PageSize,
                Messages.Teams_Listed_TR
            );
        }
    }
}
