using System;
using System.Windows.Forms;

namespace FlowchartFromAsmX86 {
    public partial class EditBlock : Form {
        public delegate void RetrieveVirtualItem(ref RetrieveVirtualItemEventArgs e);
        RetrieveVirtualItem func;

        internal int connection = -1;
        public EditBlock(RetrieveVirtualItem func) {
            InitializeComponent();
            this.func = func;
        }

        private void button3_Click(object sender, EventArgs e) {
            description.Text = "";
        }

        private void type_SelectedIndexChanged(object sender, EventArgs e) {
            switch ((MainForm.BlockModel.Figure)type.SelectedIndex) {
                case MainForm.BlockModel.Figure.FDecision:
                case MainForm.BlockModel.Figure.JDecision:
                    container.Panel1Collapsed = container.Panel2Collapsed = false;
                    break;
                case MainForm.BlockModel.Figure.Connector:
                    container.Panel1Collapsed = true;
                    container.Panel2Collapsed = false;
                    break;
                default:
                    container.Panel1Collapsed = false;
                    container.Panel2Collapsed = true;
                    break;
            }
            if (clearSuppose) {
                chbx_clearCmd.Checked = true;
                clearSuppose = false;
            }
        }

        private void lv_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e) {
            func(ref e);
        }

        private void button1_Click(object sender, EventArgs e) {
            if (lv.SelectedIndices.Count == 0) { 
                if (type.SelectedIndex == (int)MainForm.BlockModel.Figure.FDecision || type.SelectedIndex == (int)MainForm.BlockModel.Figure.JDecision || type.SelectedIndex == (int)MainForm.BlockModel.Figure.Connector) {
                    MessageBox.Show(this, "You must specify the element with which this should be connected!", "Missing connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            } else
                connection = lv.SelectedIndices[0];

            if (chbx_clearCmd.Visible && chbx_clearCmd.Checked)
                textCmd.Text = "";

            DialogResult = DialogResult.OK;
            Close();
        }

        private void EditBlock_Shown(object sender, EventArgs e) {
            if (connection != -1) {
                lv.SelectedIndices.Add(connection);
                lv.EnsureVisible(connection);
            }
            clearSuppose = true;
        }

        private bool clearSuppose = false;
    }
}
