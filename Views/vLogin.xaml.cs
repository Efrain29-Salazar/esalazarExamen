namespace esalazarExamen.Views;

public partial class vLogin : ContentPage
{
    
    private readonly string[,] usuarios =
    {
        { "recepcion",  "gym2026"  },
        { "entrenador", "fit_01"   },
        { "uisrael",    "2026"     },
        { "gerente",    "admin123" }
    };

    public vLogin()
    {
        InitializeComponent();
    }

    private async void btnIniciar_Clicked(object? sender, EventArgs e)
    {
        try
        {
            string usuario = txtUsuario.Text?.Trim() ?? string.Empty;
            string clave = txtClave.Text ?? string.Empty;

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(clave))
            {
                await DisplayAlertAsync("Dato incorrecto", "Ingrese el usuario y la contraseña.", "OK");
                return;
            }

            
            bool acceso = false;
            for (int i = 0; i < usuarios.GetLength(0); i++)
            {
                if (usuarios[i, 0] == usuario && usuarios[i, 1] == clave)
                {
                    acceso = true;
                    break;
                }
            }

            if (!acceso)
            {
                await DisplayAlertAsync("Dato incorrecto", "Usuario o contraseña incorrectos.", "OK");
                txtClave.Text = string.Empty;
                txtClave.Focus();
                return;
            }

            
            txtUsuario.Text = string.Empty;
            txtClave.Text = string.Empty;
            await Navigation.PushAsync(new vRegistro(usuario));
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"Ocurrió un error: {ex.Message}", "OK");
        }
    }

    private async void btnAcerca_Clicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new vAcercaDe());
    }
}
