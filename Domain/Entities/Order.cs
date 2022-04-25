using System;
using System.ComponentModel.DataAnnotations;

namespace DukkanTek.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Customer name is required")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }
        [Required(ErrorMessage ="Quantity is required")]
        [Range(1,5)]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Product is required")]
        public int ProductId { get; set; }        
        public DateTime OrderDate { get; set; }
        public virtual Product Product { get; set; }
    }
}
