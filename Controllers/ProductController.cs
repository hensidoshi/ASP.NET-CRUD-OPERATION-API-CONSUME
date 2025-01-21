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
    public class ProductController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri(_configuration["WebApiBaseUrl"])
            };
        }

        #region ProductList
        public async Task<IActionResult> ProductList()
        {
            var response = await _httpClient.GetAsync("api/product");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<ProductModel>>(data);
                return View(products);
            }
            return View(new List<ProductModel>());
        }
        #endregion

        #region UserDropDown
        public async Task UserDropDown()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/product/users");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<UserDropDownModel>>(jsonData);
                ViewBag.UserList = users;
            }
        }
        #endregion

        #region ProductAddEdit
        public async Task<IActionResult> ProductAddEdit(int? ProductID)
        {
            await UserDropDown();
            if (ProductID.HasValue)
            {
                var response = await _httpClient.GetAsync($"api/product/{ProductID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var product = JsonConvert.DeserializeObject<ProductModel>(data);
                    return View(product);
                }
            }
            return View(new ProductModel());
        }
        #endregion

        #region ProductSave
        [HttpPost]
        public async Task<IActionResult> Save(ProductModel product)
        {
            await UserDropDown();
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (product.ProductID == null)
                    response = await _httpClient.PostAsync("api/product", content);
                else
                    response = await _httpClient.PutAsync($"api/product/{product.ProductID}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = product.ProductID == null
                        ? "Product successfully added."
                        : "Product successfully updated.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Operation failed. Please try again.";
                }

                return RedirectToAction("ProductList");
            }
            return View("ProductAddEdit", product);
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int ProductID)
        {
            var response = await _httpClient.DeleteAsync($"api/product/{ProductID}");
            if (response.IsSuccessStatusCode)
            {
                TempData["DeleteSuccessMessage"] = "Product deleted successfully.";
            }
            else
            {
                TempData["DeleteErrorMessage"] = "Failed to delete the product.";
            }
            return RedirectToAction("ProductList");
        }
        #endregion
    }
}