using System.ComponentModel.DataAnnotations;

namespace CoffeeShop_APIConsume.Models
{
    public class OrderModel
    {
        public int? OrderID { get; set; }
        [Required(ErrorMessage = "Please Enter Date")]
        [DisplayFormat(DataFormatString = "{0:MM.DD.YYYY}")]
        public DateTime OrderDate { get; set; }
        [Required(ErrorMessage = "Please Enter Order Number")]
        public string OrderNumber { get; set; }
        public int CustomerID { get; set; }
        [Required(ErrorMessage = "Please Enter Payment Mode")]
        public string PaymentMode { get; set; }
        [Required(ErrorMessage = "Please Enter Total Amount")]
        public decimal TotalAmount { get; set; }
        [Required(ErrorMessage = "Please Enter Shipping Address")]
        public string ShippingAddress { get; set; }
        public int UserID { get; set; }
    }
    public class CustomerDropDownModel
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
    }
}
