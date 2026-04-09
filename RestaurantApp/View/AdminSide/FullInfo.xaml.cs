namespace RestaurantApp;

public partial class FullInfo : ContentPage
{
	public FullInfo(FullOrder fullOrder)
	{
		InitializeComponent();
		lblName.Text = fullOrder.users.clientName;
        lblLocation.Text = fullOrder.order.eatlocation + " • " + fullOrder.order.status;
		lblDate.Text = fullOrder.order.date.ToString("dd/MM/yyyy");
		lblTotal.Text = "Total: $" + fullOrder.order.totalprice.ToString("0.00");

        gdOrderInfo.ItemsSource = fullOrder.details;
    }
}