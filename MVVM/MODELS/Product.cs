namespace POS_OLDWAY_SALOON.MVVM.MODELS;

public class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public double SizeMl { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string PhotoPath { get; set; } = string.Empty;

    public string PriceDisplay => $"₱{Price:F2}";
    public string SizeDisplay => $"Size: {SizeMl}ml";
    public string StockDisplay => $"Stock: {Quantity}";
}
