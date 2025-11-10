namespace WinFormsAdmin.Forms.Enrollments
{
    partial class FormEnrollManager
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
            panel1 = new Panel();
            dtpUnenrollmentTo = new DateTimePicker();
            dtpEnrollmentFrom = new DateTimePicker();
            lblSearchCycleYear = new Label();
            txtCycleYearFilter = new TextBox();
            lblSearchSubject = new Label();
            txtSubjectFilter = new TextBox();
            lblSearch = new Label();
            lblStatus = new Label();
            btnReload = new Button();
            cmbStatus = new ComboBox();
            txtSearch = new TextBox();
            dgvEnrollments = new DataGridView();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEnrollments).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(dtpUnenrollmentTo);
            panel1.Controls.Add(dtpEnrollmentFrom);
            panel1.Controls.Add(lblSearchCycleYear);
            panel1.Controls.Add(txtCycleYearFilter);
            panel1.Controls.Add(lblSearchSubject);
            panel1.Controls.Add(txtSubjectFilter);
            panel1.Controls.Add(lblSearch);
            panel1.Controls.Add(lblStatus);
            panel1.Controls.Add(btnReload);
            panel1.Controls.Add(cmbStatus);
            panel1.Controls.Add(txtSearch);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 77);
            panel1.TabIndex = 0;
            // 
            // dtpUnenrollmentTo
            // 
            dtpUnenrollmentTo.Location = new Point(644, 34);
            dtpUnenrollmentTo.Name = "dtpUnenrollmentTo";
            dtpUnenrollmentTo.ShowCheckBox = true;
            dtpUnenrollmentTo.Size = new Size(153, 23);
            dtpUnenrollmentTo.TabIndex = 11;
            dtpUnenrollmentTo.ValueChanged += txtSearch_TextChanged;
            // 
            // dtpEnrollmentFrom
            // 
            dtpEnrollmentFrom.Location = new Point(644, 5);
            dtpEnrollmentFrom.Name = "dtpEnrollmentFrom";
            dtpEnrollmentFrom.ShowCheckBox = true;
            dtpEnrollmentFrom.Size = new Size(153, 23);
            dtpEnrollmentFrom.TabIndex = 10;
            dtpEnrollmentFrom.ValueChanged += txtSearch_TextChanged;
            // 
            // lblSearchCycleYear
            // 
            lblSearchCycleYear.AutoSize = true;
            lblSearchCycleYear.Location = new Point(325, 31);
            lblSearchCycleYear.Name = "lblSearchCycleYear";
            lblSearchCycleYear.Size = new Size(76, 15);
            lblSearchCycleYear.TabIndex = 9;
            lblSearchCycleYear.Text = "Ciclo del año";
            // 
            // txtCycleYearFilter
            // 
            txtCycleYearFilter.Location = new Point(312, 5);
            txtCycleYearFilter.Name = "txtCycleYearFilter";
            txtCycleYearFilter.Size = new Size(100, 23);
            txtCycleYearFilter.TabIndex = 8;
            // 
            // lblSearchSubject
            // 
            lblSearchSubject.AutoSize = true;
            lblSearchSubject.Location = new Point(231, 31);
            lblSearchSubject.Name = "lblSearchSubject";
            lblSearchSubject.Size = new Size(47, 15);
            lblSearchSubject.TabIndex = 7;
            lblSearchSubject.Text = "Materia";
            // 
            // txtSubjectFilter
            // 
            txtSubjectFilter.Location = new Point(206, 5);
            txtSubjectFilter.Name = "txtSubjectFilter";
            txtSubjectFilter.Size = new Size(100, 23);
            txtSubjectFilter.TabIndex = 6;
            txtSubjectFilter.TextChanged += txtSearch_TextChanged;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(439, 32);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(101, 15);
            lblSearch.TabIndex = 5;
            lblSearch.Text = "Estudiate / Legajo";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(124, 31);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(42, 15);
            lblStatus.TabIndex = 4;
            lblStatus.Text = "Estado";
            // 
            // btnReload
            // 
            btnReload.Location = new Point(12, 5);
            btnReload.Name = "btnReload";
            btnReload.Size = new Size(75, 23);
            btnReload.TabIndex = 3;
            btnReload.Text = "Refrescar";
            btnReload.UseVisualStyleBackColor = true;
            btnReload.Click += btnReload_Click;
            // 
            // cmbStatus
            // 
            cmbStatus.FormattingEnabled = true;
            cmbStatus.Location = new Point(93, 5);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Size = new Size(107, 23);
            cmbStatus.TabIndex = 2;
            cmbStatus.TextChanged += cmbStatus_SelectedIndexChanged;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(418, 5);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(142, 23);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // dgvEnrollments
            // 
            dgvEnrollments.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEnrollments.Dock = DockStyle.Fill;
            dgvEnrollments.Location = new Point(0, 77);
            dgvEnrollments.Name = "dgvEnrollments";
            dgvEnrollments.Size = new Size(800, 373);
            dgvEnrollments.TabIndex = 1;
            dgvEnrollments.CellEndEdit += dgvEnrollments_CellEndEdit;
            // 
            // FormEnrollManager
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dgvEnrollments);
            Controls.Add(panel1);
            Name = "FormEnrollManager";
            Text = "FormEnrollManager";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEnrollments).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private TextBox txtSearch;
        private ComboBox cmbStatus;
        private DataGridView dgvEnrollments;
        private Button btnReload;
        private Label lblStatus;
        private TextBox txtCycleYearFilter;
        private Label lblSearchSubject;
        private TextBox txtSubjectFilter;
        private Label lblSearch;
        private DateTimePicker dtpEnrollmentFrom;
        private Label lblSearchCycleYear;
        private DateTimePicker dtpUnenrollmentTo;
    }
}