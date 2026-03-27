namespace POS_OLDWAY_SALOON.MVVM.MODELS;

public class Category
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int NoStocks { get; set; }
    public string PhotoPath { get; set; } = string.Empty;

    public string StockDisplay => $"Stock : {NoStocks}";
}
