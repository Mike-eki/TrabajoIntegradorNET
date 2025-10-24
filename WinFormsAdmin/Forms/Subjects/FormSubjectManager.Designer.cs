namespace WinFormsAdmin.Forms.Subjects
{
    partial class FormSubjectManager
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
            dgvSubjects = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            colCareers = new DataGridViewTextBoxColumn();
            btnManageCareers = new DataGridViewButtonColumn();
            btnEdit = new DataGridViewButtonColumn();
            btnDelete = new DataGridViewButtonColumn();
            panel1 = new Panel();
            btnRefresh = new Button();
            btnNew = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvSubjects).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // dgvSubjects
            // 
            dgvSubjects.AllowUserToDeleteRows = false;
            dgvSubjects.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSubjects.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colCareers, btnManageCareers, btnEdit, btnDelete });
            dgvSubjects.Location = new Point(12, 99);
            dgvSubjects.Name = "dgvSubjects";
            dgvSubjects.ReadOnly = true;
            dgvSubjects.Size = new Size(776, 339);
            dgvSubjects.TabIndex = 0;
            dgvSubjects.CellContentClick += dgvSubjects_CellContentClick;
            // 
            // colId
            // 
            colId.DataPropertyName = "Id";
            colId.HeaderText = "ID";
            colId.Name = "colId";
            colId.ReadOnly = true;
            colId.Width = 50;
            // 
            // colName
            // 
            colName.DataPropertyName = "Name";
            colName.HeaderText = "Nombre de Materia";
            colName.Name = "colName";
            colName.ReadOnly = true;
            colName.Width = 200;
            // 
            // colCareers
            // 
            colCareers.HeaderText = "Carreras";
            colCareers.Name = "colCareers";
            colCareers.ReadOnly = true;
            colCareers.Resizable = DataGridViewTriState.True;
            // 
            // btnManageCareers
            // 
            btnManageCareers.HeaderText = "Gestionar";
            btnManageCareers.Name = "btnManageCareers";
            btnManageCareers.ReadOnly = true;
            btnManageCareers.Text = "Gestionar";
            btnManageCareers.UseColumnTextForButtonValue = true;
            // 
            // btnEdit
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.Beige;
            btnEdit.DefaultCellStyle = dataGridViewCellStyle1;
            btnEdit.HeaderText = "Editar";
            btnEdit.Name = "btnEdit";
            btnEdit.ReadOnly = true;
            btnEdit.Resizable = DataGridViewTriState.True;
            btnEdit.SortMode = DataGridViewColumnSortMode.Automatic;
            btnEdit.Text = "Editar";
            btnEdit.UseColumnTextForButtonValue = true;
            // 
            // btnDelete
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(255, 192, 192);
            btnDelete.DefaultCellStyle = dataGridViewCellStyle2;
            btnDelete.HeaderText = "Eliminar";
            btnDelete.Name = "btnDelete";
            btnDelete.ReadOnly = true;
            btnDelete.Resizable = DataGridViewTriState.True;
            btnDelete.SortMode = DataGridViewColumnSortMode.Automatic;
            btnDelete.Text = "Eliminar";
            btnDelete.UseColumnTextForButtonValue = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnRefresh);
            panel1.Controls.Add(btnNew);
            panel1.Location = new Point(12, 10);
            panel1.Name = "panel1";
            panel1.Size = new Size(211, 83);
            panel1.TabIndex = 1;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(114, 32);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(75, 23);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "Refrescar";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnNew
            // 
            btnNew.Location = new Point(18, 32);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(75, 23);
            btnNew.TabIndex = 0;
            btnNew.Text = "Añadir";
            btnNew.UseVisualStyleBackColor = true;
            btnNew.Click += btnNew_Click;
            // 
            // FormSubjectManager
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel1);
            Controls.Add(dgvSubjects);
            Name = "FormSubjectManager";
            Text = "FormSubjectManager";
            ((System.ComponentModel.ISupportInitialize)dgvSubjects).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvSubjects;
        private Panel panel1;
        private Button btnRefresh;
        private Button btnNew;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colCareers;
        private DataGridViewButtonColumn btnManageCareers;
        private DataGridViewButtonColumn btnEdit;
        private DataGridViewButtonColumn btnDelete;
    }
}