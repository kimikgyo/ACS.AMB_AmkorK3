using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ACS.RobotMap
{
    public partial class UCSettingView : UserControl
    {
        class RobotNameAliasViewModel
        {
            [ReadOnly(true)] public int No { get; set; }
            [ReadOnly(true)] public string RobotName { get; set; }
            [ReadOnly(true)] public string RobotAlias { get; set; }
            [ReadOnly(false)] public bool Display { get; set; }
        }

        private BindingList<RobotNameAliasViewModel> bindingList = null;
        //private SortableBindingList<RobotNameAliasViewModel> bindingList = null;

        private MonitorConfigData monitorConfig;


        public UCSettingView()
        {
            InitializeComponent();
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.RowTemplate.Height = 40;
            this.dataGridView1.DefaultCellStyle.Padding = new Padding(3, 0, 3, 0);
            this.dataGridView1.DoubleBuffered(true);
        }

        public void InitConfig(MonitorConfigData monitorConfig)
        {
            this.monitorConfig = monitorConfig;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadSettings();
        }

        public void DisplayData()
        {
            // 데이터소스 바인딩
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = GetBindingSource();
            dataGridView1.AutoResizeColumns();

            if (dataGridView1.Columns.Count > 0)
                dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }



        private BindingList<RobotNameAliasViewModel> GetBindingSource()
        {
            // DB에서 데이터 가져온다
            var readData = RobotNameAlias.GetAll();   //.OrderBy(x => x.RobotAlias).ToList();
            var viewData = readData?.Select((x, index) => new RobotNameAliasViewModel
            {
                No = index + 1,
                RobotName = x.RobotName,
                RobotAlias = x.RobotAlias,
                Display = x.RobotName != null && monitorConfig.DisplayRobotNames != null && monitorConfig.DisplayRobotNames.ContainsKey(x.RobotName)
            }).ToList();

            // 바인딩 소스 설정
            if (viewData != null)
            {
                bindingList = new BindingList<RobotNameAliasViewModel>(viewData);
                //bindingList = new SortableBindingList<RobotNameAliasViewModel>(viewData);
            }
            else
            {
                bindingList = null;
            }

            return bindingList;
        }


        // reload data
        private void Button1_Click(object sender, EventArgs e)
        {
            DisplayData();
        }


        // save data
        private void Button2_Click(object sender, EventArgs e)
        {
            // get data
            var data = bindingList;

            // add data to dict.
            monitorConfig.DisplayRobotNames.Clear();
            foreach (var item in data)
            {
                if (item.Display)
                {
                    if (string.IsNullOrEmpty(item.RobotName) == false)
                    {
                        if (monitorConfig.DisplayRobotNames.ContainsKey(item.RobotName) == false)
                            monitorConfig.DisplayRobotNames.Add(item.RobotName, item.RobotAlias);
                    }
                }
            }

            // save dict.
            string saveDictText = Util.ConvertDictionaryToString(monitorConfig.DisplayRobotNames);
            SaveSettings(saveDictText);

            // reload data
            DisplayData();
        }


        private void LoadSettings()
        {
            //throw new Exception("LoadSettings: 설정은 여기서 로드하지 않고, 이벤트로 부모에게 전달하도록 작업 필요!");
            try
            {
                string tmp = ConfigurationManager.AppSettings["RobotNames"];
                monitorConfig.DisplayRobotNames = Util.ConvertStringToDictionary(tmp) ?? new Dictionary<string, string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        private void SaveSettings(string value)
        {
            //throw new Exception("SaveSettings: 설정은 여기서 저장하지 않고, 이벤트로 부모에게 전달하도록 작업 필요!");
            AppConfiguration.SetAppConfig("RobotNames", value);
        }




        class Util
        {
            public static string ConvertDictionaryToString<TKey, TValue>(Dictionary<TKey, TValue> dict)
            {
                try
                {
                    var result = JsonConvert.SerializeObject(dict);
                    return result;
                }
                catch { }
                return null;

                //string format = "{0}=`{1}`|";
                //var sb = new StringBuilder();
                //foreach (var kv in dict)
                //{
                //    sb.AppendFormat(format, kv.Key, kv.Value);
                //}
                //if (sb.Length > 0) sb.Remove(sb.Length - 1, 1);

                //return sb.ToString();
            }

            public static Dictionary<string, string> ConvertStringToDictionary(string dictString)
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(dictString);
                    return result;
                }
                catch { }
                return null;

                //var result = new Dictionary<string, string>();
                //var items = dictString.Split('|').Select(x => x.Trim().Split('='));
                //foreach (string[] tokens in items)
                //{
                //    if (tokens.Length == 2)
                //    {
                //        string k = tokens[0];
                //        string v = tokens[1].Trim('`');

                //        if (result.ContainsKey(k) == false)
                //            result.Add(k, v);
                //    }
                //}
                //return result;
            }

            // form class를 만날때 까지 계속 부모 개체를 찾아 올라가는 함수
            public static Form GetParentForm(Control control)
            {
                if (control.Parent == null) return null;
                if (control.Parent is Form)
                {
                    return control.Parent as Form;
                }
                else
                {
                    return GetParentForm(control.Parent);
                }
            }

            public static string GetLocalIP()
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                string localIP = string.Empty;
                for (int i = 0; i < host.AddressList.Length; i++)
                {
                    if (host.AddressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        localIP = host.AddressList[i].ToString();
                        break;
                    }
                }
                return localIP;
            }
        }
    }

}
