using System.Globalization;
using System.Text.RegularExpressions;
using esalazarExamen.Models;

namespace esalazarExamen.Views;

public partial class vRegistro : ContentPage
{
    
    private const decimal CostoMembresia = 1200m;
    private const decimal PorcentajeMinimo = 0.10m;   
    private const decimal PorcentajeMaximo = 0.50m;   
    private const int NumeroCuotas = 5;
    private const decimal PorcentajeCuota = 0.02m;    
    private const decimal ExtraCasillero = 10m;       
    private const int EdadMinima = 16;

    private readonly string _usuario;

    public vRegistro(string usuario)
    {
        InitializeComponent();
        _usuario = usuario;
        lblUsuario.Text = $"Usuario conectado: {_usuario}";

        
        dtpFecha.MinimumDate = DateTime.Today;
        dtpFecha.Date = DateTime.Today;
    }
    
    private void txtEdad_TextChanged(object? sender, TextChangedEventArgs e)
    {
        string texto = e.NewTextValue ?? string.Empty;
        string soloDigitos = new string(texto.Where(char.IsDigit).ToArray());
        if (soloDigitos != texto)
            txtEdad.Text = soloDigitos;
    }

   
    private void txtMonto_TextChanged(object? sender, TextChangedEventArgs e)
    {
        txtPagoMensual.Text = string.Empty;
    }

    private void swCasillero_Toggled(object? sender, ToggledEventArgs e)
    {
        txtPagoMensual.Text = string.Empty;
    }

    

    private async void btnCalcular_Clicked(object? sender, EventArgs e)
    {
        try
        {
            if (!TryCalcularPago(out decimal monto, out decimal pagoMensual, out string error))
            {
                await DisplayAlertAsync("Dato inválido", error, "OK");
                txtMonto.Focus();
                return;
            }

            txtPagoMensual.Text = pagoMensual.ToString("F2", CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"Ocurrió un error: {ex.Message}", "OK");
        }
    }

    private async void btnResumen_Clicked(object? sender, EventArgs e)
    {
        try
        {
            
            string nombre = txtNombre.Text?.Trim() ?? string.Empty;
            string apellido = txtApellido.Text?.Trim() ?? string.Empty;

            if (!EsTextoValido(nombre))
            {
                await DisplayAlertAsync("Dato inválido", "Ingrese un nombre válido (solo letras).", "OK");
                txtNombre.Focus();
                return;
            }
            if (!EsTextoValido(apellido))
            {
                await DisplayAlertAsync("Dato inválido", "Ingrese un apellido válido (solo letras).", "OK");
                txtApellido.Focus();
                return;
            }

            
            if (!int.TryParse(txtEdad.Text?.Trim(), out int edad))
            {
                await DisplayAlertAsync("Dato inválido", "Ingrese la edad (solo números).", "OK");
                txtEdad.Focus();
                return;
            }
            if (edad < EdadMinima || edad > 100)
            {
                await DisplayAlertAsync("Dato inválido", $"La edad debe ser mayor o igual a {EdadMinima} (máximo 100).", "OK");
                txtEdad.Focus();
                return;
            }

            
            DateTime fecha = dtpFecha.Date ?? DateTime.Today;
            if (fecha.Date < DateTime.Today)
            {
                await DisplayAlertAsync("Dato inválido", "La fecha de inicio no puede ser una fecha pasada.", "OK");
                return;
            }

         
            if (pkPlan.SelectedIndex == -1)
            {
                await DisplayAlertAsync("Dato inválido", "Seleccione un plan.", "OK");
                return;
            }

            
            if (!TryCalcularPago(out decimal monto, out decimal pagoMensual, out string error))
            {
                await DisplayAlertAsync("Dato inválido", error, "OK");
                txtMonto.Focus();
                return;
            }
            txtPagoMensual.Text = pagoMensual.ToString("F2", CultureInfo.InvariantCulture);

            
            var socio = new Socio
            {
                Nombre = nombre,
                Apellido = apellido,
                Edad = edad,
                FechaInicio = fecha,
                Plan = pkPlan.Items[pkPlan.SelectedIndex],
                Casillero = swCasillero.IsToggled,
                MontoInicial = monto,
                PagoMensual = pagoMensual,
                PagoTotal = monto + (pagoMensual * NumeroCuotas)
            };

            await Navigation.PushAsync(new vResumen(_usuario, socio, this));
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"Ocurrió un error: {ex.Message}", "OK");
        }
    }

   
    private bool TryCalcularPago(out decimal monto, out decimal pagoMensual, out string error)
    {
        monto = 0;
        pagoMensual = 0;
        error = string.Empty;

        decimal minimo = CostoMembresia * PorcentajeMinimo;   
        decimal maximo = CostoMembresia * PorcentajeMaximo;   

        string texto = txtMonto.Text?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(texto))
        {
            error = "Ingrese el monto inicial.";
            return false;
        }

        
        texto = texto.Replace(',', '.');
        if (!decimal.TryParse(texto, NumberStyles.Number, CultureInfo.InvariantCulture, out monto))
        {
            error = "El monto inicial debe ser un valor numérico.";
            return false;
        }

        if (monto < minimo || monto > maximo)
        {
            error = $"El monto inicial debe estar entre {minimo:F0} y {maximo:F0}.";
            return false;
        }

        decimal cuota = (CostoMembresia - monto) / NumeroCuotas;
        pagoMensual = cuota + (CostoMembresia * PorcentajeCuota);
        if (swCasillero.IsToggled)
            pagoMensual += ExtraCasillero;

        return true;
    }

    
    private static bool EsTextoValido(string texto)
    {
        return !string.IsNullOrWhiteSpace(texto) && Regex.IsMatch(texto, @"^[\p{L} ]+$");
    }

    
    public void Limpiar()
    {
        txtNombre.Text = string.Empty;
        txtApellido.Text = string.Empty;
        txtEdad.Text = string.Empty;
        dtpFecha.Date = DateTime.Today;
        pkPlan.SelectedIndex = -1;
        swCasillero.IsToggled = false;
        txtMonto.Text = string.Empty;
        txtPagoMensual.Text = string.Empty;
    }
}
