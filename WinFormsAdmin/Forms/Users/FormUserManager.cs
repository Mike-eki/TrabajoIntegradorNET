using Models.DTOs;
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

namespace WinFormsAdmin.Forms.Users
{
    public partial class FormUserManager : Form
    {
        private readonly ApiClient _apiClient = new ApiClient();
        public FormUserManager(ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
            LoadUsers();
        }

        private async void LoadUsers()
        {
            dgvUsers.AutoGenerateColumns = false;

            try
            {
                var users = await _apiClient.GetListAsync<UserResponseDto>("api/Users");

                if (users != null && users.Any())
                {
                    dgvUsers.DataSource = users;
                }
                else
                {
                    dgvUsers.DataSource = null;
                    MessageBox.Show("No hay usuarios registrados.", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error de conexión con la API: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnNew_Click(object sender, EventArgs e)
        {
            using var form = new FormUserEdit(_apiClient);
            if (form.ShowDialog() == DialogResult.OK)
                LoadUsers();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }


        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var columnName = dgvUsers.Columns[e.ColumnIndex].Name;
            var selected = (UserResponseDto)dgvUsers.Rows[e.RowIndex].DataBoundItem;

            switch (columnName)
            {
                case "btnEdit":
                    using (var editForm = new FormUserEdit(_apiClient, selected.Id))
                    {
                        if (editForm.ShowDialog() == DialogResult.OK)
                            LoadUsers();
                    }
                    break;

                case "btnDelete":
                    DeleteUser(selected);
                    break;

                case "btnResetPassword":
                    using (var resetForm = new FormResetPassword(_apiClient, selected.Id, selected.Username))
                    {
                        if (resetForm.ShowDialog() == DialogResult.OK)
                            MessageBox.Show("Contraseña restablecida correctamente.");
                    }
                    break;
            }
        }

        private async void DeleteUser(UserResponseDto user)
        {
            var confirm = MessageBox.Show(
                $"¿Está seguro de eliminar el usuario '{user.Username}'?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm != DialogResult.Yes) return;

            try
            {
                bool success = await _apiClient.DeleteAsync($"api/Users/{user.Id}");
                if (success)
                {
                    MessageBox.Show("Usuario eliminado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar usuario: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
