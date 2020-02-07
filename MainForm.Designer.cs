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
            this.btngo = new System.Windows.Forms.Button();
            this.btnback = new System.Windows.Forms.Button();
            this.tbxpath = new System.Windows.Forms.TextBox();
            this.panelbg = new System.Windows.Forms.Panel();
            this.paneladdr = new System.Windows.Forms.Panel();
            this.panelfv = new System.Windows.Forms.Panel();
            this.lvFile = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.paneltv = new System.Windows.Forms.Panel();
            this.tvdir = new System.Windows.Forms.TreeView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblstatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.prgpercent = new System.Windows.Forms.ToolStripProgressBar();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.panelbg.SuspendLayout();
            this.paneladdr.SuspendLayout();
            this.panelfv.SuspendLayout();
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
            // btngo
            // 
            this.btngo.Enabled = false;
            this.btngo.Location = new System.Drawing.Point(34, 3);
            this.btngo.Name = "btngo";
            this.btngo.Size = new System.Drawing.Size(30, 25);
            this.btngo.TabIndex = 34;
            this.btngo.Text = ">";
            this.btngo.UseVisualStyleBackColor = true;
            // 
            // btnback
            // 
            this.btnback.Enabled = false;
            this.btnback.Location = new System.Drawing.Point(2, 3);
            this.btnback.Name = "btnback";
            this.btnback.Size = new System.Drawing.Size(30, 25);
            this.btnback.TabIndex = 33;
            this.btnback.Text = "<";
            this.btnback.UseVisualStyleBackColor = true;
            // 
            // tbxpath
            // 
            this.tbxpath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxpath.Location = new System.Drawing.Point(0, 0);
            this.tbxpath.Name = "tbxpath";
            this.tbxpath.Size = new System.Drawing.Size(606, 21);
            this.tbxpath.TabIndex = 32;
            // 
            // panelbg
            // 
            this.panelbg.Controls.Add(this.btnback);
            this.panelbg.Controls.Add(this.btngo);
            this.panelbg.Location = new System.Drawing.Point(1, 1);
            this.panelbg.Name = "panelbg";
            this.panelbg.Size = new System.Drawing.Size(71, 33);
            this.panelbg.TabIndex = 36;
            // 
            // paneladdr
            // 
            this.paneladdr.Controls.Add(this.tbxpath);
            this.paneladdr.Location = new System.Drawing.Point(71, 6);
            this.paneladdr.Name = "paneladdr";
            this.paneladdr.Size = new System.Drawing.Size(606, 25);
            this.paneladdr.TabIndex = 37;
            // 
            // panelfv
            // 
            this.panelfv.Controls.Add(this.lvFile);
            this.panelfv.Location = new System.Drawing.Point(71, 37);
            this.panelfv.Name = "panelfv";
            this.panelfv.Size = new System.Drawing.Size(606, 390);
            this.panelfv.TabIndex = 38;
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
            this.lvFile.Size = new System.Drawing.Size(606, 390);
            this.lvFile.TabIndex = 36;
            this.lvFile.UseCompatibleStateImageBehavior = false;
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
            this.paneltv.Controls.Add(this.tvdir);
            this.paneltv.Location = new System.Drawing.Point(3, 37);
            this.paneltv.Name = "paneltv";
            this.paneltv.Size = new System.Drawing.Size(65, 390);
            this.paneltv.TabIndex = 39;
            // 
            // tvdir
            // 
            this.tvdir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvdir.Location = new System.Drawing.Point(0, 0);
            this.tvdir.Name = "tvdir";
            this.tvdir.Size = new System.Drawing.Size(65, 390);
            this.tvdir.TabIndex = 0;
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
            this.imageList.ImageSize = new System.Drawing.Size(128, 128);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(1264, 660);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.paneltv);
            this.Controls.Add(this.panelfv);
            this.Controls.Add(this.paneladdr);
            this.Controls.Add(this.panelbg);
            this.Menu = this.mainMenu1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LocalSearcher";
            this.Load += new System.EventHandler(this.ExplorerForm_Load);
            this.Resize += new System.EventHandler(this.ExplorerForm_Resize);
            this.panelbg.ResumeLayout(false);
            this.paneladdr.ResumeLayout(false);
            this.paneladdr.PerformLayout();
            this.panelfv.ResumeLayout(false);
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
        private Button btngo;
        private Button btnback;
        private TextBox tbxpath;
        private Panel panelbg;
        private Panel paneladdr;
        private Panel panelfv;
        private ListView lvFile;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private Panel paneltv;
        private TreeView tvdir;
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


