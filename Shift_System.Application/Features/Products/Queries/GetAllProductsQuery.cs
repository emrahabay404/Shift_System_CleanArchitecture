using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Products.Queries
{
   public record GetAllProductsQuery : IRequest<Result<List<GetAllProductsDto>>>;

   internal class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, Result<List<GetAllProductsDto>>>
   {
      private readonly IUnitOfWork _unitOfWork;
      private readonly IMapper _mapper;

      public GetAllProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
      {
         _unitOfWork = unitOfWork;
         _mapper = mapper;
      }

      public async Task<Result<List<GetAllProductsDto>>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
      {
         var _Products = await _unitOfWork.Repository<Product>().Entities
                .ProjectTo<GetAllProductsDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

         return await Result<List<GetAllProductsDto>>.SuccessAsync(_Products, "Products_Listed");
      }

   }
}
