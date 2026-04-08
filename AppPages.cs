using POS_OLDWAY_SALOON.MVVM.VIEWS;

namespace POS_OLDWAY_SALOON;

/// <summary>
/// Central page cache. Each main page is instantiated once and reused
/// so state and ViewModels are preserved across FlyoutPage navigation.
/// </summary>
public static class AppPages
{
    // ── Main Pages (cached — never re-created) ───────────────────────────────
    private static DashboardView?            _dashboard;
    private static InventoryManagementView?  _inventory;
    private static MVVM.VIEWS.Reports?       _reports;

    public static DashboardView           Dashboard  => _dashboard  ??= new DashboardView();
    public static InventoryManagementView Inventory  => _inventory  ??= new InventoryManagementView();
    public static MVVM.VIEWS.Reports       Reports    => _reports    ??= new MVVM.VIEWS.Reports();

    // ── Push Pages (fresh each time — they are form/detail pages) ───────────
    // These are intentionally NOT cached because they need clean state on every open.
    public static AddCategoryView       NewAddCategoryView()       => new AddCategoryView();
    public static ProductManagementView NewProductManagementView() => new ProductManagementView();
    public static AddProductView        NewAddProductView()        => new AddProductView();
    public static EditProductPage       NewEditProductPage()       => new EditProductPage();
    public static OrderingManagement NewOrderingManagementView() => new OrderingManagement();
}
