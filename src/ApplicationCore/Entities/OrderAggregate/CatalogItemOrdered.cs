using Ardalis.GuardClauses;

namespace Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;

/// <summary>
/// Represents a snapshot of the item that was ordered. If catalog item details change, details of
/// the item that was part of a completed order should not change.
/// </summary>
public class CatalogItemOrdered // ValueObject
{
    public CatalogItemOrdered(int catalogItemId, string productName, string pictureUri)
    {
        Guard.Against.OutOfRange(catalogItemId, nameof(catalogItemId), 1, int.MaxValue);
        Guard.Against.NullOrEmpty(productName, nameof(productName));
        Guard.Against.NullOrEmpty(pictureUri, nameof(pictureUri));

        CatalogItemId = catalogItemId;
        ProductName = productName;
        PictureUri = pictureUri;
    }

#pragma warning disable CS8618 // Required by Entity Framework
    public CatalogItemOrdered() { }

    public int CatalogItemId { get; set; }
    public string ProductName { get; set; }
    public string PictureUri { get; private set; }
}
