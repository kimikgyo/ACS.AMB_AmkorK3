using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using ACS.RobotApi;
using INA_ACS_Server;

namespace ACS.RobotMap
{
    public partial class UCMapView : UserControl
    {
        private readonly static ILog EventLogger = LogManager.GetLogger("Event"); //Function 실행관련 Log
        private readonly static object lockObj = new object();

        private IFleetApi _fleetApi;
        private MapReadDtoQueue<MapReadDto> _queue;
        private MapRender _mapRender = new MapRender();
        private bool bMapLoaded = false;
        private Image backgroundImage = null;
        private float mapScale = 1.0f;

        // map 관련 속성
        public string MapID { get; set; }
        public string MapName { get; set; }
        public FleetMap Map { get; set; }
        public List<int> RobotIdList { get; private set; }
        public List<FleetRobot> RobotInfoList { get; set; }
        public Image mapImageFromDB = null;    // db map image

        // mouse 관련 변수
        private Point mouseFirstLocation = Point.Empty;
        private Point mouseMoveOffset = Point.Empty;

        // custom map 관련 변수
        private bool customMapMode = false;      // map mode (0=fleet / 1=custom)
        private bool needMapUpdate = true;      // db map image를 다시 로드해야 할때 true. (기동시에는 무조건 로드해야하므로 true)
        private DateTime oldDbUpdatedTime = default; // keep map UpdateDime
        // ...
        private MonitorConfigData monitorConfig;

        public UCMapView()
        {
            InitializeComponent();
        }

        public void InitConfig(MonitorConfigData monitorConfig)
        {
            this.monitorConfig = monitorConfig;
        }

        public void Init(IFleetApi apiClient, MapReadDtoQueue<MapReadDto> queue,
            string mapID, string mapName, float mapScale = 1.0f, Point mouseFirstLocation = default, Point mouseMoveOffset = default)
        {
            this._fleetApi = apiClient;
            this._queue = queue;
            this.MapID = mapID;
            this.MapName = mapName;
            this.mapScale = mapScale;
            this.mouseFirstLocation = mouseFirstLocation;
            this.mouseMoveOffset = mouseMoveOffset;

            this.BackColor = Color.FromArgb(43, 52, 59); //Color.White;

            // 맵 표시 픽처박스 초기화
            pictureBox1.MouseClick += PictureBox1_MouseClick;
            pictureBox1.MouseWheel += PictureBox1_MouseWheel;
            pictureBox1.MouseDown += PictureBox1_MouseDown;
            pictureBox1.MouseMove += PictureBox1_MouseMove;

            pictureBox1.SizeMode = PictureBoxSizeMode.Normal; // 변경 금지!!
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Resize += PictureBox1_Resize;
            if (backgroundImage == null) PictureBox1_Resize(this, null);
            pictureBox1.Visible = true;

            // custom map 체크박스 설정
            chkCustomMap.CheckState = customMapMode ? CheckState.Checked : CheckState.Unchecked;
            chkCustomMap.Click += (s, e) =>
            {
                customMapMode = chkCustomMap.Checked;
                needMapUpdate = chkCustomMap.Checked;
            };

            chkCustomMap.Visible = false;
            // display info 체크박스 설정
            //cb_DisplayInfo.Click += (s, e) => btnMapDownload.Visible = cb_DisplayInfo.Checked;
        }


        // 픽쳐박스 사이즈변경시 배경 이미지 크기 재설정
        private void PictureBox1_Resize(object sender, EventArgs e)
        {
            if (backgroundImage != null)
            {
                backgroundImage.Dispose();
                backgroundImage = null;
            }

            if (pictureBox1.ClientSize.Width <= 0 || pictureBox1.ClientSize.Height <= 0)
                return;


            //// 배경 이미지 설정
            ////backgroundImage = Image.FromFile("layout.png"); // 화일에서 불러온 이미지 사용
            ////backgroundImage = (Image)pictureBox1.Image.Clone(); // 폼에 설정된 이미지 복제해서 사용
            //backgroundImage = Resources._70_Auto; // 폼에 설정된 이미지 복제해서 사용

            //// 배경 이미지 투명도 설정
            ////((Bitmap)backgroundImage).MakeTransparent();
            //backgroundImage = ImageUtils.ImageTransparency.ChangeOpacity(backgroundImage, 0.3f);

            //// 배경 이미지 크기 조정
            //backgroundImage = ImageUtils.ImageTransparency.ResizeImage(backgroundImage, pictureBox1.ClientSize);
        }



