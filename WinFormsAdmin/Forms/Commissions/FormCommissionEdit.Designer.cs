namespace WinFormsAdmin.Forms.Commissions
{
    partial class FormCommissionEdit
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
            btnSave = new Button();
            btnCancel = new Button();
            gbAvaiableSubjects = new GroupBox();
            clbSubjects = new CheckedListBox();
            cmbProfessors = new ComboBox();
            lblProfessor = new Label();
            lblCycleYear = new Label();
            cmbDayOfWeek = new ComboBox();
            lblDayOfWeek = new Label();
            numStartHour = new NumericUpDown();
            lblStartHour = new Label();
            numStartMinute = new NumericUpDown();
            lblStartMinute = new Label();
            gbSchedule = new GroupBox();
            numEndMinute = new NumericUpDown();
            lblEndMinute = new Label();
            lblEndHour = new Label();
            numEndHour = new NumericUpDown();
            lblEndTime = new Label();
            lblStartTime = new Label();
            numCapacity = new NumericUpDown();
            numCycleYear = new NumericUpDown();
            lblCapacity = new Label();
            lblStatus = new Label();
            cmbStatus = new ComboBox();
            gbAvaiableSubjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numStartHour).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numStartMinute).BeginInit();
            gbSchedule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numEndMinute).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numEndHour).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCapacity).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCycleYear).BeginInit();
            SuspendLayout();
            // 
            // btnSave
            // 
            btnSave.Location = new Point(291, 373);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 0;
            btnSave.Text = "Guardar";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(397, 373);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancelar";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // gbAvaiableSubjects
            // 
            gbAvaiableSubjects.Controls.Add(clbSubjects);
            gbAvaiableSubjects.Location = new Point(82, 14);
            gbAvaiableSubjects.Name = "gbAvaiableSubjects";
            gbAvaiableSubjects.Size = new Size(261, 249);
            gbAvaiableSubjects.TabIndex = 2;
            gbAvaiableSubjects.TabStop = false;
            gbAvaiableSubjects.Text = "Materias disponibles";
            // 
            // clbSubjects
            // 
            clbSubjects.Dock = DockStyle.Fill;
            clbSubjects.FormattingEnabled = true;
            clbSubjects.Location = new Point(3, 19);
            clbSubjects.Name = "clbSubjects";
            clbSubjects.Size = new Size(255, 227);
            clbSubjects.TabIndex = 0;
            clbSubjects.ItemCheck += clbSubjects_ItemCheck;
            // 
            // cmbProfessors
            // 
            cmbProfessors.FormattingEnabled = true;
            cmbProfessors.Location = new Point(85, 300);
            cmbProfessors.Name = "cmbProfessors";
            cmbProfessors.Size = new Size(219, 23);
            cmbProfessors.TabIndex = 3;
            // 
            // lblProfessor
            // 
            lblProfessor.AutoSize = true;
            lblProfessor.Location = new Point(82, 282);
            lblProfessor.Name = "lblProfessor";
            lblProfessor.Size = new Size(54, 15);
            lblProfessor.TabIndex = 4;
            lblProfessor.Text = "Profesor:";
            // 
            // lblCycleYear
            // 
            lblCycleYear.AutoSize = true;
            lblCycleYear.Location = new Point(310, 282);
            lblCycleYear.Name = "lblCycleYear";
            lblCycleYear.Size = new Size(81, 15);
            lblCycleYear.TabIndex = 6;
            lblCycleYear.Text = "Ciclo del Año:";
            // 
            // cmbDayOfWeek
            // 
            cmbDayOfWeek.FormattingEnabled = true;
            cmbDayOfWeek.Location = new Point(13, 39);
            cmbDayOfWeek.Name = "cmbDayOfWeek";
            cmbDayOfWeek.Size = new Size(227, 23);
            cmbDayOfWeek.TabIndex = 7;
            // 
            // lblDayOfWeek
            // 
            lblDayOfWeek.AutoSize = true;
            lblDayOfWeek.Location = new Point(13, 21);
            lblDayOfWeek.Name = "lblDayOfWeek";
            lblDayOfWeek.Size = new Size(99, 15);
            lblDayOfWeek.TabIndex = 8;
            lblDayOfWeek.Text = "Día de la semana:";
            // 
            // numStartHour
            // 
            numStartHour.Location = new Point(13, 120);
            numStartHour.Name = "numStartHour";
            numStartHour.Size = new Size(100, 23);
            numStartHour.TabIndex = 9;
            // 
            // lblStartHour
            // 
            lblStartHour.AutoSize = true;
            lblStartHour.Location = new Point(10, 102);
            lblStartHour.Name = "lblStartHour";
            lblStartHour.Size = new Size(36, 15);
            lblStartHour.TabIndex = 10;
            lblStartHour.Text = "Hora:";
            // 
            // numStartMinute
            // 
            numStartMinute.Location = new Point(140, 120);
            numStartMinute.Name = "numStartMinute";
            numStartMinute.Size = new Size(100, 23);
            numStartMinute.TabIndex = 11;
            // 
            // lblStartMinute
            // 
            lblStartMinute.AutoSize = true;
            lblStartMinute.Location = new Point(138, 102);
            lblStartMinute.Name = "lblStartMinute";
            lblStartMinute.Size = new Size(49, 15);
            lblStartMinute.TabIndex = 12;
            lblStartMinute.Text = "Minuto:";
            // 
            // gbSchedule
            // 
            gbSchedule.Controls.Add(numEndMinute);
            gbSchedule.Controls.Add(lblDayOfWeek);
            gbSchedule.Controls.Add(lblEndMinute);
            gbSchedule.Controls.Add(cmbDayOfWeek);
            gbSchedule.Controls.Add(lblEndHour);
            gbSchedule.Controls.Add(numEndHour);
            gbSchedule.Controls.Add(lblEndTime);
            gbSchedule.Controls.Add(lblStartTime);
            gbSchedule.Controls.Add(numStartMinute);
            gbSchedule.Controls.Add(lblStartMinute);
            gbSchedule.Controls.Add(lblStartHour);
            gbSchedule.Controls.Add(numStartHour);
            gbSchedule.Location = new Point(367, 22);
            gbSchedule.Name = "gbSchedule";
            gbSchedule.Size = new Size(256, 241);
            gbSchedule.TabIndex = 13;
            gbSchedule.TabStop = false;
            gbSchedule.Text = "Horarios de cursada";
            // 
            // numEndMinute
            // 
            numEndMinute.Location = new Point(140, 198);
            numEndMinute.Name = "numEndMinute";
            numEndMinute.Size = new Size(100, 23);
            numEndMinute.TabIndex = 18;
            // 
            // lblEndMinute
            // 
            lblEndMinute.AutoSize = true;
            lblEndMinute.Location = new Point(140, 180);
            lblEndMinute.Name = "lblEndMinute";
            lblEndMinute.Size = new Size(49, 15);
            lblEndMinute.TabIndex = 17;
            lblEndMinute.Text = "Minuto:";
            // 
            // lblEndHour
            // 
            lblEndHour.AutoSize = true;
            lblEndHour.Location = new Point(13, 180);
            lblEndHour.Name = "lblEndHour";
            lblEndHour.Size = new Size(36, 15);
            lblEndHour.TabIndex = 16;
            lblEndHour.Text = "Hora:";
            // 
            // numEndHour
            // 
            numEndHour.Location = new Point(13, 198);
            numEndHour.Name = "numEndHour";
            numEndHour.Size = new Size(100, 23);
            numEndHour.TabIndex = 15;
            // 
            // lblEndTime
            // 
            lblEndTime.AutoSize = true;
            lblEndTime.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEndTime.Location = new Point(13, 156);
            lblEndTime.Name = "lblEndTime";
            lblEndTime.Size = new Size(32, 15);
            lblEndTime.TabIndex = 14;
            lblEndTime.Text = "Final";
            // 
            // lblStartTime
            // 
            lblStartTime.AutoSize = true;
            lblStartTime.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStartTime.Location = new Point(11, 75);
            lblStartTime.Name = "lblStartTime";
            lblStartTime.Size = new Size(37, 15);
            lblStartTime.TabIndex = 13;
            lblStartTime.Text = "Inicio";
            // 
            // numCapacity
            // 
            numCapacity.Location = new Point(397, 300);
            numCapacity.Name = "numCapacity";
            numCapacity.Size = new Size(67, 23);
            numCapacity.TabIndex = 14;
            // 
            // numCycleYear
            // 
            numCycleYear.Location = new Point(310, 300);
            numCycleYear.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            numCycleYear.Name = "numCycleYear";
            numCycleYear.Size = new Size(81, 23);
            numCycleYear.TabIndex = 15;
            // 
            // lblCapacity
            // 
            lblCapacity.AutoSize = true;
            lblCapacity.Location = new Point(397, 282);
            lblCapacity.Name = "lblCapacity";
            lblCapacity.Size = new Size(44, 15);
            lblCapacity.TabIndex = 16;
            lblCapacity.Text = "Cupos:";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(470, 281);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(45, 15);
            lblStatus.TabIndex = 18;
            lblStatus.Text = "Estado:";
            // 
            // cmbStatus
            // 
            cmbStatus.FormattingEnabled = true;
            cmbStatus.Location = new Point(470, 299);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Size = new Size(153, 23);
            cmbStatus.TabIndex = 19;
            // 
            // FormCommissionEdit
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(cmbStatus);
            Controls.Add(lblStatus);
            Controls.Add(lblCapacity);
            Controls.Add(numCycleYear);
            Controls.Add(numCapacity);
            Controls.Add(gbSchedule);
            Controls.Add(lblCycleYear);
            Controls.Add(lblProfessor);
            Controls.Add(cmbProfessors);
            Controls.Add(gbAvaiableSubjects);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Name = "FormCommissionEdit";
            Text = "FormCommissionEdit";
            gbAvaiableSubjects.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numStartHour).EndInit();
            ((System.ComponentModel.ISupportInitialize)numStartMinute).EndInit();
            gbSchedule.ResumeLayout(false);
            gbSchedule.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numEndMinute).EndInit();
            ((System.ComponentModel.ISupportInitialize)numEndHour).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCapacity).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCycleYear).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSave;
        private Button btnCancel;
        private GroupBox gbAvaiableSubjects;
        private CheckedListBox clbSubjects;
        private ComboBox cmbProfessors;
        private Label lblProfessor;
        private Label lblCycleYear;
        private ComboBox cmbDayOfWeek;
        private Label lblDayOfWeek;
        private NumericUpDown numStartHour;
        private Label lblStartHour;
        private NumericUpDown numStartMinute;
        private Label lblStartMinute;
        private GroupBox gbSchedule;
        private Label lblStartTime;
        private Label lblEndMinute;
        private Label lblEndHour;
        private NumericUpDown numEndHour;
        private Label lblEndTime;
        private NumericUpDown numEndMinute;
        private NumericUpDown numCapacity;
        private NumericUpDown numCycleYear;
        private Label lblCapacity;
        private Label lblStatus;
        private ComboBox cmbStatus;
    }
}