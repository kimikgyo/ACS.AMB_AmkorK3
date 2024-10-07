using INA_ACS_Server.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INA_ACS_Server
{
    public partial class RegisterScreen : Form
    {


        private readonly MainForm mainForm;
        private readonly IUnitOfWork uow;

        public RegisterScreen(MainForm mainForm, IUnitOfWork uow)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.uow = uow;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.FormBorderStyle = FormBorderStyle.None; //윈도우(상단) 테두리 제거 source code
            dtg_RegisterDisPlay.AlternatingRowsDefaultCellStyle = null;
            dtg_RegisterInfo.AlternatingRowsDefaultCellStyle = null;
            dtg_RegisterDisPlay.DoubleBuffered(true);
            dtg_RegisterInfo.DoubleBuffered(true);

            RegisterDisPlayInit();
            RegisterDisPlay();
            RegisterInfoDisPlayInit();
            RegisterInfoDisPlay();
        }
        private void RegisterDisPlayInit()
        {
            try
            {
                dtg_RegisterDisPlay.ScrollBars = ScrollBars.Both;
                dtg_RegisterDisPlay.AllowUserToResizeColumns = true;
                dtg_RegisterDisPlay.ColumnHeadersHeight = 80;
                dtg_RegisterDisPlay.RowTemplate.Height = 40;
                dtg_RegisterDisPlay.ReadOnly = false;

                DataGridViewCell currentCellTemplate = dtg_RegisterDisPlay.Columns[0].CellTemplate;
                dtg_RegisterDisPlay.Columns.Clear();
                dtg_RegisterDisPlay.Columns.Add(new DataGridViewColumn() { Name = "DGV_RegisterDisPlay_RobotName", HeaderText = "RobotName", Width = 150, CellTemplate = currentCellTemplate });
                dtg_RegisterDisPlay.Columns.Add(new DataGridViewColumn() { Name = "DGV_RegisterDisPlay_Register20", HeaderText = "Register20Value", Width = 100, CellTemplate = currentCellTemplate });
                dtg_RegisterDisPlay.Columns.Add(new DataGridViewColumn() { Name = "DGV_RegisterDisPlay_Register21", HeaderText = "Register21Value", Width = 100, CellTemplate = currentCellTemplate });
                dtg_RegisterDisPlay.Columns.Add(new DataGridViewColumn() { Name = "DGV_RegisterDisPlay_Register22", HeaderText = "Register22Value", Width = 100, CellTemplate = currentCellTemplate });
                dtg_RegisterDisPlay.Columns.Add(new DataGridViewColumn() { Name = "DGV_RegisterDisPlay_Register23", HeaderText = "Register23Value", Width = 100, CellTemplate = currentCellTemplate });
                dtg_RegisterDisPlay.Columns.Add(new DataGridViewColumn() { Name = "DGV_RegisterDisPlay_Register24", HeaderText = "Register24Value", Width = 100, CellTemplate = currentCellTemplate });
                dtg_RegisterDisPlay.Columns.Add(new DataGridViewColumn() { Name = "DGV_RegisterDisPlay_Register25", HeaderText = "Register25Value", Width = 100, CellTemplate = currentCellTemplate });
                dtg_RegisterDisPlay.Columns.Add(new DataGridViewColumn() { Name = "DGV_RegisterDisPlay_Register26", HeaderText = "Register26Value", Width = 100, CellTemplate = currentCellTemplate });
                dtg_RegisterDisPlay.Columns.Add(new DataGridViewColumn() { Name = "DGV_RegisterDisPlay_Register30", HeaderText = "Register30Value", Width = 100, CellTemplate = currentCellTemplate });
                dtg_RegisterDisPlay.Columns.Add(new DataGridViewColumn() { Name = "DGV_RegisterDisPlay_Register31", HeaderText = "Register31Value", Width = 100, CellTemplate = currentCellTemplate });
                dtg_RegisterDisPlay.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
                dtg_RegisterDisPlay.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
                dtg_RegisterDisPlay.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
                dtg_RegisterDisPlay.Columns["DGV_RegisterDisPlay_Register20"].HeaderText = "Registar 20" + "\r\n" + "Value";
                dtg_RegisterDisPlay.Columns["DGV_RegisterDisPlay_Register21"].HeaderText = "Registar 21" + "\r\n" + "Value";
                dtg_RegisterDisPlay.Columns["DGV_RegisterDisPlay_Register22"].HeaderText = "Registar 22" + "\r\n" + "Value";
                dtg_RegisterDisPlay.Columns["DGV_RegisterDisPlay_Register23"].HeaderText = "Registar 23" + "\r\n" + "Value";
                dtg_RegisterDisPlay.Columns["DGV_RegisterDisPlay_Register24"].HeaderText = "Registar 24" + "\r\n" + "Value";
                dtg_RegisterDisPlay.Columns["DGV_RegisterDisPlay_Register25"].HeaderText = "Registar 25" + "\r\n" + "Value";
                dtg_RegisterDisPlay.Columns["DGV_RegisterDisPlay_Register26"].HeaderText = "Registar 26" + "\r\n" + "Value";
                dtg_RegisterDisPlay.Columns["DGV_RegisterDisPlay_Register30"].HeaderText = "Registar 30" + "\r\n" + "Value";
                dtg_RegisterDisPlay.Columns["DGV_RegisterDisPlay_Register31"].HeaderText = "Registar 31" + "\r\n" + "Value";


                dtg_RegisterDisPlay.Rows.Clear();
            }
            catch (Exception ex)
            {
                mainForm.EventLog("Register Screen", "RegisterDisPlayInit() Fail = " + ex);
                mainForm.ACS_UI_Log("Register Screen" + "/" + "RegisterDisPlayInit() Fail = " + ex);
            }
        }
        private void RegisterDisPlay()
        {
            try
            {

                dtg_RegisterDisPlay.Columns["DGV_RegisterDisPlay_RobotName"].ReadOnly = true; //편집사용 불가 . 클릭시 팝업창에서 편집
                dtg_RegisterDisPlay.Columns["DGV_RegisterDisPlay_Register20"].ReadOnly = true; //편집사용 불가 . 클릭시 팝업창에서 편집
                dtg_RegisterDisPlay.Columns["DGV_RegisterDisPlay_Register21"].ReadOnly = true; //편집사용 불가 . 클릭시 팝업창에서 편집
                dtg_RegisterDisPlay.Columns["DGV_RegisterDisPlay_Register22"].ReadOnly = true; //편집사용 불가 . 클릭시 팝업창에서 편집
                dtg_RegisterDisPlay.Columns["DGV_RegisterDisPlay_Register23"].ReadOnly = true; //편집사용 불가 . 클릭시 팝업창에서 편집
                dtg_RegisterDisPlay.Columns["DGV_RegisterDisPlay_Register24"].ReadOnly = true; //편집사용 불가 . 클릭시 팝업창에서 편집
                dtg_RegisterDisPlay.Columns["DGV_RegisterDisPlay_Register25"].ReadOnly = true; //편집사용 불가 . 클릭시 팝업창에서 편집
                dtg_RegisterDisPlay.Columns["DGV_RegisterDisPlay_Register26"].ReadOnly = true; //편집사용 불가 . 클릭시 팝업창에서 편집
                dtg_RegisterDisPlay.Columns["DGV_RegisterDisPlay_Register30"].ReadOnly = true; //편집사용 불가 . 클릭시 팝업창에서 편집
                dtg_RegisterDisPlay.Columns["DGV_RegisterDisPlay_Register31"].ReadOnly = true; //편집사용 불가 . 클릭시 팝업창에서 편집

                dtg_RegisterDisPlay.CurrentCell = null;
                dtg_RegisterDisPlay.ClearSelection();
                dtg_RegisterDisPlay.Rows.Clear();
                dtg_RegisterDisPlay.RowTemplate.Height = 50;

                foreach (var robot in uow.Robots.GetAll())
                {
                    int newRowIndex = dtg_RegisterDisPlay.Rows.Add();
                    var newRow = dtg_RegisterDisPlay.Rows[newRowIndex];

                    newRow.Cells["DGV_RegisterDisPlay_RobotName"].Value = robot.RobotName;
                    newRow.Cells["DGV_RegisterDisPlay_Register20"].Value = robot.Registers.dMiR_Register_Value20.ToString();
                    newRow.Cells["DGV_RegisterDisPlay_Register21"].Value = robot.Registers.dMiR_Register_Value21.ToString();
                    newRow.Cells["DGV_RegisterDisPlay_Register22"].Value = robot.Registers.dMiR_Register_Value22.ToString();
                    newRow.Cells["DGV_RegisterDisPlay_Register23"].Value = robot.Registers.dMiR_Register_Value23.ToString();
                    newRow.Cells["DGV_RegisterDisPlay_Register24"].Value = robot.Registers.dMiR_Register_Value24.ToString();
                    newRow.Cells["DGV_RegisterDisPlay_Register25"].Value = robot.Registers.dMiR_Register_Value25.ToString();
                    newRow.Cells["DGV_RegisterDisPlay_Register26"].Value = robot.Registers.dMiR_Register_Value26.ToString();
                    newRow.Cells["DGV_RegisterDisPlay_Register30"].Value = robot.Registers.dMiR_Register_Value30.ToString();
                    newRow.Cells["DGV_RegisterDisPlay_Register31"].Value = robot.Registers.dMiR_Register_Value31.ToString();
                    newRow.Tag = robot;
                }
            }
            catch (Exception ex)
            {
                mainForm.EventLog("Register Screen", "RegisterStatusDisPlay() Fail = " + ex);
                mainForm.ACS_UI_Log("Register Screen" + "/" + "RegisterStatusDisPlay() Fail = " + ex);
            }
        }

        private void DisPlayTimer_Tick(object sender, EventArgs e)
        {
            DisPlayTimer.Enabled = false;
            RegisterDisPlay();
            DisPlayTimer.Interval = 1000; //타이머 인터벌 1초
            DisPlayTimer.Enabled = true;

        }

        private void txt_RegisterInfo_MaxNum_Click(object sender, EventArgs e)
        {
            NumPadForm insertNum = new NumPadForm(((TextBox)sender).Text, ((TextBox)sender).Name, ((TextBox)sender).AccessibleDescription);
            DialogResult result = insertNum.ShowDialog();
            if (result == DialogResult.OK)
            {
                {
                    mainForm.UserLog("Register Screen", "RegisterInfo_MaxNum Count Value Change " + ((TextBox)sender).Text + " -> " + int.Parse(insertNum.InputValue).ToString());

                    ((TextBox)sender).Text = int.Parse(insertNum.InputValue).ToString();
                    AppConfiguration.ConfigDataSetting("RegisterInfo_MaxNum", ((TextBox)sender).Text);
                    ConfigData.RegisterInfo_MaxNum = int.Parse(AppConfiguration.GetAppConfig("RegisterInfo_MaxNum"));
                    insertNum.Close();

                    RegisterInfoDisPlay();
                }
            }
        }

        private void RegisterInfoDisPlayInit()
        {
            try
            {

                dtg_RegisterInfo.ScrollBars = ScrollBars.Both;
                dtg_RegisterInfo.AllowUserToResizeColumns = true;
                dtg_RegisterInfo.ColumnHeadersHeight = 80;
                dtg_RegisterInfo.RowTemplate.Height = 40;
                dtg_RegisterInfo.ReadOnly = false;

                DataGridViewCell currentCellTemplate = dtg_RegisterInfo.Columns[0].CellTemplate;
                dtg_RegisterInfo.Columns.Clear();
                dtg_RegisterInfo.Columns.Add(new DataGridViewColumn() { Name = "DGV_RegisterInfo_ACSRobotGroup", HeaderText = "ACSRobotGroup", Width = 150, CellTemplate = currentCellTemplate });
                dtg_RegisterInfo.Columns.Add(new DataGridViewColumn() { Name = "DGV_RegisterInfo_RegisterNumber", HeaderText = "RegisterNumber", Width = 100, CellTemplate = currentCellTemplate });
                dtg_RegisterInfo.Columns.Add(new DataGridViewColumn() { Name = "DGV_RegisterInfo_RegisterValue", HeaderText = "RegisterValue", Width = 100, CellTemplate = currentCellTemplate });
                dtg_RegisterInfo.Columns.Add(new DataGridViewColumn() { Name = "DGV_RegisterInfo_RegisterInfoMessge", HeaderText = "RegisterInfoMessge", Width = 200, CellTemplate = currentCellTemplate });
                dtg_RegisterInfo.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
                dtg_RegisterInfo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
                dtg_RegisterInfo.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
                dtg_RegisterInfo.Columns["DGV_RegisterInfo_RegisterNumber"].HeaderText = "Register" + "\r\n" + "Number";
                dtg_RegisterInfo.Columns["DGV_RegisterInfo_RegisterValue"].HeaderText = "Register" + "\r\n" + "Value";
                dtg_RegisterInfo.Columns["DGV_RegisterInfo_RegisterInfoMessge"].HeaderText = "Register" + "\r\n" + "InfoMessge";


                dtg_RegisterInfo.Rows.Clear();
            }
            catch (Exception ex)
            {
                mainForm.EventLog("Register Screen", "RegisterInfoDisPlayInit() Fail = " + ex);
                mainForm.ACS_UI_Log("Register Screen" + "/" + "RegisterInfoDisPlayInit() Fail = " + ex);
            }

        }

        private void RegisterInfoDisPlay()
        {
            try
            {
                txt_RegisterInfo_MaxNum.Text = ConfigData.RegisterInfo_MaxNum.ToString();
                uow.RegisterInfo.Validate_DB_Items();

                //dtg_RegisterInfo.Columns["DGV_RegisterInfo_ACSRobotGroup"].ReadOnly = true; //편집사용 불가 . 클릭시 팝업창에서 편집

                dtg_RegisterInfo.CurrentCell = null;
                dtg_RegisterInfo.ClearSelection();
                dtg_RegisterInfo.Rows.Clear();
                dtg_RegisterInfo.RowTemplate.Height = 50;

                foreach (var registerInfo in uow.RegisterInfo.GetDisplayFlagtrueData())
                {
                    int newRowIndex = dtg_RegisterInfo.Rows.Add();
                    var newRow = dtg_RegisterInfo.Rows[newRowIndex];

                    newRow.Cells["DGV_RegisterInfo_ACSRobotGroup"].Value = registerInfo.ACSRobotGroup;
                    newRow.Cells["DGV_RegisterInfo_RegisterNumber"].Value = registerInfo.RegisterNumber;
                    newRow.Cells["DGV_RegisterInfo_RegisterValue"].Value = registerInfo.RegisterValue;
                    newRow.Cells["DGV_RegisterInfo_RegisterInfoMessge"].Value = registerInfo.RegisterInfoMessge;
                    newRow.Tag = registerInfo;
                }
            }
            catch (Exception ex)
            {
                mainForm.EventLog("Register Screen", "RegisterInfoDisPlay() Fail = " + ex);
                mainForm.ACS_UI_Log("Register Screen" + "/" + "RegisterInfoDisPlay() Fail = " + ex);
            }
        }

        private void dtg_RegisterInfo_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

            DataGridView grid = (DataGridView)sender;
            DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
            DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

            RegisterInfoModel targetConfig = (RegisterInfoModel)selectedRow.Tag;

            if (selectedCell.Value != null)
            {
                string newValue = String.Concat(selectedCell.Value.ToString().Where(c => !char.IsWhiteSpace(c)));

                if (!string.IsNullOrEmpty(newValue))
                {
                    if (e.ColumnIndex == grid.Columns["DGV_RegisterInfo_ACSRobotGroup"].Index)
                    {
                        string oldValue = selectedCell.Value.ToString().Trim();
                        selectedCell.Value = newValue;
                        targetConfig.ACSRobotGroup = newValue;
                        uow.RegisterInfo.Update(targetConfig);
                        MessageBox.Show("저장되었습니다");
                        mainForm.UserLog("Register Screen", $"RegisterInfo {targetConfig.Id} RegisterInfo ACSRobotGroup Change from {oldValue} to {newValue}");
                    }

                    else if (e.ColumnIndex == grid.Columns["DGV_RegisterInfo_RegisterNumber"].Index)
                    {
                        string oldValue = selectedCell.Value.ToString().Trim();
                        selectedCell.Value = newValue;
                        targetConfig.RegisterNumber = Convert.ToInt32(newValue);
                        uow.RegisterInfo.Update(targetConfig);
                        MessageBox.Show("저장되었습니다");
                        mainForm.UserLog("Register Screen", $"RegisterInfo {targetConfig.Id} RegisterInfo RegisterNumber Change from {oldValue} to {newValue}");
                    }

                    else if (e.ColumnIndex == grid.Columns["DGV_RegisterInfo_RegisterValue"].Index)
                    {
                        string oldValue = selectedCell.Value.ToString().Trim();
                        selectedCell.Value = newValue;
                        targetConfig.RegisterValue = Convert.ToInt32(newValue);
                        uow.RegisterInfo.Update(targetConfig);
                        MessageBox.Show("저장되었습니다");
                        mainForm.UserLog("Register Screen", $"RegisterInfo {targetConfig.Id} RegisterInfo RegisterValue Change from {oldValue} to {newValue}");
                    }
                    else if (e.ColumnIndex == grid.Columns["DGV_RegisterInfo_RegisterInfoMessge"].Index)
                    {
                        string oldValue = selectedCell.Value.ToString().Trim();
                        selectedCell.Value = newValue;
                        targetConfig.RegisterInfoMessge = newValue;
                        uow.RegisterInfo.Update(targetConfig);
                        MessageBox.Show("저장되었습니다");
                        mainForm.UserLog("Register Screen", $"RegisterInfo {targetConfig.Id} RegisterInfo Messge Change from {oldValue} to {newValue}");
                    }
                }
            }
            else
            {
                MessageBox.Show("문자를 입력해 주세요!!");
            }

        }

        //private void dtg_RegisterInfo_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

        //    DataGridView grid = (DataGridView)sender;
        //    DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
        //    DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

        //    RegisterInfoModle targetConfig = (RegisterInfoModle)selectedRow.Tag;

        //    if (e.ColumnIndex == grid.Columns["DGV_RegisterInfo_ACSRobotGroup"].Index)
        //    {

        //        var insertNum = new GroupSelectForm(mainForm);
        //        if (insertNum.ShowDialog() == DialogResult.OK)
        //        {
        //            string oldValue = selectedCell.Value.ToString().Trim();
        //            string newValue = insertNum.InputValue.Trim();

        //            selectedCell.Value = newValue;
        //            targetConfig.ACSRobotGroup = newValue;
        //            uow.RegisterInfo.Update(targetConfig);

        //            insertNum.Close();
        //            mainForm.UserLog("Register Screen", $"RegisterInfo {targetConfig.Id} RegisterInfo ACSRobotGroup Change from {oldValue} to {newValue}");

        //        }
        //    }
        //}
    }


}
