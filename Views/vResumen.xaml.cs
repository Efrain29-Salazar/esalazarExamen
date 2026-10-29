using System.Globalization;
using System.Text;
using esalazarExamen.Models;

namespace esalazarExamen.Views;

public partial class vResumen : ContentPage
{
    private readonly string _usuario;
    private readonly Socio _socio;
    private readonly vRegistro _registro;

    public vResumen(string usuario, Socio socio, vRegistro registro)
    {
        InitializeComponent();
        _usuario = usuario;
        _socio = socio;
        _registro = registro;

        lblUsuario.Text = $"Usuario conectado: {_usuario}";
        lblNombre.Text = socio.Nombre;
        lblApellido.Text = socio.Apellido;
        lblEdad.Text = socio.Edad.ToString();
        lblFecha.Text = socio.FechaInicio.ToString("dd/MM/yyyy");
        lblPlan.Text = socio.Plan;
        lblCasillero.Text = socio.Casillero ? "Sí" : "No";
        lblMonto.Text = Dinero(socio.MontoInicial);
        lblPagoMensual.Text = Dinero(socio.PagoMensual);
        lblPagoTotal.Text = Dinero(socio.PagoTotal);
    }

    private static string Dinero(decimal valor) =>
        "$" + valor.ToString("F2", CultureInfo.InvariantCulture);

    
    private async void btnExportar_Clicked(object? sender, EventArgs e)
    {
        try
        {
            string nombreArchivo = LimpiarNombre(
                $"Socio_{_socio.Nombre}_{_socio.Apellido}_{DateTime.Now:yyyyMMdd_HHmmss}") + ".txt";
            string ruta = Path.Combine(ObtenerCarpeta(), nombreArchivo);

            var sb = new StringBuilder();
            sb.AppendLine("REPORTE DE SOCIO - GIMNASIO");
            sb.AppendLine("---------------------------");
            sb.AppendLine($"Usuario conectado: {_usuario}");
            sb.AppendLine($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            sb.AppendLine();
            sb.AppendLine($"Nombre: {_socio.Nombre}");
            sb.AppendLine($"Apellido: {_socio.Apellido}");
            sb.AppendLine($"Edad: {_socio.Edad}");
            sb.AppendLine($"Fecha de inicio: {_socio.FechaInicio:dd/MM/yyyy}");
            sb.AppendLine($"Plan: {_socio.Plan}");
            sb.AppendLine($"Casillero: {(_socio.Casillero ? "Sí" : "No")}");
            sb.AppendLine($"Monto inicial: {Dinero(_socio.MontoInicial)}");
            sb.AppendLine($"Pago mensual: {Dinero(_socio.PagoMensual)}");
            sb.AppendLine($"Pago total: {Dinero(_socio.PagoTotal)}");

            await File.WriteAllTextAsync(ruta, sb.ToString(), Encoding.UTF8);

            await DisplayAlertAsync("Archivo exportado",
                $"Nombre: {nombreArchivo}\nRuta: {ruta}", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"No se pudo exportar el archivo: {ex.Message}", "OK");
        }
    }

  
    private static string ObtenerCarpeta()
    {
        if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            string docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (!string.IsNullOrEmpty(docs) && Directory.Exists(docs))
                return docs;
        }
        return FileSystem.AppDataDirectory;
    }

    private static string LimpiarNombre(string nombre)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
            nombre = nombre.Replace(c, '_');
        return nombre.Replace(' ', '_');
    }

    
    private async void btnNuevo_Clicked(object? sender, EventArgs e)
    {
        _registro.Limpiar();
        await Navigation.PopAsync();
    }

   
    private async void btnCerrar_Clicked(object? sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}
