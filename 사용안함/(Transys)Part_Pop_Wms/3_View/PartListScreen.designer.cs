namespace INA_ACS_Server
{
    partial class PartListScreen
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnAddToBindingSource = new System.Windows.Forms.Button();
            this.lblIsDone = new System.Windows.Forms.Label();
            this.blnIsDone = new System.Windows.Forms.CheckBox();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnEdit);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.btnAddToBindingSource);
            this.panel1.Controls.Add(this.lblIsDone);
            this.panel1.Controls.Add(this.blnIsDone);
            this.panel1.Controls.Add(this.txtTitle);
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1638, 97);
            this.panel1.TabIndex = 10;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(14, 13);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(172, 69);
            this.btnRefresh.TabIndex = 18;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(558, 13);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(172, 69);
            this.btnDelete.TabIndex = 17;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(376, 13);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(172, 69);
            this.btnEdit.TabIndex = 16;
            this.btnEdit.Text = "수정";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(194, 13);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(172, 69);
            this.btnAdd.TabIndex = 15;
            this.btnAdd.Text = "추가";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnAddToBindingSource
            // 
            this.btnAddToBindingSource.Location = new System.Drawing.Point(1368, 15);
            this.btnAddToBindingSource.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnAddToBindingSource.Name = "btnAddToBindingSource";
            this.btnAddToBindingSource.Size = new System.Drawing.Size(242, 68);
            this.btnAddToBindingSource.TabIndex = 14;
            this.btnAddToBindingSource.Text = "add to binding source";
            this.btnAddToBindingSource.UseVisualStyleBackColor = true;
            this.btnAddToBindingSource.Visible = false;
            this.btnAddToBindingSource.Click += new System.EventHandler(this.btnAddToBindingSource_Click);
            // 
            // lblIsDone
            // 
            this.lblIsDone.AutoSize = true;
            this.lblIsDone.Location = new System.Drawing.Point(1034, 59);
            this.lblIsDone.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblIsDone.Name = "lblIsDone";
            this.lblIsDone.Size = new System.Drawing.Size(62, 18);
            this.lblIsDone.TabIndex = 13;
            this.lblIsDone.Text = "완 료: ";
            this.lblIsDone.Visible = false;
            // 
            // blnIsDone
            // 
            this.blnIsDone.AutoSize = true;
            this.blnIsDone.Location = new System.Drawing.Point(1098, 59);
            this.blnIsDone.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.blnIsDone.Name = "blnIsDone";
            this.blnIsDone.Size = new System.Drawing.Size(51, 22);
            this.blnIsDone.TabIndex = 12;
            this.blnIsDone.Text = "완료";
            this.blnIsDone.UseVisualStyleBackColor = true;
            this.blnIsDone.Visible = false;
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(1098, 9);
            this.txtTitle.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(234, 26);
            this.txtTitle.TabIndex = 11;
            this.txtTitle.Visible = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(1034, 13);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(52, 18);
            this.lblTitle.TabIndex = 10;
            this.lblTitle.Text = "할 일:";
            this.lblTitle.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 97);
            this.panel2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1638, 698);
            this.panel2.TabIndex = 11;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1638, 698);
            this.dataGridView1.TabIndex = 6;
            this.dataGridView1.Text = "dataGridView1";
            // 
            // PartListScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1638, 795);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.Name = "PartListScreen";
            this.Text = "Part List";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnAddToBindingSource;
        private System.Windows.Forms.Label lblIsDone;
        private System.Windows.Forms.CheckBox blnIsDone;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

