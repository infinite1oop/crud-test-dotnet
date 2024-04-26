using Core.Models.ViewModels;
using Core.Models.ViewModels.RequestModels;

namespace Mc2.CrudTest.Presentation.Client.Services.CustomerService
{
    public interface ICustomerService
    {
        List<CustomerViewModel> Customers { get; set; }
        Task GetCustomers();
        Task<CustomerViewModel> GetCustomerById(Guid Id);
        Task<(bool success, string errorMessage)> CreateCustomer(CreateCustomerRequestModel request);
        Task<(bool success, string errorMessage)> UpdateCustomer(UpdateCustomerRequestModel request);
        Task DeleteCustomer(Guid Id);
    }
}
