using MauiCryptApp.Views;

namespace MauiCryptApp
{
    public partial class AppShell : Shell
    {
        public Dictionary<string, Type> Routes { get; private set; } = new Dictionary<string, Type>();
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }

        void RegisterRoutes()
        {

            Routes.Add("synchronize", typeof(SynchronizePage));
            Routes.Add($"/{nameof(ItemDetailPage)}", typeof(ItemDetailPage));
            Routes.Add($"/{nameof(ItemsPage)}", typeof(ItemsPage));
            foreach (var item in Routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }
    }
}