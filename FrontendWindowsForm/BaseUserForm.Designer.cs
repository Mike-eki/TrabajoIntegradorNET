namespace FrontendWindowsForm
{
    partial class BaseUserForm
    {
        /// <summamary>
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
            btnViewUsers = new Button();
            btnViewCourses = new Button();
            btnViewCommissions = new Button();
            mainPanel = new Panel();
            userLabel = new Label();
            courseLabel = new Label();
            commisionLabel = new Label();
            btnLogout = new Button();
            btnCreateUser = new Button();
            btnCreateCourse = new Button();
            btnCreateCommission = new Button();
            SuspendLayout();
            // 
            // btnViewUsers
            // 
            btnViewUsers.Location = new Point(19, 78);
            btnViewUsers.Name = "btnViewUsers";
            btnViewUsers.Size = new Size(75, 23);
            btnViewUsers.TabIndex = 0;
            btnViewUsers.Text = "Ver Lista";
            btnViewUsers.UseVisualStyleBackColor = true;
            // 
            // btnViewCourses
            // 
            btnViewCourses.Location = new Point(173, 78);
            btnViewCourses.Name = "btnViewCourses";
            btnViewCourses.Size = new Size(75, 23);
            btnViewCourses.TabIndex = 1;
            btnViewCourses.Text = "Ver Lista";
            btnViewCourses.UseVisualStyleBackColor = true;
            // 
            // btnViewCommissions
            // 
            btnViewCommissions.Location = new Point(322, 78);
            btnViewCommissions.Name = "btnViewCommissions";
            btnViewCommissions.Size = new Size(75, 23);
            btnViewCommissions.TabIndex = 2;
            btnViewCommissions.Text = "Ver Lista";
            btnViewCommissions.UseVisualStyleBackColor = true;
            // 
            // mainPanel
            // 
            mainPanel.Location = new Point(12, 146);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(776, 292);
            mainPanel.TabIndex = 3;
            // 
            // userLabel
            // 
            userLabel.AutoSize = true;
            userLabel.Location = new Point(19, 45);
            userLabel.Name = "userLabel";
            userLabel.Size = new Size(52, 15);
            userLabel.TabIndex = 4;
            userLabel.Text = "Usuarios";
            // 
            // courseLabel
            // 
            courseLabel.AutoSize = true;
            courseLabel.Location = new Point(173, 45);
            courseLabel.Name = "courseLabel";
            courseLabel.Size = new Size(43, 15);
            courseLabel.TabIndex = 5;
            courseLabel.Text = "Cursos";
            // 
            // commisionLabel
            // 
            commisionLabel.AutoSize = true;
            commisionLabel.Location = new Point(322, 45);
            commisionLabel.Name = "commisionLabel";
            commisionLabel.Size = new Size(69, 15);
            commisionLabel.TabIndex = 6;
            commisionLabel.Text = "Comisiones";
            // 
            // btnLogout
            // 
            btnLogout.Location = new Point(687, 119);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(88, 23);
            btnLogout.TabIndex = 7;
            btnLogout.Text = "Cerrar Sesión";
            btnLogout.UseVisualStyleBackColor = true;
            // 
            // btnCreateUser
            // 
            btnCreateUser.Location = new Point(19, 107);
            btnCreateUser.Name = "btnCreateUser";
            btnCreateUser.Size = new Size(75, 23);
            btnCreateUser.TabIndex = 8;
            btnCreateUser.Text = "Crear";
            btnCreateUser.UseVisualStyleBackColor = true;
            // 
            // btnCreateCourse
            // 
            btnCreateCourse.Location = new Point(173, 107);
            btnCreateCourse.Name = "btnCreateCourse";
            btnCreateCourse.Size = new Size(75, 23);
            btnCreateCourse.TabIndex = 9;
            btnCreateCourse.Text = "Crear";
            btnCreateCourse.UseVisualStyleBackColor = true;
            // 
            // btnCreateCommission
            // 
            btnCreateCommission.Location = new Point(322, 107);
            btnCreateCommission.Name = "btnCreateCommission";
            btnCreateCommission.Size = new Size(75, 23);
            btnCreateCommission.TabIndex = 10;
            btnCreateCommission.Text = "Crear";
            btnCreateCommission.UseVisualStyleBackColor = true;
            // 
            // BaseUserForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnCreateCommission);
            Controls.Add(btnCreateCourse);
            Controls.Add(btnCreateUser);
            Controls.Add(btnLogout);
            Controls.Add(commisionLabel);
            Controls.Add(courseLabel);
            Controls.Add(userLabel);
            Controls.Add(mainPanel);
            Controls.Add(btnViewCommissions);
            Controls.Add(btnViewCourses);
            Controls.Add(btnViewUsers);
            Name = "BaseUserForm";
            Text = "BaseUserForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnViewUsers;
        private Button btnViewCourses;
        private Button btnViewCommissions;
        private Panel mainPanel;
        private Label userLabel;
        private Label courseLabel;
        private Label commisionLabel;
        private Button btnLogout;
        private Button btnCreateUser;
        private Button btnCreateCourse;
        private Button btnCreateCommission;
    }
}