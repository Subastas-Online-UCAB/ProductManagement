using System;

namespace ProductManagement.Domain.ValueObjects
{
    public sealed class ProductId
    {
        public Guid Value { get; }

        public ProductId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("Product ID cannot be empty", nameof(value));
            }

            Value = value;
        }

        public static ProductId CreateUnique()
        {
            return new ProductId(Guid.NewGuid());
        }

        public override bool Equals(object? obj)  //
        {
            if (obj is ProductId other)
            {
                return Value.Equals(other.Value);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static implicit operator Guid(ProductId id) => id.Value;
    }
}