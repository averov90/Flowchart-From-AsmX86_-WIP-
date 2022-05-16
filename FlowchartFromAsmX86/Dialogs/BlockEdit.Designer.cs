
namespace FlowchartFromAsmX86 {
    partial class BlockEdit {
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
            this.lv = new System.Windows.Forms.ListView();
            this.Index = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Cmd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Description = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Figure = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Connection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MetaCommand = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuS = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editBlockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editDescriptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.insertBlockbeforeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertBlockafterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.copyBlockToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.removeBlocksingleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeBlockcascadeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.contextMenuS.SuspendLayout();
            this.SuspendLayout();
            // 
            // lv
            // 
            this.lv.AllowColumnReorder = true;
            this.lv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Index,
            this.Cmd,
            this.Description,
            this.Figure,
            this.Connection,
            this.MetaCommand});
            this.lv.ContextMenuStrip = this.contextMenuS;
            this.lv.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lv.FullRowSelect = true;
            this.lv.GridLines = true;
            this.lv.HideSelection = false;
            this.lv.LabelWrap = false;
            this.lv.Location = new System.Drawing.Point(12, 12);
            this.lv.MultiSelect = false;
            this.lv.Name = "lv";
            this.lv.ShowItemToolTips = true;
            this.lv.Size = new System.Drawing.Size(718, 327);
            this.lv.TabIndex = 0;
            this.lv.UseCompatibleStateImageBehavior = false;
            this.lv.View = System.Windows.Forms.View.Details;
            this.lv.SelectedIndexChanged += new System.EventHandler(this.lv_SelectedIndexChanged);
            this.lv.DoubleClick += new System.EventHandler(this.lv_DoubleClick);
            this.lv.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lv_KeyUp);
            // 
            // Index
            // 
            this.Index.Text = "Row";
            this.Index.Width = 40;
            // 
            // Cmd
            // 
            this.Cmd.Text = "Command";
            this.Cmd.Width = 180;
            // 
            // Description
            // 
            this.Description.Text = "Description";
            this.Description.Width = 300;
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
            // contextMenuS
            // 
            this.contextMenuS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editBlockToolStripMenuItem,
            this.editDescriptionToolStripMenuItem,
            this.toolStripSeparator2,
            this.findToolStripMenuItem,
            this.toolStripSeparator1,
            this.insertBlockbeforeToolStripMenuItem,
            this.insertBlockafterToolStripMenuItem,
            this.toolStripSeparator3,
            this.copyBlockToToolStripMenuItem,
            this.toolStripSeparator4,
            this.removeBlocksingleToolStripMenuItem,
            this.removeBlockcascadeToolStripMenuItem});
            this.contextMenuS.Name = "contextMenuS";
            this.contextMenuS.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuS.ShowImageMargin = false;
            this.contextMenuS.Size = new System.Drawing.Size(178, 204);
            // 
            // editBlockToolStripMenuItem
            // 
            this.editBlockToolStripMenuItem.Name = "editBlockToolStripMenuItem";
            this.editBlockToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.editBlockToolStripMenuItem.Text = "Edit block";
            this.editBlockToolStripMenuItem.Click += new System.EventHandler(this.editBlockToolStripMenuItem_Click);
            // 
            // editDescriptionToolStripMenuItem
            // 
            this.editDescriptionToolStripMenuItem.Name = "editDescriptionToolStripMenuItem";
            this.editDescriptionToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.editDescriptionToolStripMenuItem.Text = "Edit description";
            this.editDescriptionToolStripMenuItem.Click += new System.EventHandler(this.editDescriptionToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(174, 6);
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.findToolStripMenuItem.Text = "Find";
            this.findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(174, 6);
            // 
            // insertBlockbeforeToolStripMenuItem
            // 
            this.insertBlockbeforeToolStripMenuItem.Name = "insertBlockbeforeToolStripMenuItem";
            this.insertBlockbeforeToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.insertBlockbeforeToolStripMenuItem.Text = "Insert block (before)";
            this.insertBlockbeforeToolStripMenuItem.Click += new System.EventHandler(this.insertBlockbeforeToolStripMenuItem_Click);
            // 
            // insertBlockafterToolStripMenuItem
            // 
            this.insertBlockafterToolStripMenuItem.Name = "insertBlockafterToolStripMenuItem";
            this.insertBlockafterToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.insertBlockafterToolStripMenuItem.Text = "Insert block (after)";
            this.insertBlockafterToolStripMenuItem.Click += new System.EventHandler(this.insertBlockafterToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(174, 6);
            // 
            // copyBlockToToolStripMenuItem
            // 
            this.copyBlockToToolStripMenuItem.Name = "copyBlockToToolStripMenuItem";
            this.copyBlockToToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.copyBlockToToolStripMenuItem.Text = "Copy block To";
            this.copyBlockToToolStripMenuItem.Click += new System.EventHandler(this.copyBlockToToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(174, 6);
            // 
            // removeBlocksingleToolStripMenuItem
            // 
            this.removeBlocksingleToolStripMenuItem.Name = "removeBlocksingleToolStripMenuItem";
            this.removeBlocksingleToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.removeBlocksingleToolStripMenuItem.Text = "Remove block (single)";
            this.removeBlocksingleToolStripMenuItem.Click += new System.EventHandler(this.removeBlocksingleToolStripMenuItem_Click);
            // 
            // removeBlockcascadeToolStripMenuItem
            // 
            this.removeBlockcascadeToolStripMenuItem.Name = "removeBlockcascadeToolStripMenuItem";
            this.removeBlockcascadeToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.removeBlockcascadeToolStripMenuItem.Text = "Remove block (cascade)";
            this.removeBlockcascadeToolStripMenuItem.Click += new System.EventHandler(this.removeBlockcascadeToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(492, 345);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(238, 29);
            this.button1.TabIndex = 1;
            this.button1.Text = "Apply changes & Next >";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseMnemonic = false;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(492, 376);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(238, 29);
            this.button2.TabIndex = 2;
            this.button2.Text = "Discard changes & Next >";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseMnemonic = false;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.button3.Location = new System.Drawing.Point(12, 368);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(79, 29);
            this.button3.TabIndex = 3;
            this.button3.Text = "Abort";
            this.button3.UseMnemonic = false;
            this.button3.UseVisualStyleBackColor = true;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.linkLabel1.Location = new System.Drawing.Point(124, 373);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(45, 19);
            this.linkLabel1.TabIndex = 6;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Help";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // BlockEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 409);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lv);
            this.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(650, 440);
            this.Name = "BlockEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BlockEdit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BlockEdit_FormClosing);
            this.Shown += new System.EventHandler(this.BlockEdit_Shown);
            this.contextMenuS.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.ListView lv;
        private System.Windows.Forms.ColumnHeader Index;
        private System.Windows.Forms.ColumnHeader Cmd;
        private System.Windows.Forms.ColumnHeader Description;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ColumnHeader Figure;
        private System.Windows.Forms.ColumnHeader Connection;
        private System.Windows.Forms.ColumnHeader MetaCommand;
        private System.Windows.Forms.ContextMenuStrip contextMenuS;
        private System.Windows.Forms.ToolStripMenuItem editDescriptionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editBlockToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertBlockbeforeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertBlockafterToolStripMenuItem;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.ToolStripMenuItem removeBlocksingleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeBlockcascadeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem copyBlockToToolStripMenuItem;
    }
}