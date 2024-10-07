using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ACS.RobotMap
{
    internal class MapRenderHelper
    {
        public static GraphicsPath GetRoundedRectanglePath(RectangleF rect, float cornerRadiusX, float cornerRadiusY, bool roundUpperLeft = true, bool roundUpperRight = true, bool roundLowerRight = true, bool roundLowerLeft = true)
        {
            PointF point1;
            PointF point2;
            GraphicsPath path = new GraphicsPath();


            if (roundUpperLeft)
            {
                RectangleF cornerRectangle = new RectangleF(rect.X, rect.Y, 2 * cornerRadiusX, 2 * cornerRadiusY);
                path.AddArc(cornerRectangle, 180, 90);
                point1 = new PointF(rect.X + cornerRadiusX, rect.Y);
            }
            else
            {
                point1 = new PointF(rect.X, rect.Y);
            }


            if (roundUpperRight)
            {
                point2 = new PointF(rect.Right - cornerRadiusX, rect.Y);
            }
            else
            {
                point2 = new PointF(rect.Right, rect.Y);
            }
            path.AddLine(point1, point2);


            if (roundUpperRight)
            {
                RectangleF cornerRectangle = new RectangleF(rect.Right - 2 * cornerRadiusX, rect.Y, 2 * cornerRadiusX, 2 * cornerRadiusY);
                path.AddArc(cornerRectangle, 270, 90);
                point1 = new PointF(rect.Right, rect.Y + cornerRadiusY);
            }
            else
            {
                point1 = new PointF(rect.Right, rect.Y);
            }


            if (roundLowerRight)
            {
                point2 = new PointF(rect.Right, rect.Bottom - cornerRadiusY);
            }
            else
            {
                point2 = new PointF(rect.Right, rect.Bottom);
            }
            path.AddLine(point1, point2);


            if (roundLowerRight)
            {
                RectangleF cornerRectangle = new RectangleF(rect.Right - 2 * cornerRadiusX, rect.Bottom - 2 * cornerRadiusY, 2 * cornerRadiusX, 2 * cornerRadiusY);
                path.AddArc(cornerRectangle, 0, 90);
                point1 = new PointF(rect.Right - cornerRadiusX, rect.Bottom);
            }
            else
            {
                point1 = new PointF(rect.Right, rect.Bottom);
            }
            if (roundLowerLeft)
            {
                point2 = new PointF(rect.X + cornerRadiusX, rect.Bottom);
            }
            else
            {
                point2 = new PointF(rect.X, rect.Bottom);
            }
            path.AddLine(point1, point2);


            if (roundLowerLeft)
            {
                RectangleF cornerRectangle = new RectangleF(rect.X, rect.Bottom - 2 * cornerRadiusY, 2 * cornerRadiusX, 2 * cornerRadiusY);
                path.AddArc(cornerRectangle, 90, 90);
                point1 = new PointF(rect.X, rect.Bottom - cornerRadiusY);
            }
            else
            {
                point1 = new PointF(rect.X, rect.Bottom);
            }
            if (roundUpperLeft)
            {
                point2 = new PointF(rect.X, rect.Y + cornerRadiusY);
            }
            else
            {
                point2 = new PointF(rect.X, rect.Y);
            }
            path.AddLine(point1, point2);


            path.CloseFigure();
            return path;
        }


        public static GraphicsPath GetChargerPath(RectangleF rect, PointF center, float scale)
        {
            float scale1 = scale * 1.0f;
            float scale2 = scale * 2.0f;

            var path = new GraphicsPath();
            path.AddPolygon(new PointF[]
            {
                new PointF( 580 / 800.0f * scale2 + center.X - scale1,  17 / 1000.0f * scale2 + center.Y - scale1 ),
                new PointF( 170 / 800.0f * scale2 + center.X - scale1, 625 / 1000.0f * scale2 + center.Y - scale1 ),
                new PointF( 407 / 800.0f * scale2 + center.X - scale1, 585 / 1000.0f * scale2 + center.Y - scale1 ),
                new PointF( 247 / 800.0f * scale2 + center.X - scale1, 999 / 1000.0f * scale2 + center.Y - scale1 ),
                new PointF( 710 / 800.0f * scale2 + center.X - scale1, 397 / 1000.0f * scale2 + center.Y - scale1 ),
                new PointF( 420 / 800.0f * scale2 + center.X - scale1, 443 / 1000.0f * scale2 + center.Y - scale1 ),
            });
            //path.AddEllipse(rect);
            path.CloseFigure();
            return path;
        }
    }
}
