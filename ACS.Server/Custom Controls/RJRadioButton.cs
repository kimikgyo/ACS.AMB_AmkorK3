using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace INA_ACS_Server
{
    public class RJRadioButton : RadioButton
    {
        //필드
        private Color checkedColor = Color.MediumSlateBlue;
        private Color unCheckedColor = Color.Black;
        private Color bordercheckedColor = Color.MediumSlateBlue;
        private Color borderunCheckedColor = Color.Black;


        public Color CheckedColor
        {
            get
            {
                return checkedColor;
            }
            
            set
            {
                checkedColor = value;
                this.Invalidate();
            }
        }

        public Color UnCheckedColor
        {
            get
            {
                return unCheckedColor;
            }

            set
            {
                unCheckedColor = value;
                this.Invalidate();
            }
        }

        public Color BordercheckedColor
        {
            get
            {
                return bordercheckedColor;
            }

            set
            {
                bordercheckedColor = value;
                this.Invalidate();
            }
        }

        public Color BorderunCheckedColor
        {
            get
            {
                return borderunCheckedColor;
            }

            set
            {
                borderunCheckedColor = value;
                this.Invalidate();
            }
        }

        public RJRadioButton()
        {
            this.MinimumSize = new Size(0, 21);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            Graphics graphics = pevent.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            float rbBorderSize = 13F;
            float rbCheckSize = 6F;

            RectangleF rectRbBorder = new RectangleF()
            {
                X = 1.2F,
                Y = (this.Height - rbBorderSize) / 2,
                Width = rbBorderSize,
                Height = rbBorderSize
            };

            RectangleF rectRbCheck = new RectangleF()
            {
                X = rectRbBorder.X + ((rectRbBorder.Width - rbCheckSize) / 2),
                Y = (this.Height - rbCheckSize) / 2,
                Width = rbCheckSize,
                Height = rbCheckSize
            };

            using (Pen penBorder = new Pen(checkedColor, 5.5F))
            using (SolidBrush brushRbCheck = new SolidBrush(checkedColor))
            using (SolidBrush brushText = new SolidBrush(this.ForeColor))
            {
                graphics.Clear(this.BackColor);

                if(this.Checked)
                {
                    penBorder.Color = bordercheckedColor;
                    graphics.DrawEllipse(penBorder, rectRbBorder);
                    graphics.FillEllipse(brushRbCheck, rectRbCheck);
                }
                else
                {
                    penBorder.Color = unCheckedColor;
                    brushRbCheck.Color = unCheckedColor;
                    //graphics.DrawEllipse(penBorder, rectRbBorder);
                    graphics.FillEllipse(brushRbCheck, rectRbBorder);
                }

                graphics.DrawString(this.Text, this.Font, brushText, rbBorderSize + 8, (this.Height - TextRenderer.MeasureText(this.Text, this.Font).Height) / 2);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            this.Width = TextRenderer.MeasureText(this.Text, this.Font).Width + 30;
        }
    }
}
