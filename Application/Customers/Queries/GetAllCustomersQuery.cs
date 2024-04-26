using Core.Models.ViewModels;
using MediatR;

namespace Application.Customers.Queries
{
    public class GetAllCustomersQuery : IRequest<List<CustomerViewModel>>
    {
    }
}