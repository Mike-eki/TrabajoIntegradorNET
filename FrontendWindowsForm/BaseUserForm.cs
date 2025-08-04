using DTOs;
using FrontendWindowsForm.Features;
using Models.Enums;
using System.Net.Http.Json;
using System.Windows.Forms;

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
            btnCreateUser.Click += BtnCreateUser_Click;
            btnLogout.Click += BtnLogout_Click;
            btnViewCourses.Click += BtnViewCourses_Click;
            btnViewCommissions.Click += BtnViewCommissions_Click;
        }

        public class ResponseUserList
        {
            public List<UserDTO> Users { get; set; }
            public string message { get; set; }
        }

        public class ResponseCourseList
        {
            public List<CourseDTO> Courses { get; set; }
            public string message { get; set; }
        }

        protected void ClearMainPanel()
        {
            mainPanel.Controls.Clear();

        }
        // User methods
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
            dataGridView.CellClick += async (sender, e) =>
            {
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
        protected virtual void BtnCreateUser_Click(object? sender, EventArgs e)
        {
            // Solo administradores pueden crear usuarios, por ejemplo
            // Puedes ajustar esta lógica según tus roles
            if (_currentUser.RoleName != RoleType.Administrator)
            {
                MessageBox.Show("No tiene permisos para crear usuarios.");
                return;
            }

            try
            {
                // Definir propiedades de solo lectura (si las hubiera, normalmente ninguna para creación)
                var readOnlyProps = new List<string>() { "Id" }; // O ["Id"] si el ID se genera en el servidor

                // Crear una instancia vacía del DTO para definir el tipo
                var newUserTemplate = new RegisterUserDTO();

                // Crear el formulario genérico para creación
                // Pasamos el Tipo del objeto, no una instancia
                var createForm = new GenericEditForm(
                    _client,
                    typeof(RegisterUserDTO),         // Tipo de objeto a crear
                    "api/Users/register",             // Endpoint de la API para creación
                    readOnlyProps            // Propiedades de solo lectura (opcional)
                );

                // Suscribirse al evento para refrescar la lista si se crea exitosamente
                createForm.ItemSaved += (s, createdItem) =>
                {
                    MessageBox.Show("Usuario creado exitosamente. Refresque la lista para verlo.");
                    // Opcionalmente, puedes volver a cargar la lista de usuarios aquí
                    // BtnViewUsers_Click(this, EventArgs.Empty);
                };

                createForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir formulario de creación: {ex.Message}");
            }
        }
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

                editForm.ItemSaved += (s, updatedItem) =>
                {
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

        // Course methods
        protected virtual async void BtnViewCourses_Click(object sender, EventArgs e)
        {
            try
            {
                var response = await _client.GetAsync("api/Course");
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadFromJsonAsync<ResponseCourseList>().Result;
                    if (result != null && result.Courses != null)
                    {
                        DisplayCourses(result.Courses);
                    }
                    else
                    {
                        MessageBox.Show("No se pudieron obtener los cursos.");
                    }
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

            var dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                // ✅ Configuración adicional para mejor apariencia
                AllowUserToResizeRows = false,
                RowTemplate = { Height = 30 }
            };

            // Configurar columnas
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                HeaderText = "ID",
                FillWeight = 8
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Nombre",
                FillWeight = 25
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "AcademicPeriod",
                HeaderText = "Período Académico",
                FillWeight = 20
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CurriculumPlan",
                HeaderText = "Plan Curricular",
                FillWeight = 20
            });

            // Columna para contar especialidades
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SpecialtiesCount",
                HeaderText = "N° Especialidades",
                FillWeight = 12
            });

            // Columna de botón para ver especialidades
            DataGridViewButtonColumn viewSpecialtiesButton = new DataGridViewButtonColumn
            {
                Name = "ViewSpecialties",
                HeaderText = "🔍 Ver",
                Text = "🔍",
                UseColumnTextForButtonValue = true,
                FillWeight = 8
            };
            dataGridView.Columns.Add(viewSpecialtiesButton);

            // Columnas de botones para Editar y Borrar
            DataGridViewButtonColumn editButton = new DataGridViewButtonColumn
            {
                Name = "Edit",
                HeaderText = "✏️",
                Text = "✏️",
                UseColumnTextForButtonValue = true,
                FillWeight = 8
            };
            dataGridView.Columns.Add(editButton);

            DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn
            {
                Name = "Delete",
                HeaderText = "🗑️",
                Text = "🗑️",
                UseColumnTextForButtonValue = true,
                FillWeight = 8
            };
            dataGridView.Columns.Add(deleteButton);

            // Llenar datos
            foreach (var course in courses)
            {
                int rowIndex = dataGridView.Rows.Add();
                DataGridViewRow row = dataGridView.Rows[rowIndex];

                row.Cells["Id"].Value = course.Id;
                row.Cells["Name"].Value = course.Name;
                row.Cells["AcademicPeriod"].Value = course.AcademicPeriod.ToString();
                row.Cells["CurriculumPlan"].Value = course.CurriculumPlan.ToString();
                row.Cells["SpecialtiesCount"].Value = course.SpecialtiesLinked.Count;

                // Guardar el objeto Course en la fila
                row.Tag = course;
            }

            // Manejar clics en botones
            dataGridView.CellClick += async (sender, e) =>
            {
                if (e.RowIndex < 0) return;

                DataGridView dgv = sender as DataGridView;
                DataGridViewRow row = dgv.Rows[e.RowIndex];
                CourseDTO course = row.Tag as CourseDTO;

                if (course == null) return;

                // ✅ Determinar qué botón fue clickeado
                if (e.ColumnIndex == dgv.Columns["ViewSpecialties"].Index)
                {
                    ShowSpecialtiesDetail(course);
                }
                else if (e.ColumnIndex == dgv.Columns["Edit"].Index)
                {
                    await EditCourse(course, dgv, e.RowIndex);
                }
                else if (e.ColumnIndex == dgv.Columns["Delete"].Index)
                {
                    await DeleteCourse(course, dgv, e.RowIndex);
                }
            };

            mainPanel.Controls.Add(dataGridView);
        }

        // Método para mostrar detalle de especialidades
        private void ShowSpecialtiesDetail(CourseDTO course)
        {
            var detailForm = new Form
            {
                Text = $"Especialidades para {course.Name}",
                Size = new Size(500, 400),
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Panel principal
            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill
            };

            // ListBox para mostrar especialidades
            var listBox = new ListBox
            {
                Dock = DockStyle.Top,
                Height = 250
            };

            // Agregar especialidades a la lista
            foreach (var specialty in course.SpecialtiesLinked)
            {
                listBox.Items.Add(specialty.Name);
            }

            // Panel de botones
            var buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60
            };

            // Botones
            var btnAdd = new Button
            {
                Text = "Agregar",
                Location = new Point(10, 15),
                Size = new Size(80, 30)
            };

            var btnRemove = new Button
            {
                Text = "Quitar",
                Location = new Point(100, 15),
                Size = new Size(80, 30)
            };

            var btnClose = new Button
            {
                Text = "Cerrar",
                Location = new Point(400, 15),
                Size = new Size(80, 30)
            };

            // Eventos de botones
            btnAdd.Click += (s, e) => {
                // Aquí iría la lógica para agregar una especialidad
                MessageBox.Show("Funcionalidad de agregar especialidad aún no implementada.\n\nPara implementarla, necesitarías:\n1. Obtener lista de todas las especialidades\n2. Mostrar diálogo de selección\n3. Llamar a API para vincular especialidad al curso", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            btnRemove.Click += (s, e) => {
                if (listBox.SelectedItem != null)
                {
                    var selectedSpecialty = listBox.SelectedItem.ToString();
                    // Aquí iría la lógica para quitar una especialidad
                    MessageBox.Show($"Funcionalidad de quitar '{selectedSpecialty}' aún no implementada.\n\nPara implementarla, necesitarías llamar a una API que desvincule la especialidad del curso.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Seleccione una especialidad para quitar.");
                }
            };

            btnClose.Click += (s, e) => {
                detailForm.Close();
            };

            // Agregar controles
            buttonPanel.Controls.AddRange(new Control[] { btnAdd, btnRemove, btnClose });
            mainPanel.Controls.AddRange(new Control[] { listBox, buttonPanel });
            detailForm.Controls.Add(mainPanel);

            detailForm.ShowDialog();
        }

        // Método para editar un curso
        private async Task EditCourse(CourseDTO course, DataGridView dataGridView, int rowIndex)
        {
            try
            {
                // Propiedades que no se pueden editar (ajusta según tus necesidades)
                var readOnlyProps = new List<string> { "Id" }; // ID normalmente es de solo lectura

                var editForm = new GenericEditForm(
                    _client,
                    course,
                    $"api/Course/{course.Id}", // Asegúrate que este endpoint sea correcto
                    readOnlyProps
                );

                editForm.ItemSaved += (s, updatedItem) => {
                    var updatedCourse = updatedItem as CourseDTO;
                    if (updatedCourse != null)
                    {
                        // Actualizar fila en el DataGridView
                        var row = dataGridView.Rows[rowIndex];
                        row.Cells["Name"].Value = updatedCourse.Name;
                        row.Cells["AcademicPeriod"].Value = updatedCourse.AcademicPeriod.ToString();
                        row.Cells["CurriculumPlan"].Value = updatedCourse.CurriculumPlan.ToString();
                        row.Cells["SpecialtiesCount"].Value = updatedCourse.SpecialtiesLinked.Count;
                        row.Tag = updatedCourse; // Actualizar objeto en Tag

                        MessageBox.Show("Curso actualizado correctamente");
                    }
                };

                editForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al editar curso: {ex.Message}");
            }
        }

        // Método para eliminar un curso
        private async Task DeleteCourse(CourseDTO course, DataGridView dataGridView, int rowIndex)
        {
            var result = MessageBox.Show(
                $"¿Está seguro que desea eliminar el curso '{course.Name}'?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var response = await _client.DeleteAsync($"api/Course/{course.Id}");

                    if (response.IsSuccessStatusCode)
                    {
                        dataGridView.Rows.RemoveAt(rowIndex);
                        MessageBox.Show("Curso eliminado correctamente");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        MessageBox.Show("No tiene permisos para eliminar este curso");
                    }
                    else
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Error al eliminar curso: {error}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error de conexión: {ex.Message}");
                }
            }
        }

        // Commission methods
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


    }
}
