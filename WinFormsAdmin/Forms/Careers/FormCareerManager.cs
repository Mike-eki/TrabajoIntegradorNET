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
using Models.DTOs;

namespace WinFormsAdmin.Forms.Careers
{
    public partial class FormCareerManager : Form
    {
        private readonly ApiClient _apiClient;
        public FormCareerManager(ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
            LoadCareers();
        }

        private async void LoadCareers()
        {
            dgvCareers.AutoGenerateColumns = false;

            try
            {
                var careers = await _apiClient.GetListAsync<CareerResponseDto>("/api/Careers");

                if (careers != null && careers.Count > 0)
                {
                    dgvCareers.DataSource = careers;

                    // Llenar manualmente la columna de materias
                    foreach (DataGridViewRow row in dgvCareers.Rows)
                    {
                        var career = (CareerResponseDto)row.DataBoundItem;

                        if (career.Subjects != null && career.Subjects.Any())
                        {
                            row.Cells["colSubjects"].Value = $"{career.Subjects.Count()} materia(s)";
                        }
                        else
                        {
                            row.Cells["colSubjects"].Value = "Sin materias";
                        }
                    }
                }
                else
                {
                    dgvCareers.DataSource = null;
                    MessageBox.Show("No hay carreras registradas.", "Información",
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
                MessageBox.Show($"Error al cargar carreras: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            var editForm = new FormCareerEdit(_apiClient);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadCareers(); // Recargar la lista
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvCareers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una carrera para editar.");
                return;
            }

            var selected = (CareerResponseDto)dgvCareers.SelectedRows[0].DataBoundItem;
            var editForm = new FormCareerEdit(_apiClient, selected.Id);

            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadCareers();
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCareers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una carrera para eliminar.");
                return;
            }

            var selected = (CareerResponseDto)dgvCareers.SelectedRows[0].DataBoundItem;
            var confirm = MessageBox.Show($"¿Está seguro de eliminar '{selected.Name}'?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    await _apiClient.DeleteAsync($"/api/Careers/{selected.Id}");
                    MessageBox.Show("Carrera eliminada correctamente.");
                    LoadCareers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadCareers();
        }

        private void dgvCareers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar que no sea el header
            if (e.RowIndex < 0) return;

            var columnName = dgvCareers.Columns[e.ColumnIndex].Name;
            var selected = (CareerResponseDto)dgvCareers.Rows[e.RowIndex].DataBoundItem;

            if (columnName == "btnEdit")
            {
                var editForm = new FormCareerEdit(_apiClient, selected.Id);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadCareers();
                }
            }
            else if (columnName == "btnDelete")
            {
                DeleteCareer(selected);
            }
            else if (columnName == "btnManageSubjects")
            {
                var manageForm = new FormManageCareerSubjects(_apiClient, selected.Id, selected.Name);
                if (manageForm.ShowDialog() == DialogResult.OK)
                {
                    LoadCareers(); // Recargar para actualizar el contador
                }
            }
        }

        private async void DeleteCareer(CareerResponseDto career)
        {
            var confirm = MessageBox.Show($"¿Está seguro de eliminar '{career.Name}'?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    await _apiClient.DeleteAsync($"/api/Careers/{career.Id}");

                    MessageBox.Show("Carrera eliminada correctamente.");
                    LoadCareers();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
