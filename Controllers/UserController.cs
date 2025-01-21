using Microsoft.AspNetCore.Mvc;
using CoffeeShop_APIConsume.Models;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CoffeeShop_APIConsume.Models;

namespace CoffeeShop_APIConsume.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri(_configuration["WebApiBaseUrl"])
            };
        }

        #region UserList
        public async Task<IActionResult> UserList()
        {
            var response = await _httpClient.GetAsync("api/User");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<UserModel>>(data);
                return View(users);
            }
            return View(new List<UserModel>());
        }
        #endregion

        #region UserAddEdit
        public async Task<IActionResult> UserAddEdit(int? UserID)
        {
            if (UserID.HasValue)
            {
                var response = await _httpClient.GetAsync($"api/User/{UserID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<UserModel>(data);
                    return View(user);
                }
            }
            return View(new UserModel());
        }
        #endregion

        #region UserSave
        [HttpPost]
        public async Task<IActionResult> Save(UserModel user)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (user.UserID == null)
                    response = await _httpClient.PostAsync("api/User", content);
                else
                    response = await _httpClient.PutAsync($"api/User/{user.UserID}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = user.UserID == null
                        ? "User successfully added."
                        : "User successfully updated.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Operation failed. Please try again.";
                }

                return RedirectToAction("UserList");
            }
            return View("UserAddEdit", user);
        }
        #endregion

        #region UserDelete
        public async Task<IActionResult> Delete(int UserID)
        {
            var response = await _httpClient.DeleteAsync($"api/User/{UserID}");
            if (response.IsSuccessStatusCode)
            {
                TempData["DeleteSuccessMessage"] = "User deleted successfully.";
            }
            else
            {
                TempData["DeleteErrorMessage"] = "Failed to delete the user.";
            }
            return RedirectToAction("UserList");
        }
        #endregion
    }
}
