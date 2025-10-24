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

namespace WinFormsAdmin.Forms.Subjects
{
    public partial class FormManageSubjectCareers : Form
    {

        private readonly ApiClient _apiClient;
        private readonly int _subjectId;
        private readonly string _subjectName;
        private List<CareerResponseDto> _availableCareers = new();
        private List<int> _initialCareerIds = new();

        public FormManageSubjectCareers(ApiClient apiClient, int subjectId, string subjectName)
        {
            InitializeComponent();
            _apiClient = apiClient;
            _subjectId = subjectId;
            _subjectName = subjectName;

            this.Text = "Gestionar Carreras";
            lblSubjectName.Text = $"Materia: {_subjectName}";
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            clbCareers.DisplayMember = "Name";
            await LoadSubjectsAsync();
        }

        private async Task LoadSubjectsAsync()
        {
            try
            {
                // 1️⃣ Obtener todas las carreras
                _availableCareers = await _apiClient.GetListAsync<CareerResponseDto>("api/Careers") ?? new();

                // 2️⃣ Obtener la materia actual con sus carreras
                var subject = await _apiClient.GetAsync<SubjectResponseDto>($"api/Subjects/{_subjectId}");
                _initialCareerIds = subject?.Careers.Select(c => c.Id).ToList() ?? new();

                // 3️⃣ Poblar el CheckedListBox
                // 🔹 Llenar el CheckedListBox con actualización suspendida para evitar parpadeos
                clbCareers.BeginUpdate();
                clbCareers.Items.Clear();

                if (_availableCareers != null && _availableCareers.Any())
                {
                    foreach (var career in _availableCareers)
                    {
                        int index = clbCareers.Items.Add(career);
                        clbCareers.SetItemChecked(index, _initialCareerIds.Contains(career.Id));
                    }
                }
                else
                {
                    MessageBox.Show("No hay carreras disponibles.\nCree carreras primero desde 'Gestión > Carreras'.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                clbCareers.EndUpdate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar carreras: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;

            try
            {
                // Obtener las carreras seleccionadas
                var selectedIds = clbCareers.CheckedItems
                    .OfType<CareerResponseDto>()
                    .Select(c => c.Id)
                    .ToList();

                // Si no hay cambios, salimos
                var areChanges = !selectedIds.Except(_initialCareerIds).Any() && !_initialCareerIds.Except(selectedIds).Any();

                if (areChanges)
                {
                    MessageBox.Show("No se ha aplicado ningún cambio.", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Llamada única al backend
                await UpdateCareersAssignedToSubjectAsync(selectedIds);

                MessageBox.Show("Carreras actualizadas correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSave.Enabled = true;
            }
        }

        /// <summary>
        /// Actualiza todas las carreras asignadas a una materia específica.
        /// </summary>
        private async Task UpdateCareersAssignedToSubjectAsync(List<int> selectedCareerIds)
        {
            await _apiClient.PutAsync($"api/Subjects/{_subjectId}/Careers", selectedCareerIds);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}

