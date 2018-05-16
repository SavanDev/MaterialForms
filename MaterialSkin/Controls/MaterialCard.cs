using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
    public class MaterialCard : Control
    {

        #region Variables

        Image image;
        bool showImage = true;
        Label InfoLabel = new Label();

        string info = "Card Content is here";

        bool Growing;

        #endregion
        #region Properties

        [Category("Appearance"), Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string ContentText
        {
            get { return info; }
            set
            {
                info = value;

                InfoLabel.Text = info;
                ResizeLabel();

                Invalidate();
            }
        }

        [Category("Appearance")]
        public Image Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public bool ShowImage
        {
            get
            {
                return showImage;
            }
            set
            {
                showImage = value;
                this.Height -= 1;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets the skin manager.
        /// </summary>
        /// <value>
        /// The skin manager.
        /// </value>
        [Browsable(false)]
        public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;

        #endregion
        #region Events

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Width = 234;
            AjustHeight();
        }

        #endregion

        public MaterialCard()
        {
            Width = 234; Height = 214; DoubleBuffered = true;

            AddLabel();
            Controls.Add(InfoLabel);
        }

        public void AddLabel()
        {
            InfoLabel.AutoSize = false;
            InfoLabel.Font = SkinManager.ROBOTO_MEDIUM_9;
            InfoLabel.Location = new Point(13, 169);
            InfoLabel.ForeColor = SkinManager.GetPrimaryTextColor();

            InfoLabel.Width = Width - 26;
            InfoLabel.Text = info;
            ResizeLabel();
        }

        private void ResizeLabel()
        {
            if (Growing) return;
            try
            {
                Growing = true;
                Size sz = new Size(InfoLabel.Width, Int32.MaxValue);
                sz = TextRenderer.MeasureText(InfoLabel.Text, InfoLabel.Font, sz, TextFormatFlags.WordBreak);
                InfoLabel.Height = sz.Height;
            }
            finally
            {
                Growing = false;
            }
            
            this.Height = InfoLabel.Location.Y + InfoLabel.Height + 30;
            Refresh();
        }

        private void AjustHeight()
        {
            this.Height = showImage ? 214 : 92;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics G = e.Graphics;
            G.SmoothingMode = SmoothingMode.HighQuality;
            G.Clear(Parent.BackColor);

            InfoLabel.BackColor = SkinManager.GetDialogBackgroundColor();
            InfoLabel.ForeColor = SkinManager.GetPrimaryTextColor();
            InfoLabel.Location = new Point(13, showImage ? 169 : 47);

            Color NonColor = SkinManager.GetBorderDialogColor();

            var BG = DrawHelper.CreateRoundRect(1, 1, Width - 3, Height - 5, 5);
            var ShadowBG = DrawHelper.CreateRoundRect(1, 1, Width - 3, Height - 4, 6);

            G.FillPath(new SolidBrush(NonColor), ShadowBG);
            G.DrawPath(new Pen(NonColor), ShadowBG);

            G.FillPath(new SolidBrush(SkinManager.GetDialogBackgroundColor()), BG);
            G.DrawPath(new Pen(NonColor), BG);

            G.DrawString(Text, SkinManager.ROBOTO_MEDIUM_15, new SolidBrush(SkinManager.GetPrimaryTextColor()), 12, showImage ? 136 : 12);

            if (showImage)
            {
                var PicBG = DrawHelper.CreateUpRoundRect(1, 1, 232, 124, 5);
                var UpRoundedRec = DrawHelper.CreateUpRoundRect(1, 1, 231, 124, 5);
                if (image != null)
                {
                    G.SetClip(PicBG);
                    G.DrawImage(image, 0, 0, 233, 126);
                }
                else
                {
                    G.FillPath(new SolidBrush(NonColor), UpRoundedRec);
                    G.DrawPath(new Pen(NonColor), UpRoundedRec);
                }
            }
        }
    }
}


