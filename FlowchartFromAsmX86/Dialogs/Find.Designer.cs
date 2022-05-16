
namespace FlowchartFromAsmX86 {
    partial class Find {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Find));
            this.tRow = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tCmd = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tDesc = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tConn = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tMCmd = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cReplaceCmd = new System.Windows.Forms.CheckBox();
            this.tNewCmd = new System.Windows.Forms.TextBox();
            this.tNewDesc = new System.Windows.Forms.TextBox();
            this.cReplaceDesc = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.cCmdC = new System.Windows.Forms.CheckBox();
            this.cDescC = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.cMCmdC = new System.Windows.Forms.CheckBox();
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.cCmdW = new System.Windows.Forms.CheckBox();
            this.cDescW = new System.Windows.Forms.CheckBox();
            this.checkBox11 = new System.Windows.Forms.CheckBox();
            this.cMCmdW = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.cWA = new System.Windows.Forms.CheckBox();
            this.tFigure = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tRow
            // 
            this.tRow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tRow.Location = new System.Drawing.Point(117, 32);
            this.tRow.Name = "tRow";
            this.tRow.Size = new System.Drawing.Size(115, 26);
            this.tRow.TabIndex = 0;
            this.toolTip1.SetToolTip(this.tRow, "Leave blank to skip.\r\nОставьте пустым для пропуска.");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Find by:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(75, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "row";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 19);
            this.label3.TabIndex = 4;
            this.label3.Text = "command";
            // 
            // tCmd
            // 
            this.tCmd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tCmd.Location = new System.Drawing.Point(117, 64);
            this.tCmd.Name = "tCmd";
            this.tCmd.Size = new System.Drawing.Size(115, 26);
            this.tCmd.TabIndex = 3;
            this.toolTip1.SetToolTip(this.tCmd, "Leave blank to skip.\r\nОставьте пустым для пропуска.\r\n");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 19);
            this.label4.TabIndex = 6;
            this.label4.Text = "description";
            // 
            // tDesc
            // 
            this.tDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tDesc.Location = new System.Drawing.Point(117, 96);
            this.tDesc.Name = "tDesc";
            this.tDesc.Size = new System.Drawing.Size(115, 26);
            this.tDesc.TabIndex = 5;
            this.toolTip1.SetToolTip(this.tDesc, "Leave blank to skip.\r\nОставьте пустым для пропуска.");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 163);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 19);
            this.label5.TabIndex = 8;
            this.label5.Text = "connection";
            // 
            // tConn
            // 
            this.tConn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tConn.Location = new System.Drawing.Point(117, 160);
            this.tConn.Name = "tConn";
            this.tConn.Size = new System.Drawing.Size(115, 26);
            this.tConn.TabIndex = 7;
            this.toolTip1.SetToolTip(this.tConn, "Leave blank to skip.\r\nОставьте пустым для пропуска.");
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(30, 195);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 19);
            this.label6.TabIndex = 10;
            this.label6.Text = "meta cmd";
            // 
            // tMCmd
            // 
            this.tMCmd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tMCmd.Location = new System.Drawing.Point(117, 192);
            this.tMCmd.Name = "tMCmd";
            this.tMCmd.Size = new System.Drawing.Size(115, 26);
            this.tMCmd.TabIndex = 9;
            this.toolTip1.SetToolTip(this.tMCmd, "Leave blank to skip.\r\nОставьте пустым для пропуска.");
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(99, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(22, 24);
            this.label7.TabIndex = 11;
            this.label7.Text = "+";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(99, 81);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(22, 24);
            this.label8.TabIndex = 12;
            this.label8.Text = "+";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(99, 146);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(22, 24);
            this.label9.TabIndex = 13;
            this.label9.Text = "+";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(99, 177);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(22, 24);
            this.label10.TabIndex = 14;
            this.label10.Text = "+";
            // 
            // cReplaceCmd
            // 
            this.cReplaceCmd.AutoSize = true;
            this.cReplaceCmd.Location = new System.Drawing.Point(12, 317);
            this.cReplaceCmd.Name = "cReplaceCmd";
            this.cReplaceCmd.Size = new System.Drawing.Size(208, 23);
            this.cReplaceCmd.TabIndex = 15;
            this.cReplaceCmd.Text = "Replace command with";
            this.cReplaceCmd.UseVisualStyleBackColor = true;
            this.cReplaceCmd.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // tNewCmd
            // 
            this.tNewCmd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tNewCmd.Enabled = false;
            this.tNewCmd.Location = new System.Drawing.Point(34, 346);
            this.tNewCmd.Name = "tNewCmd";
            this.tNewCmd.Size = new System.Drawing.Size(298, 26);
            this.tNewCmd.TabIndex = 16;
            // 
            // tNewDesc
            // 
            this.tNewDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tNewDesc.Enabled = false;
            this.tNewDesc.Location = new System.Drawing.Point(34, 417);
            this.tNewDesc.Multiline = true;
            this.tNewDesc.Name = "tNewDesc";
            this.tNewDesc.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tNewDesc.Size = new System.Drawing.Size(298, 68);
            this.tNewDesc.TabIndex = 18;
            this.tNewDesc.WordWrap = false;
            // 
            // cReplaceDesc
            // 
            this.cReplaceDesc.AutoSize = true;
            this.cReplaceDesc.Location = new System.Drawing.Point(12, 388);
            this.cReplaceDesc.Name = "cReplaceDesc";
            this.cReplaceDesc.Size = new System.Drawing.Size(244, 23);
            this.cReplaceDesc.TabIndex = 17;
            this.cReplaceDesc.Text = "Replace description with";
            this.cReplaceDesc.UseVisualStyleBackColor = true;
            this.cReplaceDesc.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox3.AutoSize = true;
            this.checkBox3.Enabled = false;
            this.checkBox3.Location = new System.Drawing.Point(238, 35);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(46, 23);
            this.checkBox3.TabIndex = 19;
            this.checkBox3.Text = "Mc";
            this.toolTip1.SetToolTip(this.checkBox3, "Math case (Учитывать регистр)");
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // cCmdC
            // 
            this.cCmdC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cCmdC.AutoSize = true;
            this.cCmdC.Location = new System.Drawing.Point(238, 66);
            this.cCmdC.Name = "cCmdC";
            this.cCmdC.Size = new System.Drawing.Size(46, 23);
            this.cCmdC.TabIndex = 20;
            this.cCmdC.Text = "Mc";
            this.toolTip1.SetToolTip(this.cCmdC, "Math case (Учитывать регистр)");
            this.cCmdC.UseVisualStyleBackColor = true;
            // 
            // cDescC
            // 
            this.cDescC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cDescC.AutoSize = true;
            this.cDescC.Location = new System.Drawing.Point(238, 98);
            this.cDescC.Name = "cDescC";
            this.cDescC.Size = new System.Drawing.Size(46, 23);
            this.cDescC.TabIndex = 21;
            this.cDescC.Text = "Mc";
            this.toolTip1.SetToolTip(this.cDescC, "Math case (Учитывать регистр)");
            this.cDescC.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            this.checkBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox6.AutoSize = true;
            this.checkBox6.Enabled = false;
            this.checkBox6.Location = new System.Drawing.Point(238, 162);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(46, 23);
            this.checkBox6.TabIndex = 22;
            this.checkBox6.Text = "Mc";
            this.toolTip1.SetToolTip(this.checkBox6, "Math case (Учитывать регистр)");
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // cMCmdC
            // 
            this.cMCmdC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cMCmdC.AutoSize = true;
            this.cMCmdC.Location = new System.Drawing.Point(238, 194);
            this.cMCmdC.Name = "cMCmdC";
            this.cMCmdC.Size = new System.Drawing.Size(46, 23);
            this.cMCmdC.TabIndex = 23;
            this.cMCmdC.Text = "Mc";
            this.toolTip1.SetToolTip(this.cMCmdC, "Math case (Учитывать регистр)");
            this.cMCmdC.UseVisualStyleBackColor = true;
            // 
            // checkBox8
            // 
            this.checkBox8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox8.AutoSize = true;
            this.checkBox8.Checked = true;
            this.checkBox8.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox8.Enabled = false;
            this.checkBox8.Location = new System.Drawing.Point(290, 35);
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.Size = new System.Drawing.Size(46, 23);
            this.checkBox8.TabIndex = 24;
            this.checkBox8.Text = "Ww";
            this.toolTip1.SetToolTip(this.checkBox8, "Math whole word (Учитывать слово целиком)");
            this.checkBox8.UseVisualStyleBackColor = true;
            // 
            // cCmdW
            // 
            this.cCmdW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cCmdW.AutoSize = true;
            this.cCmdW.Location = new System.Drawing.Point(290, 66);
            this.cCmdW.Name = "cCmdW";
            this.cCmdW.Size = new System.Drawing.Size(46, 23);
            this.cCmdW.TabIndex = 25;
            this.cCmdW.Text = "Ww";
            this.toolTip1.SetToolTip(this.cCmdW, "Math whole word (Учитывать слово целиком)");
            this.cCmdW.UseVisualStyleBackColor = true;
            // 
            // cDescW
            // 
            this.cDescW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cDescW.AutoSize = true;
            this.cDescW.Location = new System.Drawing.Point(290, 98);
            this.cDescW.Name = "cDescW";
            this.cDescW.Size = new System.Drawing.Size(46, 23);
            this.cDescW.TabIndex = 26;
            this.cDescW.Text = "Ww";
            this.toolTip1.SetToolTip(this.cDescW, "Math whole word (Учитывать слово целиком)");
            this.cDescW.UseVisualStyleBackColor = true;
            // 
            // checkBox11
            // 
            this.checkBox11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox11.AutoSize = true;
            this.checkBox11.Checked = true;
            this.checkBox11.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox11.Enabled = false;
            this.checkBox11.Location = new System.Drawing.Point(290, 162);
            this.checkBox11.Name = "checkBox11";
            this.checkBox11.Size = new System.Drawing.Size(46, 23);
            this.checkBox11.TabIndex = 27;
            this.checkBox11.Text = "Ww";
            this.toolTip1.SetToolTip(this.checkBox11, "Math whole word (Учитывать слово целиком)");
            this.checkBox11.UseVisualStyleBackColor = true;
            // 
            // cMCmdW
            // 
            this.cMCmdW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cMCmdW.AutoSize = true;
            this.cMCmdW.Location = new System.Drawing.Point(290, 194);
            this.cMCmdW.Name = "cMCmdW";
            this.cMCmdW.Size = new System.Drawing.Size(46, 23);
            this.cMCmdW.TabIndex = 28;
            this.cMCmdW.Text = "Ww";
            this.toolTip1.SetToolTip(this.cMCmdW, "Math whole word (Учитывать слово целиком)");
            this.cMCmdW.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(212, 224);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 30);
            this.button1.TabIndex = 29;
            this.button1.Text = "Find first";
            this.toolTip1.SetToolTip(this.button1, "Find the first occurrence from the beginning.\r\n\r\nНайти первое вхождение с самого " +
        "начала.");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(212, 260);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 30);
            this.button2.TabIndex = 31;
            this.button2.Text = "Find next";
            this.toolTip1.SetToolTip(this.button2, "Find the next occurrence from the current position.\r\n\r\nНайти следующее вхождение " +
        "из текущей позиции.");
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(80, 501);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(126, 30);
            this.button3.TabIndex = 32;
            this.button3.Text = "Replace next";
            this.toolTip1.SetToolTip(this.button3, "Replace the next occurrence from the current position.\r\n\r\nЗаменить следующее вхож" +
        "дение с текущей позиции.");
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Enabled = false;
            this.button4.Location = new System.Drawing.Point(212, 501);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(120, 30);
            this.button4.TabIndex = 33;
            this.button4.Text = "Replace all";
            this.toolTip1.SetToolTip(this.button4, "Replace the all occurrences.\r\n\r\nЗаменить все вхождения.\r\n");
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(12, 270);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(64, 30);
            this.button5.TabIndex = 34;
            this.button5.Text = "Clear";
            this.toolTip1.SetToolTip(this.button5, "Find the next occurrence from the current position.\r\n\r\nНайти следующее вхождение " +
        "из текущей позиции.");
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(290, 130);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(46, 23);
            this.checkBox1.TabIndex = 38;
            this.checkBox1.Text = "Ww";
            this.toolTip1.SetToolTip(this.checkBox1, "Math whole word (Учитывать слово целиком)");
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox2.AutoSize = true;
            this.checkBox2.Enabled = false;
            this.checkBox2.Location = new System.Drawing.Point(238, 130);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(46, 23);
            this.checkBox2.TabIndex = 37;
            this.checkBox2.Text = "Mc";
            this.toolTip1.SetToolTip(this.checkBox2, "Math case (Учитывать регистр)");
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // cWA
            // 
            this.cWA.AutoSize = true;
            this.cWA.Checked = true;
            this.cWA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cWA.Location = new System.Drawing.Point(12, 229);
            this.cWA.Name = "cWA";
            this.cWA.Size = new System.Drawing.Size(127, 23);
            this.cWA.TabIndex = 30;
            this.cWA.Text = "Wrap around";
            this.toolTip1.SetToolTip(this.cWA, resources.GetString("cWA.ToolTip"));
            this.cWA.UseVisualStyleBackColor = true;
            // 
            // tFigure
            // 
            this.tFigure.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tFigure.Location = new System.Drawing.Point(117, 128);
            this.tFigure.Name = "tFigure";
            this.tFigure.Size = new System.Drawing.Size(115, 26);
            this.tFigure.TabIndex = 35;
            this.toolTip1.SetToolTip(this.tFigure, "Leave blank to skip.\r\nОставьте пустым для пропуска.");
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(48, 131);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(63, 19);
            this.label11.TabIndex = 36;
            this.label11.Text = "figure";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label12.Location = new System.Drawing.Point(99, 112);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(22, 24);
            this.label12.TabIndex = 39;
            this.label12.Text = "+";
            // 
            // Find
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 543);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.tFigure);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.cWA);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cMCmdW);
            this.Controls.Add(this.checkBox11);
            this.Controls.Add(this.cDescW);
            this.Controls.Add(this.cCmdW);
            this.Controls.Add(this.checkBox8);
            this.Controls.Add(this.cMCmdC);
            this.Controls.Add(this.checkBox6);
            this.Controls.Add(this.cDescC);
            this.Controls.Add(this.cCmdC);
            this.Controls.Add(this.tMCmd);
            this.Controls.Add(this.tConn);
            this.Controls.Add(this.tDesc);
            this.Controls.Add(this.tCmd);
            this.Controls.Add(this.tRow);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.tNewDesc);
            this.Controls.Add(this.cReplaceDesc);
            this.Controls.Add(this.tNewCmd);
            this.Controls.Add(this.cReplaceCmd);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label12);
            this.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(340, 560);
            this.Name = "Find";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Find";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Find_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tRow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tCmd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tDesc;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tConn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tMCmd;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox cReplaceCmd;
        private System.Windows.Forms.TextBox tNewCmd;
        private System.Windows.Forms.TextBox tNewDesc;
        private System.Windows.Forms.CheckBox cReplaceDesc;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox cCmdC;
        private System.Windows.Forms.CheckBox cDescC;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.CheckBox cMCmdC;
        private System.Windows.Forms.CheckBox checkBox8;
        private System.Windows.Forms.CheckBox cCmdW;
        private System.Windows.Forms.CheckBox cDescW;
        private System.Windows.Forms.CheckBox checkBox11;
        private System.Windows.Forms.CheckBox cMCmdW;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox cWA;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.TextBox tFigure;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
    }
}