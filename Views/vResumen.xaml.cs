namespace esalazarExamen.Views;

public partial class vResumen : ContentPage
{
    public vResumen(string usuario)
    {
        InitializeComponent();
        lblUsuario.Text = $"Usuario conectado: {usuario}";
    }

    private void btnExportar_Clicked(object? sender, EventArgs e) { }

    private async void btnNuevo_Clicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void btnCerrar_Clicked(object? sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}