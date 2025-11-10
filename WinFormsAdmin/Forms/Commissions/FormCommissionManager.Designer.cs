namespace WinFormsAdmin.Forms.Commissions
{
    partial class FormCommissionManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            dgvCommissions = new DataGridView();
            panel1 = new Panel();
            btnRefresh = new Button();
            btnNew = new Button();
            colId = new DataGridViewTextBoxColumn();
            colSubject = new DataGridViewTextBoxColumn();
            colProfessor = new DataGridViewTextBoxColumn();
            colCycleYear = new DataGridViewTextBoxColumn();
            colDayOfWeek = new DataGridViewTextBoxColumn();
            colStartTime = new DataGridViewTextBoxColumn();
            colEndTime = new DataGridViewTextBoxColumn();
            colCapacity = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            btnEdit = new DataGridViewButtonColumn();
            btnEnrollStudent = new DataGridViewButtonColumn();
            btnDelete = new DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)dgvCommissions).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // dgvCommissions
            // 
            dgvCommissions.AllowUserToDeleteRows = false;
            dgvCommissions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvCommissions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCommissions.Columns.AddRange(new DataGridViewColumn[] { colId, colSubject, colProfessor, colCycleYear, colDayOfWeek, colStartTime, colEndTime, colCapacity, colStatus, btnEdit, btnEnrollStudent, btnDelete });
            dgvCommissions.Location = new Point(12, 89);
            dgvCommissions.Name = "dgvCommissions";
            dgvCommissions.ReadOnly = true;
            dgvCommissions.Size = new Size(856, 349);
            dgvCommissions.TabIndex = 0;
            dgvCommissions.CellContentClick += dgvCommissions_CellContentClick;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnRefresh);
            panel1.Controls.Add(btnNew);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(213, 71);
            panel1.TabIndex = 1;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(112, 27);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(75, 23);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "Refrescar";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnNew
            // 
            btnNew.Location = new Point(20, 27);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(75, 23);
            btnNew.TabIndex = 0;
            btnNew.Text = "Añadir";
            btnNew.UseVisualStyleBackColor = true;
            btnNew.Click += btnNew_Click;
            // 
            // colId
            // 
            colId.DataPropertyName = "Id";
            colId.HeaderText = "ID";
            colId.Name = "colId";
            colId.ReadOnly = true;
            colId.Width = 43;
            // 
            // colSubject
            // 
            colSubject.DataPropertyName = "SubjectName";
            colSubject.HeaderText = "Materia";
            colSubject.Name = "colSubject";
            colSubject.ReadOnly = true;
            colSubject.Width = 72;
            // 
            // colProfessor
            // 
            colProfessor.DataPropertyName = "ProfessorName";
            colProfessor.HeaderText = "Profesor";
            colProfessor.Name = "colProfessor";
            colProfessor.ReadOnly = true;
            colProfessor.Width = 76;
            // 
            // colCycleYear
            // 
            colCycleYear.DataPropertyName = "CycleYear";
            colCycleYear.HeaderText = "Año";
            colCycleYear.Name = "colCycleYear";
            colCycleYear.ReadOnly = true;
            colCycleYear.Width = 54;
            // 
            // colDayOfWeek
            // 
            colDayOfWeek.DataPropertyName = "DayOfWeek";
            colDayOfWeek.HeaderText = "Día";
            colDayOfWeek.Name = "colDayOfWeek";
            colDayOfWeek.ReadOnly = true;
            colDayOfWeek.Width = 49;
            // 
            // colStartTime
            // 
            colStartTime.DataPropertyName = "StartTime";
            colStartTime.HeaderText = "Horario de inicio";
            colStartTime.Name = "colStartTime";
            colStartTime.ReadOnly = true;
            colStartTime.Width = 84;
            // 
            // colEndTime
            // 
            colEndTime.DataPropertyName = "EndTime";
            colEndTime.HeaderText = "Horario de fin";
            colEndTime.Name = "colEndTime";
            colEndTime.ReadOnly = true;
            colEndTime.Width = 84;
            // 
            // colCapacity
            // 
            colCapacity.DataPropertyName = "Capacity";
            colCapacity.HeaderText = "Capacidad";
            colCapacity.Name = "colCapacity";
            colCapacity.ReadOnly = true;
            colCapacity.Width = 88;
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Estado";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            colStatus.Width = 67;
            // 
            // btnEdit
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.Beige;
            btnEdit.DefaultCellStyle = dataGridViewCellStyle1;
            btnEdit.HeaderText = "Editar";
            btnEdit.Name = "btnEdit";
            btnEdit.ReadOnly = true;
            btnEdit.Text = "Editar";
            btnEdit.UseColumnTextForButtonValue = true;
            btnEdit.Width = 43;
            // 
            // btnEnrollStudent
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(192, 255, 192);
            btnEnrollStudent.DefaultCellStyle = dataGridViewCellStyle2;
            btnEnrollStudent.HeaderText = "Inscribir estudiante";
            btnEnrollStudent.Name = "btnEnrollStudent";
            btnEnrollStudent.ReadOnly = true;
            btnEnrollStudent.Text = "Inscribir estudiante";
            btnEnrollStudent.UseColumnTextForButtonValue = true;
            btnEnrollStudent.Width = 102;
            // 
            // btnDelete
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(255, 192, 192);
            btnDelete.DefaultCellStyle = dataGridViewCellStyle3;
            btnDelete.HeaderText = "Eliminar";
            btnDelete.Name = "btnDelete";
            btnDelete.ReadOnly = true;
            btnDelete.Text = "Eliminar";
            btnDelete.UseColumnTextForButtonValue = true;
            btnDelete.Width = 56;
            // 
            // FormCommissionManager
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(880, 450);
            Controls.Add(panel1);
            Controls.Add(dgvCommissions);
            Name = "FormCommissionManager";
            Text = "FormCommissionManager";
            ((System.ComponentModel.ISupportInitialize)dgvCommissions).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvCommissions;
        private Panel panel1;
        private Button btnRefresh;
        private Button btnNew;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colSubject;
        private DataGridViewTextBoxColumn colProfessor;
        private DataGridViewTextBoxColumn colCycleYear;
        private DataGridViewTextBoxColumn colDayOfWeek;
        private DataGridViewTextBoxColumn colStartTime;
        private DataGridViewTextBoxColumn colEndTime;
        private DataGridViewTextBoxColumn colCapacity;
        private DataGridViewTextBoxColumn colStatus;
        private DataGridViewButtonColumn btnEdit;
        private DataGridViewButtonColumn btnEnrollStudent;
        private DataGridViewButtonColumn btnDelete;
    }
}