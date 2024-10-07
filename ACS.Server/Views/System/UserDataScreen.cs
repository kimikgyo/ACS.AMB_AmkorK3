using INA_ACS_Server.Models;
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

namespace INA_ACS_Server
{
    public partial class UserDataScreen : Form
    {
        MainForm mainForm;
        IUnitOfWork uow;

        private readonly Font textFont1 = new Font("맑은 고딕", 12, FontStyle.Bold);

        public UserDataScreen(MainForm mainForm, IUnitOfWork uow)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.uow = uow;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.FormBorderStyle = FormBorderStyle.None; //윈도우(상단) 테두리 제거 source code
            dtg_UserNumber.AlternatingRowsDefaultCellStyle = null;
            dtg_UserEmailAddress.AlternatingRowsDefaultCellStyle = null;
            dtg_UserNumber.DoubleBuffered(true);
            dtg_UserEmailAddress.DoubleBuffered(true);

            Init();
            TextBoxInit();
            UserNumberInit();
            UserNumberDisplay();
            UserEmailAddressInit();
            UserEmailAddressDisplay();
        }

        private void Init()
        {
            this.BackColor = mainForm.skinColor;

            groupBox1.ForeColor = Color.White;
            label1.ForeColor = Color.White;
            btn_UserNumberBackUp.BackColor = mainForm.skinColor;
            btn_UserNumberBackUp.ForeColor = Color.White;
            dtg_UserNumber.ScrollBars = ScrollBars.Both;
            dtg_UserNumber.AllowUserToResizeColumns = true;
            dtg_UserNumber.ColumnHeadersHeight = 70;
            dtg_UserNumber.RowTemplate.Height = 82;
            dtg_UserNumber.ReadOnly = false;
            dtg_UserNumber.BackgroundColor = mainForm.skinColor;
            dtg_UserNumber.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dtg_UserNumber.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtg_UserNumber.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtg_UserNumber.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dtg_UserNumber.ColumnHeadersDefaultCellStyle.Font = textFont1;
            dtg_UserNumber.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(80, 89, 96);
            dtg_UserNumber.EnableHeadersVisualStyles = false;
            dtg_UserNumber.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            groupBox2.ForeColor = Color.White;
            label2.ForeColor = Color.White;
            btn_UserEmailAddressBackUp.BackColor = mainForm.skinColor;
            btn_UserEmailAddressBackUp.ForeColor = Color.White;
            txt_ErrorSolutionName1.BackColor = mainForm.skinColor;
            txt_ErrorSolutionName1.ForeColor = Color.White;
            txt_ErrorSolutionName1.BorderStyle = BorderStyle.None;
            txt_ErrorSolutionURL1.BackColor = mainForm.skinColor;
            txt_ErrorSolutionURL1.ForeColor = Color.White;
            txt_ErrorSolutionName2.BackColor = mainForm.skinColor;
            txt_ErrorSolutionName2.ForeColor = Color.White;
            txt_ErrorSolutionName2.BorderStyle = BorderStyle.None;
            txt_ErrorSolutionURL2.BackColor = mainForm.skinColor;
            txt_ErrorSolutionURL2.ForeColor = Color.White;
            dtg_UserEmailAddress.ScrollBars = ScrollBars.Both;
            dtg_UserEmailAddress.AllowUserToResizeColumns = true;
            dtg_UserEmailAddress.ColumnHeadersHeight = 70;
            dtg_UserEmailAddress.RowTemplate.Height = 82;
            dtg_UserEmailAddress.ReadOnly = false;
            dtg_UserEmailAddress.BackgroundColor = mainForm.skinColor;
            dtg_UserEmailAddress.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dtg_UserEmailAddress.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtg_UserEmailAddress.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtg_UserEmailAddress.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dtg_UserEmailAddress.ColumnHeadersDefaultCellStyle.Font = textFont1;
            dtg_UserEmailAddress.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(80, 89, 96);
            dtg_UserEmailAddress.EnableHeadersVisualStyles = false;
            dtg_UserEmailAddress.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void TextBoxInit()
        {
            txt_UserNumberMaxNum.Text = ConfigData.UserNumber_MaxNum.ToString();
            txt_UserEmailAddressMaxNum.Text = ConfigData.UserEmail_MaxNum.ToString();
            txt_ErrorSolutionName1.Text = ConfigData.ErrorSolutionName1;
            txt_ErrorSolutionName2.Text = ConfigData.ErrorSolutionName2;
            txt_ErrorSolutionURL1.Text = ConfigData.sErrorSolution_URL1;
            txt_ErrorSolutionURL2.Text = ConfigData.sErrorSolution_URL2;
        }

        private void UserNumberInit()
        {
            dtg_UserNumber.ScrollBars = ScrollBars.Both;
            dtg_UserNumber.AllowUserToResizeColumns = true;
            dtg_UserNumber.ColumnHeadersHeight = 40;
            dtg_UserNumber.RowTemplate.Height = 40;
            dtg_UserNumber.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_UserNumber.Columns[0].CellTemplate;
            dtg_UserNumber.Columns.Clear();
            dtg_UserNumber.Columns.Add(new DataGridViewColumn() { Name = "DGV_UserNumberList_Id", HeaderText = "Index", Width = 70, CellTemplate = currentCellTemplate });
            dtg_UserNumber.Columns.Add(new DataGridViewColumn() { Name = "DGV_UserNumberList_UserNumber", HeaderText = "UserNumber", Width = 150, CellTemplate = currentCellTemplate });
            dtg_UserNumber.Columns.Add(new DataGridViewColumn() { Name = "DGV_UserNumberList_UserName", HeaderText = "UserName", Width = 150, CellTemplate = currentCellTemplate });
            dtg_UserNumber.Columns.Add(new DataGridViewColumn() { Name = "DGV_UserNumberList_CallMissionAuthority", HeaderText = "CallMissionAuthority", Width = 200, CellTemplate = currentCellTemplate });
            dtg_UserNumber.Columns.Add(new DataGridViewColumn() { Name = "DGV_UserNumberList_ElevatorAuthority", HeaderText = "ElevatorAuthority", Width = 200, CellTemplate = currentCellTemplate });

            dtg_UserNumber.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_UserNumber.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_UserNumber.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
            dtg_UserNumber.Rows.Clear();
        }

        private void UserNumberDisplay()
        {
            dtg_UserNumber.Columns["DGV_UserNumberList_Id"].ReadOnly = true;

            int firstRowIndex = dtg_UserNumber.FirstDisplayedScrollingColumnIndex;
            dtg_UserNumber.Rows.Clear();
            dtg_UserNumber.RowTemplate.Height = 40;
            foreach (var UserNumber in uow.UserNumber.GetAll())
            {
                int newRowIndex = dtg_UserNumber.Rows.Add();
                var newRow = dtg_UserNumber.Rows[newRowIndex];
                newRow.Cells["DGV_UserNumberList_Id"].Value = UserNumber.Id.ToString();
                newRow.Cells["DGV_UserNumberList_UserNumber"].Value = UserNumber.UserNumber.ToString();
                newRow.Cells["DGV_UserNumberList_UserName"].Value = UserNumber.UserName.ToString();
                newRow.Cells["DGV_UserNumberList_CallMissionAuthority"].Value = UserNumber.CallMissionAuthority.ToString();
                newRow.Cells["DGV_UserNumberList_ElevatorAuthority"].Value = UserNumber.ElevatorAuthority.ToString();
                newRow.Tag = UserNumber;
            }

            if (firstRowIndex >= 0 && firstRowIndex < dtg_UserNumber.Rows.Count)
            {
                dtg_UserNumber.FirstDisplayedScrollingRowIndex = firstRowIndex;
            }
            dtg_UserNumber.ClearSelection();
        }
        private void UserEmailAddressInit()
        {
            dtg_UserEmailAddress.ScrollBars = ScrollBars.Both;
            dtg_UserEmailAddress.AllowUserToResizeColumns = true;
            dtg_UserEmailAddress.ColumnHeadersHeight = 40;
            dtg_UserEmailAddress.RowTemplate.Height = 40;
            dtg_UserEmailAddress.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_UserEmailAddress.Columns[0].CellTemplate;
            dtg_UserEmailAddress.Columns.Clear();
            dtg_UserEmailAddress.Columns.Add(new DataGridViewColumn() { Name = "DGV_UserEmailAddress_Index", HeaderText = "Index", Width = 70, CellTemplate = currentCellTemplate });
            dtg_UserEmailAddress.Columns.Add(new DataGridViewColumn() { Name = "DGV_UserEmailAddress_Use", HeaderText = "Use", Width = 70, CellTemplate = currentCellTemplate });
            dtg_UserEmailAddress.Columns.Add(new DataGridViewColumn() { Name = "DGV_UserEmailAddress_UserEmailAddress", HeaderText = "EmailAddress", Width = 200, CellTemplate = currentCellTemplate });

            dtg_UserEmailAddress.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_UserEmailAddress.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_UserEmailAddress.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
            dtg_UserEmailAddress.Rows.Clear();
        }
        private void UserEmailAddressDisplay()
        {
            dtg_UserEmailAddress.Columns["DGV_UserEmailAddress_Index"].ReadOnly = true;
            dtg_UserEmailAddress.Columns["DGV_UserEmailAddress_Use"].ReadOnly = true;

            int firstRowIndex = dtg_UserEmailAddress.FirstDisplayedScrollingColumnIndex;
            dtg_UserEmailAddress.Rows.Clear();
            dtg_UserEmailAddress.RowTemplate.Height = 40;
            foreach (var UserEmailAddressConfig in uow.UserEmailAddress.GetAll())
            {
                int newRowIndex = dtg_UserEmailAddress.Rows.Add();
                var newRow = dtg_UserEmailAddress.Rows[newRowIndex];
                newRow.Cells["DGV_UserEmailAddress_Index"].Value = UserEmailAddressConfig.Id;
                newRow.Cells["DGV_UserEmailAddress_Use"].Value = UserEmailAddressConfig.EmailUse;
                newRow.Cells["DGV_UserEmailAddress_UserEmailAddress"].Value = UserEmailAddressConfig.UserEmailAddress;
                newRow.Tag = UserEmailAddressConfig;

                if (UserEmailAddressConfig.EmailUse == "Unuse") dtg_UserEmailAddress.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                else dtg_UserEmailAddress.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.White;
            }

            if (firstRowIndex >= 0 && firstRowIndex < dtg_UserEmailAddress.Rows.Count)
            {
                dtg_UserEmailAddress.FirstDisplayedScrollingRowIndex = firstRowIndex;
            }
            dtg_UserEmailAddress.ClearSelection();
        }

        private void txt_UserNumberMaxNum_Click(object sender, EventArgs e)
        {
            NumPadForm insertNum = new NumPadForm(((TextBox)sender).Text, ((TextBox)sender).Name, "RT10");
            if (insertNum.ShowDialog() == DialogResult.OK)
            {
                string newValue = int.Parse(insertNum.InputValue).ToString();

                if (((TextBox)sender).Text != newValue)
                {
                    string oldValue = ((TextBox)sender).Text;
                    ((TextBox)sender).Text = newValue;

                    AppConfiguration.ConfigDataSetting("UserNumber_MaxNum", ((TextBox)sender).Text);
                    ConfigData.UserNumber_MaxNum = int.Parse(AppConfiguration.GetAppConfig("UserNumber_MaxNum"));
                    insertNum.Close();
                    uow.UserNumber.Validate_DB_Items();
                    UserNumberDisplay();
                    mainForm.UserLog("UserNumber Screen", $"UserNumber_MaxNum Count Value Change from {oldValue} to {newValue}");
                }

            }
        }
        private void txt_UserEmailAddressMaxNum_Click(object sender, EventArgs e)
        {
            NumPadForm insertNum = new NumPadForm(((TextBox)sender).Text, ((TextBox)sender).Name, "RT10");
            if (insertNum.ShowDialog() == DialogResult.OK)
            {
                string newValue = int.Parse(insertNum.InputValue).ToString();

                if (((TextBox)sender).Text != int.Parse(insertNum.InputValue).ToString())
                {
                    string oldValue = ((TextBox)sender).Text;

                    ((TextBox)sender).Text = newValue;
                    AppConfiguration.ConfigDataSetting("UserEmail_MaxNum", newValue);
                    ConfigData.UserEmail_MaxNum = int.Parse(AppConfiguration.GetAppConfig("UserEmail_MaxNum"));
                    insertNum.Close();
                    uow.UserEmailAddress.Validate_DB_Items();
                    UserEmailAddressDisplay();
                    mainForm.UserLog("Position", $"UserEmailAddress_MaxNum changed from {oldValue} to {newValue}.");

                }
            }
        }
        private void dtg_UserEmailAddress_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

            DataGridView grid = (DataGridView)sender;
            DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
            DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

            UserEmailAddressModel selectedRowTag = (UserEmailAddressModel)selectedRow.Tag;
            UserEmailAddressModel targetConfig = uow.UserEmailAddress.Find(m => m.Id == selectedRowTag.Id).FirstOrDefault();

            string ChangeDatamsg = null;

            if (targetConfig != null)
            {
                if (e.ColumnIndex == grid.Columns["DGV_UserEmailAddress_Use"].Index)
                {
                    var insertUse = new UseSelectForm();
                    if (insertUse.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insertUse.InputValue.Trim();
                        if (targetConfig.EmailUse != newValue)
                        {
                            string oldValue = targetConfig.EmailUse;
                            targetConfig.EmailUse = newValue;

                            insertUse.Close();
                            ChangeDatamsg = $"Email,UserEmailAddress Config{targetConfig.Id} UserEmailAddress Use Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                            if (targetConfig.EmailUse == "Use") dtg_UserEmailAddress.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                            else dtg_UserEmailAddress.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                        }

                    }
                }
            }
            else
            {
                MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                UserEmailAddressDisplay();
            }

            void ChageData(string newValue, string changeDatamsg)
            {
                if (changeDatamsg != null)
                {
                    selectedCell.Value = newValue;
                    selectedRowTag = targetConfig;
                    uow.UserEmailAddress.Update(targetConfig);
                    string[] ChangeDatamsgSplit = changeDatamsg.Split(',');
                    mainForm.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                }
            }
        }

        private void dtg_UserEmailAddress_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

                DataGridView grid = (DataGridView)sender;
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

                UserEmailAddressModel selectedRowTag = (UserEmailAddressModel)selectedRow.Tag;
                UserEmailAddressModel targetConfig = uow.UserEmailAddress.Find(m => m.Id == selectedRowTag.Id).FirstOrDefault();

                string ChangeDatamsg = null;
                string oldUserNumber = targetConfig.UserEmailAddress.ToString();

                //Convert.ToString 사용시 변수가 null인경우 빈문자열을 반환함 
                //.Tostring() 사용시 변수가 null인경우 에러발생
                if (!string.IsNullOrEmpty(Convert.ToString(selectedCell.Value)) && targetConfig != null)
                {
                    string newValue = String.Concat(selectedCell.Value.ToString().Where(c => !char.IsWhiteSpace(c)));    //문자열 빈칸없애기

                    if (e.ColumnIndex == grid.Columns["DGV_UserEmailAddress_UserEmailAddress"].Index)
                    {
                        var UserEmailAddress = uow.UserEmailAddress.Find(m => m.UserEmailAddress.ToString() == newValue).FirstOrDefault();
                        if (UserEmailAddress == null)
                        {
                            if (targetConfig.UserEmailAddress.ToString() != newValue)
                            {
                                targetConfig.UserEmailAddress = newValue;
                                ChangeDatamsg = $"UserEmailAddress,UserEmailAddress {targetConfig.Id} UserEmailAddress UserEmailAddress changed from {oldUserNumber} to {newValue}.";
                                ChageData(newValue, ChangeDatamsg);
                            }
                        }
                        else
                        {
                            MessageBox.Show("같은 UserNumber가 있습니다." + "\r\n" + "다시확인후 입력해주시기 바랍니다.");
                            UserNumberDisplay();
                        }

                    }

                }
                else
                {
                    MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                    UserNumberDisplay();
                }

                void ChageData(string newValue, string changeDatamsg)
                {
                    if (changeDatamsg != null)
                    {
                        selectedCell.Value = newValue;
                        selectedRowTag = targetConfig;
                        uow.UserEmailAddress.Update(targetConfig);
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

        private void dtg_UserNumber_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

                DataGridView grid = (DataGridView)sender;
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

                UserNumberModel selectedRowTag = (UserNumberModel)selectedRow.Tag;
                UserNumberModel targetConfig = uow.UserNumber.Find(m => m.Id == selectedRowTag.Id).FirstOrDefault();

                string ChangeDatamsg = null;
                string oldUserNumber = targetConfig.UserNumber;

                //Convert.ToString 사용시 변수가 null인경우 빈문자열을 반환함 
                //.Tostring() 사용시 변수가 null인경우 에러발생
                if (!string.IsNullOrEmpty(Convert.ToString(selectedCell.Value)) && targetConfig != null)
                {
                    string newValue = String.Concat(selectedCell.Value.ToString().Where(c => !char.IsWhiteSpace(c)));    //문자열 빈칸없애기

                    if (e.ColumnIndex == grid.Columns["DGV_UserNumberList_UserNumber"].Index)
                    {
                        var UserNumber = uow.UserNumber.Find(m => m.UserNumber == newValue).FirstOrDefault();
                        if (UserNumber == null || UserNumber.UserNumber == "0")
                        {
                            if (targetConfig.UserNumber != newValue)
                            {
                                targetConfig.UserNumber = newValue;
                                ChangeDatamsg = $"UserNumber,UserNumber {targetConfig.Id} UserNumber UserNumber changed from {oldUserNumber} to {newValue}.";
                                ChageData(newValue, ChangeDatamsg);
                            }
                        }
                        else
                        {
                            MessageBox.Show("같은 UserNumber가 있습니다." + "\r\n" + "다시확인후 입력해주시기 바랍니다.");
                            UserNumberDisplay();
                        }
                    }
                    else if (e.ColumnIndex == grid.Columns["DGV_UserNumberList_UserName"].Index)
                    {
                        if (targetConfig.UserNumber != newValue)
                        {
                            targetConfig.UserName = newValue;
                            ChangeDatamsg = $"UserNumber,UserNumber {targetConfig.Id} UserNumber UserName changed from {oldUserNumber} to {newValue}.";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                    UserNumberDisplay();
                }

                void ChageData(string newValue, string changeDatamsg)
                {
                    if (changeDatamsg != null)
                    {
                        selectedCell.Value = newValue;
                        selectedRowTag = targetConfig;
                        uow.UserNumber.ContentUpdate(targetConfig);
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

        private void btn_BackUpSaveFile_Click(object sender, EventArgs e)
        {
            string ButtonName = ((Button)sender).Name;
            switch (ButtonName)
            {
                case "btn_UserNumberBackUp":
                    if (dtg_UserNumber.Rows.Count == 0) return;
                    mainForm.SaveAsDataGridviewToCSV(dtg_UserNumber);
                    mainForm.UserLog("UserNumber Screen", " BackUp Click ");
                    break;
                case "btn_UserEmailAddressBackUp":
                    if (dtg_UserEmailAddress.Rows.Count == 0) return;
                    mainForm.SaveAsDataGridviewToCSV(dtg_UserEmailAddress);
                    mainForm.UserLog("UserEamilAddress Screen", " BackUp Click ");
                    break;
            }
        }

        private void txt_ErrorSolutionURL1_Click(object sender, EventArgs e)
        {
            InputTextBoxForm insertText = new InputTextBoxForm(ConfigData.sErrorSolution_URL1);
            DialogResult result = insertText.ShowDialog();
            if (result == DialogResult.OK)
            {
                string newValue = insertText.InputValue.Trim();
                if (((TextBox)sender).Text != newValue)
                {
                    string oldValue = ((TextBox)sender).Text;
                    ((TextBox)sender).Text = newValue;
                    AppConfiguration.ConfigDataSetting("sErrorSolution_URL1", newValue);
                    ConfigData.sErrorSolution_URL1 = AppConfiguration.GetAppConfig("sErrorSolution_URL1");

                    insertText.Close();
                    mainForm.UserLog($"UserDataScreen", $"ErrorSolution1 URL Change from {oldValue} to {newValue}");
                }
            }
        }

        private void txt_ErrorSolutionURL2_Click(object sender, EventArgs e)
        {
            InputTextBoxForm insertText = new InputTextBoxForm(ConfigData.sErrorSolution_URL2);
            DialogResult result = insertText.ShowDialog();
            if (result == DialogResult.OK)
            {
                string newValue = insertText.InputValue.Trim();
                if (((TextBox)sender).Text != newValue)
                {
                    string oldValue = ((TextBox)sender).Text;
                    ((TextBox)sender).Text = newValue;
                    AppConfiguration.ConfigDataSetting("sErrorSolution_URL2", newValue);
                    ConfigData.sErrorSolution_URL2 = AppConfiguration.GetAppConfig("sErrorSolution_URL2");

                    insertText.Close();
                    mainForm.UserLog($"UserDataScreen", $"ErrorSolution2 URL Change from {oldValue} to {newValue}");
                }
            }
        }

        private void txt_ErrorSolutionName1_Click(object sender, EventArgs e)
        {
            InputTextBoxForm insertText = new InputTextBoxForm(ConfigData.ErrorSolutionName1);
            DialogResult result = insertText.ShowDialog();
            if (result == DialogResult.OK)
            {
                string newValue = insertText.InputValue.Trim();
                if (((TextBox)sender).Text != newValue)
                {
                    string oldValue = ((TextBox)sender).Text;
                    ((TextBox)sender).Text = newValue;
                    AppConfiguration.ConfigDataSetting("ErrorSolutionName1", newValue);
                    ConfigData.ErrorSolutionName1 = AppConfiguration.GetAppConfig("ErrorSolutionName1");

                    insertText.Close();
                    mainForm.UserLog($"UserDataScreen", $"ErrorSolutionName1 URL Change from {oldValue} to {newValue}");
                }
            }
        }

        private void txt_ErrorSolutionName2_Click(object sender, EventArgs e)
        {
            InputTextBoxForm insertText = new InputTextBoxForm(ConfigData.ErrorSolutionName2);
            DialogResult result = insertText.ShowDialog();
            if (result == DialogResult.OK)
            {
                string newValue = insertText.InputValue.Trim();
                if (((TextBox)sender).Text != newValue)
                {
                    string oldValue = ((TextBox)sender).Text;
                    ((TextBox)sender).Text = newValue;
                    AppConfiguration.ConfigDataSetting("ErrorSolutionName2", newValue);
                    ConfigData.ErrorSolutionName2 = AppConfiguration.GetAppConfig("ErrorSolutionName2");

                    insertText.Close();
                    mainForm.UserLog($"UserDataScreen", $"ErrorSolutionName2 URL Change from {oldValue} to {newValue}");
                }
            }
        }

        private void dtg_UserNumber_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

            DataGridView grid = (DataGridView)sender;
            DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
            DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

            UserNumberModel selectedRowTag = (UserNumberModel)selectedRow.Tag;
            UserNumberModel targetConfig = uow.UserNumber.Find(m => m.Id == selectedRowTag.Id).FirstOrDefault();

            string ChangeDatamsg = null;

            if (targetConfig != null)
            {
                if (e.ColumnIndex == grid.Columns["DGV_UserNumberList_CallMissionAuthority"].Index)
                {
                    var insertUse = new NumPadForm(dtg_UserNumber.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "dtg_CallMissionAuthority", "RT33");
                    if (insertUse.ShowDialog() == DialogResult.OK)
                    {
                        int newValue = Convert.ToInt32(insertUse.InputValue.Trim());
                        if (targetConfig.CallMissionAuthority != newValue)
                        {
                            int oldValue = targetConfig.CallMissionAuthority;
                            targetConfig.CallMissionAuthority = newValue;

                            insertUse.Close();
                            ChangeDatamsg = $"UserNumber Config{targetConfig.Id} CallMissionAuthority Use Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }

                    }
                }
                else if (e.ColumnIndex == grid.Columns["DGV_UserNumberList_ElevatorAuthority"].Index)
                {
                    var insertUse = new NumPadForm(dtg_UserNumber.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "dtg_ElevatorAuthority", "RT33");
                    if (insertUse.ShowDialog() == DialogResult.OK)
                    {
                        int newValue = Convert.ToInt32(insertUse.InputValue.Trim());
                        if (targetConfig.ElevatorAuthority != newValue)
                        {
                            int oldValue = targetConfig.ElevatorAuthority;
                            targetConfig.ElevatorAuthority = newValue;

                            insertUse.Close();
                            ChangeDatamsg = $"UserNumber Config{targetConfig.Id} ElevatorAuthority Use Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }

                    }
                }
            }
            else
            {
                MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                UserEmailAddressDisplay();
            }

            void ChageData(int newValue, string changeDatamsg)
            {
                if (changeDatamsg != null)
                {
                    selectedCell.Value = newValue;
                    selectedRowTag = targetConfig;
                    uow.UserNumber.ContentUpdate(targetConfig);
                    string[] ChangeDatamsgSplit = changeDatamsg.Split(',');
                    mainForm.UserLog("UserNumber", ChangeDatamsgSplit[0]);
                }
            }
        }
    }
}
