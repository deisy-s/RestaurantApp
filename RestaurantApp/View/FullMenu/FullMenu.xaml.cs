using RestaurantApp.Controller;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace RestaurantApp;

public partial class FullMenu : ContentPage
{
    private List<MenuData> fullMenu;

    public FullMenu()
	{
		InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateCartNum();
        LoadMenu();
    }

    private async void LoadMenu()
    {
        try
        {
            GlobalController globalController = new GlobalController();
            fullMenu = await globalController.getMenuItems();

            cvSushiMenu.ItemsSource = fullMenu;
        } catch(Exception ex)
        {
            await DisplayAlertAsync("Error", $"No se pudo cargar el menú: {ex.Message}", "OK");
        }
    }

    private void btnAllMenu_Clicked(object sender, EventArgs e) => FilterMenu("All", (Button)sender);
    private void btnSushiMenu_Clicked(object sender, EventArgs e) => FilterMenu("Sushi", (Button)sender);
    private void btnDrinkMenu_Clicked(object sender, EventArgs e) => FilterMenu("Drink", (Button)sender);

    private void FilterMenu(string type, Button selectedButton)
    {
        // Filtrar la lista
        if (type == "All")
            cvSushiMenu.ItemsSource = fullMenu;
        else
            cvSushiMenu.ItemsSource = fullMenu.Where(x => x.Type == type).ToList();

        // Resetear colores de botones
        var normalBg = Color.FromArgb("#2A2A2A");
        var normalText = Color.FromArgb("#E5E2E1");

        btnAllMenu.BackgroundColor = btnSushiMenu.BackgroundColor = btnDrinkMenu.BackgroundColor = normalBg;
        btnAllMenu.TextColor = btnSushiMenu.TextColor = btnDrinkMenu.TextColor = normalText;

        // Modificar color de botón seleccionado
        selectedButton.BackgroundColor = Color.FromArgb("#74A173");
        selectedButton.TextColor = Color.FromArgb("#131313");
    }

    private void cvSushiMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ((CollectionView)sender).SelectedItem = null; // Eliminar el color de selección del CollectionView
        var selectedItem = e.CurrentSelection.FirstOrDefault() as MenuData; // Obtener el item seleccionado

        if (selectedItem != null) // Verificar que se haya seleccionado un item del menú
        {
            NavigateToMenuDetails(selectedItem, sender); // Abrir la pantalla de detalles del platillo
        }
    }

    private async void NavigateToMenuDetails(MenuData menuItem, object sender)
    {
        MenuDetails menuDetails = new MenuDetails(menuItem); // Crear una instancia de MenuDetails pasando el menú seleccionado

        // Obtener el FlyoutPage y navegar dentro del NavigationPage de menuDetails
        if (Application.Current.MainPage is FlyoutPage flyout)
        {
            await flyout.Detail.Navigation.PushAsync(menuDetails); // Navegar dentro del NavigationPage que está en el Detail
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Orders cart = new Orders();
        if (Application.Current.MainPage is FlyoutPage flyout)
        {
            flyout.Detail.Navigation.PushAsync(cart);
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
                string currentComment = "";
                var existingItem = AppSession.cartItems.FirstOrDefault(x => x.menuID == selectedItem.id && x.comments == currentComment);

                if (existingItem != null)
                {
                    // Si existe, solo aumentamos la cantidad
                    existingItem.quantity ++;
                }
                else
                {
                    // Si no existe, agregamos el nuevo producto
                    AppSession.cartItems.Add(new CartItems
                    {
                        menuID = selectedItem.id,
                        name = selectedItem.Name,
                        comments = "",
                        price = selectedItem.Price,
                        imageUrl = selectedItem.ImageUrl,
                        quantity = 1
                    });
                }

                UpdateCartNum();
            }

        } catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private void UpdateCartNum()
    {
        // Calcular el total de items
        int totalItems = AppSession.cartItems.Sum(x => x.quantity);

        if (totalItems > 0)
        {
            lblCartCount.Text = totalItems.ToString();
            cartBadge.IsVisible = true;
        }
        else
        {
            cartBadge.IsVisible = false;
        }
    }
}