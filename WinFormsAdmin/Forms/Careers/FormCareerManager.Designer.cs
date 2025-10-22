namespace WinFormsAdmin.Forms.Careers
{
    partial class FormCareerManager
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
            dgvCareers = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            colSubjects = new DataGridViewTextBoxColumn();
            btnManageSubjects = new DataGridViewButtonColumn();
            btnEdit = new DataGridViewButtonColumn();
            btnDelete = new DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)dgvCareers).BeginInit();
            SuspendLayout();
            // 
            // dgvCareers
            // 
            dgvCareers.AllowUserToAddRows = false;
            dgvCareers.AllowUserToDeleteRows = false;
            dgvCareers.AllowUserToOrderColumns = true;
            dgvCareers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCareers.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colSubjects, btnManageSubjects, btnEdit, btnDelete });
            dgvCareers.Dock = DockStyle.Fill;
            dgvCareers.Location = new Point(0, 0);
            dgvCareers.Name = "dgvCareers";
            dgvCareers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCareers.Size = new Size(871, 450);
            dgvCareers.TabIndex = 0;
            dgvCareers.CellContentClick += dgvCareers_CellContentClick;
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
            colName.HeaderText = "Nombre de la Carrera";
            colName.Name = "colName";
            colName.ReadOnly = true;
            colName.Width = 250;
            // 
            // colSubjects
            // 
            colSubjects.HeaderText = "Materias";
            colSubjects.Name = "colSubjects";
            colSubjects.Resizable = DataGridViewTriState.True;
            colSubjects.SortMode = DataGridViewColumnSortMode.NotSortable;
            colSubjects.Width = 120;
            // 
            // btnManageSubjects
            // 
            btnManageSubjects.HeaderText = "Gestionar";
            btnManageSubjects.Name = "btnManageSubjects";
            btnManageSubjects.Resizable = DataGridViewTriState.True;
            btnManageSubjects.SortMode = DataGridViewColumnSortMode.Automatic;
            btnManageSubjects.Text = "Gestionar";
            btnManageSubjects.UseColumnTextForButtonValue = true;
            // 
            // btnEdit
            // 
            btnEdit.HeaderText = "Editar";
            btnEdit.Name = "btnEdit";
            btnEdit.Text = "Editar";
            // 
            // btnDelete
            // 
            btnDelete.HeaderText = "Eliminar";
            btnDelete.Name = "btnDelete";
            btnDelete.Text = "Eliminar";
            // 
            // FormCareerManager
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(871, 450);
            Controls.Add(dgvCareers);
            Name = "FormCareerManager";
            Text = "FormCareerManager";
            ((System.ComponentModel.ISupportInitialize)dgvCareers).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvCareers;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colSubjects;
        private DataGridViewButtonColumn btnManageSubjects;
        private DataGridViewButtonColumn btnEdit;
        private DataGridViewButtonColumn btnDelete;
    }
}