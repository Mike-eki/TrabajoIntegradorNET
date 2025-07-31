using DTOs;
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
    public partial class StudentForm : BaseUserForm
    {
        public StudentForm(UserDTO currentUser, ApplicationManager appManager) : base(currentUser, appManager)
        {
            InitializeComponent();
            this.Text = $"Panel de Estudiante - {currentUser.Name}";
        }

        private void StudentForm_Load(object sender, EventArgs e)
        {

        }
    }
}
