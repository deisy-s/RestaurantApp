using RestaurantApp.Controller;
using RestaurantApp.GoogleAuth;
using RestaurantApp.Platforms.Android;

namespace RestaurantApp
{
    public partial class MainPage : FlyoutPage
    {
        public UserData loggedUser = new UserData();

        public MainPage(UserData user)
        {
            loggedUser = user;
            InitializeComponent();
            UserChange();
            flyoutPage.collectionView.SelectionChanged += OnSelectionChanged;
        }

        void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = e.CurrentSelection.FirstOrDefault() as FlyoutPageItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                if (!((IFlyoutPageController)this).ShouldShowSplitMode)
                    IsPresented = false;
            }
        }

        private void UserChange()
        {
            var menuItems = new List<FlyoutPageItem>();

            // Pestañas comunes para todos
            menuItems.Add(new FlyoutPageItem { Title = "Menu", TargetType = typeof(FullMenu), IconSource = "menu.png" });

            if (loggedUser.clientType == "admin")
            {
                // Pestañas para administradores
                menuItems.Add(new FlyoutPageItem { Title = "Agregar artículos", TargetType = typeof(AddMenu), IconSource = "addmenu.png" });
                menuItems.Add(new FlyoutPageItem { Title = "Pedidos", TargetType = typeof(OrderInfo), IconSource = "order.png" });
                menuItems.Add(new FlyoutPageItem { Title = "Reporte de ventas", TargetType = typeof(SalesReport), IconSource = "report.png" });
            }
            else
            {
                // Pestañas para clientes normales
                menuItems.Add(new FlyoutPageItem { Title = "Mis pedidos", TargetType = typeof(History), IconSource = "history.png" });
                menuItems.Add(new FlyoutPageItem { Title = "Mi información", TargetType = typeof(MyInfo), IconSource = "client.png" });
            }

            flyoutPage.collectionView.ItemsSource = menuItems;
        }
    }
}
