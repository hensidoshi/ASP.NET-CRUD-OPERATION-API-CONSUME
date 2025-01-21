using System.ComponentModel.DataAnnotations;

namespace CoffeeShop_APIConsume.Models
{
    public class OrderDetailModel
    {
        public int? OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        [Required(ErrorMessage = "Please Enter Quantity")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Please Enter Amount")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "Please Enter Total Amount")]
        public decimal TotalAmount { get; set; }
        public int UserID { get; set; }

    }
    public class ProductDropDownModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
    }
    public class OrderDropDownModel
    {
        public int OrderID { get; set; }
        public string OrderNumber { get; set; }
    }
}
