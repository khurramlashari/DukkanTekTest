using System;

namespace DukkanTek.Domain.DTOs
{
    public class OrderRequest
    {       
        public string CustomerName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public DateTime OrderDate { get; set; }
        
    }
}
