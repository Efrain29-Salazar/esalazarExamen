namespace esalazarExamen.Views;

public partial class vRegistro : ContentPage
{
    private readonly string _usuario;

    public vRegistro(string usuario)
    {
        InitializeComponent();
        _usuario = usuario;
        lblUsuario.Text = $"Usuario conectado: {_usuario}";
    }

    private void txtEdad_TextChanged(object? sender, TextChangedEventArgs e) { }
    private void txtMonto_TextChanged(object? sender, TextChangedEventArgs e) { }
    private void swCasillero_Toggled(object? sender, ToggledEventArgs e) { }
    private void btnCalcular_Clicked(object? sender, EventArgs e) { }

    private async void btnResumen_Clicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new vResumen(_usuario));
    }
}