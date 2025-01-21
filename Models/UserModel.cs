using System.ComponentModel.DataAnnotations;

namespace CoffeeShop_APIConsume.Models
{
    public class UserModel
    {
        public int? UserID { get; set; }
        [Required(ErrorMessage = "Please Enter User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please Enter Email")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Your Email is not valid.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please Enter Mobile Name")]
        [MaxLength(10)]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "Please Enter Address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Please Enter Is Active")]
        public bool IsActive { get; set; }
    }
}
