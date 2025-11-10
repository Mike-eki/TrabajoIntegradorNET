namespace WinFormsAdmin.Forms.Users
{
    partial class FormUserManager
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
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            dgvUsers = new DataGridView();
            panel1 = new Panel();
            btnRefresh = new Button();
            btnNew = new Button();
            colId = new DataGridViewTextBoxColumn();
            colUsername = new DataGridViewTextBoxColumn();
            colLegajo = new DataGridViewTextBoxColumn();
            colFullName = new DataGridViewTextBoxColumn();
            colRole = new DataGridViewTextBoxColumn();
            colEmail = new DataGridViewTextBoxColumn();
            colCareers = new DataGridViewTextBoxColumn();
            btnEdit = new DataGridViewButtonColumn();
            btnResetPassword = new DataGridViewButtonColumn();
            btnDelete = new DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // dgvUsers
            // 
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsers.Columns.AddRange(new DataGridViewColumn[] { colId, colUsername, colLegajo, colFullName, colRole, colEmail, colCareers, btnEdit, btnResetPassword, btnDelete });
            dgvUsers.Location = new Point(12, 75);
            dgvUsers.Name = "dgvUsers";
            dgvUsers.ReadOnly = true;
            dgvUsers.Size = new Size(887, 363);
            dgvUsers.TabIndex = 0;
            dgvUsers.CellContentClick += dgvUsers_CellContentClick;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnRefresh);
            panel1.Controls.Add(btnNew);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(206, 57);
            panel1.TabIndex = 1;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(108, 19);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(75, 23);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "Refrescar";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnNew
            // 
            btnNew.Location = new Point(13, 19);
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
            // colUsername
            // 
            colUsername.DataPropertyName = "Username";
            colUsername.HeaderText = "Usuario";
            colUsername.Name = "colUsername";
            colUsername.ReadOnly = true;
            colUsername.Width = 72;
            // 
            // colLegajo
            // 
            colLegajo.DataPropertyName = "Legajo";
            colLegajo.HeaderText = "Legajo";
            colLegajo.Name = "colLegajo";
            colLegajo.ReadOnly = true;
            colLegajo.Width = 67;
            // 
            // colFullName
            // 
            colFullName.DataPropertyName = "FullName";
            colFullName.HeaderText = "Nombre completo";
            colFullName.Name = "colFullName";
            colFullName.ReadOnly = true;
            colFullName.Width = 119;
            // 
            // colRole
            // 
            colRole.DataPropertyName = "Role";
            colRole.HeaderText = "Rol";
            colRole.Name = "colRole";
            colRole.ReadOnly = true;
            colRole.Width = 49;
            // 
            // colEmail
            // 
            colEmail.DataPropertyName = "Email";
            colEmail.HeaderText = "Correo electronico";
            colEmail.Name = "colEmail";
            colEmail.ReadOnly = true;
            colEmail.Width = 119;
            // 
            // colCareers
            // 
            colCareers.DataPropertyName = "CareersSummary";
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            colCareers.DefaultCellStyle = dataGridViewCellStyle1;
            colCareers.HeaderText = "Carreras";
            colCareers.Name = "colCareers";
            colCareers.ReadOnly = true;
            colCareers.Width = 75;
            // 
            // btnEdit
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.Beige;
            btnEdit.DefaultCellStyle = dataGridViewCellStyle2;
            btnEdit.HeaderText = "Editar";
            btnEdit.Name = "btnEdit";
            btnEdit.ReadOnly = true;
            btnEdit.Text = "Editar";
            btnEdit.UseColumnTextForButtonValue = true;
            btnEdit.Width = 43;
            // 
            // btnResetPassword
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(192, 192, 255);
            btnResetPassword.DefaultCellStyle = dataGridViewCellStyle3;
            btnResetPassword.HeaderText = "Restablecer clave";
            btnResetPassword.Name = "btnResetPassword";
            btnResetPassword.ReadOnly = true;
            btnResetPassword.Text = "Restablecer clave";
            btnResetPassword.UseColumnTextForButtonValue = true;
            btnResetPassword.Width = 93;
            // 
            // btnDelete
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(255, 192, 192);
            btnDelete.DefaultCellStyle = dataGridViewCellStyle4;
            btnDelete.HeaderText = "Eliminar";
            btnDelete.Name = "btnDelete";
            btnDelete.ReadOnly = true;
            btnDelete.Text = "Eliminar";
            btnDelete.UseColumnTextForButtonValue = true;
            btnDelete.Width = 56;
            // 
            // FormUserManager
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(960, 450);
            Controls.Add(panel1);
            Controls.Add(dgvUsers);
            Name = "FormUserManager";
            Text = "FormUserManager";
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvUsers;
        private Panel panel1;
        private Button btnRefresh;
        private Button btnNew;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colUsername;
        private DataGridViewTextBoxColumn colLegajo;
        private DataGridViewTextBoxColumn colFullName;
        private DataGridViewTextBoxColumn colRole;
        private DataGridViewTextBoxColumn colEmail;
        private DataGridViewTextBoxColumn colCareers;
        private DataGridViewButtonColumn btnEdit;
        private DataGridViewButtonColumn btnResetPassword;
        private DataGridViewButtonColumn btnDelete;
    }
}