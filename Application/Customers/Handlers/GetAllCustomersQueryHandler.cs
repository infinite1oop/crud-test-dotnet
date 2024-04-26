using Application.Customers.Queries;
using Core.Interfaces;
using Core.Models.ViewModels;
using Mapster;
using MediatR;

namespace Application.Customers.Handlers
{
    public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, List<CustomerViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCustomersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CustomerViewModel>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.CustomerRepository.GetAll();
            if (result is not null)
            {
                return result.Adapt<List<CustomerViewModel>>();
            }
            return null;
        }
    }
}
