using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsAdmin.Services;

namespace WinFormsAdmin.Forms
{
    public partial class FormLogin : Form
    {
        private readonly ApiClient _apiClient;

        public FormLogin()
        {
            InitializeComponent();
            _apiClient = new ApiClient();
            lblError.Visible = false;
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;
            btnLogin.Enabled = false;

            try
            {
                var (authResponse, errorMessage) = await _apiClient.LoginAsync(txtUsername.Text, txtPassword.Text);

                if (authResponse != null && authResponse.IsValid)
                {
                    if (authResponse.Role == "Admin")
                    {
                        // Login exitoso y es Admin
                        var mainForm = new FormMain(_apiClient);
                        mainForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        lblError.Text = "Solo administradores pueden acceder a este sistema.";
                        lblError.Visible = true;
                    }
                }
                else
                {
                    // Mostrar el mensaje de error del servidor
                    lblError.Text = errorMessage ?? "Credenciales inválidas";
                    lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = $"Error: {ex.Message}";
                lblError.Visible = true;
            }
            finally
            {
                btnLogin.Enabled = true;
            }
        }

        private void lblError_Click(object sender, EventArgs e)
        {

        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
