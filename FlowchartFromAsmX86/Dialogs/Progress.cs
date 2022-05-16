using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace FlowchartFromAsmX86 {
    public partial class Progress : Form {
        public bool cancelled = false;

        Stopwatch stw = new Stopwatch();
        int max, current;
        string chapter_text = "";

        void FillDate(long seconds) {
            string nval = "";

            if (seconds >= 86400) { //Day
                nval = (seconds / 86400).ToString() + " days ";
                seconds %= 86400;
            }
            if (seconds >= 3600) { //Hour
                nval += (seconds / 3600).ToString() + "h ";
                seconds %= 3600;
            }
            if (seconds >= 60) { //Minute
                nval += (seconds / 60).ToString() + "m ";
                seconds %= 60;
            }

            nval += seconds.ToString() + "s ";

            lbl_time.Text = nval;
        }

        public void clb_NewСhapter(string title, int max) {
            progressBar1.Value = current = 0;
            progressBar1.Maximum = this.max = max;

            chapter_text = title;
            lbl_cases.Text = chapter_text + ": " + current.ToString() + "/" + max.ToString();
            Text = "Progress: 0%";
            lbl_progress.Text = "0%";

            stw.Restart();
        }

        public void clb_ChangeMaxAndCurrent(int max, int current) {
            if (current > max) current = max;

            if (current * this.max > this.current * max) {
                progressBar1.Maximum = this.max = max;
                progressBar1.Value = this.current = current;
                Text = "Progress: " + (lbl_progress.Text = (current * 100 / max).ToString() + "%");
            } else {
                this.max = max;
                this.current = current;
            }
            lbl_cases.Text = chapter_text + ": " + current.ToString() + "/" + max.ToString();

            FillDate(stw.ElapsedMilliseconds / (1000 * max) * (max - current));
        }

        public void clb_ChangeMax(int val) {
            if (max < val) {
                progressBar1.Maximum = max = val;
                Text = "Progress: " + (lbl_progress.Text = (current * 100 / max).ToString() + "%");
            } else {
                max = val;
            }
            lbl_cases.Text = chapter_text + ": " + current.ToString() + "/" + max.ToString();

            FillDate(stw.ElapsedMilliseconds / (1000 * max) * (max - current));
        }

        public void clb_ChangePercent(byte val) {
            progressBar1.Value = current = (int)Math.Round(max * val / 100F);
            Text = "Progress: " + (lbl_progress.Text = (current * 100 / max).ToString() + "%");
            lbl_cases.Text = chapter_text + ": " + current.ToString() + "/" + max.ToString();

            FillDate(stw.ElapsedMilliseconds / (1000 * max) * (max - current));
        }

        public void clb_ChangeCurrent(int val) {
            progressBar1.Value = current = (val > max ? max : val);
            Text = "Progress: " + (lbl_progress.Text = (current * 100 / max).ToString() + "%");
            lbl_cases.Text = chapter_text + ": " + current.ToString() + "/" + max.ToString();

            FillDate(stw.ElapsedMilliseconds / (1000 * max) * (max - current));
        }

        public void clb_Increment() {
            if (current < max) ++current;
            progressBar1.Value = current;
            Text = "Progress: " + (lbl_progress.Text = (current * 100 / max).ToString() + "%");
            lbl_cases.Text = chapter_text + ": " + current.ToString() + "/" + max.ToString();

            FillDate(stw.ElapsedMilliseconds / (1000 * max) * (max - current));
        }

        public Progress() {
            InitializeComponent();
        }

        private void Progress_Shown(object sender, EventArgs e) {
            stw.Start();
        }

        private void button1_Click(object sender, EventArgs e) {
            button1.Enabled = false;
            cancelled = true;
            button1.Text = "Cancelled";
            stw.Stop();
        }
    }
}
