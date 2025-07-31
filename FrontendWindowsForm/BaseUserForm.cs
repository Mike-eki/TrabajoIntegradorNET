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

        // Controles comunes (declara los que necesites)
        protected Button btnEditProfile;
        protected Button btnLogout;
        protected Button btnViewCourses;
        protected Button btnViewSpecialties;
        protected Button btnViewPlans;
        protected Button btnViewCommissions;
        protected Button btnViewGrades;
        protected Panel mainPanel; // Panel para contenido dinámico

        public BaseUserForm(UserDTO user, ApplicationManager appManager)
        {
            InitializeComponent();
            _client = HttpClientManager.GetClient();
            _currentUser = user;
            _appManager = appManager;
            InitializeCommonControls();
            SetupCommonEventHandlers();
        }

        private void InitializeCommonControls()
        {
            // Crear botones comunes
            btnEditProfile = new Button { Text = "Editar Perfil", Top = 10, Left = 10 };
            btnLogout = new Button { Text = "Cerrar Sesión", Top = 10, Left = 120 };
            btnViewCourses = new Button { Text = "Cursos", Top = 10, Left = 230 };
            btnViewSpecialties = new Button { Text = "Especialidades", Top = 10, Left = 310 };
            btnViewPlans = new Button { Text = "Planes", Top = 10, Left = 420 };
            btnViewCommissions = new Button { Text = "Comisiones", Top = 10, Left = 490 };
            btnViewGrades = new Button { Text = "Notas", Top = 10, Left = 600 };

            mainPanel = new Panel
            {
                Top = 50,
                Left = 10,
                Width = 760,
                Height = 400,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Agregar controles al formulario
            this.Controls.AddRange(new Control[]
            {
                btnEditProfile,
                btnLogout,
                btnViewCourses,
                btnViewSpecialties,
                btnViewPlans,
                btnViewCommissions,
                btnViewGrades,
                mainPanel
            });
        }

        private void SetupCommonEventHandlers()
        {
            btnEditProfile.Click += BtnEditProfile_Click;
            btnLogout.Click += BtnLogout_Click;
            btnViewCourses.Click += BtnViewCourses_Click;
            btnViewSpecialties.Click += BtnViewSpecialties_Click;
            btnViewPlans.Click += BtnViewPlans_Click;
            btnViewCommissions.Click += BtnViewCommissions_Click;
            btnViewGrades.Click += BtnViewGrades_Click;
        }

        // Métodos comunes que pueden ser sobrescritos
        protected virtual async void BtnEditProfile_Click(object sender, EventArgs e)
        {
            //var editForm = new EditProfileForm(_client, _currentUser);
            //editForm.UserUpdated += (s, user) => _currentUser = user;
            //editForm.ShowDialog();
        }

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

        protected virtual async void BtnViewSpecialties_Click(object sender, EventArgs e)
        {
            try
            {
                var response = await _client.GetAsync("api/specialties");
                if (response.IsSuccessStatusCode)
                {
                    var specialties = await response.Content.ReadFromJsonAsync<List<SpecialtyDTO>>();
                    DisplaySpecialties(specialties);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        protected virtual async void BtnViewPlans_Click(object sender, EventArgs e)
        {
            try
            {
                var response = await _client.GetAsync("api/curricularplans");
                if (response.IsSuccessStatusCode)
                {
                    var plans = await response.Content.ReadFromJsonAsync<List<CurricularPlanReportDTO>>();
                    DisplayPlans(plans);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        protected virtual async void BtnViewCommissions_Click(object sender, EventArgs e)
        {
            try
            {
                var response = await _client.GetAsync("api/commissions");
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

        protected virtual async void BtnViewGrades_Click(object sender, EventArgs e)
        {
            // Implementación específica por rol
            await LoadGrades();
        }

        // Métodos virtuales que se pueden sobrescribir
        protected virtual async Task LoadGrades()
        {
            // Implementación base - puede ser sobrescrita
            MessageBox.Show("Funcionalidad de notas no disponible para este rol");
        }

        // Métodos para mostrar datos en el panel principal
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

        protected virtual void DisplaySpecialties(List<SpecialtyDTO> specialties)
        {
            ClearMainPanel();
            var listBox = new ListBox { Dock = DockStyle.Fill };
            listBox.Items.AddRange(specialties.Select(s => s.Name).ToArray());
            mainPanel.Controls.Add(listBox);
        }

        protected virtual void DisplayPlans(List<CurricularPlanReportDTO> plans)
        {
            ClearMainPanel();
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            var bindingSource = new BindingSource();
            bindingSource.DataSource = plans;
            grid.DataSource = bindingSource;

            mainPanel.Controls.Add(grid);
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
