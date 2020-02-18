namespace LocalSearcher
{
    partial class EditForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.pboxcover = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnaddstar = new System.Windows.Forms.Button();
            this.tbxstar = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnaddtag = new System.Windows.Forms.Button();
            this.lvtags = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnsave = new System.Windows.Forms.Button();
            this.btnreset = new System.Windows.Forms.Button();
            this.cbxtagtype = new System.Windows.Forms.ComboBox();
            this.tbxtag = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxtitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbxcid = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pboxcover)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pboxcover);
            this.panel1.Location = new System.Drawing.Point(12, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(320, 320);
            this.panel1.TabIndex = 0;
            // 
            // pboxcover
            // 
            this.pboxcover.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pboxcover.Location = new System.Drawing.Point(0, 0);
            this.pboxcover.Name = "pboxcover";
            this.pboxcover.Size = new System.Drawing.Size(318, 318);
            this.pboxcover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pboxcover.TabIndex = 0;
            this.pboxcover.TabStop = false;
            this.pboxcover.Click += new System.EventHandler(this.pboxcover_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnaddstar);
            this.panel2.Controls.Add(this.tbxstar);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.btnaddtag);
            this.panel2.Controls.Add(this.lvtags);
            this.panel2.Controls.Add(this.btnsave);
            this.panel2.Controls.Add(this.btnreset);
            this.panel2.Controls.Add(this.cbxtagtype);
            this.panel2.Controls.Add(this.tbxtag);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.tbxtitle);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.tbxcid);
            this.panel2.Location = new System.Drawing.Point(360, 26);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(320, 318);
            this.panel2.TabIndex = 1;
            // 
            // btnaddstar
            // 
            this.btnaddstar.Location = new System.Drawing.Point(238, 132);
            this.btnaddstar.Name = "btnaddstar";
            this.btnaddstar.Size = new System.Drawing.Size(77, 25);
            this.btnaddstar.TabIndex = 14;
            this.btnaddstar.Text = "添加";
            this.btnaddstar.UseVisualStyleBackColor = true;
            this.btnaddstar.Click += new System.EventHandler(this.btnaddstar_Click);
            // 
            // tbxstar
            // 
            this.tbxstar.Location = new System.Drawing.Point(3, 135);
            this.tbxstar.Name = "tbxstar";
            this.tbxstar.Size = new System.Drawing.Size(229, 21);
            this.tbxstar.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "艺人";
            // 
            // btnaddtag
            // 
            this.btnaddtag.Location = new System.Drawing.Point(238, 92);
            this.btnaddtag.Name = "btnaddtag";
            this.btnaddtag.Size = new System.Drawing.Size(77, 25);
            this.btnaddtag.TabIndex = 11;
            this.btnaddtag.Text = "添加";
            this.btnaddtag.UseVisualStyleBackColor = true;
            this.btnaddtag.Click += new System.EventHandler(this.btnaddtag_Click);
            // 
            // lvtags
            // 
            this.lvtags.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvtags.FullRowSelect = true;
            this.lvtags.GridLines = true;
            this.lvtags.HideSelection = false;
            this.lvtags.Location = new System.Drawing.Point(3, 165);
            this.lvtags.Name = "lvtags";
            this.lvtags.Size = new System.Drawing.Size(229, 144);
            this.lvtags.TabIndex = 10;
            this.lvtags.UseCompatibleStateImageBehavior = false;
            this.lvtags.View = System.Windows.Forms.View.Details;
            this.lvtags.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvtags_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "标签名            ";
            this.columnHeader1.Width = 164;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "标签类型";
            // 
            // btnsave
            // 
            this.btnsave.Location = new System.Drawing.Point(238, 245);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(77, 64);
            this.btnsave.TabIndex = 9;
            this.btnsave.Text = "保存";
            this.btnsave.UseVisualStyleBackColor = true;
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // btnreset
            // 
            this.btnreset.Location = new System.Drawing.Point(238, 165);
            this.btnreset.Name = "btnreset";
            this.btnreset.Size = new System.Drawing.Size(77, 64);
            this.btnreset.TabIndex = 8;
            this.btnreset.Text = "复位";
            this.btnreset.UseVisualStyleBackColor = true;
            this.btnreset.Click += new System.EventHandler(this.btnreset_Click);
            // 
            // cbxtagtype
            // 
            this.cbxtagtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxtagtype.FormattingEnabled = true;
            this.cbxtagtype.Items.AddRange(new object[] {
            "源站标签",
            "自定义标签"});
            this.cbxtagtype.Location = new System.Drawing.Point(155, 95);
            this.cbxtagtype.Name = "cbxtagtype";
            this.cbxtagtype.Size = new System.Drawing.Size(77, 20);
            this.cbxtagtype.TabIndex = 6;
            // 
            // tbxtag
            // 
            this.tbxtag.Location = new System.Drawing.Point(3, 95);
            this.tbxtag.Name = "tbxtag";
            this.tbxtag.Size = new System.Drawing.Size(146, 21);
            this.tbxtag.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "标签";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "标题";
            // 
            // tbxtitle
            // 
            this.tbxtitle.Location = new System.Drawing.Point(3, 56);
            this.tbxtitle.Name = "tbxtitle";
            this.tbxtitle.Size = new System.Drawing.Size(312, 21);
            this.tbxtitle.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "CID";
            // 
            // tbxcid
            // 
            this.tbxcid.Location = new System.Drawing.Point(3, 17);
            this.tbxcid.Name = "tbxcid";
            this.tbxcid.Size = new System.Drawing.Size(312, 21);
            this.tbxcid.TabIndex = 0;
            // 
            // EditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 366);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "EditForm";
            this.Load += new System.EventHandler(this.EditForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pboxcover)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pboxcover;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxcid;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxtitle;
        private System.Windows.Forms.ComboBox cbxtagtype;
        private System.Windows.Forms.TextBox tbxtag;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.Button btnreset;
        private System.Windows.Forms.ListView lvtags;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btnaddtag;
        private System.Windows.Forms.TextBox tbxstar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnaddstar;
    }
}