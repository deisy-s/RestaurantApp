using Android.Service.Voice;
using RestaurantApp.Controller;

namespace RestaurantApp;

public partial class OrderInfo : ContentPage
{
    List<OrderData> orders = new List<OrderData>();
    List<FullOrder> fullOrders = new List<FullOrder>();

    public OrderInfo()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        pkOrders.SelectedIndex = 0;
        await LoadUserOrders();
    }

    private async Task LoadUserOrders()
    {
        try
        {
            fullOrders.Clear();
            cvOrders.ItemsSource = null;
            AdminController adminController = new AdminController();
            DateTime dtToday = DateTime.Now;

            orders = await adminController.getOrders(dtToday);

            if (orders != null)
            {
                foreach (var order in orders)
                {
                    if(order.status != "Entregado")
                    {
                        var orderDetails = await adminController.getOrderDetails(order);
                        var clientDetails = await adminController.getClientDetails(order);
                        fullOrders.Add(new FullOrder
                        {
                            order = order,
                            details = orderDetails,
                            users = clientDetails
                        });
                    }
                }

                cvOrders.ItemsSource = fullOrders;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", "No pudimos cargar los pedidos", "OK");
        }
    }

    private async Task LoadUserFinishedOrders()
    {
        try
        {
            fullOrders.Clear();
            cvOrders.ItemsSource = null;
            AdminController adminController = new AdminController();
            DateTime dtToday = DateTime.Now;

            orders = await adminController.getOrders(dtToday);

            if (orders != null)
            {
                foreach (var order in orders)
                {
                    if (order.status == "Entregado" || order.status == "Cancelado")
                    {
                        var orderDetails = await adminController.getOrderDetails(order);
                        var clientDetails = await adminController.getClientDetails(order);
                        fullOrders.Add(new FullOrder
                        {
                            order = order,
                            details = orderDetails,
                            users = clientDetails
                        });
                    }
                }

                cvOrders.ItemsSource = fullOrders;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", "No pudimos cargar los pedidos", "OK");
        }
    }

    private async void btnStatusChange(string newStatus, object sender, EventArgs e)
    {
        try
        {
            // Obtener el botón que disparó el evento
            var button = (Button)sender;

            // Obtener el modelo de datos asociado al botón
            var selectedItem = (FullOrder)button.BindingContext;

            if (selectedItem != null && selectedItem.order.status != newStatus)
            {
                GlobalController globalController = new GlobalController();
                string oldStatus = selectedItem.order.status;
                selectedItem.order.status = newStatus;

                if (await globalController.updateStatus(selectedItem.order))
                {
                    selectedItem.RefreshStatus();
                }
                else
                {
                    selectedItem.order.status = oldStatus;
                    await DisplayAlertAsync("Error", "No se pudo actualizar en el servidor", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", "No pudimos cambiar el estado del pedido", "OK");
        }
    }

    private void btnInProcess_Clicked(object sender, EventArgs e) => btnStatusChange("En preparación", sender, e);

    private void btnSent_Clicked(object sender, EventArgs e) => btnStatusChange("Enviado", sender, e);

    private void btnDelivered_Clicked(object sender, EventArgs e) => btnStatusChange("Entregado", sender, e);

    private async void pkOrders_SelectedIndexChanged(object sender, EventArgs e)
    {
        pkOrders.TextColor = Color.FromArgb("#E5E2E1");
        if(pkOrders.SelectedIndex == 0) 
        {
            await LoadUserOrders();
        }
        else
        {
            await LoadUserFinishedOrders();
        }
    }
}