using System.ComponentModel;
using System.Drawing;
using System.Threading;

namespace System.Windows.Forms
{
    public class ProgressBarCustom : ProgressBar
    {
        private BackgroundWorker marqueeAnimation;
        private int position;

        public ProgressBarCustom()
        {
            this.position = Minimum;
            this.SetStyle(ControlStyles.UserPaint, true);
            this.marqueeAnimation = new BackgroundWorker();
            this.marqueeAnimation.WorkerReportsProgress = true;
            this.marqueeAnimation.DoWork += new DoWorkEventHandler(DoInBackground);
            this.marqueeAnimation.ProgressChanged += new ProgressChangedEventHandler(DoProgress);
            this.marqueeAnimation.RunWorkerAsync();
        }

        private void DoProgress(object sender, ProgressChangedEventArgs e)
        {
            position += this.Step;
            if (position > this.Width)
                position = 0;

            Invalidate();
        }

        private void DoInBackground(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            while(true)
            {
                Thread.Sleep(this.MarqueeAnimationSpeed);
                worker.ReportProgress(0);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            switch (this.Style)
            {
                case ProgressBarStyle.Continuous:
                case ProgressBarStyle.Blocks:
                    e.Graphics.FillRectangle(new SolidBrush(this.ForeColor), 2, 2, (int)(this.Width * ((double)this.Value / this.Maximum)) - 4, this.Height - 4);
                    break;
                case ProgressBarStyle.Marquee:
                    e.Graphics.FillRectangle(new SolidBrush(this.ForeColor), position, 2, 30, this.Height - 4);
                    break;
            }
        }
    }
}
