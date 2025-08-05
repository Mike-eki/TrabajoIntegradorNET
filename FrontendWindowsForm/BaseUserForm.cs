using DTOs;
using FrontendWindowsForm.Features;
using Models.Enums;
using Models.Entities;
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
            btnCreateCourse.Click += BtnCreateCourse_Click;
            btnViewCommissions.Click += BtnViewCommissions_Click;
        }

        private void BtnCreateCourse_Click1(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
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

        public class ResponseSpecialtyList
        {
            public List<SpecialtyDTO> Specialties { get; set; }
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
                row.Cells["AcademicPeriod"].Value = course.AcademicPeriod;
                row.Cells["CurriculumPlan"].Value = course.CurriculumPlan;
                row.Cells["SpecialtiesCount"].Value = course.SpecialtiesLinked?.Count ?? 0;

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

        // Método simplificado para gestionar especialidades de un curso
        private async void ShowSpecialtiesDetail(CourseDTO course)
        {
            var detailForm = new Form
            {
                Text = $"Gestionar Especialidades para {course.Name}",
                Size = new Size(600, 500),
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Etiqueta informativa
            var lblTitle = new Label
            {
                Text = $"Seleccionar Especialidades para '{course.Name}':",
                Location = new Point(10, 10),
                Size = new Size(500, 20),
                Font = new Font(this.Font, FontStyle.Bold)
            };

            // CheckedListBox para mostrar todas las especialidades y permitir selección
            var checkedListBox = new CheckedListBox
            {
                Location = new Point(10, 40),
                Size = new Size(560, 350),
                CheckOnClick = true // Permite marcar/desmarcar al hacer clic
            };

            checkedListBox.DisplayMember = "Name";

            // Panel de botones
            var panelButtons = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50
            };

            var btnSave = new Button
            {
                Text = "Guardar",
                Location = new Point(200, 10),
                Size = new Size(80, 30)
            };

            var btnCancel = new Button
            {
                Text = "Cancelar",
                Location = new Point(300, 10),
                Size = new Size(80, 30)
            };

            var btnClose = new Button
            {
                Text = "Cerrar",
                Location = new Point(400, 10),
                Size = new Size(80, 30)
            };

            // Variable para almacenar todas las especialidades disponibles
            List<SpecialtyDTO> allSpecialties = new List<SpecialtyDTO>();

            try
            {
                // 1. Obtener todas las especialidades del servidor
                var response = await _client.GetAsync("api/Specialty"); // Asegúrate que este endpoint sea correcto
                if (response.IsSuccessStatusCode)
                {
                    // Asumiendo que la API devuelve una lista directamente
                    // Si devuelve un objeto contenedor, ajusta según tu DTO (por ejemplo, ResponseSpecialtyList)
                    var result =  response.Content.ReadFromJsonAsync<ResponseSpecialtyList>().Result;
                    foreach (var specialty in result.Specialties)
                    {
                        allSpecialties.Add(specialty);
                    }
                }
                else
                {
                    MessageBox.Show($"Error al obtener especialidades. Código: {response.StatusCode}");
                    detailForm.Close();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de red al obtener especialidades: {ex.Message}");
                detailForm.Close();
                return;
            }

            // 2. Llenar el CheckedListBox
            var linkedSpecialtyIds = new HashSet<int>(course.SpecialtiesLinked.Select(s => s.Id));

            foreach (var specialty in allSpecialties)
            {
                int index = checkedListBox.Items.Add(specialty); // Agrega el objeto completo
                                                                 // Marcar como seleccionado si ya está vinculado al curso
                if (linkedSpecialtyIds.Contains(specialty.Id))
                {
                    checkedListBox.SetItemChecked(index, true);
                }
            }

            // 3. Configurar eventos de botones
            btnSave.Click += async (s, e) => {
                try
                {
                    // 4. Crear una nueva lista con las especialidades seleccionadas
                    var selectedSpecialties = new List<Specialty>();
                    foreach (SpecialtyDTO specialtyDTO in checkedListBox.CheckedItems)
                    {
                        selectedSpecialties.Add(new Specialty 
                        { 
                            Id = specialtyDTO.Id,
                            Name = specialtyDTO.Name
                        });
                    }


                    // 5. Crear un objeto CourseDTO actualizado
                    // Es crucial copiar las propiedades existentes del curso original
                    var updatedCourse = new CourseDTO
                    {
                        Id = course.Id,
                        Name = course.Name,
                        AcademicPeriod = course.AcademicPeriod,
                        CurriculumPlan = course.CurriculumPlan,
                        SpecialtiesLinked = selectedSpecialties // La lista actualizada
                    };

                    // 6. Enviar la actualización al servidor
                    var response = await _client.PutAsJsonAsync($"api/Course/{course.Id}", updatedCourse);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Especialidades actualizadas correctamente.");
                        detailForm.DialogResult = DialogResult.OK; // Opcional, para comunicación con formulario padre
                        detailForm.Close();
                        // Opcional: Actualizar la lista de cursos en la pantalla principal
                        // BtnViewCourses_Click(this, EventArgs.Empty);
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Error al actualizar el curso: {response.StatusCode}\nDetalles: {errorContent}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar cambios: {ex.Message}");
                }
            };

            btnCancel.Click += (s, e) => {
                // 7. Revertir a la selección original
                checkedListBox.Items.Clear();
                foreach (var specialty in allSpecialties)
                {
                    int index = checkedListBox.Items.Add(specialty);
                    if (linkedSpecialtyIds.Contains(specialty.Id))
                    {
                        checkedListBox.SetItemChecked(index, true);
                    }
                }
            };

            btnClose.Click += (s, e) => {
                detailForm.Close();
            };

            // 8. Agregar controles al formulario
            detailForm.Controls.AddRange(new Control[] { lblTitle, checkedListBox, panelButtons });
            panelButtons.Controls.AddRange(new Control[] { btnSave, btnCancel, btnClose });

            detailForm.ShowDialog();
        }

        // Clase auxiliar para almacenar SpecialtyDTO en ListBox y mostrar el nombre
        public class SpecialtyItem
        {
            public SpecialtyDTO Specialty { get; set; }

            public SpecialtyItem(SpecialtyDTO specialty)
            {
                Specialty = specialty;
            }

            public override string ToString()
            {
                return Specialty.Name;
            }
        }
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
                        row.Cells["AcademicPeriod"].Value = updatedCourse.AcademicPeriod;
                        row.Cells["CurriculumPlan"].Value = updatedCourse.CurriculumPlan;
                        row.Cells["SpecialtiesCount"].Value = updatedCourse.SpecialtiesLinked?.Count ?? 0;
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

        private void BtnCreateCourse_Click(object sender, EventArgs e)
        {
            try
            {
                // Definir propiedades de solo lectura (si las hubiera, normalmente el ID se genera en el servidor)
                var readOnlyProps = new List<string>() { "Id" }; // O ["Id"] si el backend no lo permite editable

                // Crear el formulario genérico para creación de un CourseDTO
                var createForm = new GenericEditForm(
                    _client,
                    typeof(CourseDTO),       // Tipo de objeto a crear
                    "api/Course",           // Endpoint de la API para creación (ajusta si es diferente)
                    readOnlyProps            // Propiedades de solo lectura
                );

                // Opcional: Suscribirse al evento para refrescar la lista si se crea exitosamente
                createForm.ItemSaved += (s, createdItem) => {
                    MessageBox.Show("Curso creado exitosamente.");
                    // Opcional: Refrescar la lista de cursos
                    BtnViewCourses_Click(this, EventArgs.Empty); // Esto volverá a cargar la lista
                };

                createForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir formulario de creación de curso: {ex.Message}");
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
