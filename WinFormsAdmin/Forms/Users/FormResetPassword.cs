using Models.DTOs;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsAdmin.Services;

namespace WinFormsAdmin.Forms.Users
{
    public partial class FormResetPassword : Form
    {
        private readonly ApiClient _apiClient;
        private readonly int _userId;
        private readonly string _username;

        public FormResetPassword(ApiClient apiClient, int userId, string username)
        {
            InitializeComponent();
            _apiClient = apiClient;
            _userId = userId;
            _username = username;

            lblUsername.Text = $"Usuario: {_username}";
        }

        private async void btnReset_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                MessageBox.Show("Debe ingresar una nueva contraseña.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var dto = new ResetPasswordDto { NewPassword = txtNewPassword.Text.Trim() };
                await _apiClient.PostAsync<object>($"api/Users/{_userId}/reset-password", dto);

                MessageBox.Show("Contraseña restablecida correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al restablecer contraseña: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            txtNewPassword.Text = Guid.NewGuid().ToString("N")[..8];
        }
    }
}
