namespace WinFormsAdmin.Forms.Users
{
    partial class FormResetPassword
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
            lblUsername = new Label();
            txtNewPassword = new TextBox();
            lblNewPassword = new Label();
            btnResetPassword = new Button();
            btnCancel = new Button();
            btnGeneratePassword = new Button();
            SuspendLayout();
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(289, 146);
            txtUsername.Name = "txtUsername";
            txtUsername.ReadOnly = true;
            txtUsername.Size = new Size(196, 23);
            txtUsername.TabIndex = 0;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(221, 149);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(50, 15);
            lblUsername.TabIndex = 1;
            lblUsername.Text = "Usuario:";
            // 
            // txtNewPassword
            // 
            txtNewPassword.Location = new Point(289, 187);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.Size = new Size(196, 23);
            txtNewPassword.TabIndex = 2;
            // 
            // lblNewPassword
            // 
            lblNewPassword.AutoSize = true;
            lblNewPassword.Location = new Point(166, 190);
            lblNewPassword.Name = "lblNewPassword";
            lblNewPassword.Size = new Size(105, 15);
            lblNewPassword.TabIndex = 3;
            lblNewPassword.Text = "Nueva contraseña:";
            // 
            // btnResetPassword
            // 
            btnResetPassword.Location = new Point(289, 233);
            btnResetPassword.Name = "btnResetPassword";
            btnResetPassword.Size = new Size(75, 23);
            btnResetPassword.TabIndex = 4;
            btnResetPassword.Text = "Restablecer";
            btnResetPassword.UseVisualStyleBackColor = true;
            btnResetPassword.Click += btnReset_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(410, 233);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Cancelar";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnGeneratePassword
            // 
            btnGeneratePassword.Location = new Point(508, 187);
            btnGeneratePassword.Name = "btnGeneratePassword";
            btnGeneratePassword.Size = new Size(128, 23);
            btnGeneratePassword.TabIndex = 6;
            btnGeneratePassword.Text = "Generar contraseña";
            btnGeneratePassword.UseVisualStyleBackColor = true;
            btnGeneratePassword.Click += btnGenerate_Click;
            // 
            // FormResetPassword
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnGeneratePassword);
            Controls.Add(btnCancel);
            Controls.Add(btnResetPassword);
            Controls.Add(lblNewPassword);
            Controls.Add(txtNewPassword);
            Controls.Add(lblUsername);
            Controls.Add(txtUsername);
            Name = "FormResetPassword";
            Text = "FormResetPassword";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtUsername;
        private Label lblUsername;
        private TextBox txtNewPassword;
        private Label lblNewPassword;
        private Button btnResetPassword;
        private Button btnCancel;
        private Button btnGeneratePassword;
    }
}