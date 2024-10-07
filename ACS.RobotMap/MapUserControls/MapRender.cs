using INA_ACS_Server;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.RobotMap
{
    internal class MapRender
    {
        private readonly static object lockObj = new object();
        private readonly Pen borderPen1 = new Pen(Color.Red, -1);
        private readonly Pen borderPen2 = new Pen(Color.Blue, -1);
        private readonly Pen borderPen3 = new Pen(Color.Green, -1);
        private readonly Pen arrowPen1 = new Pen(Color.Chocolate, 5.0f);
        private readonly Pen arrowPen2 = new Pen(Color.SteelBlue, 5.0f) { EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor };
        private readonly Font font1 = new Font("맑은 고딕", 10, FontStyle.Bold);
        private readonly Font font2 = new Font("Calibri", 8, FontStyle.Regular);
        private readonly Font textFont1 = new Font("Calibri", 18, FontStyle.Bold);
        private PointF RobotcenterPoint = new PointF();
        private PointF ImagePosCenterPoint = new PointF();

        private static string imagePositionPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image", "POS16.png");
        Image imagePosition = Image.FromFile(imagePositionPath);

        public Bitmap GetRenderImage(Size drawingAreaSize, Point moveOffset, FleetMap map, IList<FleetRobot> robots, IList<FleetPositionModel> fleetPositionsDB, float scaleFactorH = 1.0F, float scaleFactorV = 1.0F)
            => GetRenderImage(drawingAreaSize, moveOffset, map, map.Image, robots, fleetPositionsDB, scaleFactorH, scaleFactorV);


        public Bitmap GetRenderImage(Size drawingAreaSize, Point moveOffset, FleetMap map, Image mapImage, IList<FleetRobot> robots, IList<FleetPositionModel> fleetPositionsDB, float scaleFactorH = 1.0F, float scaleFactorV = 1.0F)
        {
            // no image
            if (mapImage == null)
            {
                return GetRenderImage_Empty();
            }


            // create bitmap (for double-buffering)
            Bitmap bitmap = new Bitmap(drawingAreaSize.Width, drawingAreaSize.Height);

            lock (lockObj)
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    g.InterpolationMode = InterpolationMode.High;

                    // map 변환
                    float dx = moveOffset.X;
                    float dy = moveOffset.Y;
                    g.TranslateTransform(dx, dy); // map을 dx,dy만큼 이동시킨다
                    g.ScaleTransform(scaleFactorH, scaleFactorV); // map을 스케일링 한다

                    // draw all
                    g.Clear(Color.Gray);

                    DrawMap(g, map, mapImage);
                    DrawPositions(g, map);

                    if (mapImage != null && fleetPositionsDB != null)
                    {
                        if (robots != null)
                        {
                            foreach (var robot in robots)
                            {
                                if (robot.FleetState != INA_ACS_Server.FleetState.unavailable && map.Guid == robot.MapID)
                                    DBImageDrawRobot(g, mapImage, robot);

                                if (robot.StateText == "Executing" && robot.MissionText.Contains("Moving to")
                                    && map.Guid == robot.MapID)
                                {
                                    foreach (var position in fleetPositionsDB)
                                    {
                                        string MissionText = $"Moving to '{position.Name}'";

                                        if (robot.MissionText.StartsWith(MissionText))
                                        {
                                            //이름으로 포지션 그리기
                                            DBImageDrawPositions(g, fleetPositionsDB, mapImage, position.Name);
                                            Draw점선그리기(g, RobotcenterPoint, ImagePosCenterPoint);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    g.ResetTransform();
                }
            }

            return bitmap;
        }


        // 빈 렌더링 이미지 생성
        private readonly Bitmap blankBitmap = new Bitmap(2000, 2000);
        private Bitmap GetRenderImage_Empty()
        {
            Bitmap renderImage = blankBitmap;

            using (Graphics g = Graphics.FromImage(blankBitmap))
            using (Font textFont = new Font("Courier New", 20))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                g.Clear(Color.Gray);
                g.DrawString("NO IMAGE", textFont, Brushes.Red, 50, 50);
            }

            return renderImage;
        }
        private void DBImageDrawPositions(Graphics g, IList<FleetPositionModel> fleetPositions, Image DataBaseImage/*, FloorMapIdConfigModel floorMapIdConfigs*/, string PositionName)
        {
            foreach (var pos in fleetPositions.Where(x => x.Name == PositionName))
            {
                Sub_DBImageDrawPosition(g, pos, DataBaseImage);
                //Console.WriteLine($"{MapName} / {pos.Name}");
            }
        }

        private void Sub_DBImageDrawPosition(Graphics g, FleetPositionModel pos, Image DataBaseImage)
        {

            //float radius = 9.5f; //7.5f;
            //float halfSize = 9.5f; //7.5f;
            float radius = 11f; //7.5f;
            float halfSize = 11f; //7.5f;
            float Resolution = 0.05f;

            float x = (float)pos.PosX / (float)Resolution;
            float y = (float)pos.PosY / (float)Resolution;
            float theta = (float)pos.Orientation;

            y = DataBaseImage.Height - y;

            // point1 (center)
            var centerPoint = new PointF(x, y);

            int imgeCenterPointX = (int)centerPoint.X - 7;
            int imgeCenterPointY = (int)centerPoint.Y - 10;
            g.DrawImage(imagePosition, imgeCenterPointX, imgeCenterPointY);

            int 화살표포지션센터X = (int)imgeCenterPointX + 10;
            int 화살표포지션센터Y = (int)imgeCenterPointY + 10;

            ImagePosCenterPoint = new PointF(화살표포지션센터X, 화살표포지션센터Y);

            // point2 (direction)
            var cosX = radius * (float)Math.Cos(-theta / 180 * Math.PI);
            var sinY = radius * (float)Math.Sin(-theta / 180 * Math.PI);
            var arrowPoint1 = new PointF(centerPoint.X + cosX * 0.65f, centerPoint.Y + sinY * 0.65f);
            var arrowPoint2 = new PointF(centerPoint.X + cosX, centerPoint.Y + sinY);


            //// draw pos (circle) ==============
            var rt1 = new RectangleF((int)(centerPoint.X - halfSize), (int)(centerPoint.Y - halfSize), (int)halfSize * 2, (int)halfSize * 2);
            //g.FillEllipse(Brushes.Orange, rt1);
            //g.DrawEllipse(borderPen1, rt1);
            ////g.DrawLine(arrowPen1, arrowPoint1, arrowPoint2); // arrow
            //g.FillEllipse(Brushes.Chocolate, new RectangleF(arrowPoint1.X - 2.5f, arrowPoint1.Y - 2.5f, 5.0f, 5.0f));

            // draw pos info
            var PositionName = pos.Name;
            rt1.Inflate(10, -10);
            //g.DrawString(PositionName, font2, Brushes.GreenYellow, rt1);
            g.DrawString(PositionName, font2, Brushes.Black, rt1.X, rt1.Y);
        }

        private void Draw점선그리기(Graphics g, PointF robotCenter, PointF POSCenter)
        {
            if (robotCenter == null || POSCenter == null) return;

            float[] dashPattern = { 10, 2 }; // 점선 패턴 (10픽셀 선, 2픽셀 간격)
            DrawDashedArrow(g, robotCenter, POSCenter, Color.Blue, dashPattern);
        }

        private void DrawDashedArrow(Graphics g, PointF start, PointF end, Color color, float[] dashPattern)
        {

            Pen pen = new Pen(Color.GreenYellow);
            // 점선 스타일 설정
            pen.DashStyle = DashStyle.Dot;

            // 점선 간격 설정
            pen.DashPattern = dashPattern;

            // 점선을 그리기
            g.DrawLine(pen, start, end);

            // 화살표 끝 그리기
            DrawArrowHead(g, pen, start, end);

            // 자원 해제
            pen.Dispose();
        }

        private void DrawArrowHead(Graphics g, Pen pen, PointF start, PointF end)
        {
            // 화살표 방향 계산
            float angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
            float arrowLength = 10; // 화살표 길이
            float arrowAngle = (float)(Math.PI / 6); // 화살표 각도

            // 화살표 끝 점 계산
            PointF p1 = new PointF(end.X - arrowLength * (float)Math.Cos(angle - arrowAngle),
                                    end.Y - arrowLength * (float)Math.Sin(angle - arrowAngle));
            PointF p2 = new PointF(end.X - arrowLength * (float)Math.Cos(angle + arrowAngle),
                                    end.Y - arrowLength * (float)Math.Sin(angle + arrowAngle));

            // 화살표 끝 그리기
            g.DrawLine(pen, end, p1);
            g.DrawLine(pen, end, p2);


        }

        public PointF GetScaledMapPoint(Point moveOffset, FleetMap map, PointF point, float scaleFactorH = 1.0F, float scaleFactorV = 1.0F)
        {
            if (map == null) return Point.Empty;

            float dx = moveOffset.X;
            float dy = moveOffset.Y;

            // map에 맞게 point값을 변환한다
            PointF convertedPoint = point;
            convertedPoint.X -= (int)dx;
            convertedPoint.Y -= (int)dy;
            convertedPoint.X = (int)(convertedPoint.X / scaleFactorH);
            convertedPoint.Y = (int)(convertedPoint.Y / scaleFactorV);

            // 변환한 point값으로 map에서의 좌표값을 구한다
            PointF mappingPoint = GetMapPoint(map, convertedPoint);

            //Console.WriteLine($"GetScaledMapPoint = {mappingPoint.X,-6:0.00}, {mappingPoint.Y,-6:0.00}");

            return mappingPoint;
        }


        private PointF GetMapPoint(FleetMap map, PointF point)
        {
            lock (lockObj)
            {
                float x = point.X;
                float y = point.Y;

                y = map.Image.Height - y;

                x *= (float)map.Resolution;
                y *= (float)map.Resolution;

                return new PointF(x, y);
            }
        }


        private void DrawMap(Graphics g, FleetMap map, Image image)
        {
            //if (map.Name == "B1F")
            //    g.DrawImage(map.Image, 30, 72);
            //else if (map.Name == "1F")
            //    g.DrawImage(map.Image, -12, 30);
            //else if (map.Name == "2F")
            //    g.DrawImage(map.Image, 18, -40);
            //else
            //    g.DrawImage(map.Image, 0, 0);

            if (map.Name == "B1F")
                g.DrawImage(image, 30, 72);
            else if (map.Name == "1F")
                g.DrawImage(image, -12, 30);
            else if (map.Name == "2F")
                g.DrawImage(image, 18, -40);
            else
                g.DrawImage(image, 0, 0);
        }

        private void DrawPositions(Graphics g, FleetMap map)
        {
            //var robot63_positions = map.Positions.Where(p => p.Name.StartsWith("L_63_")); // 63번 로봇 포지션

            //foreach (var pos in map.Positions.Except(robot63_positions)) // 62,63번 로봇 포지션이 중첩되어서, 63번 로봇 포지션은 제외한다
            foreach (var pos in map.Positions) // 62,63번 로봇 포지션이 중첩되어서, 63번 로봇 포지션은 제외한다
            {
                // 0: moving pos (way point?)

                // 1: target pos (cart?)   로봇 MAP 에서 보이는 초록색 포지션?

                // 2: cart pos
                // 3: cart entry pos1
                // 4: cart entry pos2

                // 7: charge pos?
                // 8: charge entry pos?

                // 11: VL marker pos
                // 12: VL marker entry pos

                // 13: L marker pos
                // 14: L marker entry pos
                // .....

                // 22: lift rack pos

                // ** position type 의 정확한 구분을 모르겠다...
                // ** position name 으로 필요한 것만 필터링하는것이 나을 듯...


                //if (pos.TypeID == "1" || pos.TypeID == "22")
                //{
                //    DrawPosition(g, map, pos);
                //}

                if (pos.Name.Contains("Area")) //$$$$$
                {
                    //DrawArea(g, map, pos);
                }

                //else if (pos.TypeID == "20" && pos.Name.ToUpper().Contains("CHARGE"))
                else if (pos.TypeID == "7" && pos.Name.ToUpper().Contains("CHARGE"))
                {
                    DrawCharger(g, map, pos);
                }
                //Position Type : Emergency position 이나 Position Name : ACS 인것
                else if (pos.TypeID == "15" || pos.Name.Contains("ACS"))
                {
                    //pos.PosX = pos.PosX - 35.0f;
                    //pos.PosY = pos.PosY + 80.0f;
                    //DrawPosition(g, map, pos);
                }

            }
        }


        private void DrawPoint(Graphics g, FleetMap map, PointF point)
        {
            //float radius = 9.5f; //7.5f;
            //float halfSize = 9.5f; //7.5f;
            float radius = 11f; //7.5f;
            float halfSize = 11f; //7.5f;

            float x = point.X / (float)map.Resolution;
            float y = point.Y / (float)map.Resolution;

            y = map.Image.Height - y;

            // point1 (center)
            var mapPoint = new PointF(x, y);

            var mapPoints = new PointF[1];
            mapPoints[0] = mapPoint;

            // draw
            g.DrawPolygon(borderPen1, mapPoints);
        }


        private void DrawPosition(Graphics g, FleetMap map, FleetPosition pos)
        {
            //float radius = 9.5f; //7.5f;
            //float halfSize = 9.5f; //7.5f;
            float radius = 11f; //7.5f;
            float halfSize = 11f; //7.5f;

            float x = ((float)pos.PosX / (float)map.Resolution);
            float y = ((float)pos.PosY / (float)map.Resolution);
            float theta = (float)pos.Orientation;

            y = map.Image.Height - y;

            // point1 (center)
            var centerPoint = new PointF(x, y);

            // point2 (direction)
            var cosX = radius * (float)Math.Cos(-theta / 180 * Math.PI);
            var sinY = radius * (float)Math.Sin(-theta / 180 * Math.PI);
            var arrowPoint1 = new PointF(centerPoint.X + cosX * 0.65f, centerPoint.Y + sinY * 0.65f);
            var arrowPoint2 = new PointF(centerPoint.X + cosX, centerPoint.Y + sinY);


            //// draw pos (circle) ==============
            var rt1 = new RectangleF((int)(centerPoint.X - halfSize), (int)(centerPoint.Y - halfSize), (int)halfSize * 2, (int)halfSize * 2);
            g.FillEllipse(Brushes.Orange, rt1);
            g.DrawEllipse(borderPen1, rt1);
            //g.DrawLine(arrowPen1, arrowPoint1, arrowPoint2); // arrow
            g.FillEllipse(Brushes.Chocolate, new RectangleF(arrowPoint1.X - 2.5f, arrowPoint1.Y - 2.5f, 5.0f, 5.0f));

            //// draw pos info
            ////var robotInfo = "pos:" + pos.Name;
            //var robotInfo = pos.Name;
            //var textPoint = centerPoint - new Size(10, 10);
            //g.DrawLine(Pens.Black, centerPoint, textPoint);
            //g.DrawString(robotInfo, font2, Brushes.Magenta, textPoint + new Size(-16, -16));

            //// draw pos info
            //var robotInfo = pos.Name;
            //rt1.Inflate(-7, -3);
            ////g.DrawString(robotInfo, font2, Brushes.Magenta, rt1);
            //g.DrawString(robotInfo, font2, Brushes.Magenta, rt1.X, rt1.Y);
            ////g.DrawString(pos.TypeID, font2, Brushes.Magenta, rt1.X,rt1.Y);  // pos type id

            // draw robot info
            var posInfo = pos.Name;
            var textPoint = centerPoint - new Size(20, 20);
            g.DrawLine(Pens.Black, centerPoint, textPoint);
            g.DrawString(posInfo, font1, Brushes.Magenta, textPoint + new Size(-16, -16));

            //// draw pos (rectangle) ==============
            //var rt2 = new Rectangle((int)(centerPoint.X - halfSize), (int)(centerPoint.Y - halfSize), (int)halfSize * 2, (int)halfSize * 2);
            //g.FillRectangle(Brushes.Orange, rt2);
            //g.DrawRectangle(borderPen1, rt2);

            //// draw pos info
            //var robotInfo = pos.Name;
            //rt2.Inflate(-3, 0);
            //g.DrawString(robotInfo, font2, Brushes.Magenta, rt2);
        }

        private void DrawArea(Graphics g, FleetMap map, FleetPosition pos)
        {
            float halfSizeX = 20.0f;
            float halfSizeY = 20.0f;

            float x = (float)pos.PosX / (float)map.Resolution;
            float y = (float)pos.PosY / (float)map.Resolution;

            y = map.Image.Height - y;

            var centerPoint = new PointF(x, y);

            var robotRect = new RectangleF(centerPoint.X - halfSizeX, centerPoint.Y - halfSizeY, halfSizeX * 2, halfSizeY * 2);
            var cornerRadius = 4.0f;

            using (GraphicsPath robotPath = MapRenderHelper.GetRoundedRectanglePath(robotRect, cornerRadius, cornerRadius))
            {
                g.DrawPath(borderPen3, robotPath);
            }
        }

        private void DBImageDrawRobot(Graphics g, Image DataBaseImage, FleetRobot robot)
        {

            float x = 0;
            float y = 0;
            float Resolution = 0.05f;
            float halfSizeX = 12.0f;
            float halfSizeY = 8.0f;

            x = (float)robot.PosX / (float)Resolution;
            y = (float)robot.PosY / (float)Resolution;

            y = DataBaseImage.Height - y;


            float theta = -(float)robot.Position_Orientation;

            // point1 (center)
            var centerPoint = new PointF(x, y);
            RobotcenterPoint = centerPoint;


            // 좌표계 회전 변환
            Matrix matrix = g.Transform;

            matrix.RotateAt(theta, centerPoint);
            g.Transform = matrix;

            // make robot path
            var robotRect = new RectangleF(centerPoint.X - halfSizeX, centerPoint.Y - halfSizeY, halfSizeX * 2, halfSizeY * 2);

            var cornerRadius = 4.0f;

            using (GraphicsPath robotPath = MapRenderHelper.GetRoundedRectanglePath(robotRect, cornerRadius, cornerRadius))
            {
                // draw robot
                g.FillPath(Brushes.DarkOrange, robotPath);
                g.DrawPath(borderPen2, robotPath);
                g.DrawLine(arrowPen2, centerPoint, centerPoint + new SizeF(halfSizeX, 0)); // arrow

            }

            // 좌표계 회전 복구
            matrix.RotateAt(-theta, centerPoint);
            g.Transform = matrix;

            // draw robot info
            var robotInfo = robot.RobotName;
            var textPoint = centerPoint - new Size(20, 20);


            g.DrawString(robotInfo, textFont1, Brushes.Magenta, textPoint + new Size(40, -10));

            //// 좌표계 회전 복구
            //matrix.RotateAt(-theta, centerPoint);
            //g.Transform = matrix;
        }

        private void DrawRobot(Graphics g, FleetMap map, FleetRobot robot)
        {
            float halfSizeX = 12.0f;
            float halfSizeY = 8.0f;

            float x = ((float)robot.PosX / (float)map.Resolution);
            float y = ((float)robot.PosY / (float)map.Resolution);
            float theta = -(float)robot.Position_Orientation;

            y = map.Image.Height - y;

            // point1 (center)
            var centerPoint = new PointF(x, y);
            RobotcenterPoint = centerPoint;
            

            // 좌표계 회전 변환
            Matrix matrix = g.Transform;

            matrix.RotateAt(theta, centerPoint);
            g.Transform = matrix;

            // make robot path
            var robotRect = new RectangleF(centerPoint.X - halfSizeX, centerPoint.Y - halfSizeY, halfSizeX * 2, halfSizeY * 2);
            var cornerRadius = 4.0f;

            //if (robotPath == null)
            //{
            //    int xx = (int)robotRect.X;
            //    int yy = (int)robotRect.Y;
            //    robotPath = new GraphicsPath();
            //    robotPath.AddPolygon(new Point[]
            //        {
            //            new Point(10,10) + new Size(xx,yy),
            //            new Point(20,20) + new Size(xx,yy),
            //            new Point(20,10) + new Size(xx,yy),
            //            new Point(10,20) + new Size(xx,yy),
            //            new Point(10,10) + new Size(xx,yy),
            //        });
            //}

            using (GraphicsPath robotPath = MapRenderHelper.GetRoundedRectanglePath(robotRect, cornerRadius, cornerRadius))
            {
                // draw robot
                if (robot.StateID == INA_ACS_Server.RobotState.EmergencyStop || robot.StateID == INA_ACS_Server.RobotState.Error)
                {
                    Pen borderPen2 = new Pen(Color.Red, -1);
                    Pen arrowPen2 = new Pen(Color.Red, 5.0f) { EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor };

                    g.FillPath(Brushes.Red, robotPath);
                    g.DrawPath(borderPen2, robotPath);
                    g.DrawLine(arrowPen2, centerPoint, centerPoint + new SizeF(halfSizeX, 0)); // arrow
                }
                else if (robot.StateID == INA_ACS_Server.RobotState.Pause)
                {
                    Pen borderPen2 = new Pen(Color.Yellow, -1);
                    Pen arrowPen2 = new Pen(Color.Yellow, 5.0f) { EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor };

                    g.FillPath(Brushes.Yellow, robotPath);
                    g.DrawPath(borderPen2, robotPath);
                    g.DrawLine(arrowPen2, centerPoint, centerPoint + new SizeF(halfSizeX, 0)); // arrow
                }
                else
                {
                    g.FillPath(Brushes.DeepSkyBlue, robotPath);
                    g.DrawPath(borderPen2, robotPath);
                    g.DrawLine(arrowPen2, centerPoint, centerPoint + new SizeF(halfSizeX, 0)); // arrow
                }
            }

            // 좌표계 회전 복구
            //matrix.Reset();
            matrix.RotateAt(-theta, centerPoint);
            g.Transform = matrix;

            // draw robot info
            //var robotInfo = "robot:" + robot.RobotName;
            var robotInfo = robot.RobotName;
            var textPoint = centerPoint - new Size(20, 20);
            g.DrawLine(Pens.Black, centerPoint, textPoint);
            g.DrawString(robotInfo, font1, Brushes.Blue, textPoint + new Size(-16, -16));
        }


        private void DrawCharger(Graphics g, FleetMap map, FleetPosition pos)
        {
            float halfSizeX = 11.0f;
            float halfSizeY = 11.0f;

            float x = (float)pos.PosX / (float)map.Resolution;
            float y = (float)pos.PosY / (float)map.Resolution;
            float theta = 0; // -(float)pos.Orientation;
                             //float theta = DateTime.Now.Second * 6; // TEST

            y = map.Image.Height - y;

            // point1 (center)
            var centerPoint = new PointF(x, y);

            // 좌표계 회전 변환
            Matrix matrix = g.Transform;

            matrix.RotateAt(theta, centerPoint);
            //matrix.Scale(0.1f, 0.1f);
            g.Transform = matrix;

            // make path
            var chargerRect = new RectangleF(centerPoint.X - halfSizeX, centerPoint.Y - halfSizeY, halfSizeX * 2, halfSizeY * 2);

            using (GraphicsPath chargerPath = MapRenderHelper.GetChargerPath(chargerRect, centerPoint, halfSizeX))
            {
                // draw charger
                g.FillPath(Brushes.DeepSkyBlue, chargerPath);
                g.DrawPath(borderPen2, chargerPath);
                //g.DrawEllipse(Pens.Red, new Rectangle((int)chargerRect.X, (int)chargerRect.Y, (int)chargerRect.Width, (int)chargerRect.Height));
            }

            // 좌표계 회전 복구
            //matrix.Reset();
            matrix.RotateAt(-theta, centerPoint);
            g.Transform = matrix;

            // draw robot info
            var posInfo = pos.Name;
            var textPoint = centerPoint - new Size(20, 20);
            g.DrawLine(Pens.Black, centerPoint, textPoint);
            g.DrawString(posInfo, font1, Brushes.Blue, textPoint + new Size(-16, -16));
        }

    }
}
