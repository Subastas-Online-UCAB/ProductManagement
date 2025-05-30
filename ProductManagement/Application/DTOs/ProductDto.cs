using ProductManagement.Domain.Enums;

namespace ProductManagement.Application.DTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ProductType Type { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }
}