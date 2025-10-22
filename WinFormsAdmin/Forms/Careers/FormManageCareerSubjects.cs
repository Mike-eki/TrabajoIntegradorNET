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

namespace WinFormsAdmin.Forms.Careers
{
    public partial class FormManageCareerSubjects : Form
    {
        private readonly ApiClient _apiClient;
        private readonly int _careerId;
        private readonly string _careerName;
        private List<SubjectResponseDto> _allSubjects;
        private List<int> _initialSubjectIds;

        public FormManageCareerSubjects(ApiClient apiClient, int careerId, string careerName)
        {
            InitializeComponent();
            _apiClient = apiClient;
            _careerId = careerId;
            _careerName = careerName;

            this.Text = "Gestionar Materias";
            lblCareerName.Text = $"Carrera: {_careerName}";

            LoadSubjects();
        }

        private async void LoadSubjects()
        {
            try
            {
                // Cargar todas las materias disponibles
                _allSubjects = await _apiClient.GetListAsync<SubjectResponseDto>("/api/Subjects");

                // Cargar la carrera actual para saber qué materias ya tiene
                var career = await _apiClient.GetAsync<CareerResponseDto>($"/api/Careers/{_careerId}");
                _initialSubjectIds = career.Subjects.Select(s => s.Id).ToList();

                // Llenar el CheckedListBox
                clbSubjects.Items.Clear();

                if (_allSubjects != null && _allSubjects.Any())
                {
                    foreach (var subject in _allSubjects)
                    {
                        // Agregar la materia
                        int index = clbSubjects.Items.Add(subject);

                        // Marcarla si ya está asociada a la carrera
                        if (_initialSubjectIds.Contains(subject.Id))
                        {
                            clbSubjects.SetItemChecked(index, true);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No hay materias disponibles en el sistema.\n\nCree materias primero desde el menú 'Gestión > Materias'.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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
                // Obtener IDs de las materias seleccionadas
                var selectedSubjectIds = new List<int>();
                foreach (var item in clbSubjects.CheckedItems)
                {
                    var subject = (SubjectResponseDto)item;
                    selectedSubjectIds.Add(subject.Id);
                }

                // Determinar qué materias agregar y cuáles quitar
                var toAdd = selectedSubjectIds.Except(_initialSubjectIds).ToList();
                var toRemove = _initialSubjectIds.Except(selectedSubjectIds).ToList();

                // IMPORTANTE: Aquí necesitamos actualizar la relación.
                // Opción 1: Si tu API tiene un endpoint específico para esto
                // Opción 2: Actualizar cada Subject para incluir/excluir esta Career

                // Para esta implementación, usaremos la Opción 2:
                // Actualizamos cada Subject afectado

                foreach (var subjectId in toAdd)
                {
                    await AddCareerToSubject(subjectId);
                }

                foreach (var subjectId in toRemove)
                {
                    await RemoveCareerFromSubject(subjectId);
                }

                MessageBox.Show("Materias actualizadas correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
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

        private async Task AddCareerToSubject(int subjectId)
        {
            // Obtener el Subject actual
            var subject = await _apiClient.GetAsync<SubjectResponseDto>($"/api/Subjects/{subjectId}");

            if (subject != null)
            {
                // Crear lista de IDs de carreras (las que ya tiene + la nueva)
                var careerIds = subject.Careers.Select(c => c.Id).ToList();

                if (!careerIds.Contains(_careerId))
                {
                    careerIds.Add(_careerId);
                }

                // Actualizar el Subject con el nuevo array de careerIds
                var updateDto = new SubjectCreateDto
                {
                    Name = subject.Name,
                    CareerIds = careerIds.ToArray()
                };

                await _apiClient.PutAsync($"/api/Subjects/{subjectId}", updateDto);
            }
        }

        private async Task RemoveCareerFromSubject(int subjectId)
        {
            // Obtener el Subject actual
            var subject = await _apiClient.GetAsync<SubjectResponseDto>($"/api/Subjects/{subjectId}");

            if (subject != null)
            {
                // Crear lista de IDs de carreras sin la que queremos quitar
                var careerIds = subject.Careers
                    .Where(c => c.Id != _careerId)
                    .Select(c => c.Id)
                    .ToList();

                // Actualizar el Subject
                var updateDto = new SubjectCreateDto
                {
                    Name = subject.Name,
                    CareerIds = careerIds.ToArray()
                };

                await _apiClient.PutAsync($"/api/Subjects/{subjectId}", updateDto);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Override del DisplayMember para el CheckedListBox
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            clbSubjects.DisplayMember = "Name";
        }
    }
}

