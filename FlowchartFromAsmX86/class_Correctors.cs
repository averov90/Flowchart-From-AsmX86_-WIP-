using System;
using System.Windows.Forms;

namespace FlowchartFromAsmX86 {
    public partial class MainForm : Form {
        private bool Trim_unnecessary(ref string[] lines) {
            string[] spl;
            int i = 0, i1, ind;
            bool error = true;
            for (; i != lines.Length; ++i)
                if ((spl = lines[i].Trim().Split(' ')).Length == 2 && spl[1].ToLower() == "proc") {
                    current_name = spl[0];
                    error = false;
                    break;
                }
            if (error) return false;
            for (i1 = i, error = true; i1 != lines.Length; ++i1) {
                if ((spl = (lines[i1] = lines[i1].Trim()).Split(' ')).Length == 2 && spl[1].ToLower() == "endp") {
                    if (spl[0] != current_name) return false;
                    error = false;
                    break;
                } else if ((ind = lines[i1].IndexOf(';')) != -1)
                    lines[i1] = lines[i1].Substring(0, ind).TrimEnd();
            }
            if (error) return false;
            Array.Resize(ref lines, i1 - i + 1);
            return true;
        }
    }
}
