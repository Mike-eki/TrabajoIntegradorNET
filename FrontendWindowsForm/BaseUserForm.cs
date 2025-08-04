using DTOs;
using System.Net.Http.Json;
using FrontendWindowsForm.Features;

namespace FrontendWindowsForm
{
    public partial class BaseUserForm : Form
    {
        protected HttpClient _client;
        protected UserDTO _currentUser;
        protected ApplicationManager _appManager; // Referencia al gestor

        public BaseUserForm(UserDTO user, ApplicationManager appManager)
        {
            InitializeComponent();
            _client = HttpClientManager.GetClient();
            _currentUser = user;
            _appManager = appManager;
            SetupCommonEventHandlers();
        }

        private void SetupCommonEventHandlers()
        {
            btnViewUsers.Click += BtnViewUsers_Click;

            btnLogout.Click += BtnLogout_Click;
            btnViewCourses.Click += BtnViewCourses_Click;
            btnViewCommissions.Click += BtnViewCommissions_Click;
        }

        public class ResponseUserList
        {
            public List<UserDTO> Users { get; set; }
            public string message { get; set; }
        }
        // Métodos comunes que pueden ser sobrescritos
        protected virtual void BtnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro que desea cerrar sesión?",
                "Cerrar Sesión", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                // ✅ Usar el gestor para volver al login
                _appManager.ShowLoginForm();

