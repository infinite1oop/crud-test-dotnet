using Application.Customers.Queries;
using Core.Interfaces;
using Core.Models.ViewModels;
using Mapster;
using MediatR;

namespace Application.Customers.Handlers
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCustomerByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerViewModel> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.CustomerRepository.FindById(request.Id);
            if (result is not null)
            {
                var response = result.Adapt<CustomerViewModel>();
                response.PhoneNumber = "+" + response.PhoneNumber;
                return response;
            }
            return null;
        }
    }
}
