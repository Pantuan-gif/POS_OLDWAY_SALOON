using CommunityToolkit.Mvvm.ComponentModel;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.Services;
using System.Collections.ObjectModel;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class ReportsViewModel : ObservableObject
{
    private readonly APISERVICES _api = new APISERVICES();

    public ObservableCollection<Transaction> Transactions { get; } = new();

    public ReportsViewModel()
    {
        LoadTransactions();
    }

    private async void LoadTransactions()
    {
        var transactions = await _api.GetAllTransactionsAsync();
        Transactions.Clear();
        foreach (var t in transactions)
            Transactions.Add(t);
    }
}