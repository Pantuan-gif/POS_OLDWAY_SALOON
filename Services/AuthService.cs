using POS_OLDWAY_SALOON.MVVM.MODELS;

namespace POS_OLDWAY_SALOON.Services
{
    // Simple centralized authentication state for the app.
    // Stores the currently logged-in user and provides role helpers.
    public static class AuthService
    {
        public static User? CurrentUser { get; set; }

        public static bool IsInRole(string role)
            => CurrentUser is not null && string.Equals(CurrentUser.Role, role, StringComparison.OrdinalIgnoreCase);
    }
}
