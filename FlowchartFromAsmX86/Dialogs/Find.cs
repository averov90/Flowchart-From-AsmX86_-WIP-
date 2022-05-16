using System;
using System.Windows.Forms;

namespace FlowchartFromAsmX86 {
    public partial class Find : Form {

        BlockEdit parent;
        public Find(BlockEdit form) {
            InitializeComponent();
            parent = form;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            if (cReplaceCmd.Checked) {
                tNewCmd.Enabled =
                    button3.Enabled = button4.Enabled = true;
            } else {
                tNewCmd.Enabled = false;
                tNewCmd.Text = "";
                if (!cReplaceDesc.Checked) {
                    button3.Enabled = button4.Enabled = false;
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e) {
            if (cReplaceDesc.Checked) {
                tNewDesc.Enabled =
                    button3.Enabled = button4.Enabled = true;
            } else {
                tNewDesc.Enabled = false;
                tNewDesc.Text = "";
                if (!cReplaceCmd.Checked) {
                    button3.Enabled = button4.Enabled = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            int cindex = -1;

            ListViewItem lvi;
            tDesc.Text = tDesc.Text.Replace("{loop letter}", "").Replace("{\\n}", "");

            while (true) {
                if (parent.lv.Items.Count == ++cindex) {
                    MessageBox.Show(this, "No one item was found.\nНи один элемент не найден.", "Search results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                lvi = parent.lv.Items[cindex];
                bool noth_found = true;

                if (tRow.Text != "") {
                    noth_found = false;
                    if (lvi.Text != tRow.Text.ToLower())
                        continue;
                }

                if (tCmd.Text != "") {
                    noth_found = false;
                    if (cCmdW.Checked) {
                        string[] spl1, spl2;

                        spl1 = tCmd.Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        spl2 = lvi.SubItems[1].Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        bool nfound = true;
                        byte clook = 0;
                        if (cCmdC.Checked) {
                            for (ushort i = 0; i != spl2.Length; ++i) {
                                if (spl1[clook] == spl2[i]) {
                                    ++clook;
                                    if (clook == spl1.Length) {
                                        nfound = false;
                                        break;
                                    }
                                } else {
                                    i -= clook;
                                    clook = 0;
                                }
                            }
                        } else {
                            for (ushort i = 0; i != spl2.Length; ++i) {
                                if (spl1[clook].ToLower() == spl2[i].ToLower()) {
                                    ++clook;
                                    if (clook == spl1.Length) {
                                        nfound = false;
                                        break;
                                    }
                                } else {
                                    i -= clook;
                                    clook = 0;
                                }
                            }
                        }

                        if (nfound)
                            continue;
                    } else {
                        if (cCmdC.Checked) {
                            if (!lvi.SubItems[1].Text.Contains(tCmd.Text))
                                continue;
                        } else {
                            if (!lvi.SubItems[1].Text.ToLower().Contains(tCmd.Text.ToLower()))
                                continue;
                        }
                    }
                }

                if (tDesc.Text != "") {
                    noth_found = false;
                    if (cDescW.Checked) {
                        string[] spl1, spl2;

                        spl1 = tDesc.Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        spl2 = lvi.ToolTipText.Split(new char[] { ' ', ',', '.', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                        bool nfound = true;
                        byte clook = 0;
                        if (cDescC.Checked) {
                            for (ushort i = 0; i != spl2.Length; ++i) {
                                if (spl1[clook] == spl2[i]) {
                                    ++clook;
                                    if (clook == spl1.Length) {
                                        nfound = false;
                                        break;
                                    }
                                } else {
                                    i -= clook;
                                    clook = 0;
                                }
                            }
                        } else {
                            for (ushort i = 0; i != spl2.Length; ++i) {
                                if (spl1[clook].ToLower() == spl2[i].ToLower()) {
                                    ++clook;
                                    if (clook == spl1.Length) {
                                        nfound = false;
                                        break;
                                    }
                                } else {
                                    i -= clook;
                                    clook = 0;
                                }
                            }
                        }

                        if (nfound)
                            continue;
                    } else {
                        if (cDescC.Checked) {
                            if (!lvi.ToolTipText.Contains(tDesc.Text))
                                continue;
                        } else {
                            if (!lvi.ToolTipText.ToLower().Contains(tDesc.Text.ToLower()))
                                continue;
                        }
                    }
                }

                if (tFigure.Text != "") {
                    noth_found = false;
                    if (!lvi.SubItems[3].Text.ToLower().Contains(tFigure.Text.ToLower()))
                        continue;
                }

                if (tConn.Text != "") {
                    noth_found = false;
                    if (lvi.SubItems[4].Text.ToLower() != tConn.Text.ToLower())
                        continue;
                }

                if (tMCmd.Text != "") {
                    noth_found = false;
                    if (cMCmdW.Checked) {
                        string[] spl1, spl2;

                        spl1 = tMCmd.Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        spl2 = lvi.SubItems[5].Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        bool nfound = true;
                        byte clook = 0;
                        if (cMCmdC.Checked) {
                            for (ushort i = 0; i != spl2.Length; ++i) {
                                if (spl1[clook] == spl2[i]) {
                                    ++clook;
                                    if (clook == spl1.Length) {
                                        nfound = false;
                                        break;
                                    }
                                } else {
                                    i -= clook;
                                    clook = 0;
                                }
                            }
                        } else {
                            for (ushort i = 0; i != spl2.Length; ++i) {
                                if (spl1[clook].ToLower() == spl2[i].ToLower()) {
                                    ++clook;
                                    if (clook == spl1.Length) {
                                        nfound = false;
                                        break;
                                    }
                                } else {
                                    i -= clook;
                                    clook = 0;
                                }
                            }
                        }

                        if (nfound)
                            continue;
                    } else {
                        if (cMCmdC.Checked) {
                            if (!lvi.SubItems[5].Text.Contains(tMCmd.Text))
                                continue;
                        } else {
                            if (!lvi.SubItems[5].Text.ToLower().Contains(tMCmd.Text.ToLower()))
                                continue;
                        }
                    }
                }

                if (noth_found)
                    continue;
                break;
            }
            parent.lv.SelectedIndices.Clear();
            parent.lv.SelectedIndices.Add(cindex);

            parent.record.Add("WHERE BLOCK");
            if (tRow.Text != "")
                parent.record.Add("ROW " + tRow.Text);
            if (tCmd.Text != "") {
                parent.record.Add("COMMAND " + tCmd.Text);
                parent.record.Add("MODE COMMAND " + (cCmdW.Checked ? "WHOLE " : "LIKE ") + (cCmdC.Checked ? "CASE" : "NOCASE"));
            }
            if (tDesc.Text != "") {
                parent.record.Add("DESCRIPTION " + tDesc.Text);
                parent.record.Add("MODE DESCRIPTION " + (cDescW.Checked ? "WHOLE " : "LIKE ") + (cDescC.Checked ? "CASE" : "NOCASE"));
            }
            if (tFigure.Text != "")
                parent.record.Add("FIGURE " + tFigure.Text);
            if (tConn.Text != "")
                parent.record.Add("CONNECTION " + tConn.Text);
            if (tMCmd.Text != "") {
                parent.record.Add("METACMD " + tMCmd.Text);
                parent.record.Add("MODE METACMD " + (cMCmdW.Checked ? "WHOLE " : "LIKE ") + (cMCmdC.Checked ? "CASE" : "NOCASE"));
            }

            parent.record.Add("SET CURSOR FROM BEGIN");
            parent.record.Commit(BlockEdit.CommandRecord.FunctionType.SELECTOR);
        }

        private void button5_Click(object sender, EventArgs e) {
            tRow.Text = tCmd.Text = tDesc.Text = tFigure.Text = tConn.Text = tMCmd.Text = "";
            cCmdC.Checked = cCmdW.Checked =
                cDescC.Checked = cDescW.Checked =
                cMCmdC.Checked = cMCmdW.Checked = false;
            cWA.Checked = true;
        }

        private void Find_FormClosing(object sender, FormClosingEventArgs e) {
            if (e.CloseReason == CloseReason.UserClosing) {
                e.Cancel = true;
                Hide();
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            int cindex = (parent.lv.SelectedIndices.Count == 1 ? parent.lv.SelectedIndices[0] : -1), prew_index = cindex;

            ListViewItem lvi;
            tDesc.Text = tDesc.Text.Replace("{loop letter}", "").Replace("{\\n}", "");

            while (true) {
                if (cWA.Checked) {
                    if (parent.lv.Items.Count == ++cindex) {
                        cindex = 0;
                        if (prew_index == -1) {
                            MessageBox.Show(this, "No one item was found next.\nНи один элемент не найден далее.", "Search results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }
                    if (prew_index == cindex) {
                        MessageBox.Show(this, "No one item was found next.\nНи один элемент не найден далее.", "Search results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                } else {
                    if (parent.lv.Items.Count == ++cindex) {
                        MessageBox.Show(this, "No one item was found next.\nНи один элемент не найден далее.", "Search results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                lvi = parent.lv.Items[cindex];
                bool noth_found = true;

                if (tRow.Text != "") {
                    noth_found = false;
                    if (lvi.Text != tRow.Text.ToLower())
                        continue;
                }

                if (tCmd.Text != "") {
                    noth_found = false;
                    if (cCmdW.Checked) {
                        string[] spl1, spl2;

                        spl1 = tCmd.Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        spl2 = lvi.SubItems[1].Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        bool nfound = true;
                        byte clook = 0;
                        if (cCmdC.Checked) {
                            for (ushort i = 0; i != spl2.Length; ++i) {
                                if (spl1[clook] == spl2[i]) {
                                    ++clook;
                                    if (clook == spl1.Length) {
                                        nfound = false;
                                        break;
                                    }
                                } else {
                                    i -= clook;
                                    clook = 0;
                                }
                            }
                        } else {
                            for (ushort i = 0; i != spl2.Length; ++i) {
                                if (spl1[clook].ToLower() == spl2[i].ToLower()) {
                                    ++clook;
                                    if (clook == spl1.Length) {
                                        nfound = false;
                                        break;
                                    }
                                } else {
                                    i -= clook;
                                    clook = 0;
                                }
                            }
                        }

                        if (nfound)
                            continue;
                    } else {
                        if (cCmdC.Checked) {
                            if (!lvi.SubItems[1].Text.Contains(tCmd.Text))
                                continue;
                        } else {
                            if (!lvi.SubItems[1].Text.ToLower().Contains(tCmd.Text.ToLower()))
                                continue;
                        }
                    }
                }

                if (tDesc.Text != "") {
                    noth_found = false;
                    if (cDescW.Checked) {
                        string[] spl1, spl2;

                        spl1 = tDesc.Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        spl2 = lvi.ToolTipText.Split(new char[] { ' ', ',', '.', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                        bool nfound = true;
                        byte clook = 0;
                        if (cDescC.Checked) {
                            for (ushort i = 0; i != spl2.Length; ++i) {
                                if (spl1[clook] == spl2[i]) {
                                    ++clook;
                                    if (clook == spl1.Length) {
                                        nfound = false;
                                        break;
                                    }
                                } else {
                                    i -= clook;
                                    clook = 0;
                                }
                            }
                        } else {
                            for (ushort i = 0; i != spl2.Length; ++i) {
                                if (spl1[clook].ToLower() == spl2[i].ToLower()) {
                                    ++clook;
                                    if (clook == spl1.Length) {
                                        nfound = false;
                                        break;
                                    }
                                } else {
                                    i -= clook;
                                    clook = 0;
                                }
                            }
                        }

                        if (nfound)
                            continue;
                    } else {
                        if (cDescC.Checked) {
                            if (!lvi.ToolTipText.Contains(tDesc.Text))
                                continue;
                        } else {
                            if (!lvi.ToolTipText.ToLower().Contains(tDesc.Text.ToLower()))
                                continue;
                        }
                    }
                }

                if (tFigure.Text != "") {
                    noth_found = false;
                    if (!lvi.SubItems[3].Text.ToLower().Contains(tFigure.Text.ToLower()))
                        continue;
                }

                if (tConn.Text != "") {
                    noth_found = false;
                    if (lvi.SubItems[4].Text.ToLower() != tConn.Text.ToLower())
                        continue;
                }

                if (tMCmd.Text != "") {
                    noth_found = false;
                    if (cMCmdW.Checked) {
                        string[] spl1, spl2;

                        spl1 = tMCmd.Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        spl2 = lvi.SubItems[5].Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        bool nfound = true;
                        byte clook = 0;
                        if (cMCmdC.Checked) {
                            for (ushort i = 0; i != spl2.Length; ++i) {
                                if (spl1[clook] == spl2[i]) {
                                    ++clook;
                                    if (clook == spl1.Length) {
                                        nfound = false;
                                        break;
                                    }
                                } else {
                                    i -= clook;
                                    clook = 0;
                                }
                            }
                        } else {
                            for (ushort i = 0; i != spl2.Length; ++i) {
                                if (spl1[clook].ToLower() == spl2[i].ToLower()) {
                                    ++clook;
                                    if (clook == spl1.Length) {
                                        nfound = false;
                                        break;
                                    }
                                } else {
                                    i -= clook;
                                    clook = 0;
                                }
                            }
                        }

                        if (nfound)
                            continue;
                    } else {
                        if (cMCmdC.Checked) {
                            if (!lvi.SubItems[5].Text.Contains(tMCmd.Text))
                                continue;
                        } else {
                            if (!lvi.SubItems[5].Text.ToLower().Contains(tMCmd.Text.ToLower()))
                                continue;
                        }
                    }
                }

                if (noth_found)
                    continue;
                break;
            }
            parent.record.Add("WHERE BLOCK");
            if (tRow.Text != "")
                parent.record.Add("ROW " + tRow.Text);
            if (tCmd.Text != "") {
                parent.record.Add("COMMAND " + tCmd.Text);
                parent.record.Add("MODE COMMAND " + (cCmdW.Checked ? "WHOLE " : "LIKE ") + (cCmdC.Checked ? "CASE" : "NOCASE"));
            }
            if (tDesc.Text != "") {
                parent.record.Add("DESCRIPTION " + tDesc.Text);
                parent.record.Add("MODE DESCRIPTION " + (cDescW.Checked ? "WHOLE " : "LIKE ") + (cDescC.Checked ? "CASE" : "NOCASE"));
            }
            if (tFigure.Text != "")
                parent.record.Add("FIGURE " + tFigure.Text);
            if (tConn.Text != "")
                parent.record.Add("CONNECTION " + tConn.Text);
            if (tMCmd.Text != "") {
                parent.record.Add("METACMD " + tMCmd.Text);
                parent.record.Add("MODE METACMD " + (cMCmdW.Checked ? "WHOLE " : "LIKE ") + (cMCmdC.Checked ? "CASE" : "NOCASE"));
            }
            parent.record.Add("SET CURSOR FROM CURSOR" + (cWA.Checked ? " WITH WRAP" : ""));
            parent.record.Commit(BlockEdit.CommandRecord.FunctionType.BOTH);

            parent.lv.SelectedIndices.Clear();
            parent.lv.SelectedIndices.Add(cindex);
            parent.record.RemoveLatest();

        }

        private void button3_Click(object sender, EventArgs e) {
            int cindex = (parent.lv.SelectedIndices.Count == 1 ? parent.lv.SelectedIndices[0] : -1), prew_index = cindex;

            ListViewItem lvi;
            tDesc.Text = tDesc.Text.Replace("{loop letter}", "").Replace("{\\n}", "");

            while (true) {
                if (cWA.Checked) {
                    if (parent.lv.Items.Count == ++cindex) {
                        cindex = 0;
                        if (prew_index == -1) {
                            MessageBox.Show(this, "No one item was found next.\nНи один элемент не найден далее.", "Replace results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }
                    if (prew_index == cindex) {
                        MessageBox.Show(this, "No one item was found next.\nНи один элемент не найден далее.", "Replace results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                } else {
                    if (parent.lv.Items.Count == ++cindex) {
                        MessageBox.Show(this, "No one item was found next.\nНи один элемент не найден далее.", "Replace results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                lvi = parent.lv.Items[cindex];
                bool noth_found = true;

                if (tRow.Text != "") {
                    noth_found = false;
                    if (lvi.Text != tRow.Text.ToLower())
                        continue;
                }

                if (tCmd.Text != "") {
                    noth_found = false;
                    if (cCmdW.Checked) {
                        string[] spl1, spl2;

                        spl1 = tCmd.Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        spl2 = lvi.SubItems[1].Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        bool nfound = true;
                        byte clook = 0;
                        if (cCmdC.Checked) {
                            for (ushort i = 0; i != spl2.Length; ++i) {
                                if (spl1[clook] == spl2[i]) {
                                    ++clook;
                                    if (clook == spl1.Length) {
                                        nfound = false;
                                        break;
                                    }
                                } else {
                                    i -= clook;
                                    clook = 0;
                                }
                            }
                        } else {
                            for (ushort i = 0; i != spl2.Length; ++i) {
                                if (spl1[clook].ToLower() == spl2[i].ToLower()) {
                                    ++clook;
                                    if (clook == spl1.Length) {
                                        nfound = false;
                                        break;
                                    }
                                } else {
                                    i -= clook;
                                    clook = 0;
                                }
                            }
                        }

                        if (nfound)
                            continue;
                    } else {
                        if (cCmdC.Checked) {
                            if (!lvi.SubItems[1].Text.Contains(tCmd.Text))
                                continue;
                        } else {
                            if (!lvi.SubItems[1].Text.ToLower().Contains(tCmd.Text.ToLower()))
                                continue;
                        }
                    }
                }

                if (tDesc.Text != "") {
                    noth_found = false;
                    if (cDescW.Checked) {
                        string[] spl1, spl2;

                        spl1 = tDesc.Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        spl2 = lvi.ToolTipText.Split(new char[] { ' ', ',', '.', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                        bool nfound = true;
                        byte clook = 0;
                        if (cDescC.Checked) {
                            for (ushort i = 0; i != spl2.Length; ++i) {
                                if (spl1[clook] == spl2[i]) {
                                    ++clook;
                                    if (clook == spl1.Length) {
                                        nfound = false;
                                        break;
                                    }
                                } else {
                                    i -= clook;
                                    clook = 0;
                                }
                            }
                        } else {
                            for (ushort i = 0; i != spl2.Length; ++i) {
                                if (spl1[clook].ToLower() == spl2[i].ToLower()) {
                                    ++clook;
                                    if (clook == spl1.Length) {
                                        nfound = false;
                                        break;
                                    }
                                } else {
                                    i -= clook;
                                    clook = 0;
                                }
                            }
                        }

                        if (nfound)
                            continue;
                    } else {
                        if (cDescC.Checked) {
                            if (!lvi.ToolTipText.Contains(tDesc.Text))
                                continue;
                        } else {
                            if (!lvi.ToolTipText.ToLower().Contains(tDesc.Text.ToLower()))
                                continue;
                        }
                    }
                }

                if (tFigure.Text != "") {
                    noth_found = false;
                    if (!lvi.SubItems[3].Text.ToLower().Contains(tFigure.Text.ToLower()))
                        continue;
                }

                if (tConn.Text != "") {
                    noth_found = false;
                    if (lvi.SubItems[4].Text.ToLower() != tConn.Text.ToLower())
                        continue;
                }

                if (tMCmd.Text != "") {
                    noth_found = false;
                    if (cMCmdW.Checked) {
                        string[] spl1, spl2;

                        spl1 = tMCmd.Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        spl2 = lvi.SubItems[5].Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        bool nfound = true;
                        byte clook = 0;
                        if (cMCmdC.Checked) {
                            for (ushort i = 0; i != spl2.Length; ++i) {
                                if (spl1[clook] == spl2[i]) {
                                    ++clook;
                                    if (clook == spl1.Length) {
                                        nfound = false;
                                        break;
                                    }
                                } else {
                                    i -= clook;
                                    clook = 0;
                                }
                            }
                        } else {
                            for (ushort i = 0; i != spl2.Length; ++i) {
                                if (spl1[clook].ToLower() == spl2[i].ToLower()) {
                                    ++clook;
                                    if (clook == spl1.Length) {
                                        nfound = false;
                                        break;
                                    }
                                } else {
                                    i -= clook;
                                    clook = 0;
                                }
                            }
                        }

                        if (nfound)
                            continue;
                    } else {
                        if (cMCmdC.Checked) {
                            if (!lvi.SubItems[5].Text.Contains(tMCmd.Text))
                                continue;
                        } else {
                            if (!lvi.SubItems[5].Text.ToLower().Contains(tMCmd.Text.ToLower()))
                                continue;
                        }
                    }
                }

                if (noth_found)
                    continue;
                break;
            }

            if (cReplaceCmd.Checked) {
                if (lvi.SubItems[1].Text != tNewCmd.Text)
                    lvi.SubItems[5].Text = "";

                lvi.SubItems[1].Text = tNewCmd.Text;
            }

            if (cReplaceDesc.Checked) {
                switch (parent.model[cindex].figure) {
                    case MainForm.BlockModel.Figure.Connector:
                        break;
                    case MainForm.BlockModel.Figure.LoopS:
                    case MainForm.BlockModel.Figure.LoopE:
                        lvi.ToolTipText = parent.model[cindex].row2 = tNewDesc.Text.Replace("{loop letter}", "").Replace("{\\n}", "").Replace("\r", "").Trim();
                        lvi.SubItems[2].Text = "{loop letter} " + string.Join("{\\n}", lvi.ToolTipText.Split('\n'));
                        break;
                    default:
                        lvi.ToolTipText = parent.model[cindex].row2 = tNewDesc.Text.Replace("{loop letter}", "").Replace("{\\n}", "").Replace("\r", "").Trim();
                        lvi.SubItems[2].Text = string.Join("{\\n}", lvi.ToolTipText.Split('\n'));
                        break;
                }
            }
            parent.record.Add("WHERE BLOCK");
            if (tRow.Text != "")
                parent.record.Add("ROW " + tRow.Text);
            if (tCmd.Text != "") {
                parent.record.Add("COMMAND " + tCmd.Text);
                parent.record.Add("MODE COMMAND " + (cCmdW.Checked ? "WHOLE " : "LIKE ") + (cCmdC.Checked ? "CASE" : "NOCASE"));
            }
            if (tDesc.Text != "") {
                parent.record.Add("DESCRIPTION " + tDesc.Text);
                parent.record.Add("MODE DESCRIPTION " + (cDescW.Checked ? "WHOLE " : "LIKE ") + (cDescC.Checked ? "CASE" : "NOCASE"));
            }
            if (tFigure.Text != "")
                parent.record.Add("FIGURE " + tFigure.Text);
            if (tConn.Text != "")
                parent.record.Add("CONNECTION " + tConn.Text);
            if (tMCmd.Text != "") {
                parent.record.Add("METACMD " + tMCmd.Text);
                parent.record.Add("MODE METACMD " + (cMCmdW.Checked ? "WHOLE " : "LIKE ") + (cMCmdC.Checked ? "CASE" : "NOCASE"));
            }
            parent.record.Add("SET DATA FROM CURSOR" + (cWA.Checked ? " WITH WRAP" : "") + " BY");
            if (cReplaceCmd.Checked) parent.record.Add("COMMAND " + tNewCmd.Text);
            if (cReplaceDesc.Checked) {
                parent.record.Add("DESCRIPTION BEGIN");
                foreach (var item in lvi.ToolTipText.Split('\n')) {
                    if (!item.StartsWith("UPDATE")) {
                        parent.record.Add(item);
                    } else {
                        parent.record.Add("{\\n}" + item);
                    }
                }
            }
            parent.record.Add("UPDATE ANY");
            parent.record.Commit(BlockEdit.CommandRecord.FunctionType.BOTH);

            parent.lv.SelectedIndices.Clear();
            parent.lv.SelectedIndices.Add(cindex);
            parent.record.RemoveLatest();
        }

        private void button4_Click(object sender, EventArgs e) {
            if (MessageBox.Show(this, "Are you sure you want to replace fields in all matching elements?\nВы уверены, что хотите заменить данные во всех подходящих элементах?", "Replace ALL", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No) return;

            int cindex = -1;
            ushort repl_count = 0;

            ListViewItem lvi = null;
            tDesc.Text = tDesc.Text.Replace("{loop letter}", "").Replace("{\\n}", "");



            while (true) {
                while (true) {
                    if (parent.lv.Items.Count == ++cindex) {
                        goto end;
                    }

                    lvi = parent.lv.Items[cindex];
                    bool noth_found = true;

                    if (tRow.Text != "") {
                        noth_found = false;
                        if (lvi.Text != tRow.Text.ToLower())
                            continue;
                    }

                    if (tCmd.Text != "") {
                        noth_found = false;
                        if (cCmdW.Checked) {
                            string[] spl1, spl2;

                            spl1 = tCmd.Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            spl2 = lvi.SubItems[1].Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                            bool nfound = true;
                            byte clook = 0;
                            if (cCmdC.Checked) {
                                for (ushort i = 0; i != spl2.Length; ++i) {
                                    if (spl1[clook] == spl2[i]) {
                                        ++clook;
                                        if (clook == spl1.Length) {
                                            nfound = false;
                                            break;
                                        }
                                    } else {
                                        i -= clook;
                                        clook = 0;
                                    }
                                }
                            } else {
                                for (ushort i = 0; i != spl2.Length; ++i) {
                                    if (spl1[clook].ToLower() == spl2[i].ToLower()) {
                                        ++clook;
                                        if (clook == spl1.Length) {
                                            nfound = false;
                                            break;
                                        }
                                    } else {
                                        i -= clook;
                                        clook = 0;
                                    }
                                }
                            }

                            if (nfound)
                                continue;
                        } else {
                            if (cCmdC.Checked) {
                                if (!lvi.SubItems[1].Text.Contains(tCmd.Text))
                                    continue;
                            } else {
                                if (!lvi.SubItems[1].Text.ToLower().Contains(tCmd.Text.ToLower()))
                                    continue;
                            }
                        }
                    }

                    if (tDesc.Text != "") {
                        noth_found = false;
                        if (cDescW.Checked) {
                            string[] spl1, spl2;

                            spl1 = tDesc.Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            spl2 = lvi.ToolTipText.Split(new char[] { ' ', ',', '.', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                            bool nfound = true;
                            byte clook = 0;
                            if (cDescC.Checked) {
                                for (ushort i = 0; i != spl2.Length; ++i) {
                                    if (spl1[clook] == spl2[i]) {
                                        ++clook;
                                        if (clook == spl1.Length) {
                                            nfound = false;
                                            break;
                                        }
                                    } else {
                                        i -= clook;
                                        clook = 0;
                                    }
                                }
                            } else {
                                for (ushort i = 0; i != spl2.Length; ++i) {
                                    if (spl1[clook].ToLower() == spl2[i].ToLower()) {
                                        ++clook;
                                        if (clook == spl1.Length) {
                                            nfound = false;
                                            break;
                                        }
                                    } else {
                                        i -= clook;
                                        clook = 0;
                                    }
                                }
                            }

                            if (nfound)
                                continue;
                        } else {
                            if (cDescC.Checked) {
                                if (!lvi.ToolTipText.Contains(tDesc.Text))
                                    continue;
                            } else {
                                if (!lvi.ToolTipText.ToLower().Contains(tDesc.Text.ToLower()))
                                    continue;
                            }
                        }
                    }

                    if (tFigure.Text != "") {
                        noth_found = false;
                        if (!lvi.SubItems[3].Text.ToLower().Contains(tFigure.Text.ToLower()))
                            continue;
                    }

                    if (tConn.Text != "") {
                        noth_found = false;
                        if (lvi.SubItems[4].Text.ToLower() != tConn.Text.ToLower())
                            continue;
                    }

                    if (tMCmd.Text != "") {
                        noth_found = false;
                        if (cMCmdW.Checked) {
                            string[] spl1, spl2;

                            spl1 = tMCmd.Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            spl2 = lvi.SubItems[5].Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                            bool nfound = true;
                            byte clook = 0;
                            if (cMCmdC.Checked) {
                                for (ushort i = 0; i != spl2.Length; ++i) {
                                    if (spl1[clook] == spl2[i]) {
                                        ++clook;
                                        if (clook == spl1.Length) {
                                            nfound = false;
                                            break;
                                        }
                                    } else {
                                        i -= clook;
                                        clook = 0;
                                    }
                                }
                            } else {
                                for (ushort i = 0; i != spl2.Length; ++i) {
                                    if (spl1[clook].ToLower() == spl2[i].ToLower()) {
                                        ++clook;
                                        if (clook == spl1.Length) {
                                            nfound = false;
                                            break;
                                        }
                                    } else {
                                        i -= clook;
                                        clook = 0;
                                    }
                                }
                            }

                            if (nfound)
                                continue;
                        } else {
                            if (cMCmdC.Checked) {
                                if (!lvi.SubItems[5].Text.Contains(tMCmd.Text))
                                    continue;
                            } else {
                                if (!lvi.SubItems[5].Text.ToLower().Contains(tMCmd.Text.ToLower()))
                                    continue;
                            }
                        }
                    }

                    if (noth_found)
                        continue;
                    break;
                }
                ++repl_count;

                if (cReplaceCmd.Checked) {
                    if (lvi.SubItems[1].Text != tNewCmd.Text)
                        lvi.SubItems[5].Text = "";

                    lvi.SubItems[1].Text = tNewCmd.Text;
                }

                if (cReplaceDesc.Checked) {
                    switch (parent.model[cindex].figure) {
                        case MainForm.BlockModel.Figure.Connector:
                            break;
                        case MainForm.BlockModel.Figure.LoopS:
                        case MainForm.BlockModel.Figure.LoopE:
                            lvi.ToolTipText = parent.model[cindex].row2 = tNewDesc.Text.Replace("{loop letter}", "").Replace("{\\n}", "").Replace("\r", "").Trim();
                            lvi.SubItems[2].Text = "{loop letter} " + string.Join("{\\n}", lvi.ToolTipText.Split('\n'));
                            break;
                        default:
                            lvi.ToolTipText = parent.model[cindex].row2 = tNewDesc.Text.Replace("{loop letter}", "").Replace("{\\n}", "").Replace("\r", "").Trim();
                            lvi.SubItems[2].Text = string.Join("{\\n}", lvi.ToolTipText.Split('\n'));
                            break;
                    }
                }
            }
        end:;
            parent.record.Add("WHERE BLOCK");
            if (tRow.Text != "")
                parent.record.Add("ROW " + tRow.Text);
            if (tCmd.Text != "") {
                parent.record.Add("COMMAND " + tCmd.Text);
                parent.record.Add("MODE COMMAND " + (cCmdW.Checked ? "WHOLE " : "LIKE ") + (cCmdC.Checked ? "CASE" : "NOCASE"));
            }
            if (tDesc.Text != "") {
                parent.record.Add("DESCRIPTION " + tDesc.Text);
                parent.record.Add("MODE DESCRIPTION " + (cDescW.Checked ? "WHOLE " : "LIKE ") + (cDescC.Checked ? "CASE" : "NOCASE"));
            }
            if (tFigure.Text != "")
                parent.record.Add("FIGURE " + tFigure.Text);
            if (tConn.Text != "")
                parent.record.Add("CONNECTION " + tConn.Text);
            if (tMCmd.Text != "") {
                parent.record.Add("METACMD " + tMCmd.Text);
                parent.record.Add("MODE METACMD " + (cMCmdW.Checked ? "WHOLE " : "LIKE ") + (cMCmdC.Checked ? "CASE" : "NOCASE"));
            }
            parent.record.Add("SET DATA FROM BEGIN");
            if (cReplaceCmd.Checked) parent.record.Add("COMMAND " + tNewCmd.Text);
            if (cReplaceDesc.Checked) {
                parent.record.Add("DESCRIPTION BEGIN");
                foreach (var item in lvi.ToolTipText.Split('\n')) {
                    if (!item.StartsWith("UPDATE")) {
                        parent.record.Add(item);
                    } else {
                        parent.record.Add("{\\n}" + item);
                    }
                }
            }
            parent.record.Add("UPDATE ALL");
            parent.record.Commit(BlockEdit.CommandRecord.FunctionType.NONE);

            MessageBox.Show(this, "Replaces count: " + repl_count + "\nКоличество сделанных замен: " + repl_count, "Replace results", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
