using Models.DTOs;
using Services;
using System;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using WinFormsAdmin.Forms.Careers;
using WinFormsAdmin.Forms.Enrollments;
using WinFormsAdmin.Services;

namespace WinFormsAdmin.Forms.Commissions
{
    public partial class FormCommissionManager : Form
    {
        private readonly ApiClient _apiClient;

        public FormCommissionManager(ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
            LoadCommissions();
        }

        private async void LoadCommissions()
        {
            dgvCommissions.AutoGenerateColumns = false;

            try
            {
                var commissions = await _apiClient.GetListAsync<CommissionWithProfessorDto>("/api/Commissions");

                if (commissions != null && commissions.Any())
                {
                    dgvCommissions.DataSource = commissions;
                }
                else
                {
                    dgvCommissions.DataSource = null;
                    MessageBox.Show("No hay comisiones registradas.", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error de conexión con la API: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_Enrollments_Commissions"))
                    MessageBox.Show("No se puede eliminar la comisión porque tiene inscripciones asociadas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show($"Error al eliminar comisión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            var editForm = new FormCommissionEdit(_apiClient);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadCommissions(); // Recargar la lista
            }
        }

        private void dgvCommissions_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var columnName = dgvCommissions.Columns[e.ColumnIndex].Name;
            var selected = (CommissionWithProfessorDto)dgvCommissions.Rows[e.RowIndex].DataBoundItem;
            var commissionName = CommissionHelper.GenerateName(selected.SubjectName, selected.CycleYear, selected.Id);

            if (columnName == "btnEdit")
            {
                var editForm = new FormCommissionEdit(_apiClient, selected.Id);
                if (editForm.ShowDialog() == DialogResult.OK)
                    LoadCommissions();
            }
            else if (columnName == "btnEnrollStudent")
            {
                var enrollForm = new FormEnrollStudents(_apiClient, selected.Id, selected.SubjectId, commissionName);
                if (enrollForm.ShowDialog() == DialogResult.OK)
                    LoadCommissions(); // refresca capacidad visible

            }
            else if (columnName == "btnDelete")
            {
                DeleteCommission(selected);
            }
        }

        private async void DeleteCommission(CommissionWithProfessorDto commission)
        {
            var confirm = MessageBox.Show($"¿Está seguro de eliminar la comisión '{commission.SubjectName}'?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    await _apiClient.DeleteAsync($"/api/Commissions/{commission.Id}");

                    MessageBox.Show("Comisión eliminada correctamente.");
                    LoadCommissions();

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
            LoadCommissions();
        }
    }
}
