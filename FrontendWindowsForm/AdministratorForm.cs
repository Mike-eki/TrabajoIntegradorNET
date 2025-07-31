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
    public partial class AdministratorForm : BaseUserForm
    {
        public AdministratorForm(UserDTO currentUser, ApplicationManager appManager) : base(currentUser, appManager)
        {
            this.Text = $"Panel de Administrador - {currentUser.Name}";
        }
    }
}
