
namespace INA_ACS_Server
{
    partial class ElevatorModeHistoryScreen
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkboxUserNumber = new System.Windows.Forms.CheckBox();
            this.cboboxUserNumber = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSearchCount = new System.Windows.Forms.TextBox();
            this.btnSearchCount2 = new System.Windows.Forms.Button();
            this.btnSearchCount1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dtg_ElevatorModeHistory = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_ElevatorModeHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.btnSearchCount2);
            this.panel1.Controls.Add(this.btnSearchCount1);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1642, 214);
            this.panel1.TabIndex = 11;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkboxUserNumber);
            this.groupBox2.Controls.Add(this.cboboxUserNumber);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtSearchCount);
            this.groupBox2.Location = new System.Drawing.Point(431, 14);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(419, 177);
            this.groupBox2.TabIndex = 38;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // chkboxUserNumber
            // 
            this.chkboxUserNumber.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkboxUserNumber.Location = new System.Drawing.Point(7, 65);
            this.chkboxUserNumber.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkboxUserNumber.Name = "chkboxUserNumber";
            this.chkboxUserNumber.Size = new System.Drawing.Size(80, 25);
            this.chkboxUserNumber.TabIndex = 22;
            this.chkboxUserNumber.Text = "User";
            this.chkboxUserNumber.UseVisualStyleBackColor = true;
            // 
            // cboboxUserNumber
            // 
            this.cboboxUserNumber.Enabled = false;
            this.cboboxUserNumber.FormattingEnabled = true;
            this.cboboxUserNumber.Location = new System.Drawing.Point(136, 59);
            this.cboboxUserNumber.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboboxUserNumber.Name = "cboboxUserNumber";
            this.cboboxUserNumber.Size = new System.Drawing.Size(243, 27);
            this.cboboxUserNumber.TabIndex = 32;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 114);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 19);
            this.label1.TabIndex = 20;
            this.label1.Text = "Total Count";
            // 
            // txtSearchCount
            // 
            this.txtSearchCount.Location = new System.Drawing.Point(136, 109);
            this.txtSearchCount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSearchCount.Name = "txtSearchCount";
            this.txtSearchCount.ReadOnly = true;
            this.txtSearchCount.Size = new System.Drawing.Size(243, 26);
            this.txtSearchCount.TabIndex = 19;
            // 
            // btnSearchCount2
            // 
            this.btnSearchCount2.Location = new System.Drawing.Point(1301, 119);
            this.btnSearchCount2.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.btnSearchCount2.Name = "btnSearchCount2";
            this.btnSearchCount2.Size = new System.Drawing.Size(163, 89);
            this.btnSearchCount2.TabIndex = 37;
            this.btnSearchCount2.Text = "Search Count(월)";
            this.btnSearchCount2.UseVisualStyleBackColor = true;
            this.btnSearchCount2.Click += new System.EventHandler(this.btnSearchCount2_Click);
            // 
            // btnSearchCount1
            // 
            this.btnSearchCount1.Location = new System.Drawing.Point(1301, 29);
            this.btnSearchCount1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.btnSearchCount1.Name = "btnSearchCount1";
            this.btnSearchCount1.Size = new System.Drawing.Size(163, 89);
            this.btnSearchCount1.TabIndex = 36;
            this.btnSearchCount1.Text = "Search Count";
            this.btnSearchCount1.UseVisualStyleBackColor = true;
            this.btnSearchCount1.Click += new System.EventHandler(this.btnSearchCount1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.dateTimePicker2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(17, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(387, 190);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Date";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(253, 130);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 38);
            this.button3.TabIndex = 24;
            this.button3.Text = "last week";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.btnRange3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(147, 130);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 38);
            this.button2.TabIndex = 23;
            this.button2.Text = "today";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnRange2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(41, 130);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 38);
            this.button1.TabIndex = 22;
            this.button1.Text = "yesterday";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnRange1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 44);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 19);
            this.label2.TabIndex = 21;
            this.label2.Text = "From";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(80, 38);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(284, 26);
            this.dateTimePicker1.TabIndex = 0;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(80, 84);
            this.dateTimePicker2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(284, 26);
            this.dateTimePicker2.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 90);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 19);
            this.label3.TabIndex = 21;
            this.label3.Text = "To";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(1472, 119);
            this.btnClear.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(163, 89);
            this.btnClear.TabIndex = 33;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btn_Clear_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(1472, 29);
            this.btnSave.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(163, 89);
            this.btnSave.TabIndex = 23;
            this.btnSave.Text = "Save to File";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(1083, 29);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(204, 89);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dtg_ElevatorModeHistory);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 214);
            this.panel2.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1642, 640);
            this.panel2.TabIndex = 12;
            // 
            // dtg_ElevatorModeHistory
            // 
            this.dtg_ElevatorModeHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtg_ElevatorModeHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtg_ElevatorModeHistory.Location = new System.Drawing.Point(0, 0);
            this.dtg_ElevatorModeHistory.Margin = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.dtg_ElevatorModeHistory.Name = "dtg_ElevatorModeHistory";
            this.dtg_ElevatorModeHistory.Size = new System.Drawing.Size(1642, 640);
            this.dtg_ElevatorModeHistory.TabIndex = 6;
            this.dtg_ElevatorModeHistory.Text = "dataGridView1";
            // 
            // ElevatorModeHistoryScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1642, 854);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ElevatorModeHistoryScreen";
            this.Text = "JobHistoryScreen";
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtg_ElevatorModeHistory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dtg_ElevatorModeHistory;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnSearchCount1;
        private System.Windows.Forms.Button btnSearchCount2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkboxUserNumber;
        private System.Windows.Forms.ComboBox cboboxUserNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSearchCount;
    }
}