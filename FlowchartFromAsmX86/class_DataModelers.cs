using System.Collections.Generic;
using System.Windows.Forms;

namespace FlowchartFromAsmX86 {
    public partial class MainForm : Form {
        internal class BlockModel {
            public enum Figure { Process, FDecision, JDecision, Subprocess, Terminator, Hexagon, Connector, LoopS, LoopE, IO } //Assumed that there is only one beginning and it's located on block 0.
            public Figure figure;
            public int code_ref = -1, //reference to AsmCommand
                connection = -1;
            public string row2; //row1 - block number, determined in "code_ref + 1"; row3 - for carrying over
            public List<int> connectedToMe = new List<int>();

            public BlockModel(Figure figure, string row2, int code_ref) {
                this.figure = figure;
                this.row2 = row2;
                this.code_ref = code_ref;
            }
            public BlockModel(Figure figure, string row2, int code_ref, int connection) {
                this.figure = figure;
                this.row2 = row2;
                this.code_ref = code_ref;
                this.connection = connection;
            }
            public BlockModel(Figure figure, string row2, int code_ref, int connection, ref List<BlockModel> model) {
                this.figure = figure;
                this.row2 = row2;
                this.code_ref = code_ref;
                this.connection = connection;

                model[connection].connectedToMe.Add(model.Count);
            }
            //For Decision: flow = Yes, noflow = No; text lables by drawer  
        }

