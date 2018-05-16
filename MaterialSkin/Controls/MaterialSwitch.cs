using MaterialSkin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
    public class MaterialSwitch : CheckBox
    {
        #region Variables

        [Browsable(false)]
        public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;

        Timer AnimationTimer = new Timer { Interval = 1 };
        GraphicsPath RoundedRectangle;

        Color EllipseBackColor;
        Color EllipseBorderBackColor;

        Color EnabledUnCheckedColor = ColorTranslator.FromHtml("#bcbfc4");
        Color EnabledUnCheckedEllipseBorderColor = ColorTranslator.FromHtml("#a9acb0");

        Color DisabledEllipseBackColor = ColorTranslator.FromHtml("#c3c4c6");
        Color DisabledEllipseBorderBackColor = ColorTranslator.FromHtml("#90949a");

        int PointAnimationNum = 4;

        #endregion
        #region  Properties

        #endregion
        #region Events

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            AnimationTimer.Start();
        }

        protected override void OnResize(EventArgs e)
        {
            Height = 19; Width = 47;

            RoundedRectangle = new GraphicsPath();
            int radius = 10;

            RoundedRectangle.AddArc(11, 4, radius - 1, radius, 180, 90);
            RoundedRectangle.AddArc(Width - 21, 4, radius - 1, radius, -90, 90);
            RoundedRectangle.AddArc(Width - 21, Height - 15, radius - 1, radius, 0, 90);
            RoundedRectangle.AddArc(11, Height - 15, radius - 1, radius, 90, 90);

            RoundedRectangle.CloseAllFigures();
            Invalidate();
        }

        #endregion
        public MaterialSwitch()
        {
            Height = 19; Width = 47; DoubleBuffered = true;
            AnimationTimer.Tick += new EventHandler(AnimationTick);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var G = pevent.Graphics;
            G.SmoothingMode = SmoothingMode.AntiAlias;
            G.Clear(Parent.BackColor);

            EllipseBackColor = SkinManager.ColorScheme.AccentColor;
            EllipseBorderBackColor = SkinManager.ColorScheme.PrimaryColor;

            G.FillPath(new SolidBrush(Color.FromArgb(115, Enabled ? Checked ? EllipseBackColor : EnabledUnCheckedColor : EnabledUnCheckedColor)), RoundedRectangle);
            G.DrawPath(new Pen(Color.FromArgb(50, Enabled ? Checked ? EllipseBackColor : EnabledUnCheckedColor : EnabledUnCheckedColor)), RoundedRectangle);

            G.FillEllipse(new SolidBrush(Enabled ? Checked ? EllipseBackColor : Color.White : DisabledEllipseBackColor), PointAnimationNum, 0, 18, 18);
            G.DrawEllipse(new Pen(Enabled ? Checked ? EllipseBorderBackColor : EnabledUnCheckedEllipseBorderColor : DisabledEllipseBorderBackColor), PointAnimationNum, 0, 18, 18);
        }

        void AnimationTick(object sender, EventArgs e)
        {
            if (Checked)
            {
                if (PointAnimationNum < 24)
                {
                    PointAnimationNum += 1;
                    this.Invalidate();
                }
            }
            else if (PointAnimationNum > 4)
            {
                PointAnimationNum -= 1;
                this.Invalidate();
            }
        }
    }


}