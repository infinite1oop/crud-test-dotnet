using Application.Customers.Commands;
using Common.Helpers;
using Core.Interfaces;
using Core.Models;
using MediatR;

namespace Application.Customers.Handlers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, (Guid, string)>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCustomerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(Guid, string)> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var existingCustomerWithEmail = await _unitOfWork.CustomerRepository.FindByEmail(request.Email);
            if (existingCustomerWithEmail != null)
            {
                return (Guid.Empty, "Email already exists.");
            }

            var existingCustomerWithDetails = await _unitOfWork.CustomerRepository.FirstOrDefaultAsync(c => c.FirstName.ToLower() == request.FirstName.ToLower() &&
                                                                                               c.LastName.ToLower() == request.LastName.ToLower() &&
                                                                                               c.DateOfBirth == request.DateOfBirth);
            if (existingCustomerWithDetails != null)
            {
                return (Guid.Empty, "Customer with the same details already exists.");
            }

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                PhoneNumber = PhoneNumberHelper.ConvertPhoneNumberToULong(request.PhoneNumber),
                Email = request.Email,
                BankAccountNumber = request.BankAccountNumber
            };

            _ = _unitOfWork.CustomerRepository.Add(customer);

            return (customer.Id, "");
        }
    }
}
