using POS_OLDWAY_SALOON.MVVM.MODELS;
using System.Text.Json;

namespace POS_OLDWAY_SALOON.MVVM.SERVICES;

public static class UserService
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    private static string UsersFilePath
    {
        get
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(folder, "users.json");
        }
    }

    public static async Task<List<User>> GetAllAsync()
    {
        var path = UsersFilePath;
        if (!File.Exists(path)) return new List<User>();
        try
        {
            var json = await File.ReadAllTextAsync(path);
            var list = JsonSerializer.Deserialize<List<User>>(json, _jsonOptions);
            return list ?? new List<User>();
        }
        catch
        {
            return new List<User>();
        }
    }

    public static async Task SaveAsync(List<User> users)
    {
        var path = UsersFilePath;
        var json = JsonSerializer.Serialize(users, _jsonOptions);
        var dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        await File.WriteAllTextAsync(path, json);
    }
}
