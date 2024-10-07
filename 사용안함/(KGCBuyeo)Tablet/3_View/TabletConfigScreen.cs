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
    public partial class TabletConfigScreen : Form
    {
        MainForm mainForm;
        IUnitOfWork uow;

        public TabletConfigScreen(MainForm mainForm, IUnitOfWork uow)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.uow = uow;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.FormBorderStyle = FormBorderStyle.None; //윈도우(상단) 테두리 제거 source code

            dtg_TabletIpAddressConfig.AlternatingRowsDefaultCellStyle = null;
            dtg_TabletZoneConfig.AlternatingRowsDefaultCellStyle = null;
            dtg_TabletZonePalletConfig.AlternatingRowsDefaultCellStyle = null;

            dtg_TabletIpAddressConfig.DoubleBuffered(true);
            dtg_TabletZoneConfig.DoubleBuffered(true);
            dtg_TabletZonePalletConfig.DoubleBuffered(true);

            TextBoxInit();
            TabletIpAddressConfigInit();
            TabletIpAddressConfigDisPlay();
            TabletZoneConfigInit();
            TabletZoneConfigDisPlay();
            TabletZonePalletConfigInit();
            TabletZonePalletConfigDisPaly();
       
        }

        private void TextBoxInit()
        {
            txt_TabletIpAddressConfigMaxNum.Text = ConfigData.TabletIpAddressConfigMaxNum.ToString();
            txt_TabletZoneConfigMaxNum.Text = ConfigData.TabletZoneConfigMaxNum.ToString();
            txt_TabletZonePalletConfigMaxNum.Text = ConfigData.TabletZonePalletConfigMaxNum.ToString();
        }
        private void TabletIpAddressConfigInit()
        {
            dtg_TabletIpAddressConfig.ScrollBars = ScrollBars.Both;
            dtg_TabletIpAddressConfig.AllowUserToResizeColumns = true;
            dtg_TabletIpAddressConfig.ColumnHeadersHeight = 70;
            dtg_TabletIpAddressConfig.RowTemplate.Height = 70;
            dtg_TabletIpAddressConfig.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_TabletIpAddressConfig.Columns[0].CellTemplate;
            dtg_TabletIpAddressConfig.Columns.Clear();
            dtg_TabletIpAddressConfig.Columns.Add(new DataGridViewColumn() { Name = "DGV_TabletIpAddress_Id", HeaderText = "Index", Width = 70, CellTemplate = currentCellTemplate });
            dtg_TabletIpAddressConfig.Columns.Add(new DataGridViewColumn() { Name = "DGV_TabletIpAddress_IpAddress", HeaderText = "IpAddress", Width = 220, CellTemplate = currentCellTemplate });
            dtg_TabletIpAddressConfig.Columns.Add(new DataGridViewColumn() { Name = "DGV_TabletIpAddress_ZoneName", HeaderText = "ZoneName", Width = 220, CellTemplate = currentCellTemplate });
            dtg_TabletIpAddressConfig.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_TabletIpAddressConfig.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_TabletIpAddressConfig.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
            dtg_TabletIpAddressConfig.Rows.Clear();
        }
        private void TabletIpAddressConfigDisPlay()
        {
            dtg_TabletIpAddressConfig.Columns["DGV_TabletIpAddress_Id"].ReadOnly = true; //편집사용 불가 . 클릭시 팝업창에서 편집

            int firstRowIndex = dtg_TabletIpAddressConfig.FirstDisplayedScrollingColumnIndex;
            dtg_TabletIpAddressConfig.CurrentCell = null;
            dtg_TabletIpAddressConfig.ClearSelection();
            dtg_TabletIpAddressConfig.Rows.Clear();
            dtg_TabletIpAddressConfig.RowTemplate.Height = 50;

            foreach (var tabletIpAddressCfg in uow.TabletIpAddressConfig.GetAll())
            {
                int newRowIndex = dtg_TabletIpAddressConfig.Rows.Add();
                var newRow = dtg_TabletIpAddressConfig.Rows[newRowIndex];
                newRow.Cells["DGV_TabletIpAddress_Id"].Value = tabletIpAddressCfg.Seq;
                newRow.Cells["DGV_TabletIpAddress_IpAddress"].Value = tabletIpAddressCfg.IP;
                newRow.Cells["DGV_TabletIpAddress_ZoneName"].Value = tabletIpAddressCfg.ZONENAME;
                newRow.Tag = tabletIpAddressCfg;

                newRow.Cells["DGV_TabletIpAddress_Id"].Style.BackColor = Color.WhiteSmoke;

            }
            if (firstRowIndex >= 0 && firstRowIndex < dtg_TabletIpAddressConfig.Rows.Count)
            {
                dtg_TabletIpAddressConfig.FirstDisplayedScrollingRowIndex = firstRowIndex;
            }
            dtg_TabletIpAddressConfig.ClearSelection();
        }
        private void TabletZoneConfigInit()
        {
            dtg_TabletZoneConfig.ScrollBars = ScrollBars.Both;
            dtg_TabletZoneConfig.AllowUserToResizeColumns = true;
            dtg_TabletZoneConfig.ColumnHeadersHeight = 70;
            dtg_TabletZoneConfig.RowTemplate.Height = 70;
            dtg_TabletZoneConfig.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_TabletZoneConfig.Columns[0].CellTemplate;
            dtg_TabletZoneConfig.Columns.Clear();
            dtg_TabletZoneConfig.Columns.Add(new DataGridViewColumn() { Name = "DGV_TabletZone_Id", HeaderText = "Index", Width = 70, CellTemplate = currentCellTemplate });
            dtg_TabletZoneConfig.Columns.Add(new DataGridViewColumn() { Name = "DGV_TabletZone_ZoneName", HeaderText = "ZoneName", Width = 220, CellTemplate = currentCellTemplate });
            dtg_TabletZoneConfig.Columns.Add(new DataGridViewColumn() { Name = "DGV_TabletZone_CreateTime", HeaderText = "CreateTime", Width = 220, CellTemplate = currentCellTemplate });
            dtg_TabletZoneConfig.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_TabletZoneConfig.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_TabletZoneConfig.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
            dtg_TabletZoneConfig.Rows.Clear();
        }
        private void TabletZoneConfigDisPlay()
        {
            dtg_TabletZoneConfig.Columns["DGV_TabletZone_Id"].ReadOnly = true; //편집사용 불가 . 클릭시 팝업창에서 편집
            dtg_TabletZoneConfig.Columns["DGV_TabletZone_CreateTime"].ReadOnly = true; //편집사용 불가 . 클릭시 팝업창에서 편집

            int firstRowIndex = dtg_TabletZoneConfig.FirstDisplayedScrollingColumnIndex;
            dtg_TabletZoneConfig.CurrentCell = null;
            dtg_TabletZoneConfig.ClearSelection();
            dtg_TabletZoneConfig.Rows.Clear();
            dtg_TabletZoneConfig.RowTemplate.Height = 50;

            foreach (var tabletZoneCfg in uow.TabletZoneConfig.GetAll())
            {
                int newRowIndex = dtg_TabletZoneConfig.Rows.Add();
                var newRow = dtg_TabletZoneConfig.Rows[newRowIndex];
                newRow.Cells["DGV_TabletZone_Id"].Value = tabletZoneCfg.SEQ;
                newRow.Cells["DGV_TabletZone_ZoneName"].Value = tabletZoneCfg.ZONENAME;
                newRow.Cells["DGV_TabletZone_CreateTime"].Value = tabletZoneCfg.REGDATE;

                newRow.Tag = tabletZoneCfg;
                newRow.Cells["DGV_TabletZone_Id"].Style.BackColor = Color.WhiteSmoke;
                newRow.Cells["DGV_TabletZone_CreateTime"].Style.BackColor = Color.WhiteSmoke;
            }
            if (firstRowIndex >= 0 && firstRowIndex < dtg_TabletZoneConfig.Rows.Count)
            {
                dtg_TabletZoneConfig.FirstDisplayedScrollingRowIndex = firstRowIndex;
            }
            dtg_TabletZoneConfig.ClearSelection();
        }
        private void TabletZonePalletConfigInit()
        {
            dtg_TabletZonePalletConfig.ScrollBars = ScrollBars.Both;
            dtg_TabletZonePalletConfig.AllowUserToResizeColumns = true;
            dtg_TabletZonePalletConfig.ColumnHeadersHeight = 70;
            dtg_TabletZonePalletConfig.RowTemplate.Height = 70;
            dtg_TabletZonePalletConfig.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_TabletZonePalletConfig.Columns[0].CellTemplate;
            dtg_TabletZonePalletConfig.Columns.Clear();
            dtg_TabletZonePalletConfig.Columns.Add(new DataGridViewColumn() { Name = "DGV_dtg_TabletZonePallet_Id", HeaderText = "Index", Width = 70, CellTemplate = currentCellTemplate });
            dtg_TabletZonePalletConfig.Columns.Add(new DataGridViewColumn() { Name = "DGV_dtg_TabletZonePallet_ZoneName", HeaderText = "ZoneName", Width = 150, CellTemplate = currentCellTemplate });
            dtg_TabletZonePalletConfig.Columns.Add(new DataGridViewColumn() { Name = "DGV_dtg_TabletZonePallet_PalletNo", HeaderText = "PalletNo", Width = 140, CellTemplate = currentCellTemplate });
            dtg_TabletZonePalletConfig.Columns.Add(new DataGridViewColumn() { Name = "DGV_dtg_TabletZonePallet_CreateTime", HeaderText = "CreateTime", Width = 150, CellTemplate = currentCellTemplate });
            dtg_TabletZonePalletConfig.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_TabletZonePalletConfig.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_TabletZonePalletConfig.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
            dtg_TabletZonePalletConfig.Rows.Clear();
        }
        private void TabletZonePalletConfigDisPaly()
        {
            dtg_TabletZonePalletConfig.Columns["DGV_dtg_TabletZonePallet_Id"].ReadOnly = true; //편집사용 불가 . 클릭시 팝업창에서 편집
            dtg_TabletZonePalletConfig.Columns["DGV_dtg_TabletZonePallet_CreateTime"].ReadOnly = true; //편집사용 불가 . 클릭시 팝업창에서 편집

            int firstRowIndex = dtg_TabletZonePalletConfig.FirstDisplayedScrollingColumnIndex;
            dtg_TabletZonePalletConfig.CurrentCell = null;
            dtg_TabletZonePalletConfig.ClearSelection();
            dtg_TabletZonePalletConfig.Rows.Clear();
            dtg_TabletZonePalletConfig.RowTemplate.Height = 50;

            foreach (var tabletZonePalletCfg in uow.TabletZonePalletConfig.GetAll())
            {
                int newRowIndex = dtg_TabletZonePalletConfig.Rows.Add();
                var newRow = dtg_TabletZonePalletConfig.Rows[newRowIndex];
                newRow.Cells["DGV_dtg_TabletZonePallet_Id"].Value = tabletZonePalletCfg.SEQ;
                newRow.Cells["DGV_dtg_TabletZonePallet_ZoneName"].Value = tabletZonePalletCfg.ZONENAME;
                newRow.Cells["DGV_dtg_TabletZonePallet_PalletNo"].Value = tabletZonePalletCfg.PALLETNO;
                newRow.Cells["DGV_dtg_TabletZonePallet_CreateTime"].Value = tabletZonePalletCfg.REGDATE;

                newRow.Tag = tabletZonePalletCfg;
                newRow.Cells["DGV_dtg_TabletZonePallet_Id"].Style.BackColor = Color.WhiteSmoke;
                newRow.Cells["DGV_dtg_TabletZonePallet_CreateTime"].Style.BackColor = Color.WhiteSmoke;
            }
            if (firstRowIndex >= 0 && firstRowIndex < dtg_TabletZonePalletConfig.Rows.Count)
            {
                dtg_TabletZonePalletConfig.FirstDisplayedScrollingRowIndex = firstRowIndex;
            }
            dtg_TabletZonePalletConfig.ClearSelection();
        }

        private void dtg_TabletIpAddressConfig_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

                DataGridView grid = (DataGridView)sender;
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

                TabletIpAddressModel selectedRowTag = (TabletIpAddressModel)selectedRow.Tag;
                TabletIpAddressModel targetConfig = uow.TabletIpAddressConfig.Find(m => m.Seq == selectedRowTag.Seq).FirstOrDefault();

                string ChangeDatamsg = null;

                //Convert.ToString 사용시 변수가 null인경우 빈문자열을 반환함 
                //.Tostring() 사용시 변수가 null인경우 에러발생
                if (!string.IsNullOrEmpty(Convert.ToString(selectedCell.Value)) && targetConfig != null)
                {
                    string newValue = String.Concat(selectedCell.Value.ToString().Where(c => !char.IsWhiteSpace(c)));    //문자열 빈칸없애기

                    if (e.ColumnIndex == grid.Columns["DGV_TabletIpAddress_IpAddress"].Index)
                    {

                        bool flag = System.Net.IPAddress.TryParse(newValue, out var newIp); // check ip address
                        string newIpAddress = flag ? newIp.ToString() : targetConfig.IP;


                        if (newValue != "0.0.0.0")
                        {
                            if (targetConfig.IP != newValue)
                            {
                                string oldIpAddress = targetConfig.IP;
                                targetConfig.IP = newValue;
                                ChangeDatamsg = $"TabletIpAddressConfig, TabletIpAddress Config{targetConfig.Seq} TabletIpAddress IpAddress Change from {oldIpAddress} to {newIpAddress}";
                                ChageData(newValue, ChangeDatamsg);
                            }
                        }
                        else
                        {
                            MessageBox.Show("다시확인후 입력해주시기 바랍니다.");
                            TabletIpAddressConfigDisPlay();
                        }

                    }
                }

                else
                {
                    MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                    TabletIpAddressConfigDisPlay();
                }
                void ChageData(string newValue, string changeDatamsg)
                {
                    if (changeDatamsg != null)
                    {
                        selectedCell.Value = newValue;
                        selectedRowTag = targetConfig;
                        uow.TabletIpAddressConfig.Update(targetConfig);
                        string[] ChangeDatamsgSplit = changeDatamsg.Split(',');
                        mainForm.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                        MessageBox.Show("저장되었습니다");
                        TabletIpAddressConfigDisPlay();
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

        private void dtg_TabletIpAddressConfig_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

            DataGridView grid = (DataGridView)sender;
            DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
            DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

            TabletIpAddressModel selectedRowTag = (TabletIpAddressModel)selectedRow.Tag;
            TabletIpAddressModel targetConfig = uow.TabletIpAddressConfig.Find(m => m.Seq == selectedRowTag.Seq).FirstOrDefault();

            string ChangeDatamsg = null;

            if (targetConfig != null)
            {
                if (e.ColumnIndex == grid.Columns["DGV_TabletIpAddress_ZoneName"].Index)
                {
                    var tabletZoneSelectForm = new TabletZoneSelectForm(mainForm, uow);
                    if (tabletZoneSelectForm.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = tabletZoneSelectForm.InputValue.Trim();

                        if (targetConfig.ZONENAME != newValue)
                        {
                            string oldValue = targetConfig.ZONENAME;
                            targetConfig.ZONENAME = newValue;
                            tabletZoneSelectForm.Close();
                            ChangeDatamsg = $"TabletIpAddressConfig, TabletIpAddress Config{targetConfig.Seq} TabletIpAddress ZoneName Change from {oldValue} to {newValue}";

                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                }


            }
            else
            {
                MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                TabletIpAddressConfigDisPlay();
            }
            void ChageData(string newValue, string changeDatamsg)
            {
                if (changeDatamsg != null)
                {
                    selectedCell.Value = newValue;
                    selectedRowTag = targetConfig;
                    uow.TabletIpAddressConfig.Update(targetConfig);
                    string[] ChangeDatamsgSplit = ChangeDatamsg.Split(',');
                    mainForm.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                    TabletIpAddressConfigDisPlay();
                }
            }
        }

        private void dtg_TabletZoneConfig_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

                DataGridView grid = (DataGridView)sender;
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

                TabletZoneModel selectedRowTag = (TabletZoneModel)selectedRow.Tag;
                TabletZoneModel targetConfig = uow.TabletZoneConfig.Find(m => m.SEQ == selectedRowTag.SEQ).FirstOrDefault();

                string ChangeDatamsg = null;
                if(targetConfig!=null)
                {
                    //Convert.ToString 사용시 변수가 null인경우 빈문자열을 반환함 
                    //.Tostring() 사용시 변수가 null인경우 에러발생
                    if (!string.IsNullOrEmpty(Convert.ToString(selectedCell.Value)) && targetConfig != null)
                    {
                        string newValue = String.Concat(selectedCell.Value.ToString().Where(c => !char.IsWhiteSpace(c)));    //문자열 빈칸없애기

                        if (e.ColumnIndex == grid.Columns["DGV_TabletZone_ZoneName"].Index)
                        {
                            var zoneNameFind = uow.TabletZoneConfig.Find(z => z.ZONENAME == newValue).FirstOrDefault();
                            if (zoneNameFind == null && targetConfig.ZONENAME != newValue)
                            {
                                string oldValue = targetConfig.ZONENAME;
                                targetConfig.ZONENAME = newValue;
                                targetConfig.REGDATE = DateTime.Now;
                                ChangeDatamsg = $"TabletZoneConfig, TabletZone Config{targetConfig.SEQ} TabletZone ZoneName Change from {oldValue} to {newValue}";
                                ChageData(newValue, ChangeDatamsg);

                                //Tablet Config 있던 기존ZoneName 설정되어있는 이름을 변경된 이름으로 전체를 변경한다

                                foreach (var tabletIpAddressChangeZoneName in uow.TabletIpAddressConfig.Find(n => n.ZONENAME == oldValue).ToList())
                                {
                                    tabletIpAddressChangeZoneName.ZONENAME = newValue;
                                    uow.TabletIpAddressConfig.Update(tabletIpAddressChangeZoneName);
                                }
                                foreach (var tabletZonePalletChangeZoneName in uow.TabletZonePalletConfig.Find(z => z.ZONENAME == oldValue).ToList())
                                {
                                    tabletZonePalletChangeZoneName.ZONENAME = newValue;
                                    tabletZonePalletChangeZoneName.REGDATE = DateTime.Now;
                                    uow.TabletZonePalletConfig.Update(tabletZonePalletChangeZoneName);
                                }
                                TabletIpAddressConfigDisPlay();
                                TabletZonePalletConfigDisPaly();
                            }
                            else
                            {
                                MessageBox.Show("다시확인후 입력해주시기 바랍니다.");
                                TabletZoneConfigDisPlay();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("다시확인후 입력해주시기 바랍니다.");
                        TabletZoneConfigDisPlay();
                    }
                }

                else
                {
                    MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                    TabletZoneConfigDisPlay();
                }
                void ChageData(string newValue, string changeDatamsg)
                {
                    if (changeDatamsg != null)
                    {
                        selectedCell.Value = newValue;
                        selectedRowTag = targetConfig;
                        uow.TabletZoneConfig.Update(targetConfig);
                        string[] ChangeDatamsgSplit = changeDatamsg.Split(',');
                        mainForm.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                        MessageBox.Show("저장되었습니다");
                        TabletZoneConfigDisPlay();
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

        private void dtg_TabletZonePalletConfig_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

            DataGridView grid = (DataGridView)sender;
            DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
            DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

            TabletZonePalletModel selectedRowTag = (TabletZonePalletModel)selectedRow.Tag;
            TabletZonePalletModel targetConfig = uow.TabletZonePalletConfig.Find(m => m.SEQ == selectedRowTag.SEQ).FirstOrDefault();

            string ChangeDatamsg = null;

            if (targetConfig != null)
            {

                if (e.ColumnIndex == grid.Columns["DGV_dtg_TabletZonePallet_ZoneName"].Index)
                {
                    var tabletZoneSelectForm = new TabletZoneSelectForm(mainForm, uow);
                    if (tabletZoneSelectForm.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = tabletZoneSelectForm.InputValue.Trim();

                        if (targetConfig.ZONENAME != newValue)
                        {
                            string oldValue = targetConfig.ZONENAME;
                            targetConfig.ZONENAME = newValue;
                            targetConfig.REGDATE = DateTime.Now;
                            tabletZoneSelectForm.Close();
                            ChangeDatamsg = $"TabletZonePalletConfig, TabletZonePallet Config{targetConfig.SEQ} TabletZonePallet ZoneName Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                }

                else if (e.ColumnIndex == grid.Columns["DGV_dtg_TabletZonePallet_PalletNo"].Index)
                {
                    var insertNum = new NumPadForm(dtg_TabletZonePalletConfig.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "dtg_TabletZonePalletConfig", "RT15");
                    if (insertNum.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insertNum.InputValue.Trim();
                        if (targetConfig.PALLETNO != Convert.ToInt32(newValue))
                        {
                            string oldValue = targetConfig.PALLETNO.ToString();
                            targetConfig.PALLETNO = Convert.ToInt32(newValue);
                            targetConfig.REGDATE = DateTime.Now;
                            insertNum.Close();
                            ChangeDatamsg = $"TabletZonePalletConfig, TabletZonePallet Config{targetConfig.SEQ} TabletZonePallet PalletNo Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                TabletZonePalletConfigDisPaly();
            }
            void ChageData(string newValue, string changeDatamsg)
            {
                if (changeDatamsg != null)
                {
                    selectedCell.Value = newValue;
                    selectedRowTag = targetConfig;
                    uow.TabletZonePalletConfig.Update(targetConfig);
                    string[] ChangeDatamsgSplit = ChangeDatamsg.Split(',');
                    mainForm.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                    TabletZonePalletConfigDisPaly();
                }
            }
        }

        private void btn_BackUpSaveFile_Click(object sender, EventArgs e)
        {
            DataGridView BackUpData = null;
            string ButtonName = ((Button)sender).Name;

            switch (ButtonName)
            {
                //case "btn_TabletIpAddressConfigBackup":
                //    if (dtg_TabletIpAddressConfig.Rows.Count == 0) return;
                //    BackUpData = dtg_TabletIpAddressConfig;
                //    break;

                //case "btn_TabletZoneConfigBackup":
                //    if (dtg_TabletZoneConfig.Rows.Count == 0) return;
                //    BackUpData = dtg_TabletZoneConfig;
                //    break;
                //case "btn_TabletZonePalletConfigBackup":
                //    if (dtg_TabletZonePalletConfig.Rows.Count == 0) return;
                //    BackUpData = dtg_TabletZonePalletConfig;
                //    break;

            }
            if (BackUpData != null)
            {
                mainForm.SaveAsDataGridviewToCSV(BackUpData);
                mainForm.UserLog("RobotControl Screen", $"{ButtonName} BackUp Click ");
            }
        }

        private void txt_MaxNum_Click(object sender, EventArgs e)
        {
            string textName = ((TextBox)sender).Name;
            NumPadForm insertNum = new NumPadForm(((TextBox)sender).Text, ((TextBox)sender).Name, "RT31");
            if (insertNum.ShowDialog() == DialogResult.OK)
            {
                string newValue = int.Parse(insertNum.InputValue).ToString();
                switch (textName)
                {
                    case "txt_TabletIpAddressConfigMaxNum":
                        if (((TextBox)sender).Text != newValue)
                        {
                            string oldValue = ((TextBox)sender).Text;
                            ((TextBox)sender).Text = newValue;
                            AppConfiguration.ConfigDataSetting("TabletIpAddressConfigMaxNum", newValue);
                            ConfigData.TabletIpAddressConfigMaxNum = Convert.ToInt32(AppConfiguration.GetAppConfig("TabletIpAddressConfigMaxNum"));
                            insertNum.Close();
                            uow.TabletIpAddressConfig.Validate_DB_Items();
                            TabletIpAddressConfigDisPlay();
                            mainForm.UserLog("TabletConfig Screen", $"TabletIpAddressConfigMaxNum changed from {oldValue} to {newValue}.");
                        }
                        break;
                    case "txt_TabletZoneConfigMaxNum":
                        if (((TextBox)sender).Text != newValue)
                        {
                            string oldValue = ((TextBox)sender).Text;
                            ((TextBox)sender).Text = newValue;
                            AppConfiguration.ConfigDataSetting("TabletZoneConfigMaxNum", newValue);
                            ConfigData.TabletZoneConfigMaxNum = Convert.ToInt32(AppConfiguration.GetAppConfig("TabletZoneConfigMaxNum"));
                            insertNum.Close();
                            uow.TabletZoneConfig.Validate_DB_Items();
                            TabletZoneConfigDisPlay();
                            mainForm.UserLog("TabletConfig Screen", $"TabletZoneConfigMaxNum changed from {oldValue} to {newValue}.");
                        }
                        break;
                    case "txt_TabletZonePalletConfigMaxNum":
                        if (((TextBox)sender).Text != newValue)
                        {
                            string oldValue = ((TextBox)sender).Text;
                            ((TextBox)sender).Text = newValue;
                            AppConfiguration.ConfigDataSetting("TabletZonePalletConfigMaxNum", newValue);
                            ConfigData.TabletZonePalletConfigMaxNum = Convert.ToInt32(AppConfiguration.GetAppConfig("TabletZonePalletConfigMaxNum"));
                            insertNum.Close();
                            uow.TabletZonePalletConfig.Validate_DB_Items();
                            TabletZonePalletConfigDisPaly();
                            mainForm.UserLog("TabletConfig Screen", $"TabletZonePalletConfigMaxNum changed from {oldValue} to {newValue}.");
                        }
                        break;
                }


            }
        }

    }
}
