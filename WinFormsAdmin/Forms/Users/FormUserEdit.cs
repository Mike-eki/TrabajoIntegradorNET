using Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsAdmin.Services;

namespace WinFormsAdmin.Forms.Users
{
    public partial class FormUserEdit : Form
    {
        private readonly ApiClient _apiClient;
        private readonly int? _userId;
        private readonly bool _isNew;
        public FormUserEdit(ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
            _isNew = true;
            this.Text = "Crear nuevo usuario";
        }

        public FormUserEdit(ApiClient apiClient, int userId)
        {
            InitializeComponent();
            _apiClient = apiClient;
            _userId = userId;
            _isNew = false;
            this.Text = "Editar usuario existente";
            txtInitialPassword.Enabled = false;
            lblInitialPassword.Enabled = false;
            btnGeneratePassword.Enabled = false;
            LoadUserData();
        }

        private async void FormUserEdit_Load(object sender, EventArgs e)
        {
            var careers = await _apiClient.GetListAsync<CareerResponseDto>("/api/Careers");
            clbCareers.Items.Clear();

            foreach (var c in careers)
                clbCareers.Items.Add(c);

            clbCareers.DisplayMember = "Name";

            // Si estamos editando un usuario existente
            if (!_isNew && _userId.HasValue)
            {
                var user = await _apiClient.GetAsync<UserResponseDto>($"/api/Users/{_userId}");

                // Verificar si es Student
                bool isStudent = user.Role.Equals("Student", StringComparison.OrdinalIgnoreCase);

                // Deshabilitar si no lo es
                clbCareers.Enabled = isStudent;
                gbCareers.Text = isStudent ? "Carreras:" : "Carreras (solo aplicable a estudiantes)";
                gbCareers.ForeColor = isStudent ? Color.Black : Color.Gray;

                // Si es estudiante, marcar sus carreras actuales
                if (isStudent)
                {
                    var userCareers = await _apiClient.GetListAsync<CareerResponseDto>($"/api/Users/{_userId}/careers");

                    clbCareers.BeginUpdate();
                    for (int i = 0; i < clbCareers.Items.Count; i++)
                    {
                        var career = (CareerResponseDto)clbCareers.Items[i];
                        clbCareers.SetItemChecked(i, userCareers.Any(c => c.Id == career.Id));
                    }
                    clbCareers.EndUpdate();
                }

                // Suscribirse a cambios de rol si se edita el rol manualmente
                cmbRole.SelectedIndexChanged += (s, ev) => ToggleCareersControl(cmbRole.SelectedItem?.ToString());
            }
            else
            {
                // En modo creación: configurar comportamiento inicial
                bool isStudent = cmbRole.SelectedItem?.ToString() == "Student";
                clbCareers.Enabled = isStudent;
                gbCareers.Text = isStudent ? "Carreras:" : "Carreras (solo aplicable a estudiantes)";
                gbCareers.ForeColor = isStudent ? Color.Black : Color.Gray;

                // Escuchar cambios del rol en tiempo real
                cmbRole.SelectedIndexChanged += (s, ev) => ToggleCareersControl(cmbRole.SelectedItem?.ToString());
            }
        }

        private void ToggleCareersControl(string? selectedRole)
        {
            bool isStudent = selectedRole?.Equals("Student", StringComparison.OrdinalIgnoreCase) ?? false;

            // Cambiar estado del control
            clbCareers.Enabled = isStudent;
            gbCareers.Text = isStudent ? "Carreras:" : "Carreras (solo aplicable a estudiantes)";
            gbCareers.ForeColor = isStudent ? Color.Black : Color.Gray;

            // Si NO es estudiante, limpiar selecciones
            if (!isStudent)
            {
                clbCareers.BeginUpdate();
                for (int i = 0; i < clbCareers.Items.Count; i++)
                    clbCareers.SetItemChecked(i, false);
                clbCareers.EndUpdate();
            }
        }



        private async void LoadUserData()
        {
            try
            {
                var user = await _apiClient.GetAsync<UserResponseDto>($"api/Users/{_userId}");

                if (user == null)
                {
                    MessageBox.Show("No se encontró el usuario.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }

                txtUsername.Text = user.Username;
                txtLegajo.Text = user.Legajo;
                txtFullName.Text = user.FullName;
                txtEmail.Text = user.Email;
                cmbRole.SelectedItem = user.Role;

                txtUsername.ReadOnly = true;
                txtLegajo.ReadOnly = true;
                txtInitialPassword.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuario: {ex.Message}");
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            if (_isNew)
                await CreateUserAsync();
            else
                await UpdateUserAsync();
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("El nombre de usuario es obligatorio.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("El nombre completo es obligatorio.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("El correo electrónico es obligatorio.");
                return false;
            }

            try
            {
                // Validar formato de email
                var _ = new MailAddress(txtEmail.Text);
            }
            catch
            {
                MessageBox.Show("El correo electrónico no tiene un formato válido.");
                return false;
            }

            if (cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un rol para el usuario.");
                return false;
            }

            if (_isNew && string.IsNullOrWhiteSpace(txtInitialPassword.Text))
            {
                var confirm = MessageBox.Show(
                    "No ingresó una contraseña. ¿Desea generar una automáticamente?",
                    "Generar contraseña",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    txtInitialPassword.Text = Guid.NewGuid().ToString("N")[..8];
                }
                else return false;
            }

            return true;
        }

        private async Task CreateUserAsync()
        {
            try
            {
                var newUser = new UserCreateDto
                {
                    Username = txtUsername.Text.Trim(),
                    Password = txtInitialPassword.Text.Trim(),
                    Legajo = txtLegajo.Text.Trim(),
                    FullName = txtFullName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Role = cmbRole.SelectedItem?.ToString() ?? "Student"
                };

                await _apiClient.PostAsync<UserResponseDto>("api/Users", newUser);
                MessageBox.Show("Usuario creado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear usuario: {ex.Message}");
            }
        }

        private async Task UpdateUserAsync()
        {
            try
            {
                var updatedUser = new UserUpdateDto
                {
                    FullName = txtFullName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Role = cmbRole.SelectedItem?.ToString() ?? "Student"
                };

                await _apiClient.PutAsync($"api/Users/{_userId}", updatedUser);
                var selectedIds = clbCareers.CheckedItems.Cast<CareerResponseDto>().Select(c => c.Id).ToList();
                //await _apiClient.PutAsync<object>($"/api/Users/{_userId}/careers", selectedIds);
                await _apiClient.PutAsync($"/api/Users/{_userId}/careers", selectedIds);

                MessageBox.Show("Usuario actualizado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar usuario: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnGeneratePassword_Click(object sender, EventArgs e)
        {
            txtInitialPassword.Text = Guid.NewGuid().ToString("N")[..8];
        }

    }
}
