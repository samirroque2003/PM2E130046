using Microsoft.Maui.Controls;

namespace PM2E130046
{
    public partial class CalculadoraEnvios : ContentPage
    {
        public CalculadoraEnvios()
        {
            InitializeComponent();

            // Ocultar la tarjeta de resultados al iniciar la pantalla (Requerimiento 3)
            tarjetaResultado.IsVisible = false;
        }

        private async void btnCalcular_Clicked(object sender, EventArgs e)
        {
            // Validar que la descripción y el peso no estén vacíos, y mayor a 0
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                await DisplayAlert("Error", "Por favor ingrese una descripción del paquete.", "OK");
                txtDescripcion.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPeso.Text) || !double.TryParse(txtPeso.Text, out double peso) || peso <= 0)
            {
                await DisplayAlert("Error", "Por favor ingrese un peso válido mayor a 0.", "OK");
                txtPeso.Focus();
                return;
            }

            // Validar que el usuario haya seleccionado una zona en el Picker (SelectedIndex != -1)
            if (pckDestino.SelectedIndex == -1)
            {
                await DisplayAlert("Error", "Por favor seleccione una zona de destino.", "OK");
                pckDestino.Focus();
                return;
            }

            double tarifaBasePorKg = 0;
            string nombreZona = "";

            switch (pckDestino.SelectedIndex)
            {
                case 0: // Zona Local
                    tarifaBasePorKg = 50.00;
                    nombreZona = "Zona Local";
                    break;
                case 1: // Zona Nacional
                    tarifaBasePorKg = 85.00;
                    nombreZona = "Zona Nacional";
                    break;
                case 2: // Zona Internacional
                    tarifaBasePorKg = 150.00;
                    nombreZona = "Zona Internacional";
                    break;
            }

            // Calcular el costo: (Peso × Tarifa_de_Zona) + Recargo_Exprés
            double costoSubtotal = peso * tarifaBasePorKg;

            double recargoExpress = 0;
            string servicioExpressTexto = "No";

            if (chkServicioExpress.IsChecked)
            {
                recargoExpress = 100.00;
                servicioExpressTexto = "Sí (+ L 100.00)";
            }

            double costoTotal = costoSubtotal + recargoExpress;

            lblCostoTotal.Text = $"Costo Total: L {costoTotal:N2}";
            lblResumenDescripcion.Text = $"Descripción: {txtDescripcion.Text}\n" +
                                         $"Peso: {peso} kg | Destino: {nombreZona}\n" +
                                         $"Servicio Express: {servicioExpressTexto}";

            // --- Lógica de cambio dinámico de color (Requerimiento 3) ---
            Color colorFondo;

            if (costoTotal < 300.00)
            {
                // Menor a L 300.00 -> Fondo verde claro (#DCFCE7)
                colorFondo = Color.FromArgb("#DCFCE7");
            }
            else
            {
                // L 300.00 o más -> Fondo azul/gris claro (#E0F2FE)
                colorFondo = Color.FromArgb("#E0F2FE");
            }

            // Se limpian y aplican ambas propiedades para garantizar el cambio visual en MAUI
            tarjetaResultado.Background = new SolidColorBrush(colorFondo);
            tarjetaResultado.BackgroundColor = colorFondo;

            // Mostrar la tarjeta de resultados
            tarjetaResultado.IsVisible = true;
        }
    }
}