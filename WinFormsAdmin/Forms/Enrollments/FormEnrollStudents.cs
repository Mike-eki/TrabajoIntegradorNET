using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsAdmin.Services;

namespace WinFormsAdmin.Forms.Enrollments
{
    public partial class FormEnrollStudents : Form
    {
        private readonly ApiClient _apiClient;
        private readonly int _commissionId;
        private readonly int _subjectId;
        private readonly string _commissionDisplayName;

        private CommissionWithProfessorDto? _commission;
        private List<EnrollmentResponseDto> _currentEnrollments = new();
        private List<UserResponseDto> _availableStudents = new();

        public FormEnrollStudents(ApiClient apiClient, int commissionId, int subjectId, string commissionDisplayName)
        {
            InitializeComponent();
            _apiClient = apiClient;
            _commissionId = commissionId;
            _subjectId = subjectId;
            _commissionDisplayName = commissionDisplayName;

            lblCommissionName.Text = commissionDisplayName;

            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                // Traer comisión y enrollments en paralelo
                var commissionTask = _apiClient.GetAsync<CommissionWithProfessorDto>($"/api/Commissions/{_commissionId}");
                var enrollmentsTask = _apiClient.GetListAsync<EnrollmentResponseDto>($"/api/Enrollments/commission/{_commissionId}");

                await Task.WhenAll(commissionTask, enrollmentsTask);

                _commission = commissionTask.Result;
                _currentEnrollments = enrollmentsTask.Result ?? new List<EnrollmentResponseDto>();

                // Traer estudiantes FILTRADOS por subject (ideal en server)
                _availableStudents = await _apiClient.GetListAsync<UserResponseDto>($"/api/Users?role=Student&subjectId={_subjectId}") ?? new List<UserResponseDto>();

                // Excluir los que ya están inscriptos
                var enrolledStudentIds = _currentEnrollments.Select(e => e.StudentId).ToHashSet();
                _availableStudents = _availableStudents.Where(s => !enrolledStudentIds.Contains(s.Id)).ToList();

                PopulateCheckedListBox();
                UpdateCapacityLabels();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void PopulateCheckedListBox()
        {
            clbStudents.BeginUpdate();
            clbStudents.Items.Clear();

            foreach (var s in _availableStudents)
            {
                // Asegurate que UserResponseDto tenga ToString() o usá DisplayMember
                clbStudents.Items.Add(s);
            }

            clbStudents.DisplayMember = "FullName"; // mostrar nombre
            clbStudents.EndUpdate();
        }

        private void UpdateCapacityLabels()
        {
            if (_commission == null) return;
            int capacity = _commission.Capacity;
            int current = _currentEnrollments.Count;
            int remaining = Math.Max(0, capacity - current);
            lblCapacity.Text = $"Capacidad: {capacity}";
            lblCurrent.Text = $"Inscriptos: {current}";
            lblRemaining.Text = $"Cupos disponibles: {remaining}";
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbStudents.Items.Count; i++)
                clbStudents.SetItemChecked(i, true);
        }

        private void btnDeselectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbStudents.Items.Count; i++)
                clbStudents.SetItemChecked(i, false);
        }

        private async void btnEnroll_Click(object sender, EventArgs e)
        {
            try
            {
                var selected = clbStudents.CheckedItems.OfType<UserResponseDto>().ToList();
                if (!selected.Any())
                {
                    MessageBox.Show("Seleccione al menos un estudiante para inscribir.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Comprobar cupos disponibles en cliente
                int capacity = _commission?.Capacity ?? 0;
                int current = _currentEnrollments.Count;
                int remaining = Math.Max(0, capacity - current);

                if (selected.Count > remaining)
                {
                    MessageBox.Show($"Ha seleccionado {selected.Count} estudiantes, pero solo quedan {remaining} cupos. Seleccione menos estudiantes o cancele.", "Cupos insuficientes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Preparar payload para bulk enroll
                var dto = new
                {
                    CommissionId = _commissionId,
                    StudentIds = selected.Select(s => s.Id).ToArray()
                };

                btnEnroll.Enabled = false;
                btnCancel.Enabled = false;

                // Llamada bulk (recomendada)
                var response = await _apiClient.PostAsync<object>($"/api/Enrollments/bulk", dto);

                // Si no hay excepción, asumimos éxito (o manejar respuesta específica)
                MessageBox.Show($"Se inscribieron {selected.Count} estudiante(s) correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error de conexión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Si el servidor devuelve que no hay cupos, idealmente su body incluye remainingSeats
                MessageBox.Show($"Error al inscribir: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // recargar para sincronizar estado
                await Task.Delay(200);
                LoadData();
            }
            finally
            {
                btnEnroll.Enabled = true;
                btnCancel.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
