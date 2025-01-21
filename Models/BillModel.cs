using System.ComponentModel.DataAnnotations;

namespace CoffeeShop_APIConsume.Models
{
    public class BillModel
    {
        public int? BillID { get; set; }
        [Required(ErrorMessage = "Please Enter Bill Number")]
        public string BillNumber { get; set; }
        [Required(ErrorMessage = "Please Enter Bill Date")]
        [DisplayFormat(DataFormatString = "{0:MM.DD.YYYY}")]
        public DateTime BillDate { get; set; }
        public int OrderID { get; set; }
        [Required(ErrorMessage = "Please Enter Total Amount")]
        public decimal TotalAmount { get; set; }
        [Required(ErrorMessage = "Please Enter Discount")]
        public decimal Discount { get; set; }
        [Required(ErrorMessage = "Please Enter Net Amount")]
        public decimal NetAmount { get; set; }
        public int UserID { get; set; }
    }
}
