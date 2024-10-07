using INA_ACS_Server.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INA_ACS_Server
{
    public partial class ErrorCodeListScreen : Form
    {
        MainForm mainForm;
        IUnitOfWork uow;

        public ErrorCodeListScreen(MainForm mainForm, IUnitOfWork uow)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.uow = uow;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.FormBorderStyle = FormBorderStyle.None; //윈도우(상단) 테두리 제거 source code
            dtg_ErrorCodeList.AlternatingRowsDefaultCellStyle = null;
            dtg_ErrorCodeList.DoubleBuffered(true);

            TextBoxInit();
            ErrorCodeListInlt();
            ErrorcodeList_Display();
        }
        private void TextBoxInit()
        {
            txt_ErrorPositionGroup.Text = ConfigData.ErrorPositionGroup;
        }

        private void ErrorCodeListInlt()
        {
            dtg_ErrorCodeList.ScrollBars = ScrollBars.Both;
            dtg_ErrorCodeList.AllowUserToResizeColumns = true;
            dtg_ErrorCodeList.ColumnHeadersHeight = 50;
            dtg_ErrorCodeList.RowTemplate.Height = 40;
            dtg_ErrorCodeList.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_ErrorCodeList.Columns[0].CellTemplate;
            dtg_ErrorCodeList.Columns.Clear();
            dtg_ErrorCodeList.Columns.Add(new DataGridViewColumn() { Name = "DGV_ErrorCodeLsit_Id", HeaderText = "Index", Width = 70, CellTemplate = currentCellTemplate });
            dtg_ErrorCodeList.Columns.Add(new DataGridViewColumn() { Name = "DGV_ErrorCodeLsit_ErrorCode", HeaderText = "ErrorCode", Width = 100, CellTemplate = currentCellTemplate });
            dtg_ErrorCodeList.Columns.Add(new DataGridViewColumn() { Name = "DGV_ErrorCodeLsit_ErrorMessage", HeaderText = "ErrorMessage", Width = 640, CellTemplate = currentCellTemplate });
            dtg_ErrorCodeList.Columns.Add(new DataGridViewColumn() { Name = "DGV_ErrorCodeLsit_ErrorType", HeaderText = "ErrorType", Width = 100, CellTemplate = currentCellTemplate });
            dtg_ErrorCodeList.Columns.Add(new DataGridViewColumn() { Name = "DGV_ErrorCodeLsit_Explanation", HeaderText = "Explanation", Width = 640, CellTemplate = currentCellTemplate });
            dtg_ErrorCodeList.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_ErrorCodeList.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_ErrorCodeList.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬

            dtg_ErrorCodeList.Rows.Clear();
        }

        private void ErrorcodeList_Display()
        {
            dtg_ErrorCodeList.Columns["DGV_ErrorCodeLsit_Id"].ReadOnly = true;
            dtg_ErrorCodeList.Columns["DGV_ErrorCodeLsit_ErrorCode"].ReadOnly = true;
            dtg_ErrorCodeList.Columns["DGV_ErrorCodeLsit_ErrorMessage"].ReadOnly = true;
            dtg_ErrorCodeList.Columns["DGV_ErrorCodeLsit_ErrorType"].ReadOnly = true;
            dtg_ErrorCodeList.Columns["DGV_ErrorCodeLsit_Explanation"].ReadOnly = true;

            int firstRowIndex = dtg_ErrorCodeList.FirstDisplayedScrollingColumnIndex;
            dtg_ErrorCodeList.Rows.Clear();
            dtg_ErrorCodeList.RowTemplate.Height = 40;
            foreach (var errorCodeList in uow.ErrorCodeList.GetAll())
            {
                int newRowIndex = dtg_ErrorCodeList.Rows.Add();
                var newRow = dtg_ErrorCodeList.Rows[newRowIndex];
                newRow.Cells["DGV_ErrorCodeLsit_Id"].Value = errorCodeList.Id.ToString();
                newRow.Cells["DGV_ErrorCodeLsit_ErrorCode"].Value = errorCodeList.ErrorCode.ToString();
                newRow.Cells["DGV_ErrorCodeLsit_ErrorMessage"].Value = errorCodeList.ErrorMessage;
                newRow.Cells["DGV_ErrorCodeLsit_ErrorType"].Value = errorCodeList.ErrorType;
                newRow.Cells["DGV_ErrorCodeLsit_Explanation"].Value = errorCodeList.Explanation;
                newRow.Tag = errorCodeList;
            }

            if (firstRowIndex >= 0 && firstRowIndex < dtg_ErrorCodeList.Rows.Count)
            {
                dtg_ErrorCodeList.FirstDisplayedScrollingRowIndex = firstRowIndex;
            }
            dtg_ErrorCodeList.ClearSelection();
        }

   
        private void btn_BackUpSaveFile_Click(object sender, EventArgs e)
        {
            DataGridView BackUpData = null;
            string ButtonName = ((Button)sender).Name;

            switch (ButtonName)
            {
                case "btn_ErrorCodeListBackup":
                    if (dtg_ErrorCodeList.Rows.Count == 0) return;
                    BackUpData = dtg_ErrorCodeList;
                    break;
            }
            if (BackUpData != null)
            {
                mainForm.SaveAsDataGridviewToCSV(BackUpData);
                mainForm.UserLog("ErrorCodeList Screen", $"{ButtonName} BackUp Click ");
            }
        }

        private void txt_ErrorPositionGroup_Click(object sender, EventArgs e)
        {
            var groupSelectForm = new GroupSelectForm(mainForm, uow);
            if (groupSelectForm.ShowDialog() == DialogResult.OK)
            {
                string newValue = groupSelectForm.InputValue.Trim();

                if (((TextBox)sender).Text != newValue)
                {
                    string oldValue = ((TextBox)sender).Text;
                    ((TextBox)sender).Text = newValue;

                    AppConfiguration.ConfigDataSetting("ErrorPositionGroup", newValue);
                    ConfigData.ErrorPositionGroup = AppConfiguration.GetAppConfig("ErrorPositionGroup");
                    groupSelectForm.Close();
                    mainForm.UserLog("ErrorCodeList Screen", $"ErrorPositionGroup Change from {oldValue} to {newValue}");
                }

            }
        }
    }
}
