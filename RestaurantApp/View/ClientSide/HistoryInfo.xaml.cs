using RestaurantApp.Controller;
using RestaurantApp.Services;

namespace RestaurantApp;

public partial class HistoryInfo : ContentPage
{
    FullOrder fullOrder = new FullOrder();

	public HistoryInfo(FullOrder f)
	{
		InitializeComponent();
        fullOrder = f;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        lblLocation.Text = "Pedido para " + fullOrder.order.eatlocation;
        lblStatus.Text = fullOrder.order.status;
        lblDate.Text = fullOrder.order.date.ToString("dd/MM/yyyy HH:mm tt");
        lblSubtotal.Text = "Subtotal: $" + fullOrder.order.subtotal.ToString("0.00");
        if (fullOrder.order.deliveryfee != null)
            lblDelivery.Text = "Envío: $" + fullOrder.order.deliveryfee.Value.ToString("0.00");
        else
            lblDelivery.Text = "Envío: N/A";
        lblIVA.Text = "IVA: $" + fullOrder.order.iva.ToString("0.00");
        lblTotal.Text = "Total: $" + fullOrder.order.totalprice.ToString("0.00");
        gdOrderInfo.ItemsSource = fullOrder.details;
        btnCancel.BindingContext = fullOrder;
    }

    // Cancel an order
    private async void btnCancel_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Get the button that was clicked
            var button = (Button)sender;

            // Obtain the order associated with the button using the BindingContext
            var selectedItem = (FullOrder)button.BindingContext;

            if (selectedItem != null && selectedItem.order.status != "Cancelado")
            {
                GlobalController globalController = new GlobalController();
                string oldStatus = selectedItem.order.status;
                selectedItem.order.status = "Cancelado";
                lblStatus.Text = selectedItem.order.status;

                if (await globalController.updateStatus(selectedItem.order)) // Update the order status in the database
                {
                    // Confirm cancellation to the user and refresh the order list
                    await DisplayAlertAsync("Éxito", "Pedido cancelado correctamente", "OK");
                    selectedItem.RefreshStatus();

                    // Send a notification to the admin
                    var notify = new NotificationService();
                    await notify.SendNotification("admin", "Pedido Cancelado", $"El usuario ha cancelado el pedido #{selectedItem.order.id}");
                }
                else
                {
                    selectedItem.order.status = oldStatus;
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