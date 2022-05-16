using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace FlowchartFromAsmX86 {
    public partial class Settings : Form {
        public Settings() {
            InitializeComponent();

            cbx_detailed.SelectedIndex = 
                cbx_tmode.SelectedIndex = 
                cbx_adj.SelectedIndex = 
                cbx_gridSzMode.SelectedIndex = 0;

            cbx_loopSize.SelectedIndex = 1;

            font = dialog_font.Font;
        }

        public Font font;

        private void btn_default_Click(object sender, EventArgs e) {
            nmb_ratioBlock.Value = 1.5M;
            nmb_ratioCanvas.Value = 0.65M;
            nmb_ratioArea.Value = 182;
            nmb_lengthLine.Value = 0.75M;
            nmb_startNum.Value = 1;
            chbx_allowFontFl.Checked = true;
            nmb_fontFlLimit.Value = 1;
            chbx_printPercent.Checked = false;
            cbx_loopSize.Enabled = true;
            cbx_loopSize.SelectedIndex = 1;
            rb_langRussian.Checked = true;
            chbx_autoRatioArea.Checked = false;
            chbx_useHexagons.Checked = false;
            cbx_detailed.SelectedIndex = 0;
            nmb_lineSpacing.Value = 1.5M;

            cbx_tmode.SelectedIndex =
                cbx_adj.SelectedIndex = 0;
            chbx_allowWayUp.Checked = true;
            chbx_allowCrossColumn.Checked = true;
            chbx_allowLeftVia.Checked = true;
            chbx_allowBetwVia.Checked = true;
            nmb_linesInBetw.Value = 1;
            nmb_lineWidth.Value = 20;
            nmb_countSizes.Value = 3;
            nmb_strDiffThr.Value = 92;
            cbx_gridSzMode.SelectedIndex = 0;

            dialog_font.Font = font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);

            chbx_blockedit.Checked = false;
        }

        private void rb_langEnglish_CheckedChanged(object sender, EventArgs e) {
            if (rb_langEnglish.Checked) {
                chbx_allowWayUp.Enabled = false;
                chbx_allowWayUp.Checked = true;

                cbx_loopSize.Enabled = false;
            } else {
                chbx_allowWayUp.Enabled = true;

                cbx_loopSize.Enabled = true;
            }
        }

        private void chbx_autoRatioArea_CheckedChanged(object sender, EventArgs e) {
            if (chbx_autoRatioArea.Checked)
                nmb_ratioArea.Enabled = false;
            else
                nmb_ratioArea.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e) {
            if (dialog_font.ShowDialog() == DialogResult.OK) {
                font = dialog_font.Font;
            }
        }

        private void chbx_useHexagons_CheckedChanged(object sender, EventArgs e) {
            if (chbx_useHexagons.Checked)
                chbx_blockedit.Checked = true;
        }

        internal string fname = "";
        private void button2_Click(object sender, EventArgs e) {
            if (chbx_saveSettings.Checked) {
                try {
                    StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + '\\' + fname + ".ffags");

                    sw.WriteLine("ratio block:" + nmb_ratioBlock.Value);
                    sw.WriteLine("ratio canvas:" + nmb_ratioCanvas.Value);
                    sw.WriteLine("ratio area:" + nmb_ratioArea.Value);
                    sw.WriteLine("line length:" + nmb_lengthLine.Value);
                    sw.WriteLine("start num:" + nmb_startNum.Value);
                    sw.WriteLine("allow font fluctuation:" + chbx_allowFontFl.Checked.ToString());
                    sw.WriteLine("font fluctuation limit:" + nmb_fontFlLimit.Value);
                    sw.WriteLine("print percent:" + chbx_printPercent.Checked.ToString());
                    sw.WriteLine("loop figure sizes equivalent:" + cbx_loopSize.SelectedIndex);
                    sw.WriteLine("language russian:" + rb_langRussian.Checked.ToString());
                    sw.WriteLine("auto ratio area:" + chbx_autoRatioArea.Checked.ToString());
                    sw.WriteLine("use hexagons:" + chbx_useHexagons.Checked.ToString());
                    sw.WriteLine("perfomance detailed:" + cbx_detailed.SelectedIndex);
                    sw.WriteLine("line spacing:" + nmb_lineSpacing.Value);

                    sw.WriteLine("tracing mode:" + cbx_tmode.SelectedIndex);
                    sw.WriteLine("adjustment mode:" + cbx_adj.SelectedIndex);
                    sw.WriteLine("allow way up:" + chbx_allowWayUp.Checked.ToString());
                    sw.WriteLine("allow cross-column:" + chbx_allowCrossColumn.Checked.ToString());
                    sw.WriteLine("allow left via:" + chbx_allowLeftVia.Checked.ToString());
                    sw.WriteLine("allow between via:" + chbx_allowBetwVia.Checked.ToString());
                    sw.WriteLine("count lines in-between:" + nmb_linesInBetw.Value);
                    sw.WriteLine("connecting line thickness:" + nmb_lineWidth.Value);
                    sw.WriteLine("count sizes:" + nmb_countSizes.Value);
                    sw.WriteLine("string difference threshold:" + nmb_strDiffThr.Value);
                    sw.WriteLine("grid size mode:" + cbx_gridSzMode.SelectedIndex);

                    sw.WriteLine("font:" + font.FontFamily.Name + ";" + font.Size + ";" + (ushort)font.Style);

                    sw.Close();
                } catch { }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void Settings_Load(object sender, EventArgs e) {
            try {
                StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + '\\' + fname + ".ffags");

                while (!sr.EndOfStream) {
                    string[] spl = sr.ReadLine().Split(':');
                    if (spl.Length != 2) continue;

                    switch (spl[0].Trim()) {
                        case "ratio block":
                            nmb_ratioBlock.Value = decimal.Parse(spl[1].Trim());
                            break;
                        case "ratio canvas":
                            nmb_ratioCanvas.Value = decimal.Parse(spl[1].Trim());
                            break;
                        case "ratio area":
                            nmb_ratioArea.Value = decimal.Parse(spl[1].Trim());
                            break;
                        case "line length":
                            nmb_lengthLine.Value = decimal.Parse(spl[1].Trim());
                            break;
                        case "start num":
                            nmb_startNum.Value = decimal.Parse(spl[1].Trim());
                            break;
                        case "allow font fluctuation":
                            chbx_allowFontFl.Checked = bool.Parse(spl[1].Trim());
                            break;
                        case "font fluctuation limit":
                            nmb_fontFlLimit.Value = decimal.Parse(spl[1].Trim());
                            break;

                        case "print percent":
                            chbx_printPercent.Checked = bool.Parse(spl[1].Trim());
                            break;
                        case "loop figure sizes equivalent":
                            cbx_loopSize.SelectedIndex = ushort.Parse(spl[1].Trim());
                            break;
                        case "language russian":
                            rb_langRussian.Checked = bool.Parse(spl[1].Trim());
                            rb_langEnglish.Checked = !rb_langRussian.Checked;
                            break;
                        case "auto ratio area":
                            chbx_autoRatioArea.Checked = bool.Parse(spl[1].Trim());
                            break;
                        case "use hexagons":
                            chbx_useHexagons.Checked = bool.Parse(spl[1].Trim());
                            break;
                        case "perfomance detailed":
                            cbx_detailed.SelectedIndex = ushort.Parse(spl[1].Trim());
                            break;
                        case "line spacing":
                            nmb_lineSpacing.Value = decimal.Parse(spl[1].Trim());
                            break;
                            

                        case "tracing mode":
                            cbx_tmode.SelectedIndex = ushort.Parse(spl[1].Trim());
                            break;
                        case "adjustment mode":
                            cbx_adj.SelectedIndex = ushort.Parse(spl[1].Trim());
                            break;
                        case "allow way up":
                            chbx_allowWayUp.Checked = bool.Parse(spl[1].Trim());
                            break;
                        case "allow cross-column":
                            chbx_allowCrossColumn.Checked = bool.Parse(spl[1].Trim());
                            break;
                        case "allow left via":
                            chbx_allowLeftVia.Checked = bool.Parse(spl[1].Trim());
                            break;
                        case "allow between via":
                            chbx_allowBetwVia.Checked = bool.Parse(spl[1].Trim());
                            break;
                        case "count lines in-between":
                            nmb_linesInBetw.Value = decimal.Parse(spl[1].Trim());
                            break;
                        case "connecting line thickness":
                            nmb_lineWidth.Value = decimal.Parse(spl[1].Trim());
                            break;
                        case "count sizes":
                            nmb_countSizes.Value = decimal.Parse(spl[1].Trim());
                            break;
                        case "string difference threshold":
                            nmb_strDiffThr.Value = decimal.Parse(spl[1].Trim());
                            break;
                        case "grid size mode":
                            cbx_gridSzMode.SelectedIndex = ushort.Parse(spl[1].Trim());
                            break;

                        case "font": 
                            spl = spl[1].Split(';');
                            dialog_font.Font = font = new Font(spl[0].Trim(), float.Parse(spl[1].Trim()), (FontStyle)ushort.Parse(spl[2].Trim()), GraphicsUnit.Point, 0);
                            break;
                    }
                }

                chbx_allowWayUp.Checked = rb_langEnglish.Checked;

                sr.Close();
            } catch { }
        }

        private void chbx_allowFontFl_CheckedChanged(object sender, EventArgs e) {
            nmb_fontFlLimit.Enabled = chbx_allowFontFl.Checked;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) {

        }
    }
}
