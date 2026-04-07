namespace POS_OLDWAY_SALOON.MVVM.MODELS;

public class Product
{
    // Server-assigned id (mockapi uses string ids). Keep local numeric ProductId for legacy code.
    public string? Id { get; set; }

    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public double SizeMl { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string PhotoPath { get; set; } = string.Empty;

    public string PriceDisplay => $"₱{Price:F2}";
    public string SizeDisplay => $"Size: {SizeMl}ml";
    public string StockDisplay => $"Stock: {Quantity}";
}
