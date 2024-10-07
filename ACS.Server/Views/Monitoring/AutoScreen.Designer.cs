namespace INA_ACS_Server.OPWindows
{
    partial class AutoScreen
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            this.AutoDisplay_Timer = new System.Windows.Forms.Timer(this.components);
            this.dtgAuto_Scheduler_Status = new System.Windows.Forms.DataGridView();
            this.DGV_Schedule_Status_MiR_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGV_Schedule_Status_CallName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGV_Schedule_Status_MissionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGV_Schedule_Status_MissionState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGV_Schedule_Status_MissionReturnID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_Cancel_All_Job = new System.Windows.Forms.Button();
            this.lbl_Robot = new DevExpress.XtraEditors.LabelControl();
            this.P_Robot = new DevExpress.XtraEditors.PanelControl();
            this.dtgAuto_MiR_Status = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.p_possible = new DevExpress.XtraEditors.PanelControl();
            this.lbl_possible = new DevExpress.XtraEditors.LabelControl();
            this.dtgAuto_Job_Status = new System.Windows.Forms.DataGridView();
            this.DGV_Job_Status_LINECD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGV_Job_Status_POSTCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGV_Job_Status_ReturnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGV_Job_Status_MissionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGV_Job_Status_MissionState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGV_Job_Status_CreateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGV_Job_Status_JobCancelBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.p_mission = new DevExpress.XtraEditors.PanelControl();
            this.lbl_mission = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.dtgAuto_Scheduler_Status)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P_Robot)).BeginInit();
            this.P_Robot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgAuto_MiR_Status)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.p_possible)).BeginInit();
            this.p_possible.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgAuto_Job_Status)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.p_mission)).BeginInit();
            this.p_mission.SuspendLayout();
            this.SuspendLayout();
            // 
            // AutoDisplay_Timer
            // 
            this.AutoDisplay_Timer.Enabled = true;
            this.AutoDisplay_Timer.Interval = 500;
            this.AutoDisplay_Timer.Tick += new System.EventHandler(this.AutoDisplay_Timer_Tick);
            // 
            // dtgAuto_Scheduler_Status
            // 
            this.dtgAuto_Scheduler_Status.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.dtgAuto_Scheduler_Status.AllowUserToAddRows = false;
            this.dtgAuto_Scheduler_Status.AllowUserToDeleteRows = false;
            this.dtgAuto_Scheduler_Status.AllowUserToResizeColumns = false;
            this.dtgAuto_Scheduler_Status.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtgAuto_Scheduler_Status.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dtgAuto_Scheduler_Status.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dtgAuto_Scheduler_Status.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(89)))), ((int)(((byte)(96)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtgAuto_Scheduler_Status.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dtgAuto_Scheduler_Status.ColumnHeadersHeight = 50;
            this.dtgAuto_Scheduler_Status.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dtgAuto_Scheduler_Status.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DGV_Schedule_Status_MiR_Name,
            this.DGV_Schedule_Status_CallName,
            this.DGV_Schedule_Status_MissionName,
            this.DGV_Schedule_Status_MissionState,
            this.DGV_Schedule_Status_MissionReturnID});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(52)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Tahoma", 9F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtgAuto_Scheduler_Status.DefaultCellStyle = dataGridViewCellStyle8;
            this.dtgAuto_Scheduler_Status.EnableHeadersVisualStyles = false;
            this.dtgAuto_Scheduler_Status.GridColor = System.Drawing.Color.LightGray;
            this.dtgAuto_Scheduler_Status.Location = new System.Drawing.Point(5, 48);
            this.dtgAuto_Scheduler_Status.MultiSelect = false;
            this.dtgAuto_Scheduler_Status.Name = "dtgAuto_Scheduler_Status";
            this.dtgAuto_Scheduler_Status.ReadOnly = true;
            this.dtgAuto_Scheduler_Status.RightToLeft = System.Windows.Forms.RightToLeft.No;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Tahoma", 9F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.NullValue = "0";
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgAuto_Scheduler_Status.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dtgAuto_Scheduler_Status.RowHeadersVisible = false;
            this.dtgAuto_Scheduler_Status.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(52)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgAuto_Scheduler_Status.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dtgAuto_Scheduler_Status.RowTemplate.DefaultCellStyle.NullValue = "null";
            this.dtgAuto_Scheduler_Status.RowTemplate.Height = 23;
            this.dtgAuto_Scheduler_Status.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dtgAuto_Scheduler_Status.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dtgAuto_Scheduler_Status.Size = new System.Drawing.Size(837, 399);
            this.dtgAuto_Scheduler_Status.TabIndex = 275;
            // 
            // DGV_Schedule_Status_MiR_Name
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.DGV_Schedule_Status_MiR_Name.DefaultCellStyle = dataGridViewCellStyle3;
            this.DGV_Schedule_Status_MiR_Name.HeaderText = "Robot Name";
            this.DGV_Schedule_Status_MiR_Name.Name = "DGV_Schedule_Status_MiR_Name";
            this.DGV_Schedule_Status_MiR_Name.ReadOnly = true;
            this.DGV_Schedule_Status_MiR_Name.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_Schedule_Status_MiR_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DGV_Schedule_Status_MiR_Name.Width = 150;
            // 
            // DGV_Schedule_Status_CallName
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.DGV_Schedule_Status_CallName.DefaultCellStyle = dataGridViewCellStyle4;
            this.DGV_Schedule_Status_CallName.HeaderText = "CallName";
            this.DGV_Schedule_Status_CallName.Name = "DGV_Schedule_Status_CallName";
            this.DGV_Schedule_Status_CallName.ReadOnly = true;
            this.DGV_Schedule_Status_CallName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_Schedule_Status_CallName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DGV_Schedule_Status_CallName.Width = 210;
            // 
            // DGV_Schedule_Status_MissionName
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.DGV_Schedule_Status_MissionName.DefaultCellStyle = dataGridViewCellStyle5;
            this.DGV_Schedule_Status_MissionName.HeaderText = "Mission Name";
            this.DGV_Schedule_Status_MissionName.Name = "DGV_Schedule_Status_MissionName";
            this.DGV_Schedule_Status_MissionName.ReadOnly = true;
            this.DGV_Schedule_Status_MissionName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_Schedule_Status_MissionName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DGV_Schedule_Status_MissionName.Width = 260;
            // 
            // DGV_Schedule_Status_MissionState
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.DGV_Schedule_Status_MissionState.DefaultCellStyle = dataGridViewCellStyle6;
            this.DGV_Schedule_Status_MissionState.HeaderText = "Mission State";
            this.DGV_Schedule_Status_MissionState.Name = "DGV_Schedule_Status_MissionState";
            this.DGV_Schedule_Status_MissionState.ReadOnly = true;
            this.DGV_Schedule_Status_MissionState.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_Schedule_Status_MissionState.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DGV_Schedule_Status_MissionState.Width = 200;
            // 
            // DGV_Schedule_Status_MissionReturnID
            // 
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.DGV_Schedule_Status_MissionReturnID.DefaultCellStyle = dataGridViewCellStyle7;
            this.DGV_Schedule_Status_MissionReturnID.HeaderText = "Return ID";
            this.DGV_Schedule_Status_MissionReturnID.Name = "DGV_Schedule_Status_MissionReturnID";
            this.DGV_Schedule_Status_MissionReturnID.ReadOnly = true;
            this.DGV_Schedule_Status_MissionReturnID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_Schedule_Status_MissionReturnID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DGV_Schedule_Status_MissionReturnID.Visible = false;
            // 
            // btn_Cancel_All_Job
            // 
            this.btn_Cancel_All_Job.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btn_Cancel_All_Job.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Cancel_All_Job.Location = new System.Drawing.Point(1547, 12);
            this.btn_Cancel_All_Job.Name = "btn_Cancel_All_Job";
            this.btn_Cancel_All_Job.Size = new System.Drawing.Size(115, 31);
            this.btn_Cancel_All_Job.TabIndex = 278;
            this.btn_Cancel_All_Job.Text = "Cancel All";
            this.btn_Cancel_All_Job.UseVisualStyleBackColor = false;
            this.btn_Cancel_All_Job.Visible = false;
            this.btn_Cancel_All_Job.Click += new System.EventHandler(this.btn_Cancel_All_Job_Click);
            // 
            // lbl_Robot
            // 
            this.lbl_Robot.Location = new System.Drawing.Point(5, 5);
            this.lbl_Robot.Name = "lbl_Robot";
            this.lbl_Robot.Size = new System.Drawing.Size(33, 14);
            this.lbl_Robot.TabIndex = 279;
            this.lbl_Robot.Text = "Robot";
            // 
            // P_Robot
            // 
            this.P_Robot.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.P_Robot.Controls.Add(this.dtgAuto_MiR_Status);
            this.P_Robot.Controls.Add(this.lbl_Robot);
            this.P_Robot.Dock = System.Windows.Forms.DockStyle.Left;
            this.P_Robot.Location = new System.Drawing.Point(0, 0);
            this.P_Robot.Name = "P_Robot";
            this.P_Robot.Size = new System.Drawing.Size(863, 956);
            this.P_Robot.TabIndex = 279;
            // 
            // dtgAuto_MiR_Status
            // 
            this.dtgAuto_MiR_Status.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtgAuto_MiR_Status.Location = new System.Drawing.Point(3, 39);
            this.dtgAuto_MiR_Status.MainView = this.gridView1;
            this.dtgAuto_MiR_Status.Name = "dtgAuto_MiR_Status";
            this.dtgAuto_MiR_Status.Size = new System.Drawing.Size(854, 917);
            this.dtgAuto_MiR_Status.TabIndex = 280;
            this.dtgAuto_MiR_Status.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.dtgAuto_MiR_Status;
            this.gridView1.Name = "gridView1";
            this.gridView1.CustomDrawColumnHeader += new DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventHandler(this.gridView1_CustomDrawColumnHeader);
            this.gridView1.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gridView1_CustomDrawCell);
            this.gridView1.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gridView1_RowCellStyle);
            // 
            // p_possible
            // 
            this.p_possible.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.p_possible.Controls.Add(this.lbl_possible);
            this.p_possible.Controls.Add(this.dtgAuto_Job_Status);
            this.p_possible.Dock = System.Windows.Forms.DockStyle.Fill;
            this.p_possible.Location = new System.Drawing.Point(863, 0);
            this.p_possible.Name = "p_possible";
            this.p_possible.Size = new System.Drawing.Size(817, 956);
            this.p_possible.TabIndex = 280;
            // 
            // lbl_possible
            // 
            this.lbl_possible.Location = new System.Drawing.Point(3, 3);
            this.lbl_possible.Name = "lbl_possible";
            this.lbl_possible.Size = new System.Drawing.Size(104, 14);
            this.lbl_possible.TabIndex = 280;
            this.lbl_possible.Text = "As soon as possible";
            // 
            // dtgAuto_Job_Status
            // 
            this.dtgAuto_Job_Status.AllowUserToAddRows = false;
            this.dtgAuto_Job_Status.AllowUserToDeleteRows = false;
            this.dtgAuto_Job_Status.AllowUserToResizeColumns = false;
            this.dtgAuto_Job_Status.AllowUserToResizeRows = false;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Arial", 11.25F);
            this.dtgAuto_Job_Status.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle11;
            this.dtgAuto_Job_Status.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dtgAuto_Job_Status.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(89)))), ((int)(((byte)(96)))));
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Tahoma", 9F);
            dataGridViewCellStyle12.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtgAuto_Job_Status.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.dtgAuto_Job_Status.ColumnHeadersHeight = 50;
            this.dtgAuto_Job_Status.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dtgAuto_Job_Status.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DGV_Job_Status_LINECD,
            this.DGV_Job_Status_POSTCD,
            this.DGV_Job_Status_ReturnType,
            this.DGV_Job_Status_MissionName,
            this.DGV_Job_Status_MissionState,
            this.DGV_Job_Status_CreateTime,
            this.DGV_Job_Status_JobCancelBtn});
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(52)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Tahoma", 9F);
            dataGridViewCellStyle20.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtgAuto_Job_Status.DefaultCellStyle = dataGridViewCellStyle20;
            this.dtgAuto_Job_Status.EnableHeadersVisualStyles = false;
            this.dtgAuto_Job_Status.GridColor = System.Drawing.Color.LightGray;
            this.dtgAuto_Job_Status.Location = new System.Drawing.Point(6, 39);
            this.dtgAuto_Job_Status.MultiSelect = false;
            this.dtgAuto_Job_Status.Name = "dtgAuto_Job_Status";
            this.dtgAuto_Job_Status.ReadOnly = true;
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle21.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle21.Font = new System.Drawing.Font("Tahoma", 9F);
            dataGridViewCellStyle21.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle21.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle21.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgAuto_Job_Status.RowHeadersDefaultCellStyle = dataGridViewCellStyle21;
            this.dtgAuto_Job_Status.RowHeadersVisible = false;
            dataGridViewCellStyle22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(52)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle22.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgAuto_Job_Status.RowsDefaultCellStyle = dataGridViewCellStyle22;
            this.dtgAuto_Job_Status.RowTemplate.Height = 23;
            this.dtgAuto_Job_Status.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dtgAuto_Job_Status.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dtgAuto_Job_Status.Size = new System.Drawing.Size(790, 419);
            this.dtgAuto_Job_Status.TabIndex = 277;
            this.dtgAuto_Job_Status.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dtgAuto_Job_Status_CellMouseClick);
            // 
            // DGV_Job_Status_LINECD
            // 
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle13.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.DGV_Job_Status_LINECD.DefaultCellStyle = dataGridViewCellStyle13;
            this.DGV_Job_Status_LINECD.HeaderText = "LINE CD";
            this.DGV_Job_Status_LINECD.Name = "DGV_Job_Status_LINECD";
            this.DGV_Job_Status_LINECD.ReadOnly = true;
            this.DGV_Job_Status_LINECD.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_Job_Status_LINECD.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DGV_Job_Status_LINECD.Width = 125;
            // 
            // DGV_Job_Status_POSTCD
            // 
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle14.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.DGV_Job_Status_POSTCD.DefaultCellStyle = dataGridViewCellStyle14;
            this.DGV_Job_Status_POSTCD.HeaderText = "POST CD";
            this.DGV_Job_Status_POSTCD.Name = "DGV_Job_Status_POSTCD";
            this.DGV_Job_Status_POSTCD.ReadOnly = true;
            this.DGV_Job_Status_POSTCD.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_Job_Status_POSTCD.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DGV_Job_Status_POSTCD.Width = 125;
            // 
            // DGV_Job_Status_ReturnType
            // 
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle15.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.DGV_Job_Status_ReturnType.DefaultCellStyle = dataGridViewCellStyle15;
            this.DGV_Job_Status_ReturnType.HeaderText = "Type";
            this.DGV_Job_Status_ReturnType.Name = "DGV_Job_Status_ReturnType";
            this.DGV_Job_Status_ReturnType.ReadOnly = true;
            this.DGV_Job_Status_ReturnType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_Job_Status_ReturnType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DGV_Job_Status_ReturnType.Width = 55;
            // 
            // DGV_Job_Status_MissionName
            // 
            dataGridViewCellStyle16.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle16.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.DGV_Job_Status_MissionName.DefaultCellStyle = dataGridViewCellStyle16;
            this.DGV_Job_Status_MissionName.HeaderText = "Mission";
            this.DGV_Job_Status_MissionName.Name = "DGV_Job_Status_MissionName";
            this.DGV_Job_Status_MissionName.ReadOnly = true;
            this.DGV_Job_Status_MissionName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_Job_Status_MissionName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DGV_Job_Status_MissionName.Width = 180;
            // 
            // DGV_Job_Status_MissionState
            // 
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle17.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.DGV_Job_Status_MissionState.DefaultCellStyle = dataGridViewCellStyle17;
            this.DGV_Job_Status_MissionState.HeaderText = "State";
            this.DGV_Job_Status_MissionState.Name = "DGV_Job_Status_MissionState";
            this.DGV_Job_Status_MissionState.ReadOnly = true;
            this.DGV_Job_Status_MissionState.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_Job_Status_MissionState.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DGV_Job_Status_MissionState.Width = 120;
            // 
            // DGV_Job_Status_CreateTime
            // 
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.DGV_Job_Status_CreateTime.DefaultCellStyle = dataGridViewCellStyle18;
            this.DGV_Job_Status_CreateTime.HeaderText = "Create Time";
            this.DGV_Job_Status_CreateTime.Name = "DGV_Job_Status_CreateTime";
            this.DGV_Job_Status_CreateTime.ReadOnly = true;
            this.DGV_Job_Status_CreateTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_Job_Status_CreateTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DGV_Job_Status_CreateTime.Width = 155;
            // 
            // DGV_Job_Status_JobCancelBtn
            // 
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle19.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle19.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.DGV_Job_Status_JobCancelBtn.DefaultCellStyle = dataGridViewCellStyle19;
            this.DGV_Job_Status_JobCancelBtn.HeaderText = "Cancel";
            this.DGV_Job_Status_JobCancelBtn.Name = "DGV_Job_Status_JobCancelBtn";
            this.DGV_Job_Status_JobCancelBtn.ReadOnly = true;
            this.DGV_Job_Status_JobCancelBtn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_Job_Status_JobCancelBtn.Text = "Cancel";
            this.DGV_Job_Status_JobCancelBtn.UseColumnTextForButtonValue = true;
            this.DGV_Job_Status_JobCancelBtn.Width = 80;
            // 
            // p_mission
            // 
            this.p_mission.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.p_mission.Controls.Add(this.lbl_mission);
            this.p_mission.Controls.Add(this.dtgAuto_Scheduler_Status);
            this.p_mission.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.p_mission.Location = new System.Drawing.Point(863, 504);
            this.p_mission.Name = "p_mission";
            this.p_mission.Size = new System.Drawing.Size(817, 452);
            this.p_mission.TabIndex = 281;
            // 
            // lbl_mission
            // 
            this.lbl_mission.Location = new System.Drawing.Point(3, 3);
            this.lbl_mission.Name = "lbl_mission";
            this.lbl_mission.Size = new System.Drawing.Size(95, 14);
            this.lbl_mission.TabIndex = 280;
            this.lbl_mission.Text = "Mission Scheduler";
            // 
            // AutoScreen
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(1680, 956);
            this.Controls.Add(this.p_mission);
            this.Controls.Add(this.p_possible);
            this.Controls.Add(this.P_Robot);
            this.Controls.Add(this.btn_Cancel_All_Job);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AutoScreen";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dtgAuto_Scheduler_Status)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P_Robot)).EndInit();
            this.P_Robot.ResumeLayout(false);
            this.P_Robot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgAuto_MiR_Status)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.p_possible)).EndInit();
            this.p_possible.ResumeLayout(false);
            this.p_possible.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgAuto_Job_Status)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.p_mission)).EndInit();
            this.p_mission.ResumeLayout(false);
            this.p_mission.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer AutoDisplay_Timer;
        private System.Windows.Forms.DataGridView dtgAuto_Scheduler_Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_JobStatus_LINECD;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_JobStatus_POSTCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_JobStatus_MissionName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_JobStatus_MissionState;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_JobStatus_CreateTime;
        private System.Windows.Forms.Button btn_Cancel_All_Job;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_Schedule_Status_MiR_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_Schedule_Status_CallName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_Schedule_Status_MissionName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_Schedule_Status_MissionState;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_Schedule_Status_MissionReturnID;
        private DevExpress.XtraEditors.LabelControl lbl_Robot;
        private DevExpress.XtraEditors.PanelControl P_Robot;
        private DevExpress.XtraEditors.PanelControl p_possible;
        private DevExpress.XtraEditors.LabelControl lbl_possible;
        private System.Windows.Forms.DataGridView dtgAuto_Job_Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_Job_Status_LINECD;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_Job_Status_POSTCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_Job_Status_ReturnType;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_Job_Status_MissionName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_Job_Status_MissionState;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_Job_Status_CreateTime;
        private System.Windows.Forms.DataGridViewButtonColumn DGV_Job_Status_JobCancelBtn;
        private DevExpress.XtraEditors.PanelControl p_mission;
        private DevExpress.XtraEditors.LabelControl lbl_mission;
        private DevExpress.XtraGrid.GridControl dtgAuto_MiR_Status;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
    }
}