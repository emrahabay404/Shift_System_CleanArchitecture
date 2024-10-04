using AutoMapper;
using MediatR;
using Shift_System.Application.Common.Mappings;
using Shift_System.Application.Features.Employees.Events;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Employees.Commands
{
    public record DeleteEmployeeCommand : IRequest<Result<Guid>>, IMapFrom<Employee>
    {
        public Guid Id { get; set; }
        public DeleteEmployeeCommand()
        {
        }
        public DeleteEmployeeCommand(Guid id)
        {
            Id = id;
        }
    }

    internal class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteEmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<Guid>> Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
        {
            var _Employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(command.Id);
            if (_Employee != null)
            {
                await _unitOfWork.Repository<Employee>().DeleteAsync(_Employee);
                _Employee.AddDomainEvent(new EmployeeDeletedEvent(_Employee));

                await _unitOfWork.Save(cancellationToken);

                return await Result<Guid>.SuccessAsync(_Employee.Id, "Employee_Deleted");
            }
            else
            {
                return await Result<Guid>.FailureAsync("Employee_Not_Found");
            }
        }
    }
}

