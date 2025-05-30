using ProductManagement.Domain.Enums;
using ProductManagement.Domain.ValueObjects;
using ProductManagement.Domain.Exceptions;

namespace ProductManagement.Domain.Models
{
    public class Product
    {
        public ProductId Id { get; private set; }
        public string Name { get; private set; }
        public ProductType Type { get; private set; }
        public string Description { get; private set; }
        public int Quantity { get; private set; }

        private Product() { } // For EF Core

        public Product(ProductId id, string name, ProductType type, string description, int quantity)
        {
            Id = id ?? throw new ProductDomainException("Product ID cannot be null");
            SetName(name);
            SetType(type);
            SetDescription(description);
            SetQuantity(quantity);
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ProductDomainException("Product name cannot be empty");
            }

            if (name.Length > 100)
            {
                throw new ProductDomainException("Product name cannot be longer than 100 characters");
            }

            Name = name;
        }

        public void SetType(ProductType type)
        {
            Type = type;
        }

        public void SetDescription(string description)
        {
            Description = description ?? string.Empty;
        }

        public void SetQuantity(int quantity)
        {
            if (quantity < 0)
            {
                throw new ProductDomainException("Product quantity cannot be negative");
            }

            Quantity = quantity;
        }

        public void IncreaseQuantity(int amount)
        {
            if (amount <= 0)
            {
                throw new ProductDomainException("Increase amount must be positive");
            }

            Quantity += amount;
        }

        public void DecreaseQuantity(int amount)
        {
            if (amount <= 0)
            {
                throw new ProductDomainException("Decrease amount must be positive");
            }

            if (Quantity - amount < 0)
            {
                throw new ProductDomainException("Insufficient product quantity");
            }

            Quantity -= amount;
        }
    }
}