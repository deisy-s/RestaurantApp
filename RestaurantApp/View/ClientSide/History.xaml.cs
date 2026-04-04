using RestaurantApp.Controller;

namespace RestaurantApp;

public partial class History : ContentPage
{
    List<OrderData> orders = new List<OrderData>();
    List<FullOrder> fullOrders = new List<FullOrder>();

    public History()
	{
		InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadUserOrders();
    }

    private async Task LoadUserOrders()
    {
        try
        {
            fullOrders.Clear();
            cvOrders.ItemsSource = null;
            UserController userController = new UserController();

            orders = await userController.getOrders(AppSession.CurrentUser);

            if (orders != null)
            {
                foreach (var order in orders)
                {
                    var orderDetails = await userController.getOrderDetails(order);
                    fullOrders.Add(new FullOrder
                    {
                        order = order,
                        details = orderDetails
                    });
                }

                cvOrders.ItemsSource = fullOrders;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", "No pudimos cargar tus pedidos", "OK");
        }
    }

    private async void btnCancel_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Obtener el botón que disparó el evento
            var button = (Button)sender;

            // Obtener el modelo de datos asociado al botón
            var selectedItem = (FullOrder)button.BindingContext;

            if (selectedItem != null && selectedItem.order.status != "Cancelado")
            {
                GlobalController globalController = new GlobalController();
                var orderToUpdate = selectedItem.order;
                orderToUpdate.status = "Cancelado";

                if (await globalController.updateStatus(orderToUpdate))
                {
                    await DisplayAlertAsync("Éxito", "Pedido cancelado correctamente", "OK");
                    await LoadUserOrders();
                }
                else
                {
                    await DisplayAlertAsync("Error", "No pudimos cancelar tu pedido", "OK");
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}