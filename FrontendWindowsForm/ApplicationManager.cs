using DTOs;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrontendWindowsForm
{
    public partial class ApplicationManager : Form
    {
        private LoginForm _loginForm;
        private BaseUserForm _currentForm;
        private bool _isTransitioningToLogin = false;

        public ApplicationManager()
        {
            InitializeComponent();

            // Hacer invisible el gestor
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Load += ApplicationManager_Load;
        }

        private void ApplicationManager_Load(object sender, EventArgs e)
        {
            ShowLoginForm();
        }

        public void ShowLoginForm()
        {
            if (_isTransitioningToLogin) return;

            _isTransitioningToLogin = true;
            // Cerrar formulario actual si existe
            _currentForm?.Close();
            _currentForm = null;

            // Crear o mostrar login
            if (_loginForm == null || _loginForm.IsDisposed)
            {
                _loginForm = new LoginForm(this); // Pasar referencia al gestor
            }

            _loginForm.Show();
            _loginForm.BringToFront();
            _isTransitioningToLogin = false;
        }

        public void ShowUserForm(UserDTO user)
        {
            // Ocultar login
            _loginForm?.Hide();

            // Cerrar formulario anterior si existe
            _currentForm?.Close();
            _currentForm = null;

            // Crear nuevo formulario según rol
            switch (user.RoleName)
            {
                case RoleType.Student:
                    _currentForm = new StudentForm(user, this);
                    break;
                case RoleType.Professor:
                    _currentForm = new ProfessorForm(user, this);
                    break;
                case RoleType.Administrator:
                    _currentForm = new AdministratorForm(user, this);
                    break;
                default:
                    MessageBox.Show("Rol no reconocido");
                    ShowLoginForm();
                    return;
            }

            _currentForm.FormClosed += (s, e) => {
                // Cuando se cierra el formulario de usuario, volver al login
                ShowLoginForm();
            };

            _currentForm.Show();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Application.Exit(); // Cerrar toda la aplicación cuando el gestor se cierre
        }
    }
}
