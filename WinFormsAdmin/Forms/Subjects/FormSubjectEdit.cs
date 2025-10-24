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
using System.Xml.Linq;
using WinFormsAdmin.Services;

namespace WinFormsAdmin.Forms.Subjects
{
    public partial class FormSubjectEdit : Form
    {

        private readonly ApiClient _apiClient;
        private static int? _subjectId;

        // Constructor para Crear
        public FormSubjectEdit(ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
            _subjectId = null;
            this.Text = "Crear Materia";
        }

        // Constructor para Editar
        public FormSubjectEdit(ApiClient apiClient, int subjectId)
        {
            InitializeComponent();
            _apiClient = apiClient;
            _subjectId = subjectId;
            this.Text = "Editar Materia";
            LoadSubject();
        }

        private async void LoadSubject()
        {
            try
            {
                var subject = await _apiClient.GetAsync<SubjectResponseDto>($"/api/Subjects/{_subjectId}");
                if (subject != null)
                {
                    txtName.Text = subject.Name;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la materia: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("El nombre de la materia es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnSave.Enabled = false;

            try
            {
                var subjectDto = new SubjectSimpleDto { Name = txtName.Text.Trim() };

                if (_subjectId.HasValue)
                {
                    // Actualizar
                    bool success = await _apiClient.PutAsync($"/api/Subjects/{_subjectId}", subjectDto);
                    if (success)
                    {
                        MessageBox.Show("Materia actualizada correctamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Error al actualizar la Materia.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Crear
                    var created = await _apiClient.PostAsync<SubjectResponseDto>("/api/Subjects", subjectDto);
                    if (created != null)
                    {
                        MessageBox.Show("Materia creada correctamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Error al crear la Materia.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSave.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
