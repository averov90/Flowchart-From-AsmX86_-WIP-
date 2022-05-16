namespace FlowchartFromAsmX86 {
    partial class MainForm {
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
            this.tb_code = new System.Windows.Forms.TextBox();
            this.splContainer = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_generate = new System.Windows.Forms.Button();
            this.lbl_scale = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_exportVisio = new System.Windows.Forms.Button();
            this.btn_scaleAdd = new System.Windows.Forms.Button();
            this.btn_scaleSub = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_exportPng = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pic_flowchart = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.splContainer)).BeginInit();
            this.splContainer.Panel1.SuspendLayout();
            this.splContainer.Panel2.SuspendLayout();
            this.splContainer.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_flowchart)).BeginInit();
            this.SuspendLayout();
            // 
            // tb_code
            // 
            this.tb_code.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_code.Location = new System.Drawing.Point(4, 23);
            this.tb_code.Margin = new System.Windows.Forms.Padding(4);
            this.tb_code.Multiline = true;
            this.tb_code.Name = "tb_code";
            this.tb_code.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tb_code.Size = new System.Drawing.Size(212, 396);
            this.tb_code.TabIndex = 0;
            this.tb_code.WordWrap = false;
            // 
            // splContainer
            // 
            this.splContainer.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splContainer.Location = new System.Drawing.Point(0, 0);
            this.splContainer.Margin = new System.Windows.Forms.Padding(4);
            this.splContainer.Name = "splContainer";
            // 
            // splContainer.Panel1
            // 
            this.splContainer.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splContainer.Panel1.Controls.Add(this.label1);
            this.splContainer.Panel1.Controls.Add(this.btn_generate);
            this.splContainer.Panel1.Controls.Add(this.tb_code);
            this.splContainer.Panel1MinSize = 212;
            // 
            // splContainer.Panel2
            // 
            this.splContainer.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splContainer.Panel2.Controls.Add(this.lbl_scale);
            this.splContainer.Panel2.Controls.Add(this.label4);
            this.splContainer.Panel2.Controls.Add(this.btn_exportVisio);
            this.splContainer.Panel2.Controls.Add(this.btn_scaleAdd);
            this.splContainer.Panel2.Controls.Add(this.btn_scaleSub);
            this.splContainer.Panel2.Controls.Add(this.label3);
            this.splContainer.Panel2.Controls.Add(this.btn_exportPng);
            this.splContainer.Panel2.Controls.Add(this.panel1);
            this.splContainer.Panel2MinSize = 480;
            this.splContainer.Size = new System.Drawing.Size(709, 461);
            this.splContainer.SplitterDistance = 221;
            this.splContainer.SplitterWidth = 8;
            this.splContainer.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(198, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Paste your code here:";
            // 
            // btn_generate
            // 
            this.btn_generate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_generate.Location = new System.Drawing.Point(4, 428);
            this.btn_generate.Margin = new System.Windows.Forms.Padding(4);
            this.btn_generate.Name = "btn_generate";
            this.btn_generate.Size = new System.Drawing.Size(213, 29);
            this.btn_generate.TabIndex = 1;
            this.btn_generate.Text = "Generate";
            this.btn_generate.UseVisualStyleBackColor = true;
            this.btn_generate.Click += new System.EventHandler(this.btn_generate_Click);
            // 
            // lbl_scale
            // 
            this.lbl_scale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_scale.AutoSize = true;
            this.lbl_scale.Location = new System.Drawing.Point(385, 1);
            this.lbl_scale.Name = "lbl_scale";
            this.lbl_scale.Size = new System.Drawing.Size(18, 19);
            this.lbl_scale.TabIndex = 11;
            this.lbl_scale.Text = "-";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(244, 1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 19);
            this.label4.TabIndex = 10;
            this.label4.Text = "current scale:";
            // 
            // btn_exportVisio
            // 
            this.btn_exportVisio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_exportVisio.Location = new System.Drawing.Point(179, 428);
            this.btn_exportVisio.Name = "btn_exportVisio";
            this.btn_exportVisio.Size = new System.Drawing.Size(152, 29);
            this.btn_exportVisio.TabIndex = 9;
            this.btn_exportVisio.Text = "Export to Visio";
            this.btn_exportVisio.UseVisualStyleBackColor = true;
            // 
            // btn_scaleAdd
            // 
            this.btn_scaleAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_scaleAdd.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_scaleAdd.Location = new System.Drawing.Point(112, 424);
            this.btn_scaleAdd.Name = "btn_scaleAdd";
            this.btn_scaleAdd.Size = new System.Drawing.Size(34, 27);
            this.btn_scaleAdd.TabIndex = 8;
            this.btn_scaleAdd.Text = "+";
            this.btn_scaleAdd.UseVisualStyleBackColor = true;
            // 
            // btn_scaleSub
            // 
            this.btn_scaleSub.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_scaleSub.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_scaleSub.Location = new System.Drawing.Point(72, 424);
            this.btn_scaleSub.Name = "btn_scaleSub";
            this.btn_scaleSub.Size = new System.Drawing.Size(34, 27);
            this.btn_scaleSub.TabIndex = 7;
            this.btn_scaleSub.Text = "-";
            this.btn_scaleSub.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 428);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 19);
            this.label3.TabIndex = 6;
            this.label3.Text = "Scale:";
            // 
            // btn_exportPng
            // 
            this.btn_exportPng.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_exportPng.Location = new System.Drawing.Point(337, 428);
            this.btn_exportPng.Name = "btn_exportPng";
            this.btn_exportPng.Size = new System.Drawing.Size(136, 29);
            this.btn_exportPng.TabIndex = 5;
            this.btn_exportPng.Text = "Export to PNG";
            this.btn_exportPng.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.Controls.Add(this.pic_flowchart);
            this.panel1.Location = new System.Drawing.Point(3, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(470, 396);
            this.panel1.TabIndex = 4;
            // 
            // pic_flowchart
            // 
            this.pic_flowchart.Location = new System.Drawing.Point(0, 0);
            this.pic_flowchart.Name = "pic_flowchart";
            this.pic_flowchart.Size = new System.Drawing.Size(100, 50);
            this.pic_flowchart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pic_flowchart.TabIndex = 0;
            this.pic_flowchart.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 461);
            this.Controls.Add(this.splContainer);
            this.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(725, 500);
            this.Name = "MainForm";
            this.Text = "Simple flowchart generator from Intel8086 assembler code";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.splContainer.Panel1.ResumeLayout(false);
            this.splContainer.Panel1.PerformLayout();
            this.splContainer.Panel2.ResumeLayout(false);
            this.splContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splContainer)).EndInit();
            this.splContainer.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_flowchart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tb_code;
        private System.Windows.Forms.SplitContainer splContainer;
        private System.Windows.Forms.Button btn_generate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pic_flowchart;
        private System.Windows.Forms.Button btn_exportPng;
        private System.Windows.Forms.Button btn_scaleAdd;
        private System.Windows.Forms.Button btn_scaleSub;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_exportVisio;
        private System.Windows.Forms.Label lbl_scale;
        private System.Windows.Forms.Label label4;
    }
}

