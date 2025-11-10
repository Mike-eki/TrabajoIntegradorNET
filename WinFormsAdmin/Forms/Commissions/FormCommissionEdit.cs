using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsAdmin.Services;
using Services;

namespace WinFormsAdmin.Forms.Commissions
{
    public partial class FormCommissionEdit : Form
    {
        private readonly ApiClient _apiClient;
        private readonly int? _commissionId;
        private List<SubjectResponseDto>? _subjects;
        private List<UserResponseDto>? _professors;

        public FormCommissionEdit(ApiClient apiClient, int? commissionId = null)
        {
            InitializeComponent();
            _apiClient = apiClient;
            _commissionId = commissionId;

            InitializeDayOfWeekCombo();
            InitializeStatusCombo();

            // Suscribirse al evento Load
            this.Load += FormCommissionEdit_Load;
        }

        private void InitializeDayOfWeekCombo()
        {
            cmbDayOfWeek.Items.AddRange(new[]
            {
            "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo"
        });
            cmbDayOfWeek.SelectedIndex = 0;
        }

        private void InitializeStatusCombo()
        {
            cmbStatus.Items.AddRange(new[]
            {
            "Activo", "Pendiente", "Inactivo"
        });
            cmbStatus.SelectedIndex = 1;
        }

        // Evento Load del formulario
        private async void FormCommissionEdit_Load(object? sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        // Método separado, async Task, con manejo seguro
        private async Task LoadDataAsync()
        {
            try
            {
                // Mostrar indicador de carga (opcional)
                this.Cursor = Cursors.WaitCursor;
                btnSave.Enabled = false;

                // Cargar materias
                _subjects = await _apiClient.GetListAsync<SubjectResponseDto>("/api/Subjects");
                clbSubjects.Items.Clear();
                clbSubjects.DisplayMember = nameof(SubjectResponseDto.Name);

                if (_subjects != null)
                {
                    foreach (var subject in _subjects)
                        clbSubjects.Items.Add(subject);
                }
                else
                {
                    _subjects = new List<SubjectResponseDto>();
                }

                // Cargar profesores
                _professors = await _apiClient.GetListAsync<UserResponseDto>("/api/Users?role=Professor")
                             ?? new List<UserResponseDto>();

                cmbProfessors.DisplayMember = "FullName";
                cmbProfessors.ValueMember = "Id";
                cmbProfessors.DataSource = _professors;
                cmbProfessors.SelectedIndex = -1;
                cmbProfessors.Enabled = _professors.Any();

                // Cargar detalles si es edición
                if (_commissionId.HasValue)
                {
                    await LoadCommissionDetailsAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al cargar los datos: {ex.Message}",
                    "Error de carga",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                // Opcional: cerrar si no se pueden cargar datos esenciales
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnSave.Enabled = true;
            }
        }

        private async Task LoadCommissionDetailsAsync()
        {
            try
            {
                var commission = await _apiClient.GetAsync<CommissionWithProfessorDto>(
                    $"/api/Commissions/{_commissionId}");

                if (commission == null)
                {
                    MessageBox.Show("No se encontró la comisión especificada.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }

                // Seleccionar materia
                var subjectItem = _subjects?.FirstOrDefault(s => s.Id == commission.SubjectId);
                if (subjectItem != null)
                {
                    int index = clbSubjects.Items.IndexOf(subjectItem);
                    if (index >= 0)
                        clbSubjects.SetItemChecked(index, true);
                }

                // Seleccionar profesor
                if (commission.ProfessorId != 0 && _professors != null && _professors.Any())
                {
                    var professorExists = _professors.Any(p => p.Id == commission.ProfessorId);
                    if (professorExists)
                    {
                        cmbProfessors.SelectedValue = commission.ProfessorId;
                    }
                    else
                    {
                        // El profesor no está en la lista: lo marcamos como no seleccionado
                        cmbProfessors.SelectedIndex = -1;
                    }
                }
                else
                {
                    cmbProfessors.SelectedIndex = -1;
                }

                // Ciclo, día, capacidad, estado
                numCycleYear.Value = Math.Max(numCycleYear.Minimum, Math.Min(numCycleYear.Maximum, commission.CycleYear));
                cmbDayOfWeek.SelectedItem = commission.DayOfWeek;
                numCapacity.Value = Math.Max(numCapacity.Minimum, Math.Min(numCapacity.Maximum, commission.Capacity));
                cmbStatus.SelectedItem = commission.Status;

                // Horarios
                var (startHour, startMinute) = ScheduleHelper.SplitHourMinute(commission.StartTime);
                var (endHour, endMinute) = ScheduleHelper.SplitHourMinute(commission.EndTime);

                numStartHour.Value = Clamp(numStartHour, startHour);
                numStartMinute.Value = Clamp(numStartMinute, startMinute);
                numEndHour.Value = Clamp(numEndHour, endHour);
                numEndMinute.Value = Clamp(numEndMinute, endMinute);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al cargar los detalles de la comisión: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Helper para limitar valores en NumericUpDown
        private decimal Clamp(NumericUpDown control, int value)
        {
            return Math.Max(control.Minimum, Math.Min(control.Maximum, value));
        }

        // === btnSave_Click (sin cambios mayores, solo mejora de null-safety) ===
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (clbSubjects.CheckedItems.Count != 1)
            {
                MessageBox.Show("Debe seleccionar exactamente una materia.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Validar horarios
                ScheduleHelper.ValidateSchedule((int)numStartHour.Value, (int)numStartMinute.Value);
                ScheduleHelper.ValidateSchedule((int)numEndHour.Value, (int)numEndMinute.Value);

                var startTime = ScheduleHelper.FormatHourMinute((int)numStartHour.Value, (int)numStartMinute.Value);
                var endTime = ScheduleHelper.FormatHourMinute((int)numEndHour.Value, (int)numEndMinute.Value);

                var startTimeSpan = ScheduleHelper.ParseTimeSpan(startTime);
                var endTimeSpan = ScheduleHelper.ParseTimeSpan(endTime);

                var selectedSubject = (SubjectResponseDto)clbSubjects.CheckedItems[0];
                int? professorId = cmbProfessors.SelectedIndex >= 0
                    ? (int?)cmbProfessors.SelectedValue
                    : null;

                string status = CommissionHelper.ResolveStatus(professorId, cmbStatus.SelectedItem?.ToString());

                if (!_commissionId.HasValue)
                {
                    var createDto = new CommissionCreateDto
                    {
                        SubjectId = selectedSubject.Id,
                        ProfessorId = professorId,
                        CycleYear = (int)numCycleYear.Value,
                        DayOfWeek = cmbDayOfWeek.SelectedItem?.ToString() ?? "Lunes",
                        StartTime = startTimeSpan,
                        EndTime = endTimeSpan,
                        Capacity = (int)numCapacity.Value,
                        Status = status
                    };

                    await _apiClient.PostAsync<ResponseFormCommissionEdit>("/api/Commissions", createDto);
                }
                else
                {
                    var updateDto = new CommissionUpdateDto
                    {
                        SubjectId = selectedSubject.Id,
                        ProfessorId = professorId,
                        CycleYear = (int)numCycleYear.Value,
                        DayOfWeek = cmbDayOfWeek.SelectedItem?.ToString() ?? "Lunes",
                        StartTime = startTimeSpan,
                        EndTime = endTimeSpan,
                        Capacity = (int)numCapacity.Value,
                        Status = status
                    };

                    await _apiClient.PutAsync($"/api/Commissions/{_commissionId}", updateDto);
                }

                MessageBox.Show("Comisión guardada con éxito.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clbSubjects_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                BeginInvoke(new Action(() =>
                {
                    for (int i = 0; i < clbSubjects.Items.Count; i++)
                    {
                        if (i != e.Index)
                            clbSubjects.SetItemChecked(i, false);
                    }
                }));
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public class ResponseFormCommissionEdit
        {
            public bool Success { get; set; }
            public string Message { get; set; } = string.Empty;
            public string Details { get; set; } = string.Empty;
        }
    }
}
