using Core.Models.ViewModels;
using MediatR;

namespace Application.Customers.Queries
{
    public class GetCustomerByIdQuery : IRequest<CustomerViewModel>
    {
        public Guid Id { get; set; }
    }
}