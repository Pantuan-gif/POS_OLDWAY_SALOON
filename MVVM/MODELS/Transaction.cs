namespace POS_OLDWAY_SALOON.MVVM.MODELS;

public class Transaction
{
    public string ProductName { get; set; } = string.Empty;
    public string ImageSource { get; set; } = string.Empty;
    public string DateTime { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int Quantity { get; set; }

    public string AmountDisplay => $"+ ${Amount:F2}";
    public string QuantityDisplay => $"Quantity: {Quantity}";
    public string DateCategoryDisplay => $"{DateTime} | {Category}";
}
