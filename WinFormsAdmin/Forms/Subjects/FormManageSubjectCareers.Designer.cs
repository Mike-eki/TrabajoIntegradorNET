namespace WinFormsAdmin.Forms.Subjects
{
    partial class FormManageSubjectCareers
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
            lblSubjectName = new Label();
            groupBox1 = new GroupBox();
            clbCareers = new CheckedListBox();
            btnSave = new Button();
            btnCancel = new Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // lblSubjectName
            // 
            lblSubjectName.AutoSize = true;
            lblSubjectName.Location = new Point(316, 60);
            lblSubjectName.Name = "lblSubjectName";
            lblSubjectName.Size = new Size(164, 15);
            lblSubjectName.TabIndex = 0;
            lblSubjectName.Text = "Materia: [Nombre de Materia]";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(clbCareers);
            groupBox1.Location = new Point(157, 89);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(474, 277);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Carreras asociadas";
            // 
            // clbCareers
            // 
            clbCareers.Dock = DockStyle.Fill;
            clbCareers.FormattingEnabled = true;
            clbCareers.Location = new Point(3, 19);
            clbCareers.Name = "clbCareers";
            clbCareers.Size = new Size(468, 255);
            clbCareers.TabIndex = 0;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(160, 387);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(122, 23);
            btnSave.TabIndex = 2;
            btnSave.Text = "Guardar cambios";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(299, 387);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancelar";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // FormManageSubjectCareers
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(groupBox1);
            Controls.Add(lblSubjectName);
            Name = "FormManageSubjectCareers";
            Text = "FormManageSubjectCareers";
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblSubjectName;
        private GroupBox groupBox1;
        private CheckedListBox clbCareers;
        private Button btnSave;
        private Button btnCancel;
    }
}