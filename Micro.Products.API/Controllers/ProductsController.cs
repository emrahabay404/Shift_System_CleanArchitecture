using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Products.Queries;
using Shift_System.Shared;

namespace Micro.Products.API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class ProductsController : ControllerBase
   {
      private readonly IMediator _mediator;

      public ProductsController(IMediator mediator)
      {
         _mediator = mediator;
      }

      [HttpGet]
      public async Task<ActionResult<Result<List<GetAllProductsDto>>>> Get()
      {
         return await _mediator.Send(new GetAllProductsQuery());
      }

   }
}
