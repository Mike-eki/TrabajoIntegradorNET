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
using WinFormsAdmin.Forms.Careers;
using WinFormsAdmin.Forms.Subjects;
using WinFormsAdmin.Forms.Users;
using WinFormsAdmin.Forms.Commissions;
using WinFormsAdmin.Forms.Enrollments;

namespace WinFormsAdmin.Forms
{
    public partial class FormMain : Form
    {
        private readonly ApiClient _apiClient;
        private bool _logoutExecuted = false;

        public FormMain(ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
            this.FormClosed += FormMain_FormClosed;
        }

        private void menuCarreras_Click(object sender, EventArgs e)
        {
            // Verificar si ya hay una ventana de carreras abierta
            foreach (Form childForm in this.MdiChildren)
            {
                if (childForm is FormCareerManager)
                {
                    childForm.Activate();
                    return;
                }
            }

            // Crear nueva ventana
            var careerManager = new FormCareerManager(_apiClient)
            {
                MdiParent = this
            };
            careerManager.Show();
        }

        private void menuMaterias_Click(object sender, EventArgs e)
        {
            // Verificar si ya hay una ventana de materias abierta
            foreach (Form childForm in this.MdiChildren)
            {
                if (childForm is FormSubjectManager)
                {
                    childForm.Activate();
                    return;
                }
            }

            // Crear nueva ventana
            var careerManager = new FormSubjectManager(_apiClient)
            {
                MdiParent = this
            };
            careerManager.Show();
        }

        private void menuUsuarios_Click(object sender, EventArgs e)
        {
            // Verificar si ya hay una ventana de materias abierta
            foreach (Form childForm in this.MdiChildren)
            {
                if (childForm is FormUserManager)
                {
                    childForm.Activate();
                    return;
                }
            }

            // Crear nueva ventana
            var usersManager = new FormUserManager(_apiClient)
            {
                MdiParent = this
            };
            usersManager.Show();
        }

        private void menuComisiones_Click(object sender, EventArgs e)
        {
            
            foreach (Form childForm in this.MdiChildren)
            {
                if (childForm is FormCommissionManager)
                {
                    childForm.Activate();
                    return;
                }
            }

            // Crear nueva ventana
            var usersManager = new FormCommissionManager(_apiClient)
            {
                MdiParent = this
            };
            usersManager.Show();
        }

        private void menuEnrollments_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in this.MdiChildren)
            {
                if (childForm is FormCommissionManager)
                {
                    childForm.Activate();
                    return;
                }
            }

            // Crear nueva ventana
            var enrollManager = new FormEnrollManager(_apiClient)
            {
                MdiParent = this
            };
            enrollManager.Show();

        }

        private async void menuSalir_Click(object sender, EventArgs e)
        {
            await _apiClient.LogoutAsync();
            new FormLogin().Show();
            this.Close();
        }

        private async void FormMain_FormClosed(object? sender, FormClosedEventArgs e)
        {
            if (_logoutExecuted) return;
            _logoutExecuted = true;

            try
            {
                await _apiClient.LogoutAsync();
                Console.WriteLine("✅ Sesión cerrada correctamente al salir.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error al cerrar sesión: {ex.Message}");
            }
        }
    }
}
