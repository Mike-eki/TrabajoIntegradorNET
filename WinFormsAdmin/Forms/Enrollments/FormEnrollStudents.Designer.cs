namespace WinFormsAdmin.Forms.Enrollments
{
    partial class FormEnrollStudents
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
            clbStudents = new CheckedListBox();
            gbStudents = new GroupBox();
            lblCommissionName = new Label();
            lblCapacity = new Label();
            lblCurrent = new Label();
            lblRemaining = new Label();
            btnEnroll = new Button();
            btnCancel = new Button();
            panel1 = new Panel();
            panel2 = new Panel();
            panel3 = new Panel();
            btnSelectAll = new Button();
            btnDeselectAll = new Button();
            gbStudents.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // clbStudents
            // 
            clbStudents.Dock = DockStyle.Fill;
            clbStudents.FormattingEnabled = true;
            clbStudents.Location = new Point(3, 19);
            clbStudents.Name = "clbStudents";
            clbStudents.Size = new Size(370, 332);
            clbStudents.TabIndex = 0;
            // 
            // gbStudents
            // 
            gbStudents.Controls.Add(clbStudents);
            gbStudents.Location = new Point(12, 12);
            gbStudents.Name = "gbStudents";
            gbStudents.Size = new Size(376, 354);
            gbStudents.TabIndex = 1;
            gbStudents.TabStop = false;
            gbStudents.Text = "Estudiantes";
            // 
            // lblCommissionName
            // 
            lblCommissionName.AutoSize = true;
            lblCommissionName.Location = new Point(19, 13);
            lblCommissionName.Name = "lblCommissionName";
            lblCommissionName.Size = new Size(186, 15);
            lblCommissionName.TabIndex = 2;
            lblCommissionName.Text = "Comisión: [Nombre de Comisión]";
            // 
            // lblCapacity
            // 
            lblCapacity.AutoSize = true;
            lblCapacity.Location = new Point(19, 44);
            lblCapacity.Name = "lblCapacity";
            lblCapacity.Size = new Size(121, 15);
            lblCapacity.TabIndex = 3;
            lblCapacity.Text = "Capacidad: [Número]";
            // 
            // lblCurrent
            // 
            lblCurrent.AutoSize = true;
            lblCurrent.Location = new Point(19, 72);
            lblCurrent.Name = "lblCurrent";
            lblCurrent.Size = new Size(116, 15);
            lblCurrent.TabIndex = 4;
            lblCurrent.Text = "Inscriptos: [Número]";
            // 
            // lblRemaining
            // 
            lblRemaining.AutoSize = true;
            lblRemaining.Location = new Point(19, 96);
            lblRemaining.Name = "lblRemaining";
            lblRemaining.Size = new Size(159, 15);
            lblRemaining.TabIndex = 5;
            lblRemaining.Text = "Cupos disponibles [Número]";
            // 
            // btnEnroll
            // 
            btnEnroll.Location = new Point(19, 24);
            btnEnroll.Name = "btnEnroll";
            btnEnroll.Size = new Size(75, 23);
            btnEnroll.TabIndex = 6;
            btnEnroll.Text = "Inscribir";
            btnEnroll.UseVisualStyleBackColor = true;
            btnEnroll.Click += btnEnroll_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(103, 24);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Cancelar";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(lblCommissionName);
            panel1.Controls.Add(lblCapacity);
            panel1.Controls.Add(lblCurrent);
            panel1.Controls.Add(lblRemaining);
            panel1.Location = new Point(441, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(347, 132);
            panel1.TabIndex = 8;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnCancel);
            panel2.Controls.Add(btnEnroll);
            panel2.Location = new Point(441, 150);
            panel2.Name = "panel2";
            panel2.Size = new Size(347, 66);
            panel2.TabIndex = 9;
            // 
            // panel3
            // 
            panel3.Controls.Add(btnDeselectAll);
            panel3.Controls.Add(btnSelectAll);
            panel3.Location = new Point(12, 372);
            panel3.Name = "panel3";
            panel3.Size = new Size(376, 66);
            panel3.TabIndex = 10;
            // 
            // btnSelectAll
            // 
            btnSelectAll.Location = new Point(46, 22);
            btnSelectAll.Name = "btnSelectAll";
            btnSelectAll.Size = new Size(120, 23);
            btnSelectAll.TabIndex = 0;
            btnSelectAll.Text = "Seleccionar todo";
            btnSelectAll.UseVisualStyleBackColor = true;
            btnSelectAll.Click += btnSelectAll_Click;
            // 
            // btnDeselectAll
            // 
            btnDeselectAll.Location = new Point(206, 22);
            btnDeselectAll.Name = "btnDeselectAll";
            btnDeselectAll.Size = new Size(113, 23);
            btnDeselectAll.TabIndex = 1;
            btnDeselectAll.Text = "Desmarcar todo";
            btnDeselectAll.UseVisualStyleBackColor = true;
            btnDeselectAll.Click += btnDeselectAll_Click;
            // 
            // FormEnrollStudents
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(gbStudents);
            Name = "FormEnrollStudents";
            Text = "FormEnrollStudents";
            gbStudents.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private CheckedListBox clbStudents;
        private GroupBox gbStudents;
        private Label lblCommissionName;
        private Label lblCapacity;
        private Label lblCurrent;
        private Label lblRemaining;
        private Button btnEnroll;
        private Button btnCancel;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Button btnDeselectAll;
        private Button btnSelectAll;
    }
}