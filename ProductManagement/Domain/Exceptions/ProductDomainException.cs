﻿using System;

namespace ProductManagement.Domain.Exceptions
{
    public class ProductDomainException : Exception
    {
        public ProductDomainException()
        { }

        public ProductDomainException(string message)
            : base(message)
        { }

        public ProductDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}