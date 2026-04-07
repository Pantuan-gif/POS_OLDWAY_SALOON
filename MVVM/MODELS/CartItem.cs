using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_OLDWAY_SALOON.MVVM.MODELS;

public class CartItem
{
    public Product Product { get; set; } = new();
    public int Quantity { get; set; } = 1;

    public decimal Total => Product.Price * Quantity;
}