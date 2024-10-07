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
    public partial class ProductNameInfoScreen : Form
    {
        private readonly MainForm mainForm;
        private readonly IUnitOfWork uow;

        public ProductNameInfoScreen(MainForm mainForm, IUnitOfWork uow)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.uow = uow;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            dtg_ProductNameInfo.AlternatingRowsDefaultCellStyle = null;
            dtg_ProductNameInfo.DoubleBuffered(true);
            ProductTextBoxInit();
            ProductNameInfoInit();
            ProductNameInfoDisplay();

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ProductTextBoxInit()
        {
            txt_ProductNameInfo_MaxNum.Text = ConfigData.ProductNameInfo_MaxNum.ToString();

        }

        private void ProductNameInfoInit()
        {
            dtg_ProductNameInfo.ScrollBars = ScrollBars.Both;
            dtg_ProductNameInfo.AllowUserToResizeColumns = true;
            dtg_ProductNameInfo.ColumnHeadersHeight = 40;
            dtg_ProductNameInfo.RowTemplate.Height = 40;
            dtg_ProductNameInfo.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_ProductNameInfo.Columns[0].CellTemplate;
            dtg_ProductNameInfo.Columns.Clear();
            dtg_ProductNameInfo.Columns.Add(new DataGridViewColumn() { Name = "DGV_ProductNameInfo_Id", HeaderText = "Index", Width = 70, CellTemplate = currentCellTemplate });
            dtg_ProductNameInfo.Columns.Add(new DataGridViewColumn() { Name = "DGV_ProductNameInfo_Register22Value", HeaderText = "Register22Value", Width = 150, CellTemplate = currentCellTemplate });
            dtg_ProductNameInfo.Columns.Add(new DataGridViewColumn() { Name = "DGV_ProductNameInfo_RegisterNo", HeaderText = "RegisterNo", Width = 150, CellTemplate = currentCellTemplate });
            dtg_ProductNameInfo.Columns.Add(new DataGridViewColumn() { Name = "DGV_ProductNameInfo_RegisterValue", HeaderText = "RegisterValue", Width = 150, CellTemplate = currentCellTemplate });
            dtg_ProductNameInfo.Columns.Add(new DataGridViewColumn() { Name = "DGV_ProductNameInfo_ProductName", HeaderText = "ProductName", Width = 200, CellTemplate = currentCellTemplate });
            dtg_ProductNameInfo.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_ProductNameInfo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_ProductNameInfo.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
            dtg_ProductNameInfo.Rows.Clear();
        }

        private void ProductNameInfoDisplay()
        {
            dtg_ProductNameInfo.Columns["DGV_ProductNameInfo_Id"].ReadOnly = true;

            int firstRowIndex = dtg_ProductNameInfo.FirstDisplayedScrollingColumnIndex;
            dtg_ProductNameInfo.Rows.Clear();
            dtg_ProductNameInfo.RowTemplate.Height = 40;

            foreach (var productNameInfos in uow.ProductNameInfos.GetAll())
            {
                int newRowIndex = dtg_ProductNameInfo.Rows.Add();
                var newRow = dtg_ProductNameInfo.Rows[newRowIndex];
                newRow.Cells["DGV_ProductNameInfo_Id"].Value = productNameInfos.Id.ToString();
                newRow.Cells["DGV_ProductNameInfo_Register22Value"].Value = productNameInfos.Regiser22Vlaue.ToString();
                newRow.Cells["DGV_ProductNameInfo_RegisterNo"].Value = productNameInfos.RegisterNo;
                newRow.Cells["DGV_ProductNameInfo_RegisterValue"].Value = productNameInfos.RegisterValue.ToString();
                newRow.Cells["DGV_ProductNameInfo_ProductName"].Value = productNameInfos.ProductName;
                newRow.Tag = productNameInfos;

                if (firstRowIndex >= 0 && firstRowIndex < dtg_ProductNameInfo.Rows.Count)
                {
                    dtg_ProductNameInfo.FirstDisplayedScrollingRowIndex = firstRowIndex;
                }
                dtg_ProductNameInfo.ClearSelection();
            }
        }

        private void dtg_ProductNameInfo_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

            DataGridView grid = (DataGridView)sender;
            DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
            DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];


            ProductNameInfoModel targetConfigTagdate = (ProductNameInfoModel)selectedRow.Tag;
            ProductNameInfoModel targetConfig = uow.ProductNameInfos.Find(m => m.Id == targetConfigTagdate.Id).FirstOrDefault();

            string ChangeDatamsg = null;
            if (targetConfig != null)
            {
                if (e.ColumnIndex == grid.Columns["DGV_ProductNameInfo_Register22Value"].Index)
                {
                    var insertNum = new NumPadForm(dtg_ProductNameInfo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "dtg_ProductNameInfo", "RT31");
                    if (insertNum.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insertNum.InputValue.Trim();
                        if (Convert.ToString(targetConfig.Regiser22Vlaue) != newValue)
                        {
                            string oldValue = Convert.ToString(targetConfig.Regiser22Vlaue);

                            targetConfig.Regiser22Vlaue = int.Parse(newValue);
                            insertNum.Close();
                            ChangeDatamsg = $"ProductNameInfo, ProductNameInfo{targetConfig.Id} ProductNameInfo Register22Value Change from {oldValue} to {newValue}";

                        }
                    }
                }
                else if (e.ColumnIndex == grid.Columns["DGV_ProductNameInfo_RegisterNo"].Index)
                {
                    var insertNum = new ProductRegistarNoSelectForm(mainForm);
                    if (insertNum.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insertNum.InputValue.Trim();

                        if (targetConfig.RegisterNo != newValue)
                        {
                            string oldValue = targetConfig.RegisterNo;
                            targetConfig.RegisterNo = insertNum.InputValue;
                            insertNum.Close();
                            ChangeDatamsg = $"ProductNameInfo, ProductNameInfo{targetConfig.Id} ProductNameInfo RegisterNo Change from {oldValue} to {newValue}";

                        }
                    }

                }
                else if (e.ColumnIndex == grid.Columns["DGV_ProductNameInfo_RegisterValue"].Index)
                {
                    var insertNum = new NumPadForm(dtg_ProductNameInfo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "dtg_ProductNameInfo", "RT31");
                    if (insertNum.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insertNum.InputValue.Trim();
                        if (Convert.ToString(targetConfig.RegisterValue) != newValue)
                        {
                            string oldValue = selectedCell.Value.ToString().Trim();

                            targetConfig.RegisterValue = int.Parse(newValue);
                            insertNum.Close();
                            ChangeDatamsg = $"ProductNameInfo, ProductNameInfo{targetConfig.Id} ProductNameInfo RegisterValue Change from {oldValue} to {newValue}";
                        }
                    }
                }
            }
            else MessageBox.Show("확인후 등록 해주시기 바랍니다.");
            if (ChangeDatamsg != null)
            {
                uow.ProductNameInfos.Update(targetConfig);
                string[] ChangeDatamsgSplit = ChangeDatamsg.Split(',');
                mainForm.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                ProductNameInfoDisplay();
            }

        }

        private void dtg_ProductNameInfo_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

                DataGridView grid = (DataGridView)sender;
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

                ProductNameInfoModel targetConfigTagdate = (ProductNameInfoModel)selectedRow.Tag;
                ProductNameInfoModel targetConfig = uow.ProductNameInfos.Find(m => m.Id == targetConfigTagdate.Id).FirstOrDefault();
            
                string ChangeDatamsg = null;

                //Convert.ToString 사용시 변수가 null인경우 빈문자열을 반환함 
                //.Tostring() 사용시 변수가 null인경우 에러발생
                if (!string.IsNullOrEmpty(Convert.ToString(selectedCell.Value)) && targetConfig != null)
                {
                    string newValue = String.Concat(selectedCell.Value.ToString().Where(c => !char.IsWhiteSpace(c)));    //문자열 빈칸없애기

                    if (e.ColumnIndex == grid.Columns["DGV_ProductNameInfo_ProductName"].Index)
                    {
                        if (targetConfig.ProductName != newValue)
                        {
                            string oldValue = targetConfig.ProductName;
                            targetConfig.ProductName = newValue;
                            ChangeDatamsg = $"ProductNameInfo, ProductNameInfo{targetConfig.Id} ProductNameInfo ProductName Change from {oldValue} to {newValue}";
                        }
                    }

                }
                else MessageBox.Show("확인후 등록 해주시기 바랍니다.");

                if (ChangeDatamsg != null)
                {
                    uow.ProductNameInfos.Update(targetConfig);
                    string[] ChangeDatamsgSplit = ChangeDatamsg.Split(',');
                    mainForm.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                    MessageBox.Show("저장되었습니다");

                    ProductNameInfoDisplay();
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


        private void txt_ProductNameInfo_MaxNum_Click(object sender, EventArgs e)
        {
            NumPadForm insertNum = new NumPadForm(((TextBox)sender).Text, ((TextBox)sender).Name, "RT21");
            DialogResult result = insertNum.ShowDialog();
            if (result == DialogResult.OK)
            {
                string newValue = insertNum.InputValue;
                if (((TextBox)sender).Text != newValue)
                {
                    string oldValue = ((TextBox)sender).Text;
                    ((TextBox)sender).Text = newValue;

                    AppConfiguration.ConfigDataSetting("ProductNameInfo_MaxNum", newValue);
                    ConfigData.ProductNameInfo_MaxNum = int.Parse(AppConfiguration.GetAppConfig("ProductNameInfo_MaxNum"));
                    insertNum.Close();
                    mainForm.UserLog("ProductNameInfo", $"ProductNameInfo MaxNum Change from {oldValue} to {newValue}");
                    uow.ProductNameInfos.Validate_DB_Items();
                    ProductNameInfoDisplay();
                }
            }
        }
    }
}
