using AutoMapper;
using MediatR;
using Shift_System.Application.Common.Mappings;
using Shift_System.Application.Features.Employees.Events;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities.Tables;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Employees.Commands
{

    public record CreateEmployeeCommand : IRequest<Result<Guid>>, IMapFrom<Employee>
    {
        public int EmployeeCode { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string UserName { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }
        public bool Activity { get; set; }
    }

    internal class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateEmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
        {
            var employee = new Employee()
            {
                EmployeeCode = command.EmployeeCode,
                Name = command.Name,
                SurName = command.SurName,
                Address = command.Address,
                Title = command.Title,
                Phone = command.Phone,
                Mail = command.Mail,
                UserName = command.UserName,
                CreatedDate = DateTime.Now,
            };

            await _unitOfWork.Repository<Employee>().AddAsync(employee);
            employee.AddDomainEvent(new EmployeeCreatedEvent(employee));

            await _unitOfWork.Save(cancellationToken);

            return await Result<Guid>.SuccessAsync(employee.Id, "Employee_Created");
        }
    }

}
