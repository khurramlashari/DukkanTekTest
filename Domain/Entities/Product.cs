using System.ComponentModel.DataAnnotations;

namespace DukkanTek.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }    
        public string Description { get; set; }
        public decimal Weight { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Barcode { get; set; }
        public int ProductStatusId { get; set; }
        public int ProductCategoryId { get; set; }
        [Required]
        public int Stock { get; set; }
        public virtual Category ProductCategory { get; set; }
        public virtual ProductStatus ProductStatus { get; set; }
    }
}
