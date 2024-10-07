using Dapper;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace INA_ACS_Server.OPWindows
{
    public partial class TestForm : Form
    {
        private readonly MainForm main;
        private readonly IUnitOfWork uow;


        public TestForm(MainForm main, IUnitOfWork uow)
        {
            InitializeComponent();
            this.main = main;
            this.uow = uow;

            txtNodeNo.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) button1_Click(null, null); };
            txtJobNodeNo.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) button8_Click(null, null); };
            cbPopCallName.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) button12_Click(null, null); };
            cbPopCallType.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) button12_Click(null, null); };

            var timer = new Timer() { Interval = 500 };
            timer.Tick += Timer_Tick;
            timer.Start();


            cbPopCallName.Items.Add("MC351_1");
            cbPopCallName.Items.Add("MC351_2");

            cbPopCallName.Items.Add("MC311_15");
            cbPopCallName.Items.Add("MC311_16");
            cbPopCallName.Items.Add("MC311_17");

            cbPopCallName.Items.Add("MC310_1");
            cbPopCallName.Items.Add("MC310_2");
            cbPopCallName.Items.Add("MC310_3");
            cbPopCallName.Items.Add("MC310_4");
            cbPopCallName.Items.Add("MC310_5");

            cbPopCallName.Items.Add("MC110_1");
            cbPopCallName.Items.Add("MC110_2");
            cbPopCallName.Items.Add("MC110_3");
            cbPopCallName.Items.Add("MC110_4");
            cbPopCallName.Items.Add("MC112_1");
            cbPopCallName.Items.Add("MC112_2");
            cbPopCallName.Items.Add("MC112_3");
            cbPopCallName.Items.Add("MC112_4");

            cbPopCallName.SelectedIndex = 0;
            cbPopCallType.SelectedIndex = 0;
            cbWmsOutPos.SelectedIndex = 0;
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            var bs1 = new BindingList<Job>(uow.Jobs.GetAll()) { AllowNew = false, AllowEdit = false, AllowRemove = false };
            dgvJobs.DataError += (s2, e2) => { };
            dgvJobs.DataSource = bs1;
            dgvJobs.DoubleBuffered(true);

            var bs2 = new BindingList<Mission>(uow.Missions.GetAll()) { AllowNew = false, AllowEdit = false, AllowRemove = false };
            dgvMissions.DataError += (s2, e2) => { };
            dgvMissions.DataSource = bs2;
            dgvMissions.DoubleBuffered(true);

            // display robot info
            var sb = new StringBuilder();
            foreach (var r in uow.Robots.GetAll())
            {
                sb.AppendLine($"{r.Id} : {r.RobotName,-15} : {r.StateText,-15} : {r.MissionText}");
            }
            tb_RobotInfo.Text = sb.ToString();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Location = Point.Add(main.Location, new Size(700, 0));
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                WindowState = FormWindowState.Minimized;
                e.Cancel = true;
            }
            base.OnFormClosing(e);
        }


        // add 1
        private void button1_Click(object sender, EventArgs e)
        {
            //int.TryParse(txtNodeNo.Text, out int nodeNo);
            //var callButton = uow.CallButtons.GetAll().SingleOrDefault(x => x.ButtonIndex == nodeNo);
            //if (callButton != null)
            //    MissionCommandQueue.Enqueue(new MissionCommand { Code = MissionCommandCode.ADD, Text = callButton.ButtonName });
            //else
            //    MessageBox.Show($"call button {nodeNo} not found");
        }
        // remove 1
        private void button2_Click(object sender, EventArgs e)
        {
            //int.TryParse(txtNodeNo.Text, out int nodeNo);
            //var callButton = uow.CallButtons.GetAll().SingleOrDefault(x => x.ButtonIndex == nodeNo);
            //if (callButton != null)
            //    MissionCommandQueue.Enqueue(new MissionCommand { Code = MissionCommandCode.TRY_REMOVE, Text = callButton.ButtonName });
            //else
            //    MessageBox.Show($"call button {nodeNo} not found");
        }
        // add all
        private void button3_Click(object sender, EventArgs e)
        {
            //var callButtons = uow.CallButtons.GetAll();
            //foreach (var callButton in callButtons)
            //{
            //    MissionCommandQueue.Enqueue(new MissionCommand { Code = MissionCommandCode.ADD, Text = callButton.ButtonName });
            //}
        }
        // remove all
        private void button4_Click(object sender, EventArgs e)
        {
            //var callButtons = uow.CallButtons.GetAll();
            //foreach (var callButton in callButtons)
            //{
            //    MissionCommandQueue.Enqueue(new MissionCommand { Code = MissionCommandCode.TRY_REMOVE, Text = callButton.ButtonName });
            //}
        }
        // remove all (force)
        private void button5_Click(object sender, EventArgs e)
        {
            JobCommandQueue.Enqueue(new JobCommand { Code = JobCommandCode.REMOVE_ALL });
        }

        // 타워램프 테스트
        private void button6_Click(object sender, EventArgs e)
        {
            //var f = new TowerLamp();
            //f.Show(this);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //var f = new PartListScreen(null, uow);
            //f.Show(this);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //int.TryParse(txtJobNodeNo.Text, out int nodeNo);
            //var callButton = uow.CallButtons.GetAll().SingleOrDefault(x => x.ButtonIndex == nodeNo);
            //if (callButton != null)
            //    JobCommandQueue.Enqueue(new JobCommand { Code = JobCommandCode.ADD, Text = callButton.ButtonName });
            //else
            //    MessageBox.Show($"call button {nodeNo} not found");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //int.TryParse(txtJobNodeNo.Text, out int nodeNo);
            //var callButton = uow.CallButtons.GetAll().SingleOrDefault(x => x.ButtonIndex == nodeNo);
            //if (callButton != null)
            //    JobCommandQueue.Enqueue(new JobCommand { Code = JobCommandCode.REMOVE, Text = callButton.ButtonName });
            //else
            //    MessageBox.Show($"call button {nodeNo} not found");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            JobCommandQueue.Enqueue(new JobCommand { Code = JobCommandCode.REMOVE_ALL });
        }

        private void button11_Click(object sender, EventArgs e)
        {
            main.jobStepFlag = true;
        }

        // POP CALL 추가
        private void button12_Click(object sender, EventArgs e)
        {
            //string popCallName = cbPopCallName.Text.Trim();
            //string popCallType = cbPopCallType.Text.Trim().StartsWith("회수") ? "Y" : "N";

            //var callButton = uow.CallButtons.GetAll().SingleOrDefault(x => x.ButtonName == popCallName);
            //if (callButton != null)
            //{
            //    string lineCD = popCallName.Split('_')[0];
            //    int postCD = Convert.ToInt32(popCallName.Split('_')[1]);

            //    using (var con = new SqlConnection(ConnectionStrings.DB1))
            //    {
            //        string INSERT_SQL = "";

            //        if (popCallType == "Y")
            //        {
            //            INSERT_SQL = $@"insert into POPSERVER_1 VALUES ('{lineCD}', {postCD}, 'N', 0,  'Y', NULL, 'N', NULL) "; // 공박스 회수미션
            //        }
            //        else
            //        {
            //            if (lineCD == "MC311" && postCD >= 14 && postCD <= 17)
            //                INSERT_SQL = $@"insert into POPSERVER_1 VALUES ('{lineCD}', {postCD}, 'Y', 92,  'N', NULL, 'N', NULL) "; // 자재 투입미션(각도Y)
            //            else
            //                INSERT_SQL = $@"insert into POPSERVER_1 VALUES ('{lineCD}', {postCD}, 'N', 0,  'N', NULL, 'N', NULL) "; // 자재 투입미션
            //        }

            //        con.ExecuteScalar<int>(INSERT_SQL);
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("pop call name input error!");
            //}
        }

        // POP CALL 취소
        // ... DB에서 삭제 못하므로
        // ... POP CALL로 생성된 JOB을 삭제하고, POP DB 처리완료로 변경한다
        private void button13_Click(object sender, EventArgs e)
        {
            //string popCallName = cbPopCallName.Text.Trim();

            //var callButton = uow.CallButtons.GetAll().SingleOrDefault(x => x.ButtonName == popCallName);
            //if (callButton != null)
            //{
            //    JobCommandQueue.Enqueue(new JobCommand { Code = JobCommandCode.REMOVE, Text = popCallName }); // JOB 삭제
            //}
            //else
            //    MessageBox.Show($"call button {popCallName} not found");
        }

        // POP CALL 여러개 추가
        private void button14_Click(object sender, EventArgs e)
        {
            using (var con = new SqlConnection(ConnectionStrings.DB1))
            {
                string INSERT_SQL = $@"
            insert into POPSERVER_1 VALUES ('MC110', 1, 'N', 0,  'N', NULL, 'N', NULL) 
            insert into POPSERVER_1 VALUES ('MC110', 3, 'N', 0,  'Y', NULL, 'N', NULL) 
            insert into POPSERVER_1 VALUES ('MC350', 1, 'N', 0,  'N', NULL, 'N', NULL) 
            insert into POPSERVER_1 VALUES ('MC350', 2, 'N', 0,  'Y', NULL, 'N', NULL) 

            insert into POPSERVER_3 VALUES ('MC111', 1, 'N', 0,  'N', NULL, 'N', NULL) 
            insert into POPSERVER_3 VALUES ('MC111', 2, 'N', 0,  'Y', NULL, 'N', NULL) 

            insert into POPSERVER_2 VALUES ('MC112', 1, 'N', 0,  'N', NULL, 'N', NULL) 
            insert into POPSERVER_2 VALUES ('MC112', 2, 'N', 0,  'Y', NULL, 'N', NULL) 
            insert into POPSERVER_2 VALUES ('MC311', 1, 'N', 0,  'N', NULL, 'N', NULL) 
            insert into POPSERVER_2 VALUES ('MC311', 2, 'N', 0,  'Y', NULL, 'N', NULL) 
            insert into POPSERVER_2 VALUES ('MC351', 1, 'N', 0,  'N', NULL, 'N', NULL) 
            insert into POPSERVER_2 VALUES ('MC351', 2, 'N', 0,  'Y', NULL, 'N', NULL) 

            insert into POPSERVER_4 VALUES ('MC310', 1, 'N', 0,  'N', NULL, 'N', NULL) 
            insert into POPSERVER_4 VALUES ('MC310', 4, 'N', 0,  'Y', NULL, 'N', NULL) 
            insert into POPSERVER_4 VALUES ('MC310', 6, 'Y', 88, 'N', NULL, 'N', NULL) 
";
                con.ExecuteScalar<int>(INSERT_SQL);
            }
        }

        // 창고 table flag Y로 변경
        private void button15_Click(object sender, EventArgs e)
        {
            // 해당 call에 해당하는 창고 항목(들)만 Y로 변경
            //var callName = cbPopCallName.Text.Trim();
            //var lineCD = callName.Split('_')[0];
            //var postCD = Convert.ToInt32(callName.Split('_')[1]);
            //foreach (var item in uow.WmsDB.GetAll_With_Flag_N().Where(x => x.LINE_CD == lineCD && x.POST_CD == postCD))
            //{
            //    int wmsOutPos = Convert.ToInt32(cbWmsOutPos.Text.Trim());
            //    item.OUT_POINT = wmsOutPos;
            //    item.WMS_IF_FLAG = "Y";
            //    item.MODIFY_DT = DateTime.Now;
            //    uow.WmsDB.Update(item);
            //}
            //var wmsItem = uow.WmsDB.GetById(job.WmsId);
            //if (wmsItem != null)
            //{
            //    wmsItem.WMS_IF_FLAG = "Y";
            //    uow.WmsDB.Update_WmsFlag(wmsItem);
            //}


            //// 모든 창고 항목 Y로 변경
            //foreach (var job in uow.Jobs.GetAll())
            //{
            //    var wmsItem = uow.WmsDB.GetById(job.WmsId);
            //    if (wmsItem != null)
            //    {
            //        wmsItem.WMS_IF_FLAG = "Y";
            //        uow.WmsDB.Update_WmsFlag(wmsItem);
            //    }
            //}

        }

        private void button17_Click(object sender, EventArgs e)
        {
            main.PopCallErrorMessageQueue.Enqueue("POP CALL ERROR 메시지 테스트");
        }

        private void btn_joblogInsert_Click(object sender, EventArgs e)
        {
            string CallButtonName = "M3F_T3F";
            string linecd = "";
            string postcd = "";
            string elapsedTime = "";
            linecd = CallButtonName.Split('_')[0];
            postcd = CallButtonName.Split('_')[1];
            TimeSpan elapsed = DateTime.Now.AddSeconds(61) - DateTime.Now;
            int elapsedTimeDay = elapsed.Days;
            int elapsedTimeHour = elapsed.Hours;
            int elapsedTimeMinute = elapsed.Minutes;
            int elapsedTimeSecond = elapsed.Seconds;
            if (elapsedTimeDay != 0) elapsedTime += elapsedTimeDay + "일";
            if (elapsedTimeHour != 0) elapsedTime += elapsedTimeHour + "시";
            if (elapsedTimeMinute != 0) elapsedTime += elapsedTimeMinute + "분";
            if (elapsedTimeSecond != 0) elapsedTime += elapsedTimeSecond + "초";

            // CALL 로그 (DB)
            using (var con = new SqlConnection(ConnectionStrings.DB1))
            {
                try
                {
                    var jobLog = new JobLog()
                    {
                        ResultCD = 1,

                        CallName = CallButtonName,
                        LineName = $"{linecd}",
                        PostName = $"{postcd}",
                        //CallType = job.PopCallReturnType,

                        //LineName = $"{GetLineNameByCallName(job.CallButtonName)}",
                        //PosName = $"{GetPosNameByCallName(job.CallButtonName)}",
                        //PartCD = $"{popSvc.GetPartInfo2(job)?.PART_CD}",
                        //PartNM = $"{popSvc.GetPartInfo2(job)?.PART_NM}",
                        //PartOutQ = uow.WmsDB.GetById(job.WmsId)?.OUT_Q,
                        //PartOutP = uow.WmsDB.GetById(job.WmsId)?.OUT_POINT,

                        RobotName = "1",
                        JobState = "1",

                        CallTime = DateTime.Now.AddDays(-(4)),
                        JobCreateTime = DateTime.Now.AddDays(-(4)),
                        JobFinishTime = DateTime.Now,

                        JobElapsedTime = $"{elapsedTime}",

                        //WmsId = job.WmsId,

                        MissionNames = "1",
                        MissionStates = "1",
                    };

                    const string INSERT_SQL = @"
                            INSERT INTO JobHistory VALUES 
                                    (@CallName
                                    ,@LineName
                                    ,@PostName
                                    ,@RobotName
                                    ,@JobState
                                    ,@CallTime
                                    ,@JobCreateTime
                                    ,@JobFinishTime
                                    ,@JobElapsedTime
                                    ,@MissionNames
                                    ,@MissionStates
                                    ,@ResultCD);";

                    con.ExecuteScalar(INSERT_SQL, param: jobLog);
                }
                catch (Exception)
                {
                    //Debug.WriteLine(ex.Message);
                    //EventLogger.Info(ex.Message);
                }
            }
        }

        private void ReelTower_Mode(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btn_ReelTower1_Mode":
                    break;
                case "btn_ReelTower2_Mode":
                    break;
                case "btn_ReelTower3_Mode":
                    break;
                case "btn_ReelTower4_Mode":
                    break;
                case "btn_ReelTower5_Mode":
                    break;

            }
        }

        private void ReelTower_Port(object sender, EventArgs e)
        {

        }

        private void ReelTower_Call(object sender, EventArgs e)
        {
            string button = ((Button)sender).Text;

            //CrevisDio crevisDio = (CrevisDio)CrevisDioManager.Instance.GetDevice("ASB");
            //bool[] Test = new bool[16];
            //for (int i = 0; i < 8; i++)
            //{
            //    Test[i] = true;
            //}


            switch (button)
            {
                case "ReelTower1_Call":
                    break;
                case "ReelTower2_Call":
                    break;
                case "ReelTower3_Call":
                    break;
                case "ReelTower4_Call":
                    break;
                case "ReelTower5_Call":
                    break;
                default:
                    break;
            }

            //var TabletCall = new TabletMissionStatusModel
            //{
            //    CALLNAME = "스테이션존1_홍삼청정",
            //    CALLFLAG = "wait",
            //    MESSIONSEQ = "0",
            //    REGDATE = DateTime.Now
            //};
            //uow.TabletMissionStatus.Add(TabletCall);

        }

        private void ReelTower_Cancel(object sender, EventArgs e)
        {

            string button = ((Button)sender).Text;

            //CrevisDio crevisDio = (CrevisDio)CrevisDioManager.Instance.GetDevice("ASB");


            //bool[] Test = new bool[16];
            //for (int i = 0; i < 8; i++)
            //{
            //    Test[i] = false;
            //}

            switch (button)
            {
                case "ReelTower1_Cancel":
                    break;
                case "ReelTower2_Cancel":
                    break;
                case "ReelTower3_Cancel":
                    break;
                case "ReelTower4_Cancel":
                    break;
                case "ReelTower5_Cancel":
                    break;
                default:
                    break;
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {

        }

        private void TowerCall_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string eqpName = btn.Text.Split(' ')[0];
            string eqpInch = btn.Text.Split(' ')[1];

            if (eqpInch != "7" && eqpInch != "13")
            {
                MessageBox.Show("muse be 7inch or 13inch !!");
                return;
            }


            // add to queue
            //EquipmentCallInfo eqpInfo = EquipmentCallInfoRepo.GetByEqpName(eqpName, eqpInch);
            //var callName = eqpInfo.CALL_NAME;
            //var groupName = eqpInfo.GROUP_NAME;
            //string targetRobotName = uow.Robots.GetAll().FirstOrDefault(x => x.ACSRobotGroup == groupName)?.RobotName;
            //JobCommandQueue.Enqueue(new JobCommand { Code = JobCommandCode.ADD, Text = callName, Extra4 = targetRobotName });


            //// add to db
            //uow.DBCalls.Add(new EquipmentOrder()
            //{
            //    EQP_NAME = eqpName,
            //    COMMAND = "CALL",
            //    INCH_TYPE = eqpInch,
            //    IF_FLAG = "N",
            //    CREATE_DT = DateTime.Now,
            //    MODIFY_DT = DateTime.Now,
            //});
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            string button = ((Button)sender).Text;
            switch (button)
            {
                case "TestReg11Value1":
                    break;
                case "TestReg12Value1":
                    break;
                case "TestReg13Value1":
                    break;
                case "TestReg14Value1":
                    break;
                case "TestReg23Value1":
                    break;
                case "TestReg24Value1":
                    break;
                case "TestReg11Value0":
                    break;
                case "TestReg12Value0":
                    break;
                case "TestReg13Value0":
                    break;
                case "TestReg14Value0":
                    break;
                case "TestReg23Value0":
                    break;
                case "TestReg24Value0":
                    break;
            }
        }

        private void PLCEntryFlag_Click(object sender, EventArgs e)
        {
            var ButtonName = ((Button)sender).Text;

        }
    }
}
