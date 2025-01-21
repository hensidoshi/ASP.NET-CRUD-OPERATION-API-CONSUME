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
    public class OrderController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri(_configuration["WebApiBaseUrl"])
            };
        }

        #region OrderList
        public async Task<IActionResult> OrderList()
        {
            var response = await _httpClient.GetAsync("api/Order");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<List<OrderModel>>(data);
                return View(orders);
            }
            return View(new List<OrderModel>());
        }
        #endregion

        #region UserDropDown
        public async Task UserDropDown()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/Order/users");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<UserDropDownModel>>(jsonData);
                ViewBag.UserList = users;
            }
        }
        #endregion

        #region CustomerDropDown
        public async Task CustomerDropDown()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/Order/customers");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var customers = JsonConvert.DeserializeObject<List<CustomerDropDownModel>>(jsonData);
                ViewBag.CustomerList = customers;
            }
        }
        #endregion


        #region OrderAddEdit
        public async Task<IActionResult> OrderAddEdit(int? OrderID)
        {
            await UserDropDown();
            await CustomerDropDown();
            if (OrderID.HasValue)
            {
                var response = await _httpClient.GetAsync($"api/Order/{OrderID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var order = JsonConvert.DeserializeObject<OrderModel>(data);
                    return View(order);
                }
            }
            return View(new OrderModel());
        }
        #endregion

        #region OrderSave
        [HttpPost]
        public async Task<IActionResult> Save(OrderModel order)
        {
            await UserDropDown();
            await CustomerDropDown();
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (order.OrderID == null)
                    response = await _httpClient.PostAsync("api/Order", content);
                else
                    response = await _httpClient.PutAsync($"api/Order/{order.OrderID}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = order.OrderID == null
                        ? "Order successfully added."
                        : "Order successfully updated.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Operation failed. Please try again.";
                }

                return RedirectToAction("OrderList");
            }
            return View("OrderAddEdit", order);
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int OrderID)
        {
            var response = await _httpClient.DeleteAsync($"api/Order/{OrderID}");
            if (response.IsSuccessStatusCode)
            {
                TempData["DeleteSuccessMessage"] = "Order deleted successfully.";
            }
            else
            {
                TempData["DeleteErrorMessage"] = "Failed to delete the order.";
            }
            return RedirectToAction("OrderList");
        }
        #endregion
    }
}
