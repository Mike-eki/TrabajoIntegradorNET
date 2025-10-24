namespace WinFormsAdmin.Forms.Careers
{
    partial class FormManageCareerSubjects
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
            lblCareerName = new Label();
            groupBox1 = new GroupBox();
            clbSubjects = new CheckedListBox();
            btnSave = new Button();
            btnCancel = new Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // lblCareerName
            // 
            lblCareerName.AutoSize = true;
            lblCareerName.Location = new Point(163, 31);
            lblCareerName.Name = "lblCareerName";
            lblCareerName.Size = new Size(160, 15);
            lblCareerName.TabIndex = 0;
            lblCareerName.Text = "Carrera: [Nombre de Carrera]";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(clbSubjects);
            groupBox1.Location = new Point(94, 59);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(295, 219);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Materias asociadas";
            // 
            // clbSubjects
            // 
            clbSubjects.Dock = DockStyle.Fill;
            clbSubjects.FormattingEnabled = true;
            clbSubjects.Location = new Point(3, 19);
            clbSubjects.Name = "clbSubjects";
            clbSubjects.Size = new Size(289, 197);
            clbSubjects.TabIndex = 0;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(97, 294);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(113, 23);
            btnSave.TabIndex = 2;
            btnSave.Text = "Guardar cambios";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(216, 294);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancelar";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // FormManageCareerSubjects
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 361);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(groupBox1);
            Controls.Add(lblCareerName);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormManageCareerSubjects";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FormManageCareerSubjects";
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblCareerName;
        private GroupBox groupBox1;
        private CheckedListBox clbSubjects;
        private Button btnSave;
        private Button btnCancel;
    }
}