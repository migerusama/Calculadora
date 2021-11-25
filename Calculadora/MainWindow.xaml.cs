using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Calculadora
{
    public partial class MainWindow : Window
    {
        String operacion;
        DataTable dt = new DataTable();
        private bool o=true;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void agregarNumero(object sender, RoutedEventArgs e)
        {
            // ?¿
            if (o) { }
            else txtResultado.Text = "0";
            var boton = (Button)sender;
            if (txtResultado.Text == "0") txtResultado.Text = "";
            txtResultado.Text += boton.Content;

        }

        private void clickOperador(object sender, RoutedEventArgs e)
        {
            var boton = (Button)sender;

            if (txtResultado.Text.Equals("Entrada no válida")) operacion += boton.Tag;
            else operacion += txtResultado.Text + boton.Tag;

            txtResultado.Text = "0";
            txtOperacion.Text = operacion;
            o = true;
        }

        private void btnIgual_Click(object sender, RoutedEventArgs e)
        {
            operacion += txtResultado.Text;
            operacion = operacion.Replace(",", ".");
            txtResultado.Text = dt.Compute(operacion, "").ToString();
            txtOperacion.Text = operacion;
            operacion = "";
            o = false;
        }

        private void btnRaiz_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToDouble(txtResultado.Text) < 0)
            {
                operacion += txtResultado.Text;
                txtResultado.Text = "Entrada no válida";
            }
            else txtResultado.Text = Math.Sqrt(Convert.ToDouble(txtResultado.Text)).ToString();
        }

        private void btnElevar_Click(object sender, RoutedEventArgs e)
        {
            operacion += txtResultado.Text;
            txtResultado.Text = Math.Pow(Convert.ToDouble(operacion), 2).ToString();
        }

        private void btnC_Click(object sender, RoutedEventArgs e)
        {
            operacion = "";
            txtResultado.Text = "0";
            txtOperacion.Text = "";
        }

        private void btnCE_Click(object sender, RoutedEventArgs e)
        {
            txtResultado.Text = "0";
        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            int indice = txtResultado.Text.Length - 1;
            if (txtResultado.Equals("Entrada no válida")) txtResultado.Text = "0";
            if (indice > 0) txtResultado.Text = txtResultado.Text.Remove(indice, 1);
            else txtResultado.Text = "0";
        }

        private void btnPorcentaje_Click(object sender, RoutedEventArgs e)
        {
            txtResultado.Text = (Convert.ToDouble(txtResultado.Text) / 100).ToString();
        }

        private void btnMasMenos_Click(object sender, RoutedEventArgs e)
        {
            if (txtResultado.Text.Contains("-")) txtResultado.Text = txtResultado.Text.Remove(0, 1);
            else
            {
                txtResultado.Text = "-" + txtResultado.Text;
            }
                
        }

        private void btnPartido_Click(object sender, RoutedEventArgs e)
        {
            txtResultado.Text = (1 / Convert.ToDouble(txtResultado.Text)).ToString();
        }

        private void btnPunto_Click(object sender, RoutedEventArgs e)
        {
            if(!txtResultado.Text.Contains(".")) txtResultado.Text += ".";
        }
    }
}
