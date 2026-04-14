using POS_OLDWAY_SALOON.MVVM.MODELS;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Text.Json;

namespace POS_OLDWAY_SALOON.Services
{
    public class APISERVICES
    {
        private readonly HttpClient _client;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public APISERVICES()
        {
            //https://mockapi.io/clone/69d34763336103955f8ebef7 for cloning the mock api datas
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://69d34763336103955f8ebef6.mockapi.io/api/oldsaloon/")
            };
        }
       

        // ════════════════════════════════════════════════════════════════════
        //  CATEGORY  CRUD
        // ════════════════════════════════════════════════════════════════════

        /// <summary>Get all categories.</summary>
        public async Task<List<Category>> GetCategoriesAsync()
        {
            var response = await _client.GetAsync("categories");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<Category>>(_jsonOptions);
            return result ?? new List<Category>();
        }

        /// <summary>Get a single category by ID.</summary>
        // Server uses string ids; accept string id
        public async Task<Category?> GetCategoryAsync(string id)
        {
            var path = $"categories/{id}";
            var response = await _client.GetAsync(path);
            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"GET {new Uri(_client.BaseAddress!, path)} returned {(int)response.StatusCode} {response.ReasonPhrase}: {body}");
            }
            return await response.Content.ReadFromJsonAsync<Category>(_jsonOptions);
        }

        /// <summary>Create a new category. Returns the saved category (with server-assigned ID).</summary>
        public async Task<Category?> AddCategoryAsync(Category category)
        {
            var json    = JsonSerializer.Serialize(category);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("categories", content);
            response.EnsureSuccessStatusCode();
            var saved = await response.Content.ReadFromJsonAsync<Category>(_jsonOptions);
            return saved;
        }

        /// <summary>Update an existing category. Returns the updated category.</summary>
        public async Task<Category?> UpdateCategoryAsync(Category category)
        {
            var json    = JsonSerializer.Serialize(category);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Use server string id if present
            var idPath = category.Id ?? category.CategoryId.ToString();
            var response = await _client.PutAsync($"categories/{idPath}", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Category>(_jsonOptions);
        }

        /// <summary>Delete a category by ID. Returns true on success.</summary>
        public async Task<bool> DeleteCategoryAsync(string id)
        {
            var response = await _client.DeleteAsync($"categories/{id}");
            return response.IsSuccessStatusCode;
        }

        // ════════════════════════════════════════════════════════════════════
        //  PRODUCT  CRUD
        // ════════════════════════════════════════════════════════════════════

        /// <summary>Get all products.</summary>
        public async Task<List<Product>> GetProductsAsync()
        {
            var response = await _client.GetAsync("products");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<Product>>(_jsonOptions);
            return result ?? new List<Product>();
        }

        /// <summary>Get all products that belong to a specific category.</summary>
        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            var all = await GetProductsAsync();
            return all.Where(p => p.CategoryId == categoryId).ToList();
        }

        /// <summary>Get a single product by ID.</summary>
        // Get single product by server string id
        public async Task<Product?> GetProductAsync(string id)
        {
            var path = $"products/{id}";
            var response = await _client.GetAsync(path);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"GET {new Uri(_client.BaseAddress!, path)} returned {(int)response.StatusCode} {response.ReasonPhrase}: {body}");
            }
            return await response.Content.ReadFromJsonAsync<Product>(_jsonOptions);
        }

        /// <summary>Create a new product. Returns the saved product (with server-assigned ID).</summary>
        public async Task<Product?> AddProductAsync(Product product)
        {
            var json    = JsonSerializer.Serialize(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("products", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Product>(_jsonOptions);
        }

        /// <summary>Update an existing product. Returns the updated product.</summary>
        public async Task<Product?> UpdateProductAsync(Product product)
        {
            var json    = JsonSerializer.Serialize(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var idPath = product.Id ?? product.ProductId.ToString();
            var response = await _client.PutAsync($"products/{idPath}", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Product>(_jsonOptions);
        }

        /// <summary>Delete a product by ID. Returns true on success.</summary>
        public async Task<bool> DeleteProductAsync(string id)
        {
            var path = $"products/{id}";
            var response = await _client.DeleteAsync(path);
            if (response.IsSuccessStatusCode)
                return true;

            // Read response body for diagnostics and throw a descriptive error
            var body = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"DELETE {new Uri(_client.BaseAddress!, path)} returned {(int)response.StatusCode} {response.ReasonPhrase}: {body}");
        }
    }
}