        private Task task1;
        private Task task2;

        private bool stopRequest = false;


        public void StopLoop()
        {
            stopRequest = true;
            Task.WaitAll(task2);
        }


        public void StartLoop()
        {
            stopRequest = false;

            // ==================== 렌더링 스레드
            task2 = Task.Run(async () =>
            {
                EventLogger.Info($"mapview rendering thread start! ({MapName})");
                var pre_mouseFirstLocation = Point.Empty;
                var pre_mapScale = 0.0f;
                var sw = new Stopwatch();
                sw.Start();

                while (!stopRequest)
                {
                    // 주기적으로 렌더링 처리, 맵조작시에는 바로 렌더링 처리
                    if (sw.ElapsedMilliseconds > 500 || pre_mouseFirstLocation != mouseFirstLocation || pre_mapScale != mapScale)
                    {
                        pre_mouseFirstLocation = mouseFirstLocation;
                        pre_mapScale = mapScale;
                        sw.Reset();
                        sw.Start();

                        //GetMapData();
                        MapHandlerProc2();
                    }
                    await Task.Delay(10);
                }
                EventLogger.Info($"mapview rendering thread stop! ({MapName})");
            });
        }


        // 큐에서 맵 데이터 가져온다
        private void GetMapData()
        {
            // 데이터가 여러개인 경우, 루프를 반복해서 마지막 데이터만 사용한다
            while (_queue.TryDequeue(out MapReadDto item))
            {
                // 아이템내 맵 데이터는 변경되었을때만 할당한다
                if (object.ReferenceEquals(this.Map, item.Map) == false)
                {
                    this.Map = item.Map;
                    EventLogger.Info($"GetMapData() : map data changed! ({MapName})");
                }

                // 아이템내 로봇상태 데이터는 항상 할당한다
                this.RobotInfoList = item.RobotInfo;
            }
        }


