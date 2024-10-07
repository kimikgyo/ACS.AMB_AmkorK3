using INA_ACS_Server.UI;
using log4net;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Drawing;

namespace INA_ACS_Server
{

    public partial class PositionScreen : Form
    {
        private readonly static ILog EventLogger = LogManager.GetLogger("Event"); //Function 실행관련 Log
        private readonly Font textFont1 = new Font("맑은 고딕", 12, FontStyle.Bold);

        MainForm main;
        IUnitOfWork uow;

        public PositionScreen(MainForm main, IUnitOfWork uow)
        {
            InitializeComponent();
            this.main = main;
            this.uow = uow;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.FormBorderStyle = FormBorderStyle.None; //윈도우(상단) 테두리 제거 source code
            dtg_PositionArea.AlternatingRowsDefaultCellStyle = null;
            dtg_PositionArea.DoubleBuffered(true);

            Init();
            PositionTextBoxInit();
            PositionAreaInit();
            PositionArea_Display();

            dtg_PositionArea.CellValidating += Dtg_PositionArea_CellValidating;

        }

        private void Init()
        {
            this.BackColor = main.skinColor;

            groupBox1.ForeColor = Color.White;
            label1.ForeColor = Color.White;
            btn_POSBackUpSaveFile.BackColor = main.skinColor;
            btn_POSBackUpSaveFile.ForeColor = Color.White;
            btn_BackUpGetFile.BackColor = main.skinColor;
            btn_BackUpGetFile.ForeColor = Color.White;

            dtg_PositionArea.ScrollBars = ScrollBars.Both;
            dtg_PositionArea.AllowUserToResizeColumns = true;
            dtg_PositionArea.ColumnHeadersHeight = 70;
            dtg_PositionArea.RowTemplate.Height = 82;
            dtg_PositionArea.ReadOnly = false;
            dtg_PositionArea.BackgroundColor = main.skinColor;
            dtg_PositionArea.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dtg_PositionArea.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtg_PositionArea.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtg_PositionArea.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dtg_PositionArea.ColumnHeadersDefaultCellStyle.Font = textFont1;
            dtg_PositionArea.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(80, 89, 96);
            dtg_PositionArea.EnableHeadersVisualStyles = false;
            dtg_PositionArea.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }

        private void Dtg_PositionArea_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid == null) return;

