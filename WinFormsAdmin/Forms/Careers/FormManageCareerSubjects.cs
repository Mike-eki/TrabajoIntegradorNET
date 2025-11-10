using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsAdmin.Services;

namespace WinFormsAdmin.Forms.Careers
{
    public partial class FormManageCareerSubjects : Form
    {
        private readonly ApiClient _apiClient;
        private readonly int _careerId;
        private readonly string _careerName;
        private List<SubjectResponseDto> _availableSubjects = new();
        private List<int> _initialSubjectIds = new();

        public FormManageCareerSubjects(ApiClient apiClient, int careerId, string careerName)
        {
            InitializeComponent();
            _apiClient = apiClient;
            _careerId = careerId;
            _careerName = careerName;

            this.Text = "Gestionar Materias";
            lblCareerName.Text = $"Carrera: {_careerName}";
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await LoadSubjectsAsync();
            
        }

        private async Task LoadSubjectsAsync()
        {
            try
            {
                // 1️⃣ Obtener todas las materias
                _availableSubjects = await _apiClient.GetListAsync<SubjectResponseDto>("api/Subjects") ?? new();

                // 2️⃣ Obtener la carrera actual con sus materias
                var career = await _apiClient.GetAsync<CareerResponseDto>($"api/Careers/{_careerId}");
                _initialSubjectIds = career?.Subjects.Select(s => s.Id).ToList() ?? new();

                // 3️⃣ Poblar el CheckedListBox
                // 🔹 Llenar el CheckedListBox con actualización suspendida para evitar parpadeos
                clbSubjects.BeginUpdate();
                clbSubjects.Items.Clear();

                if (_availableSubjects != null && _availableSubjects.Any())
                {
                    foreach (var subject in _availableSubjects)
                    {
                        int index = clbSubjects.Items.Add(subject);
                        clbSubjects.SetItemChecked(index, _initialSubjectIds.Contains(subject.Id));
                    }
                }
                else
                {
                    MessageBox.Show("No hay materias disponibles.\nCree materias primero desde 'Gestión > Materias'.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                clbSubjects.DisplayMember = "Name";

                clbSubjects.EndUpdate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar materias: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;

            try
            {
                // Obtener las materias seleccionadas
                var selectedIds = clbSubjects.CheckedItems
                    .OfType<SubjectResponseDto>()
                    .Select(s => s.Id)
                    .ToList();

                // Si no hay cambios, salimos
                var areChanges = !selectedIds.Except(_initialSubjectIds).Any() && !_initialSubjectIds.Except(selectedIds).Any();

                if (areChanges)
                {
                    MessageBox.Show("No se ha aplicado ningún cambio.", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Llamada única al backend
                await UpdateSubjectsAssignedToCareerAsync(selectedIds);

                MessageBox.Show("Materias actualizadas correctamente.", "Éxito",
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
        /// Actualiza todas las materias asignadas a una carrera específica.
        /// </summary>
        private async Task UpdateSubjectsAssignedToCareerAsync(List<int> selectedSubjectIds)
        {
            //var request = new { subjectIds = selectedSubjectIds };
            await _apiClient.PutAsync($"api/Careers/{_careerId}/Subjects", selectedSubjectIds);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
