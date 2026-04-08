using System;

namespace POS_OLDWAY_SALOON.MVVM.MODELS;

public class CartItem
{
    //product model
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string PhotoPath { get; set; } = string.Empty;

    //Price Multiplier
    public decimal TotalPrice => UnitPrice * Quantity;
}