                // ✅ Cerrar este formulario
                this.Close();
            }
        }
        protected virtual async void BtnViewUsers_Click(object sender, EventArgs e)
        {
            ClearMainPanel();

            try
            {
                var response = await _client.GetAsync("api/Users");
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadFromJsonAsync<ResponseUserList>().Result;
                    if (result != null)
                        DisplayUsers(result.Users);
                    else
                        MessageBox.Show("La lista de usuarios está vacía o no se pudo obtener.");
                }
                else
                {
                    MessageBox.Show("Error al obtener la lista de usuarios.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        protected virtual void DisplayUsers(List<UserDTO> users)
        {
            ClearMainPanel();
            var dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true
            };

            // Configurar columnas
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                HeaderText = "ID",
                FillWeight = 10
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Username",
                HeaderText = "Usuario",
                FillWeight = 20
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Nombre",
                FillWeight = 25
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Email",
                HeaderText = "Email",
                FillWeight = 25
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RoleName",
                HeaderText = "Rol",
                FillWeight = 15
            });

            // Agregar columnas de botones
            DataGridViewButtonColumn editButton = new DataGridViewButtonColumn
            {
                Name = "Edit",
                HeaderText = "✏️",
                Text = "Editar",
                UseColumnTextForButtonValue = true,
                FillWeight = 10
            };
            dataGridView.Columns.Add(editButton);

            DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn
            {
                Name = "Delete",
                HeaderText = "🗑️",
                Text = "Eliminar",
                UseColumnTextForButtonValue = true,
                FillWeight = 10
            };
            dataGridView.Columns.Add(deleteButton);

            // Llenar datos
            foreach (var user in users)
            {
                int rowIndex = dataGridView.Rows.Add();
                DataGridViewRow row = dataGridView.Rows[rowIndex];

                row.Cells["Id"].Value = user.Id;
                row.Cells["Username"].Value = user.Username;
                row.Cells["Name"].Value = user.Name;
                row.Cells["Email"].Value = user.Email;
                row.Cells["RoleName"].Value = user.RoleName;

                // Guardar el objeto User en la fila
                row.Tag = user;
            }

            // Manejar clics en botones
            dataGridView.CellClick += async (sender, e) => {
                if (e.RowIndex < 0) return;

                DataGridView dgv = sender as DataGridView;
                DataGridViewRow row = dgv.Rows[e.RowIndex];
                UserDTO user = row.Tag as UserDTO;

                if (user == null) return;

                if (e.ColumnIndex == dgv.Columns["Edit"].Index)
                {
                    await EditUserGeneric(user, dgv, e.RowIndex);
                }
                else if (e.ColumnIndex == dgv.Columns["Delete"].Index)
                {
                    await DeleteUser(user, dgv, e.RowIndex);
                }
            };

            mainPanel.Controls.Add(dataGridView);
        }

        // Método genérico para editar cualquier usuario
        private async Task EditUserGeneric(UserDTO user, DataGridView dataGridView, int rowIndex)
        {
            try
            {
                // Propiedades de solo lectura
                var readOnlyProps = new List<string> { "Id", "Username" };

                var editForm = new GenericEditForm(
                    _client,
                    user,
                    $"api/Users/{user.Id}",
                    readOnlyProps
                );

                editForm.ItemUpdated += (s, updatedItem) => {
                    var updatedUser = updatedItem as UserDTO;
                    if (updatedUser != null)
                    {
                        // Actualizar fila en el DataGridView
                        var row = dataGridView.Rows[rowIndex];
                        row.Cells["Name"].Value = updatedUser.Name;
                        row.Cells["Email"].Value = updatedUser.Email;
                        row.Cells["RoleName"].Value = updatedUser.RoleName;
                        row.Tag = updatedUser; // Actualizar objeto en Tag
                    }
                };

                editForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al editar usuario: {ex.Message}");
            }
        }
        // Método para eliminar usuario (igual que antes)
        private async Task DeleteUser(UserDTO user, DataGridView dataGridView, int rowIndex)
        {
            // No permitir auto-eliminación
            if (user.Id == _currentUser.Id)
            {
                MessageBox.Show("No puede eliminarse a sí mismo");
                return;
            }

            var result = MessageBox.Show(
                $"¿Está seguro que desea eliminar al usuario '{user.Name}'?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var response = await _client.DeleteAsync($"api/Users/{user.Id}");

                    if (response.IsSuccessStatusCode)
                    {
                        dataGridView.Rows.RemoveAt(rowIndex);
                        MessageBox.Show("Usuario eliminado correctamente");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        MessageBox.Show("No tiene permisos para eliminar este usuario");
                    }
                    else
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Error al eliminar usuario: {error}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error de conexión: {ex.Message}");
                }
            }
        }
        protected virtual async void BtnViewCourses_Click(object sender, EventArgs e)
        {
            try
            {
                var response = await _client.GetAsync("api/Courses");
                if (response.IsSuccessStatusCode)
                {
                    var courses = await response.Content.ReadFromJsonAsync<List<CourseDTO>>();
                    DisplayCourses(courses);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        protected virtual void DisplayCourses(List<CourseDTO> courses)
        {
            ClearMainPanel();
            var listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details
            };
            listView.Columns.Add("ID", 50);
            listView.Columns.Add("Nombre", 200);
            listView.Columns.Add("Créditos", 80);
            listView.Columns.Add("Período Académico", 150);

            foreach (var course in courses)
            {
                var item = new ListViewItem(new[]
                {
                    course.Id.ToString(),
                    course.Name,
                    course.Credits.ToString(),
                    course.AcademicPeriod
                });
                listView.Items.Add(item);
            }

            mainPanel.Controls.Add(listView);
        }
        protected virtual async void BtnViewCommissions_Click(object sender, EventArgs e)
        {
            try
            {
                var response = await _client.GetAsync("api/Commissions");
                if (response.IsSuccessStatusCode)
                {
                    var commissions = await response.Content.ReadFromJsonAsync<List<CommissionDTO>>();
                    DisplayCommissions(commissions);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        protected virtual void DisplayCommissions(List<CommissionDTO> commissions)
        {
            ClearMainPanel();
            var treeView = new TreeView { Dock = DockStyle.Fill };

            var courses = commissions.GroupBy(c => c.CourseName);
            foreach (var courseGroup in courses)
            {
                var courseNode = new TreeNode(courseGroup.Key);
                foreach (var commission in courseGroup)
                {
                    courseNode.Nodes.Add($"{commission.Name} - {commission.Schedule}");
                }
                treeView.Nodes.Add(courseNode);
            }

            mainPanel.Controls.Add(treeView);
        }
        protected void ClearMainPanel()
        {
            mainPanel.Controls.Clear();

        }
    }
}