            if (e.ColumnIndex == grid.Columns["DGV_PositionArea_X1"].Index || e.ColumnIndex == grid.Columns["DGV_PositionArea_Y1"].Index
             || e.ColumnIndex == grid.Columns["DGV_PositionArea_X2"].Index || e.ColumnIndex == grid.Columns["DGV_PositionArea_Y2"].Index
             || e.ColumnIndex == grid.Columns["DGV_PositionArea_X3"].Index || e.ColumnIndex == grid.Columns["DGV_PositionArea_Y3"].Index
             || e.ColumnIndex == grid.Columns["DGV_PositionArea_X4"].Index || e.ColumnIndex == grid.Columns["DGV_PositionArea_Y4"].Index)
            {
                if (!double.TryParse(e.FormattedValue.ToString(), out double output))
                {
                    MessageBox.Show("Please enter a numeric value.");
                    e.Cancel = true;
                }

                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    MessageBox.Show("Cell cannot be left blank.");
                    e.Cancel = true;
                }
            }
        }

        private void PositionTextBoxInit()
        {
            txt_PositionDataMaxNum.ReadOnly = true;
            txt_PositionDataMaxNum.BackColor = Color.White;
            txt_PositionDataMaxNum.Text = ConfigData.PosAreaData_MaxNum.ToString();
        }

        private void PositionAreaInit()
        {
            dtg_PositionArea.ScrollBars = ScrollBars.Both;
            dtg_PositionArea.AllowUserToResizeColumns = true;
            dtg_PositionArea.ColumnHeadersHeight = 40;
            dtg_PositionArea.RowTemplate.Height = 40;
            dtg_PositionArea.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_PositionArea.Columns[0].CellTemplate;
            dtg_PositionArea.Columns.Clear();
            dtg_PositionArea.Columns.Add(new DataGridViewColumn() { Name = "DGV_PositionArea_Index", HeaderText = "POS_AreaIndex", Width = 120, CellTemplate = currentCellTemplate });
            dtg_PositionArea.Columns.Add(new DataGridViewColumn() { Name = "DGV_PositionArea_RobotGroup", HeaderText = "RobotGoup", Width = 140, CellTemplate = currentCellTemplate });
            dtg_PositionArea.Columns.Add(new DataGridViewColumn() { Name = "DGV_PositionArea_Use", HeaderText = "POS_AreaUse", Width = 130, CellTemplate = currentCellTemplate });
            dtg_PositionArea.Columns.Add(new DataGridViewColumn() { Name = "DGV_PositionArea_FloorName", HeaderText = "POS_FloorName", Width = 120, CellTemplate = currentCellTemplate });
            dtg_PositionArea.Columns.Add(new DataGridViewColumn() { Name = "DGV_PositionArea_Name", HeaderText = "POS_AreaName", Width = 140, CellTemplate = currentCellTemplate });
            dtg_PositionArea.Columns.Add(new DataGridViewColumn() { Name = "DGV_PositionArea_X1", HeaderText = "POS_AreaX1", Width = 120, CellTemplate = currentCellTemplate });
            dtg_PositionArea.Columns.Add(new DataGridViewColumn() { Name = "DGV_PositionArea_X2", HeaderText = "POS_AreaX2", Width = 120, CellTemplate = currentCellTemplate });
            dtg_PositionArea.Columns.Add(new DataGridViewColumn() { Name = "DGV_PositionArea_X3", HeaderText = "POS_AreaX3", Width = 120, CellTemplate = currentCellTemplate });
            dtg_PositionArea.Columns.Add(new DataGridViewColumn() { Name = "DGV_PositionArea_X4", HeaderText = "POS_AreaX4", Width = 120, CellTemplate = currentCellTemplate });
            dtg_PositionArea.Columns.Add(new DataGridViewColumn() { Name = "DGV_PositionArea_Y1", HeaderText = "POS_AreaY1", Width = 120, CellTemplate = currentCellTemplate });
            dtg_PositionArea.Columns.Add(new DataGridViewColumn() { Name = "DGV_PositionArea_Y2", HeaderText = "POS_AreaY2", Width = 120, CellTemplate = currentCellTemplate });
            dtg_PositionArea.Columns.Add(new DataGridViewColumn() { Name = "DGV_PositionArea_Y3", HeaderText = "POS_AreaY3", Width = 120, CellTemplate = currentCellTemplate });
            dtg_PositionArea.Columns.Add(new DataGridViewColumn() { Name = "DGV_PositionArea_Y4", HeaderText = "POS_AreaY4", Width = 120, CellTemplate = currentCellTemplate });
            dtg_PositionArea.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_PositionArea.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_PositionArea.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
            dtg_PositionArea.Columns["DGV_PositionArea_Index"].HeaderText = "POS_Area" + "\r\n" + "Index";
            dtg_PositionArea.Columns["DGV_PositionArea_Use"].HeaderText = "POS" + "\r\n" + "Use/Unuse";
            dtg_PositionArea.Columns["DGV_PositionArea_FloorName"].HeaderText = "POS" + "\r\n" + "FloorName";
            dtg_PositionArea.Columns["DGV_PositionArea_Name"].HeaderText = "POS" + "\r\n" + "AreaName";
         
            dtg_PositionArea.Rows.Clear();

            dtg_PositionArea.Columns["DGV_PositionArea_X2"].Visible = false;
            dtg_PositionArea.Columns["DGV_PositionArea_X4"].Visible = false;
            dtg_PositionArea.Columns["DGV_PositionArea_Y2"].Visible = false;
            dtg_PositionArea.Columns["DGV_PositionArea_Y4"].Visible = false;

        }

        private void PositionArea_Display()
        {
            dtg_PositionArea.Columns["DGV_PositionArea_RobotGroup"].ReadOnly = true;
            dtg_PositionArea.Columns["DGV_PositionArea_Index"].ReadOnly = true;
            dtg_PositionArea.Columns["DGV_PositionArea_Use"].ReadOnly = true;
            dtg_PositionArea.Columns["DGV_PositionArea_X2"].ReadOnly = true;
            dtg_PositionArea.Columns["DGV_PositionArea_X4"].ReadOnly = true;
            dtg_PositionArea.Columns["DGV_PositionArea_Y2"].ReadOnly = true;
            dtg_PositionArea.Columns["DGV_PositionArea_Y4"].ReadOnly = true;

            int firstRowIndex = dtg_PositionArea.FirstDisplayedScrollingColumnIndex;
            dtg_PositionArea.Rows.Clear();
            dtg_PositionArea.RowTemplate.Height = 40;
            foreach (var positionAreaConfig in uow.PositionAreaConfigs.GetAll())
            {
                int newRowIndex = dtg_PositionArea.Rows.Add();
                var newRow = dtg_PositionArea.Rows[newRowIndex];
                newRow.Cells["DGV_PositionArea_Index"].Value = $"Area{positionAreaConfig.Id}";
                newRow.Cells["DGV_PositionArea_RobotGroup"].Value = positionAreaConfig.ACSRobotGroup;
                newRow.Cells["DGV_PositionArea_Use"].Value = positionAreaConfig.PositionAreaUse;
                newRow.Cells["DGV_PositionArea_FloorName"].Value = positionAreaConfig.PositionAreaFloorName;
                newRow.Cells["DGV_PositionArea_Name"].Value = positionAreaConfig.PositionAreaName;
                newRow.Cells["DGV_PositionArea_X1"].Value = positionAreaConfig.PositionAreaX1;
                newRow.Cells["DGV_PositionArea_X2"].Value = positionAreaConfig.PositionAreaX2;
                newRow.Cells["DGV_PositionArea_X3"].Value = positionAreaConfig.PositionAreaX3;
                newRow.Cells["DGV_PositionArea_X4"].Value = positionAreaConfig.PositionAreaX4;
                newRow.Cells["DGV_PositionArea_Y1"].Value = positionAreaConfig.PositionAreaY1;
                newRow.Cells["DGV_PositionArea_Y2"].Value = positionAreaConfig.PositionAreaY2;
                newRow.Cells["DGV_PositionArea_Y3"].Value = positionAreaConfig.PositionAreaY3;
                newRow.Cells["DGV_PositionArea_Y4"].Value = positionAreaConfig.PositionAreaY4;
                newRow.Tag = positionAreaConfig;

                dtg_PositionArea.Rows[newRowIndex].Cells[newRow.Cells["DGV_PositionArea_X2"].ColumnIndex].Style.BackColor = Color.MediumAquamarine;
                dtg_PositionArea.Rows[newRowIndex].Cells[newRow.Cells["DGV_PositionArea_X4"].ColumnIndex].Style.BackColor = Color.MediumAquamarine;
                dtg_PositionArea.Rows[newRowIndex].Cells[newRow.Cells["DGV_PositionArea_Y2"].ColumnIndex].Style.BackColor = Color.MediumAquamarine;
                dtg_PositionArea.Rows[newRowIndex].Cells[newRow.Cells["DGV_PositionArea_Y4"].ColumnIndex].Style.BackColor = Color.MediumAquamarine;

                if (positionAreaConfig.PositionAreaUse == "Unuse")
                {
                    dtg_PositionArea.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                    dtg_PositionArea.Rows[newRowIndex].DefaultCellStyle.ForeColor = Color.Black;
                }
                else
                {
                    dtg_PositionArea.Rows[newRowIndex].DefaultCellStyle.BackColor = main.skinColor;
                    dtg_PositionArea.Rows[newRowIndex].DefaultCellStyle.ForeColor = Color.White;
                }
            }

            if (firstRowIndex >= 0 && firstRowIndex < dtg_PositionArea.Rows.Count)
            {
                dtg_PositionArea.FirstDisplayedScrollingRowIndex = firstRowIndex;
            }
            dtg_PositionArea.ClearSelection();
        }

        private void dtg_PositionArea_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

            DataGridView grid = (DataGridView)sender;
            DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
            DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

            PositionAreaConfig selectedRowTag = (PositionAreaConfig)selectedRow.Tag;
            PositionAreaConfig targetConfig = uow.PositionAreaConfigs.Find(m => m.Id == selectedRowTag.Id).FirstOrDefault();

            string ChangeDatamsg = null;

            if (targetConfig != null)
            {
                if (e.ColumnIndex == grid.Columns["DGV_PositionArea_RobotGroup"].Index)
                {
                    var insertNum = new GroupSelectForm(main, uow);
                    if (insertNum.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insertNum.InputValue.Trim();

                        if (targetConfig.ACSRobotGroup != newValue)
                        {
                            string oldValue = targetConfig.ACSRobotGroup;
                            targetConfig.ACSRobotGroup = newValue;
                            insertNum.Close();
                            ChangeDatamsg = $"Position,PositionArea Config{targetConfig.Id} PositionArea ACSRobotGroup from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }


                    }
                }
                else if (e.ColumnIndex == grid.Columns["DGV_PositionArea_Use"].Index)
                {
                    var insertUse = new UseSelectForm();
                    if (insertUse.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insertUse.InputValue.Trim();
                        if (targetConfig.PositionAreaUse != newValue)
                        {
                            string oldValue = targetConfig.PositionAreaUse;
                            targetConfig.PositionAreaUse = newValue;

                            insertUse.Close();
                            ChangeDatamsg = $"Position,PositionArea Config{targetConfig.Id} PositionArea Use Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                            if (targetConfig.PositionAreaUse == "Use")
                            {
                                dtg_PositionArea.Rows[e.RowIndex].DefaultCellStyle.BackColor = main.skinColor;
                                dtg_PositionArea.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                            }
                            else
                            {
                                dtg_PositionArea.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                                dtg_PositionArea.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                            }
                        }

                    }
                }
                else if (e.ColumnIndex == grid.Columns["DGV_PositionArea_FloorName"].Index)
                {
                    var insertFloor = new FloorMapIdSelectForm(uow);
                    if (insertFloor.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insertFloor.InputValue.Trim();

                        var positionAreaFloorMapId = uow.FloorMapIDConfigs.Find(m => m.FloorName == newValue).FirstOrDefault();
                        if (positionAreaFloorMapId != null && targetConfig.PositionAreaFloorName != newValue)
                        {
                            string oldValue = targetConfig.PositionAreaFloorName;
                            targetConfig.PositionAreaFloorName = newValue;
                            targetConfig.PositionAreaFloorMapId = positionAreaFloorMapId.MapID;

                            targetConfig.PositionAreaName = newValue + targetConfig.PositionAreaName.Substring(oldValue.Length);
                            selectedRow.Cells["DGV_PositionArea_Name"].Value = targetConfig.PositionAreaName;

                            insertFloor.Close();
                            ChangeDatamsg = $"Position,PositionArea Config{targetConfig.Id} PositionArea FloorName Change from {oldValue} to {newValue} newValue FloorMapId{targetConfig.PositionAreaFloorMapId}";
                            ChageData(newValue, ChangeDatamsg);

                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                PositionArea_Display();
            }


            void ChageData(string newValue, string changeDatamsg)
            {
                if (changeDatamsg != null)
                {
                    selectedCell.Value = newValue;
                    selectedRowTag = targetConfig;
                    uow.PositionAreaConfigs.Update(targetConfig);
                    string[] ChangeDatamsgSplit = changeDatamsg.Split(',');
                    main.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                }
            }

        }

        private void dtg_PositionArea_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

                DataGridView grid = (DataGridView)sender;
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

                PositionAreaConfig selectedRowTag = (PositionAreaConfig)selectedRow.Tag;
                PositionAreaConfig targetConfig = uow.PositionAreaConfigs.Find(m => m.Id == selectedRowTag.Id).FirstOrDefault();

                string ChangeDatamsg = null;

                //Convert.ToString 사용시 변수가 null인경우 빈문자열을 반환함 
                //.Tostring() 사용시 변수가 null인경우 에러발생
                if (!string.IsNullOrEmpty(Convert.ToString(selectedCell.Value)) && targetConfig != null)
                {
                    //selectedCell.Value = String.Concat(selectedCell.Value.ToString().Where(c => !char.IsWhiteSpace(c)));    //문자열 빈칸없애기
                    //string newValue = selectedCell.Value.ToString(); 

                    string newValue = String.Concat(selectedCell.Value.ToString().Where(c => !char.IsWhiteSpace(c)));    //문자열 빈칸없애기


                    if (e.ColumnIndex == grid.Columns["DGV_PositionArea_Name"].Index)
                    {
                        if (newValue.Contains("_"))
                        {
                            MessageBox.Show(" '_ '문자를 사용하실수 없습니다 확인후 다시 등록해주세요!!");
                            PositionArea_Display();
                        }
                        else
                        {
                            if (targetConfig.PositionAreaName != $"{targetConfig.PositionAreaFloorName}{newValue}")
                            {
                                string oldValue = "";

                                if (newValue.StartsWith(targetConfig.PositionAreaFloorName))
                                {
                                    oldValue = targetConfig.PositionAreaName;
                                    targetConfig.PositionAreaName = newValue;
                                }
                                else
                                {
                                    oldValue = $"{targetConfig.PositionAreaFloorName}{targetConfig.PositionAreaName}";
                                    targetConfig.PositionAreaName = $"{targetConfig.PositionAreaFloorName}{newValue}";
                                }
                                ChangeDatamsg = $"Position,PositionArea Config{targetConfig.Id} PositionArea Name Change from {oldValue} to {targetConfig.PositionAreaName}";
                                ChageData(targetConfig.PositionAreaName, ChangeDatamsg);
                            }
                        }
                    }
                    else if (e.ColumnIndex == grid.Columns["DGV_PositionArea_X1"].Index)
                    {
                        newValue = Convert.ToDouble(selectedCell.Value).ToString("0.0"); // 소수 1자리로 포맷한다
                        selectedCell.Value = newValue; // 셀값을 포맷한 값으로 다시 설정한다

                        if (targetConfig.PositionAreaX1 != newValue)
                        {
                            string oldValue = targetConfig.PositionAreaX1;
                            targetConfig.PositionAreaX1 = newValue;
                            targetConfig.PositionAreaX4 = newValue;
                            ChangeDatamsg = $"Position,PositionArea Config{targetConfig.Id} PositionArea X1 Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                    else if (e.ColumnIndex == grid.Columns["DGV_PositionArea_X2"].Index)
                    {
                        newValue = Convert.ToDouble(selectedCell.Value).ToString("0.0"); // 소수 1자리로 포맷한다
                        selectedCell.Value = newValue; // 셀값을 포맷한 값으로 다시 설정한다

                        if (targetConfig.PositionAreaX2 != newValue)
                        {
                            string oldValue = targetConfig.PositionAreaX2;
                            targetConfig.PositionAreaX2 = newValue;
                            ChangeDatamsg = $"Position,PositionArea Config{targetConfig.Id} PositionArea X2 Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                    else if (e.ColumnIndex == grid.Columns["DGV_PositionArea_X3"].Index)
                    {
                        newValue = Convert.ToDouble(selectedCell.Value).ToString("0.0"); // 소수 1자리로 포맷한다
                        selectedCell.Value = newValue; // 셀값을 포맷한 값으로 다시 설정한다

                        if (targetConfig.PositionAreaX3 != newValue)
                        {
                            string oldValue = targetConfig.PositionAreaX3;
                            targetConfig.PositionAreaX2 = newValue;
                            targetConfig.PositionAreaX3 = newValue;
                            ChangeDatamsg = $"Position,PositionArea Config{targetConfig.Id} PositionArea X3 Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                    else if (e.ColumnIndex == grid.Columns["DGV_PositionArea_X4"].Index)
                    {
                        newValue = Convert.ToDouble(selectedCell.Value).ToString("0.0"); // 소수 1자리로 포맷한다
                        selectedCell.Value = newValue; // 셀값을 포맷한 값으로 다시 설정한다

                        if (targetConfig.PositionAreaX4 != newValue)
                        {
                            string oldValue = targetConfig.PositionAreaX4;
                            targetConfig.PositionAreaX4 = newValue;
                            ChangeDatamsg = $"Position,PositionArea Config{targetConfig.Id} PositionArea X4 Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                    else if (e.ColumnIndex == grid.Columns["DGV_PositionArea_Y1"].Index)
                    {
                        newValue = Convert.ToDouble(selectedCell.Value).ToString("0.0"); // 소수 1자리로 포맷한다
                        selectedCell.Value = newValue; // 셀값을 포맷한 값으로 다시 설정한다

                        if (targetConfig.PositionAreaY1 != newValue)
                        {
                            string oldValue = targetConfig.PositionAreaY1;
                            targetConfig.PositionAreaY1 = newValue;
                            targetConfig.PositionAreaY2 = newValue;
                            ChangeDatamsg = $"Position,PositionArea Config{targetConfig.Id} PositionArea Y1 Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                    else if (e.ColumnIndex == grid.Columns["DGV_PositionArea_Y2"].Index)
                    {
                        newValue = Convert.ToDouble(selectedCell.Value).ToString("0.0"); // 소수 1자리로 포맷한다
                        selectedCell.Value = newValue; // 셀값을 포맷한 값으로 다시 설정한다

                        if (targetConfig.PositionAreaY2 != newValue)
                        {
                            string oldValue = targetConfig.PositionAreaY2;
                            targetConfig.PositionAreaY2 = newValue;
                            ChangeDatamsg = $"Position,PositionArea Config{targetConfig.Id} PositionArea Y2 Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                    else if (e.ColumnIndex == grid.Columns["DGV_PositionArea_Y3"].Index)
                    {
                        newValue = Convert.ToDouble(selectedCell.Value).ToString("0.0"); // 소수 1자리로 포맷한다
                        selectedCell.Value = newValue; // 셀값을 포맷한 값으로 다시 설정한다

                        if (targetConfig.PositionAreaY3 != newValue)
                        {
                            string oldValue = targetConfig.PositionAreaY3;
                            targetConfig.PositionAreaY3 = newValue;
                            targetConfig.PositionAreaY4 = newValue;
                            ChangeDatamsg = $"Position,PositionArea Config{targetConfig.Id} PositionArea Y3 Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                    else if (e.ColumnIndex == grid.Columns["DGV_PositionArea_Y4"].Index)
                    {
                        newValue = Convert.ToDouble(selectedCell.Value).ToString("0.0"); // 소수 1자리로 포맷한다
                        selectedCell.Value = newValue; // 셀값을 포맷한 값으로 다시 설정한다

                        if (targetConfig.PositionAreaY4 != newValue)
                        {
                            string oldValue = targetConfig.PositionAreaY4;
                            targetConfig.PositionAreaY4 = newValue;
                            ChangeDatamsg = $"Position,PositionArea Config{targetConfig.Id} PositionArea Y4 Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                    PositionArea_Display();
                }

                void ChageData(string newValue, string changeDatamsg)
                {
                    if (changeDatamsg != null)
                    {
                        //int matchingPos = selectedCell.ColumnIndex;
                        //switch (selectedCell.ColumnIndex)
                        //{
                        //    case 5: matchingPos = 8; break;
                        //    case 7: matchingPos = 6; break;
                        //    case 9: matchingPos = 10; break;
                        //    case 11: matchingPos = 12; break;
                        //}
                        //selectedRow.Cells[matchingPos].Value = newValue;
                        selectedRowTag = targetConfig;
                        uow.PositionAreaConfigs.Update(targetConfig);
                        string[] ChangeDatamsgSplit = changeDatamsg.Split(',');
                        main.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                        MessageBox.Show("저장되었습니다");
                    }
                }

            }
            catch (Exception ex)
            {
                main.LogExceptionMessage(ex);
            }
        }

        private void btn_POSBackUpSaveFile_Click(object sender, EventArgs e)
        {
            if (dtg_PositionArea.Rows.Count == 0) return;
            main.SaveAsDataGridviewToCSV(dtg_PositionArea);
            main.UserLog("Position Screen", " BackUp Click ");

        }

        private void btn_BackUpGetFile_Click(object sender, EventArgs e)
        {

            //Excel.Application excelApp = null;
            //Excel.Workbook wb = null;
            //Excel.Worksheet ws = null;
            //try
            //{
            //    string Path = "";
            //    dtg_PositionSetting.Rows.Clear();
            //    OpenFileDialog dlg = new OpenFileDialog();
            //    DialogResult Result = dlg.ShowDialog();
            //    if (Result == DialogResult.OK)
            //    {
            //        Path = dlg.FileName;

            //        if (!string.IsNullOrEmpty(Path) && (Path.EndsWith(".csv") || Path.EndsWith(".xslx")))
            //        {
            //            excelApp = new Excel.Application();
            //            wb = excelApp.Workbooks.Open(Path);
            //            // path 대신 문자열도 가능합니다
            //            // 예. Open(@"D:\test\test.xslx");
            //            ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;
            //            // 첫번째 Worksheet를 선택합니다.
            //            Excel.Range rng = ws.UsedRange;   // '여기'
            //                                              // 현재 Worksheet에서 사용된 셀 전체를 선택합니다.
            //            object[,] data = rng.Value;
            //            // 열들에 들어있는 Data를 배열 (One-based array)로 받아옵니다.

            //            for (int r = 1; r <= data.GetLength(0); r++)
            //            {
            //                int newRowIndex = dtg_PositionSetting.Rows.Add();
            //                var newRow = dtg_PositionSetting.Rows[r - 1];

            //                for (int c = 1; c <= data.GetLength(1); c++)
            //                {
            //                    if (data[r, c] == null)
            //                    {
            //                        continue;
            //                    }
            //                    // Data 빼오기
            //                    // data[r, c] 는 excel의 (r, c) 셀 입니다.
            //                    // data.GetLength(0)은 엑셀에서 사용되는 행의 수를 가져오는 것이고,
            //                    // data.GetLength(1)은 엑셀에서 사용되는 열의 수를 가져오는 것입니다.
            //                    // GetLength와 [ r, c] 의 순서를 바꿔서 사용할 수 있습니다.

            //                    //newRow.Cells["DGV_PositionSetting_Id"].Value = positionAreaConfig.Id.ToString();
            //                    //newRow.Cells["DGV_PositionSetting_Index"].Value = positionAreaConfig.PositionAreaIndex;
            //                    //newRow.Cells["DGV_PositionSetting_Use"].Value = positionAreaConfig.PositionAreaUse;
            //                    //newRow.Cells["DGV_PositionSetting_Name"].Value = positionAreaConfig.PositionAreaName;
            //                    //newRow.Cells["DGV_PositionSetting_X1"].Value = positionAreaConfig.PositionAreaX1;
            //                    //newRow.Cells["DGV_PositionSetting_X2"].Value = positionAreaConfig.PositionAreaX2;
            //                    //newRow.Cells["DGV_PositionSetting_X3"].Value = positionAreaConfig.PositionAreaX3;
            //                    //newRow.Cells["DGV_PositionSetting_X4"].Value = positionAreaConfig.PositionAreaX4;
            //                    //newRow.Cells["DGV_PositionSetting_Y1"].Value = positionAreaConfig.PositionAreaY1;
            //                    //newRow.Cells["DGV_PositionSetting_Y2"].Value = positionAreaConfig.PositionAreaY2;
            //                    //newRow.Cells["DGV_PositionSetting_Y3"].Value = positionAreaConfig.PositionAreaY3;
            //                    //newRow.Cells["DGV_PositionSetting_Y4"].Value = positionAreaConfig.PositionAreaY4;
            //                    //newRow.Tag = positionAreaConfig;


            //                }
            //            }
            //            wb.Close(true);
            //            excelApp.Quit();
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    ReleaseExcelObject(ws);
            //    ReleaseExcelObject(wb);
            //    ReleaseExcelObject(excelApp);
            //}
        }    //엑셀 데이터 불러오기

        private void txt_PositionDataMaxNum_Click(object sender, EventArgs e)
        {
            NumPadForm insertNum = new NumPadForm(((TextBox)sender).Text, ((TextBox)sender).Name, "RT10");
            if (insertNum.ShowDialog() == DialogResult.OK)
            {
                string newValue = int.Parse(insertNum.InputValue).ToString();

                if (((TextBox)sender).Text != int.Parse(insertNum.InputValue).ToString())
                {
                    string oldValue = ((TextBox)sender).Text;

                    ((TextBox)sender).Text = newValue;
                    AppConfiguration.ConfigDataSetting("PosAreaData_MaxNum", newValue);
                    ConfigData.PosAreaData_MaxNum = int.Parse(AppConfiguration.GetAppConfig("PosAreaData_MaxNum"));
                    insertNum.Close();
                    uow.PositionAreaConfigs.Validate_DB_Items();
                    PositionArea_Display();
                    main.UserLog("Position", $"PosAreaData_MaxNum changed from {oldValue} to {newValue}.");

                }
            }
        }


        //static void ReleaseExcelObject(Object obj)
        //{
        //    try
        //    {
        //        if (obj != null)
        //        {
        //            Marshal.ReleaseComObject(obj); // 액셀 객체 해제
        //            obj = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        obj = null;
        //        throw ex;
        //    }
        //    finally
        //    {
        //        GC.Collect(); // 가비지 수집
        //    }
        //}   //엑셀 데이터 불러오기

    }
}
