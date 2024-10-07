using System;
using System.Data;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace INA_ACS_Server
{
    public partial class PartListScreen : Form
    {
        private readonly MainForm mainForm;
        private readonly IUnitOfWork uow;
        private BindingSource _bindingSource = null;


        public PartListScreen(MainForm mainForm, IUnitOfWork uow)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.uow = uow;

            this.FormClosing += (s, e) =>
            {
                if (e.CloseReason == CloseReason.UserClosing) // 사용자가 ALT-F4 누르거나 x 버튼 눌러서 창을 닫으려 할때
                    e.Cancel = true;
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitGrid();
            DisplayData();
            dataGridView1.DoubleBuffered(true);
        }

        private void InitGrid()
        {
            // 데이터그리드뷰 설정
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.CellDoubleClick += (s, e) => { btnEdit_Click(this, null); };

            // 데이터그리드뷰 헤더/셀 스타일 설정
            dataGridView1.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle()
            {
                Padding = new Padding(10, 3, 10, 3),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                WrapMode = DataGridViewTriState.True,
            };
            dataGridView1.DefaultCellStyle = dataGridView1.ColumnHeadersDefaultCellStyle;

            var rowTemplate = dataGridView1.RowTemplate;
            rowTemplate.DefaultCellStyle.SelectionForeColor = Color.Black;
            rowTemplate.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;// Bisque;
            rowTemplate.Height = 30; // 35;

            // ==========================

            //// button 컬럼 추가
            //DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            //buttonColumn.Name = "Details";
            //buttonColumn.HeaderText = "Details";
            //buttonColumn.Text = "View Details";
            //buttonColumn.UseColumnTextForButtonValue = true;
            //dataGridView1.Columns.Insert(0, buttonColumn);

            //// row number 컬럼 추가
            //DataGridViewTextBoxColumn textboxColumn = new DataGridViewTextBoxColumn();
            //textboxColumn.Name = "No";
            //textboxColumn.HeaderText = "No";
            //dataGridView1.Columns.Insert(0, textboxColumn);

            //// row number 컬럼 데이터 처리
            //dataGridView1.CellFormatting += (s, e) =>
            //{
            //    if (dataGridView1.Columns[e.ColumnIndex].Name == "No")
            //        e.Value = (e.RowIndex + 1).ToString();
            //};
        }

        private void DisplayData()
        {
            // 데이터소스 바인딩
            dataGridView1.DataSource = null; // 초간단 Refresh
            dataGridView1.DataSource = GetBindingSource();

            // id 컬럼 숨김
            dataGridView1.Columns["Id"].Visible = false;

            // 전체 컬럼폭 조정
            dataGridView1.AutoResizeColumns();

            for (int n = 0; n < dataGridView1.Columns.Count; n++) // 컬럼 폭 설정
            {
                dataGridView1.Columns[n].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

        }

        private BindingSource GetBindingSource()
        {
            //var list = uow.PartDB.GetAll().OrderBy(x => string.Join(x.LINE_CD, "_", x.POST_CD)).ToList();
            var list = uow.PartDB.GetAll().OrderBy(x => x.LINE_CD).ThenBy(x=>x.POST_CD).ToList();
            _bindingSource = new BindingSource(new BindingList<PartModel>(list), null);
            return _bindingSource;
        }

        //private BindingSource GetBindingSource_AnynymousType()
        //{
        //    var list = uow.PartDB.GetAll();

        //    // 익명 타입 바인딩은 조회시에만 사용해야 한다.
        //    var viewList = list.Select(x => new
        //    {
        //        LINE_CD = x.LINE_CD,
        //        POST_CD = x.POST_CD,
        //        COMM_PO = x.COMM_PO,
        //        OUT_Q = x.OUT_Q,
        //        COMM_ANG = x.COMM_ANG,
        //        PART_CD = x.PART_CD,
        //        PART_NM = x.PART_NM,
        //        NP_MODE = x.NP_MODE,
        //        NP_OUT_Q = x.NP_OUT_Q,
        //        NP_PART_CD = x.NP_PART_CD,
        //        NP_PART_NM = x.NP_PART_NM,
        //    })
        //    .ToList()
        //    .ToBindingList();

        //    _bindingSource = new BindingSource(viewList, null);
        //    return _bindingSource;
        //}

        private void btnAddToBindingSource_Click(object sender, EventArgs e)
        {
            _bindingSource.Add(new PartModel { LINE_CD = "신설라인1", POST_CD = 99 });
            _bindingSource.Add(new PartModel { LINE_CD = "신설라인2", POST_CD = 99 });
        }

        private void btnAdd_Click(object sender, EventArgs e)//추가
        {
            //새로운 데이터 생성(초기값을 설정하지 않고 보낼시 NULL 값으로 전송되어 에러발생)
            var newModel = new PartModel { COMM_PO = "N", NP_MODE = "N" };

            //편집 윈도우에 전달하여 편집한다
            var editForm = new PartEditor { EditDataModel = newModel };
            if (editForm.ShowDialog(this) == DialogResult.OK)
            {
                uow.PartDB.Add(newModel);
                DisplayData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // 편집할 데이터
            PartModel editModel = (PartModel)_bindingSource.Current;

            //편집 윈도우에 전달하여 편집한다
            var editForm = new PartEditor { EditDataModel = editModel };
            if (editForm.ShowDialog(this) == DialogResult.OK)
            {
                uow.PartDB.Update(editModel);

                int iTopRowIndex = dataGridView1.FirstDisplayedScrollingRowIndex;
                int iRow = dataGridView1.CurrentCell.RowIndex;
                int iCol = dataGridView1.CurrentCell.ColumnIndex;

                DisplayData();

                dataGridView1.FirstDisplayedScrollingRowIndex = iTopRowIndex;
                dataGridView1.Rows[iRow].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[iRow].Cells[iCol];
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // 삭제할 데이터
            PartModel removeModel = (PartModel)_bindingSource.Current;

            if (MessageBox.Show("선택한 항목을 삭제 하시겠습니까?", "User Confirm", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                uow.PartDB.Remove(removeModel);
                DisplayData();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            DisplayData();
        }

        public void Init()
        {
            //
        }
    }
}
