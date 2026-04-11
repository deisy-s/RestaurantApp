using RestaurantApp.Controller;
using RestaurantApp.Services;
using System.Collections.ObjectModel;

namespace RestaurantApp;

public partial class History : ContentPage
{
    List<OrderData> orders = new List<OrderData>();
    ObservableCollection<FullOrder> observableOrders = new ObservableCollection<FullOrder>();

    public History()
	{
		InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        orderGrid.ItemsSource = observableOrders;
        await LoadUserOrders();
    }

    // Load the users' orders
    private async Task LoadUserOrders()
    {
        try
        {
            observableOrders.Clear();
            UserController userController = new UserController();

            orders = await userController.getOrders(AppSession.CurrentUser);

            if (orders != null)
            {
                var orderDetails = orders.Select(async order =>
                {
                    var details = await userController.getOrderDetails(order);
                    return new FullOrder { order = order, details = details };
                });

                var results = await Task.WhenAll(orderDetails);

                foreach (var res in results) 
                {
                    observableOrders.Add(res);
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", "No pudimos cargar tus pedidos", "OK");
        }
    }

    // Load the order details when the image is clicked
    private async void imgLink_Clicked(object sender, EventArgs e)
    {
        try
        {
            var image = (Image)sender;

            var item = (FullOrder)image.BindingContext;

            HistoryInfo fullInfo = new HistoryInfo(item);

            if (Application.Current.MainPage is FlyoutPage flyout)
            {
                await flyout.Detail.Navigation.PushAsync(fullInfo);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", "No se pudo abrir la pantalla", "OK");
        }
    }
}