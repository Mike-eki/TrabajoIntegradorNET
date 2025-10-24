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

        private async void menuSalir_Click(object sender, EventArgs e)
        {
            await _apiClient.LogoutAsync();
            Application.Exit();
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
