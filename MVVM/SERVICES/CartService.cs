using POS_OLDWAY_SALOON.MVVM.MODELS;
using System.Collections.ObjectModel;
using System.Linq;

namespace POS_OLDWAY_SALOON.MVVM.SERVICES;

public static class CartService
{
    public static ObservableCollection<CartItem> Cart { get; } = new();

    public static void AddToCart(Product product, int qty = 1)
    {
        var existing = Cart.FirstOrDefault(c => c.Product.ProductId == product.ProductId);
        if (existing != null)
            existing.Quantity += qty;
        else
            Cart.Add(new CartItem { Product = product, Quantity = qty });
    }

    public static void RemoveFromCart(CartItem item) => Cart.Remove(item);

    public static void Clear() => Cart.Clear();

    public static decimal GetTotal() => Cart.Sum(c => c.Total);
}