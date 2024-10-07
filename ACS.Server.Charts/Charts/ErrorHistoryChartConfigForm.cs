using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INA_ACS_Server
{
    public partial class ErrorHistoryChartConfigForm : Form
    {
        public class MyItem
        {
            public string Text;
            public string Tag;
            public override string ToString()
            {
                if (string.IsNullOrEmpty(Tag)) return $"{Text}";
                else return $"{Text} ({Tag})";
            }
        }

        public ErrorHistoryChartConfigForm(ErrorHistoryChartConfigFilter allItems, ErrorHistoryChartConfigFilter filteredItems = null)
        {
            InitializeComponent();
            this.AcceptButton = button1;
            this.CancelButton = button2;

            Init(allItems);

            if (filteredItems != null) SetSelectedItem(filteredItems);
        }

        private void Init(ErrorHistoryChartConfigFilter allItems)
        {
            checkedListBox1.Items.Clear();

            for (int i = 0; i < allItems.RobotNames.Count; i++)
            {
                var item = new MyItem { Text = allItems.RobotNames[i], Tag = allItems.RobotAlias[i] };
                checkedListBox1.Items.Add(item, false);
            }
        }

        private void SetSelectedItem(ErrorHistoryChartConfigFilter filter)
        {
            foreach (var v in new CheckedListBox[] { checkedListBox1 })
            {
                for (int i = 0; i < v.Items.Count; i++)
                {
                    v.SetItemChecked(i, false);
                }
            }

            foreach (string name in filter.RobotNames)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    var item = (MyItem)checkedListBox1.Items[i];
                    if (item.Text == name)
                    {
                        checkedListBox1.SetItemChecked(i, true);
                        break;
                    }
                }
            }
        }

        public ErrorHistoryChartConfigFilter GetSelectedItems()
        {
            var newFilter = new ErrorHistoryChartConfigFilter();

            foreach (MyItem item in checkedListBox1.CheckedItems)
            {
                newFilter.RobotNames.Add(item.Text);
                newFilter.RobotAlias.Add(item.Tag);
            }

            return newFilter;
        }

        // OK 버튼
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        // CANCEL 버튼
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        // 체크리스트 전체 선택/해제 버튼 처리
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            CheckedListBox checkedListBox = null;

            switch (checkBox.Name)
            {
                case nameof(checkBox1): checkedListBox = checkedListBox1; break;
                //case nameof(checkBox2): checkedListBox = checkedListBox2; break;
                //case nameof(checkBox3): checkedListBox = checkedListBox3; break;
            }

            if (checkBox.Checked)
            {
                for (int i = 0; i < checkedListBox.Items.Count; i++)
                    checkedListBox.SetItemChecked(i, checkBox.Checked);
            }
            else
            {
                for (int i = 0; i < checkedListBox.Items.Count; i++)
                    checkedListBox.SetItemChecked(i, false);
            }
        }
    }

}
