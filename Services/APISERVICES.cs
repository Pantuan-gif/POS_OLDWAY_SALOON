using POS_OLDWAY_SALOON.MVVM.MODELS;
using System.Net.Http.Json;

namespace POS_OLDWAY_SALOON.Services;

public class APISERVICES
{
    private readonly HttpClient _client;

    // Your actual MockAPI base URL
    private const string BaseUrl = "https://69d3476336103955f8ebef6.mockapi.io/";

    public APISERVICES()
    {
        _client = new HttpClient { BaseAddress = new Uri(BaseUrl) };
    }

    // ── Login using username ─────────────────────────────────────────────
    public async Task<User?> LoginAsync(string username, string password)
    {
        try
        {
            var response = await _client.GetAsync("users");
            if (!response.IsSuccessStatusCode) return null;

            var users = await response.Content.ReadFromJsonAsync<List<User>>();
            return users?.FirstOrDefault(u => 
                u.Username?.Equals(username, StringComparison.OrdinalIgnoreCase) == true && 
                u.Password == password);
        }
        catch { return null; }
    }

    // ── Users ────────────────────────────────────────────────────────────
    public async Task<List<User>> GetAllUsersAsync()
    {
        try
        {
            var response = await _client.GetAsync("users");
            return response.IsSuccessStatusCode 
                ? await response.Content.ReadFromJsonAsync<List<User>>() ?? new()
                : new();
        }
        catch { return new(); }
    }

    public async Task<bool> AddUserAsync(User user)
    {
        try
        {
            var response = await _client.PostAsJsonAsync("users", user);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        try
        {
            var response = await _client.PutAsJsonAsync($"users/{user.Id}", user);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        try
        {
            var response = await _client.DeleteAsync($"users/{id}");
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    // ── Categories ───────────────────────────────────────────────────────
    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        try
        {
            var response = await _client.GetAsync("categories");
            return response.IsSuccessStatusCode 
                ? await response.Content.ReadFromJsonAsync<List<Category>>() ?? new()
                : new();
        }
        catch { return new(); }
    }

    public async Task<bool> AddCategoryAsync(Category category)
    {
        try
        {
            var response = await _client.PostAsJsonAsync("categories", category);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> UpdateCategoryAsync(Category category)
    {
        try
        {
            var response = await _client.PutAsJsonAsync($"categories/{category.CategoryId}", category);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        try
        {
            var response = await _client.DeleteAsync($"categories/{id}");
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    // ── Products ─────────────────────────────────────────────────────────
    public async Task<List<Product>> GetAllProductsAsync()
    {
        try
        {
            var response = await _client.GetAsync("products");
            return response.IsSuccessStatusCode 
                ? await response.Content.ReadFromJsonAsync<List<Product>>() ?? new()
                : new();
        }
        catch { return new(); }
    }

    public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        try
        {
            var response = await _client.GetAsync($"products?categoryId={categoryId}");
            return response.IsSuccessStatusCode 
                ? await response.Content.ReadFromJsonAsync<List<Product>>() ?? new()
                : new();
        }
        catch { return new(); }
    }

    public async Task<bool> AddProductAsync(Product product)
    {
        try
        {
            var response = await _client.PostAsJsonAsync("products", product);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        try
        {
            var response = await _client.PutAsJsonAsync($"products/{product.ProductId}", product);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        try
        {
            var response = await _client.DeleteAsync($"products/{id}");
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    // ── Transactions ─────────────────────────────────────────────────────
    public async Task<bool> AddTransactionAsync(Transaction transaction)
    {
        try
        {
            var response = await _client.PostAsJsonAsync("transactions", transaction);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<List<Transaction>> GetAllTransactionsAsync()
    {
        try
        {
            var response = await _client.GetAsync("transactions");
            return response.IsSuccessStatusCode 
                ? await response.Content.ReadFromJsonAsync<List<Transaction>>() ?? new()
                : new();
        }
        catch { return new(); }
    }
}