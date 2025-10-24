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
using WinFormsAdmin.Forms.Careers;
using WinFormsAdmin.Services;

namespace WinFormsAdmin.Forms.Subjects
{
    public partial class FormSubjectManager : Form
    {
        private readonly ApiClient _apiClient;
        public FormSubjectManager(ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
            LoadSubjects();
        }

        private async void LoadSubjects()
        {
            dgvSubjects.AutoGenerateColumns = false;

            try
            {
                var subjects = await _apiClient.GetListAsync<SubjectResponseDto>("/api/Subjects");

                if (subjects != null && subjects.Count > 0)
                {
                    dgvSubjects.DataSource = subjects;

                    // Llenar manualmente la columna de materias
                    foreach (DataGridViewRow row in dgvSubjects.Rows)
                    {
                        var subject = (SubjectResponseDto)row.DataBoundItem;

                        if (subject.Careers != null && subject.Careers.Any())
                        {
                            row.Cells["colCareers"].Value = $"{subject.Careers.Count()} carrera(s)";
                        }
                        else
                        {
                            row.Cells["colCareers"].Value = "Sin carreras";
                        }
                    }
                }
                else
                {
                    dgvSubjects.DataSource = null;
                    MessageBox.Show("No hay materias registradas.", "Información",
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
                MessageBox.Show($"Error al cargar materias: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            var editForm = new FormSubjectEdit(_apiClient);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadSubjects(); // Recargar la lista
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvSubjects.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una materia para editar.");
                return;
            }

            var selected = (SubjectResponseDto)dgvSubjects.SelectedRows[0].DataBoundItem;
            var editForm = new FormSubjectEdit(_apiClient, selected.Id);

            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadSubjects();
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvSubjects.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una materia para eliminar.");
                return;
            }

            var selected = (CareerResponseDto)dgvSubjects.SelectedRows[0].DataBoundItem;
            var confirm = MessageBox.Show($"¿Está seguro de eliminar '{selected.Name}'?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    await _apiClient.DeleteAsync($"/api/Subjects/{selected.Id}");
                    MessageBox.Show("Materia eliminada correctamente.");
                    LoadSubjects();
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
            LoadSubjects();
        }

        private void dgvSubjects_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar que no sea el header
            if (e.RowIndex < 0) return;

            var columnName = dgvSubjects.Columns[e.ColumnIndex].Name;
            var selected = (SubjectResponseDto)dgvSubjects.Rows[e.RowIndex].DataBoundItem;

            if (columnName == "btnEdit")
            {
                var editForm = new FormSubjectEdit(_apiClient, selected.Id);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadSubjects();
                }
            }
            else if (columnName == "btnDelete")
            {
                DeleteSubject(selected);
            }
            else if (columnName == "btnManageCareers")
            {
                var manageForm = new FormManageSubjectCareers(_apiClient, selected.Id, selected.Name);
                if (manageForm.ShowDialog() == DialogResult.OK)
                {
                    LoadSubjects(); // Recargar para actualizar el contador
                }
            }
        }

        private async void DeleteSubject(SubjectResponseDto subject)
        {
            var confirm = MessageBox.Show($"¿Está seguro de eliminar '{subject.Name}'?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    bool success = await _apiClient.DeleteAsync($"/api/Subjects/{subject.Id}");
                    if (success)
                    {
                        MessageBox.Show("Materia eliminada correctamente.");
                        LoadSubjects();
                    }
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
