using Core.Models.ViewModels;
using Core.Models.ViewModels.RequestModels;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Mc2.CrudTest.Presentation.Client.Services.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;

        public CustomerService(HttpClient httpClient, NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
        }
        public List<CustomerViewModel> Customers { get; set; } = new List<CustomerViewModel>();

        public async Task<(bool success, string errorMessage)> CreateCustomer(CreateCustomerRequestModel request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/customer/Create", request);
            if (response.IsSuccessStatusCode)
            {
                _navigationManager.NavigateTo("customers");
                return (true, null);
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return (false, errorMessage);
            }
        }

        public async Task DeleteCustomer(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/customer/DeleteById/{id}");
            _navigationManager.NavigateTo("customers");
        }

        public async Task<CustomerViewModel> GetCustomerById(Guid id)
        {
            var result = await _httpClient.GetAsync($"api/customer/GetById/{id}");
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await result.Content.ReadFromJsonAsync<CustomerViewModel>();
            }
            return null;
        }

        public async Task GetCustomers()
        {
            var result = await _httpClient.GetFromJsonAsync<List<CustomerViewModel>>("api/customer/GetAll");
            if (result is not null)
                Customers = result;
        }

        public async Task<(bool success, string errorMessage)> UpdateCustomer(UpdateCustomerRequestModel request)
        {
            var response = await _httpClient.PutAsJsonAsync("api/customer/Update", request);
            if (response.IsSuccessStatusCode)
            {
                _navigationManager.NavigateTo("customers");
                return (true, null);
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return (false, errorMessage);
            }
        }
    }
}
