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
            SuspendLayout();
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(284, 38);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(195, 23);
            txtUsername.TabIndex = 0;
            // 
            // txtLegajo
            // 
            txtLegajo.Location = new Point(284, 85);
            txtLegajo.Name = "txtLegajo";
            txtLegajo.Size = new Size(195, 23);
            txtLegajo.TabIndex = 1;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(217, 41);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(50, 15);
            lblUsername.TabIndex = 2;
            lblUsername.Text = "Usuario:";
            // 
            // lblLegajo
            // 
            lblLegajo.AutoSize = true;
            lblLegajo.Location = new Point(222, 88);
            lblLegajo.Name = "lblLegajo";
            lblLegajo.Size = new Size(45, 15);
            lblLegajo.TabIndex = 3;
            lblLegajo.Text = "Legajo:";
            // 
            // txtFullName
            // 
            txtFullName.Location = new Point(284, 129);
            txtFullName.Name = "txtFullName";
            txtFullName.Size = new Size(195, 23);
            txtFullName.TabIndex = 4;
            // 
            // lblFullName
            // 
            lblFullName.AutoSize = true;
            lblFullName.Location = new Point(159, 132);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(108, 15);
            lblFullName.TabIndex = 5;
            lblFullName.Text = "Nombre completo:";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(284, 171);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(195, 23);
            txtEmail.TabIndex = 6;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(159, 174);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(108, 15);
            lblEmail.TabIndex = 7;
            lblEmail.Text = "Correo electronico:";
            // 
            // cmbRole
            // 
            cmbRole.FormattingEnabled = true;
            cmbRole.Items.AddRange(new object[] { "Admin", "Student", "Professor" });
            cmbRole.Location = new Point(284, 215);
            cmbRole.Name = "cmbRole";
            cmbRole.Size = new Size(195, 23);
            cmbRole.TabIndex = 8;
            // 
            // lblRol
            // 
            lblRol.AutoSize = true;
            lblRol.Location = new Point(240, 218);
            lblRol.Name = "lblRol";
            lblRol.Size = new Size(27, 15);
            lblRol.TabIndex = 9;
            lblRol.Text = "Rol:";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(284, 317);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 10;
            btnSave.Text = "Guardar";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(404, 317);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 11;
            btnCancel.Text = "Canelar";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // txtInitialPassword
            // 
            txtInitialPassword.Location = new Point(284, 260);
            txtInitialPassword.Name = "txtInitialPassword";
            txtInitialPassword.Size = new Size(195, 23);
            txtInitialPassword.TabIndex = 12;
            txtInitialPassword.TabStop = false;
            // 
            // lblInitialPassword
            // 
            lblInitialPassword.AutoSize = true;
            lblInitialPassword.Location = new Point(163, 263);
            lblInitialPassword.Name = "lblInitialPassword";
            lblInitialPassword.Size = new Size(104, 15);
            lblInitialPassword.TabIndex = 13;
            lblInitialPassword.Text = "Contraseña inicial:";
            // 
            // btnGeneratePassword
            // 
            btnGeneratePassword.Location = new Point(504, 260);
            btnGeneratePassword.Name = "btnGeneratePassword";
            btnGeneratePassword.Size = new Size(132, 23);
            btnGeneratePassword.TabIndex = 14;
            btnGeneratePassword.Text = "Generar contraseña";
            btnGeneratePassword.UseVisualStyleBackColor = true;
            btnGeneratePassword.Click += btnGeneratePassword_Click;
            // 
            // FormUserEdit
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
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
    }
}