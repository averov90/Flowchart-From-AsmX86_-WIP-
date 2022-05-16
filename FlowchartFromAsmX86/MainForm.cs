using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 * IMPORTANT!
 * This code is not optimized because it was written in a short time and for a rather narrow task. 
 * This task might be more correct to solve using scripting languages (such as python), but 
 * I did not find enough reasons to use them (there were no libraries that would solve task "in 2 lines"). 
 * I chose C# for this project. It is convenient for me for relatively simple projects.
 * 
 * I also write in a relatively simple style to make it easier for other people to understand and complete this code. 
 * I hope not in vain. Waiting for pull requests :)
 * 
 */

namespace FlowchartFromAsmX86 {
    public partial class MainForm : Form {
        //29 - size of RUS_ALPHABET
        public readonly char[] RUS_ALPHABET = { 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ж', 'З', 'И', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ы', 'Э', 'Ю', 'Я' };
        public MainForm() {
            InitializeComponent();
            ctx = SynchronizationContext.Current;
        }

        SynchronizationContext ctx;
        string current_name = null;
        Settings form_settings = new Settings();

        private void btn_generate_Click(object sender, EventArgs e) {
            splContainer.Enabled = false;

            string[] code = tb_code.Lines;
            if (!Trim_unnecessary(ref code)) {
                MessageBox.Show("The procedure was not found or the provided code is incorrect.\nP.S. Are you sure this compiles?", "Procedure detection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                splContainer.Enabled = true;
                return;
            }

            form_settings.fname = current_name;
            if (form_settings.ShowDialog() != DialogResult.OK) { 
                splContainer.Enabled = true; 
                return; 
            }

            if (!ParseCode(ref code, out List<AsmCommand> rawCodeModel, out Dictionary<int, LableMapEntry> rawCodeModelLables)) {
                MessageBox.Show("The provided code is incorrect.\nNote that the parser does not accept next:\n\thalt (\"hlt\") statement\n\tjump (\"jmp\") to address not by label", "Parsing error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                splContainer.Enabled = true;
                return;
            }

            if (!CreateDataModel(rawCodeModel, rawCodeModelLables, out List<BlockModel> dataModel)) {
                MessageBox.Show("The provided code is incorrect.\nP.S. Are you sure this compiles?", "Processing error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                splContainer.Enabled = true;
                return;
            }

            if (form_settings.chbx_blockedit.Checked) {
                form_settings.chbx_blockedit.Checked = false;

            retry:;
                BlockEdit be = new BlockEdit(new List<BlockModel>(dataModel), rawCodeModel, ref code, current_name, form_settings.rb_langRussian.Checked);
                
                switch (be.ShowDialog()) {
                    case DialogResult.OK:
                        dataModel = be.model;
                        break;
                    case DialogResult.Abort:
                        return;
                    case DialogResult.Retry:
                        goto retry;
                    default:
                        break;
                }
            }

            if (dataModel.Count == 0) {
                MessageBox.Show("No assembler commands found.\nContinuing calculations is useless.", "Empty flowchart", MessageBoxButtons.OK, MessageBoxIcon.Information);
                splContainer.Enabled = true;
                return;
            }

            //Blocks sizes distributor
            //TODO: uncomment bellow
            //Progress prg = new Progress();

            //Task.Run(() => GetGraphicsBlocks(dataModel, out List<GraphicsBlock> gaphicsBlocks, out List<GraphicsBlockSizes> gaphicsBlocks_sizes, prg));

            //prg.ShowDialog(); 
            //TODO: uncomment above

            StreamWriter str = new StreamWriter("test.txt", false);
            foreach (BlockModel item in dataModel) {
                str.WriteLine(item.figure.ToString() + " : " + (item.row2 ?? "").Replace("\n", " ; "));
            }
            str.Close();

            splContainer.Enabled = true;
        }

        private void MainForm_Shown(object sender, EventArgs e) {
            /*if (MessageBox.Show("This program is neither a compiler nor a static analyzer!\nIt is assumed that you have already checked your code and it has been compiled in TASM successfully. Otherwise, an incorrect result is possible.\nNote that the parser does not accept the halt statement.\nClick \"OK\" if you are sure that the code is correct.", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                Close();*/
        }
    }
}
