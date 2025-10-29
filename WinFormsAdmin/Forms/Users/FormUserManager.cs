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
                    foreach (var user in users)
                    {
                        try
                        {
                            // ✅ Solo si el usuario es Student
                            if (user.Role.Equals("Student", StringComparison.OrdinalIgnoreCase))
                            {
                                var careers = await _apiClient.GetListAsync<CareerResponseDto>($"/api/Users/{user.Id}/careers");

                                if (careers != null && careers.Any())
                                    user.CareersSummary = $"({careers.Count()}) carrera/s";
                                else
                                    user.CareersSummary = "Sin carreras";
                            }
                            else
                            {
                                user.CareersSummary = "-"; // o "No aplica"
                            }
                        }
                        catch
                        {
                            user.CareersSummary = "Error al cargar";
                        }
                    }

                    dgvUsers.DataSource = null;
                    dgvUsers.DataSource = users;

                    // 🔄 Rellenar la columna de carreras manualmente (si es columna custom)
                    foreach (DataGridViewRow row in dgvUsers.Rows)
                    {
                        var user = (UserResponseDto)row.DataBoundItem;
                        row.Cells["colCareers"].Value = user.CareersSummary;
                    }
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
                await _apiClient.DeleteAsync($"api/Users/{user.Id}");
                
                
                MessageBox.Show("Usuario eliminado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsers();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar usuario: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
