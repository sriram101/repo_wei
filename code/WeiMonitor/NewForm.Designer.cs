namespace Telavance.AdvantageSuite.Wei.ServiceManager
{
    partial class NewForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnForceShutdown = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnGracefulShutdown = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.frmInterfaceGrid = new System.Windows.Forms.DataGridView();
            this.lblMessage = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.StatusStrip();
            this.InterfaceID1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InterfaceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ShowStats = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ShowErrors = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frmInterfaceGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnForceShutdown);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.btnGracefulShutdown);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(727, 59);
            this.panel1.TabIndex = 0;
            // 
            // btnForceShutdown
            // 
            this.btnForceShutdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnForceShutdown.Location = new System.Drawing.Point(338, 10);
            this.btnForceShutdown.Name = "btnForceShutdown";
            this.btnForceShutdown.Size = new System.Drawing.Size(129, 23);
            this.btnForceShutdown.TabIndex = 16;
            this.btnForceShutdown.Text = "Force Stop Service";
            this.btnForceShutdown.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(514, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 15;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click_1);
            // 
            // btnGracefulShutdown
            // 
            this.btnGracefulShutdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGracefulShutdown.Location = new System.Drawing.Point(172, 10);
            this.btnGracefulShutdown.Name = "btnGracefulShutdown";
            this.btnGracefulShutdown.Size = new System.Drawing.Size(125, 23);
            this.btnGracefulShutdown.TabIndex = 17;
            this.btnGracefulShutdown.Text = "Stop Service";
            this.btnGracefulShutdown.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(21, 10);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(107, 23);
            this.btnStart.TabIndex = 18;
            this.btnStart.Text = "Start Service ";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(12, 139);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(727, 271);
            this.panel2.TabIndex = 1;
            // 
            // frmInterfaceGrid
            // 
            this.frmInterfaceGrid.AllowUserToAddRows = false;
            this.frmInterfaceGrid.AllowUserToDeleteRows = false;
            this.frmInterfaceGrid.AllowUserToOrderColumns = true;
            this.frmInterfaceGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedHeaders;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.frmInterfaceGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.frmInterfaceGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.frmInterfaceGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.InterfaceID1,
            this.InterfaceName,
            this.Status,
            this.ShowStats,
            this.ShowErrors});
            this.frmInterfaceGrid.GridColor = System.Drawing.Color.Blue;
            this.frmInterfaceGrid.Location = new System.Drawing.Point(12, 86);
            this.frmInterfaceGrid.Name = "frmInterfaceGrid";
            this.frmInterfaceGrid.ReadOnly = true;
            this.frmInterfaceGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.frmInterfaceGrid.Size = new System.Drawing.Size(727, 271);
            this.frmInterfaceGrid.TabIndex = 0;
            this.frmInterfaceGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.frmInterfaceGrid_CellContentClick_1);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(12, 112);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(0, 13);
            this.lblMessage.TabIndex = 2;
            // 
            // StatusLabel
            // 
            this.StatusLabel.Location = new System.Drawing.Point(0, 450);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(816, 22);
            this.StatusLabel.TabIndex = 3;
            this.StatusLabel.Text = "statusStrip1";
            // 
            // InterfaceID1
            // 
            this.InterfaceID1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.InterfaceID1.HeaderText = "Interface ID";
            this.InterfaceID1.Name = "InterfaceID1";
            this.InterfaceID1.ReadOnly = true;
            // 
            // InterfaceName
            // 
            this.InterfaceName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.InterfaceName.HeaderText = "Interface Name";
            this.InterfaceName.Name = "InterfaceName";
            this.InterfaceName.ReadOnly = true;
            this.InterfaceName.Width = 109;
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // ShowStats
            // 
            this.ShowStats.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowStats.DefaultCellStyle = dataGridViewCellStyle2;
            this.ShowStats.HeaderText = "Show Message Stats";
            this.ShowStats.Name = "ShowStats";
            this.ShowStats.ReadOnly = true;
            this.ShowStats.Text = "Show Stats";
            this.ShowStats.UseColumnTextForButtonValue = true;
            this.ShowStats.Width = 118;
            // 
            // ShowErrors
            // 
            this.ShowErrors.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowErrors.DefaultCellStyle = dataGridViewCellStyle3;
            this.ShowErrors.HeaderText = "Show Error Messages";
            this.ShowErrors.Name = "ShowErrors";
            this.ShowErrors.ReadOnly = true;
            this.ShowErrors.Text = "Shows Errors";
            this.ShowErrors.UseColumnTextForButtonValue = true;
            this.ShowErrors.Width = 122;
            // 
            // NewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 472);
            this.Controls.Add(this.frmInterfaceGrid);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "NewForm";
            this.Text = "WEI Service Manager";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.frmInterfaceGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnForceShutdown;
        private System.Windows.Forms.Button btnGracefulShutdown;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.DataGridView frmInterfaceGrid;
        private System.Windows.Forms.StatusStrip StatusLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn InterfaceID1;
        private System.Windows.Forms.DataGridViewTextBoxColumn InterfaceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewButtonColumn ShowStats;
        private System.Windows.Forms.DataGridViewButtonColumn ShowErrors;
    }
}