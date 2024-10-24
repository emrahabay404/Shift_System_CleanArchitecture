﻿using AutoMapper;
using MediatR;
using Shift_System.Application.Features.Employees.Events;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities.Tables;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Employees.Commands
{
    public record UpdateEmployeeCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
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

    internal class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateEmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<Guid>> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
        {
            var _Employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(command.Id);
            if (_Employee != null)
            {
                _Employee.EmployeeCode = command.EmployeeCode;
                _Employee.Name = command.Name;
                _Employee.SurName = command.SurName;
                _Employee.UserName = command.UserName;
                _Employee.Mail = command.Mail;
                _Employee.Phone = command.Phone;
                _Employee.Address = command.Address;
                _Employee.Title = command.Title;
                _Employee.Activity = command.Activity;

                _Employee.UpdatedDate = DateTime.Now;

                await _unitOfWork.Repository<Employee>().UpdateAsync(_Employee);
                _Employee.AddDomainEvent(new EmployeeUpdatedEvent(_Employee));

                await _unitOfWork.Save(cancellationToken);

                return await Result<Guid>.SuccessAsync(_Employee.Id, "Employee_Updated");
            }
            else
            {
                return await Result<Guid>.FailureAsync("Employee_Not_Found");
            }
        }
    }
}
