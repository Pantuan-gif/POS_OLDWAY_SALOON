using POS_OLDWAY_SALOON.MVVM.MODELS;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace POS_OLDWAY_SALOON.MVVM.SERVICES;

public static class CartService
{
    private static readonly ObservableCollection<CartItem> _items = new();
    public static ReadOnlyObservableCollection<CartItem> Items { get; } = new ReadOnlyObservableCollection<CartItem>(_items);

    public static void AddToCart(Product p, int quantity = 1)
    {
        var existing = _items.FirstOrDefault(i => i.ProductId == p.ProductId);
        if (existing != null)
        {
            existing.Quantity += quantity;
        }
        else
        {
            _items.Add(new CartItem
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Quantity = quantity,
                UnitPrice = p.Price,
                PhotoPath = p.PhotoPath
            });
        }
    }

    public static void RemoveFromCart(int productId)
    {
        var existing = _items.FirstOrDefault(i => i.ProductId == productId);
        if (existing != null) _items.Remove(existing);
    }

    public static void Clear() => _items.Clear();

    public static int TotalItems => _items.Sum(i => i.Quantity);
    public static decimal TotalPrice => _items.Sum(i => i.TotalPrice);

    // Local JSON persistence
    public static async Task SaveToLocalJsonAsync(string path)
    {
        var json = JsonSerializer.Serialize(_items);
        await File.WriteAllTextAsync(path, json);
    }

    public static async Task LoadFromLocalJsonAsync(string path)
    {
        if (!File.Exists(path)) return;
        var json = await File.ReadAllTextAsync(path);
        var list = JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
        _items.Clear();
        foreach (var it in list) _items.Add(it);
    }
}
