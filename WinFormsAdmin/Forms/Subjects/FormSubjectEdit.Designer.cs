namespace WinFormsAdmin.Forms.Subjects
{
    partial class FormSubjectEdit
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
            txtName = new TextBox();
            btnSave = new Button();
            lblSubject = new Label();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // txtName
            // 
            txtName.Location = new Point(299, 137);
            txtName.Name = "txtName";
            txtName.Size = new Size(194, 23);
            txtName.TabIndex = 0;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(299, 197);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(106, 23);
            btnSave.TabIndex = 1;
            btnSave.Text = "Guardar cambios";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // lblSubject
            // 
            lblSubject.AutoSize = true;
            lblSubject.Location = new Point(170, 140);
            lblSubject.Name = "lblSubject";
            lblSubject.Size = new Size(113, 15);
            lblSubject.TabIndex = 2;
            lblSubject.Text = "Nombre de Materia:";
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(418, 197);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancelar";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // FormSubjectEdit
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnCancel);
            Controls.Add(lblSubject);
            Controls.Add(btnSave);
            Controls.Add(txtName);
            Name = "FormSubjectEdit";
            Text = "FormSubjectEdit";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtName;
        private Button btnSave;
        private Label lblSubject;
        private Button btnCancel;
    }
}