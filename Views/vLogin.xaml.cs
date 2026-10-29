namespace esalazarExamen.Views;

public partial class vLogin : ContentPage
{
    public vLogin()
    {
        InitializeComponent();
    }

    private async void btnIniciar_Clicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new vRegistro(txtUsuario.Text ?? string.Empty));
    }

    private async void btnAcerca_Clicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new vAcercaDe());
    }
}