using CommunityToolkit.Mvvm.ComponentModel;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using System.Collections.ObjectModel;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class InventoryReportViewModel : ObservableObject
{
    public ObservableCollection<Category> Categories => new InventoryManagementViewModel().FilteredCategories;
}