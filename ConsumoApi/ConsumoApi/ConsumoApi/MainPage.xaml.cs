using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ConsumoApi
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private string Url = "https://cuentas2.azurewebsites.net/api/cliente/";
        private Cliente[] Clientes { get; set; }
        private void Btn_read(object sender, EventArgs e)
        {
            using (var wc = new WebClient())
            {
                
                wc.Headers.Add("Content-Type", "application/json");
                var json = wc.DownloadString(Url+ cedula.Text);
                var cliente = Newtonsoft.Json.JsonConvert.DeserializeObject<Cliente>(json);
                if (cliente != null)
                {
                    cedula.Text = cliente.cedulaCli;
                    nombre.Text = cliente.nombresCli;
                    totalPa.Text = cliente.totalPagarCre.ToString();
                    saldo.Text = cliente.saldo.ToString();
                    debe.Text = cliente.debe.ToString();
                    lblRes.Text = "Cédula del Cliente: "+ cliente.cedulaCli;
                }
                else
                {
                    cedula.Text = "";
                    nombre.Text = "";
                    totalPa.Text = "";
                    saldo.Text = "";
                    debe.Text = "";
                }
            }
        }
        private void Btn_create(object sender, EventArgs e)
        {
            using (var wc = new WebClient())
            {
                wc.Headers.Add("Content-Type", "application/json");
                var datos = new Cliente
                {
                    cedulaCli = cedula.Text,
                    nombresCli = nombre.Text,
                    totalPagarCre = float.Parse(totalPa.Text),
                    debe = float.Parse(debe.Text),
                    saldo = float.Parse(saldo.Text)
                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(datos);
                lblRes.Text = "Datos Ingresados";
            }
        }
        private void Btn_update(object sender, EventArgs e)
        {
            using (var wc = new WebClient())
            {
                wc.Headers.Add("Content-Type", "application/json");

                var datos = new Cliente
                {
                    cedulaCli = cedula.Text,
                    nombresCli = nombre.Text,
                    totalPagarCre = float.Parse(totalPa.Text),
                    debe = float.Parse(debe.Text),
                    saldo = float.Parse(saldo.Text)
                };
                var json = JsonConvert.SerializeObject(datos);
                try
                {
                    var response = wc.UploadString(Url,  "POST", json);
                    lblRes.Text = "Datos actualizados correctamente";
                }
                catch (WebException)
                {
                    lblRes.Text = "Error al actualizar";
                }
            }
        }
        private void Btn_delete(object sender, EventArgs e)
        {
            using (var wc = new WebClient())
            {
                wc.Headers.Add("Content-Type", "application/json");
                wc.UploadString(Url + "/" + cedula.Text, "DELETE", "");
                lblRes.Text = "Datos eliminados correctamente";
                cedula.Text = "";
                nombre.Text = "";
                totalPa.Text = "";
                saldo.Text = "";
                debe.Text = "";
            }
        }
    }
}
