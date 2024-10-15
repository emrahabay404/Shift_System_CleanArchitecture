using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities.Tables;
using Shift_System.Shared.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Shift_System.Application.Features.Assignments.Queries
{
    // Query kaydını tanımlıyoruz
    public record GetAllAssignsQueryDynamic(DynamicQuery Query) : IRequest<Result<List<GetAllAssignsDto>>>;

    internal class GetAllAssignsQueryDynamicHandler : IRequestHandler<GetAllAssignsQueryDynamic, Result<List<GetAllAssignsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllAssignsQueryDynamicHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Handle metodu, dinamik sorguyu işler ve sonucu döner
        public async Task<Result<List<GetAllAssignsDto>>> Handle(GetAllAssignsQueryDynamic request, CancellationToken cancellationToken)
        {
            // Temel sorguyu alıyoruz
            var query = _unitOfWork.Repository<AssignList>().Entities.AsQueryable();

            // Dinamik filtre uygulaması
            if (request.Query.Filter != null)
            {
                query = ApplyFilter(query, request.Query.Filter);
            }

            // Dinamik sıralama uygulaması
            if (request.Query.Sort != null && request.Query.Sort.Any())
            {
                query = ApplySorting(query, request.Query.Sort);
            }

            // Pagination işlemi
            var skip = (request.Query.Page - 1) * request.Query.PageSize;
            var take = request.Query.PageSize;

            // Sonuçlara pagination uyguluyoruz
            var paginatedResult = await query
                .Skip(skip)    // Kaç kayıt atlanacak
                .Take(take)    // Kaç kayıt alınacak
                .ProjectTo<GetAllAssignsDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return await Result<List<GetAllAssignsDto>>.SuccessAsync(paginatedResult, "AssignList_Listed");
        }

        // Filtreleme işlemi
        private IQueryable<AssignList> ApplyFilter(IQueryable<AssignList> query, Filter filter)
        {
            if (filter == null || string.IsNullOrEmpty(filter.Field) || string.IsNullOrEmpty(filter.Operator))
                return query;

            // Operator'e göre farklı filtreleme işlemleri yapılabilir
            if (filter.Operator == "eq")
            {
                query = query.Where(a => EF.Property<string>(a, filter.Field) == filter.Value);
            }

            // Alt filtrelerin işlenmesi
            if (filter.Filters != null && filter.Filters.Any())
            {
                foreach (var subFilter in filter.Filters)
                {
                    query = ApplyFilter(query, subFilter);
                }
            }

            return query;
        }

        // Sıralama işlemi
        private IQueryable<AssignList> ApplySorting(IQueryable<AssignList> query, IEnumerable<Sort> sorts)
        {
            bool isFirstSort = true;

            foreach (var sort in sorts)
            {
                if (string.IsNullOrEmpty(sort.Field) || string.IsNullOrEmpty(sort.Dir))
                    continue;

                if (sort.Dir == "asc")
                {
                    query = isFirstSort ? query.OrderBy(a => EF.Property<object>(a, sort.Field)) :
                                          ((IOrderedQueryable<AssignList>)query).ThenBy(a => EF.Property<object>(a, sort.Field));
                }
                else if (sort.Dir == "desc")
                {
                    query = isFirstSort ? query.OrderByDescending(a => EF.Property<object>(a, sort.Field)) :
                                          ((IOrderedQueryable<AssignList>)query).ThenByDescending(a => EF.Property<object>(a, sort.Field));
                }

                isFirstSort = false;
            }

            return query;
        }
    }
}
