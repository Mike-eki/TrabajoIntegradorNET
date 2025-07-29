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
    public partial class AdministratorForm : Form
    {
        private HttpClient client;
        private UserDTO currentUser;
        public AdministratorForm(HttpClient client, UserDTO currentUser)
        {
            InitializeComponent();
            this.client = client;
            this.currentUser = currentUser;
        }
    }
}
