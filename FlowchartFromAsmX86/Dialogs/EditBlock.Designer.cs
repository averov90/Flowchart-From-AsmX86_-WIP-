
namespace FlowchartFromAsmX86 {
    partial class EditBlock {
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
            this.label1 = new System.Windows.Forms.Label();
            this.type = new System.Windows.Forms.ComboBox();
            this.description = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lv = new System.Windows.Forms.ListView();
            this.Index = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Cmd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Figure = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Connection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MetaCommand = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.container = new System.Windows.Forms.SplitContainer();
            this.button3 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textCmd = new System.Windows.Forms.TextBox();
            this.chbx_clearCmd = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.container)).BeginInit();
            this.container.Panel1.SuspendLayout();
            this.container.Panel2.SuspendLayout();
            this.container.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Change type:";
            // 
            // type
            // 
            this.type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.type.FormattingEnabled = true;
            this.type.Items.AddRange(new object[] {
            "Process",
            "FDecision",
            "JDecision",
            "Subprocess",
            "Terminator",
            "Hexagon",
            "Connector",
            "LoopS",
            "LoopE",
            "I/O"});
            this.type.Location = new System.Drawing.Point(136, 10);
            this.type.Name = "type";
            this.type.Size = new System.Drawing.Size(154, 27);
            this.type.TabIndex = 3;
            this.type.SelectedIndexChanged += new System.EventHandler(this.type_SelectedIndexChanged);
            // 
            // description
            // 
            this.description.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.description.Location = new System.Drawing.Point(3, 25);
            this.description.Multiline = true;
            this.description.Name = "description";
            this.description.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.description.Size = new System.Drawing.Size(266, 222);
            this.description.TabIndex = 1;
            this.description.WordWrap = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(153, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "New description:";
            // 
            // lv
            // 
            this.lv.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.lv.AllowColumnReorder = true;
            this.lv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Index,
            this.Cmd,
            this.columnHeader1,
            this.Figure,
            this.Connection,
            this.MetaCommand});
            this.lv.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lv.FullRowSelect = true;
            this.lv.GridLines = true;
            this.lv.HideSelection = false;
            this.lv.LabelWrap = false;
            this.lv.Location = new System.Drawing.Point(3, 25);
            this.lv.MultiSelect = false;
            this.lv.Name = "lv";
            this.lv.ShowItemToolTips = true;
            this.lv.Size = new System.Drawing.Size(308, 251);
            this.lv.TabIndex = 2;
            this.lv.UseCompatibleStateImageBehavior = false;
            this.lv.View = System.Windows.Forms.View.Details;
            this.lv.VirtualMode = true;
            this.lv.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.lv_RetrieveVirtualItem);
            // 
            // Index
            // 
            this.Index.Text = "Row";
            this.Index.Width = 40;
            // 
            // Cmd
            // 
            this.Cmd.Text = "Command";
            this.Cmd.Width = 70;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Description";
            this.columnHeader1.Width = 100;
            // 
            // Figure
            // 
            this.Figure.Text = "Figure";
            this.Figure.Width = 95;
            // 
            // Connection
            // 
            this.Connection.Text = "Connection";
            this.Connection.Width = 95;
            // 
            // MetaCommand
            // 
            this.MetaCommand.Text = "Meta command";
            this.MetaCommand.Width = 110;
            // 
            // container
            // 
            this.container.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.container.BackColor = System.Drawing.SystemColors.ControlDark;
            this.container.Location = new System.Drawing.Point(12, 43);
            this.container.Name = "container";
            // 
            // container.Panel1
            // 
            this.container.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.container.Panel1.Controls.Add(this.button3);
            this.container.Panel1.Controls.Add(this.label2);
            this.container.Panel1.Controls.Add(this.description);
            this.container.Panel1MinSize = 200;
            // 
            // container.Panel2
            // 
            this.container.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.container.Panel2.Controls.Add(this.label3);
            this.container.Panel2.Controls.Add(this.lv);
            this.container.Panel2MinSize = 130;
            this.container.Size = new System.Drawing.Size(590, 276);
            this.container.SplitterDistance = 272;
            this.container.TabIndex = 5;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.Location = new System.Drawing.Point(0, 247);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 29);
            this.button3.TabIndex = 8;
            this.button3.Text = "Clear";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 19);
            this.label3.TabIndex = 6;
            this.label3.Text = "Connect to:";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(446, 362);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 29);
            this.button2.TabIndex = 7;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(527, 362);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 29);
            this.button1.TabIndex = 6;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 328);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(189, 19);
            this.label4.TabIndex = 8;
            this.label4.Text = "Change command text:";
            // 
            // textCmd
            // 
            this.textCmd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textCmd.Location = new System.Drawing.Point(208, 325);
            this.textCmd.Name = "textCmd";
            this.textCmd.Size = new System.Drawing.Size(394, 26);
            this.textCmd.TabIndex = 9;
            // 
            // chbx_clearCmd
            // 
            this.chbx_clearCmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chbx_clearCmd.AutoSize = true;
            this.chbx_clearCmd.Location = new System.Drawing.Point(17, 357);
            this.chbx_clearCmd.Name = "chbx_clearCmd";
            this.chbx_clearCmd.Size = new System.Drawing.Size(289, 23);
            this.chbx_clearCmd.TabIndex = 10;
            this.chbx_clearCmd.Text = "Clear source command on apply";
            this.chbx_clearCmd.UseVisualStyleBackColor = true;
            // 
            // EditBlock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 403);
            this.Controls.Add(this.chbx_clearCmd);
            this.Controls.Add(this.textCmd);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.container);
            this.Controls.Add(this.type);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimizeBox = false;
            this.Name = "EditBlock";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit block";
            this.Shown += new System.EventHandler(this.EditBlock_Shown);
            this.container.Panel1.ResumeLayout(false);
            this.container.Panel1.PerformLayout();
            this.container.Panel2.ResumeLayout(false);
            this.container.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.container)).EndInit();
            this.container.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.TextBox description;
        private System.Windows.Forms.Label label2;
        internal System.Windows.Forms.ListView lv;
        private System.Windows.Forms.ColumnHeader Index;
        private System.Windows.Forms.ColumnHeader Cmd;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader Figure;
        private System.Windows.Forms.ColumnHeader Connection;
        private System.Windows.Forms.ColumnHeader MetaCommand;
        internal System.Windows.Forms.SplitContainer container;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label4;
        internal System.Windows.Forms.ComboBox type;
        internal System.Windows.Forms.TextBox textCmd;
        internal System.Windows.Forms.CheckBox chbx_clearCmd;
    }
}