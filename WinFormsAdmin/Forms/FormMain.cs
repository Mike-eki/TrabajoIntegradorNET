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

namespace WinFormsAdmin.Forms
{
    public partial class FormMain : Form
    {
        private readonly ApiClient _apiClient;

        public FormMain(ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
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

        private async void menuSalir_Click(object sender, EventArgs e)
        {
            await _apiClient.LogoutAsync();
            Application.Exit();
        }
    }
}