        private bool CreateDataModel(List<AsmCommand> cmds, Dictionary<int, LableMapEntry> lables_map, out List<BlockModel> model) {
            model = new List<BlockModel>();
            Stack<uint> loop_letter = new Stack<uint>(); //Needs only for Russian
            try {
                //Stage 1
                uint next_loop_letter = 0;
                for (int index = 0; index != cmds.Count; ++index) {
                    if (lables_map.ContainsKey(index)) {
                        lables_map[index].index = model.Count; //Needs to set correct indexes to connection
                        if (form_settings.rb_langRussian.Checked) {
                            for (uint i = 0; i != lables_map[index].loop_cnt; ++i) {
                                loop_letter.Push(next_loop_letter);
                                model.Add(new BlockModel(BlockModel.Figure.LoopS, GetStrFromUint(next_loop_letter++), index)); //Yes, this is a postfix increment. Yes, this is not optimal...
                            }
                        }
                    }
                    switch (cmds[index].name) {
                        case "aaa": //
                            if (form_settings.cbx_detailed.SelectedIndex == 2)
                                if (form_settings.chbx_printPercent.Checked) {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%AL - 9", index));
                                    if (form_settings.rb_langEnglish.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF | CF == 0 or AF == 1", index, model.Count + 7, ref model));
                                    else model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF | CF == 0 или AF == 1", index, model.Count + 7, ref model));

                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%AL = %AL + 6", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%AH = %AH + 1", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AF = 1", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "CF = 1", index, model.Count + 3, ref model));

                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AF = 0", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "CF = 0", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%AL = %AL & 0xF", index));
                                } else {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AL - 9", index));
                                    if (form_settings.rb_langEnglish.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF | CF == 0 or AF == 1", index, model.Count + 7, ref model));
                                    else model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF | CF == 0 или AF == 1", index, model.Count + 7, ref model));

                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AL = AL + 6", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AH = AH + 1", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AF = 1", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "CF = 1", index, model.Count + 3, ref model));

                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AF = 0", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "CF = 0", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AL = AL & 0xF", index));
                                }
                            else {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Adj. AX after BCD adding", index));
                                else model.Add(new BlockModel(BlockModel.Figure.Process, "Корр. AX после сложения BCD", index));
                            }
                            break;
                        case "aas":
                            if (form_settings.cbx_detailed.SelectedIndex == 2)
                                if (form_settings.chbx_printPercent.Checked) {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%AL - 9", index));
                                    if (form_settings.rb_langEnglish.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF | CF == 0 or AF == 1", index, model.Count + 7, ref model));
                                    else model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF | CF == 0 или AF == 1", index, model.Count + 7, ref model));

                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%AL = %AL - 6", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%AH = %AH - 1", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AF = 1", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "CF = 1", index, model.Count + 3, ref model));

                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AF = 0", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "CF = 0", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%AL = %AL & 0xF", index));
                                } else {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AL - 9", index));
                                    if (form_settings.rb_langEnglish.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF | CF == 0 or AF == 1", index, model.Count + 7, ref model));
                                    else model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF | CF == 0 или AF == 1", index, model.Count + 7, ref model));

                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AL = AL - 6", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AH = AH - 1", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AF = 1", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "CF = 1", index, model.Count + 3, ref model));

                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AF = 0", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "CF = 0", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AL = AL & 0xF", index));
                                }
                            else {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Adj. AX after BCD subtracting", index));
                                else model.Add(new BlockModel(BlockModel.Figure.Process, "Корр. AX после вычитания BCD", index));
                            }
                            break;
                        case "aam":
                            if (form_settings.cbx_detailed.SelectedIndex == 2)
                                if (form_settings.chbx_printPercent.Checked) {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%AH = %AL / 10", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%AL = %AL % 10", index));
                                } else {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AH = AL / 10", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AL = AL % 10", index));
                                }
                            else {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Adj. AX after BCD multiplying", index));
                                else model.Add(new BlockModel(BlockModel.Figure.Process, "Корр. AX после умножения BCD", index));
                            }
                            break;
                        case "aad":
                            if (form_settings.cbx_detailed.SelectedIndex == 2)
                                if (form_settings.chbx_printPercent.Checked) {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%AL = (%AH * 10) + %AL", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%AH = 0", index));
                                } else {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AL = (AH * 10) + AL", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AH = 0", index));
                                }
                            else {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Adj. AX before BCD division", index));
                                else model.Add(new BlockModel(BlockModel.Figure.Process, "Корр. AX перед делением BCD", index));
                            }
                            break;
                        case "daa": //
                            if (form_settings.cbx_detailed.SelectedIndex == 2)
                                if (form_settings.chbx_printPercent.Checked) {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%AL - 9", index));
                                    if (form_settings.rb_langEnglish.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF | CF == 0 or AF == 1", index, model.Count + 3, ref model));
                                    else model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF | CF == 0 или AF == 1", index, model.Count + 3, ref model));

                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%AL = %AL + 6", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AF = 1", index));

                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%AL - 0x9F", -1));
                                    if (form_settings.rb_langEnglish.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF | CF == 0 or CF == 1", index, model.Count + 3, ref model));
                                    else model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF | CF == 0 или CF == 1", index, model.Count + 3, ref model));

                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%AL = %AL + 0x60", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "CF = 1", index));
                                } else {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AL - 9", index));
                                    if (form_settings.rb_langEnglish.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF | CF == 0 or AF == 1", index, model.Count + 3, ref model));
                                    else model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF | CF == 0 или AF == 1", index, model.Count + 3, ref model));

                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AL = AL + 6", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AF = 1", index));

                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AL - 0x9F", -1));
                                    if (form_settings.rb_langEnglish.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF | CF == 0 or CF == 1", index, model.Count + 3, ref model));
                                    else model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF | CF == 0 или CF == 1", index, model.Count + 3, ref model));

                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AL = AL + 0x60", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "CF = 1", index));
                                }
                            else {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Adj. AX after BCD division", index));
                                else model.Add(new BlockModel(BlockModel.Figure.Process, "Корр. AX после делениея BCD", index));
                            }
                            break;
                        case "das":
                            if (form_settings.cbx_detailed.SelectedIndex == 2)
                                if (form_settings.chbx_printPercent.Checked) {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%AL - 9", index));
                                    if (form_settings.rb_langEnglish.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF | CF == 0 or AF == 1", index, model.Count + 3, ref model));
                                    else model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF | CF == 0 или AF == 1", index, model.Count + 3, ref model));

                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%AL = %AL - 6", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AF = 1", index));

                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%AL - 0x9F", -1));
                                    if (form_settings.rb_langEnglish.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF | CF == 0 or CF == 1", index, model.Count + 3, ref model));
                                    else model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF | CF == 0 или CF == 1", index, model.Count + 3, ref model));

                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%AL = %AL - 0x60", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "CF = 1", index));
                                } else {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AL - 9", index));
                                    if (form_settings.rb_langEnglish.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF | CF == 0 or AF == 1", index, model.Count + 3, ref model));
                                    else model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF | CF == 0 или AF == 1", index, model.Count + 3, ref model));

                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AL = AL - 6", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AF = 1", index));

                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AL - 0x9F", -1));
                                    if (form_settings.rb_langEnglish.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF | CF == 0 or CF == 1", index, model.Count + 3, ref model));
                                    else model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF | CF == 0 или CF == 1", index, model.Count + 3, ref model));

                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AL = AL - 0x60", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "CF = 1", index));
                                }
                            else {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Adj. AX after BCD subtracting", index));
                                else model.Add(new BlockModel(BlockModel.Figure.Process, "Корр. AX после вычитания BCD", index));
                            }
                            break;
                        case "cmc": //
                            if (form_settings.cbx_detailed.SelectedIndex == 2) {
                                model.Add(new BlockModel(BlockModel.Figure.FDecision, "CF == 1", index, model.Count + 2, ref model));
                                model.Add(new BlockModel(BlockModel.Figure.Process, "CF = 0", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "CF = 1", index));
                            } else
                                model.Add(new BlockModel(BlockModel.Figure.Process, "CF = ~CF", index));
                            break;
                        case "stc":
                            model.Add(new BlockModel(BlockModel.Figure.Process, "CF = 1", index));
                            break;
                        case "std":
                            model.Add(new BlockModel(BlockModel.Figure.Process, "DF = 1", index));
                            break;
                        case "sti":
                            model.Add(new BlockModel(BlockModel.Figure.Process, "IF = 1", index));
                            break;
                        case "clc":
                            model.Add(new BlockModel(BlockModel.Figure.Process, "CF = 0", index));
                            break;
                        case "cld":
                            model.Add(new BlockModel(BlockModel.Figure.Process, "DF = 0", index));
                            break;
                        case "cli":
                            model.Add(new BlockModel(BlockModel.Figure.Process, "IF = 0", index));
                            break;
                        case "lahf": //
                            if (form_settings.chbx_printPercent.Checked) {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%AH = flags register", index));
                                else model.Add(new BlockModel(BlockModel.Figure.Process, "%AH = регистр флагов", index));
                            } else {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AH = flags register", index));
                                else model.Add(new BlockModel(BlockModel.Figure.Process, "AH = регистр флагов", index));
                            }
                            break;
                        case "sahf":
                            if (form_settings.chbx_printPercent.Checked) {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "flags register = %AH", index));
                                else model.Add(new BlockModel(BlockModel.Figure.Process, "регистр флагов = %AH", index));
                            } else {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "flags register = AH", index));
                                else model.Add(new BlockModel(BlockModel.Figure.Process, "регистр флагов = AH", index));
                            }
                            break;
                        case "cbw": //
                            if (form_settings.chbx_printPercent.Checked) {
                                model.Add(new BlockModel(BlockModel.Figure.Process, "%AL & 0x80", index));
                                model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF == 0", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "%AH = 0xFF", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "%AH = 0", index));
                            } else {
                                model.Add(new BlockModel(BlockModel.Figure.Process, "AL & 0x80", index));
                                model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF == 0", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "AH = 0xFF", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "AH = 0", index));
                            }
                            break;
                        case "cwd":
                            if (form_settings.chbx_printPercent.Checked) {
                                model.Add(new BlockModel(BlockModel.Figure.Process, "%AX & 0x8000", index));
                                model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF == 0", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "%DX = 0xFFFF", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "%DX = 0", index));
                            } else {
                                model.Add(new BlockModel(BlockModel.Figure.Process, "AX & 0x8000", index));
                                model.Add(new BlockModel(BlockModel.Figure.FDecision, "ZF == 0", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "DX = 0xFFFF", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "DX = 0", index));
                            }
                            break;
                        case "lodsb": //
                            if (form_settings.chbx_printPercent.Checked) {
                                model.Add(new BlockModel(BlockModel.Figure.Process, "%AL = %DS:[%SI]", index));
                                model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "%SI = %SI + 1", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "%SI = %SI - 1", index));
                            } else {
                                model.Add(new BlockModel(BlockModel.Figure.Process, "AL = DS:[SI]", index));
                                model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "SI = SI + 1", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "SI = SI - 1", index));
                            }
                            break;
                        case "lodsw":
                            if (form_settings.chbx_printPercent.Checked) {
                                model.Add(new BlockModel(BlockModel.Figure.Process, "%AX = %DS:[%SI]", index));
                                model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "%SI = %SI + 2", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "%SI = %SI - 2", index));
                            } else {
                                model.Add(new BlockModel(BlockModel.Figure.Process, "AX = DS:[SI]", index));
                                model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "SI = SI + 2", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "SI = SI - 2", index));
                            }
                            break;
                        case "stosb": //
                            if (form_settings.chbx_printPercent.Checked) {
                                model.Add(new BlockModel(BlockModel.Figure.Process, "%ES:[%DI] = %AL", index));
                                model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI + 1", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI - 1", index));
                            } else {
                                model.Add(new BlockModel(BlockModel.Figure.Process, "ES:[DI] = AL", index));
                                model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI + 1", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI - 1", index));
                            }
                            break;
                        case "stosw":
                            if (form_settings.chbx_printPercent.Checked) {
                                model.Add(new BlockModel(BlockModel.Figure.Process, "%ES:[%DI] = %AX", index));
                                model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI + 2", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI - 2", index));
                            } else {
                                model.Add(new BlockModel(BlockModel.Figure.Process, "ES:[DI] = AX", index));
                                model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI + 2", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI - 2", index));
                            }
                            break;
                        case "movsb": //
                            if (form_settings.chbx_printPercent.Checked) {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Byte %ES:[%DI] = %DS:[%SI]", index));
                                else model.Add(new BlockModel(BlockModel.Figure.Process, "Байт %ES:[%DI] = %DS:[%SI]", index));
                                model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 3, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "%SI = %SI + 1", index));
                                model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI + 1", index, model.Count + 3, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "%SI = %SI - 1", index));
                                model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI - 1", index));
                            } else {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Byte ES:[DI] = DS:[SI]", index));
                                else model.Add(new BlockModel(BlockModel.Figure.Process, "Байт ES:[DI] = DS:[SI]", index));
                                model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 3, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "SI = SI + 1", index));
                                model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI + 1", index, model.Count + 3, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "SI = SI - 1", index));
                                model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI - 1", index));
                            }
                            break;
                        case "movsw":
                            if (form_settings.chbx_printPercent.Checked) {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Word %ES:[%DI] = %DS:[%SI]", index));
                                else model.Add(new BlockModel(BlockModel.Figure.Process, "Слово %ES:[%DI] = %DS:[%SI]", index));
                                model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 3, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "%SI = %SI + 2", index));
                                model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI + 2", index, model.Count + 3, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "%SI = %SI - 2", index));
                                model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI - 2", index));
                            } else {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Word ES:[DI] = DS:[SI]", index));
                                else model.Add(new BlockModel(BlockModel.Figure.Process, "Слово ES:[DI] = DS:[SI]", index));
                                model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 3, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "SI = SI + 2", index));
                                model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI + 2", index, model.Count + 3, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "SI = SI - 2", index));
                                model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI - 2", index));
                            }
                            break;
                        case "cmpsb": //
                            if (form_settings.chbx_printPercent.Checked) {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Byte %DS:[%SI] - %ES:[%DI]", index));
                                else model.Add(new BlockModel(BlockModel.Figure.Process, "Байт %DS:[%SI] - %ES:[%DI]", index));
                                model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 3, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "%SI = %SI + 1", index));
                                model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI + 1", index, model.Count + 3, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "%SI = %SI - 1", index));
                                model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI - 1", index));
                            } else {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Byte DS:[SI] - ES:[DI]", index));
                                else model.Add(new BlockModel(BlockModel.Figure.Process, "Байт DS:[SI] - ES:[DI]", index));
                                model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 3, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "SI = SI + 1", index));
                                model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI + 1", index, model.Count + 3, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "SI = SI - 1", index));
                                model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI - 1", index));
                            }
                            break;
                        case "cmpsw":
                            if (form_settings.chbx_printPercent.Checked) {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Word DS:[SI] - ES:[DI]", index));
                                else model.Add(new BlockModel(BlockModel.Figure.Process, "Слово DS:[SI] - ES:[DI]", index));
                                model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 3, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "%SI = %SI + 2", index));
                                model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI + 2", index, model.Count + 3, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "%SI = %SI - 2", index));
                                model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI - 2", index));
                            } else {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Word DS:[SI] - ES:[DI]", index));
                                else model.Add(new BlockModel(BlockModel.Figure.Process, "Слово DS:[SI] - ES:[DI]", index));
                                model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 3, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "SI = SI + 2", index));
                                model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI + 2", index, model.Count + 3, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "SI = SI - 2", index));
                                model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI - 2", index));
                            }
                            break;
                        case "scasb": //
                            if (form_settings.chbx_printPercent.Checked) {
                                model.Add(new BlockModel(BlockModel.Figure.Process, "%ES:[%DI] - %AL", index));
                                model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI + 1", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI - 1", index));
                            } else {
                                model.Add(new BlockModel(BlockModel.Figure.Process, "ES:[DI] - AL", index));
                                model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI + 1", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI - 1", index));
                            }
                            break;
                        case "scasw":
                            if (form_settings.chbx_printPercent.Checked) {
                                model.Add(new BlockModel(BlockModel.Figure.Process, "%ES:[%DI] - %AX", index));
                                model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI + 2", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI - 2", index));
                            } else {
                                model.Add(new BlockModel(BlockModel.Figure.Process, "ES:[DI] - AX", index));
                                model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI + 2", index, model.Count + 2, ref model));

                                model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI - 2", index));
                            }
                            break;
                        case "pusha": //
                            if (form_settings.cbx_detailed.SelectedIndex == 2)
                                if (form_settings.chbx_printPercent.Checked) {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SP = %SP - 2", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SS:[%SP] = %AX", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SP = %SP - 2", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SS:[%SP] = %CX", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SP = %SP - 2", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SS:[%SP] = %DX", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SP = %SP - 2", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SS:[%SP] = %BX", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SP = %SP - 2", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SS:[%SP] = %SP", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SP = %SP - 2", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SS:[%SP] = %BP", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SP = %SP - 2", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SS:[%SP] = %SI", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SP = %SP - 2", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SS:[%SP] = %DI", index));
                                } else {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "SP = SP - 16", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "SS:[SP] = AX", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "SP = SP - 2", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "SS:[SP] = CX", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "SP = SP - 2", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "SS:[SP] = DX", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "SP = SP - 2", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "SS:[SP] = BX", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "SP = SP - 2", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "SS:[SP] = SP", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "SP = SP - 2", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "SS:[SP] = BP", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "SP = SP - 2", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "SS:[SP] = SI", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "SP = SP - 2", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "SS:[SP] = DI", index));
                                }
                            else if (form_settings.cbx_detailed.SelectedIndex == 1) {
                                if (form_settings.rb_langEnglish.Checked) {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Put registers %AX,%CX,%DX,%BX,%SP,%BP,%SI,%DI on stack", index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Put registers AX,CX,DX,BX,SP,BP,SI,DI on stack", index));
                                } else {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Положить регистры %AX,%CX,%DX,%BX,%SP,%BP,%SI,%DI на стек", index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Положить регистры  AX,CX,DX,BX,SP,BP,SI,DI на стек", index));
                                }
                            } else {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Put registers on stack", index));
                                else
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Положить регистры на стек", index));
                            }
                            break;
                        case "pushf":
                            if (form_settings.cbx_detailed.SelectedIndex == 2)
                                if (form_settings.rb_langEnglish.Checked) {
                                    if (form_settings.chbx_printPercent.Checked) {
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "%SP = %SP - 2", index));
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "%SS:[%SP] = flags register", index));
                                    } else {
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "SP = SP - 2", index));
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "SS:[SP] = flags register", index));
                                    }
                                } else {
                                    if (form_settings.chbx_printPercent.Checked) {
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "%SP = %SP - 2", index));
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "%SS:[%SP] = регистр флагов", index));
                                    } else {
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "SP = SP - 2", index));
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "SS:[SP] = регистр флагов", index));
                                    }
                                }
                            else {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Put flags register on stack", index));
                                else
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Положить регистр флагов на стек", index));
                            }
                            break;
                        case "popa":
                            if (form_settings.cbx_detailed.SelectedIndex == 2)
                                if (form_settings.chbx_printPercent.Checked) {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %SS:[%SP]", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SP = %SP + 2", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SI = %SS:[%SP]", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SP = %SP + 2", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%BP = %SS:[%SP]", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SP = %SP + 4", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%BX = %SS:[%SP]", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SP = %SP + 2", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%DX = %SS:[%SP]", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SP = %SP + 2", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%CX = %SS:[%SP]", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SP = %SP + 2", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%AX = %SS:[%SP]", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SP = %SP + 2", index));
                                } else {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "DI = SS:[SP]", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "SP = SP + 2", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "SI = SS:[SP]", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "SP = SP + 2", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "BP = SS:[SP]", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "SP = SP + 4", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "BX = SS:[SP]", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "SP = SP + 2", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "DX = SS:[SP]", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "SP = SP + 2", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "CX = SS:[SP]", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "SP = SP + 2", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "AX = SS:[SP]", index));
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "SP = SP + 2", index));
                                }
                            else if (form_settings.cbx_detailed.SelectedIndex == 1) {
                                if (form_settings.rb_langEnglish.Checked) {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Get registers %DI,%SI,%BP,xx,%BX,%DX,%CX,%AX from stack", index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Get registers DI,SI,BP,xx,BX,DX,CX,AX from stack", index));
                                } else {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Взать значения %DI,%SI,%BP,xx,%BX,%DX,%CX,%AX со стека", index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Взать значения DI,SI,BP,xx,BX,DX,CX,AX со стека", index));
                                }
                            } else {
                                if (form_settings.rb_langEnglish.Checked) {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Get registers from stack", index));
                                } else {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Взать регистры со стека", index));
                                }
                            }
                            break;
                        case "popf":
                            if (form_settings.cbx_detailed.SelectedIndex == 2)
                                if (form_settings.rb_langEnglish.Checked) {
                                    if (form_settings.chbx_printPercent.Checked) {
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "flags register = %SS:[%SP]", index));
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "%SP = %SP + 2", index));
                                    } else {

                                        model.Add(new BlockModel(BlockModel.Figure.Process, "flags register = SS:[SP]", index));
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "SP = SP + 2", index));
                                    }
                                } else {
                                    if (form_settings.chbx_printPercent.Checked) {
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "регистр флагов = %SS:[%SP]", index));
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "%SP = %SP + 2", index));
                                    } else {

                                        model.Add(new BlockModel(BlockModel.Figure.Process, "регистр флагов = SS:[SP]", index));
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "SP = SP + 2", index));
                                    }
                                }
                            else {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Get flags register from stack", index));
                                else
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Взять регистр флагов со стека", index));
                            }
                            break;
                        case "xlatb": //
                            if (form_settings.chbx_printPercent.Checked)
                                model.Add(new BlockModel(BlockModel.Figure.Process, "%AL = %DS:[%BX + %AL]", index));
                            else
                                model.Add(new BlockModel(BlockModel.Figure.Process, "AL = DS:[BX + AL]", index));
                            break;
                        case "into": //
                            model.Add(new BlockModel(BlockModel.Figure.FDecision, "OF == 1", index, model.Count + 2, ref model));
                            if (form_settings.rb_langEnglish.Checked)
                                model.Add(new BlockModel(BlockModel.Figure.Subprocess, "Interrupt 4", index));
                            else model.Add(new BlockModel(BlockModel.Figure.Subprocess, "Прерывание 4", index));
                            break;
                        case "iret": //
                            if (form_settings.rb_langEnglish.Checked)
                                model.Add(new BlockModel(BlockModel.Figure.Terminator, "Interruption end", index));
                            else model.Add(new BlockModel(BlockModel.Figure.Terminator, "Конец прерывания", index));
                            break;

                        case "neg": //
                            model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = -" + cmds[index].arg1.arg, index));
                            break;
                        case "not":
                            model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = ~" + cmds[index].arg1.arg, index));
                            break;
                        case "inc": //
                            model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = " + cmds[index].arg1.arg + " + 1", index));
                            break;
                        case "dec":
                            model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = " + cmds[index].arg1.arg + " - 1", index));
                            break;
                        case "mul": //
                            if (form_settings.rb_langEnglish.Checked) {
                                if (cmds[index].arg1.isWord) {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Unsigned %DX:%AX = %AX * " + cmds[index].arg1.arg, index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Unsigned DX:AX = AX * " + cmds[index].arg1.arg, index));
                                } else {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Unsigned %AX = %AL * " + cmds[index].arg1.arg, index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Unsigned AX = AL * " + cmds[index].arg1.arg, index));
                                }
                            } else {
                                if (cmds[index].arg1.isWord) {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Беззнаковое %DX:%AX = %AX * " + cmds[index].arg1.arg, index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Беззнаковое DX:AX = AX * " + cmds[index].arg1.arg, index));
                                } else {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Беззнаковое %AX = %AL * " + cmds[index].arg1.arg, index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Беззнаковое AX = AL * " + cmds[index].arg1.arg, index));
                                }
                            }
                            break;
                        case "imul":
                            if (form_settings.rb_langEnglish.Checked) {
                                if (cmds[index].arg1.isWord) {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Signed %DX:%AX = %AX * " + cmds[index].arg1.arg, index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Signed DX:AX = AX * " + cmds[index].arg1.arg, index));
                                } else {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Signed %AX = %AL * " + cmds[index].arg1.arg, index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Signed AX = AL * " + cmds[index].arg1.arg, index));
                                }
                            } else {
                                if (cmds[index].arg1.isWord) {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Знаковое %DX:%AX = %AX * " + cmds[index].arg1.arg, index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Знаковое DX:AX = AX * " + cmds[index].arg1.arg, index));
                                } else {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Знаковое %AX = %AL * " + cmds[index].arg1.arg, index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Знаковое AX = AL * " + cmds[index].arg1.arg, index));
                                }
                            }
                            break;
                        case "div": //
                            if (form_settings.rb_langEnglish.Checked) {
                                if (cmds[index].arg1.isWord) {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Unsigned %AX = %DX:%AX / " + cmds[index].arg1.arg + "\n%DX = %DX:%AX % " + cmds[index].arg1.arg, index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Unsigned AX = DX:AX / " + cmds[index].arg1.arg + "\nDX = DX:AX % " + cmds[index].arg1.arg, index));
                                } else {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Unsigned %AL = %AX / " + cmds[index].arg1.arg + "\n%AH = %AX % " + cmds[index].arg1.arg, index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Unsigned AL = AX / " + cmds[index].arg1.arg + "\nAH = AX % " + cmds[index].arg1.arg, index));
                                }
                            } else {
                                if (cmds[index].arg1.isWord) {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Беззнаковое %AX = %DX:%AX / " + cmds[index].arg1.arg + "\n%DX = %DX:%AX % " + cmds[index].arg1.arg, index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Беззнаковое AX = DX:AX / " + cmds[index].arg1.arg + "\nDX = DX:AX % " + cmds[index].arg1.arg, index));
                                } else {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Беззнаковое %AL = %AX / " + cmds[index].arg1.arg + "\n%AH = %AX % " + cmds[index].arg1.arg, index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Беззнаковое AL = AX / " + cmds[index].arg1.arg + "\nAH = AX % " + cmds[index].arg1.arg, index));
                                }
                            }
                            break;
                        case "idiv":
                            if (form_settings.rb_langEnglish.Checked) {
                                if (cmds[index].arg1.isWord) {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Signed %AX = %DX:%AX / " + cmds[index].arg1.arg + "\n%DX = %DX:%AX % " + cmds[index].arg1.arg, index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Signed AX = DX:AX / " + cmds[index].arg1.arg + "\nDX = DX:AX % " + cmds[index].arg1.arg, index));
                                } else {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Signed %AL = %AX / " + cmds[index].arg1.arg + "\n%AH = %AX % " + cmds[index].arg1.arg, index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Signed AL = AX / " + cmds[index].arg1.arg + "\nAH = AX % " + cmds[index].arg1.arg, index));
                                }
                            } else {
                                if (cmds[index].arg1.isWord) {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Знаковое %AX = %DX:%AX / " + cmds[index].arg1.arg + "\n%DX = %DX:%AX % " + cmds[index].arg1.arg, index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Знаковое AX = DX:AX / " + cmds[index].arg1.arg + "\nDX = DX:AX % " + cmds[index].arg1.arg, index));
                                } else {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Знаковое %AL = %AX / " + cmds[index].arg1.arg + "\n%AH = %AX % " + cmds[index].arg1.arg, index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Знаковое AL = AX / " + cmds[index].arg1.arg + "\nAH = AX % " + cmds[index].arg1.arg, index));
                                }
                            }
                            break;
                        case "pop": //
                            if (form_settings.cbx_detailed.SelectedIndex == 2) {
                                model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = %SS:[%SP]", index));
                                if (cmds[index].arg1.isWord)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SP = %SP + 2", index));
                                else
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SP = %SP + 1", index));
                            } else {
                                if (form_settings.rb_langEnglish.Checked) {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Get " + cmds[index].arg1.arg + " from stack", index));
                                } else {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Взять " + cmds[index].arg1.arg + " со стека", index));
                                }
                            }
                            break;
                        case "push":
                            if (form_settings.cbx_detailed.SelectedIndex == 2) {
                                if (cmds[index].arg1.isWord)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SP = %SP - 2", index));
                                else
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "%SP = %SP - 1", index));
                                model.Add(new BlockModel(BlockModel.Figure.Process, "%SS:[%SP] = " + cmds[index].arg1.arg, index));
                            } else {
                                if (form_settings.rb_langEnglish.Checked) {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Put " + cmds[index].arg1.arg + " on stack", index));
                                } else {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Положить " + cmds[index].arg1.arg + " на стек", index));
                                }
                            }
                            break;
                        case "call": //
                            if (form_settings.rb_langEnglish.Checked) {
                                model.Add(new BlockModel(BlockModel.Figure.Subprocess, "Call " + cmds[index].arg1.arg, index));
                            } else {
                                model.Add(new BlockModel(BlockModel.Figure.Subprocess, "Вызов " + cmds[index].arg1.arg, index));
                            }
                            break;
                        case "int": //
                            if (form_settings.rb_langEnglish.Checked) {
                                model.Add(new BlockModel(BlockModel.Figure.Subprocess, "Interruption " + cmds[index].arg1.arg, index));
                            } else {
                                model.Add(new BlockModel(BlockModel.Figure.Subprocess, "Прерывание " + cmds[index].arg1.arg, index));
                            }
                            break;
                        case "jnp":
                        case "jpo":
                            model.Add(new BlockModel(BlockModel.Figure.JDecision, "PF == 0", index, -2));
                            break;
                        case "jp":
                        case "jpe":
                            model.Add(new BlockModel(BlockModel.Figure.JDecision, "PF == 1", index, -2));
                            break;
                        case "jg":
                        case "jnle":
                            if (form_settings.rb_langEnglish.Checked)
                                model.Add(new BlockModel(BlockModel.Figure.JDecision, "ZF == 0 and SF == OF", index, -2));
                            else
                                model.Add(new BlockModel(BlockModel.Figure.JDecision, "ZF == 0 и SF == OF", index, -2));
                            break;
                        case "jle":
                        case "jng":
                            if (form_settings.rb_langEnglish.Checked)
                                model.Add(new BlockModel(BlockModel.Figure.JDecision, "ZF == 1 or SF != OF", index, -2));
                            else
                                model.Add(new BlockModel(BlockModel.Figure.JDecision, "ZF == 1 или SF != OF", index, -2));
                            break;
                        case "jge":
                        case "jnl":
                            model.Add(new BlockModel(BlockModel.Figure.JDecision, "SF == OF", index, -2));
                            break;
                        case "jl":
                        case "jnge":
                            model.Add(new BlockModel(BlockModel.Figure.JDecision, "SF != OF", index, -2));
                            break;
                        case "ja":
                        case "jnbe":
                            if (form_settings.rb_langEnglish.Checked)
                                model.Add(new BlockModel(BlockModel.Figure.JDecision, "CF == 0 and ZF == 0", index, -2));
                            else
                                model.Add(new BlockModel(BlockModel.Figure.JDecision, "CF == 0 и ZF == 0", index, -2));
                            break;
                        case "jbe":
                        case "jna":
                            if (form_settings.rb_langEnglish.Checked)
                                model.Add(new BlockModel(BlockModel.Figure.JDecision, "CF == 1 or ZF == 1", index, -2));
                            else
                                model.Add(new BlockModel(BlockModel.Figure.JDecision, "CF == 1 или ZF == 1", index, -2));
                            break;
                        case "jnb":
                        case "jae":
                        case "jnc":
                            model.Add(new BlockModel(BlockModel.Figure.JDecision, "CF == 0", index, -2));
                            break;
                        case "jb":
                        case "jnae":
                        case "jc":
                            model.Add(new BlockModel(BlockModel.Figure.JDecision, "CF == 1", index, -2));
                            break;
                        case "jnz":
                        case "jne":
                            model.Add(new BlockModel(BlockModel.Figure.JDecision, "ZF == 0", index, -2));
                            break;
                        case "jz":
                        case "je":
                            model.Add(new BlockModel(BlockModel.Figure.JDecision, "ZF == 1", index, -2));
                            break;
                        case "jns":
                            model.Add(new BlockModel(BlockModel.Figure.JDecision, "SF == 0", index, -2));
                            break;
                        case "js":
                            model.Add(new BlockModel(BlockModel.Figure.JDecision, "SF == 1", index, -2));
                            break;
                        case "jno":
                            model.Add(new BlockModel(BlockModel.Figure.JDecision, "OF == 0", index, -2));
                            break;
                        case "jo":
                            model.Add(new BlockModel(BlockModel.Figure.JDecision, "OF == 1", index, -2));
                            break;
                        case "jcxz":
                            if (form_settings.chbx_printPercent.Checked)
                                model.Add(new BlockModel(BlockModel.Figure.JDecision, "%CX = 0", index, -2));
                            else
                                model.Add(new BlockModel(BlockModel.Figure.JDecision, "CX = 0", index, -2));
                            break;
                        case "jmp":
                            model.Add(new BlockModel(BlockModel.Figure.Connector, "", index, -2));
                            break;
                        case "loop": //
                            if (form_settings.rb_langEnglish.Checked) {
                                if (form_settings.chbx_printPercent.Checked) {
                                    model.Add(new BlockModel(BlockModel.Figure.FDecision, "--%CX == 0", index, -2));
                                } else {
                                    model.Add(new BlockModel(BlockModel.Figure.FDecision, "--CX == 0", index, -2));
                                }
                            } else {
                                if (form_settings.chbx_printPercent.Checked) {
                                    model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(loop_letter.Pop()) + '\n' + "--%CX != 0", index));
                                } else {
                                    model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(loop_letter.Pop()) + '\n' + "--CX != 0", index));
                                }
                            }
                            break;
                        case "loope":
                        case "loopz":
                            if (form_settings.rb_langEnglish.Checked) {
                                if (form_settings.chbx_printPercent.Checked) {
                                    model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(loop_letter.Pop()) + '\n' + "ZF == 1 and --%CX != 0", index, -2));
                                } else {
                                    model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(loop_letter.Pop()) + '\n' + "ZF == 1 and --CX != 0", index, -2));
                                }
                            } else {
                                if (form_settings.chbx_printPercent.Checked) {
                                    model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(loop_letter.Pop()) + '\n' + "ZF == 1 и --%CX != 0", index, -2));
                                } else {
                                    model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(loop_letter.Pop()) + '\n' + "ZF == 1 и --CX != 0", index, -2));
                                }
                            }
                            break;
                        case "loopne":
                        case "loopnz":
                            if (form_settings.rb_langEnglish.Checked) {
                                if (form_settings.chbx_printPercent.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(loop_letter.Pop()) + '\n' + "ZF == 0 and --%CX != 0 and", index, -2));
                                else
                                    model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(loop_letter.Pop()) + '\n' + "ZF == 0 and --CX != 0", index, -2));

                            } else {
                                if (form_settings.chbx_printPercent.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(loop_letter.Pop()) + '\n' + "ZF == 0 и --%CX != 0", index, -2));
                                else
                                    model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(loop_letter.Pop()) + '\n' + "ZF == 0 и --CX != 0", index, -2));
                            }
                            break;

                        case "adc": //
                            model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = " + cmds[index].arg1.arg + " + " + cmds[index].arg2.arg + " + CF", index));
                            break;
                        case "add":
                            model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = " + cmds[index].arg1.arg + " + " + cmds[index].arg2.arg, index));
                            break;
                        case "sub": //
                            model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = " + cmds[index].arg1.arg + " - " + cmds[index].arg2.arg, index));
                            break;
                        case "sbb":
                            model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = " + cmds[index].arg1.arg + " - " + cmds[index].arg2.arg + " - CF", index));
                            break;
                        case "sal": //
                        case "shl":
                            if (form_settings.cbx_detailed.SelectedIndex == 2) {
                                if (cmds[index].arg2.isImmediate) {
                                    if (form_settings.chbx_useHexagons.Checked) {
                                        if (form_settings.chbx_printPercent.Checked) //Optimally non-optimal...
                                            model.Add(new BlockModel(BlockModel.Figure.Hexagon, "%CL = " + cmds[index].arg2.arg, index));
                                        else model.Add(new BlockModel(BlockModel.Figure.Hexagon, "CL = " + cmds[index].arg2.arg, index));
                                    } else {
                                        if (form_settings.chbx_printPercent.Checked) //Optimally non-optimal...
                                            model.Add(new BlockModel(BlockModel.Figure.Process, "%CL = " + cmds[index].arg2.arg, index));
                                        else model.Add(new BlockModel(BlockModel.Figure.Process, "CL = " + cmds[index].arg2.arg, index));
                                    }
                                }
                                model.Add(new BlockModel(BlockModel.Figure.LoopS, GetStrFromUint(next_loop_letter), index));

                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = " + cmds[index].arg1.arg + " << 1\n[CF = shifted MSB]", index));
                                else
                                    model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = " + cmds[index].arg1.arg + " << 1\n[CF = стар. сдв. бит]", index));

                                if (form_settings.chbx_printPercent.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(next_loop_letter++) + "\n--%CL != 0", index));
                                else model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(next_loop_letter++) + "\n--CL != 0", index));
                            } else {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = " + cmds[index].arg1.arg + " << " + cmds[index].arg2.arg + "\n[CF = shifted MSB]", index));
                                else
                                    model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = " + cmds[index].arg1.arg + " << " + cmds[index].arg2.arg + "\n[CF = стар. сдв. бит]", index));
                            }
                            break;
                        case "shr":
                            if (form_settings.cbx_detailed.SelectedIndex == 2) {
                                if (cmds[index].arg2.isImmediate) {
                                    if (form_settings.chbx_useHexagons.Checked) {
                                        if (form_settings.chbx_printPercent.Checked) //Optimally non-optimal...
                                            model.Add(new BlockModel(BlockModel.Figure.Hexagon, "%CL = " + cmds[index].arg2.arg, index));
                                        else model.Add(new BlockModel(BlockModel.Figure.Hexagon, "CL = " + cmds[index].arg2.arg, index));
                                    } else {
                                        if (form_settings.chbx_printPercent.Checked) //Optimally non-optimal...
                                            model.Add(new BlockModel(BlockModel.Figure.Process, "%CL = " + cmds[index].arg2.arg, index));
                                        else model.Add(new BlockModel(BlockModel.Figure.Process, "CL = " + cmds[index].arg2.arg, index));
                                    }
                                }
                                model.Add(new BlockModel(BlockModel.Figure.LoopS, GetStrFromUint(next_loop_letter), index));

                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = " + cmds[index].arg1.arg + " >> 1\n[CF = shifted LSB]", index));
                                else
                                    model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = " + cmds[index].arg1.arg + " >> 1\n[CF = млад. сдв. бит]", index));

                                if (form_settings.chbx_printPercent.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(next_loop_letter++) + "\n--%CL != 0", index));
                                else model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(next_loop_letter++) + "\n--CL != 0", index));
                            } else {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = " + cmds[index].arg1.arg + " >> " + cmds[index].arg2.arg + "\n[CF = shifted LSB]", index));
                                else
                                    model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = " + cmds[index].arg1.arg + " >> " + cmds[index].arg2.arg + "\n[CF = млад. сдв. бит]", index));

                            }
                            break;
                        case "sar":
                            if (form_settings.cbx_detailed.SelectedIndex == 2) {
                                if (cmds[index].arg2.isImmediate) {
                                    if (form_settings.chbx_useHexagons.Checked) {
                                        if (form_settings.chbx_printPercent.Checked) //Optimally non-optimal...
                                            model.Add(new BlockModel(BlockModel.Figure.Hexagon, "%CL = " + cmds[index].arg2.arg, index));
                                        else model.Add(new BlockModel(BlockModel.Figure.Hexagon, "CL = " + cmds[index].arg2.arg, index));
                                    } else {
                                        if (form_settings.chbx_printPercent.Checked) //Optimally non-optimal...
                                            model.Add(new BlockModel(BlockModel.Figure.Process, "%CL = " + cmds[index].arg2.arg, index));
                                        else model.Add(new BlockModel(BlockModel.Figure.Process, "CL = " + cmds[index].arg2.arg, index));
                                    }
                                }
                                model.Add(new BlockModel(BlockModel.Figure.LoopS, GetStrFromUint(next_loop_letter), index));

                                if (form_settings.rb_langEnglish.Checked) {
                                    if (cmds[index].arg1.isWord)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = ((" + cmds[index].arg1.arg + " & 0x7FFF) >> 1)|(" + cmds[index].arg1.arg + " & 0x8000)\n[CF = shifted LSB]", index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = ((" + cmds[index].arg1.arg + " & 0x7F) >> 1)|(" + cmds[index].arg1.arg + " & 0x80)\n[CF = shifted LSB]", index));
                                } else {
                                    if (cmds[index].arg1.isWord)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = ((" + cmds[index].arg1.arg + " & 0x7FFF) >> 1)|(" + cmds[index].arg1.arg + " & 0x8000)\n[CF = млад. сдв. бит]", index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = ((" + cmds[index].arg1.arg + " & 0x7F) >> 1)|(" + cmds[index].arg1.arg + " & 0x80)\n[CF = млад. сдв. бит]", index));
                                }

                                if (form_settings.chbx_printPercent.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(next_loop_letter++) + "\n--%CL != 0", index));
                                else model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(next_loop_letter++) + "\n--CL != 0", index));
                            } else if (form_settings.cbx_detailed.SelectedIndex == 1) {
                                if (form_settings.rb_langEnglish.Checked) {
                                    if (cmds[index].arg1.isWord)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = ((" + cmds[index].arg1.arg + " & 0x7FFF) >> " + cmds[index].arg2.arg + ")|(" + cmds[index].arg1.arg + " & 0x8000)\n[CF = shifted LSB]", index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = ((" + cmds[index].arg1.arg + " & 0x7F) >> " + cmds[index].arg2.arg + ")|(" + cmds[index].arg1.arg + " & 0x80)\n[CF = shifted LSB]", index));
                                } else {
                                    if (cmds[index].arg1.isWord)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = ((" + cmds[index].arg1.arg + " & 0x7FFF) >> " + cmds[index].arg2.arg + ")|(" + cmds[index].arg1.arg + " & 0x8000)\n[CF = млад. сдв. бит]", index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = ((" + cmds[index].arg1.arg + " & 0x7F) >> " + cmds[index].arg2.arg + ")|(" + cmds[index].arg1.arg + " & 0x80)\n[CF = млад. сдв. бит]", index));
                                }
                            } else {
                                if (form_settings.rb_langEnglish.Checked) {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = Arithm. " + cmds[index].arg1.arg + " >> " + cmds[index].arg2.arg + "\n[CF = shifted LSB]", index));
                                } else {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = Арифм. " + cmds[index].arg1.arg + " >> " + cmds[index].arg2.arg + "\n[CF = млад. сдв. бит]", index));
                                }
                            }
                            break;
                        case "rol": //
                            if (form_settings.cbx_detailed.SelectedIndex == 2) {
                                if (cmds[index].arg2.isImmediate) {
                                    if (form_settings.chbx_useHexagons.Checked) {
                                        if (form_settings.chbx_printPercent.Checked) //Optimally non-optimal...
                                            model.Add(new BlockModel(BlockModel.Figure.Hexagon, "%CL = " + cmds[index].arg2.arg, index));
                                        else model.Add(new BlockModel(BlockModel.Figure.Hexagon, "CL = " + cmds[index].arg2.arg, index));
                                    } else {
                                        if (form_settings.chbx_printPercent.Checked) //Optimally non-optimal...
                                            model.Add(new BlockModel(BlockModel.Figure.Process, "%CL = " + cmds[index].arg2.arg, index));
                                        else model.Add(new BlockModel(BlockModel.Figure.Process, "CL = " + cmds[index].arg2.arg, index));
                                    }
                                }
                                model.Add(new BlockModel(BlockModel.Figure.LoopS, GetStrFromUint(next_loop_letter), index));

                                if (form_settings.rb_langEnglish.Checked) {
                                    if (cmds[index].arg1.isWord)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " << 1)|(" + cmds[index].arg1.arg + " >> 15)\n[CF = shifted MSB]", index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " << 1)|(" + cmds[index].arg1.arg + " >> 7)\n[CF = shifted MSB]", index));
                                } else {
                                    if (cmds[index].arg1.isWord)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " << 1)|(" + cmds[index].arg1.arg + " >> 15)\n[CF = стар. сдв. бит]", index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " << 1)|(" + cmds[index].arg1.arg + " >> 7)\n[CF = стар. сдв. бит]", index));
                                }

                                if (form_settings.chbx_printPercent.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(next_loop_letter++) + "\n--%CL != 0", index));
                                else model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(next_loop_letter++) + "\n--CL != 0", index));
                            } else if (form_settings.cbx_detailed.SelectedIndex == 1) {
                                if (form_settings.rb_langEnglish.Checked) {
                                    if (cmds[index].arg1.isWord)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " << " + cmds[index].arg2.arg + " % 16)|(" + cmds[index].arg1.arg + " >> (16 - " + cmds[index].arg2.arg + " % 16))\n[CF = shifted MSB]", index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " << " + cmds[index].arg2.arg + " % 8)|(" + cmds[index].arg1.arg + " >> (8 - " + cmds[index].arg2.arg + " % 8))\n[CF = shifted MSB]", index));
                                } else {
                                    if (cmds[index].arg1.isWord)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " << " + cmds[index].arg2.arg + " % 16)|(" + cmds[index].arg1.arg + " >> (16 - " + cmds[index].arg2.arg + " % 16))\n[CF = стар. сдв. бит]", index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " << " + cmds[index].arg2.arg + " % 8)|(" + cmds[index].arg1.arg + " >> (8 - " + cmds[index].arg2.arg + " % 8))\n[CF = стар. сдв. бит]", index));
                                }
                            } else {
                                if (form_settings.rb_langEnglish.Checked) {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = Rotate " + cmds[index].arg1.arg + " << " + cmds[index].arg2.arg + "\n[CF = shifted LSB]", index));
                                } else {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = Циклический " + cmds[index].arg1.arg + " << " + cmds[index].arg2.arg + "\n[CF = млад. сдв. бит]", index));
                                }
                            }
                            break;
                        case "ror":
                            if (form_settings.cbx_detailed.SelectedIndex == 2) {
                                if (cmds[index].arg2.isImmediate) {
                                    if (form_settings.chbx_useHexagons.Checked) {
                                        if (form_settings.chbx_printPercent.Checked) //Optimally non-optimal...
                                            model.Add(new BlockModel(BlockModel.Figure.Hexagon, "%CL = " + cmds[index].arg2.arg, index));
                                        else model.Add(new BlockModel(BlockModel.Figure.Hexagon, "CL = " + cmds[index].arg2.arg, index));
                                    } else {
                                        if (form_settings.chbx_printPercent.Checked) //Optimally non-optimal...
                                            model.Add(new BlockModel(BlockModel.Figure.Process, "%CL = " + cmds[index].arg2.arg, index));
                                        else model.Add(new BlockModel(BlockModel.Figure.Process, "CL = " + cmds[index].arg2.arg, index));
                                    }
                                }
                                model.Add(new BlockModel(BlockModel.Figure.LoopS, GetStrFromUint(next_loop_letter), index));

                                if (form_settings.rb_langEnglish.Checked) {
                                    if (cmds[index].arg1.isWord)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " >> 1)|(" + cmds[index].arg1.arg + " << 15)\n[CF = shifted LSB]", index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " >> 1)|(" + cmds[index].arg1.arg + " << 7)\n[CF = shifted LSB]", index));
                                } else {
                                    if (cmds[index].arg1.isWord)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " >> 1)|(" + cmds[index].arg1.arg + " << 15)\n[CF = млад. сдв. бит]", index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " >> 1)|(" + cmds[index].arg1.arg + " << 7)\n[CF = млад. сдв. бит]", index));
                                }

                                if (form_settings.chbx_printPercent.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(next_loop_letter++) + "\n--%CL != 0", index));
                                else model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(next_loop_letter++) + "\n--CL != 0", index));
                            } else if (form_settings.cbx_detailed.SelectedIndex == 1) {
                                if (form_settings.rb_langEnglish.Checked) {
                                    if (cmds[index].arg1.isWord)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " >> " + cmds[index].arg2.arg + " % 16)|(" + cmds[index].arg1.arg + " << (16 - " + cmds[index].arg2.arg + " % 16))\n[CF = shifted LSB]", index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " >> " + cmds[index].arg2.arg + " % 8)|(" + cmds[index].arg1.arg + " << (8 - " + cmds[index].arg2.arg + " % 8))\n[CF = shifted LSB]", index));
                                } else {
                                    if (cmds[index].arg1.isWord)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " >> " + cmds[index].arg2.arg + " % 16)|(" + cmds[index].arg1.arg + " << (16 - " + cmds[index].arg2.arg + " % 16))\n[CF = млад. сдв. бит]", index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " >> " + cmds[index].arg2.arg + " % 8)|(" + cmds[index].arg1.arg + " << (8 - " + cmds[index].arg2.arg + " % 8))\n[CF = млад. сдв. бит]", index));
                                }
                            } else {
                                if (form_settings.rb_langEnglish.Checked) {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = Rotate " + cmds[index].arg1.arg + " >> " + cmds[index].arg2.arg + "\n[CF = shifted LSB]", index));
                                } else {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = Циклический " + cmds[index].arg1.arg + " >> " + cmds[index].arg2.arg + "\n[CF = млад. сдв. бит]", index));
                                }
                            }
                            break;
                        case "rcl":
                            if (form_settings.cbx_detailed.SelectedIndex == 2) {
                                if (cmds[index].arg2.isImmediate) {
                                    if (form_settings.chbx_useHexagons.Checked) {
                                        if (form_settings.chbx_printPercent.Checked) //Optimally non-optimal...
                                            model.Add(new BlockModel(BlockModel.Figure.Hexagon, "%CL = " + cmds[index].arg2.arg, index));
                                        else model.Add(new BlockModel(BlockModel.Figure.Hexagon, "CL = " + cmds[index].arg2.arg, index));
                                    } else {
                                        if (form_settings.chbx_printPercent.Checked) //Optimally non-optimal...
                                            model.Add(new BlockModel(BlockModel.Figure.Process, "%CL = " + cmds[index].arg2.arg, index));
                                        else model.Add(new BlockModel(BlockModel.Figure.Process, "CL = " + cmds[index].arg2.arg, index));
                                    }
                                }
                                model.Add(new BlockModel(BlockModel.Figure.LoopS, GetStrFromUint(next_loop_letter), index));

                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " << 1)\n[CF = shifted MSB]", index));
                                else
                                    model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " << 1)\n[CF = стар. сдв. бит]", index));
                                model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = " + cmds[index].arg1.arg + " | CF", index));


                                if (form_settings.chbx_printPercent.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(next_loop_letter++) + "\n--%CL != 0", index));
                                else model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(next_loop_letter++) + "\n--CL != 0", index));
                            } else if (form_settings.cbx_detailed.SelectedIndex == 1) {
                                if (form_settings.rb_langEnglish.Checked) {
                                    if (form_settings.chbx_useHexagons.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Hexagon, "XX = CF", index));
                                    else model.Add(new BlockModel(BlockModel.Figure.Process, "XX = CF", index));

                                    if (cmds[index].arg1.isWord)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " << " + cmds[index].arg2.arg + " % 16 + 1)|(" + cmds[index].arg1.arg + " >> (16 - " + cmds[index].arg2.arg + " % 16))|(XX << " + cmds[index].arg2.arg + " % 16)\n[CF = shifted MSB]", index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " << " + cmds[index].arg2.arg + " % 8 + 1)|(" + cmds[index].arg1.arg + " >> (8 - " + cmds[index].arg2.arg + " % 8))|(XX << " + cmds[index].arg2.arg + " % 8)\n[CF = shifted MSB]", index));
                                } else {
                                    if (form_settings.chbx_useHexagons.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Hexagon, "XX = CF", index));
                                    else model.Add(new BlockModel(BlockModel.Figure.Process, "XX = CF", index));

                                    if (cmds[index].arg1.isWord)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " << " + cmds[index].arg2.arg + " % 16 + 1)|(" + cmds[index].arg1.arg + " >> (16 - " + cmds[index].arg2.arg + " % 16))|(XX << " + cmds[index].arg2.arg + " % 16)\n[CF = стар. сдв. бит]", index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " << " + cmds[index].arg2.arg + " % 8 + 1)|(" + cmds[index].arg1.arg + " >> (8 - " + cmds[index].arg2.arg + " % 8))|(XX << " + cmds[index].arg2.arg + " % 8)\n[CF = стар. сдв. бит]", index));
                                }
                            } else {
                                if (form_settings.rb_langEnglish.Checked) {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = Rotate (CF with " + cmds[index].arg1.arg + ") << " + cmds[index].arg2.arg, index));
                                } else {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = Циклический (CF вместе c " + cmds[index].arg1.arg + ") << " + cmds[index].arg2.arg, index));
                                }
                            }
                            break;
                        case "rcr":
                            if (form_settings.cbx_detailed.SelectedIndex == 2) {
                                if (cmds[index].arg2.isImmediate) {
                                    if (form_settings.chbx_useHexagons.Checked) {
                                        if (form_settings.chbx_printPercent.Checked) //Optimally non-optimal...
                                            model.Add(new BlockModel(BlockModel.Figure.Hexagon, "%CL = " + cmds[index].arg2.arg, index));
                                        else model.Add(new BlockModel(BlockModel.Figure.Hexagon, "CL = " + cmds[index].arg2.arg, index));
                                    } else {
                                        if (form_settings.chbx_printPercent.Checked) //Optimally non-optimal...
                                            model.Add(new BlockModel(BlockModel.Figure.Process, "%CL = " + cmds[index].arg2.arg, index));
                                        else model.Add(new BlockModel(BlockModel.Figure.Process, "CL = " + cmds[index].arg2.arg, index));
                                    }
                                }
                                model.Add(new BlockModel(BlockModel.Figure.LoopS, GetStrFromUint(next_loop_letter), index));

                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " >> 1)\n[CF = shifted LSB]", index));
                                else
                                    model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " >> 1)\n[CF = млад. сдв. бит]", index));
                                if (cmds[index].arg1.isWord)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = " + cmds[index].arg1.arg + " | (CF << 7)", index));
                                else
                                    model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = " + cmds[index].arg1.arg + " | (CF << 15)", index));

                                if (form_settings.chbx_printPercent.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(next_loop_letter++) + "\n--%CL != 0", index));
                                else model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(next_loop_letter++) + "\n--CL != 0", index));
                            } else if (form_settings.cbx_detailed.SelectedIndex == 1) {
                                if (form_settings.rb_langEnglish.Checked) {
                                    if (form_settings.chbx_useHexagons.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Hexagon, "XX = CF", index));
                                    else model.Add(new BlockModel(BlockModel.Figure.Process, "XX = CF", index));

                                    if (cmds[index].arg1.isWord)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " >> " + cmds[index].arg2.arg + " % 16 + 1)|(" + cmds[index].arg1.arg + " << (16 - " + cmds[index].arg2.arg + " % 16))|(XX << (15 - " + cmds[index].arg2.arg + " % 16)\n[CF = shifted LSB]", index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " >> " + cmds[index].arg2.arg + " % 8 + 1)|(" + cmds[index].arg1.arg + " << (8 - " + cmds[index].arg2.arg + " % 8))|(XX << (7 - " + cmds[index].arg2.arg + " % 8)\n[CF = shifted LSB]", index));
                                } else {
                                    if (form_settings.chbx_useHexagons.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Hexagon, "XX = CF", index));
                                    else model.Add(new BlockModel(BlockModel.Figure.Process, "XX = CF", index));

                                    if (cmds[index].arg1.isWord)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " >> " + cmds[index].arg2.arg + " % 16 + 1)|(" + cmds[index].arg1.arg + " >> (16 - " + cmds[index].arg2.arg + " % 16))|(XX << (15 - " + cmds[index].arg2.arg + " % 16)\n[CF = млад. сдв. бит]", index));
                                    else
                                        model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = (" + cmds[index].arg1.arg + " >> " + cmds[index].arg2.arg + " % 8 + 1)|(" + cmds[index].arg1.arg + " >> (8 - " + cmds[index].arg2.arg + " % 8))|(XX << (7 - " + cmds[index].arg2.arg + " % 8)\n[CF = млад. сдв. бит]", index));
                                }
                            } else {
                                if (form_settings.rb_langEnglish.Checked) {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = Rotate (" + cmds[index].arg1.arg + " with CF) >> " + cmds[index].arg2.arg, index));
                                } else {
                                    model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = Циклический (" + cmds[index].arg1.arg + " вместе c CF) >> " + cmds[index].arg2.arg, index));
                                }
                            }
                            break;
                        case "and": //
                            model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = " + cmds[index].arg1.arg + " & " + cmds[index].arg2.arg, index));
                            break;
                        case "test":
                            model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " & " + cmds[index].arg2.arg, index));
                            break;
                        case "or":
                            model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = " + cmds[index].arg1.arg + " | " + cmds[index].arg2.arg, index));
                            break;
                        case "xor":
                            model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = " + cmds[index].arg1.arg + " ^ " + cmds[index].arg2.arg, index));
                            break;
                        case "cmp": //
                            model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " - " + cmds[index].arg2.arg, index));
                            break;
                        case "lds": //
                            if (form_settings.cbx_detailed.SelectedIndex == 0) {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Load 1-st word of \"" + cmds[index].arg2.arg + "\" to DS, 2-nd to " + cmds[index].arg1.arg, index));
                                else
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Загр. 1-ое слово \"" + cmds[index].arg2.arg + "\" в %DS, 2-е в " + cmds[index].arg1.arg, index));
                            } else {
                                if (form_settings.rb_langEnglish.Checked) {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Load 1-st word from var. \"" + cmds[index].arg2.arg + "\" into %DS, 2-nd - into " + cmds[index].arg1.arg, index));
                                    else model.Add(new BlockModel(BlockModel.Figure.Process, "Load 1-st word from var. \"" + cmds[index].arg2.arg + "\" into DS, 2-nd - into " + cmds[index].arg1.arg, index));
                                } else {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Загр. 1-ое слово из перем. \"" + cmds[index].arg2.arg + "\" в %DS, 2-е - в " + cmds[index].arg1.arg, index));
                                    else model.Add(new BlockModel(BlockModel.Figure.Process, "Загр. 1-ое слово из перем. \"" + cmds[index].arg2.arg + "\" в DS, 2-е - в " + cmds[index].arg1.arg, index));
                                }
                            }
                            break;
                        case "les":
                            if (form_settings.cbx_detailed.SelectedIndex == 0) {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Load 1-st word of \"" + cmds[index].arg2.arg + "\" to %ES, 2-nd to " + cmds[index].arg1.arg, index));
                                else
                                    model.Add(new BlockModel(BlockModel.Figure.Process, "Загр. 1-ое слово \"" + cmds[index].arg2.arg + "\" в %ES, 2-е в " + cmds[index].arg1.arg, index));
                            } else {
                                if (form_settings.rb_langEnglish.Checked) {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Load 1-st word from \"" + cmds[index].arg2.arg + "\" into %ES, 2-nd - into " + cmds[index].arg1.arg, index));
                                    else model.Add(new BlockModel(BlockModel.Figure.Process, "Load 1-st word from \"" + cmds[index].arg2.arg + "\" into ES, 2-nd - into " + cmds[index].arg1.arg, index));
                                } else {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Загр. 1-ое слово из \"" + cmds[index].arg2.arg + "\" в %ES, 2-е - в " + cmds[index].arg1.arg, index));
                                    else model.Add(new BlockModel(BlockModel.Figure.Process, "Загр. 1-ое слово из \"" + cmds[index].arg2.arg + "\" в ES, 2-е - в " + cmds[index].arg1.arg, index));
                                }
                            }
                            break;
                        case "lea": //
                            if (form_settings.rb_langEnglish.Checked) {
                                model.Add(new BlockModel(BlockModel.Figure.Process, "Load offset of \"" + cmds[index].arg2.arg + "\" into " + cmds[index].arg1.arg, index));
                            } else {
                                model.Add(new BlockModel(BlockModel.Figure.Process, "Загр. смещ. \"" + cmds[index].arg2.arg + "\" в " + cmds[index].arg1.arg, index));
                            }
                            break;
                        case "mov":
                            model.Add(new BlockModel(BlockModel.Figure.Process, LetUp(cmds[index].arg1.arg) + " = " + cmds[index].arg2.arg, index));
                            break;
                        case "in": //
                            if (form_settings.cbx_detailed.SelectedIndex == 0) {
                                if (form_settings.rb_langEnglish.Checked) {
                                    if (cmds[index].arg2.isImmediate) {
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Get " + cmds[index].arg1.arg + " from port " + cmds[index].arg2.arg, index));
                                    } else {
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Get " + cmds[index].arg1.arg + " from port from " + cmds[index].arg2.arg, index));
                                    }
                                } else {
                                    if (cmds[index].arg2.isImmediate) {
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Получить " + cmds[index].arg1.arg + " из порта " + cmds[index].arg2.arg, index));
                                    } else {
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Получить " + cmds[index].arg1.arg + " из порта из \"" + cmds[index].arg2.arg + "\"", index));
                                    }
                                }
                            } else {
                                if (form_settings.rb_langEnglish.Checked) {
                                    if (cmds[index].arg2.isImmediate) {
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Get " + cmds[index].arg1.arg + " from the port number " + cmds[index].arg2.arg, index));
                                    } else {
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Get " + cmds[index].arg1.arg + " from the port number from " + cmds[index].arg2.arg, index));
                                    }
                                } else {
                                    if (cmds[index].arg2.isImmediate) {
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Получить данные в " + cmds[index].arg1.arg + " из порта с номером " + cmds[index].arg2.arg, index));
                                    } else {
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Получить данные в " + cmds[index].arg1.arg + " из порта с номером из \"" + cmds[index].arg2.arg + "\"", index));
                                    }
                                }
                            }
                            break;
                        case "out":
                            if (form_settings.cbx_detailed.SelectedIndex == 0) {
                                if (form_settings.rb_langEnglish.Checked) {
                                    if (cmds[index].arg2.isImmediate) {
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Set " + cmds[index].arg1.arg + " to port " + cmds[index].arg2.arg, index));
                                    } else {
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Set " + cmds[index].arg1.arg + " to port from " + cmds[index].arg2.arg, index));
                                    }
                                } else {
                                    if (cmds[index].arg2.isImmediate) {
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Записать " + cmds[index].arg1.arg + " в порт " + cmds[index].arg2.arg, index));
                                    } else {
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Записать " + cmds[index].arg1.arg + " в порт из \"" + cmds[index].arg2.arg + "\"", index));
                                    }
                                }
                            } else {
                                if (form_settings.rb_langEnglish.Checked) {
                                    if (cmds[index].arg2.isImmediate) {
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Set " + cmds[index].arg1.arg + " to the port number " + cmds[index].arg2.arg, index));
                                    } else {
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Set " + cmds[index].arg1.arg + " to the port number from " + cmds[index].arg2.arg, index));
                                    }
                                } else {
                                    if (cmds[index].arg2.isImmediate) {
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Записать " + cmds[index].arg1.arg + " в порт с номером " + cmds[index].arg2.arg, index));
                                    } else {
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "Записать " + cmds[index].arg1.arg + " в порт с номером из \"" + cmds[index].arg2.arg + "\"", index));
                                    }
                                }
                            }
                            break;
                        case "xchg": //
                            if (form_settings.rb_langEnglish.Checked)
                                model.Add(new BlockModel(BlockModel.Figure.Process, "Swap " + cmds[index].arg1.arg + " and " + cmds[index].arg2.arg, index));
                            else
                                model.Add(new BlockModel(BlockModel.Figure.Process, "Поменять местами " + cmds[index].arg1.arg + " и " + cmds[index].arg2.arg, index));
                            break;
                        case "rep":
                            model.Add(new BlockModel(BlockModel.Figure.LoopS, GetStrFromUint(next_loop_letter), index));
                            RepBlockInterpret(model, cmds[index].arg1.arg, index);
                            if (form_settings.chbx_printPercent.Checked)
                                model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(next_loop_letter++) + "\n--%CX != 0", index));
                            else model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(next_loop_letter++) + "\n--CX != 0", index));
                            break;
                        case "repe":
                            model.Add(new BlockModel(BlockModel.Figure.LoopS, GetStrFromUint(next_loop_letter), index));
                            RepBlockInterpret(model, cmds[index].arg1.arg, index);
                            if (form_settings.rb_langEnglish.Checked) {
                                if (form_settings.chbx_printPercent.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(next_loop_letter++) + "\n--%CX != 0 and ZF == 1", index));
                                else model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(next_loop_letter++) + "\n--CX != 0 and ZF == 1", index));
                            } else {
                                if (form_settings.chbx_printPercent.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(next_loop_letter++) + "\n--%CX != 0 и ZF == 1", index));
                                else model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(next_loop_letter++) + "\n--CX != 0 и ZF == 1", index));
                            }
                            break;
                        case "repne":
                        case "repz":
                            model.Add(new BlockModel(BlockModel.Figure.LoopS, GetStrFromUint(next_loop_letter), index));
                            RepBlockInterpret(model, cmds[index].arg1.arg, index);
                            if (form_settings.rb_langEnglish.Checked) {
                                if (form_settings.chbx_printPercent.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(next_loop_letter++) + "\n--%CX != 0 and ZF == 0", index));
                                else model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(next_loop_letter++) + "\n--CX != 0 and ZF == 0", index));
                            } else {
                                if (form_settings.chbx_printPercent.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(next_loop_letter++) + "\n--%CX != 0 и ZF == 0", index));
                                else model.Add(new BlockModel(BlockModel.Figure.LoopE, GetStrFromUint(next_loop_letter++) + "\n--CX != 0 и ZF == 0", index));
                            }
                            break;

                        case "ret": //
                        case "retf":
                            if (cmds[index].arg1.arg == null) {
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Terminator, "End " + current_name, index));
                                else model.Add(new BlockModel(BlockModel.Figure.Terminator, "Конец " + current_name, index));
                            } else {
                                if (form_settings.chbx_useHexagons.Checked) {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Hexagon, "%SP = %SP + 2", index));
                                    else model.Add(new BlockModel(BlockModel.Figure.Hexagon, "SP = SP + 2", index));
                                } else {
                                    if (form_settings.chbx_printPercent.Checked)
                                        model.Add(new BlockModel(BlockModel.Figure.Process, "%SP = %SP + 2", index));
                                    else model.Add(new BlockModel(BlockModel.Figure.Process, "SP = SP + 2", index));
                                }
                                if (form_settings.rb_langEnglish.Checked)
                                    model.Add(new BlockModel(BlockModel.Figure.Terminator, "End " + current_name, index));
                                else model.Add(new BlockModel(BlockModel.Figure.Terminator, "Конец " + current_name, index));
                            }
                            break;
                    }
                }
                //Stage 2
                for (int i = 0; i != model.Count; ++i) {
                    if (model[i].connection == -2) {
                        lables_map.TryGetValue(cmds[model[i].code_ref].reference, out LableMapEntry lable);
                        model[i].connection = lable.index;
                        model[lable.index].connectedToMe.Add(i);
                    }
                }
            } catch {
                return false;
            }
            return true;
        }

        private void RepBlockInterpret(List<BlockModel> model, string cmd, int index) {
            switch (cmd) {
                case "lodsb": //
                    if (form_settings.chbx_printPercent.Checked) {
                        model.Add(new BlockModel(BlockModel.Figure.Process, "%AL = %DS:[%SI]", index));
                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 2, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "%SI = %SI + 1", index, model.Count + 2, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "%SI = %SI - 1", index));
                    } else {
                        model.Add(new BlockModel(BlockModel.Figure.Process, "AL = DS:[SI]", index));
                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 2, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "SI = SI + 1", index, model.Count + 2, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "SI = SI - 1", index));
                    }
                    break;
                case "lodsw":
                    if (form_settings.chbx_printPercent.Checked) {
                        model.Add(new BlockModel(BlockModel.Figure.Process, "%AX = %DS:[%SI]", index));
                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 2, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "%SI = %SI + 2", index, model.Count + 2, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "%SI = %SI - 2", index));
                    } else {
                        model.Add(new BlockModel(BlockModel.Figure.Process, "AX = DS:[SI]", index));
                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 2, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "SI = SI + 2", index, model.Count + 2, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "SI = SI - 2", index));
                    }
                    break;
                case "stosb": //
                    if (form_settings.chbx_printPercent.Checked) {
                        model.Add(new BlockModel(BlockModel.Figure.Process, "%ES:[%DI] = %AL", index));
                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 2, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI + 1", index, model.Count + 2, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI - 1", index));
                    } else {
                        model.Add(new BlockModel(BlockModel.Figure.Process, "ES:[DI] = AL", index));
                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 2, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI + 1", index, model.Count + 2, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI - 1", index));
                    }
                    break;
                case "stosw":
                    if (form_settings.chbx_printPercent.Checked) {
                        model.Add(new BlockModel(BlockModel.Figure.Process, "%ES:[%DI] = %AX", index));
                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 2, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI + 2", index, model.Count + 2, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI - 2", index));
                    } else {
                        model.Add(new BlockModel(BlockModel.Figure.Process, "ES:[DI] = AX", index));
                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 2, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI + 2", index, model.Count + 2, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI - 2", index));
                    }
                    break;
                case "movsb": //
                    if (form_settings.chbx_printPercent.Checked) {
                        if (form_settings.rb_langEnglish.Checked)
                            model.Add(new BlockModel(BlockModel.Figure.Process, "Byte %ES:[%DI] = %DS:[%SI]", index));
                        else model.Add(new BlockModel(BlockModel.Figure.Process, "Байт %ES:[%DI] = %DS:[%SI]", index));
                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 3, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "%SI = %SI + 1", index));
                        model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI + 1", index, model.Count + 3, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "%SI = %SI - 1", index));
                        model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI - 1", index));
                    } else {
                        if (form_settings.rb_langEnglish.Checked)
                            model.Add(new BlockModel(BlockModel.Figure.Process, "Byte ES:[DI] = DS:[SI]", index));
                        else model.Add(new BlockModel(BlockModel.Figure.Process, "Байт ES:[DI] = DS:[SI]", index));
                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 3, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "SI = SI + 1", index));
                        model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI + 1", index, model.Count + 3, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "SI = SI - 1", index));
                        model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI - 1", index));
                    }
                    break;
                case "movsw":
                    if (form_settings.chbx_printPercent.Checked) {
                        if (form_settings.rb_langEnglish.Checked)
                            model.Add(new BlockModel(BlockModel.Figure.Process, "Word %ES:[%DI] = %DS:[%SI]", index));
                        else model.Add(new BlockModel(BlockModel.Figure.Process, "Слово %ES:[%DI] = %DS:[%SI]", index));
                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 3, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "%SI = %SI + 2", index));
                        model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI + 2", index, model.Count + 3, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "%SI = %SI - 2", index));
                        model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI - 2", index));
                    } else {
                        if (form_settings.rb_langEnglish.Checked)
                            model.Add(new BlockModel(BlockModel.Figure.Process, "Word ES:[DI] = DS:[SI]", index));
                        else model.Add(new BlockModel(BlockModel.Figure.Process, "Слово ES:[DI] = DS:[SI]", index));
                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 3, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "SI = SI + 2", index));
                        model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI + 2", index, model.Count + 3, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "SI = SI - 2", index));
                        model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI - 2", index));
                    }
                    break;
                case "cmpsb": //
                    if (form_settings.chbx_printPercent.Checked) {
                        if (form_settings.rb_langEnglish.Checked)
                            model.Add(new BlockModel(BlockModel.Figure.Process, "Byte %DS:[%SI] - %ES:[%DI]", index));
                        else model.Add(new BlockModel(BlockModel.Figure.Process, "Байт %DS:[%SI] - %ES:[%DI]", index));
                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 3, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "%SI = %SI + 1", index));
                        model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI + 1", index, model.Count + 3, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "%SI = %SI - 1", index));
                        model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI - 1", index));
                    } else {
                        if (form_settings.rb_langEnglish.Checked)
                            model.Add(new BlockModel(BlockModel.Figure.Process, "Byte DS:[SI] - ES:[DI]", index));
                        else model.Add(new BlockModel(BlockModel.Figure.Process, "Байт DS:[SI] - ES:[DI]", index));
                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 3, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "SI = SI + 1", index));
                        model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI + 1", index, model.Count + 3, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "SI = SI - 1", index));
                        model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI - 1", index));
                    }
                    break;
                case "cmpsw":
                    if (form_settings.chbx_printPercent.Checked) {
                        if (form_settings.rb_langEnglish.Checked)
                            model.Add(new BlockModel(BlockModel.Figure.Process, "Word DS:[SI] - ES:[DI]", index));
                        else model.Add(new BlockModel(BlockModel.Figure.Process, "Слово DS:[SI] - ES:[DI]", index));
                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 3, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "%SI = %SI + 2", index));
                        model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI + 2", index, model.Count + 3, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "%SI = %SI - 2", index));
                        model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI - 2", index));
                    } else {
                        if (form_settings.rb_langEnglish.Checked)
                            model.Add(new BlockModel(BlockModel.Figure.Process, "Word DS:[SI] - ES:[DI]", index));
                        else model.Add(new BlockModel(BlockModel.Figure.Process, "Слово DS:[SI] - ES:[DI]", index));
                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 3, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "SI = SI + 2", index));
                        model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI + 2", index, model.Count + 3, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "SI = SI - 2", index));
                        model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI - 2", index));
                    }
                    break;
                case "scasb": //
                    if (form_settings.chbx_printPercent.Checked) {
                        model.Add(new BlockModel(BlockModel.Figure.Process, "%ES:[%DI] - %AL", index));
                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 2, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI + 1", index, model.Count + 2, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI - 1", index));
                    } else {
                        model.Add(new BlockModel(BlockModel.Figure.Process, "ES:[DI] - AL", index));
                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 2, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI + 1", index, model.Count + 2, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI - 1", index));
                    }
                    break;
                case "scasw":
                    if (form_settings.chbx_printPercent.Checked) {
                        model.Add(new BlockModel(BlockModel.Figure.Process, "%ES:[%DI] - %AX", index));
                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 2, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI + 2", index, model.Count + 2, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "%DI = %DI - 2", index));
                    } else {
                        model.Add(new BlockModel(BlockModel.Figure.Process, "ES:[DI] - AX", index));
                        model.Add(new BlockModel(BlockModel.Figure.FDecision, "DF == 0", index, model.Count + 2, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI + 2", index, model.Count + 2, ref model));

                        model.Add(new BlockModel(BlockModel.Figure.Process, "DI = DI - 2", index));
                    }
                    break;
            }
        }

        private string GetStrFromUint(uint arg) {
            ushort quotient = (ushort)(arg / 29),
                remainder = (ushort)(arg % 29);
            if (quotient != 0) {
                return GetStrFromUint(quotient) + '.' + RUS_ALPHABET[remainder];
            } else
                return RUS_ALPHABET[remainder].ToString();
        }

        private string LetUp(string str) {
            return char.ToUpper(str[0]) + str.Substring(1);
        }
    }
}
