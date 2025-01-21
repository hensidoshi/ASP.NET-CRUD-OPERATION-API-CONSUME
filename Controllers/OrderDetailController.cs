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
    public class OrderDetailController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public OrderDetailController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri(_configuration["WebApiBaseUrl"])
            };
        }

        #region OrderDetailList
        public async Task<IActionResult> OrderDetailList()
        {
            var response = await _httpClient.GetAsync("api/OrderDetail");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var orderDetails = JsonConvert.DeserializeObject<List<OrderDetailModel>>(data);
                return View(orderDetails);
            }
            return View(new List<OrderDetailModel>());
        }
        #endregion

        #region Dropdowns for Product, Order, and User
        public async Task Dropdowns()
        {
            // Product Dropdown
            HttpResponseMessage productResponse = await _httpClient.GetAsync("api/OrderDetail/Products");
            if (productResponse.IsSuccessStatusCode)
            {
                var productData = await productResponse.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<ProductModel>>(productData);
                ViewBag.ProductList = products;
            }

            // Order Dropdown
            HttpResponseMessage orderResponse = await _httpClient.GetAsync("api/OrderDetail/Orders");
            if (orderResponse.IsSuccessStatusCode)
            {
                var orderData = await orderResponse.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<List<OrderModel>>(orderData);
                ViewBag.OrderList = orders;
            }

            // User Dropdown
            HttpResponseMessage userResponse = await _httpClient.GetAsync("api/OrderDetail/Users");
            if (userResponse.IsSuccessStatusCode)
            {
                var userData = await userResponse.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<UserDropDownModel>>(userData);
                ViewBag.UserList = users;
            }
        }
        #endregion

        #region OrderDetailAddEdit
        public async Task<IActionResult> OrderDetailAddEdit(int? OrderDetailID)
        {
            await Dropdowns(); // Load dropdowns for Product, Order, and User
            if (OrderDetailID.HasValue)
            {
                var response = await _httpClient.GetAsync($"api/OrderDetail/{OrderDetailID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var orderDetail = JsonConvert.DeserializeObject<OrderDetailModel>(data);
                    return View(orderDetail);
                }
            }
            return View(new OrderDetailModel());
        }
        #endregion

        #region OrderDetailSave
        [HttpPost]
        public async Task<IActionResult> Save(OrderDetailModel orderDetail)
        {
            await Dropdowns(); // Load dropdowns for Product, Order, and User
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(orderDetail);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (orderDetail.OrderDetailID == null)
                    response = await _httpClient.PostAsync("api/OrderDetail", content);
                else
                    response = await _httpClient.PutAsync($"api/OrderDetail/{orderDetail.OrderDetailID}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = orderDetail.OrderDetailID == null
                        ? "OrderDetail successfully added."
                        : "OrderDetail successfully updated.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Operation failed. Please try again.";
                }

                return RedirectToAction("OrderDetailList");
            }
            return View("OrderDetailAddEdit", orderDetail);
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int OrderDetailID)
        {
            var response = await _httpClient.DeleteAsync($"api/OrderDetail/{OrderDetailID}");
            if (response.IsSuccessStatusCode)
            {
                TempData["DeleteSuccessMessage"] = "OrderDetail deleted successfully.";
            }
            else
            {
                TempData["DeleteErrorMessage"] = "Failed to delete the order detail.";
            }
            return RedirectToAction("OrderDetailList");
        }
        #endregion
    }
}
