﻿using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Calculadora
{
    public partial class MainWindow : Window
    {
        private string operacion;       //String con la operación a ejecutar
        DataTable dt = new DataTable();
        private bool operador = true;   //Controlar después de un igual si es nueva operacion o la misma
        private bool igual = false;     //Igual después de otro igual
        private string num;              //Ultimo numero introducido
        private string op;              //Ultimo operador introducido
        private string resultado;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void AgregarNumero(object sender, RoutedEventArgs e)
        {
            /*Compruebo si al agregar un nuevo numero se ha introducido un operador,
             en caso de que sea falso se comienza una nueva operación*/
            if (!operador) { txtResultado.Text = "0"; operador = true; }

            var boton = (Button)sender;

            if (txtResultado.Text.Equals("0")) txtResultado.Text = "";
            else if (txtResultado.Text.Equals("-0")) txtResultado.Text = "-";
            txtResultado.Text += boton.Content;
            //igual = false;
            HabilitarDeshabilitarBtn(true);
        }

        private void ClickOperador(object sender, RoutedEventArgs e)
        {
            var boton = (Button)sender;

            if (txtResultado.Text.Equals("Entrada no válida")) operacion += boton.Tag;
            else operacion += txtResultado.Text + boton.Tag;

            txtResultado.Text = "0";
            txtOperacion.Text = operacion;
            operador = true;
            op = boton.Tag.ToString();
            //igual = false;
        }

        private void BtnIgual_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (igual)
                {
                    igual = txtResultado.Text.Equals(resultado);
                    //tengo que comrpobar el operador y el numero
                    if (igual) operacion = txtResultado.Text + op + num;
                    else
                    {
                        if (txtResultado.Text.EndsWith(".")) txtResultado.Text = txtResultado.Text + "0";
                        operacion += txtResultado.Text;
                        num = txtResultado.Text;
                    }
                }
                else
                {
                    if (txtResultado.Text.EndsWith(".")) txtResultado.Text = txtResultado.Text + "0";
                    operacion += txtResultado.Text;
                    num = txtResultado.Text;
                }
                operacion = operacion.Replace(",", ".");
                txtOperacion.Text = operacion;
                /*Operacion necesaria para operar con numeros grandes o pequeños, sino da error por int32*/
                operacion = Regex.Replace(operacion, @"\d+(\.\d+)?", m =>
                {
                    var x = m.ToString();
                    return x.Contains(".") ? x : string.Format("{0}.0", x);
                });
                resultado = (Convert.ToDouble(dt.Compute(operacion, ""))).ToString("0.00");
                txtHistorial.Text = txtOperacion.Text + "=" + txtResultado.Text + "\n\n" + txtHistorial.Text;
                if (resultado.Substring(resultado.Length - 2, 2) == "00") resultado = resultado.Substring(0, resultado.Length - 3);
                resultado = resultado.Replace(",", ".");
                txtResultado.Text = resultado;
                operacion = "";
                operador = false;
                igual = true;
            }
            catch (DivideByZeroException)
            {
                txtResultado.Text = "No se puede dividir por 0";
                InterfazModoException();
            }
            catch (OverflowException)
            {
                txtResultado.Text = "Numero muy grande";
                InterfazModoException();
            }


        }

        private void InterfazModoException()
        {
            txtOperacion.Text = "";
            HabilitarDeshabilitarBtn(false);
            operacion = "";
            operador = false;
            igual = true;
        }

        private void BtnRaiz_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToDouble(txtResultado.Text) < 0)
            {
                operacion += txtResultado.Text;
                txtResultado.Text = "Entrada no válida";
            }
            else txtResultado.Text = Math.Sqrt(Convert.ToDouble(txtResultado.Text)).ToString();
            //igual = false;
        }

        private void BtnElevar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                operacion += txtResultado.Text;
                txtResultado.Text = Math.Pow(Convert.ToDouble(operacion), 2).ToString();
                //igual = false;
            }
            catch (FormatException)
            {
                txtResultado.Text = "Numero muy grande";
                txtOperacion.Text = "";
                HabilitarDeshabilitarBtn(false);
                operacion = "";
                operador = false;
            }

        }

        private void BtnC_Click(object sender, RoutedEventArgs e)
        {
            operacion = "";
            txtResultado.Text = "0";
            txtOperacion.Text = "";
            //igual = false;
            HabilitarDeshabilitarBtn(true);
        }

        private void BtnCE_Click(object sender, RoutedEventArgs e)
        {
            txtResultado.Text = "0";
            //igual = false;
            HabilitarDeshabilitarBtn(true);
        }

        private void BtnBorrar_Click(object sender, RoutedEventArgs e)
        {
            int indice = txtResultado.Text.Length - 1;
            if (txtResultado.Text.Equals("Entrada no válida")) txtResultado.Text = "0";
            if (indice > 0) txtResultado.Text = txtResultado.Text.Remove(indice, 1);
            else txtResultado.Text = "0";
            //igual = false;
        }

        private void BtnPorcentaje_Click(object sender, RoutedEventArgs e)
        {
            txtResultado.Text = (Convert.ToDouble(txtResultado.Text) / 100).ToString();
            //igual = false;
        }

        private void BtnMasMenos_Click(object sender, RoutedEventArgs e)
        {
            if (txtResultado.Text.Equals("Entrada no válida")) txtResultado.Text = operacion;
            if (txtResultado.Text.Contains("-")) txtResultado.Text = txtResultado.Text.Remove(0, 1);
            else txtResultado.Text = "-" + txtResultado.Text;
            //igual = false;
        }

        private void BtnPartido_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtResultado.Text = (1 / Convert.ToDouble(txtResultado.Text)).ToString();
                //igual = false;
            }
            catch (DivideByZeroException)
            {
                txtResultado.Text = "Numero muy grande";
                txtOperacion.Text = "";
                HabilitarDeshabilitarBtn(false);
                operacion = "";
                operador = false;
                igual = true;
            }
        }

        private void BtnPunto_Click(object sender, RoutedEventArgs e)
        {
            if (!txtResultado.Text.Contains(".")) txtResultado.Text += ".";
            //igual = false;
        }
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                BtnIgual_Click(sender, e);
            }
        }

        private void HabilitarDeshabilitarBtn(Boolean op)
        {
            if (op)
            {
                btnDividir.IsEnabled = true;
                btnMulti.IsEnabled = true;
                btnSuma.IsEnabled = true;
                btnResta.IsEnabled = true;
                btnRaiz.IsEnabled = true;
                btnElevar.IsEnabled = true;
                btnPorcentaje.IsEnabled = true;
                btnPartido.IsEnabled = true;
                btnMasMenos.IsEnabled = true;
                btnPunto.IsEnabled = true;
                btnBorrar.IsEnabled = true;
                btnIgual.IsEnabled = true;
            }
            else
            {
                btnDividir.IsEnabled = false;
                btnMulti.IsEnabled = false;
                btnSuma.IsEnabled = false;
                btnResta.IsEnabled = false;
                btnRaiz.IsEnabled = false;
                btnElevar.IsEnabled = false;
                btnPorcentaje.IsEnabled = false;
                btnPartido.IsEnabled = false;
                btnMasMenos.IsEnabled = false;
                btnPunto.IsEnabled = false;
                btnBorrar.IsEnabled = false;
                btnIgual.IsEnabled = false;
            }
        }

        private void BorrarHistorial(object sender, RoutedEventArgs e)
        {
            txtHistorial.Text = "";
        }
    }
}
