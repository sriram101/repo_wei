namespace Telavance.AdvantageSuite.Wei.ServiceManager
{
    partial class ShowRequestsWithErrors
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.frmErrorMessages = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.frmAuditMessages = new System.Windows.Forms.DataGridView();
            this.lblAuditMessages = new System.Windows.Forms.Label();
            this.RequestID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InterfaceID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HasCTC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsError = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OFACStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AuditLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Messages = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.frmErrorMessages)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frmAuditMessages)).BeginInit();
            this.SuspendLayout();
            // 
            // frmErrorMessages
            // 
            this.frmErrorMessages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.frmErrorMessages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RequestID,
            this.InterfaceID,
            this.HasCTC,
            this.IsError,
            this.OFACStatus,
            this.Status});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.Format = "G";
            dataGridViewCellStyle6.NullValue = null;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.frmErrorMessages.DefaultCellStyle = dataGridViewCellStyle6;
            this.frmErrorMessages.Location = new System.Drawing.Point(52, 65);
            this.frmErrorMessages.Name = "frmErrorMessages";
            this.frmErrorMessages.ReadOnly = true;
            this.frmErrorMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.frmErrorMessages.Size = new System.Drawing.Size(719, 240);
            this.frmErrorMessages.TabIndex = 0;
            this.frmErrorMessages.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.frmErrorMessages_CellContentClick);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnLast);
            this.panel1.Controls.Add(this.btnPrevious);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnFirst);
            this.panel1.Controls.Add(this.btnProcess);
            this.panel1.Location = new System.Drawing.Point(52, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(599, 47);
            this.panel1.TabIndex = 1;
            // 
            // btnLast
            // 
            this.btnLast.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLast.Location = new System.Drawing.Point(305, 12);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(75, 23);
            this.btnLast.TabIndex = 5;
            this.btnLast.Text = "Last";
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Visible = false;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrevious.Location = new System.Drawing.Point(208, 12);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(75, 23);
            this.btnPrevious.TabIndex = 4;
            this.btnPrevious.Text = "Previous";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Visible = false;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnNext
            // 
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.Location = new System.Drawing.Point(111, 12);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Visible = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnFirst
            // 
            this.btnFirst.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFirst.Location = new System.Drawing.Point(14, 12);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(75, 23);
            this.btnFirst.TabIndex = 1;
            this.btnFirst.Text = "First";
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Visible = false;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcess.Location = new System.Drawing.Point(402, 12);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(193, 23);
            this.btnProcess.TabIndex = 0;
            this.btnProcess.Text = "Process All Error Messages";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // frmAuditMessages
            // 
            this.frmAuditMessages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.frmAuditMessages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AuditLevel,
            this.Messages,
            this.Status1});
            this.frmAuditMessages.Location = new System.Drawing.Point(52, 368);
            this.frmAuditMessages.Name = "frmAuditMessages";
            this.frmAuditMessages.Size = new System.Drawing.Size(599, 90);
            this.frmAuditMessages.TabIndex = 2;
            // 
            // lblAuditMessages
            // 
            this.lblAuditMessages.AutoSize = true;
            this.lblAuditMessages.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuditMessages.Location = new System.Drawing.Point(49, 341);
            this.lblAuditMessages.Name = "lblAuditMessages";
            this.lblAuditMessages.Size = new System.Drawing.Size(157, 13);
            this.lblAuditMessages.TabIndex = 3;
            this.lblAuditMessages.Text = "Audit Details for Request: ";
            // 
            // RequestID
            // 
            this.RequestID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.RequestID.DataPropertyName = "RequestID";
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RequestID.DefaultCellStyle = dataGridViewCellStyle1;
            this.RequestID.HeaderText = "Request ID";
            this.RequestID.Name = "RequestID";
            this.RequestID.ReadOnly = true;
            this.RequestID.Width = 86;
            // 
            // InterfaceID
            // 
            this.InterfaceID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.InterfaceID.DataPropertyName = "InterfaceID";
            this.InterfaceID.HeaderText = "Interface ID";
            this.InterfaceID.Name = "InterfaceID";
            this.InterfaceID.ReadOnly = true;
            this.InterfaceID.Width = 88;
            // 
            // HasCTC
            // 
            this.HasCTC.DataPropertyName = "HasCTC";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HasCTC.DefaultCellStyle = dataGridViewCellStyle2;
            this.HasCTC.HeaderText = "Has CTC";
            this.HasCTC.Name = "HasCTC";
            this.HasCTC.ReadOnly = true;
            // 
            // IsError
            // 
            this.IsError.DataPropertyName = "IsError";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsError.DefaultCellStyle = dataGridViewCellStyle3;
            this.IsError.HeaderText = "Is Error";
            this.IsError.Name = "IsError";
            this.IsError.ReadOnly = true;
            this.IsError.Visible = false;
            // 
            // OFACStatus
            // 
            this.OFACStatus.DataPropertyName = "OFACStatus";
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OFACStatus.DefaultCellStyle = dataGridViewCellStyle4;
            this.OFACStatus.HeaderText = "OFAC Status";
            this.OFACStatus.Name = "OFACStatus";
            this.OFACStatus.ReadOnly = true;
            // 
            // Status
            // 
            this.Status.DataPropertyName = "Status";
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Status.DefaultCellStyle = dataGridViewCellStyle5;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // AuditLevel
            // 
            this.AuditLevel.DataPropertyName = "AuditLevel";
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AuditLevel.DefaultCellStyle = dataGridViewCellStyle7;
            this.AuditLevel.HeaderText = "Audit Level";
            this.AuditLevel.Name = "AuditLevel";
            // 
            // Messages
            // 
            this.Messages.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Messages.DataPropertyName = "Message";
            this.Messages.HeaderText = "Audit Message";
            this.Messages.Name = "Messages";
            this.Messages.Width = 102;
            // 
            // Status1
            // 
            this.Status1.DataPropertyName = "Status";
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Status1.DefaultCellStyle = dataGridViewCellStyle8;
            this.Status1.HeaderText = "Status";
            this.Status1.Name = "Status1";
            // 
            // ShowRequestsWithErrors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 541);
            this.Controls.Add(this.frmErrorMessages);
            this.Controls.Add(this.lblAuditMessages);
            this.Controls.Add(this.frmAuditMessages);
            this.Controls.Add(this.panel1);
            this.Name = "ShowRequestsWithErrors";
            this.Text = "Show Error Messages";
            ((System.ComponentModel.ISupportInitialize)(this.frmErrorMessages)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.frmAuditMessages)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView frmErrorMessages;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView frmAuditMessages;
        private System.Windows.Forms.Label lblAuditMessages;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequestID;
        private System.Windows.Forms.DataGridViewTextBoxColumn InterfaceID;
        private System.Windows.Forms.DataGridViewTextBoxColumn HasCTC;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsError;
        private System.Windows.Forms.DataGridViewTextBoxColumn OFACStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn AuditLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Messages;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status1;
    }
}