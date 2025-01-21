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
    public class BillController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public BillController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri(_configuration["WebApiBaseUrl"])
            };
        }

        #region BillList
        public async Task<IActionResult> BillList()
        {
            var response = await _httpClient.GetAsync("api/Bill");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var bills = JsonConvert.DeserializeObject<List<BillModel>>(data);
                return View(bills);
            }
            return View(new List<BillModel>());
        }
        #endregion

        #region UserDropDown
        public async Task UserDropDown()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/Bill/Users");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<UserDropDownModel>>(jsonData);
                ViewBag.UserList = users;
            }
        }
        #endregion

        #region OrderDropDown
        public async Task OrderDropDown()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/Bill/Orders");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<List<OrderDropDownModel>>(jsonData);
                ViewBag.OrderList = orders;
            }
        }
        #endregion


        #region BillAddEdit
        public async Task<IActionResult> BillAddEdit(int? BillID)
        {
            await OrderDropDown();
            await UserDropDown();
            if (BillID.HasValue)
            {
                var response = await _httpClient.GetAsync($"api/Bill/{BillID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var bill = JsonConvert.DeserializeObject<BillModel>(data);
                    return View(bill);
                }
            }
            return View(new BillModel());
        }
        #endregion

        #region BillSave
        [HttpPost]
        public async Task<IActionResult> Save(BillModel bill)
        {
            await OrderDropDown();
            await UserDropDown();
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(bill);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (bill.BillID == null)
                    response = await _httpClient.PostAsync("api/Bill", content);
                else
                    response = await _httpClient.PutAsync($"api/Bill/{bill.BillID}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = bill.BillID == null
                        ? "Bill successfully added."
                        : "Bill successfully updated.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Operation failed. Please try again.";
                }

                return RedirectToAction("BillList");
            }
            return View("BillAddEdit", bill);
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int BillID)
        {
            var response = await _httpClient.DeleteAsync($"api/Bill/{BillID}");
            if (response.IsSuccessStatusCode)
            {
                TempData["DeleteSuccessMessage"] = "Bill deleted successfully.";
            }
            else
            {
                TempData["DeleteErrorMessage"] = "Failed to delete the bill.";
            }
            return RedirectToAction("BillList");
        }
        #endregion
    }
}
