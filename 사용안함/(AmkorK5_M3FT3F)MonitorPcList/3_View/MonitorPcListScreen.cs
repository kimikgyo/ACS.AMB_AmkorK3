using INA_ACS_Server.Models.AmkorK5_M3F_T3F;
using INA_ACS_Server.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INA_ACS_Server.Views
{
    public partial class MonitorPcListScreen : Form
    {
        MainForm mainForm;
        IUnitOfWork uow;

        public MonitorPcListScreen(MainForm mainForm, IUnitOfWork uow)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.uow = uow;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.FormBorderStyle = FormBorderStyle.None; //윈도우(상단) 테두리 제거 source code
            dtg_MonitorPcList.AlternatingRowsDefaultCellStyle = null;
            dtg_MonitorPcList.DoubleBuffered(true);

            TextBoxInit();
            MonitorPcListInit();
            MonitorPcListDisplay();
        }

        private void TextBoxInit()
        {
            txt_MonitorPcListMaxNum.Text = ConfigData.MonitorPcList_MaxNum.ToString();
        }

        private void MonitorPcListInit()
        {
            dtg_MonitorPcList.ScrollBars = ScrollBars.Both;
            dtg_MonitorPcList.AllowUserToResizeColumns = true;
            dtg_MonitorPcList.ColumnHeadersHeight = 40;
            dtg_MonitorPcList.RowTemplate.Height = 40;
            dtg_MonitorPcList.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_MonitorPcList.Columns[0].CellTemplate;
            dtg_MonitorPcList.Columns.Clear();
            dtg_MonitorPcList.Columns.Add(new DataGridViewColumn() { Name = "DGV_MonitorPcList_Id", HeaderText = "Index", Width = 70, CellTemplate = currentCellTemplate });
            dtg_MonitorPcList.Columns.Add(new DataGridViewColumn() { Name = "DGV_MonitorPcList_IpAddress", HeaderText = "IpAddress", Width = 200, CellTemplate = currentCellTemplate });
            dtg_MonitorPcList.Columns.Add(new DataGridViewColumn() { Name = "DGV_MonitorPcList_ZoneName", HeaderText = "PositionZoneName", Width = 200, CellTemplate = currentCellTemplate });
            dtg_MonitorPcList.Columns.Add(new DataGridViewCheckBoxColumn() { Name = "DGV_MonitorPcList_BcrExist", HeaderText = "Barcode", Width = 125 });
            dtg_MonitorPcList.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_MonitorPcList.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_MonitorPcList.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
            dtg_MonitorPcList.Rows.Clear();
        }

        private void MonitorPcListDisplay()
        {
            dtg_MonitorPcList.Columns["DGV_MonitorPcList_Id"].ReadOnly = true;
            dtg_MonitorPcList.Columns["DGV_MonitorPcList_ZoneName"].ReadOnly = true;
            dtg_MonitorPcList.Columns["DGV_MonitorPcList_BcrExist"].ReadOnly = true;

            int firstRowIndex = dtg_MonitorPcList.FirstDisplayedScrollingColumnIndex;
            dtg_MonitorPcList.Rows.Clear();
            dtg_MonitorPcList.RowTemplate.Height = 40;
            foreach (var monitorPcList in uow.MonitorPcList.GetAll())
            {
                int newRowIndex = dtg_MonitorPcList.Rows.Add();
                var newRow = dtg_MonitorPcList.Rows[newRowIndex];
                newRow.Cells["DGV_MonitorPcList_Id"].Value = monitorPcList.Id.ToString();
                newRow.Cells["DGV_MonitorPcList_IpAddress"].Value = monitorPcList.IpAddress;
                newRow.Cells["DGV_MonitorPcList_ZoneName"].Value = monitorPcList.ZoneName;
                newRow.Cells["DGV_MonitorPcList_BcrExist"].Value = monitorPcList.BcrExist;
                newRow.Tag = monitorPcList;

            }

            if (firstRowIndex >= 0 && firstRowIndex < dtg_MonitorPcList.Rows.Count)
            {
                dtg_MonitorPcList.FirstDisplayedScrollingRowIndex = firstRowIndex;
            }
            dtg_MonitorPcList.ClearSelection();
        }

        private void txt_MonitorPcListMaxNum_Click(object sender, EventArgs e)
        {
            NumPadForm insertNum = new NumPadForm(((TextBox)sender).Text, ((TextBox)sender).Name, "RT10");
            if (insertNum.ShowDialog() == DialogResult.OK)
            {
                string newValue = int.Parse(insertNum.InputValue).ToString();

                if (((TextBox)sender).Text != newValue)
                {
                    string oldValue = ((TextBox)sender).Text;
                    ((TextBox)sender).Text = newValue;

                    AppConfiguration.ConfigDataSetting("MonitorPcList_MaxNum", ((TextBox)sender).Text);
                    ConfigData.MonitorPcList_MaxNum = int.Parse(AppConfiguration.GetAppConfig("MonitorPcList_MaxNum"));
                    insertNum.Close();

                    uow.MonitorPcList.Validate_DB_Items();
                    MonitorPcListDisplay();

                    mainForm.UserLog("MonitorPcList Screen", $"MonitorPcList_MaxNum Count Value Change from {oldValue} to {newValue}");
                }

            }
        }

        private void dtg_MonitorPcList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

                DataGridView grid = (DataGridView)sender;
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

                MonitorPcListModel selectedRowTag = (MonitorPcListModel)selectedRow.Tag;
                MonitorPcListModel targetConfig = uow.MonitorPcList.Find(m => m.Id == selectedRowTag.Id).FirstOrDefault();

                string ChangeDatamsg = null;

                //Convert.ToString 사용시 변수가 null인경우 빈문자열을 반환함 
                //.Tostring() 사용시 변수가 null인경우 에러발생
                if (!string.IsNullOrEmpty(Convert.ToString(selectedCell.Value)) && targetConfig != null)
                {
                    string newValue = String.Concat(selectedCell.Value.ToString().Where(c => !char.IsWhiteSpace(c)));    //문자열 빈칸없애기

                    if (e.ColumnIndex == grid.Columns["DGV_MonitorPcList_IpAddress"].Index)
                    {

                        bool flag = System.Net.IPAddress.TryParse(newValue, out var newIp); // check ip address
                        string newIpAddress = flag ? newIp.ToString() : targetConfig.IpAddress;

                        var monitorPcListIpAddress = uow.MonitorPcList.Find(m => m.IpAddress == newIpAddress).FirstOrDefault();
                        if (monitorPcListIpAddress == null || (newValue == "0.0.0.0"))
                        {
                            if (targetConfig.IpAddress != newValue)
                            {
                                string oldIpAddress = targetConfig.IpAddress;
                                targetConfig.IpAddress = newIpAddress;
                                ChangeDatamsg = $"MonitorPcList,MonitorPcList {targetConfig.Id} MonitorPcList IpAddress changed from {oldIpAddress} to {newIpAddress}.";
                                ChageData(newValue, ChangeDatamsg);
                            }
                        }
                        else
                        {
                            MessageBox.Show("같은 IpAddress가 있습니다." + "\r\n" + "다시확인후 입력해주시기 바랍니다.");

                        }

                    }

                }
                else MessageBox.Show("확인후 등록 해주시기 바랍니다.");


                void ChageData(string newValue, string changeDatamsg)
                {
                    if (changeDatamsg != null)
                    {
                        selectedCell.Value = newValue;
                        selectedRowTag = targetConfig;
                        uow.MonitorPcList.Update(targetConfig);
                        string[] ChangeDatamsgSplit = ChangeDatamsg.Split(',');
                        mainForm.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                        MessageBox.Show("저장되었습니다");

                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException?.Message ?? ex.Message;
                Debug.WriteLine(message);
                //EventLogger.Info(message + "\n[StackTrace]\n" + ex.StackTrace);
                mainForm.ACS_UI_Log(message);
            }
        }

        private void dtg_MonitorPcList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

            DataGridView grid = (DataGridView)sender;
            DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
            DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

            MonitorPcListModel selectedRowTag = (MonitorPcListModel)selectedRow.Tag;
            MonitorPcListModel targetConfig = uow.MonitorPcList.Find(m => m.Id == selectedRowTag.Id).FirstOrDefault();

            string ChangeDatamsg = null;

            if (targetConfig != null)
            {
                if (e.ColumnIndex == grid.Columns["DGV_MonitorPcList_ZoneName"].Index) //자재 유 (true) 무 (false)
                {
                    var insert = new PositionZoneSelectForm(null,uow);
                    if (insert.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insert.InputValue.Trim();
                        if (targetConfig.ZoneName != newValue)
                        {
                            string oldValue = targetConfig.ZoneName;
                            targetConfig.ZoneName = newValue;
                            insert.Close();

                            ChangeDatamsg = $"MonitorPcList,MonitorPcList {targetConfig.Id} MonitorPcList ZoneName changed from {oldValue} to {newValue}.";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                }

                else if (e.ColumnIndex == grid.Columns["DGV_MonitorPcList_BcrExist"].Index) //자재 유 (true) 무 (false)
                {
                    bool newValue = !Convert.ToBoolean(selectedRow.Cells["DGV_MonitorPcList_BcrExist"].Value);

                    if (targetConfig.BcrExist != newValue)
                    {
                        bool oldValue = targetConfig.BcrExist;
                        targetConfig.BcrExist = newValue;
                        ChangeDatamsg = $"MonitorPcList,MonitorPcList {targetConfig.Id} MonitorPcList BcrExist changed from {oldValue} to {newValue}.";
                        ChageData(newValue.ToString(), ChangeDatamsg);
                    }
                }
            }
            else MessageBox.Show("확인후 등록 해주시기 바랍니다.");

            void ChageData(string newValue, string changeDatamsg)
            {
                if (changeDatamsg != null)
                {
                    selectedCell.Value = newValue;
                    selectedRowTag = targetConfig;
                    uow.MonitorPcList.Update(targetConfig);
                    string[] ChangeDatamsgSplit = ChangeDatamsg.Split(',');
                    mainForm.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);

                }
            }
        }

        private void btn_BackUpSaveFile_Click(object sender, EventArgs e)
        {
            if (dtg_MonitorPcList.Rows.Count == 0) return;
            mainForm.SaveAsDataGridviewToCSV(dtg_MonitorPcList);
            mainForm.UserLog("MonitorPcList Screen", " BackUp Click ");

        }
    }
}
