using Android.Graphics.Pdf.Models;

namespace RestaurantApp;

public partial class FullInfo : ContentPage
{
	FullOrder forder = new FullOrder();

	// Show all the details of the order that was selected
	public FullInfo(FullOrder fullOrder)
	{
		InitializeComponent();
		forder = fullOrder;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        lblName.Text = forder.users.clientName;
        lblLocation.Text = forder.order.eatlocation + " • " + forder.order.status;
        lblDate.Text = forder.order.date.ToString("dd/MM/yyyy HH:mm tt");
        lblSubtotal.Text = "Subtotal: $" + forder.order.subtotal.ToString("0.00");
        if (forder.order.deliveryfee != null)
            lblDelivery.Text = "Envío: $" + forder.order.deliveryfee.Value.ToString("0.00");
        else
            lblDelivery.Text = "Envío: N/A";
        lblIVA.Text = "IVA: $" + forder.order.iva.ToString("0.00");
        lblTotal.Text = "Total: $" + forder.order.totalprice.ToString("0.00");
        gdOrderInfo.ItemsSource = forder.details;
    }
}