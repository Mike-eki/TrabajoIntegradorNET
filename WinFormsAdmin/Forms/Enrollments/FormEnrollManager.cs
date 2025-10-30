using Models.DTOs;
using System.Windows.Forms;
using WinFormsAdmin.Services;

namespace WinFormsAdmin.Forms.Enrollments
{
    public partial class FormEnrollManager : Form
    {
        private readonly ApiClient _apiClient;
        private List<EnrollmentDetailDto> _allEnrollments = new();

        // Estados posibles
        private static readonly string[] StatusOptions = { "Todos", "Inscripto", "Aprobado", "Cerrado", "Baja"};

        public FormEnrollManager(ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;

            // Vincular
            dgvEnrollments.CellClick += dgvEnrollments_CellClick;
            dgvEnrollments.CellEndEdit += dgvEnrollments_CellEndEdit;

            // ✅ Vincular eventos de filtros
            txtSearch.TextChanged += txtSearch_TextChanged;
            cmbStatus.SelectedIndexChanged += cmbStatus_SelectedIndexChanged;
            txtSubjectFilter.TextChanged += (s, e) => ApplyFilters();
            txtCycleYearFilter.TextChanged += (s, e) => ApplyFilters();
            dtpEnrollmentFrom.ValueChanged += (s, e) => ApplyFilters();
            dtpUnenrollmentTo.ValueChanged += (s, e) => ApplyFilters();
            txtCycleYearFilter.KeyPress += txtCycleYearFilter_KeyPress;

            // ✅ Desactivar autogeneración
            dgvEnrollments.AutoGenerateColumns = false;


            // ✅ Configurar columnas manualmente
            SetupDataGridViewColumns();

            // Configurar ComboBox de estado
            cmbStatus.Items.AddRange(StatusOptions);
            cmbStatus.SelectedIndex = 0;
        }

        private async void FormEnrollManager_Load(object sender, EventArgs e)
        {
            await LoadEnrollmentsAsync();
        }

        private async Task LoadEnrollmentsAsync()
        {
            try
            {
                var enrollments = await _apiClient.GetListAsync<EnrollmentDetailDto>("api/enrollments/all");
                _allEnrollments = enrollments ?? new List<EnrollmentDetailDto>();
                ApplyFilters(); // Mostrar todos al inicio
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar inscripciones: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGridViewColumns()
        {
            dgvEnrollments.Columns.Clear();

            // Columna: ID
            dgvEnrollments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(EnrollmentDetailDto.Id),
                DataPropertyName = nameof(EnrollmentDetailDto.Id),
                HeaderText = "ID",
                Width = 50,
                ReadOnly = true
            });

            // Columna: Nombre Estudiante
            dgvEnrollments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(EnrollmentDetailDto.StudentFullName),
                DataPropertyName = nameof(EnrollmentDetailDto.StudentFullName),
                HeaderText = "Estudiante",
                Width = 180,
                ReadOnly = true
            });

