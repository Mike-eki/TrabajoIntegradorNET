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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            dgvCareers = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            colSubjects = new DataGridViewTextBoxColumn();
            btnManageSubjects = new DataGridViewButtonColumn();
            btnEdit = new DataGridViewButtonColumn();
            btnDelete = new DataGridViewButtonColumn();
            panel1 = new Panel();
            btnRefresh = new Button();
            btnNew = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvCareers).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // dgvCareers
            // 
            dgvCareers.AllowUserToDeleteRows = false;
            dgvCareers.AllowUserToOrderColumns = true;
            dgvCareers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCareers.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colSubjects, btnManageSubjects, btnEdit, btnDelete });
            dgvCareers.Location = new Point(0, 57);
            dgvCareers.Name = "dgvCareers";
            dgvCareers.ReadOnly = true;
            dgvCareers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCareers.Size = new Size(871, 393);
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
            colSubjects.ReadOnly = true;
            colSubjects.Resizable = DataGridViewTriState.True;
            colSubjects.SortMode = DataGridViewColumnSortMode.NotSortable;
            colSubjects.Width = 120;
            // 
            // btnManageSubjects
            // 
            btnManageSubjects.HeaderText = "Gestionar";
            btnManageSubjects.Name = "btnManageSubjects";
            btnManageSubjects.ReadOnly = true;
            btnManageSubjects.Resizable = DataGridViewTriState.True;
            btnManageSubjects.SortMode = DataGridViewColumnSortMode.Automatic;
            btnManageSubjects.Text = "Gestionar";
            btnManageSubjects.UseColumnTextForButtonValue = true;
            // 
            // btnEdit
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.Beige;
            dataGridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnEdit.DefaultCellStyle = dataGridViewCellStyle1;
            btnEdit.HeaderText = "Editar";
            btnEdit.Name = "btnEdit";
            btnEdit.ReadOnly = true;
            btnEdit.SortMode = DataGridViewColumnSortMode.Automatic;
            btnEdit.Text = "Editar";
            btnEdit.UseColumnTextForButtonValue = true;
            // 
            // btnDelete
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(255, 192, 192);
            dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDelete.DefaultCellStyle = dataGridViewCellStyle2;
            btnDelete.HeaderText = "Eliminar";
            btnDelete.Name = "btnDelete";
            btnDelete.ReadOnly = true;
            btnDelete.Text = "Eliminar";
            btnDelete.UseColumnTextForButtonValue = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnRefresh);
            panel1.Controls.Add(btnNew);
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(211, 51);
            panel1.TabIndex = 1;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(111, 12);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(75, 23);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "Refrescar";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnNew
            // 
            btnNew.Location = new Point(12, 12);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(75, 23);
            btnNew.TabIndex = 0;
            btnNew.Text = "Añadir";
            btnNew.UseVisualStyleBackColor = true;
            btnNew.Click += btnNew_Click;
            // 
            // FormCareerManager
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(871, 450);
            Controls.Add(panel1);
            Controls.Add(dgvCareers);
            Name = "FormCareerManager";
            Text = "FormCareerManager";
            ((System.ComponentModel.ISupportInitialize)dgvCareers).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvCareers;
        private Panel panel1;
        private Button btnNew;
        private Button btnRefresh;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colSubjects;
        private DataGridViewButtonColumn btnManageSubjects;
        private DataGridViewButtonColumn btnEdit;
        private DataGridViewButtonColumn btnDelete;
    }
}