namespace WinFormsAdmin.Forms.Users
{
    partial class FormUserEdit
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
            txtUsername = new TextBox();
            txtLegajo = new TextBox();
            lblUsername = new Label();
            lblLegajo = new Label();
            txtFullName = new TextBox();
            lblFullName = new Label();
            txtEmail = new TextBox();
            lblEmail = new Label();
            cmbRole = new ComboBox();
            lblRol = new Label();
            btnSave = new Button();
            btnCancel = new Button();
            txtInitialPassword = new TextBox();
            lblInitialPassword = new Label();
            btnGeneratePassword = new Button();
            gbCareers = new GroupBox();
            clbCareers = new CheckedListBox();
            gbCareers.SuspendLayout();
            SuspendLayout();
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(137, 25);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(195, 23);
            txtUsername.TabIndex = 0;
            // 
            // txtLegajo
            // 
            txtLegajo.Location = new Point(137, 72);
            txtLegajo.Name = "txtLegajo";
            txtLegajo.Size = new Size(195, 23);
            txtLegajo.TabIndex = 1;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(70, 28);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(50, 15);
            lblUsername.TabIndex = 2;
            lblUsername.Text = "Usuario:";
            // 
            // lblLegajo
            // 
            lblLegajo.AutoSize = true;
            lblLegajo.Location = new Point(75, 75);
            lblLegajo.Name = "lblLegajo";
            lblLegajo.Size = new Size(45, 15);
            lblLegajo.TabIndex = 3;
            lblLegajo.Text = "Legajo:";
            // 
            // txtFullName
            // 
            txtFullName.Location = new Point(523, 25);
            txtFullName.Name = "txtFullName";
            txtFullName.Size = new Size(195, 23);
            txtFullName.TabIndex = 4;
            // 
            // lblFullName
            // 
            lblFullName.AutoSize = true;
            lblFullName.Location = new Point(398, 28);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(108, 15);
            lblFullName.TabIndex = 5;
            lblFullName.Text = "Nombre completo:";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(523, 67);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(195, 23);
            txtEmail.TabIndex = 6;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(398, 70);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(108, 15);
            lblEmail.TabIndex = 7;
            lblEmail.Text = "Correo electronico:";
            // 
            // cmbRole
            // 
            cmbRole.FormattingEnabled = true;
            cmbRole.Items.AddRange(new object[] { "Admin", "Student", "Professor" });
            cmbRole.Location = new Point(523, 112);
            cmbRole.Name = "cmbRole";
            cmbRole.Size = new Size(195, 23);
            cmbRole.TabIndex = 8;
            // 
            // lblRol
            // 
            lblRol.AutoSize = true;
            lblRol.Location = new Point(479, 115);
            lblRol.Name = "lblRol";
            lblRol.Size = new Size(27, 15);
            lblRol.TabIndex = 9;
            lblRol.Text = "Rol:";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(294, 397);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 10;
            btnSave.Text = "Guardar";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(414, 397);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 11;
            btnCancel.Text = "Canelar";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // txtInitialPassword
            // 
            txtInitialPassword.Location = new Point(137, 119);
            txtInitialPassword.Name = "txtInitialPassword";
            txtInitialPassword.Size = new Size(195, 23);
            txtInitialPassword.TabIndex = 12;
            txtInitialPassword.TabStop = false;
            // 
            // lblInitialPassword
            // 
            lblInitialPassword.AutoSize = true;
            lblInitialPassword.Location = new Point(16, 122);
            lblInitialPassword.Name = "lblInitialPassword";
            lblInitialPassword.Size = new Size(104, 15);
            lblInitialPassword.TabIndex = 13;
            lblInitialPassword.Text = "Contraseña inicial:";
            // 
            // btnGeneratePassword
            // 
            btnGeneratePassword.Location = new Point(169, 148);
            btnGeneratePassword.Name = "btnGeneratePassword";
            btnGeneratePassword.Size = new Size(132, 23);
            btnGeneratePassword.TabIndex = 14;
            btnGeneratePassword.Text = "Generar contraseña";
            btnGeneratePassword.UseVisualStyleBackColor = true;
            btnGeneratePassword.Click += btnGeneratePassword_Click;
            // 
            // gbCareers
            // 
            gbCareers.Controls.Add(clbCareers);
            gbCareers.Location = new Point(169, 204);
            gbCareers.Name = "gbCareers";
            gbCareers.Size = new Size(448, 162);
            gbCareers.TabIndex = 15;
            gbCareers.TabStop = false;
            gbCareers.Text = "Carreras disponibles";
            // 
            // clbCareers
            // 
            clbCareers.Dock = DockStyle.Fill;
            clbCareers.FormattingEnabled = true;
            clbCareers.Location = new Point(3, 19);
            clbCareers.Name = "clbCareers";
            clbCareers.Size = new Size(442, 140);
            clbCareers.TabIndex = 0;
            // 
            // FormUserEdit
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(gbCareers);
            Controls.Add(btnGeneratePassword);
            Controls.Add(lblInitialPassword);
            Controls.Add(txtInitialPassword);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(lblRol);
            Controls.Add(cmbRole);
            Controls.Add(lblEmail);
            Controls.Add(txtEmail);
            Controls.Add(lblFullName);
            Controls.Add(txtFullName);
            Controls.Add(lblLegajo);
            Controls.Add(lblUsername);
            Controls.Add(txtLegajo);
            Controls.Add(txtUsername);
            Name = "FormUserEdit";
            Text = "FormUserEdit";
            Load += FormUserEdit_Load;
            gbCareers.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtUsername;
        private TextBox txtLegajo;
        private Label lblUsername;
        private Label lblLegajo;
        private TextBox txtFullName;
        private Label lblFullName;
        private TextBox txtEmail;
        private Label lblEmail;
        private ComboBox cmbRole;
        private Label lblRol;
        private Button btnSave;
        private Button btnCancel;
        private TextBox txtInitialPassword;
        private Label lblInitialPassword;
        private Button btnGeneratePassword;
        private GroupBox gbCareers;
        private CheckedListBox clbCareers;
    }
}