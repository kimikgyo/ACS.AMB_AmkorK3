
namespace INA_ACS_Server
{
    partial class ElevatorScreen
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dtg_ElevatorStatusDisPlay = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DisPlaytimer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.btn_ElevatorAGVMode = new System.Windows.Forms.Button();
            this.btn_ElevatorAGVModeCancel = new System.Windows.Forms.Button();
            this.txt_Elevator_PortNumber = new System.Windows.Forms.TextBox();
            this.label154 = new System.Windows.Forms.Label();
            this.txt_Elevator_IP_Address = new System.Windows.Forms.TextBox();
            this.label152 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txt_M3F_ElevatorEndPOS = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_M3F_ElevatorEnterPOS = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txt_T3F_ElevatorEnterPOS = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_T3F_ElevatorStartPOS = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txt_T3F_ElevatorEndPOS = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_T3F_ElevatorEnterPOS_1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_M3F_ElevatorEnterPOS_1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_M3F_ElevatorStartPOS = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_ElevatorStatusDisPlay)).BeginInit();
            this.groupBox20.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtg_ElevatorStatusDisPlay
            // 
            this.dtg_ElevatorStatusDisPlay.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.dtg_ElevatorStatusDisPlay.AllowUserToAddRows = false;
            this.dtg_ElevatorStatusDisPlay.AllowUserToDeleteRows = false;
            this.dtg_ElevatorStatusDisPlay.AllowUserToResizeColumns = false;
            this.dtg_ElevatorStatusDisPlay.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 11.25F);
            this.dtg_ElevatorStatusDisPlay.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dtg_ElevatorStatusDisPlay.BackgroundColor = System.Drawing.Color.White;
            this.dtg_ElevatorStatusDisPlay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_ElevatorStatusDisPlay.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dtg_ElevatorStatusDisPlay.ColumnHeadersHeight = 50;
            this.dtg_ElevatorStatusDisPlay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dtg_ElevatorStatusDisPlay.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtg_ElevatorStatusDisPlay.DefaultCellStyle = dataGridViewCellStyle4;
            this.dtg_ElevatorStatusDisPlay.EnableHeadersVisualStyles = false;
            this.dtg_ElevatorStatusDisPlay.GridColor = System.Drawing.Color.LightGray;
            this.dtg_ElevatorStatusDisPlay.Location = new System.Drawing.Point(11, 347);
            this.dtg_ElevatorStatusDisPlay.MultiSelect = false;
            this.dtg_ElevatorStatusDisPlay.Name = "dtg_ElevatorStatusDisPlay";
            this.dtg_ElevatorStatusDisPlay.ReadOnly = true;
            this.dtg_ElevatorStatusDisPlay.RowHeadersVisible = false;
            this.dtg_ElevatorStatusDisPlay.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtg_ElevatorStatusDisPlay.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dtg_ElevatorStatusDisPlay.RowTemplate.Height = 23;
            this.dtg_ElevatorStatusDisPlay.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dtg_ElevatorStatusDisPlay.Size = new System.Drawing.Size(721, 187);
            this.dtg_ElevatorStatusDisPlay.TabIndex = 11;
            this.dtg_ElevatorStatusDisPlay.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dtg_ElevatorStatusDisPlay_CellMouseClick);
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewTextBoxColumn2.HeaderText = "EtnLampSettnig_Id";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DisPlaytimer
            // 
            this.DisPlaytimer.Enabled = true;
            this.DisPlaytimer.Interval = 1000;
            this.DisPlaytimer.Tick += new System.EventHandler(this.DisPlaytimer_Tick);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(748, 140);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(404, 394);
            this.label1.TabIndex = 12;
            this.label1.Text = "label1";
            // 
            // groupBox20
            // 
            this.groupBox20.Controls.Add(this.btn_ElevatorAGVMode);
            this.groupBox20.Controls.Add(this.btn_ElevatorAGVModeCancel);
            this.groupBox20.Controls.Add(this.txt_Elevator_PortNumber);
            this.groupBox20.Controls.Add(this.label154);
            this.groupBox20.Controls.Add(this.txt_Elevator_IP_Address);
            this.groupBox20.Controls.Add(this.label152);
            this.groupBox20.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox20.ForeColor = System.Drawing.Color.Black;
            this.groupBox20.Location = new System.Drawing.Point(11, 12);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Size = new System.Drawing.Size(1142, 104);
            this.groupBox20.TabIndex = 586;
            this.groupBox20.TabStop = false;
            this.groupBox20.Text = "Elevator_IP_Address";
            // 
            // btn_ElevatorAGVMode
            // 
            this.btn_ElevatorAGVMode.Location = new System.Drawing.Point(708, 41);
            this.btn_ElevatorAGVMode.Name = "btn_ElevatorAGVMode";
            this.btn_ElevatorAGVMode.Size = new System.Drawing.Size(182, 47);
            this.btn_ElevatorAGVMode.TabIndex = 270;
            this.btn_ElevatorAGVMode.Text = "Elevator AGV Mode ON";
            this.btn_ElevatorAGVMode.UseVisualStyleBackColor = true;
            this.btn_ElevatorAGVMode.Click += new System.EventHandler(this.ElevatorAgvMode_Click);
            // 
            // btn_ElevatorAGVModeCancel
            // 
            this.btn_ElevatorAGVModeCancel.Location = new System.Drawing.Point(905, 41);
            this.btn_ElevatorAGVModeCancel.Name = "btn_ElevatorAGVModeCancel";
            this.btn_ElevatorAGVModeCancel.Size = new System.Drawing.Size(190, 47);
            this.btn_ElevatorAGVModeCancel.TabIndex = 269;
            this.btn_ElevatorAGVModeCancel.Text = "Elevator AGV Mode OFF";
            this.btn_ElevatorAGVModeCancel.UseVisualStyleBackColor = true;
            this.btn_ElevatorAGVModeCancel.Click += new System.EventHandler(this.ElevatorAgvMode_Click);
            // 
            // txt_Elevator_PortNumber
            // 
            this.txt_Elevator_PortNumber.AccessibleDescription = "RT15";
            this.txt_Elevator_PortNumber.AccessibleName = "";
            this.txt_Elevator_PortNumber.BackColor = System.Drawing.Color.White;
            this.txt_Elevator_PortNumber.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Elevator_PortNumber.Location = new System.Drawing.Point(533, 39);
            this.txt_Elevator_PortNumber.Name = "txt_Elevator_PortNumber";
            this.txt_Elevator_PortNumber.ReadOnly = true;
            this.txt_Elevator_PortNumber.Size = new System.Drawing.Size(140, 25);
            this.txt_Elevator_PortNumber.TabIndex = 267;
            this.txt_Elevator_PortNumber.Text = "0";
            this.txt_Elevator_PortNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txt_Elevator_PortNumber.Click += new System.EventHandler(this.txt_Elevator_PortNumber_Click);
            // 
            // label154
            // 
            this.label154.AutoSize = true;
            this.label154.BackColor = System.Drawing.Color.Transparent;
            this.label154.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label154.ForeColor = System.Drawing.Color.Black;
            this.label154.Location = new System.Drawing.Point(368, 41);
            this.label154.Name = "label154";
            this.label154.Size = new System.Drawing.Size(157, 18);
            this.label154.TabIndex = 266;
            this.label154.Text = "Elevator Port Number";
            this.label154.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_Elevator_IP_Address
            // 
            this.txt_Elevator_IP_Address.AccessibleDescription = "RT15";
            this.txt_Elevator_IP_Address.AccessibleName = "";
            this.txt_Elevator_IP_Address.BackColor = System.Drawing.Color.White;
            this.txt_Elevator_IP_Address.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Elevator_IP_Address.Location = new System.Drawing.Point(167, 39);
            this.txt_Elevator_IP_Address.Name = "txt_Elevator_IP_Address";
            this.txt_Elevator_IP_Address.ReadOnly = true;
            this.txt_Elevator_IP_Address.Size = new System.Drawing.Size(156, 25);
            this.txt_Elevator_IP_Address.TabIndex = 265;
            this.txt_Elevator_IP_Address.Text = "0.0.0.0";
            this.txt_Elevator_IP_Address.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txt_Elevator_IP_Address.Click += new System.EventHandler(this.txt_Elevator_IP_Address_Click);
            // 
            // label152
            // 
            this.label152.AutoSize = true;
            this.label152.BackColor = System.Drawing.Color.Transparent;
            this.label152.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label152.ForeColor = System.Drawing.Color.Black;
            this.label152.Location = new System.Drawing.Point(6, 41);
            this.label152.Name = "label152";
            this.label152.Size = new System.Drawing.Size(156, 18);
            this.label152.TabIndex = 259;
            this.label152.Text = "Elevator_IP_Address";
            this.label152.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txt_M3F_ElevatorEndPOS);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txt_M3F_ElevatorEnterPOS);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txt_T3F_ElevatorEnterPOS);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txt_T3F_ElevatorStartPOS);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(11, 140);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(359, 201);
            this.groupBox1.TabIndex = 587;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "T3F Go M3F Elevator Position Setting";
            // 
            // txt_M3F_ElevatorEndPOS
            // 
            this.txt_M3F_ElevatorEndPOS.AccessibleDescription = "RT15";
            this.txt_M3F_ElevatorEndPOS.AccessibleName = "";
            this.txt_M3F_ElevatorEndPOS.BackColor = System.Drawing.Color.White;
            this.txt_M3F_ElevatorEndPOS.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_M3F_ElevatorEndPOS.Location = new System.Drawing.Point(167, 155);
            this.txt_M3F_ElevatorEndPOS.Name = "txt_M3F_ElevatorEndPOS";
            this.txt_M3F_ElevatorEndPOS.ReadOnly = true;
            this.txt_M3F_ElevatorEndPOS.Size = new System.Drawing.Size(156, 25);
            this.txt_M3F_ElevatorEndPOS.TabIndex = 281;
            this.txt_M3F_ElevatorEndPOS.Text = "None";
            this.txt_M3F_ElevatorEndPOS.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_M3F_ElevatorEndPOS.Click += new System.EventHandler(this.txt_ElevatorPOSSetting_Click);
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(6, 144);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(121, 44);
            this.label8.TabIndex = 280;
            this.label8.Text = "M3F End Position\r\n";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_M3F_ElevatorEnterPOS
            // 
            this.txt_M3F_ElevatorEnterPOS.AccessibleDescription = "RT15";
            this.txt_M3F_ElevatorEnterPOS.AccessibleName = "";
            this.txt_M3F_ElevatorEnterPOS.BackColor = System.Drawing.Color.White;
            this.txt_M3F_ElevatorEnterPOS.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_M3F_ElevatorEnterPOS.Location = new System.Drawing.Point(168, 112);
            this.txt_M3F_ElevatorEnterPOS.Name = "txt_M3F_ElevatorEnterPOS";
            this.txt_M3F_ElevatorEnterPOS.ReadOnly = true;
            this.txt_M3F_ElevatorEnterPOS.Size = new System.Drawing.Size(156, 25);
            this.txt_M3F_ElevatorEnterPOS.TabIndex = 279;
            this.txt_M3F_ElevatorEnterPOS.Text = "None";
            this.txt_M3F_ElevatorEnterPOS.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_M3F_ElevatorEnterPOS.Click += new System.EventHandler(this.txt_ElevatorPOSSetting_Click);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(6, 104);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(121, 44);
            this.label9.TabIndex = 278;
            this.label9.Text = "M3F Enter Position";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_T3F_ElevatorEnterPOS
            // 
            this.txt_T3F_ElevatorEnterPOS.AccessibleDescription = "RT15";
            this.txt_T3F_ElevatorEnterPOS.AccessibleName = "";
            this.txt_T3F_ElevatorEnterPOS.BackColor = System.Drawing.Color.White;
            this.txt_T3F_ElevatorEnterPOS.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_T3F_ElevatorEnterPOS.Location = new System.Drawing.Point(167, 71);
            this.txt_T3F_ElevatorEnterPOS.Name = "txt_T3F_ElevatorEnterPOS";
            this.txt_T3F_ElevatorEnterPOS.ReadOnly = true;
            this.txt_T3F_ElevatorEnterPOS.Size = new System.Drawing.Size(156, 25);
            this.txt_T3F_ElevatorEnterPOS.TabIndex = 273;
            this.txt_T3F_ElevatorEnterPOS.Text = "None";
            this.txt_T3F_ElevatorEnterPOS.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_T3F_ElevatorEnterPOS.Click += new System.EventHandler(this.txt_ElevatorPOSSetting_Click);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(6, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(121, 44);
            this.label5.TabIndex = 272;
            this.label5.Text = "T3F Enter Position";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_T3F_ElevatorStartPOS
            // 
            this.txt_T3F_ElevatorStartPOS.AccessibleDescription = "RT15";
            this.txt_T3F_ElevatorStartPOS.AccessibleName = "";
            this.txt_T3F_ElevatorStartPOS.BackColor = System.Drawing.Color.White;
            this.txt_T3F_ElevatorStartPOS.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_T3F_ElevatorStartPOS.Location = new System.Drawing.Point(168, 30);
            this.txt_T3F_ElevatorStartPOS.Name = "txt_T3F_ElevatorStartPOS";
            this.txt_T3F_ElevatorStartPOS.ReadOnly = true;
            this.txt_T3F_ElevatorStartPOS.Size = new System.Drawing.Size(156, 25);
            this.txt_T3F_ElevatorStartPOS.TabIndex = 271;
            this.txt_T3F_ElevatorStartPOS.Text = "None";
            this.txt_T3F_ElevatorStartPOS.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_T3F_ElevatorStartPOS.Click += new System.EventHandler(this.txt_ElevatorPOSSetting_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 44);
            this.label2.TabIndex = 260;
            this.label2.Text = "T3F \r\nStart Position";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txt_T3F_ElevatorEndPOS);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txt_T3F_ElevatorEnterPOS_1);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txt_M3F_ElevatorEnterPOS_1);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txt_M3F_ElevatorStartPOS);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.Black;
            this.groupBox2.Location = new System.Drawing.Point(376, 144);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(356, 197);
            this.groupBox2.TabIndex = 588;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "M3F Go T3F Elevator Position Setting";
            // 
            // txt_T3F_ElevatorEndPOS
            // 
            this.txt_T3F_ElevatorEndPOS.AccessibleDescription = "RT15";
            this.txt_T3F_ElevatorEndPOS.AccessibleName = "";
            this.txt_T3F_ElevatorEndPOS.BackColor = System.Drawing.Color.White;
            this.txt_T3F_ElevatorEndPOS.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_T3F_ElevatorEndPOS.Location = new System.Drawing.Point(167, 151);
            this.txt_T3F_ElevatorEndPOS.Name = "txt_T3F_ElevatorEndPOS";
            this.txt_T3F_ElevatorEndPOS.ReadOnly = true;
            this.txt_T3F_ElevatorEndPOS.Size = new System.Drawing.Size(156, 25);
            this.txt_T3F_ElevatorEndPOS.TabIndex = 281;
            this.txt_T3F_ElevatorEndPOS.Text = "None";
            this.txt_T3F_ElevatorEndPOS.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_T3F_ElevatorEndPOS.Click += new System.EventHandler(this.txt_ElevatorPOSSetting_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(6, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 44);
            this.label3.TabIndex = 280;
            this.label3.Text = "T3F End Position\r\n";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_T3F_ElevatorEnterPOS_1
            // 
            this.txt_T3F_ElevatorEnterPOS_1.AccessibleDescription = "RT15";
            this.txt_T3F_ElevatorEnterPOS_1.AccessibleName = "";
            this.txt_T3F_ElevatorEnterPOS_1.BackColor = System.Drawing.Color.White;
            this.txt_T3F_ElevatorEnterPOS_1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_T3F_ElevatorEnterPOS_1.Location = new System.Drawing.Point(168, 108);
            this.txt_T3F_ElevatorEnterPOS_1.Name = "txt_T3F_ElevatorEnterPOS_1";
            this.txt_T3F_ElevatorEnterPOS_1.ReadOnly = true;
            this.txt_T3F_ElevatorEnterPOS_1.Size = new System.Drawing.Size(156, 25);
            this.txt_T3F_ElevatorEnterPOS_1.TabIndex = 279;
            this.txt_T3F_ElevatorEnterPOS_1.Text = "None";
            this.txt_T3F_ElevatorEnterPOS_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_T3F_ElevatorEnterPOS_1.Click += new System.EventHandler(this.txt_ElevatorPOSSetting_Click);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(6, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(121, 44);
            this.label4.TabIndex = 278;
            this.label4.Text = "T3F Enter Position";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_M3F_ElevatorEnterPOS_1
            // 
            this.txt_M3F_ElevatorEnterPOS_1.AccessibleDescription = "RT15";
            this.txt_M3F_ElevatorEnterPOS_1.AccessibleName = "";
            this.txt_M3F_ElevatorEnterPOS_1.BackColor = System.Drawing.Color.White;
            this.txt_M3F_ElevatorEnterPOS_1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_M3F_ElevatorEnterPOS_1.Location = new System.Drawing.Point(167, 67);
            this.txt_M3F_ElevatorEnterPOS_1.Name = "txt_M3F_ElevatorEnterPOS_1";
            this.txt_M3F_ElevatorEnterPOS_1.ReadOnly = true;
            this.txt_M3F_ElevatorEnterPOS_1.Size = new System.Drawing.Size(156, 25);
            this.txt_M3F_ElevatorEnterPOS_1.TabIndex = 273;
            this.txt_M3F_ElevatorEnterPOS_1.Text = "None";
            this.txt_M3F_ElevatorEnterPOS_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_M3F_ElevatorEnterPOS_1.Click += new System.EventHandler(this.txt_ElevatorPOSSetting_Click);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(6, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(121, 44);
            this.label6.TabIndex = 272;
            this.label6.Text = "M3F Enter Position";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_M3F_ElevatorStartPOS
            // 
            this.txt_M3F_ElevatorStartPOS.AccessibleDescription = "RT15";
            this.txt_M3F_ElevatorStartPOS.AccessibleName = "";
            this.txt_M3F_ElevatorStartPOS.BackColor = System.Drawing.Color.White;
            this.txt_M3F_ElevatorStartPOS.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_M3F_ElevatorStartPOS.Location = new System.Drawing.Point(168, 26);
            this.txt_M3F_ElevatorStartPOS.Name = "txt_M3F_ElevatorStartPOS";
            this.txt_M3F_ElevatorStartPOS.ReadOnly = true;
            this.txt_M3F_ElevatorStartPOS.Size = new System.Drawing.Size(156, 25);
            this.txt_M3F_ElevatorStartPOS.TabIndex = 271;
            this.txt_M3F_ElevatorStartPOS.Text = "None";
            this.txt_M3F_ElevatorStartPOS.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_M3F_ElevatorStartPOS.Click += new System.EventHandler(this.txt_ElevatorPOSSetting_Click);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(6, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(121, 44);
            this.label7.TabIndex = 260;
            this.label7.Text = "M3F \r\nStart Position";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ElevatorScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1170, 581);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox20);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtg_ElevatorStatusDisPlay);
            this.Name = "ElevatorScreen";
            this.Text = "ElevatorScreen";
            ((System.ComponentModel.ISupportInitialize)(this.dtg_ElevatorStatusDisPlay)).EndInit();
            this.groupBox20.ResumeLayout(false);
            this.groupBox20.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dtg_ElevatorStatusDisPlay;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.Timer DisPlaytimer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox20;
        private System.Windows.Forms.Button btn_ElevatorAGVMode;
        private System.Windows.Forms.Button btn_ElevatorAGVModeCancel;
        private System.Windows.Forms.TextBox txt_Elevator_PortNumber;
        private System.Windows.Forms.Label label154;
        private System.Windows.Forms.TextBox txt_Elevator_IP_Address;
        private System.Windows.Forms.Label label152;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txt_T3F_ElevatorEnterPOS;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_T3F_ElevatorStartPOS;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_M3F_ElevatorEndPOS;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_M3F_ElevatorEnterPOS;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txt_T3F_ElevatorEndPOS;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_T3F_ElevatorEnterPOS_1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_M3F_ElevatorEnterPOS_1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_M3F_ElevatorStartPOS;
        private System.Windows.Forms.Label label7;
    }
}