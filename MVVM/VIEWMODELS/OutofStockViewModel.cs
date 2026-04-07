using CommunityToolkit.Mvvm.ComponentModel;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using System.Collections.ObjectModel;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class OutofStockViewModel : ObservableObject
{
    public ObservableCollection<Product> OutOfStockItems { get; } = new();

    public OutofStockViewModel()
    {
        // You can load low-stock items from API later
        // For demo:
        OutOfStockItems.Add(new Product { ProductName = "Sample Low Stock Item", Quantity = 0 });
    }
}