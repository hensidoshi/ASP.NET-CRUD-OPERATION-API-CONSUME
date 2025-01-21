using Microsoft.AspNetCore.Mvc;
using CoffeeShop_APIConsume.Models;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoffeeShop_APIConsume.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri(_configuration["WebApiBaseUrl"])
            };
        }

        #region CustomerList
        public async Task<IActionResult> CustomerList()
        {
            var response = await _httpClient.GetAsync("api/Customer");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var customers = JsonConvert.DeserializeObject<List<CustomerModel>>(data);
                return View(customers);
            }
            return View(new List<CustomerModel>());
        }
        #endregion

        #region UserDropDown
        public async Task UserDropDown()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/Customer/Users");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<UserDropDownModel>>(jsonData);
                ViewBag.UserList = users;
            }
        }
        #endregion

        #region CustomerAddEdit
        public async Task<IActionResult> CustomerAddEdit(int? CustomerID)
        {
            await UserDropDown();
            if (CustomerID.HasValue)
            {
                var response = await _httpClient.GetAsync($"api/Customer/{CustomerID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var customer = JsonConvert.DeserializeObject<CustomerModel>(data);
                    return View(customer);
                }
            }
            return View(new CustomerModel());
        }

        #endregion

        #region CustomerSave
        [HttpPost]
        public async Task<IActionResult> Save(CustomerModel customer)
        {
            await UserDropDown();
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(customer);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (customer.CustomerID == null)
                    response = await _httpClient.PostAsync("api/Customer", content);
                else
                    response = await _httpClient.PutAsync($"api/Customer/{customer.CustomerID}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = customer.CustomerID == null
                        ? "Customer successfully added."
                        : "Customer successfully updated.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Operation failed. Please try again.";
                }

                return RedirectToAction("CustomerList");
            }
            return View("CustomerAddEdit", customer);
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int CustomerID)
        {
            var response = await _httpClient.DeleteAsync($"api/Customer/{CustomerID}");
            if (response.IsSuccessStatusCode)
            {
                TempData["DeleteSuccessMessage"] = "Customer deleted successfully.";
            }
            else
            {
                TempData["DeleteErrorMessage"] = "Failed to delete the customer.";
            }
            return RedirectToAction("CustomerList");
        }
        #endregion
    }
}
