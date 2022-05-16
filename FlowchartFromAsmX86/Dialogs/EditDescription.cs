using System;
using System.Windows.Forms;

namespace FlowchartFromAsmX86 {
    public partial class EditDescription : Form {
        public EditDescription(string text) {
            InitializeComponent();
            description.Lines = text.Split('\n');
        }

        private void button3_Click(object sender, EventArgs e) {
            description.Text = "";
        }
    }
}
