using Java.Net;
using Java.Nio.Channels;
using System.Net;

namespace RestaurantApp;

public partial class AddMenu : ContentPage
{
    public string imgFinalURL = "";
	public AddMenu()
	{
		InitializeComponent();
	}

    private void pkType_SelectedIndexChanged(object sender, EventArgs e)
    {
        pkType.TextColor = Color.FromArgb("#E5E2E1");
        if (pkType.SelectedIndex == 0)
		{
			charsSushi.IsVisible = true;
			charsDrink.IsVisible = false;
            CleanDrinkRB();
        } else
		{
			charsSushi.IsVisible = false;
            charsDrink.IsVisible = true;
            CleanSushiRB();
        }
    }

    private void CleanSushiRB()
    {
        rb6.IsChecked = false;
        rb8.IsChecked = false;
        rb12.IsChecked = false;
        rbVeg.IsChecked = false;
        rbNoSea.IsChecked = false;
    }

    private void CleanDrinkRB()
    {
        rbAlc.IsChecked = false;
        rbNoAlc.IsChecked = false;
    }

    private void enName_TextChanged(object sender, TextChangedEventArgs e)
    {
		if (!string.IsNullOrEmpty(enName.Text))
		{
			lblName.Text = enName.Text;
		}
    }

    private async void btnSave_Clicked(object sender, EventArgs e)
    {
        btnSave.IsEnabled = false;
        try
        {
            if(!string.IsNullOrEmpty(enName.Text) && !string.IsNullOrEmpty(enDescription.Text) && !string.IsNullOrEmpty(imgFinalURL) && !string.IsNullOrEmpty(enPrice.Text) && pkType.SelectedItem.ToString() !="Seleccione")
            {
                string type = pkType.SelectedItem.ToString();
                List<string> list = new List<string>();

                if (type == "Sushi")
                {
                    if(rb6.IsChecked)
                    {
                        list.Add("6 PIEZAS");
                    }
                    else if (rb8.IsChecked)
                    {
                        list.Add("8 PIEZAS");
                    }
                    else if (rb12.IsChecked)
                    {
                        list.Add("12 PIEZAS");
                    }

                    if(rbVeg.IsChecked)
                    {
                        list.Add("VEGETARIANO");
                    }
                    else if (rbNoSea.IsChecked)
                    {
                        list.Add("SIN MARISCO");
                    }
                } else if (type == "Bebida")
                {
                    type = "Drink";
                    if (rbAlc.IsChecked)
                    {
                        list.Add("ALCOHÓLICO");
                    }
                    else if (rbNoAlc.IsChecked)
                    {
                        list.Add("SIN ALCOHOL");
                    }
                }

                MenuData menuData = new MenuData
                {
                    Name = enName.Text,
                    Description = enDescription.Text,
                    ImageUrl = imgFinalURL,
                    Price = float.Parse(enPrice.Text),
                    Type = type,
                    Characteristics = list,
                    Availability = "Disponible"
                };

                AdminController menuController = new AdminController();

                bool result = await menuController.saveMenu(menuData);

                if (result)
                {
                    await DisplayAlertAsync("Éxito", "Guardado correctamente", "OK");

                    // Limpiar campos después de guardar
                    enName.Text = string.Empty;
                    enDescription.Text = string.Empty;
                    enPrice.Text = string.Empty;
                    imgFinalURL = string.Empty;
                    pkType.SelectedItem = null;
                    lblName.Text = string.Empty;
                    imgItem.Source = null;
                    CleanSushiRB();
                    CleanDrinkRB();
                }
                else
                {
                    await DisplayAlertAsync("Error", "No se pudo guardar en el menú", "OK");
                }
            }

        }
        catch (Exception ex)
        {
            // Manejar cualquier error que pueda ocurrir durante el guardado
            Console.WriteLine($"Error al guardar el menú: {ex.Message}");
        }
    }

    private async void btnUpload_Clicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Seleccione una imagen",
                FileTypes = FilePickerFileType.Images
            });

            if (result == null)
                return;

            var stream = await result.OpenReadAsync();

            imgItem.Source = ImageSource.FromStream(() => stream);

            AdminController menuController = new AdminController();
            imgFinalURL = await menuController.saveImage(result as FileResult);

        } catch(Exception ex)
        {
            Console.WriteLine($"Error al subir la imagen: {ex.Message}");
        }
    }
}