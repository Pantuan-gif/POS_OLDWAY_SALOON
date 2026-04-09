using System;
using System.Collections.Generic;
using System.Linq;

namespace POS_OLDWAY_SALOON.MVVM.MODELS;

public class Order
{
    //Order models for Ordering
    public string ReferenceNumber { get; set; } = Guid.NewGuid().ToString("N").ToUpper();
    public DateTime Date { get; set; } = DateTime.Now;
    public string PaymentMethod { get; set; } = "Cash";
    public List<CartItem> Items { get; set; } = new List<CartItem>();

    // Operator who processed this order (cashier name)
    public string? OperatorName { get; set; }

    // Amount tendered by customer (for cash payments)
    public decimal TenderedAmount { get; set; }

    // Change due to customer
    public decimal Change { get; set; }

    public int ItemCount => Items?.Count ?? 0;
    public decimal Total => Items?.Sum(i => i.TotalPrice) ?? 0m;

    // Helper properties used by receipt and reports
    public string Time => Date.ToString("hh:mm tt");
    public string TotalPayment => Total.ToString("C2");
}
