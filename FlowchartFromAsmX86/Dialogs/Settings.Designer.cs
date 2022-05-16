namespace FlowchartFromAsmX86 {
    partial class Settings {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.label1 = new System.Windows.Forms.Label();
            this.nmb_countSizes = new System.Windows.Forms.NumericUpDown();
            this.btn_default = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.nmb_ratioBlock = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nmb_ratioCanvas = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nmb_ratioArea = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.rb_langEnglish = new System.Windows.Forms.RadioButton();
            this.rb_langRussian = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.nmb_lengthLine = new System.Windows.Forms.NumericUpDown();
            this.chbx_printPercent = new System.Windows.Forms.CheckBox();
            this.chbx_allowFontFl = new System.Windows.Forms.CheckBox();
            this.chbx_allowWayUp = new System.Windows.Forms.CheckBox();
            this.chbx_autoRatioArea = new System.Windows.Forms.CheckBox();
            this.chbx_useHexagons = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tTip = new System.Windows.Forms.ToolTip(this.components);
            this.nmb_startNum = new System.Windows.Forms.NumericUpDown();
            this.chbx_allowCrossColumn = new System.Windows.Forms.CheckBox();
            this.chbx_allowLeftVia = new System.Windows.Forms.CheckBox();
            this.nmb_linesInBetw = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.cbx_detailed = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbx_tmode = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.chbx_allowBetwVia = new System.Windows.Forms.CheckBox();
            this.cbx_adj = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.nmb_strDiffThr = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.cbx_gridSzMode = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.nmb_lineSpacing = new System.Windows.Forms.NumericUpDown();
            this.nmb_fontFlLimit = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.cbx_loopSize = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.nmb_lineWidth = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dialog_font = new System.Windows.Forms.FontDialog();
            this.chbx_blockedit = new System.Windows.Forms.CheckBox();
            this.chbx_saveSettings = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nmb_countSizes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmb_ratioBlock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmb_ratioCanvas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmb_ratioArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmb_lengthLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmb_startNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmb_linesInBetw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmb_strDiffThr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmb_lineSpacing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmb_fontFlLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmb_lineWidth)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 277);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(234, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cnt of avail block sizes:";
            this.tTip.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // nmb_countSizes
            // 
            this.nmb_countSizes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nmb_countSizes.Location = new System.Drawing.Point(246, 275);
            this.nmb_countSizes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmb_countSizes.Name = "nmb_countSizes";
            this.nmb_countSizes.Size = new System.Drawing.Size(52, 26);
            this.nmb_countSizes.TabIndex = 1;
            this.nmb_countSizes.TabStop = false;
            this.tTip.SetToolTip(this.nmb_countSizes, resources.GetString("nmb_countSizes.ToolTip"));
            this.nmb_countSizes.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // btn_default
            // 
            this.btn_default.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_default.Location = new System.Drawing.Point(13, 451);
            this.btn_default.Name = "btn_default";
            this.btn_default.Size = new System.Drawing.Size(80, 29);
            this.btn_default.TabIndex = 2;
            this.btn_default.TabStop = false;
            this.btn_default.Text = "Default";
            this.btn_default.UseVisualStyleBackColor = true;
            this.btn_default.Click += new System.EventHandler(this.btn_default_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(616, 451);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 29);
            this.button2.TabIndex = 0;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3.Location = new System.Drawing.Point(530, 451);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(80, 29);
            this.button3.TabIndex = 4;
            this.button3.TabStop = false;
            this.button3.Text = "Cancel";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(261, 19);
            this.label2.TabIndex = 5;
            this.label2.Text = "Aspect ratio of block (W/H):";
            this.tTip.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
            // 
            // nmb_ratioBlock
            // 
            this.nmb_ratioBlock.DecimalPlaces = 3;
            this.nmb_ratioBlock.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.nmb_ratioBlock.Location = new System.Drawing.Point(278, 26);
            this.nmb_ratioBlock.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmb_ratioBlock.Name = "nmb_ratioBlock";
            this.nmb_ratioBlock.Size = new System.Drawing.Size(85, 26);
            this.nmb_ratioBlock.TabIndex = 9;
            this.nmb_ratioBlock.TabStop = false;
            this.tTip.SetToolTip(this.nmb_ratioBlock, resources.GetString("nmb_ratioBlock.ToolTip"));
            this.nmb_ratioBlock.Value = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(270, 19);
            this.label3.TabIndex = 10;
            this.label3.Text = "Aspect ratio of canvas (W/H):";
            this.tTip.SetToolTip(this.label3, resources.GetString("label3.ToolTip"));
            // 
            // nmb_ratioCanvas
            // 
            this.nmb_ratioCanvas.DecimalPlaces = 3;
            this.nmb_ratioCanvas.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.nmb_ratioCanvas.Location = new System.Drawing.Point(278, 59);
            this.nmb_ratioCanvas.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.nmb_ratioCanvas.Name = "nmb_ratioCanvas";
            this.nmb_ratioCanvas.Size = new System.Drawing.Size(85, 26);
            this.nmb_ratioCanvas.TabIndex = 11;
            this.nmb_ratioCanvas.TabStop = false;
            this.tTip.SetToolTip(this.nmb_ratioCanvas, resources.GetString("nmb_ratioCanvas.ToolTip"));
            this.nmb_ratioCanvas.Value = new decimal(new int[] {
            65,
            0,
            0,
            131072});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(198, 19);
            this.label4.TabIndex = 12;
            this.label4.Text = "Area ratio (canv/bl):";
            this.tTip.SetToolTip(this.label4, resources.GetString("label4.ToolTip"));
            // 
            // nmb_ratioArea
            // 
            this.nmb_ratioArea.Location = new System.Drawing.Point(208, 91);
            this.nmb_ratioArea.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nmb_ratioArea.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmb_ratioArea.Name = "nmb_ratioArea";
            this.nmb_ratioArea.Size = new System.Drawing.Size(85, 26);
            this.nmb_ratioArea.TabIndex = 13;
            this.nmb_ratioArea.TabStop = false;
            this.tTip.SetToolTip(this.nmb_ratioArea, resources.GetString("nmb_ratioArea.ToolTip"));
            this.nmb_ratioArea.Value = new decimal(new int[] {
            182,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 297);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 19);
            this.label5.TabIndex = 15;
            this.label5.Text = "Language set:";
            this.tTip.SetToolTip(this.label5, "The language of the text in the blocks and the version of the standards.\r\n\r\nЯзык " +
        "текста в блоках и вариант стандартов.");
            // 
            // rb_langEnglish
            // 
            this.rb_langEnglish.AutoSize = true;
            this.rb_langEnglish.Location = new System.Drawing.Point(138, 295);
            this.rb_langEnglish.Name = "rb_langEnglish";
            this.rb_langEnglish.Size = new System.Drawing.Size(90, 23);
            this.rb_langEnglish.TabIndex = 16;
            this.rb_langEnglish.Text = "English";
            this.tTip.SetToolTip(this.rb_langEnglish, "American text and standards.");
            this.rb_langEnglish.UseVisualStyleBackColor = true;
            this.rb_langEnglish.CheckedChanged += new System.EventHandler(this.rb_langEnglish_CheckedChanged);
            // 
            // rb_langRussian
            // 
            this.rb_langRussian.AutoSize = true;
            this.rb_langRussian.Checked = true;
            this.rb_langRussian.Location = new System.Drawing.Point(242, 295);
            this.rb_langRussian.Name = "rb_langRussian";
            this.rb_langRussian.Size = new System.Drawing.Size(90, 23);
            this.rb_langRussian.TabIndex = 17;
            this.rb_langRussian.TabStop = true;
            this.rb_langRussian.Text = "Russian";
            this.tTip.SetToolTip(this.rb_langRussian, "Русский текст и стандарты.");
            this.rb_langRussian.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 125);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(279, 19);
            this.label6.TabIndex = 18;
            this.label6.Text = "Connection line length (frc.):";
            this.tTip.SetToolTip(this.label6, resources.GetString("label6.ToolTip"));
            // 
            // nmb_lengthLine
            // 
            this.nmb_lengthLine.DecimalPlaces = 2;
            this.nmb_lengthLine.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.nmb_lengthLine.Location = new System.Drawing.Point(278, 123);
            this.nmb_lengthLine.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nmb_lengthLine.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.nmb_lengthLine.Name = "nmb_lengthLine";
            this.nmb_lengthLine.Size = new System.Drawing.Size(85, 26);
            this.nmb_lengthLine.TabIndex = 19;
            this.nmb_lengthLine.TabStop = false;
            this.tTip.SetToolTip(this.nmb_lengthLine, resources.GetString("nmb_lengthLine.ToolTip"));
            this.nmb_lengthLine.Value = new decimal(new int[] {
            75,
            0,
            0,
            131072});
            // 
            // chbx_printPercent
            // 
            this.chbx_printPercent.AutoSize = true;
            this.chbx_printPercent.Location = new System.Drawing.Point(6, 242);
            this.chbx_printPercent.Name = "chbx_printPercent";
            this.chbx_printPercent.Size = new System.Drawing.Size(361, 23);
            this.chbx_printPercent.TabIndex = 20;
            this.chbx_printPercent.TabStop = false;
            this.chbx_printPercent.Text = "Add \"%\" char before the register name";
            this.tTip.SetToolTip(this.chbx_printPercent, "Add the \"%\" character before the register names.\r\n\r\nДобавлять символ \"%\" перед им" +
        "енами регистров.");
            this.chbx_printPercent.UseVisualStyleBackColor = true;
            // 
            // chbx_allowFontFl
            // 
            this.chbx_allowFontFl.AutoSize = true;
            this.chbx_allowFontFl.Checked = true;
            this.chbx_allowFontFl.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbx_allowFontFl.Location = new System.Drawing.Point(7, 187);
            this.chbx_allowFontFl.Name = "chbx_allowFontFl";
            this.chbx_allowFontFl.Size = new System.Drawing.Size(280, 23);
            this.chbx_allowFontFl.TabIndex = 21;
            this.chbx_allowFontFl.TabStop = false;
            this.chbx_allowFontFl.Text = "Allow font size fluctuations";
            this.tTip.SetToolTip(this.chbx_allowFontFl, resources.GetString("chbx_allowFontFl.ToolTip"));
            this.chbx_allowFontFl.UseVisualStyleBackColor = true;
            this.chbx_allowFontFl.CheckedChanged += new System.EventHandler(this.chbx_allowFontFl_CheckedChanged);
            // 
            // chbx_allowWayUp
            // 
            this.chbx_allowWayUp.AutoSize = true;
            this.chbx_allowWayUp.Checked = true;
            this.chbx_allowWayUp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbx_allowWayUp.Location = new System.Drawing.Point(6, 94);
            this.chbx_allowWayUp.Name = "chbx_allowWayUp";
            this.chbx_allowWayUp.Size = new System.Drawing.Size(298, 23);
            this.chbx_allowWayUp.TabIndex = 22;
            this.chbx_allowWayUp.TabStop = false;
            this.chbx_allowWayUp.Text = "Allow up-going connecting line";
            this.tTip.SetToolTip(this.chbx_allowWayUp, resources.GetString("chbx_allowWayUp.ToolTip"));
            this.chbx_allowWayUp.UseVisualStyleBackColor = true;
            // 
            // chbx_autoRatioArea
            // 
            this.chbx_autoRatioArea.AutoSize = true;
            this.chbx_autoRatioArea.Location = new System.Drawing.Point(299, 92);
            this.chbx_autoRatioArea.Name = "chbx_autoRatioArea";
            this.chbx_autoRatioArea.Size = new System.Drawing.Size(64, 23);
            this.chbx_autoRatioArea.TabIndex = 23;
            this.chbx_autoRatioArea.TabStop = false;
            this.chbx_autoRatioArea.Text = "Auto";
            this.tTip.SetToolTip(this.chbx_autoRatioArea, resources.GetString("chbx_autoRatioArea.ToolTip"));
            this.chbx_autoRatioArea.UseVisualStyleBackColor = true;
            this.chbx_autoRatioArea.CheckedChanged += new System.EventHandler(this.chbx_autoRatioArea_CheckedChanged);
            // 
            // chbx_useHexagons
            // 
            this.chbx_useHexagons.AutoSize = true;
            this.chbx_useHexagons.Location = new System.Drawing.Point(6, 271);
            this.chbx_useHexagons.Name = "chbx_useHexagons";
            this.chbx_useHexagons.Size = new System.Drawing.Size(370, 23);
            this.chbx_useHexagons.TabIndex = 25;
            this.chbx_useHexagons.TabStop = false;
            this.chbx_useHexagons.Text = "Use hexagons (requires manual editing)";
            this.tTip.SetToolTip(this.chbx_useHexagons, resources.GetString("chbx_useHexagons.ToolTip"));
            this.chbx_useHexagons.UseVisualStyleBackColor = true;
            this.chbx_useHexagons.CheckedChanged += new System.EventHandler(this.chbx_useHexagons_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 157);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(135, 19);
            this.label7.TabIndex = 26;
            this.label7.Text = "Start number: ";
            this.tTip.SetToolTip(this.label7, resources.GetString("label7.ToolTip"));
            // 
            // tTip
            // 
            this.tTip.AutoPopDelay = 50000;
            this.tTip.InitialDelay = 250;
            this.tTip.IsBalloon = true;
            this.tTip.ReshowDelay = 100;
            this.tTip.UseFading = false;
            // 
            // nmb_startNum
            // 
            this.nmb_startNum.Location = new System.Drawing.Point(135, 155);
            this.nmb_startNum.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nmb_startNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmb_startNum.Name = "nmb_startNum";
            this.nmb_startNum.Size = new System.Drawing.Size(90, 26);
            this.nmb_startNum.TabIndex = 27;
            this.nmb_startNum.TabStop = false;
            this.tTip.SetToolTip(this.nmb_startNum, resources.GetString("nmb_startNum.ToolTip"));
            this.nmb_startNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chbx_allowCrossColumn
            // 
            this.chbx_allowCrossColumn.AutoSize = true;
            this.chbx_allowCrossColumn.Checked = true;
            this.chbx_allowCrossColumn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbx_allowCrossColumn.Location = new System.Drawing.Point(6, 123);
            this.chbx_allowCrossColumn.Name = "chbx_allowCrossColumn";
            this.chbx_allowCrossColumn.Size = new System.Drawing.Size(298, 23);
            this.chbx_allowCrossColumn.TabIndex = 23;
            this.chbx_allowCrossColumn.TabStop = false;
            this.chbx_allowCrossColumn.Text = "Allow cross-column connections";
            this.tTip.SetToolTip(this.chbx_allowCrossColumn, resources.GetString("chbx_allowCrossColumn.ToolTip"));
            this.chbx_allowCrossColumn.UseVisualStyleBackColor = true;
            // 
            // chbx_allowLeftVia
            // 
            this.chbx_allowLeftVia.AutoSize = true;
            this.chbx_allowLeftVia.Checked = true;
            this.chbx_allowLeftVia.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbx_allowLeftVia.Location = new System.Drawing.Point(6, 152);
            this.chbx_allowLeftVia.Name = "chbx_allowLeftVia";
            this.chbx_allowLeftVia.Size = new System.Drawing.Size(199, 23);
            this.chbx_allowLeftVia.TabIndex = 24;
            this.chbx_allowLeftVia.TabStop = false;
            this.chbx_allowLeftVia.Text = "Allow left-side via";
            this.tTip.SetToolTip(this.chbx_allowLeftVia, "Allow the use of connecting circles to the left of the block or line to make the " +
        "connection.\r\n\r\nРазрешить использовать соединительные круги слева от блока или ли" +
        "нии для выполнения соединения.");
            this.chbx_allowLeftVia.UseVisualStyleBackColor = true;
            // 
            // nmb_linesInBetw
            // 
            this.nmb_linesInBetw.Location = new System.Drawing.Point(238, 210);
            this.nmb_linesInBetw.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nmb_linesInBetw.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmb_linesInBetw.Name = "nmb_linesInBetw";
            this.nmb_linesInBetw.Size = new System.Drawing.Size(52, 26);
            this.nmb_linesInBetw.TabIndex = 27;
            this.nmb_linesInBetw.TabStop = false;
            this.tTip.SetToolTip(this.nmb_linesInBetw, resources.GetString("nmb_linesInBetw.ToolTip"));
            this.nmb_linesInBetw.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 327);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(135, 19);
            this.label11.TabIndex = 28;
            this.label11.Text = "Details level:";
            this.tTip.SetToolTip(this.label11, resources.GetString("label11.ToolTip"));
            // 
            // cbx_detailed
            // 
            this.cbx_detailed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_detailed.FormattingEnabled = true;
            this.cbx_detailed.Items.AddRange(new object[] {
            "No long rows",
            "Allow long rows",
            "Detailed"});
            this.cbx_detailed.Location = new System.Drawing.Point(147, 324);
            this.cbx_detailed.Name = "cbx_detailed";
            this.cbx_detailed.Size = new System.Drawing.Size(173, 27);
            this.cbx_detailed.TabIndex = 30;
            this.tTip.SetToolTip(this.cbx_detailed, resources.GetString("cbx_detailed.ToolTip"));
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 31);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(126, 19);
            this.label8.TabIndex = 1;
            this.label8.Text = "Tracing mode:";
            this.tTip.SetToolTip(this.label8, resources.GetString("label8.ToolTip"));
            // 
            // cbx_tmode
            // 
            this.cbx_tmode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_tmode.FormattingEnabled = true;
            this.cbx_tmode.Items.AddRange(new object[] {
            "Quick",
            "Accurate"});
            this.cbx_tmode.Location = new System.Drawing.Point(139, 28);
            this.cbx_tmode.Name = "cbx_tmode";
            this.cbx_tmode.Size = new System.Drawing.Size(96, 27);
            this.cbx_tmode.TabIndex = 0;
            this.tTip.SetToolTip(this.cbx_tmode, resources.GetString("cbx_tmode.ToolTip"));
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(129, 355);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(119, 27);
            this.button1.TabIndex = 32;
            this.button1.TabStop = false;
            this.button1.Text = "Select font";
            this.tTip.SetToolTip(this.button1, "Change the font of the chart.\r\n\r\nИзменить шрифт диаграммы.");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 212);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(225, 19);
            this.label10.TabIndex = 26;
            this.label10.Text = "Cnt of lines in \"betw.\":";
            this.tTip.SetToolTip(this.label10, resources.GetString("label10.ToolTip"));
            // 
            // chbx_allowBetwVia
            // 
            this.chbx_allowBetwVia.AutoSize = true;
            this.chbx_allowBetwVia.Checked = true;
            this.chbx_allowBetwVia.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbx_allowBetwVia.Location = new System.Drawing.Point(6, 182);
            this.chbx_allowBetwVia.Name = "chbx_allowBetwVia";
            this.chbx_allowBetwVia.Size = new System.Drawing.Size(262, 23);
            this.chbx_allowBetwVia.TabIndex = 25;
            this.chbx_allowBetwVia.Text = "Try to place via \"between\"";
            this.tTip.SetToolTip(this.chbx_allowBetwVia, resources.GetString("chbx_allowBetwVia.ToolTip"));
            this.chbx_allowBetwVia.UseVisualStyleBackColor = true;
            // 
            // cbx_adj
            // 
            this.cbx_adj.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_adj.FormattingEnabled = true;
            this.cbx_adj.Items.AddRange(new object[] {
            "Flowchart",
            "Full canvas"});
            this.cbx_adj.Location = new System.Drawing.Point(121, 61);
            this.cbx_adj.Name = "cbx_adj";
            this.cbx_adj.Size = new System.Drawing.Size(125, 27);
            this.cbx_adj.TabIndex = 3;
            this.tTip.SetToolTip(this.cbx_adj, resources.GetString("cbx_adj.ToolTip"));
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 64);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(108, 19);
            this.label9.TabIndex = 2;
            this.label9.Text = "Adjustment:";
            this.tTip.SetToolTip(this.label9, resources.GetString("label9.ToolTip"));
            // 
            // nmb_strDiffThr
            // 
            this.nmb_strDiffThr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nmb_strDiffThr.DecimalPlaces = 1;
            this.nmb_strDiffThr.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.nmb_strDiffThr.Location = new System.Drawing.Point(222, 340);
            this.nmb_strDiffThr.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nmb_strDiffThr.Name = "nmb_strDiffThr";
            this.nmb_strDiffThr.Size = new System.Drawing.Size(60, 26);
            this.nmb_strDiffThr.TabIndex = 32;
            this.nmb_strDiffThr.TabStop = false;
            this.tTip.SetToolTip(this.nmb_strDiffThr, resources.GetString("nmb_strDiffThr.ToolTip"));
            this.nmb_strDiffThr.Value = new decimal(new int[] {
            92,
            0,
            0,
            0});
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(9, 342);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(207, 19);
            this.label15.TabIndex = 31;
            this.label15.Text = "String diff threshold:";
            this.tTip.SetToolTip(this.label15, resources.GetString("label15.ToolTip"));
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(284, 342);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(18, 19);
            this.label16.TabIndex = 33;
            this.label16.Text = "%";
            this.tTip.SetToolTip(this.label16, resources.GetString("label16.ToolTip"));
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(8, 375);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(162, 19);
            this.label17.TabIndex = 34;
            this.label17.Text = "Grid sizing mode:";
            this.tTip.SetToolTip(this.label17, resources.GetString("label17.ToolTip"));
            // 
            // cbx_gridSzMode
            // 
            this.cbx_gridSzMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbx_gridSzMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_gridSzMode.FormattingEnabled = true;
            this.cbx_gridSzMode.Items.AddRange(new object[] {
            "Sparsed",
            "Inscribed",
            "Ceilinged"});
            this.cbx_gridSzMode.Location = new System.Drawing.Point(176, 372);
            this.cbx_gridSzMode.Name = "cbx_gridSzMode";
            this.cbx_gridSzMode.Size = new System.Drawing.Size(126, 27);
            this.cbx_gridSzMode.TabIndex = 31;
            this.tTip.SetToolTip(this.cbx_gridSzMode, resources.GetString("cbx_gridSzMode.ToolTip"));
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 390);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(126, 19);
            this.label12.TabIndex = 33;
            this.label12.Text = "Line spacing:";
            this.tTip.SetToolTip(this.label12, resources.GetString("label12.ToolTip"));
            // 
            // nmb_lineSpacing
            // 
            this.nmb_lineSpacing.DecimalPlaces = 2;
            this.nmb_lineSpacing.Increment = new decimal(new int[] {
            15,
            0,
            0,
            131072});
            this.nmb_lineSpacing.Location = new System.Drawing.Point(138, 388);
            this.nmb_lineSpacing.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nmb_lineSpacing.Minimum = new decimal(new int[] {
            11,
            0,
            0,
            65536});
            this.nmb_lineSpacing.Name = "nmb_lineSpacing";
            this.nmb_lineSpacing.Size = new System.Drawing.Size(110, 26);
            this.nmb_lineSpacing.TabIndex = 34;
            this.nmb_lineSpacing.TabStop = false;
            this.tTip.SetToolTip(this.nmb_lineSpacing, resources.GetString("nmb_lineSpacing.ToolTip"));
            this.nmb_lineSpacing.Value = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            // 
            // nmb_fontFlLimit
            // 
            this.nmb_fontFlLimit.DecimalPlaces = 1;
            this.nmb_fontFlLimit.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nmb_fontFlLimit.Location = new System.Drawing.Point(253, 210);
            this.nmb_fontFlLimit.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nmb_fontFlLimit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nmb_fontFlLimit.Name = "nmb_fontFlLimit";
            this.nmb_fontFlLimit.Size = new System.Drawing.Size(110, 26);
            this.nmb_fontFlLimit.TabIndex = 36;
            this.nmb_fontFlLimit.TabStop = false;
            this.tTip.SetToolTip(this.nmb_fontFlLimit, resources.GetString("nmb_fontFlLimit.ToolTip"));
            this.nmb_fontFlLimit.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(158, 212);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(90, 19);
            this.label13.TabIndex = 37;
            this.label13.Text = "Limit to:";
            this.tTip.SetToolTip(this.label13, resources.GetString("label13.ToolTip"));
            // 
            // cbx_loopSize
            // 
            this.cbx_loopSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbx_loopSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_loopSize.FormattingEnabled = true;
            this.cbx_loopSize.Items.AddRange(new object[] {
            "Common",
            "Group letter",
            "Group type"});
            this.cbx_loopSize.Location = new System.Drawing.Point(157, 307);
            this.cbx_loopSize.Name = "cbx_loopSize";
            this.cbx_loopSize.Size = new System.Drawing.Size(141, 27);
            this.cbx_loopSize.TabIndex = 32;
            this.tTip.SetToolTip(this.cbx_loopSize, resources.GetString("cbx_loopSize.ToolTip"));
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(7, 310);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(144, 19);
            this.label18.TabIndex = 35;
            this.label18.Text = "Loop size mode:";
            this.tTip.SetToolTip(this.label18, resources.GetString("label18.ToolTip"));
            // 
            // label19
            // 
            this.label19.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(7, 244);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(216, 19);
            this.label19.TabIndex = 32;
            this.label19.Text = "Connect line thickness:";
            this.tTip.SetToolTip(this.label19, resources.GetString("label19.ToolTip"));
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(284, 244);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(18, 19);
            this.label20.TabIndex = 37;
            this.label20.Text = "%";
            this.tTip.SetToolTip(this.label20, resources.GetString("label20.ToolTip"));
            // 
            // nmb_lineWidth
            // 
            this.nmb_lineWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nmb_lineWidth.DecimalPlaces = 1;
            this.nmb_lineWidth.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.nmb_lineWidth.Location = new System.Drawing.Point(222, 242);
            this.nmb_lineWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nmb_lineWidth.Name = "nmb_lineWidth";
            this.nmb_lineWidth.Size = new System.Drawing.Size(60, 26);
            this.nmb_lineWidth.TabIndex = 36;
            this.nmb_lineWidth.TabStop = false;
            this.tTip.SetToolTip(this.nmb_lineWidth, resources.GetString("nmb_lineWidth.ToolTip"));
            this.nmb_lineWidth.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 359);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(117, 19);
            this.label14.TabIndex = 31;
            this.label14.Text = "Change font:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.nmb_fontFlLimit);
            this.groupBox1.Controls.Add(this.nmb_lineSpacing);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.cbx_detailed);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.nmb_startNum);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.chbx_useHexagons);
            this.groupBox1.Controls.Add(this.nmb_ratioBlock);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.chbx_autoRatioArea);
            this.groupBox1.Controls.Add(this.nmb_ratioCanvas);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.chbx_allowFontFl);
            this.groupBox1.Controls.Add(this.nmb_ratioArea);
            this.groupBox1.Controls.Add(this.chbx_printPercent);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.nmb_lengthLine);
            this.groupBox1.Controls.Add(this.rb_langEnglish);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.rb_langRussian);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(376, 427);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General (flowchart settings)";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.nmb_lineWidth);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.cbx_loopSize);
            this.groupBox2.Controls.Add(this.cbx_gridSzMode);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.nmb_strDiffThr);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.nmb_linesInBetw);
            this.groupBox2.Controls.Add(this.nmb_countSizes);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.chbx_allowBetwVia);
            this.groupBox2.Controls.Add(this.chbx_allowLeftVia);
            this.groupBox2.Controls.Add(this.chbx_allowCrossColumn);
            this.groupBox2.Controls.Add(this.cbx_adj);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.cbx_tmode);
            this.groupBox2.Controls.Add(this.chbx_allowWayUp);
            this.groupBox2.Location = new System.Drawing.Point(394, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(304, 409);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tracing settings";
            // 
            // dialog_font
            // 
            this.dialog_font.AllowVerticalFonts = false;
            this.dialog_font.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dialog_font.FontMustExist = true;
            this.dialog_font.MinSize = 2;
            this.dialog_font.ShowEffects = false;
            // 
            // chbx_blockedit
            // 
            this.chbx_blockedit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chbx_blockedit.AutoSize = true;
            this.chbx_blockedit.Location = new System.Drawing.Point(328, 455);
            this.chbx_blockedit.Name = "chbx_blockedit";
            this.chbx_blockedit.Size = new System.Drawing.Size(181, 23);
            this.chbx_blockedit.TabIndex = 30;
            this.chbx_blockedit.Text = "Open in BlockEdit";
            this.chbx_blockedit.UseVisualStyleBackColor = true;
            // 
            // chbx_saveSettings
            // 
            this.chbx_saveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chbx_saveSettings.AutoSize = true;
            this.chbx_saveSettings.Checked = true;
            this.chbx_saveSettings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbx_saveSettings.Location = new System.Drawing.Point(99, 455);
            this.chbx_saveSettings.Name = "chbx_saveSettings";
            this.chbx_saveSettings.Size = new System.Drawing.Size(145, 23);
            this.chbx_saveSettings.TabIndex = 31;
            this.chbx_saveSettings.Text = "Save settings";
            this.chbx_saveSettings.UseVisualStyleBackColor = true;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 492);
            this.Controls.Add(this.chbx_saveSettings);
            this.Controls.Add(this.chbx_blockedit);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btn_default);
            this.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nmb_countSizes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmb_ratioBlock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmb_ratioCanvas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmb_ratioArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmb_lengthLine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmb_startNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmb_linesInBetw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmb_strDiffThr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmb_lineSpacing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmb_fontFlLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmb_lineWidth)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FontDialog dialog_font;


        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;

        private System.Windows.Forms.ToolTip tTip;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;

        internal System.Windows.Forms.NumericUpDown nmb_countSizes;
        internal System.Windows.Forms.NumericUpDown nmb_ratioBlock;
        internal System.Windows.Forms.Button btn_default; 
        internal System.Windows.Forms.NumericUpDown nmb_ratioCanvas;
        internal System.Windows.Forms.NumericUpDown nmb_ratioArea;
        internal System.Windows.Forms.RadioButton rb_langEnglish;
        internal System.Windows.Forms.RadioButton rb_langRussian;
        internal System.Windows.Forms.NumericUpDown nmb_lengthLine;
        internal System.Windows.Forms.CheckBox chbx_printPercent;
        internal System.Windows.Forms.CheckBox chbx_allowFontFl;
        internal System.Windows.Forms.CheckBox chbx_allowWayUp;
        internal System.Windows.Forms.CheckBox chbx_autoRatioArea;
        internal System.Windows.Forms.CheckBox chbx_useHexagons;
        internal System.Windows.Forms.NumericUpDown nmb_startNum;
        internal System.Windows.Forms.ComboBox cbx_tmode;
        internal System.Windows.Forms.ComboBox cbx_adj;
        internal System.Windows.Forms.CheckBox chbx_allowCrossColumn;
        internal System.Windows.Forms.CheckBox chbx_allowLeftVia;
        internal System.Windows.Forms.CheckBox chbx_allowBetwVia;
        internal System.Windows.Forms.NumericUpDown nmb_linesInBetw;
        internal System.Windows.Forms.ComboBox cbx_detailed;
        internal System.Windows.Forms.NumericUpDown nmb_strDiffThr;
        internal System.Windows.Forms.ComboBox cbx_gridSzMode;
        internal System.Windows.Forms.CheckBox chbx_blockedit;
        private System.Windows.Forms.CheckBox chbx_saveSettings;
        private System.Windows.Forms.Label label12;
        internal System.Windows.Forms.NumericUpDown nmb_lineSpacing;
        private System.Windows.Forms.Label label13;
        internal System.Windows.Forms.NumericUpDown nmb_fontFlLimit;
        internal System.Windows.Forms.ComboBox cbx_loopSize;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        internal System.Windows.Forms.NumericUpDown nmb_lineWidth;
    }
}