using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace LocalSearcher
{
    public partial class EditForm : Form
    {
        private MainForm mainform;
        public MainForm.Movie MovieData;


        public void ResetForm()
        {
            this.Text = MovieData.fname;
            cbxtagtype.SelectedIndex = 0;
            pboxcover.Image = Image.FromStream(MovieData.cover);
            tbxcid.Text = MovieData.code;
            tbxtitle.Text = MovieData.title;
            lvtags.Items.Clear();

            foreach (var t in MovieData.tags)
            {
                var li = new ListViewItem();
                li.Text = t;
                li.SubItems.Add("0");
                lvtags.Items.Add(li);
            }

            foreach (var t in MovieData.customtags)
            {
                var li = new ListViewItem();
                li.Text = t;
                li.SubItems.Add("1");
                lvtags.Items.Add(li);
            }

            foreach (var t in MovieData.stars)
            {
                var li = new ListViewItem();
                li.Text = t;
                li.SubItems.Add("2");
                lvtags.Items.Add(li);
            }

            foreach (ColumnHeader ch in lvtags.Columns)
            {
                ch.Width = -2;
            }

        }


        public AutoCompleteStringCollection GetTags(int type)
        {
            var result = new AutoCompleteStringCollection();
            string sql = "";

            if (type < 2)
            {
                sql = "SELECT CAPTION FROM TAGS WHERE TYPE < 2";
            }
            else
            {
                sql = "SELECT CAPTION FROM TAGS WHERE TYPE = 2";
            }

            
            var r = MySqlHelper.ExecuteReader(sql, CommandType.Text, null);

            while (r.Read())
            {
                result.Add(r.GetString(r.GetOrdinal("CAPTION")));
            }

      
            r.Close();
            return result;
        }


        private System.Drawing.Imaging.ImageFormat GetImageFormat(Image _img)
        {
            if (_img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
            {
                return System.Drawing.Imaging.ImageFormat.Jpeg;
            }
            if (_img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
            {
                return System.Drawing.Imaging.ImageFormat.Gif;
            }
            if (_img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
            {

                return System.Drawing.Imaging.ImageFormat.Png;
            }
            if (_img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
            {

                return System.Drawing.Imaging.ImageFormat.Bmp;
            }

            return System.Drawing.Imaging.ImageFormat.Jpeg;
        }





        public EditForm(MainForm mf, MainForm.Movie md)
        {
            InitializeComponent();
            MovieData = md;
            mainform = mf;
        }



        private void EditForm_Load(object sender, EventArgs e)
        {
            ResetForm();
            tbxtag.AutoCompleteCustomSource = GetTags(0);
            tbxtag.AutoCompleteMode = AutoCompleteMode.Suggest;
            tbxtag.AutoCompleteSource = AutoCompleteSource.CustomSource;

            tbxstar.AutoCompleteCustomSource = GetTags(2);
            tbxstar.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tbxstar.AutoCompleteSource = AutoCompleteSource.CustomSource;


            if (MovieData.cover.Length < 0x512)
            {
                if (System.Windows.Forms.Clipboard.ContainsImage())
                {
                    var img = System.Windows.Forms.Clipboard.GetImage();
                    pboxcover.Image = img;

                }
            }



            
        }

        private void pboxcover_Click(object sender, EventArgs e)
        {
            //
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "图像文件(*.jpg;*.jpeg;*.png)|*.jpg;*.png";

            if (op.ShowDialog() == DialogResult.OK)
            {
                pboxcover.Image = Image.FromFile(op.FileName);
            }



        }

        private void btnaddtag_Click(object sender, EventArgs e)
        {
            var tag = tbxtag.Text.Trim();

            if (tag != "")
            {
                if (lvtags.Items.Count > 0 && lvtags.FindItemWithText(tag, true, 0, false) != null)
                {
                    return;
                }

                var li = new ListViewItem(tag);
                li.SubItems.Add(cbxtagtype.SelectedIndex.ToString());
                lvtags.Items.Add(li);
                tbxtag.Text = "";

            }
        }


        private void btnaddstar_Click(object sender, EventArgs e)
        {
            var star = tbxstar.Text.Trim();

            if (star != "")
            {
                if (lvtags.Items.Count > 0 && lvtags.FindItemWithText(star, true, 0, false) != null)
                {
                    return;
                }

                var li = new ListViewItem(star);
                li.SubItems.Add("2");
                lvtags.Items.Add(li);
                tbxstar.Text = "";

            }
        }

        private void btnreset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            var md = new MainForm.Movie(MovieData.fname);
            md.id = MovieData.id;
            pboxcover.Image.Save(md.cover, GetImageFormat(pboxcover.Image));
            pboxcover.Image = null;
            md.code = tbxcid.Text;
            md.title = tbxtitle.Text;
            md.tags.Clear(); md.customtags.Clear(); md.stars.Clear();

            foreach(ListViewItem li in lvtags.Items)
            {
                var type = li.SubItems[1].Text.ToString();

                switch(type)
                {
                    case "0":
                        md.tags.Add(li.Text);
                        break;
                    case "1":
                        md.customtags.Add(li.Text);
                        break;
                    case "2":
                        md.stars.Add(li.Text);
                        break;
                }


            }

            mainform.UpdateMovie(md);
            mainform.GetMoviesList(mainform.searchtype, mainform.searchpage, mainform.searchword);
            this.Close();

        }

        private void lvtags_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && lvtags.SelectedIndices.Count >= 0)
            {
                lvtags.Items.RemoveAt(lvtags.FocusedItem.Index);
            }

        }
    }
}
