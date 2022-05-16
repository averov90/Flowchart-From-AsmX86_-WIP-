using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FlowchartFromAsmX86 {
    public partial class MainForm : Form {
        internal struct AsmCommandArg {
            public AsmCommandArg(string arg, bool isWord, bool isImmediate) {
                this.arg = arg;
                this.isWord = isWord;
                this.isImmediate = isImmediate;
            }
            public string arg;
            public bool isWord, isImmediate;
        }
        internal class AsmCommand {
            public AsmCommand(int line, string name) {
                this.line = line;
                this.name = name;
            }
            public AsmCommand(int line, string name, AsmCommandArg arg1) {
                this.line = line;
                this.name = name;
                this.arg1 = arg1;
            }
            public AsmCommand(int line, string name, AsmCommandArg arg1, AsmCommandArg arg2) {
                this.line = line;
                this.name = name;
                this.arg1 = arg1;
                this.arg2 = arg2;
            }
            public AsmCommand(int line, string name, int reference) {
                this.line = line;
                this.name = name;
                this.reference = reference;
            }

            public string name;
            public AsmCommandArg arg1, arg2;
            public int reference = -1, line;
        }

        private class LableMapEntry {
            public LableMapEntry() {
                loop_cnt = 0;
                index = -1;
            }
            public LableMapEntry(ushort loop_cnt) {
                this.loop_cnt = loop_cnt;
                index = -1;
            }
            public LableMapEntry(int index) {
                this.index = index;
                loop_cnt = 0;
            }
            public LableMapEntry(int index, ushort loop_cnt) {
                this.index = index;
                this.loop_cnt = loop_cnt;
            }
            public int index;
            public ushort loop_cnt;
        }

        private bool ParseCode(ref string[] code, out List<AsmCommand> cmds, out Dictionary<int, LableMapEntry> lables_map) {
            cmds = new List<AsmCommand>();
            lables_map = new Dictionary<int, LableMapEntry>(); //Preparing a lables map for the future

            Dictionary<string, int> lables_found = new Dictionary<string, int>();
            Dictionary<string, List<LableMapEntry>> lables_wanted = new Dictionary<string, List<LableMapEntry>>();
            try {
                int tmpi;
                string cline, str;
                for (int i = 1, count = code.Length - 1; i != count; ++i) {
                    cline = code[i];
                    int ind;
                    if ((ind = cline.IndexOf(':')) != -1) {
                        str = cline.Substring(0, ind).Trim();
                        if (Regex.IsMatch(str, @"\s") || lables_found.ContainsKey(str)) return false; //Check label is correct

                        lables_found.Add(str, cmds.Count);
                        if (lables_wanted.TryGetValue(str, out List<LableMapEntry> tmpl)) {
                            tmpi = 0;
                            foreach (var item in tmpl) {
                                cmds[item.index].reference = cmds.Count;
                                if (item.loop_cnt != 0) ++tmpi;
                            }
                            lables_wanted.Remove(str);
                            lables_map.Add(cmds.Count, new LableMapEntry((ushort)tmpi)); //Filling in the lables map for the future
                        } else
                            lables_map.Add(cmds.Count, new LableMapEntry()); //Filling in the lables map for the future
                    } else {
                        if ((ind = cline.TakeWhile(c => !char.IsWhiteSpace(c)).Count()) == 0) //INFO: unoptimal place
                            str = cline.ToLower();
                        else {
                            str = cline.Substring(0, ind).ToLower();
                            cline = cline.Substring(ind).TrimStart();
                        }
                        string[] spl;
                        switch (str) {
                            case "aaa": //
                            case "aas":
                            case "aam":
                            case "aad":
                            case "daa": //
                            case "das":
                            case "cmc": //
                            case "stc":
                            case "std":
                            case "sti":
                            case "clc":
                            case "cld":
                            case "cli":
                            case "lahf": //
                            case "sahf":
                            case "cbw": //
                            case "cwd":
                            case "lodsb": //
                            case "lodsw":
                            case "stosb": //
                            case "stosw":
                            case "movsb": //
                            case "movsw":
                            case "cmpsb": //
                            case "cmpsw":
                            case "scasb": //
                            case "scasw":
                            case "pusha": //
                            case "pushf":
                            case "popa":
                            case "popf":
                            case "xlatb": //
                            case "into": //
                            case "iret": //
                            case "hlt": //
                                cmds.Add(new AsmCommand(i, str));
                                break;
                            case "neg": //
                            case "not":
                            case "inc": //
                            case "dec":
                            case "mul": //
                            case "imul":
                            case "div": //
                            case "idiv":
                            case "pop": //
                            case "push":
                            case "call": //
                            case "int": //
                                cmds.Add(new AsmCommand(i, str, CorrectCommandArg(Regex.Replace(cline, @"\s", ""))));
                                break;
                            case "loop": //
                            case "loope":
                            case "loopne":
                            case "loopnz":
                            case "loopz":
                                if (lables_found.TryGetValue(cline, out tmpi)) {
                                    cmds.Add(new AsmCommand(i, str, tmpi));
                                    ++lables_map[tmpi].loop_cnt;
                                } else {
                                    if (lables_wanted.ContainsKey(cline)) lables_wanted[cline].Add(new LableMapEntry(cmds.Count, 1));
                                    else lables_wanted.Add(cline, new List<LableMapEntry>(new LableMapEntry[] { new LableMapEntry(cmds.Count, 1) }));
                                    cmds.Add(new AsmCommand(i, str));
                                }
                                break;
                            case "ja": //
                            case "jae":
                            case "jb":
                            case "jbe":
                            case "jc":
                            case "jcxz":
                            case "je":
                            case "jg":
                            case "jge":
                            case "jl":
                            case "jle":
                            case "jna":
                            case "jnae":
                            case "jnb":
                            case "jnbe":
                            case "jnc":
                            case "jne":
                            case "jng":
                            case "jnge":
                            case "jnl":
                            case "jnle":
                            case "jno":
                            case "jnp":
                            case "jns":
                            case "jnz":
                            case "jo":
                            case "jp":
                            case "jpe":
                            case "jpo":
                            case "js":
                            case "jz":
                                if (lables_found.TryGetValue(cline, out tmpi))
                                    cmds.Add(new AsmCommand(i, str, tmpi));
                                else {
                                    if (lables_wanted.ContainsKey(cline)) lables_wanted[cline].Add(new LableMapEntry(cmds.Count));
                                    else lables_wanted.Add(cline, new List<LableMapEntry>(new LableMapEntry[] { new LableMapEntry(cmds.Count) }));
                                    cmds.Add(new AsmCommand(i, str));
                                }
                                break;
                            case "jmp":
                                if (int.TryParse(cline, out _) || int.TryParse(cline.Remove(cline.Length - 1), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out _)) {
                                    throw new Exception();
                                } else {
                                    if (lables_found.TryGetValue(cline, out tmpi))
                                        cmds.Add(new AsmCommand(i, str, tmpi));
                                    else {
                                        if (lables_wanted.ContainsKey(cline)) lables_wanted[cline].Add(new LableMapEntry(cmds.Count));
                                        else lables_wanted.Add(cline, new List<LableMapEntry>(new LableMapEntry[] { new LableMapEntry(cmds.Count) }));
                                        cmds.Add(new AsmCommand(i, str));
                                    }
                                }
                                break;
                            case "adc": //
                            case "add":
                            case "sub": //
                            case "sbb":
                            case "rcl": //
                            case "rcr":
                            case "rol":
                            case "ror":
                            case "sal": //
                            case "sar":
                            case "shl":
                            case "shr":
                            case "and": //
                            case "test":
                            case "or":
                            case "xor":
                            case "cmp": //
                            case "lds": //
                            case "les":
                            case "lea": //
                            case "mov":
                            case "in": //
                            case "out":
                            case "xchg": //
                                spl = Regex.Replace(cline, @"\s", "").Split(',');
                                cmds.Add(new AsmCommand(i, str, CorrectCommandArg(spl[0]), CorrectCommandArg(spl[1])));
                                break;
                            case "rep":
                            case "repe":
                            case "repne":
                            case "repz":
                                spl = Regex.Replace(cline, @"\s{1,}", " ").Split(' ');
                                cmds.Add(new AsmCommand(i, str, new AsmCommandArg(spl[0].ToLower(), false, false), new AsmCommandArg(string.Join(" ", spl, 1, spl.Length - 1), false, false)));
                                break;
                            case "ret": //
                            case "retf":
                                if (cline != "") {
                                    if (int.TryParse(cline, out _)) {
                                        cline = cline.TrimStart('0');
                                        if (cline.Length == 0) throw new Exception();
                                        cmds.Add(new AsmCommand(i, str, new AsmCommandArg(cline, true, true)));
                                    } else if (int.TryParse(cline.Remove(cline.Length - 1), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out _)) {
                                        cline = cline.Remove(cline.Length - 1).TrimStart('0');
                                        if (cline.Length == 0) throw new Exception();
                                        cmds.Add(new AsmCommand(i, str, new AsmCommandArg("0x" + cline, true, true)));
                                    } else throw new Exception();
                                } else
                                    cmds.Add(new AsmCommand(i, str));
                                break;
                            case "nop":
                            case "":
                                break;
                            default: //"hlt" here
                                return false;
                        }
                    }
                }
            } catch {
                return false;
            }
            if (lables_wanted.Count != 0) return false;
            return true;
        }

        /// <summary>
        /// Universal argument processor. 
        /// Conniving to some violations in the command arguments, however, the correct arguments are handled correctly.
        /// </summary>
        private AsmCommandArg CorrectCommandArg(string arg) { //There are many shortcomings.
            string tmp = arg.ToUpper(); //Non-optimal. It would be better to use a global variables. But to make it easier for you to read...
            if (tmp.StartsWith("OFFSET")) {
                tmp = tmp.Substring(6);

                if (form_settings.rb_langEnglish.Checked)
                    return new AsmCommandArg("address of " + tmp, true, false);
                else return new AsmCommandArg("адрес " + tmp, true, false);
            } else if (tmp.StartsWith("BYTEPTR")) {
                tmp = tmp.Substring(7);

                int tpos1 = tmp.LastIndexOf('['), tpos2 = tmp.IndexOf(']');
                if (tpos1 == 0 && tpos2 == tmp.Length - 1) {
                    tmp = tmp.Substring(1, tmp.Length - 2);

                    if (form_settings.rb_langEnglish.Checked)
                        return new AsmCommandArg("byte [" + CorrectPtrExpression(arg, 8, tmp) + "]", false, false);
                    else
                        return new AsmCommandArg("байт [" + CorrectPtrExpression(arg, 8, tmp) + "]", false, false);
                } else if (tpos1 == -1 || tpos2 == -1) {
                    if (form_settings.rb_langEnglish.Checked)
                        return new AsmCommandArg("byte [" + CorrectPtrExpression(arg, 7, tmp) + "]", false, false);
                    else
                        return new AsmCommandArg("байт [" + CorrectPtrExpression(arg, 7, tmp) + "]", false, false);
                } else
                    throw new Exception();

            } else if (tmp.StartsWith("WORDPTR")) {
                tmp = tmp.Substring(7);

                int tpos1 = tmp.LastIndexOf('['), tpos2 = tmp.IndexOf(']');
                if (tpos1 == 0 && tpos2 == tmp.Length - 1) {
                    tmp = tmp.Substring(1, tmp.Length - 2);

                    if (form_settings.rb_langEnglish.Checked)
                        return new AsmCommandArg("word [" + CorrectPtrExpression(arg, 8, tmp) + "]", true, false);
                    else
                        return new AsmCommandArg("слово [" + CorrectPtrExpression(arg, 8, tmp) + "]", true, false);
                } else if (tpos1 == -1 || tpos2 == -1) {
                    if (form_settings.rb_langEnglish.Checked)
                        return new AsmCommandArg("word [" + CorrectPtrExpression(arg, 7, tmp) + "]", true, false);
                    else
                        return new AsmCommandArg("слово [" + CorrectPtrExpression(arg, 7, tmp) + "]", true, false);
                } else
                    throw new Exception();
            } else {
                int tpos1 = tmp.LastIndexOf('['), tpos2 = tmp.IndexOf(']');
                if (tpos1 == 0 && tpos2 == tmp.Length - 1) {
                    tmp = tmp.Substring(1, tmp.Length - 2);
                    return new AsmCommandArg("[" + CorrectPtrExpression(arg, 1, tmp) + "]", true, false);
                } else if (tpos1 == -1 && tpos2 == -1) {
                    switch (tmp) {
                        case "AH":
                        case "AL":
                        case "BH":
                        case "BL":
                        case "CH":
                        case "CL":
                        case "DH":
                        case "DL":
                            if (form_settings.chbx_printPercent.Checked)
                                return new AsmCommandArg("%" + tmp, false, false);
                            else return new AsmCommandArg(tmp, false, false);
                        case "AX":
                        case "BX":
                        case "CX":
                        case "DX":
                        case "DI":
                        case "SI":
                        case "BP":
                        case "SP":
                        case "DS":
                        case "ES":
                        case "SS":
                        case "CS":
                            if (form_settings.chbx_printPercent.Checked)
                                return new AsmCommandArg("%" + tmp, true, false);
                            else return new AsmCommandArg(tmp, true, false);
                        default:
                            if (int.TryParse(tmp, out _)) {
                                tmp = tmp.TrimStart('0');
                                if (tmp.Length == 0) tmp = "0";
                                return new AsmCommandArg(tmp, true, true); //Double remove is optimal? May be...
                            } else if (int.TryParse(tmp.Remove(tmp.Length - 1), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out _)) {
                                tmp = tmp.Remove(tmp.Length - 1).TrimStart('0');
                                if (tmp.Length == 0) tmp = "0x0";
                                return new AsmCommandArg("0x" + tmp, true, true); //Double remove is optimal? May be...
                            } else
                                return new AsmCommandArg(arg.Substring(arg.ToUpper().IndexOf(tmp), tmp.Length), true, false);
                    }
                } else
                    throw new Exception();
            }
        }
        private string CorrectPtrExpression(string arg, ushort arg_index, string tmp) {
            short aindex = 0;
            foreach (Match item in Regex.Matches(tmp, @"[^\-()+*]+")) {
                switch (item.Value) {
                    case "AH":
                    case "AL":
                    case "BH":
                    case "BL":
                    case "CH":
                    case "CL":
                    case "DH":
                    case "DL":
                    case "AX":
                    case "BX":
                    case "CX":
                    case "DX":
                    case "DI":
                    case "SI":
                    case "BP":
                    case "SP":
                    case "DS":
                    case "ES":
                    case "SS":
                    case "CS":
                        if (form_settings.chbx_printPercent.Checked) {
                            tmp = tmp.Insert(item.Index + aindex, " %");
                            aindex += 2;
                        } else
                            tmp = tmp.Insert(item.Index + aindex++, " ");
                        tmp = tmp.Insert(item.Index + aindex++ + item.Length, " ");
                        break;
                    default:
                        string temp;
                        if (int.TryParse(item.Value, out _)) {
                            temp = item.Value.TrimStart('0');
                            if (temp.Length != 0) {
                                tmp = tmp.Remove(item.Index + aindex, item.Length).
                                    Insert(item.Index + aindex, ' ' + temp + ' ');
                                aindex -= (short)(item.Length - temp.Length - 2);
                            } else
                                throw new Exception();
                        } else if (int.TryParse(item.Value.Remove(item.Length - 1), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out _)) {
                            temp = item.Value.Remove(item.Length - 1).TrimStart('0');
                            if (temp.Length != 0) {
                                tmp = tmp.Remove(item.Index + aindex, item.Length).
                                    Insert(item.Index + aindex, " 0x" + temp + ' ');
                                aindex += (short)(temp.Length - item.Length + 4);
                            } else
                                throw new Exception();
                        } else {
                            tmp = tmp.Remove(item.Index + aindex, item.Length).
                                Insert(item.Index + aindex, ' ' + arg.Substring(arg_index + item.Index, item.Length) + ' ');
                            aindex += 2;
                        }
                        break;
                }
            }
            return tmp.Substring(1, tmp.Length - 2);
        }
    }
}
