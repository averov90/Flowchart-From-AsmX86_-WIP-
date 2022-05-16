using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FlowchartFromAsmX86 {
    public partial class BlockEdit : Form {
        internal CommandRecord record;

        internal List<MainForm.BlockModel> model;

        bool rus;
        ushort ucmds = 0;

        Find fform;
        internal BlockEdit(List<MainForm.BlockModel> model, List<MainForm.AsmCommand> acmd, ref string[] code, string filename, bool rus) {
            InitializeComponent();

            lvi_default = new ListViewItem("==>");
            lvi_default.ToolTipText = "Select this row to replace it by a copy of the real block.";
            lvi_default.SubItems.Add("");
            lvi_default.SubItems.Add("");
            lvi_default.SubItems.Add("");
            lvi_default.SubItems.Add("");
            lvi_default.SubItems.Add("");

            this.model = model;
            this.rus = rus;

            for (int i = 0; i != model.Count; ++i) {
                ListViewItem tvi = new ListViewItem((acmd[model[i].code_ref].line + 1).ToString());
                tvi.ToolTipText = model[i].row2;
                switch (model[i].figure) {
                    case MainForm.BlockModel.Figure.FDecision:
                    case MainForm.BlockModel.Figure.JDecision:
                        tvi.SubItems.Add(code[acmd[model[i].code_ref].line]);
                        tvi.SubItems.Add(string.Join("{\\n}", model[i].row2.Split('\n')));
                        tvi.SubItems.Add(model[i].figure.ToString());
                        tvi.SubItems.Add((acmd[model[model[i].connection].code_ref].line + 1).ToString());
                        tvi.SubItems.Add(acmd[model[i].code_ref].name + " " + (acmd[model[i].code_ref].arg1.arg ?? "") + (acmd[model[i].code_ref].arg2.arg != null ? "," + acmd[model[i].code_ref].arg2.arg : ""));
                        break;
                    case MainForm.BlockModel.Figure.Connector:
                        tvi.SubItems.Add(code[acmd[model[i].code_ref].line]);
                        tvi.SubItems.Add("");
                        tvi.SubItems.Add("");
                        tvi.SubItems.Add((acmd[model[model[i].connection].code_ref].line + 1).ToString());
                        tvi.SubItems.Add(acmd[model[i].code_ref].name + " " + (acmd[model[i].code_ref].arg1.arg ?? "") + (acmd[model[i].code_ref].arg2.arg != null ? "," + acmd[model[i].code_ref].arg2.arg : ""));
                        break;
                    case MainForm.BlockModel.Figure.LoopS:
                        tvi.SubItems.Add("");
                        tvi.SubItems.Add("{loop letter} " + string.Join("{\\n}", model[i].row2.Split('\n')));
                        tvi.SubItems.Add(model[i].figure.ToString());
                        tvi.SubItems.Add("");
                        tvi.SubItems.Add("");
                        break;
                    case MainForm.BlockModel.Figure.LoopE:
                        tvi.SubItems.Add(code[acmd[model[i].code_ref].line]);
                        tvi.SubItems.Add("{loop letter} " + string.Join("{\\n}", model[i].row2.Split('\n')));
                        tvi.SubItems.Add(model[i].figure.ToString());
                        tvi.SubItems.Add("");
                        tvi.SubItems.Add(acmd[model[i].code_ref].name + " " + (acmd[model[i].code_ref].arg1.arg ?? "") + (acmd[model[i].code_ref].arg2.arg != null ? "," + acmd[model[i].code_ref].arg2.arg : ""));
                        break;
                    default:
                        tvi.SubItems.Add(code[acmd[model[i].code_ref].line]);
                        tvi.SubItems.Add(string.Join("{\\n}", model[i].row2.Split('\n')));
                        tvi.SubItems.Add(model[i].figure.ToString());
                        tvi.SubItems.Add("");
                        tvi.SubItems.Add(acmd[model[i].code_ref].name + " " + (acmd[model[i].code_ref].arg1.arg ?? "") + (acmd[model[i].code_ref].arg2.arg != null ? "," + acmd[model[i].code_ref].arg2.arg : ""));
                        break;
                }

                lv.Items.Add(tvi);
            }


            fform = new Find(this);
            record = new CommandRecord(filename);
        }

        private void editDescriptionToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lv.SelectedIndices.Count != 1) return;
            fform.Hide();

            EditDescription edit = new EditDescription(model[lv.SelectedIndices[0]].row2);
            if (edit.ShowDialog() == DialogResult.OK) {
                switch (model[lv.SelectedIndices[0]].figure) {
                    case MainForm.BlockModel.Figure.LoopS:
                    case MainForm.BlockModel.Figure.LoopE:
                        lv.SelectedItems[0].SubItems[2].Text = "{loop letter} " + string.Join("{\\n}", (lv.SelectedItems[0].ToolTipText = model[lv.SelectedIndices[0]].row2 = edit.description.Text.Replace("{loop letter}", "").Replace("{\\n}", "").Replace("\r", "").Trim()).Split('\n'));
                        break;
                    default:
                        lv.SelectedItems[0].SubItems[2].Text = string.Join("{\\n}", (lv.SelectedItems[0].ToolTipText = model[lv.SelectedIndices[0]].row2 = edit.description.Text.Replace("{loop letter}", "").Replace("{\\n}", "").Replace("\r", "").Trim()).Split('\n'));
                        break;
                }

                record.Add("UPDATE DESCRIPTION BEGIN");
                foreach (var item in lv.SelectedItems[0].ToolTipText.Split('\n')) {
                    if (!item.StartsWith("UPDATE")) {
                        record.Add(item);
                    } else {
                        record.Add("{\\n}" + item);
                    }
                }
                record.Add("UPDATE DESCRIPTION END");
                record.Commit(CommandRecord.FunctionType.USER);
            }
        }

        private void lv_DoubleClick(object sender, EventArgs e) {
            editDescriptionToolStripMenuItem_Click(null, null);
        }

        private void editBlockToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lv.SelectedIndices.Count != 1) return;

            EditBlock(model[lv.SelectedIndices[0]].figure, false);
        }

        private void EditBlock(MainForm.BlockModel.Figure cfigure, bool csource) {
            EditBlock edit = new EditBlock(RetrieveVirtualItem);

            edit.type.SelectedIndex = (int)cfigure;
            edit.description.Lines = model[lv.SelectedIndices[0]].row2.Split('\n');
            edit.lv.VirtualListSize = lv.Items.Count;
            edit.textCmd.Text = lv.Items[lv.SelectedIndices[0]].SubItems[1].Text;

            if (csource)
                edit.chbx_clearCmd.Checked = true;

            switch (model[lv.SelectedIndices[0]].figure) {
                case MainForm.BlockModel.Figure.FDecision:
                case MainForm.BlockModel.Figure.JDecision:
                case MainForm.BlockModel.Figure.Connector:
                    edit.connection = model[lv.SelectedIndices[0]].connection;
                    break;
            }
            fform.Hide();

            if (edit.ShowDialog() == DialogResult.OK) {
                lv.SelectedItems[0].SubItems[3].Text = (model[lv.SelectedIndices[0]].figure = (MainForm.BlockModel.Figure)edit.type.SelectedIndex).ToString();
                if (lv.SelectedItems[0].SubItems[1].Text != edit.textCmd.Text) {
                    lv.SelectedItems[0].SubItems[1].Text = edit.textCmd.Text;
                    lv.SelectedItems[0].SubItems[5].Text = "";
                }


                record.Add("UPDATE BLOCK BEGIN");
                record.Add("FIGURE " + ((MainForm.BlockModel.Figure)edit.type.SelectedIndex).ToString());
                record.Add("COMMAND " + edit.textCmd.Text);

                switch ((MainForm.BlockModel.Figure)edit.type.SelectedIndex) {
                    case MainForm.BlockModel.Figure.FDecision:
                    case MainForm.BlockModel.Figure.JDecision:
                        if (model[lv.SelectedIndices[0]].connection != -1)
                            model[model[lv.SelectedIndices[0]].connection].connectedToMe.Remove(lv.SelectedIndices[0]);
                        model[lv.SelectedIndices[0]].connection = edit.connection;
                        model[edit.connection].connectedToMe.Add(lv.SelectedIndices[0]);

                        lv.SelectedItems[0].SubItems[2].Text = string.Join("{\\n}", (lv.SelectedItems[0].ToolTipText = model[lv.SelectedIndices[0]].row2 = edit.description.Text.Replace("{loop letter}", "").Replace("{\\n}", "").Replace("\r", "").Trim()).Split('\n'));
                        lv.SelectedItems[0].SubItems[4].Text = lv.Items[edit.connection].Text;

                        record.Add("CONNECTION " + lv.Items[edit.connection].Text);
                        record.Add("DESCRIPTION BEGIN");
                        foreach (var item in lv.SelectedItems[0].ToolTipText.Split('\n')) {
                            if (!item.StartsWith("UPDATE")) {
                                record.Add(item);
                            } else {
                                record.Add("{\\n}" + item);
                            }
                        }
                        record.Add("UPDATE BLOCK END");
                        break;
                    case MainForm.BlockModel.Figure.Connector:
                        if (model[lv.SelectedIndices[0]].connection != -1)
                            model[model[lv.SelectedIndices[0]].connection].connectedToMe.Remove(lv.SelectedIndices[0]);
                        model[lv.SelectedIndices[0]].connection = edit.connection;
                        model[edit.connection].connectedToMe.Add(lv.SelectedIndices[0]);

                        lv.SelectedItems[0].SubItems[2].Text = lv.SelectedItems[0].ToolTipText = model[lv.SelectedIndices[0]].row2 = "";
                        lv.SelectedItems[0].SubItems[4].Text = lv.Items[edit.connection].Text;

                        record.Add("CONNECTION " + lv.Items[edit.connection].Text);
                        record.Add("UPDATE BLOCK END");
                        break;
                    case MainForm.BlockModel.Figure.LoopS:
                    case MainForm.BlockModel.Figure.LoopE:
                        if (model[lv.SelectedIndices[0]].connection != -1) {
                            model[model[lv.SelectedIndices[0]].connection].connectedToMe.Remove(lv.SelectedIndices[0]);
                            model[lv.SelectedIndices[0]].connection = -1;
                        }

                        lv.SelectedItems[0].SubItems[2].Text = "{loop letter} " + string.Join("{\\n}", (lv.SelectedItems[0].ToolTipText = model[lv.SelectedIndices[0]].row2 = edit.description.Text.Replace("{loop letter}", "").Replace("{\\n}", "").Replace("\r", "").Trim()).Split('\n'));
                        lv.SelectedItems[0].SubItems[4].Text = "";

                        record.Add("DESCRIPTION BEGIN");
                        foreach (var item in lv.SelectedItems[0].ToolTipText.Split('\n')) {
                            if (!item.StartsWith("UPDATE")) {
                                record.Add(item);
                            } else {
                                record.Add("{\\n}" + item);
                            }
                        }
                        record.Add("UPDATE BLOCK END");
                        break;
                    default:
                        if (model[lv.SelectedIndices[0]].connection != -1) {
                            model[model[lv.SelectedIndices[0]].connection].connectedToMe.Remove(lv.SelectedIndices[0]);
                            model[lv.SelectedIndices[0]].connection = -1;
                        }

                        lv.SelectedItems[0].SubItems[2].Text = string.Join("{\\n}", (lv.SelectedItems[0].ToolTipText = model[lv.SelectedIndices[0]].row2 = edit.description.Text.Replace("{loop letter}", "").Replace("{\\n}", "").Replace("\r", "").Trim()).Split('\n'));
                        lv.SelectedItems[0].SubItems[4].Text = "";

                        record.Add("DESCRIPTION BEGIN");
                        foreach (var item in lv.SelectedItems[0].ToolTipText.Split('\n')) {
                            if (!item.StartsWith("UPDATE")) {
                                record.Add(item);
                            } else {
                                record.Add("{\\n}" + item);
                            }
                        }
                        record.Add("UPDATE BLOCK END");
                        break;
                }

                record.Commit(CommandRecord.FunctionType.USER);
            }
        }

        bool rvi_same = true;
        ListViewItem lvi_default;
        private void RetrieveVirtualItem(ref RetrieveVirtualItemEventArgs e) {
            if (rvi_same)
                e.Item = (ListViewItem)lv.Items[e.ItemIndex].Clone();
            else {
                if ((e.ItemIndex & 1) == 0)
                    e.Item = lvi_default;
                else
                    e.Item = (ListViewItem)lv.Items[e.ItemIndex / 2].Clone();
            }
        }

        private void insertBlockbeforeToolStripMenuItem_Click(object sender, EventArgs e) {
            EditBlock edit = new EditBlock(RetrieveVirtualItem);
            edit.Text = "Create block";
            edit.type.SelectedIndex = 0;
            edit.chbx_clearCmd.Visible = false;
            edit.lv.VirtualListSize = lv.Items.Count;

            fform.Hide();

            if (edit.ShowDialog() == DialogResult.OK) {
                if (model.Count == 0) {
                    model.Add(new MainForm.BlockModel((MainForm.BlockModel.Figure)edit.type.SelectedIndex, edit.description.Text.Replace("{loop letter}", "").Replace("{\\n}", "").Replace("\r", "").Trim(), -1));

                    ListViewItem tvi = new ListViewItem("u" + ++ucmds);
                    tvi.ToolTipText = model.First().row2;
                    tvi.SubItems.Add(edit.textCmd.Text);


                    record.Add("INSERT BLOCK BEGIN");
                    record.Add("FIGURE " + ((MainForm.BlockModel.Figure)edit.type.SelectedIndex).ToString());
                    record.Add("COMMAND " + edit.textCmd.Text);

                    switch (model.First().figure) {
                        case MainForm.BlockModel.Figure.LoopS:
                        case MainForm.BlockModel.Figure.LoopE:
                            tvi.SubItems.Add("{loop letter} " + string.Join("{\\n}", model.First().row2.Split('\n')));
                            tvi.SubItems.Add(model.First().figure.ToString());
                            break;
                        default:
                            tvi.SubItems.Add(string.Join("{\\n}", model.First().row2.Split('\n')));
                            tvi.SubItems.Add(model.First().figure.ToString());
                            break;
                    }
                    tvi.SubItems.Add("");
                    tvi.SubItems.Add("");

                    record.Add("DESCRIPTION BEGIN");
                    foreach (var item in tvi.ToolTipText.Split('\n')) {
                        if (!item.StartsWith("INSERT")) {
                            record.Add(item);
                        } else {
                            record.Add("{\\n}" + item);
                        }
                    }
                    record.Add("INSERT BLOCK END");
                    record.Commit(CommandRecord.FunctionType.NONE);

                    lv.Items.Add(tvi);
                } else if (lv.SelectedIndices.Count == 1) {
                    int insert_index = lv.SelectedIndices[0];

                    ListViewItem tvi = new ListViewItem("u" + ++ucmds);
                    tvi.SubItems.Add(edit.textCmd.Text);


                    record.Add("INSERT BLOCK BEGIN");
                    record.Add("POSITION BEFORE");
                    record.Add("FIGURE " + ((MainForm.BlockModel.Figure)edit.type.SelectedIndex).ToString());
                    record.Add("COMMAND " + edit.textCmd.Text);

                    switch ((MainForm.BlockModel.Figure)edit.type.SelectedIndex) {
                        case MainForm.BlockModel.Figure.FDecision:
                        case MainForm.BlockModel.Figure.JDecision:
                            model.Insert(insert_index, new MainForm.BlockModel((MainForm.BlockModel.Figure)edit.type.SelectedIndex, edit.description.Text.Replace("{loop letter}", "").Replace("{\\n}", "").Replace("\r", "").Trim(), -1, edit.connection));

                            tvi.ToolTipText = model[insert_index].row2;
                            tvi.SubItems.Add(string.Join("{\\n}", model[insert_index].row2.Split('\n')));
                            tvi.SubItems.Add(model[insert_index].figure.ToString());
                            tvi.SubItems.Add(lv.Items[edit.connection].Text);

                            record.Add("CONNECTION " + lv.Items[edit.connection].Text);
                            record.Add("DESCRIPTION BEGIN");
                            foreach (var item in tvi.ToolTipText.Split('\n')) {
                                if (!item.StartsWith("INSERT")) {
                                    record.Add(item);
                                } else {
                                    record.Add("{\\n}" + item);
                                }
                            }
                            record.Add("INSERT BLOCK END");
                            break;
                        case MainForm.BlockModel.Figure.Connector:
                            model.Insert(insert_index, new MainForm.BlockModel(MainForm.BlockModel.Figure.Connector, "", -1, edit.connection));

                            tvi.SubItems.Add("");
                            tvi.SubItems.Add(MainForm.BlockModel.Figure.Connector.ToString());
                            tvi.SubItems.Add(lv.Items[edit.connection].Text);

                            record.Add("CONNECTION " + lv.Items[edit.connection].Text);
                            record.Add("INSERT BLOCK END");
                            break;
                        case MainForm.BlockModel.Figure.LoopS:
                        case MainForm.BlockModel.Figure.LoopE:
                            model.Insert(insert_index, new MainForm.BlockModel((MainForm.BlockModel.Figure)edit.type.SelectedIndex, edit.description.Text.Replace("{loop letter}", "").Replace("{\\n}", "").Replace("\r", "").Trim(), -1));

                            tvi.ToolTipText = model[insert_index].row2;
                            tvi.SubItems.Add("{loop letter} " + string.Join("{\\n}", model[insert_index].row2.Split('\n')));
                            tvi.SubItems.Add(model[insert_index].figure.ToString());
                            tvi.SubItems.Add("");

                            record.Add("DESCRIPTION BEGIN");
                            foreach (var item in tvi.ToolTipText.Split('\n')) {
                                if (!item.StartsWith("INSERT")) {
                                    record.Add(item);
                                } else {
                                    record.Add("{\\n}" + item);
                                }
                            }
                            record.Add("INSERT BLOCK END");
                            break;
                        default:
                            model.Insert(insert_index, new MainForm.BlockModel((MainForm.BlockModel.Figure)edit.type.SelectedIndex, edit.description.Text.Replace("{loop letter}", "").Replace("{\\n}", "").Replace("\r", "").Trim(), -1));

                            tvi.ToolTipText = model[insert_index].row2;
                            tvi.SubItems.Add(string.Join("{\\n}", model[insert_index].row2.Split('\n')));
                            tvi.SubItems.Add(model[insert_index].figure.ToString());
                            tvi.SubItems.Add("");

                            record.Add("DESCRIPTION BEGIN");
                            foreach (var item in tvi.ToolTipText.Split('\n')) {
                                if (!item.StartsWith("INSERT")) {
                                    record.Add(item);
                                } else {
                                    record.Add("{\\n}" + item);
                                }
                            }
                            record.Add("INSERT BLOCK END");
                            break;
                    }
                    tvi.SubItems.Add("");

                    record.Commit(CommandRecord.FunctionType.USER);

                    lv.Items.Insert(insert_index--, tvi);

                    MainForm.BlockModel tmp;
                    for (int i = 0; i != model.Count; ++i) {
                        tmp = model[i];
                        if (tmp.connection > insert_index)
                            ++tmp.connection;
                        for (int j = 0; j < tmp.connectedToMe.Count; ++j) {
                            if (tmp.connectedToMe[j] > insert_index)
                                ++tmp.connectedToMe[j];
                        }
                    }

                    if (model[++insert_index].connection > -1)
                        model[model[insert_index].connection].connectedToMe.Add(insert_index);
                }
            }
        }

        private void insertBlockafterToolStripMenuItem_Click(object sender, EventArgs e) {
            EditBlock edit = new EditBlock(RetrieveVirtualItem);
            edit.Text = "Create block";
            edit.type.SelectedIndex = 0;
            edit.chbx_clearCmd.Visible = false;
            edit.lv.VirtualListSize = lv.Items.Count;

            fform.Hide();

            if (edit.ShowDialog() == DialogResult.OK) {
                if (model.Count == 0) {
                    model.Add(new MainForm.BlockModel((MainForm.BlockModel.Figure)edit.type.SelectedIndex, edit.description.Text.Replace("{loop letter}", "").Replace("{\\n}", "").Replace("\r", "").Trim(), -1));

                    ListViewItem tvi = new ListViewItem("u" + ++ucmds);
                    tvi.ToolTipText = model.First().row2;
                    tvi.SubItems.Add(edit.textCmd.Text);


                    record.Add("INSERT BLOCK BEGIN");
                    record.Add("FIGURE " + ((MainForm.BlockModel.Figure)edit.type.SelectedIndex).ToString());
                    record.Add("COMMAND " + edit.textCmd.Text);

                    switch (model.First().figure) {
                        case MainForm.BlockModel.Figure.LoopS:
                        case MainForm.BlockModel.Figure.LoopE:
                            tvi.SubItems.Add("{loop letter} " + string.Join("{\\n}", model.First().row2.Split('\n')));
                            tvi.SubItems.Add(model.First().figure.ToString());
                            break;
                        default:
                            tvi.SubItems.Add(string.Join("{\\n}", model.First().row2.Split('\n')));
                            tvi.SubItems.Add(model.First().figure.ToString());
                            break;
                    }
                    tvi.SubItems.Add("");
                    tvi.SubItems.Add("");

                    record.Add("DESCRIPTION BEGIN");
                    foreach (var item in tvi.ToolTipText.Split('\n')) {
                        if (!item.StartsWith("INSERT")) {
                            record.Add(item);
                        } else {
                            record.Add("{\\n}" + item);
                        }
                    }
                    record.Add("INSERT BLOCK END");
                    record.Commit(CommandRecord.FunctionType.NONE);

                    lv.Items.Add(tvi);
                } else if (lv.SelectedIndices.Count == 1) {
                    int insert_index = lv.SelectedIndices[0] + 1;

                    ListViewItem tvi = new ListViewItem("u" + ++ucmds);
                    tvi.SubItems.Add(edit.textCmd.Text);


                    record.Add("INSERT BLOCK BEGIN");
                    record.Add("POSITION AFTER");
                    record.Add("FIGURE " + ((MainForm.BlockModel.Figure)edit.type.SelectedIndex).ToString());
                    record.Add("COMMAND " + edit.textCmd.Text);

                    switch ((MainForm.BlockModel.Figure)edit.type.SelectedIndex) {
                        case MainForm.BlockModel.Figure.FDecision:
                        case MainForm.BlockModel.Figure.JDecision:
                            if (insert_index != model.Count)
                                model.Insert(insert_index, new MainForm.BlockModel((MainForm.BlockModel.Figure)edit.type.SelectedIndex, edit.description.Text.Replace("{loop letter}", "").Replace("{\\n}", "").Replace("\r", "").Trim(), -1, edit.connection));
                            else
                                model.Add(new MainForm.BlockModel((MainForm.BlockModel.Figure)edit.type.SelectedIndex, edit.description.Text.Replace("{loop letter}", "").Replace("{\\n}", "").Replace("\r", "").Trim(), -1, edit.connection));

                            tvi.ToolTipText = model[insert_index].row2;
                            tvi.SubItems.Add(string.Join("{\\n}", model[insert_index].row2.Split('\n')));
                            tvi.SubItems.Add(model[insert_index].figure.ToString());
                            tvi.SubItems.Add(lv.Items[edit.connection].Text);

                            record.Add("CONNECTION " + lv.Items[edit.connection].Text);
                            record.Add("DESCRIPTION BEGIN");
                            foreach (var item in tvi.ToolTipText.Split('\n')) {
                                if (!item.StartsWith("INSERT")) {
                                    record.Add(item);
                                } else {
                                    record.Add("{\\n}" + item);
                                }
                            }
                            record.Add("INSERT BLOCK END");
                            break;
                        case MainForm.BlockModel.Figure.Connector:
                            if (insert_index != model.Count)
                                model.Insert(insert_index, new MainForm.BlockModel(MainForm.BlockModel.Figure.Connector, "", -1, edit.connection));
                            else
                                model.Add(new MainForm.BlockModel(MainForm.BlockModel.Figure.Connector, "", -1, edit.connection));

                            tvi.SubItems.Add("");
                            tvi.SubItems.Add(MainForm.BlockModel.Figure.Connector.ToString());
                            tvi.SubItems.Add(lv.Items[edit.connection].Text);

                            record.Add("CONNECTION " + lv.Items[edit.connection].Text);
                            record.Add("INSERT BLOCK END");
                            break;
                        case MainForm.BlockModel.Figure.LoopS:
                        case MainForm.BlockModel.Figure.LoopE:
                            if (insert_index != model.Count)
                                model.Insert(insert_index, new MainForm.BlockModel((MainForm.BlockModel.Figure)edit.type.SelectedIndex, edit.description.Text.Replace("{loop letter}", "").Replace("{\\n}", "").Replace("\r", "").Trim(), -1));
                            else
                                model.Add(new MainForm.BlockModel((MainForm.BlockModel.Figure)edit.type.SelectedIndex, edit.description.Text.Replace("{loop letter}", "").Replace("{\\n}", "").Replace("\r", "").Trim(), -1));

                            tvi.ToolTipText = model[insert_index].row2;
                            tvi.SubItems.Add("{loop letter} " + string.Join("{\\n}", model[insert_index].row2.Split('\n')));
                            tvi.SubItems.Add(model[insert_index].figure.ToString());
                            tvi.SubItems.Add("");

                            record.Add("DESCRIPTION BEGIN");
                            foreach (var item in tvi.ToolTipText.Split('\n')) {
                                if (!item.StartsWith("INSERT")) {
                                    record.Add(item);
                                } else {
                                    record.Add("{\\n}" + item);
                                }
                            }
                            record.Add("INSERT BLOCK END");
                            break;
                        default:
                            if (insert_index != model.Count)
                                model.Insert(insert_index, new MainForm.BlockModel((MainForm.BlockModel.Figure)edit.type.SelectedIndex, edit.description.Text.Replace("{loop letter}", "").Replace("{\\n}", "").Replace("\r", "").Trim(), -1));
                            else
                                model.Add(new MainForm.BlockModel((MainForm.BlockModel.Figure)edit.type.SelectedIndex, edit.description.Text.Replace("{loop letter}", "").Replace("{\\n}", "").Replace("\r", "").Trim(), -1));

                            tvi.ToolTipText = model[insert_index].row2;
                            tvi.SubItems.Add(string.Join("{\\n}", model[insert_index].row2.Split('\n')));
                            tvi.SubItems.Add(model[insert_index].figure.ToString());
                            tvi.SubItems.Add("");

                            record.Add("DESCRIPTION BEGIN");
                            foreach (var item in tvi.ToolTipText.Split('\n')) {
                                if (!item.StartsWith("INSERT")) {
                                    record.Add(item);
                                } else {
                                    record.Add("{\\n}" + item);
                                }
                            }
                            record.Add("INSERT BLOCK END");
                            break;
                    }
                    tvi.SubItems.Add("");

                    record.Commit(CommandRecord.FunctionType.USER);

                    if (insert_index != model.Count) {
                        lv.Items.Insert(insert_index--, tvi);

                        MainForm.BlockModel tmp;
                        for (int i = 0; i != model.Count; ++i) {
                            tmp = model[i];
                            if (tmp.connection > insert_index)
                                ++tmp.connection;
                            for (int j = 0; j < tmp.connectedToMe.Count; ++j) {
                                if (tmp.connectedToMe[j] > insert_index)
                                    ++tmp.connectedToMe[j];
                            }
                        }
                    } else {
                        lv.Items.Add(tvi);
                        --insert_index;
                    }

                    if (model[++insert_index].connection > -1)
                        model[model[insert_index].connection].connectedToMe.Add(insert_index);
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            if (rus) {
                MessageBox.Show(this, "Для отображения описания с учётом многострочности наведите и удерживайте курсор на элементе в колонке \"Row\".\n\n" +
                    "* Row - относительный номер строки в исходнике\n" +
                    "* Command - команда в строке\n" +
                    "* Description - описание действия (будет в блоке блок-схемы)\n" +
                    "* Figure - фигура блок-схемы\n" +
                    "** Process (P) - команда действия (например, inc, dec)\n" +
                    "** FDecision (F) - условное продолжение (при выполнении условия выполнение идёт без перехода)\n" +
                    "** JDecision (J) - условный переход (например, jc, jz)\n" +
                    "** Subprocess (S) - вызов процедуры (например, call, int)\n" +
                    "** Terminator (T) - завершение выполнения программы\n" +
                    "** Hexagon (H) - подготовка данных (по мнению пользователя)\n" +
                    "** Connector (C) - особая \"фигура\", которая представляет собой только соединение (нужно для jmp)\n" +
                    "** LoopS (L) - начало цикла (ставится перед первой командой цикла)\n" +
                    "** LoopE (E) - конец цикла (например, loop, loopz)\n" +
                    "** I/O (I) - блок ввода-вывода (для ручного использования)\n" +
                    "* Connection - соединение с другим элементом блок-схемы (используется, например, командами условного перехода)\n" +
                    "* Meta command - синтезированная псевдо-команда (не содержит адресов переходов)\n\n" +
                    "Вы можете изменить тип блока, нажав комбинацию Alt+{letter}, где {letter} - код фигуры (указан в скобках для каждой фигуры).\n" +
                    "Другие горячие клавиши:\n" +
                    "* B - вставка перед выбранным элементом\n" +
                    "* A - вставка после выбранного элемента\n" +
                    "* C - копирование выбранного элемента\n" +
                    "* {Delete} - удаление выбранного элемента\n\n" +
                    "Помимо всего этого, важно заметить, что BlockEdit поддерживает функцию сохранения и восстановления сесии редактирования. Это означает, что вы можете не переживать о данных, выполняя масштабное редактирование - программа спросит вас о сохранении, и после согласия все ваши правки будут зафиксированы. " +
                    "Вы даже сможете их просмотреть и изменить, открыв файл с расширением '.ffagber' и именем, совпадающим с именем обрабатываемой процедуры. Файл создаётся в рабочей папке программы.", 
                    "Помощь", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else {
                MessageBox.Show(this, "To display a multi-line description, hover and hold the cursor over the item in the \"Row\" column.\n\n" +
                    "* Row - relative row number in the source\n" +
                    "* Command - the command in the line\n" +
                    "* Description - description of the action (will be in the flowchart block)\n" +
                    "* Figure - flowchart shape\n" +
                    "** Process (P) - action command (e.g. inc, dec)\n" +
                    "** FDecision (F) - conditional continuation (if the condition is fulfilled, the execution proceeds without a transition)\n" +
                    "** JDecision (J) - conditional jump (e.g. jc, jz)\n" +
                    "** Subprocess (S) - procedure call (e.g. call, int)\n" +
                    "** Terminator (T) - end of program execution\n" +
                    "** Hexagon (H) - data preparation (according to the user)\n" +
                    "** Connector (C) - a special \"shape\" that represents only a connection (needed for jmp)\n" +
                    "** LoopS (L) - the beginning of the cycle (placed before the first command of the cycle)\n" +
                    "** LoopE (E) - end of loop (e.g. loop, loopz)\n" +
                    "** I/O (I) - input/output block (for manual use)\n" +
                    "* Connection - connection to another element of the flowchart (used, for example, by conditional branch commands)\n" +
                    "* Meta command - synthesized pseudo-command (does not contain jump addresses)\n\n" +
                    "You can change the block type by pressing Alt + {letter}, where {letter} is the shape code (shown in parentheses for each shape).\n" +
                    "Other hotkeys:\n" +
                    "* B - insert before the selected item\n" +
                    "* A - insert after the selected item\n" +
                    "* C - copy the selected item\n" +
                    "* {Delete} - delete the selected item\n\n" +
                    "In addition to all this, it is important to note that BlockEdit supports the function of saving and restoring an edit session. This means that you do not have to worry about the data, performing large-scale editing - the program will ask you to save, and after consent, all your edits will be committed. " +
                    "You can even view and modify them by opening a file with the extension '.ffagber' and a name that matches the name of the procedure being processed. The file is created in the working folder of the program.", 
                    "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ToggleFigure(MainForm.BlockModel.Figure figure) { //Target type
            switch (model[lv.SelectedIndices[0]].figure) {
                case MainForm.BlockModel.Figure.Process:
                case MainForm.BlockModel.Figure.Hexagon:
                case MainForm.BlockModel.Figure.Subprocess:
                case MainForm.BlockModel.Figure.Terminator:
                case MainForm.BlockModel.Figure.IO:
                    switch (figure) {
                        case MainForm.BlockModel.Figure.Process:
                        case MainForm.BlockModel.Figure.Hexagon:
                        case MainForm.BlockModel.Figure.Subprocess:
                        case MainForm.BlockModel.Figure.Terminator:
                        case MainForm.BlockModel.Figure.IO:
                            lv.SelectedItems[0].SubItems[3].Text = (model[lv.SelectedIndices[0]].figure = figure).ToString();


                            record.Add("UPDATE BLOCK BEGIN");
                            record.Add("FIGURE " + figure.ToString());
                            record.Add("COMMAND " + lv.SelectedItems[0].SubItems[1].Text);
                            record.Add("DESCRIPTION BEGIN");
                            foreach (var item in lv.SelectedItems[0].ToolTipText.Split('\n')) {
                                if (!item.StartsWith("UPDATE")) {
                                    record.Add(item);
                                } else {
                                    record.Add("{\\n}" + item);
                                }
                            }
                            record.Add("UPDATE BLOCK END");
                            record.Commit(CommandRecord.FunctionType.USER);
                            break;
                        case MainForm.BlockModel.Figure.FDecision:
                        case MainForm.BlockModel.Figure.JDecision:
                        case MainForm.BlockModel.Figure.Connector:
                            EditBlock(figure, true);
                            break;
                        case MainForm.BlockModel.Figure.LoopS:
                        case MainForm.BlockModel.Figure.LoopE:
                            lv.SelectedItems[0].SubItems[3].Text = (model[lv.SelectedIndices[0]].figure = figure).ToString();
                            lv.SelectedItems[0].SubItems[2].Text = "{loop letter} " + string.Join("{\\n}", lv.SelectedItems[0].ToolTipText.Split('\n'));


                            record.Add("UPDATE BLOCK BEGIN");
                            record.Add("FIGURE " + figure.ToString());
                            record.Add("COMMAND " + lv.SelectedItems[0].SubItems[1].Text);
                            record.Add("DESCRIPTION BEGIN");
                            foreach (var item in lv.SelectedItems[0].ToolTipText.Split('\n')) {
                                if (!item.StartsWith("UPDATE")) {
                                    record.Add(item);
                                } else {
                                    record.Add("{\\n}" + item);
                                }
                            }
                            record.Add("UPDATE BLOCK END");
                            record.Commit(CommandRecord.FunctionType.USER);
                            break;
                    }
                    break;
                case MainForm.BlockModel.Figure.LoopS:
                case MainForm.BlockModel.Figure.LoopE:
                    switch (figure) {
                        case MainForm.BlockModel.Figure.Process:
                        case MainForm.BlockModel.Figure.Hexagon:
                        case MainForm.BlockModel.Figure.Subprocess:
                        case MainForm.BlockModel.Figure.Terminator:
                        case MainForm.BlockModel.Figure.IO:
                            lv.SelectedItems[0].SubItems[3].Text = (model[lv.SelectedIndices[0]].figure = figure).ToString();
                            lv.SelectedItems[0].SubItems[2].Text = string.Join("{\\n}", lv.SelectedItems[0].ToolTipText.Split('\n'));


                            record.Add("UPDATE BLOCK BEGIN");
                            record.Add("FIGURE " + figure.ToString());
                            record.Add("COMMAND " + lv.SelectedItems[0].SubItems[1].Text);
                            record.Add("DESCRIPTION BEGIN");
                            foreach (var item in lv.SelectedItems[0].ToolTipText.Split('\n')) {
                                if (!item.StartsWith("UPDATE")) {
                                    record.Add(item);
                                } else {
                                    record.Add("{\\n}" + item);
                                }
                            }
                            record.Add("UPDATE BLOCK END");
                            record.Commit(CommandRecord.FunctionType.USER);
                            break;
                        case MainForm.BlockModel.Figure.FDecision:
                        case MainForm.BlockModel.Figure.JDecision:
                        case MainForm.BlockModel.Figure.Connector:
                            EditBlock(figure, true);
                            break;
                        case MainForm.BlockModel.Figure.LoopS:
                        case MainForm.BlockModel.Figure.LoopE:
                            lv.SelectedItems[0].SubItems[3].Text = (model[lv.SelectedIndices[0]].figure = figure).ToString();


                            record.Add("UPDATE BLOCK BEGIN");
                            record.Add("FIGURE " + figure.ToString());
                            record.Add("COMMAND " + lv.SelectedItems[0].SubItems[1].Text);
                            record.Add("DESCRIPTION BEGIN");
                            foreach (var item in lv.SelectedItems[0].ToolTipText.Split('\n')) {
                                if (!item.StartsWith("UPDATE")) {
                                    record.Add(item);
                                } else {
                                    record.Add("{\\n}" + item);
                                }
                            }
                            record.Add("UPDATE BLOCK END");
                            record.Commit(CommandRecord.FunctionType.USER);
                            break;
                    }
                    break;
                case MainForm.BlockModel.Figure.FDecision:
                case MainForm.BlockModel.Figure.JDecision:
                    switch (figure) {
                        case MainForm.BlockModel.Figure.Process:
                        case MainForm.BlockModel.Figure.Hexagon:
                        case MainForm.BlockModel.Figure.Subprocess:
                        case MainForm.BlockModel.Figure.Terminator:
                        case MainForm.BlockModel.Figure.IO:
                            model[model[lv.SelectedIndices[0]].connection].connectedToMe.Remove(lv.SelectedIndices[0]);
                            model[lv.SelectedIndices[0]].connection = -1;

                            lv.SelectedItems[0].SubItems[3].Text = (model[lv.SelectedIndices[0]].figure = figure).ToString();
                            lv.SelectedItems[0].SubItems[4].Text = "";


                            record.Add("UPDATE BLOCK BEGIN");
                            record.Add("FIGURE " + figure.ToString());
                            record.Add("COMMAND " + lv.SelectedItems[0].SubItems[1].Text);
                            record.Add("DESCRIPTION BEGIN");
                            foreach (var item in lv.SelectedItems[0].ToolTipText.Split('\n')) {
                                if (!item.StartsWith("UPDATE")) {
                                    record.Add(item);
                                } else {
                                    record.Add("{\\n}" + item);
                                }
                            }
                            record.Add("UPDATE BLOCK END");
                            record.Commit(CommandRecord.FunctionType.USER);
                            break;
                        case MainForm.BlockModel.Figure.FDecision:
                        case MainForm.BlockModel.Figure.JDecision:
                            lv.SelectedItems[0].SubItems[3].Text = (model[lv.SelectedIndices[0]].figure = figure).ToString();


                            record.Add("UPDATE BLOCK BEGIN");
                            record.Add("FIGURE " + figure.ToString());
                            record.Add("COMMAND " + lv.SelectedItems[0].SubItems[1].Text);
                            record.Add("CONNECTION " + lv.SelectedItems[0].SubItems[4].Text);
                            record.Add("DESCRIPTION BEGIN");
                            foreach (var item in lv.SelectedItems[0].ToolTipText.Split('\n')) {
                                if (!item.StartsWith("UPDATE")) {
                                    record.Add(item);
                                } else {
                                    record.Add("{\\n}" + item);
                                }
                            }
                            record.Add("UPDATE BLOCK END");
                            record.Commit(CommandRecord.FunctionType.USER);
                            break;
                        case MainForm.BlockModel.Figure.Connector:
                            lv.SelectedItems[0].SubItems[2].Text = lv.SelectedItems[0].ToolTipText = model[lv.SelectedIndices[0]].row2 = "";
                            lv.SelectedItems[0].SubItems[3].Text = (model[lv.SelectedIndices[0]].figure = figure).ToString();


                            record.Add("UPDATE BLOCK BEGIN");
                            record.Add("FIGURE " + figure.ToString());
                            record.Add("COMMAND " + lv.SelectedItems[0].SubItems[1].Text);
                            record.Add("CONNECTION " + lv.SelectedItems[0].SubItems[4].Text);
                            record.Add("UPDATE BLOCK END");
                            record.Commit(CommandRecord.FunctionType.USER);
                            break;
                        case MainForm.BlockModel.Figure.LoopS:
                        case MainForm.BlockModel.Figure.LoopE:
                            model[model[lv.SelectedIndices[0]].connection].connectedToMe.Remove(lv.SelectedIndices[0]);
                            model[lv.SelectedIndices[0]].connection = -1;

                            lv.SelectedItems[0].SubItems[2].Text = "{loop letter} " + string.Join("{\\n}", lv.SelectedItems[0].ToolTipText.Split('\n'));
                            lv.SelectedItems[0].SubItems[3].Text = (model[lv.SelectedIndices[0]].figure = figure).ToString();
                            lv.SelectedItems[0].SubItems[4].Text = "";


                            record.Add("UPDATE BLOCK BEGIN");
                            record.Add("FIGURE " + figure.ToString());
                            record.Add("COMMAND " + lv.SelectedItems[0].SubItems[1].Text);
                            record.Add("DESCRIPTION BEGIN");
                            foreach (var item in lv.SelectedItems[0].ToolTipText.Split('\n')) {
                                if (!item.StartsWith("UPDATE")) {
                                    record.Add(item);
                                } else {
                                    record.Add("{\\n}" + item);
                                }
                            }
                            record.Add("UPDATE BLOCK END");
                            record.Commit(CommandRecord.FunctionType.USER);
                            break;
                    }
                    break;
                case MainForm.BlockModel.Figure.Connector:
                    EditBlock(figure, true);
                    break;
            }
        }
        private void lv_KeyUp(object sender, KeyEventArgs e) {
            if (lv.SelectedIndices.Count != 1) return;
            if (e.Alt) {
                switch (e.KeyCode) {
                    case Keys.P:
                        ToggleFigure(MainForm.BlockModel.Figure.Process);
                        break;
                    case Keys.F:
                        ToggleFigure(MainForm.BlockModel.Figure.FDecision);
                        break;
                    case Keys.J:
                        ToggleFigure(MainForm.BlockModel.Figure.JDecision);
                        break;
                    case Keys.S:
                        ToggleFigure(MainForm.BlockModel.Figure.Subprocess);
                        break;
                    case Keys.T:
                        ToggleFigure(MainForm.BlockModel.Figure.Terminator);
                        break;
                    case Keys.H:
                        ToggleFigure(MainForm.BlockModel.Figure.Hexagon);
                        break;
                    case Keys.C:
                        ToggleFigure(MainForm.BlockModel.Figure.Connector);
                        break;
                    case Keys.L:
                        ToggleFigure(MainForm.BlockModel.Figure.LoopS);
                        break;
                    case Keys.E:
                        ToggleFigure(MainForm.BlockModel.Figure.LoopE);
                        break;
                    case Keys.I:
                        ToggleFigure(MainForm.BlockModel.Figure.IO);
                        break;
                }
                return;
            }

            switch (e.KeyCode) {
                case Keys.Delete:
                    removeBlocksingleToolStripMenuItem_Click(null, null);
                    break;
                case Keys.B:
                    insertBlockbeforeToolStripMenuItem_Click(null, null);
                    break;
                case Keys.A:
                    insertBlockafterToolStripMenuItem_Click(null, null);
                    break;
                case Keys.C:
                    copyBlockToToolStripMenuItem_Click(null, null);
                    break;
            }
        }

        private void removeBlocksingleToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lv.SelectedIndices.Count != 1) return;

            int cstart_index = lv.SelectedIndices[0];
            if (model[cstart_index].connectedToMe.Count != 0) {
                MessageBox.Show(this, "To delete this element, you need to delete all elements that reference this one.\nOr you can choose a different delete mode.", "This element is referenced", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show(this, "Are you sure you want to remove selected element?", "Removing single item", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No) return;

            record.Add("DELETE SINGLE");
            record.Commit(CommandRecord.FunctionType.BOTH);

            switch (model[cstart_index].figure) {
                case MainForm.BlockModel.Figure.FDecision:
                case MainForm.BlockModel.Figure.JDecision:
                case MainForm.BlockModel.Figure.Connector:
                    model[model[cstart_index].connection].connectedToMe.Remove(cstart_index);
                    break;
            }

            model.RemoveAt(cstart_index);

            lv.Items.RemoveAt(cstart_index--);

            MainForm.BlockModel tmp;
            for (int i = 0; i != model.Count; ++i) {
                tmp = model[i];
                if (tmp.connection > cstart_index)
                    --tmp.connection;
                for (int j = 0; j < tmp.connectedToMe.Count; ++j) {
                    if (tmp.connectedToMe[j] > cstart_index)
                        --tmp.connectedToMe[j];
                }
            }
        }

        private void removeBlockcascadeToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lv.SelectedIndices.Count != 1) return;

            if (MessageBox.Show(this, "Are you sure you want to remove the cascade of elements?\nВы уверены, что хотите удалить каскад элементов?", "Removing CASCADE", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No) return;

            record.Add("DELETE CASCADE");
            record.Commit(CommandRecord.FunctionType.BOTH);

            List<int[]> toDel = new List<int[]>() { new int[] { lv.SelectedIndices[0], 0 } };
            RemoveCascade(toDel);
            toDel = toDel.OrderBy(x => x[0]).ToList();

            ushort addition = 0;
            for (int i = 0; i != toDel.Count; ++i) {
                toDel[i][0] -= addition;
                toDel[i][1] = ++addition;

                model.RemoveAt(toDel[i][0]);

                lv.Items.RemoveAt(toDel[i][0]--); //Decrement is needed to use ">" instead of ">="
            }

            int[] cind;
            MainForm.BlockModel tmp;
            for (int i = 0; i != model.Count; ++i) {
                tmp = model[i];

                cind = toDel.Where(x => tmp.connection > x[0]).LastOrDefault();
                if (cind != null)
                    tmp.connection -= cind[1];

                for (int j = 0; j < tmp.connectedToMe.Count; ++j) {
                    cind = toDel.Where(x => tmp.connectedToMe[j] > x[0]).LastOrDefault();
                    if (cind != null)
                        tmp.connectedToMe[j] -= cind[1];
                }
            }
        }

        private void RemoveCascade(List<int[]> idexes) {
            switch (model[idexes.Last()[0]].figure) {
                case MainForm.BlockModel.Figure.FDecision:
                case MainForm.BlockModel.Figure.JDecision:
                case MainForm.BlockModel.Figure.Connector:
                    if (!idexes.Take(idexes.Count - 1).Any(x => x[0] == model[idexes.Last()[0]].connection))
                        model[model[idexes.Last()[0]].connection].connectedToMe.Remove(idexes.Last()[0]);
                    break;
            }

            foreach (int item in model[idexes.Last()[0]].connectedToMe) {
                idexes.Add(new int[] { item, 0 });
                RemoveCascade(idexes);
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            if (model.Count == 0) {
                MessageBox.Show("Flowchart contains no blocks.\nContinuing calculations is useless.", "Empty flowchart", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.Abort;
                Close();
                return;
            }

            if (MessageBox.Show("Do you want to save this editing session?", "BlockEdit session saving", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes) {
                try {
                    record.Save();
                } catch {
                    MessageBox.Show("Unable to save because there is no write access to the folder.", "BlockEdit session saving error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void copyBlockToToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lv.SelectedIndices.Count != 1) return;

            EditBlock edit = new EditBlock(RetrieveVirtualItem);
            edit.Text = "Copy block To";
            edit.type.SelectedIndex = 6;
            edit.type.Enabled = false;
            edit.textCmd.Enabled = false;
            edit.textCmd.Text = "[hint] Select one of empty rows (where a figure feild is empty).";
            edit.chbx_clearCmd.Visible = false;

            edit.lv.VirtualListSize = (lv.Items.Count << 1) + 1;

            fform.Hide();

            rvi_same = false;
            if (edit.ShowDialog() == DialogResult.OK) {
                if ((edit.connection & 1) == 1) {
                    rvi_same = true;
                    MessageBox.Show(this, "It is not possible to copy the item to the specified location because the space is occupied by another item!", "Invalid copy destination", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int copy_index = lv.SelectedIndices[0];
                int paste_index = edit.connection / 2;

                ListViewItem tvi = new ListViewItem("u" + ++ucmds);
                tvi.SubItems.Add(lv.Items[copy_index].SubItems[1].Text);
                tvi.ToolTipText = model[copy_index].row2;
                tvi.SubItems.Add(lv.Items[copy_index].SubItems[2].Text);
                tvi.SubItems.Add(lv.Items[copy_index].SubItems[3].Text);
                tvi.SubItems.Add(lv.Items[copy_index].SubItems[4].Text);
                tvi.SubItems.Add(lv.Items[copy_index].SubItems[5].Text);

                record.Add("COPYTO " + (paste_index == 0 ? "BEGIN" : (paste_index == model.Count ? "END" : "BETWEEN " + lv.Items[paste_index - 1].Text + " AND " + lv.Items[paste_index].Text)));
                record.Commit(CommandRecord.FunctionType.USER);

                if (paste_index != model.Count) {
                    model.Insert(paste_index, new MainForm.BlockModel(model[copy_index].figure, model[copy_index].row2, -1, model[copy_index].connection));
                    lv.Items.Insert(paste_index--, tvi);

                    MainForm.BlockModel tmp;
                    for (int i = 0; i != model.Count; ++i) {
                        tmp = model[i];
                        if (tmp.connection > paste_index)
                            ++tmp.connection;
                        for (int j = 0; j < tmp.connectedToMe.Count; ++j) {
                            if (tmp.connectedToMe[j] > paste_index)
                                ++tmp.connectedToMe[j];
                        }
                    }
                } else {
                    model.Add(new MainForm.BlockModel(model[copy_index].figure, model[copy_index].row2, -1, model[copy_index].connection));
                    lv.Items.Add(tvi);
                    --paste_index;
                }

                if (model[copy_index = (copy_index < paste_index ? copy_index : copy_index + 1)].connection > -1)
                    model[model[copy_index].connection].connectedToMe.Add(paste_index + 1);
            }
            rvi_same = true;
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e) {
            fform.Show();
            fform.Activate();
        }

        private void BlockEdit_FormClosing(object sender, FormClosingEventArgs e) {
            fform.Close();
        }

        int prew_selected = -1;
        private void lv_SelectedIndexChanged(object sender, EventArgs e) {
            if (lv.SelectedIndices.Count != 0 && lv.SelectedIndices[0] != prew_selected) {
                record.Add("SELECT " + lv.SelectedItems[0].Text);
                record.Commit(CommandRecord.FunctionType.SELECTOR);

                prew_selected = lv.SelectedIndices[0];
            }
        }

        private void BlockEdit_Shown(object sender, EventArgs e) {
            try {
                if (record.Exists()) {
                    if (MessageBox.Show("An edit session record was detected for this chart.\nDo you want to load a saved session?", "BlockEdit session loading", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        record.Load(lv, model, ref ucmds);
                }
            } catch {
                MessageBox.Show("An error occurred while loading the session: the sessions script is corrupted.\nTip: answer \"No\" in previous session restoring dialog after BlockEdit relaunch.", "BlockEdit: corrupted session", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Retry;
                Close();
            }
        }
        internal class CommandRecord {
            string filename;
            internal CommandRecord(string filename) {
                this.filename = filename;
            }
            internal enum FunctionType { NONE, SELECTOR, USER, BOTH, };
            struct CommandLog {
                internal CommandLog(FunctionType role, string[] command) {
                    this.role = role;
                    this.command = command;
                }
                internal FunctionType role;
                internal string[] command;
            }

            List<string> stage = new List<string>();
            List<CommandLog> record = new List<CommandLog>();

            internal void Add(string text) {
                stage.Add(text);
            }

            internal void Commit(FunctionType role) {
                if (record.Count != 0 && role == FunctionType.SELECTOR && record.Last().role == FunctionType.SELECTOR) {
                    record.RemoveAt(record.Count - 1);
                    record.Add(new CommandLog(role, stage.ToArray()));
                } else
                    record.Add(new CommandLog(role, stage.ToArray()));
                stage.Clear();
            }

            internal void RemoveLatest() {
                record.RemoveAt(record.Count - 1);
            }

            string GetComment(FunctionType role) {
                switch (role) {
                    case FunctionType.NONE:
                    default:
                        return "-- Independent of the cursor position.";
                    case FunctionType.SELECTOR:
                        return "-- Element selection (sets cursor position).";
                    case FunctionType.USER:
                        return "-- Uses the cursor position to make changes to the selected item (modifed item still be selected).";
                    case FunctionType.BOTH:
                        return "-- Changes the cursor position based on its current position. It can affect the items selected by the cursor.";
                }
            }
            internal void Save() {
                StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + '\\' + filename + ".ffagber", save_loaded);
                for (ushort i = 0; i != record.Count; ++i) {
                    if (record[i].role == FunctionType.SELECTOR) {
                        if (i + 1 == record.Count)
                            break;

                        sw.WriteLine(GetComment(record[i].role));
                        foreach (var item in record[i].command)
                            sw.WriteLine(item);
                    } else {
                        sw.WriteLine(GetComment(record[i].role));
                        foreach (var item in record[i].command)
                            sw.WriteLine(item);
                    }
                    sw.WriteLine();
                }
                sw.Close();
            }

            internal bool Exists() {
                return File.Exists(Directory.GetCurrentDirectory() + '\\' + filename + ".ffagber");
            }

            struct Command {
                internal enum Type { SELECT, INSERT_FIRST, INSERT_INTO, UPDATE_DESC, UPDATE_BLOCK, DELETE, FIND_BEGIN, FIND_NEXT, FIND_REPLACE_NEXT, FIND_REPLACE_ALL, COPYTO }

                internal Command(Type type, Dictionary<string, string> options) {
                    this.type = type;
                    this.options = new Dictionary<string, string>(options);
                }
                internal Type type;
                internal Dictionary<string, string> options;
            }
            bool save_loaded = false;
            internal void Load(ListView lv, List<MainForm.BlockModel> model, ref ushort ucmds) {
                StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + '\\' + filename + ".ffagber");

                List<Command> script = new List<Command>();
                {
                    Regex rg = new Regex("(--)+.*$");
                    while (!sr.EndOfStream) {
                        string line = rg.Replace(sr.ReadLine(), "");
                        if (line != "") {
                            Dictionary<string, string> opt = new Dictionary<string, string>();

                            string[] cmd = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            switch (cmd[0]) {
                                case "SELECT":
                                    opt.Add("row", cmd[1]);
                                    script.Add(new Command(Command.Type.SELECT, opt));
                                    break;
                                case "DELETE":
                                    opt.Add("mode", cmd[1]);
                                    script.Add(new Command(Command.Type.DELETE, opt));
                                    break;
                                case "COPYTO":
                                    switch (cmd[1]) {
                                        case "BEGIN":
                                            opt.Add("pos", "BEGIN");
                                            break;
                                        case "END":
                                            opt.Add("pos", "END");
                                            break;
                                        case "BETWEEN":
                                            opt.Add("pos", "BETWEEN");
                                            opt.Add("target", cmd[4]);
                                            break;
                                    }
                                    script.Add(new Command(Command.Type.COPYTO, opt));
                                    break;
                                case "INSERT":
                                    while (true) {
                                        line = rg.Replace(sr.ReadLine(), "");
                                        while (line == "") line = rg.Replace(sr.ReadLine(), "");
                                        cmd = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                                        switch (cmd[0]) {
                                            case "FIGURE":
                                                opt.Add("figure", cmd[1]);
                                                break;
                                            case "COMMAND":
                                                opt.Add("cmd", line.Substring(8));
                                                break;
                                            case "POSITION":
                                                opt.Add("pos", cmd[1]);
                                                break;
                                            case "CONNECTION":
                                                opt.Add("link", cmd[1]);
                                                break;
                                            case "DESCRIPTION":
                                                string text = "";
                                                while (true) {
                                                    line = sr.ReadLine().Replace("{loop letter}", "").Replace("\r", "");
                                                    if (line.StartsWith("INSERT")) {
                                                        break;
                                                    } else {
                                                        text += line.Replace("{\\n}", "") + '\n';
                                                    }
                                                }
                                                text = text.Trim();
                                                opt.Add("desc", text);
                                                goto icommit;
                                            case "INSERT":
                                                goto icommit;
                                        }
                                    }
                                icommit:;
                                    if (opt.ContainsKey("pos"))
                                        script.Add(new Command(Command.Type.INSERT_INTO, opt));
                                    else
                                        script.Add(new Command(Command.Type.INSERT_FIRST, opt));
                                    break;
                                case "UPDATE":
                                    if (cmd[1] == "BLOCK") {
                                        while (true) {
                                            line = rg.Replace(sr.ReadLine(), "");
                                            while (line == "") line = rg.Replace(sr.ReadLine(), "");
                                            cmd = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                                            switch (cmd[0]) {
                                                case "FIGURE":
                                                    opt.Add("figure", cmd[1]);
                                                    break;
                                                case "COMMAND":
                                                    opt.Add("cmd", line.Substring(8));
                                                    break;
                                                case "CONNECTION":
                                                    opt.Add("link", cmd[1]);
                                                    break;
                                                case "DESCRIPTION":
                                                    string text = "";
                                                    while (true) {
                                                        line = sr.ReadLine().Replace("{loop letter}", "").Replace("\r", "");
                                                        if (line.StartsWith("UPDATE")) {
                                                            break;
                                                        } else {
                                                            text += line.Replace("{\\n}", "") + '\n';
                                                        }
                                                    }
                                                    text = text.Trim();
                                                    opt.Add("desc", text);
                                                    goto ucommit;
                                                case "UPDATE":
                                                    goto ucommit;
                                            }
                                        }
                                    ucommit:;
                                        script.Add(new Command(Command.Type.UPDATE_BLOCK, opt));
                                    } else {
                                        string text = "";
                                        while (true) {
                                            line = sr.ReadLine().Replace("{loop letter}", "").Replace("\r", "");
                                            if (line.StartsWith("UPDATE")) {
                                                break;
                                            } else {
                                                text += line.Replace("{\\n}", "") + '\n';
                                            }
                                        }
                                        text = text.Trim();
                                        opt.Add("desc", text);
                                        script.Add(new Command(Command.Type.UPDATE_DESC, opt));
                                    }
                                    break;
                                case "WHERE":
                                    while (true) {
                                        line = rg.Replace(sr.ReadLine(), "");
                                        while (line == "") line = rg.Replace(sr.ReadLine(), "");
                                        cmd = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                                        switch (cmd[0]) {
                                            case "ROW":
                                                opt.Add("row", cmd[1]);
                                                break;
                                            case "COMMAND":
                                                opt.Add("cmd", line.Substring(8));
                                                break;
                                            case "DESCRIPTION":
                                                opt.Add("desc", line.Substring(12));
                                                break;
                                            case "FIGURE":
                                                opt.Add("figure", cmd[1]);
                                                break;
                                            case "CONNECTION":
                                                opt.Add("link", cmd[1]);
                                                break;
                                            case "METACMD":
                                                opt.Add("metacmd", line.Substring(8));
                                                break;
                                            case "MODE":
                                                switch (cmd[1]) {
                                                    case "COMMAND":
                                                        opt.Add("cmd_case", cmd[3]);
                                                        opt.Add("cmd_cmp", cmd[2]);
                                                        break;
                                                    case "DESCRIPTION":
                                                        opt.Add("desc_case", cmd[3]);
                                                        opt.Add("desc_cmp", cmd[2]);
                                                        break;
                                                    case "METACMD":
                                                        opt.Add("metacmd_case", cmd[3]);
                                                        opt.Add("metacmd_cmp", cmd[2]);
                                                        break;
                                                }
                                                break;
                                            case "SET":
                                                goto fnext;
                                        }
                                    }
                                fnext:;
                                    if (cmd[1] == "CURSOR") {
                                        if (cmd[3] == "BEGIN") {
                                            script.Add(new Command(Command.Type.FIND_BEGIN, opt));
                                        } else {
                                            if (cmd.Length == 6)
                                                opt.Add("wrap", null);
                                            script.Add(new Command(Command.Type.FIND_NEXT, opt));
                                        }
                                    } else {
                                        bool all = true;
                                        if (cmd[3] == "CURSOR") {
                                            if (cmd.Length == 7)
                                                opt.Add("wrap", null);
                                            all = false;
                                        }

                                        while (true) {
                                            line = rg.Replace(sr.ReadLine(), "");
                                            while (line == "") line = rg.Replace(sr.ReadLine(), "");
                                            cmd = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                                            switch (cmd[0]) {
                                                case "COMMAND":
                                                    opt.Add("new_cmd", line.Substring(8));
                                                    break;
                                                case "DESCRIPTION":
                                                    string text = "";
                                                    while (true) {
                                                        line = sr.ReadLine().Replace("{loop letter}", "").Replace("\r", "");
                                                        if (line.StartsWith("UPDATE")) {
                                                            break;
                                                        } else {
                                                            text += line.Replace("{\\n}", "") + '\n';
                                                        }
                                                    }
                                                    text = text.Trim();
                                                    opt.Add("new_desc", text);
                                                    goto repl;
                                                case "UPDATE":
                                                    goto repl;
                                            }
                                        }
                                    repl:;
                                        if (all) {
                                            script.Add(new Command(Command.Type.FIND_REPLACE_ALL, opt));
                                        } else {
                                            script.Add(new Command(Command.Type.FIND_REPLACE_NEXT, opt));
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
                sr.Close();

                ListView.ListViewItemCollection lvi = lv.Items;
                int index = -1;
                foreach (Command item in script) {
                    switch (item.type) {
                        case Command.Type.SELECT:
                            index = lv.FindItemWithText(item.options["row"], false, 0, false).Index;
                            break;
                        case Command.Type.DELETE:
                            if (index == -1) throw new Exception();

                            if (item.options["mode"] == "CASCADE") {
                                void CascadeProc(List<int[]> idexes) {
                                    switch (model[idexes.Last()[0]].figure) {
                                        case MainForm.BlockModel.Figure.FDecision:
                                        case MainForm.BlockModel.Figure.JDecision:
                                        case MainForm.BlockModel.Figure.Connector:
                                            if (!idexes.Take(idexes.Count - 1).Any(x => x[0] == model[idexes.Last()[0]].connection))
                                                model[model[idexes.Last()[0]].connection].connectedToMe.Remove(idexes.Last()[0]);
                                            break;
                                    }
                                    foreach (int item2 in model[idexes.Last()[0]].connectedToMe) {
                                        idexes.Add(new int[] { item2, 0 });
                                        CascadeProc(idexes);
                                    }
                                }

                                List<int[]> toDel = new List<int[]>() { new int[] { index, 0 } };
                                CascadeProc(toDel);
                                toDel = toDel.OrderBy(x => x[0]).ToList();

                                ushort addition = 0;
                                for (int i = 0; i != toDel.Count; ++i) {
                                    toDel[i][0] -= addition;
                                    toDel[i][1] = ++addition;

                                    model.RemoveAt(toDel[i][0]);
                                    lvi.RemoveAt(toDel[i][0]--); //Decrement is needed to use ">" instead of ">="
                                }

                                int[] cind;
                                MainForm.BlockModel tmp;
                                for (int i = 0; i != model.Count; ++i) {
                                    tmp = model[i];

                                    cind = toDel.Where(x => tmp.connection > x[0]).LastOrDefault();
                                    if (cind != null)
                                        tmp.connection -= cind[1];

                                    for (int j = 0; j < tmp.connectedToMe.Count; ++j) {
                                        cind = toDel.Where(x => tmp.connectedToMe[j] > x[0]).LastOrDefault();
                                        if (cind != null)
                                            tmp.connectedToMe[j] -= cind[1];
                                    }
                                }
                            } else {
                                if (model[index].connectedToMe.Count != 0) throw new Exception();
                                switch (model[index].figure) {
                                    case MainForm.BlockModel.Figure.FDecision:
                                    case MainForm.BlockModel.Figure.JDecision:
                                    case MainForm.BlockModel.Figure.Connector:
                                        model[model[index].connection].connectedToMe.Remove(index);
                                        break;
                                }

                                model.RemoveAt(index);
                                lvi.RemoveAt(index--);

                                MainForm.BlockModel tmp;
                                for (int i = 0; i != model.Count; ++i) {
                                    tmp = model[i];
                                    if (tmp.connection > index)
                                        --tmp.connection;
                                    for (int j = 0; j < tmp.connectedToMe.Count; ++j) {
                                        if (tmp.connectedToMe[j] > index)
                                            --tmp.connectedToMe[j];
                                    }
                                }
                            }
                            index = -1;
                            break;
                        case Command.Type.COPYTO: {
                            if (index == -1) throw new Exception();
                            string tmps = item.options["pos"];
                            int paste_index = (tmps == "BEGIN" ? 0 : (tmps == "END" ? model.Count : lv.FindItemWithText(item.options["target"], false, 0, false).Index));

                            ListViewItem tvi = new ListViewItem("u" + ++ucmds);
                            tvi.SubItems.Add(lvi[index].SubItems[1].Text);
                            tvi.ToolTipText = model[index].row2;
                            tvi.SubItems.Add(lvi[index].SubItems[2].Text);
                            tvi.SubItems.Add(lvi[index].SubItems[3].Text);
                            tvi.SubItems.Add(lvi[index].SubItems[4].Text);
                            tvi.SubItems.Add(lvi[index].SubItems[5].Text);

                            if (paste_index != model.Count) {
                                model.Insert(paste_index, new MainForm.BlockModel(model[index].figure, model[index].row2, -1, model[index].connection));
                                lvi.Insert(paste_index--, tvi);

                                MainForm.BlockModel tmp;
                                for (int i = 0; i != model.Count; ++i) {
                                    tmp = model[i];
                                    if (tmp.connection > paste_index)
                                        ++tmp.connection;
                                    for (int j = 0; j < tmp.connectedToMe.Count; ++j) {
                                        if (tmp.connectedToMe[j] > paste_index)
                                            ++tmp.connectedToMe[j];
                                    }
                                }
                            } else {
                                model.Add(new MainForm.BlockModel(model[index].figure, model[index].row2, -1, model[index].connection));
                                lvi.Add(tvi);
                                --paste_index;
                            }

                            if (model[index = (index < paste_index ? index : index + 1)].connection > -1)
                                model[model[index].connection].connectedToMe.Add(paste_index + 1);
                        }
                        break;

                        case Command.Type.INSERT_FIRST: {
                            if (model.Count > 0) throw new Exception();
                            model.Add(new MainForm.BlockModel((MainForm.BlockModel.Figure)Enum.Parse(typeof(MainForm.BlockModel.Figure), item.options["figure"]), item.options["desc"], -1));

                            ListViewItem tvi = new ListViewItem("u" + ++ucmds);
                            tvi.ToolTipText = model.First().row2;
                            tvi.SubItems.Add(item.options["cmd"]);

                            switch (model.First().figure) {
                                case MainForm.BlockModel.Figure.LoopS:
                                case MainForm.BlockModel.Figure.LoopE:
                                    tvi.SubItems.Add("{loop letter} " + string.Join("{\\n}", model.First().row2.Split('\n')));
                                    tvi.SubItems.Add(model.First().figure.ToString());
                                    break;
                                default:
                                    tvi.SubItems.Add(string.Join("{\\n}", model.First().row2.Split('\n')));
                                    tvi.SubItems.Add(model.First().figure.ToString());
                                    break;
                            }
                            tvi.SubItems.Add("");
                            tvi.SubItems.Add("");
                        }
                        break;
                        case Command.Type.INSERT_INTO: {
                            if (index == -1) throw new Exception();
                            int insert_index = (item.options["pos"] == "BEFORE" ? index : index + 1);

                            ListViewItem tvi = new ListViewItem("u" + ++ucmds);
                            tvi.SubItems.Add(item.options["cmd"]);

                            switch ((MainForm.BlockModel.Figure)Enum.Parse(typeof(MainForm.BlockModel.Figure), item.options["figure"])) {
                                case MainForm.BlockModel.Figure.FDecision:
                                case MainForm.BlockModel.Figure.JDecision:
                                    if (insert_index != model.Count)
                                        model.Insert(insert_index, new MainForm.BlockModel((MainForm.BlockModel.Figure)Enum.Parse(typeof(MainForm.BlockModel.Figure), item.options["figure"]), item.options["desc"], -1, lv.FindItemWithText(item.options["link"], false, 0, false).Index));
                                    else
                                        model.Add(new MainForm.BlockModel((MainForm.BlockModel.Figure)Enum.Parse(typeof(MainForm.BlockModel.Figure), item.options["figure"]), item.options["desc"], -1, lv.FindItemWithText(item.options["link"], false, 0, false).Index));

                                    tvi.ToolTipText = model[insert_index].row2;
                                    tvi.SubItems.Add(string.Join("{\\n}", model[insert_index].row2.Split('\n')));
                                    tvi.SubItems.Add(model[insert_index].figure.ToString());
                                    tvi.SubItems.Add(lvi[model[insert_index].connection].Text);
                                    break;
                                case MainForm.BlockModel.Figure.Connector:
                                    if (insert_index != model.Count)
                                        model.Insert(insert_index, new MainForm.BlockModel(MainForm.BlockModel.Figure.Connector, "", -1, lv.FindItemWithText(item.options["link"], false, 0, false).Index));
                                    else
                                        model.Add(new MainForm.BlockModel(MainForm.BlockModel.Figure.Connector, "", -1, lv.FindItemWithText(item.options["link"], false, 0, false).Index));

                                    tvi.SubItems.Add("");
                                    tvi.SubItems.Add(MainForm.BlockModel.Figure.Connector.ToString());
                                    tvi.SubItems.Add(lvi[model[insert_index].connection].Text);
                                    break;
                                case MainForm.BlockModel.Figure.LoopS:
                                case MainForm.BlockModel.Figure.LoopE:
                                    if (insert_index != model.Count)
                                        model.Insert(insert_index, new MainForm.BlockModel((MainForm.BlockModel.Figure)Enum.Parse(typeof(MainForm.BlockModel.Figure), item.options["figure"]), item.options["desc"], -1));
                                    else
                                        model.Add(new MainForm.BlockModel((MainForm.BlockModel.Figure)Enum.Parse(typeof(MainForm.BlockModel.Figure), item.options["figure"]), item.options["desc"], -1));

                                    tvi.ToolTipText = model[insert_index].row2;
                                    tvi.SubItems.Add("{loop letter} " + string.Join("{\\n}", model[insert_index].row2.Split('\n')));
                                    tvi.SubItems.Add(model[insert_index].figure.ToString());
                                    tvi.SubItems.Add("");
                                    break;
                                default:
                                    if (insert_index != model.Count)
                                        model.Insert(insert_index, new MainForm.BlockModel((MainForm.BlockModel.Figure)Enum.Parse(typeof(MainForm.BlockModel.Figure), item.options["figure"]), item.options["desc"], -1));
                                    else
                                        model.Add(new MainForm.BlockModel((MainForm.BlockModel.Figure)Enum.Parse(typeof(MainForm.BlockModel.Figure), item.options["figure"]), item.options["desc"], -1));

                                    tvi.ToolTipText = model[insert_index].row2;
                                    tvi.SubItems.Add(string.Join("{\\n}", model[insert_index].row2.Split('\n')));
                                    tvi.SubItems.Add(model[insert_index].figure.ToString());
                                    tvi.SubItems.Add("");
                                    break;
                            }
                            tvi.SubItems.Add("");

                            if (insert_index != model.Count) {
                                lvi.Insert(insert_index--, tvi);

                                MainForm.BlockModel tmp;
                                for (int i = 0; i != model.Count; ++i) {
                                    tmp = model[i];
                                    if (tmp.connection > insert_index)
                                        ++tmp.connection;
                                    for (int j = 0; j < tmp.connectedToMe.Count; ++j) {
                                        if (tmp.connectedToMe[j] > insert_index)
                                            ++tmp.connectedToMe[j];
                                    }
                                }
                            } else {
                                lvi.Add(tvi);
                                --insert_index;
                            }

                            if (model[++insert_index].connection > -1)
                                model[model[insert_index].connection].connectedToMe.Add(insert_index);
                        }
                        break;

                        case Command.Type.UPDATE_DESC:
                            switch (model[index].figure) {
                                case MainForm.BlockModel.Figure.LoopS:
                                case MainForm.BlockModel.Figure.LoopE:
                                    lvi[index].SubItems[2].Text = "{loop letter} " + string.Join("{\\n}", (lvi[index].ToolTipText = model[index].row2 = item.options["desc"]).Split('\n'));
                                    break;
                                default:
                                    lvi[index].SubItems[2].Text = string.Join("{\\n}", (lvi[index].ToolTipText = model[index].row2 = item.options["desc"]).Split('\n'));
                                    break;
                            }
                            break;
                        case Command.Type.UPDATE_BLOCK: {
                            lvi[index].SubItems[3].Text = item.options["figure"];
                            model[index].figure = (MainForm.BlockModel.Figure)Enum.Parse(typeof(MainForm.BlockModel.Figure), item.options["figure"]);
                            if (lvi[index].SubItems[1].Text != item.options["cmd"]) {
                                lvi[index].SubItems[1].Text = item.options["cmd"];
                                lvi[index].SubItems[5].Text = "";
                            }

                            switch (model[index].figure) {
                                case MainForm.BlockModel.Figure.FDecision:
                                case MainForm.BlockModel.Figure.JDecision:
                                    if (model[index].connection != -1)
                                        model[model[index].connection].connectedToMe.Remove(index);
                                    model[index].connection = lv.FindItemWithText(item.options["link"], false, 0, false).Index;
                                    model[model[index].connection].connectedToMe.Add(index);

                                    lvi[index].SubItems[2].Text = string.Join("{\\n}", (lvi[index].ToolTipText = model[index].row2 = item.options["desc"]).Split('\n'));
                                    lvi[index].SubItems[4].Text = item.options["link"];
                                    break;
                                case MainForm.BlockModel.Figure.Connector:
                                    if (model[index].connection != -1)
                                        model[model[index].connection].connectedToMe.Remove(index);
                                    model[index].connection = lv.FindItemWithText(item.options["link"], false, 0, false).Index;
                                    model[model[index].connection].connectedToMe.Add(index);

                                    lvi[index].SubItems[2].Text = lvi[index].ToolTipText = model[index].row2 = "";
                                    lvi[index].SubItems[4].Text = item.options["link"];
                                    break;
                                case MainForm.BlockModel.Figure.LoopS:
                                case MainForm.BlockModel.Figure.LoopE:
                                    if (model[index].connection != -1) {
                                        model[model[index].connection].connectedToMe.Remove(index);
                                        model[index].connection = -1;
                                    }

                                    lvi[index].SubItems[2].Text = "{loop letter} " + string.Join("{\\n}", (lvi[index].ToolTipText = model[index].row2 = item.options["desc"]).Split('\n'));
                                    lvi[index].SubItems[4].Text = "";
                                    break;
                                default:
                                    if (model[index].connection != -1) {
                                        model[model[index].connection].connectedToMe.Remove(index);
                                        model[index].connection = -1;
                                    }

                                    lvi[index].SubItems[2].Text = string.Join("{\\n}", (lvi[index].ToolTipText = model[index].row2 = item.options["desc"]).Split('\n'));
                                    lvi[index].SubItems[4].Text = "";
                                    break;
                            }
                        }
                        break;

                        case Command.Type.FIND_BEGIN: {
                            int cindex = -1;
                            ListViewItem tvi;

                            string ts;
                            string row = (item.options.TryGetValue("row", out ts) ? ts : ""),
                                cmd = (item.options.TryGetValue("cmd", out ts) ? ts : ""),
                                desc = (item.options.TryGetValue("desc", out ts) ? ts : ""),
                                figure = (item.options.TryGetValue("figure", out ts) ? ts : ""),
                                link = (item.options.TryGetValue("link", out ts) ? ts : ""),
                                metacmd = (item.options.TryGetValue("metacmd", out ts) ? ts : "");

                            bool cmd_case = (cmd == "" ? false : item.options["cmd_case"] == "CASE"),
                               desc_case = (desc == "" ? false : item.options["desc_case"] == "CASE"),
                               metacmd_case = (metacmd == "" ? false : item.options["metacmd_case"] == "CASE"),
                               cmd_cmp = (cmd == "" ? false : item.options["cmd_cmp"] == "WHOLE"),
                               desc_cmp = (desc == "" ? false : item.options["desc_cmp"] == "WHOLE"),
                               metacmd_cmp = (metacmd == "" ? false : item.options["metacmd_cmp"] == "WHOLE");

                            while (true) {
                                if (lvi.Count == ++cindex) {
                                    throw new Exception();
                                }
                                tvi = lvi[cindex];
                                bool noth_found = true;

                                if (row != "") {
                                    noth_found = false;
                                    if (tvi.Text != row.ToLower())
                                        continue;
                                }

                                if (cmd != "") {
                                    noth_found = false;
                                    if (cmd_cmp) {
                                        string[] spl1, spl2;

                                        spl1 = cmd.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                        spl2 = tvi.SubItems[1].Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                                        bool nfound = true;
                                        byte clook = 0;
                                        if (cmd_case) {
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
                                        if (cmd_case) {
                                            if (!tvi.SubItems[1].Text.Contains(cmd))
                                                continue;
                                        } else {
                                            if (!tvi.SubItems[1].Text.ToLower().Contains(cmd.ToLower()))
                                                continue;
                                        }
                                    }
                                }

                                if (desc != "") {
                                    noth_found = false;
                                    if (desc_cmp) {
                                        string[] spl1, spl2;

                                        spl1 = desc.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                        spl2 = tvi.ToolTipText.Split(new char[] { ' ', ',', '.', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                                        bool nfound = true;
                                        byte clook = 0;
                                        if (desc_case) {
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
                                        if (desc_case) {
                                            if (!tvi.SubItems[2].Text.Contains(desc))
                                                continue;
                                        } else {
                                            if (!tvi.SubItems[2].Text.ToLower().Contains(desc.ToLower()))
                                                continue;
                                        }
                                    }
                                }

                                if (figure != "") {
                                    noth_found = false;
                                    if (!tvi.SubItems[3].Text.ToLower().Contains(figure.ToLower()))
                                        continue;
                                }

                                if (link != "") {
                                    noth_found = false;
                                    if (tvi.SubItems[4].Text.ToLower() != link.ToLower())
                                        continue;
                                }

                                if (metacmd != "") {
                                    noth_found = false;
                                    if (metacmd_cmp) {
                                        string[] spl1, spl2;

                                        spl1 = metacmd.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                        spl2 = tvi.SubItems[5].Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                                        bool nfound = true;
                                        byte clook = 0;
                                        if (metacmd_case) {
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
                                        if (metacmd_case) {
                                            if (!tvi.SubItems[5].Text.Contains(metacmd))
                                                continue;
                                        } else {
                                            if (!tvi.SubItems[5].Text.ToLower().Contains(metacmd.ToLower()))
                                                continue;
                                        }
                                    }
                                }

                                if (noth_found)
                                    continue;
                                break;
                            }
                            index = cindex;
                        }
                        break;
                        case Command.Type.FIND_NEXT: {
                            int cindex = index, prew_index = index;

                            ListViewItem tvi;

                            string ts;
                            string row = (item.options.TryGetValue("row", out ts) ? ts : ""),
                                cmd = (item.options.TryGetValue("cmd", out ts) ? ts : ""),
                                desc = (item.options.TryGetValue("desc", out ts) ? ts : ""),
                                figure = (item.options.TryGetValue("figure", out ts) ? ts : ""),
                                link = (item.options.TryGetValue("link", out ts) ? ts : ""),
                                metacmd = (item.options.TryGetValue("metacmd", out ts) ? ts : "");

                            bool cmd_case = (cmd == "" ? false : item.options["cmd_case"] == "CASE"),
                               desc_case = (desc == "" ? false : item.options["desc_case"] == "CASE"),
                               metacmd_case = (metacmd == "" ? false : item.options["metacmd_case"] == "CASE"),
                               cmd_cmp = (cmd == "" ? false : item.options["cmd_cmp"] == "WHOLE"),
                               desc_cmp = (desc == "" ? false : item.options["desc_cmp"] == "WHOLE"),
                               metacmd_cmp = (metacmd == "" ? false : item.options["metacmd_cmp"] == "WHOLE");

                            bool wrap = item.options.TryGetValue("wrap", out ts);
                            while (true) {
                                if (wrap) {
                                    if (lvi.Count == ++cindex) {
                                        cindex = 0;
                                        if (prew_index == -1) {
                                            goto findNext_end;
                                        }
                                    }
                                    if (prew_index == cindex) {
                                        goto findNext_end;
                                    }
                                } else {
                                    if (lvi.Count == ++cindex) {
                                        goto findNext_end;
                                    }
                                }
                                tvi = lvi[cindex];
                                bool noth_found = true;

                                if (row != "") {
                                    noth_found = false;
                                    if (tvi.Text != row.ToLower())
                                        continue;
                                }

                                if (cmd != "") {
                                    noth_found = false;
                                    if (cmd_cmp) {
                                        string[] spl1, spl2;

                                        spl1 = cmd.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                        spl2 = tvi.SubItems[1].Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                                        bool nfound = true;
                                        byte clook = 0;
                                        if (cmd_case) {
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
                                        if (cmd_case) {
                                            if (!tvi.SubItems[1].Text.Contains(cmd))
                                                continue;
                                        } else {
                                            if (!tvi.SubItems[1].Text.ToLower().Contains(cmd.ToLower()))
                                                continue;
                                        }
                                    }
                                }

                                if (desc != "") {
                                    noth_found = false;
                                    if (desc_cmp) {
                                        string[] spl1, spl2;

                                        spl1 = desc.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                        spl2 = tvi.ToolTipText.Split(new char[] { ' ', ',', '.', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                                        bool nfound = true;
                                        byte clook = 0;
                                        if (desc_case) {
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
                                        if (desc_case) {
                                            if (!tvi.SubItems[2].Text.Contains(desc))
                                                continue;
                                        } else {
                                            if (!tvi.SubItems[2].Text.ToLower().Contains(desc.ToLower()))
                                                continue;
                                        }
                                    }
                                }

                                if (figure != "") {
                                    noth_found = false;
                                    if (!tvi.SubItems[3].Text.ToLower().Contains(figure.ToLower()))
                                        continue;
                                }

                                if (link != "") {
                                    noth_found = false;
                                    if (tvi.SubItems[4].Text.ToLower() != link.ToLower())
                                        continue;
                                }

                                if (metacmd != "") {
                                    noth_found = false;
                                    if (metacmd_cmp) {
                                        string[] spl1, spl2;

                                        spl1 = metacmd.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                        spl2 = tvi.SubItems[5].Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                                        bool nfound = true;
                                        byte clook = 0;
                                        if (metacmd_case) {
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
                                        if (metacmd_case) {
                                            if (!tvi.SubItems[5].Text.Contains(metacmd))
                                                continue;
                                        } else {
                                            if (!tvi.SubItems[5].Text.ToLower().Contains(metacmd.ToLower()))
                                                continue;
                                        }
                                    }
                                }

                                if (noth_found)
                                    continue;
                                break;
                            }
                            index = cindex;
                        }
                    findNext_end:;
                        break;
                        case Command.Type.FIND_REPLACE_NEXT: {
                            int cindex = index, prew_index = index;

                            ListViewItem tvi;

                            string ts;
                            string row = (item.options.TryGetValue("row", out ts) ? ts : ""),
                                cmd = (item.options.TryGetValue("cmd", out ts) ? ts : ""),
                                desc = (item.options.TryGetValue("desc", out ts) ? ts : ""),
                                figure = (item.options.TryGetValue("figure", out ts) ? ts : ""),
                                link = (item.options.TryGetValue("link", out ts) ? ts : ""),
                                metacmd = (item.options.TryGetValue("metacmd", out ts) ? ts : "");

                            bool cmd_case = (cmd == "" ? false : item.options["cmd_case"] == "CASE"),
                               desc_case = (desc == "" ? false : item.options["desc_case"] == "CASE"),
                               metacmd_case = (metacmd == "" ? false : item.options["metacmd_case"] == "CASE"),
                               cmd_cmp = (cmd == "" ? false : item.options["cmd_cmp"] == "WHOLE"),
                               desc_cmp = (desc == "" ? false : item.options["desc_cmp"] == "WHOLE"),
                               metacmd_cmp = (metacmd == "" ? false : item.options["metacmd_cmp"] == "WHOLE");

                            bool wrap = item.options.TryGetValue("wrap", out ts);
                            while (true) {
                                if (wrap) {
                                    if (lvi.Count == ++cindex) {
                                        cindex = 0;
                                        if (prew_index == -1) {
                                            goto replaceNext_end;
                                        }
                                    }
                                    if (prew_index == cindex) {
                                        goto replaceNext_end;
                                    }
                                } else {
                                    if (lvi.Count == ++cindex) {
                                        goto replaceNext_end;
                                    }
                                }
                                tvi = lvi[cindex];
                                bool noth_found = true;

                                if (row != "") {
                                    noth_found = false;
                                    if (tvi.Text != row.ToLower())
                                        continue;
                                }

                                if (cmd != "") {
                                    noth_found = false;
                                    if (cmd_cmp) {
                                        string[] spl1, spl2;

                                        spl1 = cmd.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                        spl2 = tvi.SubItems[1].Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                                        bool nfound = true;
                                        byte clook = 0;
                                        if (cmd_case) {
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
                                        if (cmd_case) {
                                            if (!tvi.SubItems[1].Text.Contains(cmd))
                                                continue;
                                        } else {
                                            if (!tvi.SubItems[1].Text.ToLower().Contains(cmd.ToLower()))
                                                continue;
                                        }
                                    }
                                }

                                if (desc != "") {
                                    noth_found = false;
                                    if (desc_cmp) {
                                        string[] spl1, spl2;

                                        spl1 = desc.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                        spl2 = tvi.ToolTipText.Split(new char[] { ' ', ',', '.', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                                        bool nfound = true;
                                        byte clook = 0;
                                        if (desc_case) {
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
                                        if (desc_case) {
                                            if (!tvi.SubItems[2].Text.Contains(desc))
                                                continue;
                                        } else {
                                            if (!tvi.SubItems[2].Text.ToLower().Contains(desc.ToLower()))
                                                continue;
                                        }
                                    }
                                }

                                if (figure != "") {
                                    noth_found = false;
                                    if (!tvi.SubItems[3].Text.ToLower().Contains(figure.ToLower()))
                                        continue;
                                }

                                if (link != "") {
                                    noth_found = false;
                                    if (tvi.SubItems[4].Text.ToLower() != link.ToLower())
                                        continue;
                                }

                                if (metacmd != "") {
                                    noth_found = false;
                                    if (metacmd_cmp) {
                                        string[] spl1, spl2;

                                        spl1 = metacmd.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                        spl2 = tvi.SubItems[5].Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                                        bool nfound = true;
                                        byte clook = 0;
                                        if (metacmd_case) {
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
                                        if (metacmd_case) {
                                            if (!tvi.SubItems[5].Text.Contains(metacmd))
                                                continue;
                                        } else {
                                            if (!tvi.SubItems[5].Text.ToLower().Contains(metacmd.ToLower()))
                                                continue;
                                        }
                                    }
                                }

                                if (noth_found)
                                    continue;
                                break;
                            }
                            index = cindex;

                            if (item.options.TryGetValue("new_cmd", out ts)) {
                                if (tvi.SubItems[1].Text != ts)
                                    tvi.SubItems[5].Text = "";

                                tvi.SubItems[1].Text = ts;
                            }

                            if (item.options.TryGetValue("new_desc", out ts)) {
                                switch (model[cindex].figure) {
                                    case MainForm.BlockModel.Figure.Connector:
                                        break;
                                    case MainForm.BlockModel.Figure.LoopS:
                                    case MainForm.BlockModel.Figure.LoopE:
                                        tvi.ToolTipText = model[cindex].row2 = ts;
                                        tvi.SubItems[2].Text = "{loop letter} " + string.Join("{\\n}", tvi.ToolTipText.Split('\n'));
                                        break;
                                    default:
                                        tvi.ToolTipText = model[cindex].row2 = ts;
                                        tvi.SubItems[2].Text = string.Join("{\\n}", tvi.ToolTipText.Split('\n'));
                                        break;
                                }
                            }
                        }
                    replaceNext_end:;
                        break;
                        case Command.Type.FIND_REPLACE_ALL: {
                            int cindex = -1;

                            ListViewItem tvi = null;

                            string ts;
                            string row = (item.options.TryGetValue("row", out ts) ? ts : ""),
                                cmd = (item.options.TryGetValue("cmd", out ts) ? ts : ""),
                                desc = (item.options.TryGetValue("desc", out ts) ? ts : ""),
                                figure = (item.options.TryGetValue("figure", out ts) ? ts : ""),
                                link = (item.options.TryGetValue("link", out ts) ? ts : ""),
                                metacmd = (item.options.TryGetValue("metacmd", out ts) ? ts : "");

                            bool cmd_case = (cmd == "" ? false : item.options["cmd_case"] == "CASE"),
                               desc_case = (desc == "" ? false : item.options["desc_case"] == "CASE"),
                               metacmd_case = (metacmd == "" ? false : item.options["metacmd_case"] == "CASE"),
                               cmd_cmp = (cmd == "" ? false : item.options["cmd_cmp"] == "WHOLE"),
                               desc_cmp = (desc == "" ? false : item.options["desc_cmp"] == "WHOLE"),
                               metacmd_cmp = (metacmd == "" ? false : item.options["metacmd_cmp"] == "WHOLE");

                            while (true) {
                                while (true) {
                                    if (lvi.Count == ++cindex) {
                                        goto replaceAll_end;
                                    }

                                    tvi = lvi[cindex];
                                    bool noth_found = true;

                                    if (row != "") {
                                        noth_found = false;
                                        if (tvi.Text != row.ToLower())
                                            continue;
                                    }

                                    if (cmd != "") {
                                        noth_found = false;
                                        if (cmd_cmp) {
                                            string[] spl1, spl2;

                                            spl1 = cmd.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                            spl2 = tvi.SubItems[1].Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                                            bool nfound = true;
                                            byte clook = 0;
                                            if (cmd_case) {
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
                                            if (cmd_case) {
                                                if (!tvi.SubItems[1].Text.Contains(cmd))
                                                    continue;
                                            } else {
                                                if (!tvi.SubItems[1].Text.ToLower().Contains(cmd.ToLower()))
                                                    continue;
                                            }
                                        }
                                    }

                                    if (desc != "") {
                                        noth_found = false;
                                        if (desc_cmp) {
                                            string[] spl1, spl2;

                                            spl1 = desc.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                            spl2 = tvi.ToolTipText.Split(new char[] { ' ', ',', '.', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                                            bool nfound = true;
                                            byte clook = 0;
                                            if (desc_case) {
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
                                            if (desc_case) {
                                                if (!tvi.SubItems[2].Text.Contains(desc))
                                                    continue;
                                            } else {
                                                if (!tvi.SubItems[2].Text.ToLower().Contains(desc.ToLower()))
                                                    continue;
                                            }
                                        }
                                    }

                                    if (figure != "") {
                                        noth_found = false;
                                        if (!tvi.SubItems[3].Text.ToLower().Contains(figure.ToLower()))
                                            continue;
                                    }

                                    if (link != "") {
                                        noth_found = false;
                                        if (tvi.SubItems[4].Text.ToLower() != link.ToLower())
                                            continue;
                                    }

                                    if (metacmd != "") {
                                        noth_found = false;
                                        if (metacmd_cmp) {
                                            string[] spl1, spl2;

                                            spl1 = metacmd.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                            spl2 = tvi.SubItems[5].Text.Split(new char[] { ' ', ',', '.', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                                            bool nfound = true;
                                            byte clook = 0;
                                            if (metacmd_case) {
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
                                            if (metacmd_case) {
                                                if (!tvi.SubItems[5].Text.Contains(metacmd))
                                                    continue;
                                            } else {
                                                if (!tvi.SubItems[5].Text.ToLower().Contains(metacmd.ToLower()))
                                                    continue;
                                            }
                                        }
                                    }

                                    if (noth_found)
                                        continue;
                                    break;
                                }

                                if (item.options.TryGetValue("new_cmd", out ts)) {
                                    if (tvi.SubItems[1].Text != ts)
                                        tvi.SubItems[5].Text = "";

                                    tvi.SubItems[1].Text = ts;
                                }

                                if (item.options.TryGetValue("new_desc", out ts)) {
                                    switch (model[cindex].figure) {
                                        case MainForm.BlockModel.Figure.Connector:
                                            break;
                                        case MainForm.BlockModel.Figure.LoopS:
                                        case MainForm.BlockModel.Figure.LoopE:
                                            tvi.ToolTipText = model[cindex].row2 = ts;
                                            tvi.SubItems[2].Text = "{loop letter} " + string.Join("{\\n}", tvi.ToolTipText.Split('\n'));
                                            break;
                                        default:
                                            tvi.ToolTipText = model[cindex].row2 = ts;
                                            tvi.SubItems[2].Text = string.Join("{\\n}", tvi.ToolTipText.Split('\n'));
                                            break;
                                    }
                                }
                            }
                        }
                    replaceAll_end:;
                        break;
                    }
                }
                save_loaded = true;
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            if (MessageBox.Show("Do you really want to continue discarding changes?", "BlockEdit", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes) {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
}