        private void MapHandlerProc2()
        {
            if (this.Disposing || this.IsDisposed) return;  // 폼이 폐기되었다
            if (this.Map == null) return;                   // 맵 데이터가 없다
            if (this.RobotInfoList == null) return;       // 로봇상태 데이터가 없다

            Bitmap renderImage = null;

            try
            {
                // ================ 렌더링 이미지를 만든다
                // 맵을 픽쳐박스사이즈로 스케일링하여 오프셋만큼 이동시켜 렌더링한 이미지 생성
                lock (lockObj)
                {
                    if (customMapMode) // db image mode
                    {
                        renderImage = _mapRender.GetRenderImage(pictureBox1.ClientSize, mouseMoveOffset, Map, mapImageFromDB, RobotInfoList, ConfigData.FleetPositions, mapScale, mapScale);
                    }
                    else // fleet image mode
                        renderImage = _mapRender.GetRenderImage(pictureBox1.ClientSize, mouseMoveOffset, Map, Map.Image, RobotInfoList, ConfigData.FleetPositions, mapScale, mapScale);
                }

                // 렌더링된 이미지 투명 설정
                //renderImage.MakeTransparent(Color.FromArgb(252, 254, 252)); // 맵 내부 투명하게
                renderImage.MakeTransparent(Color.Gray); // 맵 외부 투명하게

                // ================ 렌더링 이미지를 ui 에 반영시킨다
                if (this.Disposing || this.IsDisposed) return;

                // ===================== A. 맵에 추가정보 오버레이
                if (true)
                {
                    #region 미사용코드
                    //// 배경 이미지 설정 (변하지 않으므로 한번만 설정)
                    //if (backgroundImage == null)
                    //{
                    //    //backgroundImage = Image.FromFile("layout.png"); // 화일에서 불러온 이미지 사용
                    //    backgroundImage = (Image)pictureBox1.Image.Clone(); // 폼에 설정된 이미지 복제해서 사용

                    //    //((Bitmap)backgroundImage).MakeTransparent();
                    //    backgroundImage = ImageUtils.ImageTransparency.ChangeOpacity(backgroundImage, 0.3f);
                    //}

                    //// 배경 이미지 크기 조정
                    //if (backgroundImage != null && backgroundImage.Size != pictureBox1.Image.Size)
                    //{
                    //    Image resizedBackgroundImage = ImageUtils.ImageTransparency.ResizeImage(backgroundImage, pictureBox1.ClientSize);
                    //    backgroundImage.Dispose();
                    //    backgroundImage = null;
                    //    backgroundImage = resizedBackgroundImage;
                    //}
                    #endregion

                    // 배경 이미지 위에 렌더링된 맵이미지를 오버레이한다
                    //Image img = new Bitmap(backgroundImage.Width, backgroundImage.Height);
                    Image renderImageWithDataOverlay = new Bitmap(pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height);
                    using (Graphics g = Graphics.FromImage(renderImageWithDataOverlay))
                    {
                        g.DrawImage(renderImage, 0, 0);

                        //============================================= 로봇 정보 오버레이
                        if (cb_DisplayInfo.Checked)
                        {
                            using (Font textFont = new Font("Courier New", 10, FontStyle.Bold))
                            {
                                int y = 3;
                                Brush stateTextBrush;
                                foreach (var r in RobotInfoList)
                                {
                                    y += 15; g.DrawString($"Robot     : {r.RobotName}", textFont, Brushes.Blue, 0, y);
                                    if (r.StateText.ToLower().Contains("abort")) stateTextBrush = Brushes.Red;
                                    else if (r.StateText.ToLower().Contains("pause")) stateTextBrush = Brushes.Violet;
                                    else if (r.StateText.ToLower().Contains("manual")) stateTextBrush = Brushes.Red;
                                    else if (r.StateText.ToLower().Contains("error")) stateTextBrush = Brushes.Red;
                                    else if (r.StateText.ToLower().Contains("emergency")) stateTextBrush = Brushes.Red;
                                    //else if (r.FleetStateText.ToLower().Contains("unavailable")) stateTextBrush = Brushes.Red;
                                    else stateTextBrush = Brushes.Blue;
                                    y += 15; g.DrawString($"State     : {r.StateText}({r.StateID})", textFont, stateTextBrush, 0, y);
                                    //y += 15; g.DrawString($"FleetState: {r.FleetStateText}({r.FleetState})", textFont, stateTextBrush, 0, y);
                                    y += 15; g.DrawString($"QueueID   : {r.MissionQueueID}", textFont, Brushes.Blue, 0, y);
                                    y += 15; g.DrawString($"Mission   : {r.MissionText}", textFont, Brushes.Blue, 0, y);
                                    y += 15; g.DrawString($"Battery   : {r.BatteryPercent:0.00}%", textFont, Brushes.Blue, 0, y);
                                    y += 30;
                                }
                            }
                        }
                        //=============================================
                    }

                    // UI 처리 (배경이미지 위에 맵 이미지를 오버레이한 이미지 표시)
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            this.pictureBox1.Image?.Dispose();
                            this.pictureBox1.Image = renderImageWithDataOverlay;
                            this.pictureBox1.Refresh();

                        }));
                    }
                }
                // ===================== B. 맵 only
                else
                {
                    //UI 처리 (맵 이미지 그대로 표시)
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            this.pictureBox1.Image?.Dispose();
                            this.pictureBox1.Image = renderImage;
                            this.pictureBox1.Refresh();
                        }));
                    }
                }
            }
            catch (ObjectDisposedException) { }
            catch (Exception ex)
            {
                var msg = ex.GetFullMessage() + Environment.NewLine + ex.StackTrace;
                Debug.WriteLine(msg);
                EventLogger.Info(msg);
            }
        }


        // DB에서 이미지 데이터 가져와서 이미지 생성한다
        private Image GetMapImageFromDB()
        {
            try
            {
                // 새로운 이미지 생성한다 (DB에서 가져온 이미지)
                string imageData = CustomMaps.GetMapImageData(MapName);
                Image image = CustomMaps.ConvertEncodedStringToImage(imageData);
                EventLogger.Info($"map image loaded! ({MapName})");
                return image;
            }
            catch (Exception ex) { EventLogger.Info($"_GetImageFromDB() Exception: MapName={MapName}     {ex}"); }
            return null;
        }


        // 마우스 클릭으로 로봇 위치 확인하기
        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (RobotInfoList == null) return;
            if (Map == null) return;

            var sb = new StringBuilder();

            Point clickPoint = e.Location;
            PointF scaledClickPoint = _mapRender.GetScaledMapPoint(mouseMoveOffset, Map, clickPoint, mapScale, mapScale);


            var posList = Map.Positions.Where(p =>
            {
                return
                    p.PosX > (scaledClickPoint.X - .6) &&
                    p.PosX < (scaledClickPoint.X + .6) &&
                    p.PosY > (scaledClickPoint.Y - .6) &&
                    p.PosY < (scaledClickPoint.Y + .6);
            });


            foreach (var pos in posList)
                sb.AppendLine($"{clickPoint}  mapXY={scaledClickPoint}   type={pos.TypeID,-2}    pos={pos.Name}");


            var robotList = RobotInfoList.Where(r =>
            {
                return
                    r.PosX > (scaledClickPoint.X - .6) &&
                    r.PosX < (scaledClickPoint.X + .6) &&
                    r.PosY > (scaledClickPoint.Y - .6) &&
                    r.PosY < (scaledClickPoint.Y + .6);
            });


            foreach (var robot in robotList)
                sb.AppendLine($"{clickPoint}  mapXY={scaledClickPoint}  robot={robot.RobotName}");


            if (posList.Count() == 0 && robotList.Count() == 0)
                sb.AppendLine($"{clickPoint}  mapXY={scaledClickPoint}");


            Debug.Write(sb.ToString());

            lbl_ClickPosInfo.Text = sb.ToString();
            lbl_ClickPosInfo.Visible = cb_DisplayInfo.Checked;


            Debug.WriteLine($"mouse_offset={this.mouseMoveOffset}");
        }


        // 마우스 휠로 맵을 줌인/줌아웃하기
        private void PictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            // The amount by which we adjust scale per wheel click.
            //const float scale_per_delta = 0.1f / 120;
            //const float scale_per_delta = 0.1f / 240;
            const float scale_per_delta = 0.1f / 720;

            // Update the drawing based upon the mouse wheel scrolling.
            mapScale += e.Delta * scale_per_delta;

            if (mapScale < 0.1f) mapScale = 0.1f;
            if (mapScale > 4.0f) mapScale = 4.0f;

            // Display the new scale.
            Debug.WriteLine(mapScale.ToString("p0"));
        }


        // 마우스 Down/Move로 맵을 이동시키기
        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseFirstLocation = e.Location; //Control.MousePosition;
            }
        }


        // 마우스 Down/Move로 맵을 이동시키기
        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mouseCurrentLocation = e.Location; //Control.MousePosition;

                Point deltaPoint = new Point(
                    mouseFirstLocation.X - mouseCurrentLocation.X,
                    mouseFirstLocation.Y - mouseCurrentLocation.Y);

                mouseMoveOffset.X -= deltaPoint.X;
                mouseMoveOffset.Y -= deltaPoint.Y;

                mouseFirstLocation = mouseCurrentLocation;
            }
        }






        private void btnMapDownload_Click(object sender, EventArgs e)
        {
            if (Map == null || Map.Image == null)
                return;

            var dlg = new SaveFileDialog { Title = "save image", DefaultExt = "png", Filter = "PNG File(*.png)|*.png" };
            dlg.FileName = $"fleet_map_{MapName}";
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                btnMapDownload.Enabled = false;
                try
                {
                    Image image = (Image)Map.Image.Clone();
                    image.Save(dlg.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    EventLogger.Info($"map downloaded ({MapName})");
                }
                catch (Exception ex)
                {
                    EventLogger.Info($"{ex} ({MapName})");
                }
                btnMapDownload.Enabled = true;
            }
        }

        private void btnMapReload_Click(object sender, EventArgs e)
        {
            //btnMapReload.Enabled = false;

            // reload map image
            StopLoop();
            bMapLoaded = false;
            needMapUpdate = true;
            StartLoop();

            //btnMapReload.Enabled = true;
        }

    }
}
