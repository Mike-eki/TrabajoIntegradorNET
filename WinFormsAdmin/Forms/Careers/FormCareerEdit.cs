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
    public partial class FormCareerEdit : Form
    {
        private readonly ApiClient _apiClient;
        private readonly int? _careerId;

        // Constructor para CREAR
        public FormCareerEdit(ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
            _careerId = null;
            this.Text = "Nueva Carrera";
        }

        // Constructor para EDITAR
        public FormCareerEdit(ApiClient apiClient, int careerId)
        {
            InitializeComponent();
            _apiClient = apiClient;
            _careerId = careerId;
            this.Text = "Editar Carrera";
            LoadCareer();
        }

        private async void LoadCareer()
        {
            try
            {
                var career = await _apiClient.GetAsync<CareerResponseDto>($"/api/Careers/{_careerId}");
                if (career != null)
                {
                    txtName.Text = career.Name;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la carrera: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("El nombre de la carrera es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnSave.Enabled = false;

            try
            {
                var careerDto = new CareerCreateDto { Name = txtName.Text.Trim() };

                if (_careerId.HasValue)
                {
                    // Actualizar
                    bool success = await _apiClient.PutAsync($"/api/Careers/{_careerId}", careerDto);
                    if (success)
                    {
                        MessageBox.Show("Carrera actualizada correctamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Error al actualizar la carrera.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Crear
                    var created = await _apiClient.PostAsync<CareerResponseDto>("/api/Careers", careerDto);
                    if (created != null)
                    {
                        MessageBox.Show("Carrera creada correctamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Error al crear la carrera.", "Error",
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
