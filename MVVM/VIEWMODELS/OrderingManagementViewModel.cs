using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.MVVM.SERVICES;
using POS_OLDWAY_SALOON.Services;
using System.Collections.ObjectModel;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class OrderingManagementViewModel : ObservableObject
{
    private readonly APISERVICES _api = new APISERVICES();

    [ObservableProperty] private string searchText = string.Empty;

    private List<Product> _allProducts = new();
    public ObservableCollection<Product> FilteredProducts { get; } = new();

    public OrderingManagementViewModel()
    {
        LoadAllProducts();
    }

    private async void LoadAllProducts()
    {
        var response = await _api.GetAllProductsAsync();
        _allProducts = response;
        FilteredProducts.Clear();
        foreach (var product in response)
            FilteredProducts.Add(product);
    }

    partial void OnSearchTextChanged(string value)
    {
        var source = string.IsNullOrWhiteSpace(value)
            ? _allProducts
            : _allProducts.Where(p => p.ProductName.Contains(value, StringComparison.OrdinalIgnoreCase)).ToList();

        FilteredProducts.Clear();
        foreach (var p in source)
            FilteredProducts.Add(p);
    }

    [RelayCommand]
    private void AddToCart(Product product)
    {
        if (product.Quantity <= 0)
        {
            Application.Current?.MainPage?.DisplayAlert("Out of Stock", $"{product.ProductName} is out of stock", "OK");
            return;
        }

        CartService.AddToCart(product);
        Application.Current?.MainPage?.DisplayAlert("Added", $"{product.ProductName} added to cart", "OK");
    }
}