using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace POS_OLDWAY_SALOON.MVVM.MODELS;

public class ProductSelection : ObservableObject
{
    //model for selection of a product
    public Product Product { get; }

    private int _selectedQuantity;

    public ProductSelection(Product product)
    {
        Product = product;
        _selectedQuantity = 1;
    }

    public int SelectedQuantity
    {
        get => _selectedQuantity;
        set => SetProperty(ref _selectedQuantity, value);
    }
}
