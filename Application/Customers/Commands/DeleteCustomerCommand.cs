using MediatR;

namespace Application.Customers.Commands
{
    public class DeleteCustomerCommand : IRequest<(bool, string)>
    {
        public Guid Id { get; set; }
    }
}