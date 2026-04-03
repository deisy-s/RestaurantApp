using RestaurantApp.Controller;

namespace RestaurantApp;

public partial class MenuDetails : ContentPage
{
	MenuData selectedMenuItem;
    string ogName = "";

    public MenuDetails(MenuData menuItem)
	{
		selectedMenuItem = menuItem;
		InitializeComponent();
        this.BindingContext = selectedMenuItem;
        this.Title = selectedMenuItem.Name;
        ogName = selectedMenuItem.Name;
        if (AppSession.CurrentUser.clientType == "client")
            imgEdit.IsVisible = false;
        else
            imgEdit.IsVisible = true;
    }

    private void btnLess_Clicked(object sender, EventArgs e)
    {
        if(lblAmount.Text != "1")
        {
            int curentAmount = int.Parse(lblAmount.Text);
            lblAmount.Text = (curentAmount - 1).ToString();
            if(lblAmount.Text == "1")
            {
                btnLess.IsEnabled = false;
            }
        }
    }

    private void btnMore_Clicked(object sender, EventArgs e)
    {
        int curentAmount = int.Parse(lblAmount.Text);
        lblAmount.Text = (curentAmount + 1).ToString();
        if (lblAmount.Text != "1")
        {
            btnLess.IsEnabled = true;
        }
    }

    private void btnAddToCart_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Obtener el botón que disparó el evento
            var button = (Button)sender;

            // Obtener el modelo de datos asociado al botón
            var selectedItem = (MenuData)button.BindingContext;

            if (selectedItem != null && selectedItem.Availability == "Disponible")
            {
                string currentComment = enComment.Text;
                var existingItem = AppSession.cartItems.FirstOrDefault(x => x.menuID == selectedItem.id && x.comments == currentComment);

                if (existingItem != null)
                {
                    // Si existe, solo aumentar la cantidad
                    existingItem.quantity++;
                }
                else
                {
                    // Si no existe, agregar el nuevo producto
                    AppSession.cartItems.Add(new CartItems
                    {
                        menuID = selectedItem.id,
                        name = selectedItem.Name,
                        comments = enComment.Text,
                        price = selectedItem.Price,
                        imageUrl = selectedItem.ImageUrl,
                        quantity = Convert.ToInt16(lblAmount.Text),
                    });
                }
            }

        } catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private void imgEdit_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            if(AppSession.CurrentUser.clientType == "admin")
            {
                EditMenu editMenu = new EditMenu(selectedMenuItem);
                if (Application.Current.MainPage is FlyoutPage flyout)
                {
                    flyout.Detail.Navigation.PushAsync(editMenu);
                }
            }

        } catch(Exception ex)
        {
                Console.WriteLine(ex.ToString());
        }
    }
}