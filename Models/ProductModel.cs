using System.ComponentModel.DataAnnotations;

namespace CoffeeShop_APIConsume.Models
{
    public class ProductModel
    {
        public int? ProductID { get; set; }
        [Required(ErrorMessage = " Please Enter Product Name")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = " Please Enter Product Price")]
        public decimal ProductPrice { get; set; }
        [Required(ErrorMessage = " Please Enter Product Code")]
        public string ProductCode { get; set; }
        [Required(ErrorMessage = " Please Enter Description")]
        public string Description { get; set; }
        public int UserID { get; set; }
    }
    public class UserDropDownModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}
