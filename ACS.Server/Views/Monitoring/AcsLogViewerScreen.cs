using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace INA_ACS_Server.OPWindows
{
    public partial class AcsLogViewerScreen : Form
    {
        private readonly MainForm mainForm;
        private readonly IUnitOfWork uow;

        public AcsLogViewerScreen(MainForm mainForm, IUnitOfWork uow)
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Init();
            txt_ACS_LogViewer.Font = new Font("Bitstream Vera Sans Mono", 8.0f);
            txt_ACS_LogViewer.WordWrap = true;
        }

        public void Init()
        {
        }



        #region Auto화면 Log표시

        public void subFunc_Client_Log_Display()
        {
            int sLineLimitValue = 150;

            // 로그뷰어에 새로운 메시지를 모두 추가한다
            while (mainForm.AcsLogMessageQueue.Count > 0)
            {
                if (mainForm.AcsLogMessageQueue.TryDequeue(out string msg))
                {
                    txt_ACS_LogViewer.AppendText(msg);
                    txt_ACS_LogViewer.AppendText(Environment.NewLine);
                }
            }

            // 로그뷰어의 최대 라인수를 제한한다
            if (txt_ACS_LogViewer.Lines.Length > sLineLimitValue)
            {
                LinkedList<string> tempsLines = new LinkedList<string>(txt_ACS_LogViewer.Lines);

                while ((tempsLines.Count - sLineLimitValue) > 0)
                {
                    tempsLines.RemoveFirst();
                }
                txt_ACS_LogViewer.Lines = tempsLines.ToArray();
            }

            // 로그뷰어를 마지막 라인으로 스크롤한다
            txt_ACS_LogViewer.SelectionStart = txt_ACS_LogViewer.TextLength;
            txt_ACS_LogViewer.ScrollToCaret();
            txt_ACS_LogViewer.Update();
        }

        #endregion


        #region Timer

        private void AutoDisplay_Timer_Tick(object sender, EventArgs e)
        {
            AutoDisplay_Timer.Enabled = false;

            subFunc_Client_Log_Display();

            AutoDisplay_Timer.Enabled = true;
        }
        #endregion

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

    }

}
