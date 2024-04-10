using System;

namespace Microsoft.eShopWeb.ApplicationCore.Exceptions;

public class EmptyBasketOnCheckoutException : Exception
{
    public EmptyBasketOnCheckoutException()
        : base($"Basket cannot have 0 items on checkout")
    {
    }

#pragma warning disable SYSLIB0051 // Type or member is obsolete
    protected EmptyBasketOnCheckoutException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
#pragma warning restore SYSLIB0051 // Type or member is obsolete
    {
    }

    public EmptyBasketOnCheckoutException(string message) : base(message)
    {
    }

    public EmptyBasketOnCheckoutException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
