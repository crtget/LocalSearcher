using System;
using System.Text;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.Runtime.InteropServices; 

namespace LocalSearcher
{
    partial class MainForm
    {
        #region Windows 窗体设计器生成的代码
        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        ///  private System.Windows.Forms.MainMenu mainMenu1;


        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.btnnext = new System.Windows.Forms.Button();
            this.btnback = new System.Windows.Forms.Button();
            this.tbxsearch = new System.Windows.Forms.TextBox();
            this.panelbn = new System.Windows.Forms.Panel();
            this.panelsearch = new System.Windows.Forms.Panel();
            this.panelv = new System.Windows.Forms.Panel();
            this.lvFile = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.paneltv = new System.Windows.Forms.Panel();
            this.tvtags = new System.Windows.Forms.TreeView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblstatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.prgpercent = new System.Windows.Forms.ToolStripProgressBar();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.panelbn.SuspendLayout();
            this.panelsearch.SuspendLayout();
            this.panelv.SuspendLayout();
            this.paneltv.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2,
            this.menuItem3,
            this.menuItem4});
            this.menuItem1.Text = "文件";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.Text = "扫描文件";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "缓存文件";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "修复数据";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // btnnext
            // 
            this.btnnext.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnnext.Enabled = false;
            this.btnnext.Location = new System.Drawing.Point(70, 0);
            this.btnnext.Name = "btnnext";
            this.btnnext.Size = new System.Drawing.Size(60, 21);
            this.btnnext.TabIndex = 34;
            this.btnnext.Text = ">";
            this.btnnext.UseVisualStyleBackColor = true;
            this.btnnext.Click += new System.EventHandler(this.btnnext_Click);
            // 
            // btnback
            // 
            this.btnback.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnback.Enabled = false;
            this.btnback.Location = new System.Drawing.Point(0, 0);
            this.btnback.Name = "btnback";
            this.btnback.Size = new System.Drawing.Size(60, 21);
            this.btnback.TabIndex = 33;
            this.btnback.Text = "<";
            this.btnback.UseVisualStyleBackColor = true;
            this.btnback.Click += new System.EventHandler(this.btnback_Click);
            // 
            // tbxsearch
            // 
            this.tbxsearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxsearch.Location = new System.Drawing.Point(0, 0);
            this.tbxsearch.Name = "tbxsearch";
            this.tbxsearch.Size = new System.Drawing.Size(540, 21);
            this.tbxsearch.TabIndex = 32;
            this.tbxsearch.TextChanged += new System.EventHandler(this.tbxpath_TextChanged);
            // 
            // panelbn
            // 
            this.panelbn.Controls.Add(this.btnback);
            this.panelbn.Controls.Add(this.btnnext);
            this.panelbn.Location = new System.Drawing.Point(1, 4);
            this.panelbn.Name = "panelbn";
            this.panelbn.Size = new System.Drawing.Size(130, 21);
            this.panelbn.TabIndex = 36;
            // 
            // panelsearch
            // 
            this.panelsearch.Controls.Add(this.tbxsearch);
            this.panelsearch.Location = new System.Drawing.Point(133, 5);
            this.panelsearch.Name = "panelsearch";
            this.panelsearch.Size = new System.Drawing.Size(540, 21);
            this.panelsearch.TabIndex = 37;
            // 
            // panelv
            // 
            this.panelv.Controls.Add(this.lvFile);
            this.panelv.Location = new System.Drawing.Point(133, 37);
            this.panelv.Name = "panelv";
            this.panelv.Size = new System.Drawing.Size(540, 390);
            this.panelv.TabIndex = 38;
            // 
            // lvFile
            // 
            this.lvFile.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lvFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvFile.HideSelection = false;
            this.lvFile.Location = new System.Drawing.Point(0, 0);
            this.lvFile.Name = "lvFile";
            this.lvFile.Size = new System.Drawing.Size(540, 390);
            this.lvFile.TabIndex = 36;
            this.lvFile.UseCompatibleStateImageBehavior = false;
            this.lvFile.ItemMouseHover += new System.Windows.Forms.ListViewItemMouseHoverEventHandler(this.lvFile_ItemMouseHover);
            this.lvFile.SelectedIndexChanged += new System.EventHandler(this.lvfile_SelectedIndexChanged);
            this.lvFile.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvfile_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "名称";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "大小";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "类型";
            this.columnHeader3.Width = 120;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "时间";
            this.columnHeader4.Width = 140;
            // 
            // paneltv
            // 
            this.paneltv.Controls.Add(this.tvtags);
            this.paneltv.Location = new System.Drawing.Point(3, 37);
            this.paneltv.Name = "paneltv";
            this.paneltv.Size = new System.Drawing.Size(128, 390);
            this.paneltv.TabIndex = 39;
            // 
            // tvtags
            // 
            this.tvtags.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvtags.Location = new System.Drawing.Point(0, 0);
            this.tvtags.Name = "tvtags";
            this.tvtags.Size = new System.Drawing.Size(128, 390);
            this.tvtags.TabIndex = 0;
            this.tvtags.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvtags_NodeMouseClick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblstatus,
            this.prgpercent});
            this.statusStrip1.Location = new System.Drawing.Point(0, 638);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1264, 22);
            this.statusStrip1.TabIndex = 40;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblstatus
            // 
            this.lblstatus.Name = "lblstatus";
            this.lblstatus.Size = new System.Drawing.Size(56, 17);
            this.lblstatus.Text = "准备就绪";
            // 
            // prgpercent
            // 
            this.prgpercent.Name = "prgpercent";
            this.prgpercent.Size = new System.Drawing.Size(100, 16);
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imageList.ImageSize = new System.Drawing.Size(150, 150);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(1264, 660);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.paneltv);
            this.Controls.Add(this.panelv);
            this.Controls.Add(this.panelsearch);
            this.Controls.Add(this.panelbn);
            this.Menu = this.mainMenu1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LocalSearcher";
            this.Load += new System.EventHandler(this.ExplorerForm_Load);
            this.Resize += new System.EventHandler(this.ExplorerForm_Resize);
            this.panelbn.ResumeLayout(false);
            this.panelsearch.ResumeLayout(false);
            this.panelsearch.PerformLayout();
            this.panelv.ResumeLayout(false);
            this.paneltv.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        #endregion
        private IContainer components;
        private Button btnnext;
        private Button btnback;
        private TextBox tbxsearch;
        private Panel panelbn;
        private Panel panelsearch;
        private Panel panelv;
        private ListView lvFile;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private Panel paneltv;
        private TreeView tvtags;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblstatus;
        private MainMenu mainMenu1;
        private MenuItem menuItem1;
        private MenuItem menuItem2;
        private MenuItem menuItem3;
        private ImageList imageList;
        private ToolStripProgressBar prgpercent;
        private MenuItem menuItem4;
    }
}


