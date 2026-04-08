using POS_OLDWAY_SALOON.MVVM.MODELS;
using System.Text.Json;

namespace POS_OLDWAY_SALOON.MVVM.SERVICES;

public static class TransactionService
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    private static string TransactionsFilePath
    {
        get
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(folder, "transactions.json");
        }
    }

    public static async Task<List<Order>> GetAllAsync()
    {
        var path = TransactionsFilePath;
        if (!File.Exists(path)) return new List<Order>();
        try
        {
            var json = await File.ReadAllTextAsync(path);
            var list = JsonSerializer.Deserialize<List<Order>>(json, _jsonOptions);
            return list ?? new List<Order>();
        }
        catch
        {
            return new List<Order>();
        }
    }

    public static async Task SaveAsync(List<Order> orders)
    {
        var path = TransactionsFilePath;
        var json = JsonSerializer.Serialize(orders, _jsonOptions);
        var dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        await File.WriteAllTextAsync(path, json);
    }

    public static async Task AppendAsync(Order order)
    {
        var list = await GetAllAsync();
        list.Add(order);
        await SaveAsync(list);
    }
}