            // Columna: Legajo
            dgvEnrollments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(EnrollmentDetailDto.StudentLegajo),
                DataPropertyName = nameof(EnrollmentDetailDto.StudentLegajo),
                HeaderText = "Legajo",
                Width = 100,
                ReadOnly = true
            });

            // Columna: Materia
            dgvEnrollments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(EnrollmentDetailDto.SubjectName),
                DataPropertyName = nameof(EnrollmentDetailDto.SubjectName),
                HeaderText = "Materia",
                Width = 150,
                ReadOnly = true
            });

            // Columna: Año Ciclo
            dgvEnrollments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(EnrollmentDetailDto.CycleYear),
                DataPropertyName = nameof(EnrollmentDetailDto.CycleYear),
                HeaderText = "Ciclo del Año",
                Width = 80,
                ReadOnly = true
            });

            // Columna: Nota Final
            dgvEnrollments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(EnrollmentDetailDto.FinalGrade),
                DataPropertyName = nameof(EnrollmentDetailDto.FinalGrade),
                HeaderText = "Nota Final",
                Width = 80,
                ReadOnly = false
            });

            // Columna: Estado
            dgvEnrollments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(EnrollmentDetailDto.Status),
                DataPropertyName = nameof(EnrollmentDetailDto.Status),
                HeaderText = "Estado",
                Width = 100,
                ReadOnly = true
            });

            // Columna: Fecha Inscripción
            dgvEnrollments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(EnrollmentDetailDto.EnrollmentDate),
                DataPropertyName = nameof(EnrollmentDetailDto.EnrollmentDate),
                HeaderText = "Fecha Inscripción",
                Width = 120,
                ReadOnly = true,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm" }
            });

            // Columna: Fecha Desinscripción
            dgvEnrollments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(EnrollmentDetailDto.UnenrollmentDate),
                DataPropertyName = nameof(EnrollmentDetailDto.UnenrollmentDate),
                HeaderText = "Fecha Baja",
                Width = 120,
                ReadOnly = true,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm" }
            });

            // Columna: Botón "Cambiar Estado"
            dgvEnrollments.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Acciones",
                Text = "Cambiar Estado",
                UseColumnTextForButtonValue = true,
                HeaderText = "Acciones",
                Width = 120
            });
        }

        private void ApplyFilters()
        {
            var filtered = _allEnrollments.AsEnumerable();

            // ✅ Solo nombre y legajo
            var searchText = txtSearch.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered.Where(e =>
                    e.StudentFullName.ToLower().Contains(searchText) ||
                    e.StudentLegajo.ToLower().Contains(searchText)
                );
            }

            // Filtro por estado
            var selectedStatus = cmbStatus.SelectedItem?.ToString();
            if (selectedStatus != "Todos" && !string.IsNullOrEmpty(selectedStatus))
            {
                string statusValue = selectedStatus;
                filtered = filtered.Where(e => e.Status == statusValue);
            }

            // Filtro por materia
            var subjectFilter = txtSubjectFilter.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(subjectFilter))
            {
                filtered = filtered.Where(e => e.SubjectName.ToLower().Contains(subjectFilter));
            }

            // Filtro por año ciclo
            if (int.TryParse(txtCycleYearFilter.Text.Trim(), out int cycleYear) && cycleYear > 0)
            {
                filtered = filtered.Where(e => e.CycleYear == cycleYear);
            }

            // Filtro por fecha de inscripción (desde)
            if (dtpEnrollmentFrom.Checked)
            {
                var fromDate = dtpEnrollmentFrom.Value.Date;
                filtered = filtered.Where(e => e.EnrollmentDate >= fromDate);
            }

            // Filtro por fecha de baja (hasta)
            if (dtpUnenrollmentTo.Checked)
            {
                var toDate = dtpUnenrollmentTo.Value.Date.AddDays(1).AddTicks(-1); // Incluir todo el día
                filtered = filtered.Where(e => e.UnenrollmentDate.HasValue && e.UnenrollmentDate <= toDate);
            }

            dgvEnrollments.DataSource = new BindingSource(filtered.ToList(), null);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        // Opcional: botón de recargar
        private async void btnReload_Click(object sender, EventArgs e)
        {
            await LoadEnrollmentsAsync();
        }

        private async void dgvEnrollments_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvEnrollments.Columns[e.ColumnIndex].Name != nameof(EnrollmentDetailDto.FinalGrade))
                return;

            var row = dgvEnrollments.Rows[e.RowIndex];
            var id = Convert.ToInt32(row.Cells[nameof(EnrollmentDetailDto.Id)].Value);

            // Obtener el nuevo valor
            var cellValue = row.Cells[e.ColumnIndex].Value;
            int? newGrade = null;

            if (cellValue != null && int.TryParse(cellValue.ToString(), out int grade))
            {
                if (grade is < 0 or > 10)
                {
                    MessageBox.Show("La nota debe estar entre 0 y 10.", "Valor inválido",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // Revertir
                    var original = _allEnrollments.First(x => x.Id == id).FinalGrade;
                    row.Cells[e.ColumnIndex].Value = original;
                    return;
                }
                newGrade = grade;
            }
            // Si es null o vacío, se interpreta como "sin nota" → null

            try
            {
                // Llamar a la API para actualizar la nota
                await _apiClient.PutAsync($"api/enrollments/{id}/grade", new { finalGrade = newGrade });

                // Actualizar en la lista local
                var enrollment = _allEnrollments.First(x => x.Id == id);
                enrollment.FinalGrade = newGrade;

                MessageBox.Show("Nota actualizada correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la nota: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Revertir visualmente
                row.Cells[e.ColumnIndex].Value = _allEnrollments.First(x => x.Id == id).FinalGrade;
            }
        }

        private string[] StatusOptionsForEditing = { "Inscripto", "Aprobado", "Cerrado", "Baja" };

        private async void dgvEnrollments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Si es la columna de "Acciones"
            if (dgvEnrollments.Columns[e.ColumnIndex].Name == "Acciones")
            {
                var row = dgvEnrollments.Rows[e.RowIndex];
                var id = Convert.ToInt32(row.Cells[nameof(EnrollmentDetailDto.Id)].Value);
                var currentStatus = row.Cells[nameof(EnrollmentDetailDto.Status)].Value?.ToString() ?? "Inscripto";

                // ✅ Diálogo mejorado
                using var dialog = new Form
                {
                    Text = "Cambiar Estado",
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    Size = new Size(250, 140),
                    Padding = new Padding(10)
                };

                var cmb = new ComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Items = { "Inscripto", "Aprobado", "Cerrado", "Baja" },
                    Dock = DockStyle.Top,
                    Margin = new Padding(0, 0, 0, 10)
                };
                cmb.SelectedItem = currentStatus;

                var btnOk = new Button
                {
                    Text = "Aceptar",
                    DialogResult = DialogResult.OK,
                    Width = 80,
                    Height = 30
                };

                var btnCancel = new Button
                {
                    Text = "Cancelar",
                    DialogResult = DialogResult.Cancel,
                    Width = 80,
                    Height = 30
                };

                // Panel para botones (alineado a la derecha)
                var panelButtons = new FlowLayoutPanel
                {
                    Dock = DockStyle.Bottom,
                    Height = 40,
                    FlowDirection = FlowDirection.RightToLeft,
                    Padding = new Padding(0, 5, 0, 0)
                };
                panelButtons.Controls.Add(btnOk);
                panelButtons.Controls.Add(btnCancel);

                dialog.Controls.Add(panelButtons);
                dialog.Controls.Add(cmb);

                // Hacer que el ComboBox tenga foco
                dialog.Shown += (s, e) => cmb.Focus();

                if (dialog.ShowDialog() == DialogResult.OK && cmb.SelectedItem != null)
                {
                    var newStatus = cmb.SelectedItem.ToString();
                    try
                    {
                        await _apiClient.PutAsync($"api/enrollments/{id}/status", new { status = newStatus });

                        // Actualizar localmente
                        var enrollment = _allEnrollments.First(x => x.Id == id);
                        enrollment.Status = newStatus;
                        row.Cells[nameof(EnrollmentDetailDto.Status)].Value = newStatus;

                        MessageBox.Show("Estado actualizado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void txtCycleYearFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo dígitos y control (backspace, etc.)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}