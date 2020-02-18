using System;
using System.Text;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using MySql.Data.MySqlClient;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

//byte[] ba = File.ReadAllBytes(Directory.GetCurrentDirectory() + "/test.jpg");
//string s = Convert.ToBase64String(ba);
//File.WriteAllText(Directory.GetCurrentDirectory() + "/test.txt", s);



//SELECT movies.* FROM (TAGS INNER JOIN TAGSMAP ON TAGSMAP.TID = TAGS.ID ) INNER JOIN MOVIES ON MOVIES.ID = TAGSMAP.MID where TAGS.caption like @word order by name desc
//查询重复
//SELECT * FROM movies AS ta WHERE ta.id <> ( SELECT min( tb.id ) FROM movies AS tb WHERE ta.fname = tb.fname ) order by fname, id

namespace LocalSearcher
{
    public partial class MainForm : Form
    {


        public class Hparam
        {
            public string url;
            public string postdata;
        }

        public class Hresp
        {
            public string respstr;
        }





        public class Movie
        {
            public int id;
            public int type;
            public string code;
            public string title;
            public List<string> tags;
            public List<string> customtags;
            public List<string> stars;
            public MemoryStream cover;
            public string name;
            public string fname;

            public Movie(string fname)
            {
                this.type = 1;
                this.code = Path.GetFileNameWithoutExtension(fname);
                this.title = "暂无";
                this.tags = new List<string>();
                this.customtags = new List<string>();
                this.stars = new List<string>();
                this.cover = new MemoryStream();
                this.name = Path.GetFileName(fname);
                this.fname = fname;

            }

        }




        public class TagData
        {
            public int type;
            public string name;
            public string caption;
        }

        public class ListItemTag
        {
            public int id;
            public int type;
            public string code;
            public string fullname;
            public string title;
            public TagData[] tags;
        }

        public class ImageData
        {
            public string name;
            public Image img;
            public ImageData(string name, Image img)
            {
                this.name = name;
                this.img = img;
            }
        }




        public string nopic = "/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/2wBDAQkJCQwLDBgNDRgyIRwhMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjL/wgARCAAQABADASIAAhEBAxEB/8QAFQABAQAAAAAAAAAAAAAAAAAAAAb/xAAUAQEAAAAAAAAAAAAAAAAAAAAA/9oADAMBAAIQAxAAAAG/B//EABQQAQAAAAAAAAAAAAAAAAAAACD/2gAIAQEAAQUCH//EABQRAQAAAAAAAAAAAAAAAAAAAAD/2gAIAQMBAT8Bf//EABQRAQAAAAAAAAAAAAAAAAAAAAD/2gAIAQIBAT8Bf//EABQQAQAAAAAAAAAAAAAAAAAAACD/2gAIAQEABj8CH//EABQQAQAAAAAAAAAAAAAAAAAAACD/2gAIAQEAAT8hH//aAAwDAQACAAMAAAAQ8//EABQRAQAAAAAAAAAAAAAAAAAAAAD/2gAIAQMBAT8Qf//EABQRAQAAAAAAAAAAAAAAAAAAAAD/2gAIAQIBAT8Qf//EABQQAQAAAAAAAAAAAAAAAAAAACD/2gAIAQEAAT8QH//Z";

        public CookieContainer coa = new CookieContainer();
        public Hparam hparam { get; set; }
        public Hresp hresp { get; set; }

        public string dmmdataurl = "https://www.dmm.co.jp/mono/dvd/-/detail/=/cid=";

        public static List<Movie> MovieList = new List<Movie>();
        public static List<Everything.FileInfo> FileCache = new List<Everything.FileInfo>();
        SynchronizationContext SyncContext = null;
        public int searchtype, searchpage = 0;
        public string searchword = "";

        public Task CacheDataTask, SaveDataTask, UpdateDataTask;




        public delegate void EventHandler();
        public event EventHandler PostErrorEvent;
        public void OnPostErrorEvent()
        {
            if (PostErrorEvent != null)
                PostErrorEvent();
        }

        public void PostError_Event()
        {
            //AddLog("请求超时 2秒后重试");
            Thread.Sleep(2000);
            CTRHttp();
        }




        public void SetStatus(object o)
        {
            this.lblstatus.Text = o as string;
        }

        public void SetProgress(object o)
        {
            this.prgpercent.Value = (Int32)o;
        }


        public void AddListItem(object o)
        {
            this.lvFile.Items.AddRange(o as ListViewItem[]);
        }


        public void AddImageList(object o)
        {
            var d = o as ImageData;
            this.imageList.Images.Add(d.name, d.img);
        }


        public void SetBackNext(object o)
        {
            var b = Convert.ToByte(o);

            if ((b & BitConverter.GetBytes(1)[0]) > 0)
            {
                btnback.Enabled = true;
            }
            else
            {
                btnback.Enabled = false;
            }

            if ((b & BitConverter.GetBytes(2)[0]) > 0)
            {
                btnnext.Enabled = true;
            }
            else
            {
                btnnext.Enabled = false;
            }

        }


        public void LockUI(object o)
        {
            var b = (bool)o;

            panelbn.Enabled = !b;
            panelsearch.Enabled = !b;
            paneltv.Enabled = !b;
            panelv.Enabled = !b;

        }


        public MainForm()
        {
            //
            // Windows 窗体设计器支持所必需的
            //
            InitializeComponent();
            SyncContext = SynchronizationContext.Current;
            PostErrorEvent += new EventHandler(PostError_Event);
            hparam = new Hparam();
            hresp = new Hresp();
            lvFile.LargeImageList = imageList;
        }


        private void ExplorerForm_Load(object sender, EventArgs e)
        {



            string sql = "SELECT count(*) as count, tags.caption as caption, tags.type as type FROM (TAGS INNER JOIN TAGSMAP ON TAGSMAP.TID = TAGS.ID ) INNER JOIN MOVIES ON MOVIES.ID = TAGSMAP.MID GROUP BY tags.caption ORDER BY count desc";
            var r = MySqlHelper.ExecuteReader(sql, CommandType.Text, null);
            TreeNode[] tnodes = { new TreeNode("源站标签"), new TreeNode("自定义标签"), new TreeNode("艺人标签") };

            while (r.Read())
            {
                var type = r.GetInt32(r.GetOrdinal("type"));
                var caption = r.GetString(r.GetOrdinal("caption"));

                if (tnodes[type].GetNodeCount(true) < 10)
                    tnodes[type].Nodes.Add(caption);
            }

            tvtags.Nodes.AddRange(tnodes);
            tvtags.ExpandAll();
            r.Close();
            GetMoviesList(1, 1, "暂无");
        }


        public void GetMoviesList(int mtype, int page, string word = null)
        {
            MovieList.Clear();
            lvFile.Items.Clear();
            imageList.Images.Clear();
            searchtype = mtype;
            searchpage = page;
            searchword = word;
            MySqlParameter[] paramarr;
            string sql = "";
            int total = 0;
            byte bback = 1, bnext = 2;
            var count = 0;
            List<ListViewItem> LvList = new List<ListViewItem>();


            Task ttask = new Task(() =>
            {




                SyncContext.Send(LockUI, true);

                if (mtype > 1)
                {
                    sql = "SELECT DISTINCT COUNT(*) FROM MOVIES WHERE TITLE LIKE @WORD OR CODE LIKE @WORD OR ID IN (SELECT MID FROM TAGSMAP WHERE TID IN (SELECT ID FROM TAGS WHERE CAPTION LIKE @WORD))";
                    paramarr = new MySqlParameter[2];
                    paramarr[0] = new MySqlParameter("@page", (page - 1) * 100);
                    paramarr[1] = new MySqlParameter("@word", "%" + word + "%");
                    total = Convert.ToInt32(MySqlHelper.ExecuteScalar(sql, CommandType.Text, paramarr));
                    sql = "SELECT DISTINCT ID, TYPE, CODE, TITLE, TAGS, STARS, NAME, FNAME FROM MOVIES WHERE TITLE LIKE @WORD OR CODE LIKE @WORD OR ID IN (SELECT MID FROM TAGSMAP WHERE TID IN (SELECT ID FROM TAGS WHERE CAPTION LIKE @WORD)) ORDER BY NAME LIMIT @PAGE, 100";
                }
                else
                {
                    sql = "SELECT DISTINCT COUNT(*) FROM MOVIES WHERE TYPE = @TYPE AND (TITLE LIKE @WORD OR CODE LIKE @WORD OR ID IN (SELECT MID FROM TAGSMAP WHERE TID IN (SELECT ID FROM TAGS WHERE CAPTION LIKE @WORD)))";
                    paramarr = new MySqlParameter[3];
                    paramarr[0] = new MySqlParameter("@type", mtype);
                    paramarr[1] = new MySqlParameter("@page", (page - 1) * 100);
                    paramarr[2] = new MySqlParameter("@word", "%" + word + "%");
                    total = Convert.ToInt32(MySqlHelper.ExecuteScalar(sql, CommandType.Text, paramarr));
                    sql = "SELECT DISTINCT ID, TYPE, CODE, TITLE, TAGS, STARS, NAME, FNAME FROM MOVIES WHERE TYPE = @TYPE AND (TITLE LIKE @WORD OR CODE LIKE @WORD OR ID IN (SELECT MID FROM TAGSMAP WHERE TID IN (SELECT ID FROM TAGS WHERE CAPTION LIKE @WORD))) ORDER BY NAME LIMIT @PAGE, 100";
                }


                var reader = MySqlHelper.ExecuteReader(sql, CommandType.Text, paramarr);
                SyncContext.Send(SetStatus, "正在查询数据...");


                while (reader.Read())
                {
                    int mid = reader.GetInt32(reader.GetOrdinal("id"));
                    int type = reader.GetInt32(reader.GetOrdinal("type"));
                    string code = reader.GetString(reader.GetOrdinal("code"));
                    string title = reader.GetString(reader.GetOrdinal("title"));
                    var mtd = GetMovieTags(mid);
                    var tags = mtd.Where(m => m.type == 0).Select(m => m.caption).ToList<string>();
                    var customtags = mtd.Where(m => m.type == 1).Select(m => m.caption).ToList<string>();
                    var stars = mtd.Where(m => m.type == 2).Select(m => m.caption).ToList<string>();
                    string name = reader.GetString(reader.GetOrdinal("name"));
                    string fname = reader.GetString(reader.GetOrdinal("fname"));
                    var cover = GetMovieCover(mid);
                    SyncContext.Send(AddImageList, new ImageData(name, Image.FromStream(cover)));
                    ListViewItem item = new ListViewItem(name);
                    item.ImageIndex = imageList.Images.IndexOfKey(name);
                    ListItemTag tag = new ListItemTag();
                    tag.id = mid;
                    tag.type = type;
                    tag.code = code;
                    tag.fullname = fname;
                    tag.title = title;
                    tag.tags = mtd;
                    item.Tag = tag;
                    LvList.Add(item);
                    var md = new Movie(fname);
                    md.id = mid; md.type = type; md.code = code; md.title = title; md.tags = tags; md.customtags = customtags; md.stars = stars; md.name = name; md.fname = fname; md.cover = cover;
                    MovieList.Add(md);
                    ++count;

                }

                reader.Close();




                if (page <= 1)
                    bback = 0;

                if (total <= (page * 100))
                    bnext = 0;

                SyncContext.Send(AddListItem, LvList.ToArray());
                SyncContext.Send(SetBackNext, (bback | bnext));
                SyncContext.Send(SetStatus, string.Format("共查询到{0}个对象", count));
                SyncContext.Send(LockUI, false);

            });

            ttask.Start();

        }



        public void FixMoviesData()
        {
            string sql = "SELECT ID, TYPE, CODE, TITLE, TAGS, STARS, NAME, FNAME FROM MOVIES";
            var reader = MySqlHelper.ExecuteReader(sql, CommandType.Text, null);


            while (reader.Read())
            {
                int mid = reader.GetInt32(reader.GetOrdinal("id"));
                int type = reader.GetInt32(reader.GetOrdinal("type"));
                string code = reader.GetString(reader.GetOrdinal("code"));
                string title = reader.GetString(reader.GetOrdinal("title"));
                string name = reader.GetString(reader.GetOrdinal("name"));
                string fname = reader.GetString(reader.GetOrdinal("fname"));


                List<string> tags = new List<string>();
   
                if (!reader.IsDBNull(reader.GetOrdinal("tags")))
                    tags = reader.GetString(reader.GetOrdinal("tags")).Split('|').ToList();


                List<string> stars = new List<string>();

                if (!reader.IsDBNull(reader.GetOrdinal("stars")))
                    stars = reader.GetString(reader.GetOrdinal("stars")).Split('|').ToList();


                foreach (var t in tags)
                {
                    int tid = CreateTag(0, t);
                    var i = CreateTagsMap(mid, tid);
                }

                foreach (var s in stars)
                {
                    int sid = CreateTag(2, s);
                    var i = CreateTagsMap(mid, sid);
                }

            }

            reader.Close();
        }





        public MemoryStream GetMovieCover(int mid)
        {
            string sql = "SELECT COVER FROM MOVIES WHERE ID = @ID";
            MemoryStream cover = new MemoryStream();
            var r = MySqlHelper.ExecuteReader(sql, CommandType.Text, new MySqlParameter("@id", mid));

            while(r.Read())
            {

                long length = r.GetBytes(r.GetOrdinal("cover"), 0, null, 0, 0);


                if (length == 0)
                {
                    cover = new MemoryStream(Convert.FromBase64String(nopic));
                }
                else
                {
                    var buffer = new byte[length];
                    length = r.GetBytes(r.GetOrdinal("cover"), 0, buffer, 0, (int)length);
                    cover.Write(buffer, 0, (int)length);

                }

            }


            r.Close();
            return cover;
        }


        public int CheckTag(int type, string tag)
        {
            string sql = "SELECT * FROM TAGS WHERE TYPE = @TYPE AND (NAME = @NAME OR CAPTION = @NAME)";
            int result = 0;

            MySqlParameter[] paramarr = { new MySqlParameter("@type", type), new MySqlParameter("@name", tag) };
            var r = MySqlHelper.ExecuteScalar(sql, CommandType.Text, paramarr);

            if (r != null)
                result = (Int32)r;


            return result;
        }



        public int CreateTag(int type, string tag)
        {

            var tid = CheckTag(type, tag);

            if (tid > 0)
                return tid;


            string sql = "INSERT INTO TAGS (TYPE, NAME, CAPTION) VALUES (@TYPE, @NAME, @NAME)";
            MySqlParameter[] paramarr = { new MySqlParameter("@type", type), new MySqlParameter("@name", tag) };
            MySqlHelper.ExecuteNonQuery(sql, CommandType.Text, paramarr);
            return (Int32)MySqlHelper.LastId;
        }

        public bool CheckTagsMap(int mid, int tid)
        {
            string sql = "SELECT * FROM TAGSMAP WHERE MID = @mid AND TID = @tid";
            MySqlParameter[] paramarr = { new MySqlParameter("@mid", mid), new MySqlParameter("@tid", tid) };
            var r = MySqlHelper.ExecuteScalar(sql, CommandType.Text, paramarr);
            return r == null;
        }


        public int CreateTagsMap(int mid, int tid)
        {
            if (CheckTagsMap(mid, tid))
            {
                string sql = "INSERT INTO TAGSMAP (MID, TID) VALUES (@mid, @tid)";
                MySqlParameter[] paramarr = { new MySqlParameter("@mid", mid), new MySqlParameter("@tid", tid) };
                return MySqlHelper.ExecuteNonQuery(sql, CommandType.Text, paramarr);
            }

            return 0;
        }




        public TagData[] GetMovieTags(int mid)
        {

            string sql = "SELECT TAGS.TYPE, TAGS.NAME, TAGS.CAPTION FROM (TAGS LEFT JOIN TAGSMAP ON TAGS.ID = TAGSMAP.TID) INNER JOIN MOVIES ON MOVIES.ID = TAGSMAP.MID WHERE MOVIES.ID = @ID";
            MySqlParameter[] paramarr = { new MySqlParameter("@id", mid) };
            var r = MySqlHelper.ExecuteReader(sql, CommandType.Text, paramarr);
            List<TagData> result = new List<TagData>();

            while (r.Read())
            {

                TagData t = new TagData();
                t.type = r.GetInt32(r.GetOrdinal("TYPE"));
                t.name = r.GetString(r.GetOrdinal("NAME"));
                t.caption = r.GetString(r.GetOrdinal("CAPTION"));
                result.Add(t);
            }

            r.Close();
            return result.ToArray();
        }



        public static bool CheckMovie(string name)
        {
            string sql = "SELECT * FROM MOVIES WHERE NAME = @name";
            MySqlParameter[] paramarr = { new MySqlParameter("@name", name) };
            var r = MySqlHelper.ExecuteScalar(sql, CommandType.Text, paramarr);
            return r == null;
        }


        public string[] GetNullMovies()
        {
            string sql = "SELECT * FROM MOVIES WHERE TITLE = '暂无'";
            var r = MySqlHelper.ExecuteReader(sql, CommandType.Text, null);
            List<string> result = new List<string>();

            while (r.Read())
            {
                result.Add(r.GetString(r.GetOrdinal("FNAME")));
            }

            r.Close();
            return result.ToArray();
        }


        public void UpdateMovie(Movie md)
        {

            string sql = "DELETE FROM TAGSMAP WHERE MID = @ID";
            MySqlHelper.ExecuteNonQuery(sql, CommandType.Text, new MySqlParameter("@id", md.id));

            foreach (var t in md.tags)
            {
                int tid = CreateTag(0, t);
                var i = CreateTagsMap(md.id, tid);
            }

            foreach (var t in md.customtags)
            {
                int tid = CreateTag(1, t);
                var i = CreateTagsMap(md.id, tid);
            }

            foreach (var s in md.stars)
            {
                int sid = CreateTag(2, s);
                var i = CreateTagsMap(md.id, sid);
            }


            sql = "UPDATE MOVIES SET CODE = @code, TITLE = @title, TAGS = @tags, STARS = @stars, COVER = @cover WHERE NAME = @name";
            MySqlParameter cover = new MySqlParameter();
            cover.ParameterName = "@cover";
            cover.MySqlDbType = MySqlDbType.VarBinary;
            cover.Value = md.cover.ToArray();
            MySqlParameter[] paramarr = new MySqlParameter[6];
            paramarr[0] = new MySqlParameter("@code", md.code);
            paramarr[1] = new MySqlParameter("@title", md.title);
            paramarr[2] = new MySqlParameter("@tags", string.Join("|", md.tags));
            paramarr[3] = new MySqlParameter("@stars", string.Join("|", md.stars));
            paramarr[4] = cover;
            paramarr[5] = new MySqlParameter("@name", md.name);
            MySqlHelper.ExecuteNonQuery(sql, CommandType.Text, paramarr);
        }





        public void DeleteMovie(int id)
        {
            string sql = "DELETE MOVIES, TAGSMAP FROM MOVIES LEFT JOIN TAGSMAP ON MOVIES.ID = TAGSMAP.MID WHERE MOVIES.ID = @ID";
            MySqlHelper.ExecuteNonQuery(sql, CommandType.Text, new MySqlParameter("@id", id));
        }



        public static void SaveMovies()
        {

            lock (MovieList)
            {
                Hashtable hs = new Hashtable();
                string sql = "INSERT INTO MOVIES (TYPE, CODE, TITLE, TAGS, STARS, COVER, NAME, FNAME) VALUES (@type, @code, @title, @tags, @stars, @cover, @name, @fname)";

                foreach (var m in MovieList)
                {
                    MySqlParameter cover = new MySqlParameter();
                    cover.ParameterName = "@cover";
                    cover.MySqlDbType = MySqlDbType.VarBinary;
                    cover.Value = m.cover.ToArray();
                    MySqlParameter[] paramarr = new MySqlParameter[8];
                    paramarr[0] = new MySqlParameter("@type", m.type);
                    paramarr[1] = new MySqlParameter("@code", m.code);
                    paramarr[2] = new MySqlParameter("@title", m.title);
                    paramarr[3] = new MySqlParameter("@tags", string.Join("|", m.tags));
                    paramarr[4] = new MySqlParameter("@stars", string.Join("|", m.stars));
                    paramarr[5] = cover;
                    paramarr[6] = new MySqlParameter("@name", m.name);
                    paramarr[7] = new MySqlParameter("@fname", m.fname);
                    hs.Add(m.fname, paramarr);
                }

                MySqlHelper.ExecuteNonQuery(sql, hs);
            }


        }




        public Movie DownloadMovieData(string fname)
        {
            Movie md = new Movie(fname);
            md.code = GetCid(md.name);

            if (md.code == null)
            {
                md.code = md.name;
                return md;
            }

            try
            {
                hparam.url = dmmdataurl + md.code + "/";
                hparam.postdata = "";

                if (CTRHttp() == false)
                {
                    return md;
                }

                string result = hresp.respstr;
                HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
                html.LoadHtml(result);


                HtmlNode tnode = html.DocumentNode.SelectSingleNode("//*[@id=\"title\"]");
                md.title = tnode.InnerText;
                tnode = html.DocumentNode.SelectSingleNode("//*[@name=\"package-image\"]");


                HtmlAttribute it = tnode.Attributes["href"];
                md.cover = DownLoad(it.Value);

                if (md.cover == null)
                    md.cover = new MemoryStream();




                tnode = tnode.SelectSingleNode("//*[@id=\"mu\"]");
                HtmlNodeCollection glist = tnode.SelectNodes(".//a[contains(@href,'/mono/dvd/-/list/=/article=keyword/')]");
                List<string> tags = new List<string>();

                if (glist != null)
                {
                    foreach (HtmlNode tt in glist)
                    {
                        tags.Add(tt.InnerText);
                    }

                    md.tags = tags;
                }



                HtmlNodeCollection slist = tnode.SelectNodes("//*[@id=\"performer\"]/a");
                List<string> star = new List<string>();

                if (slist != null)
                {
                    foreach (HtmlNode tt in slist)
                    {
                        star.Add(tt.InnerText);
                    }



                    md.stars = star;
                }



                return md;

            }
            catch (Exception)
            {
                return md;
            }



        }

        public bool CTRHttp()
        {


            try
            {

                string url = hparam.url;
                string postdata = hparam.postdata;
                HttpWebResponse resp = null;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.AllowAutoRedirect = false;
                req.Accept = "*/*";
                req.Headers["Accept-Language"] = "zh-CN/en-US";
                req.KeepAlive = true;
                const string gUserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1700.107 Safari/537.36";
                req.UserAgent = gUserAgent;
                req.Headers["Accept-Encoding"] = "gzip, deflate";
                req.AutomaticDecompression = DecompressionMethods.GZip;

                WebProxy proxyObject = new WebProxy("127.0.0.1", 10809);
                req.Proxy = proxyObject;

                req.ServicePoint.Expect100Continue = false;
                req.Timeout = 5000;
                req.ReadWriteTimeout = 5000;
                req.CookieContainer = coa;

                if (postdata != "")
                {
                    req.Method = "POST";
                    req.ContentType = "application/x-www-form-urlencoded";
                    byte[] postBytes = Encoding.UTF8.GetBytes(postdata);
                    req.ContentLength = postBytes.Length;
                    Stream postDataStream = req.GetRequestStream();
                    postDataStream.Write(postBytes, 0, postBytes.Length);
                    postDataStream.Close();
                }
                else
                {
                    req.Method = "GET";
                }

                resp = (HttpWebResponse)req.GetResponse();



                var stream = resp.GetResponseStream();
                var sr = new StreamReader(stream, Encoding.GetEncoding("UTF-8"));
                string html = sr.ReadToEnd();

                if (html.Trim() == "")
                {
                    //OnPostErrorEvent();
                    return false;
                }

                hresp.respstr = html;
                return true;
            }
            catch (WebException)
            {
                //OnPostErrorEvent();
                return false;
            }

        }


        public MemoryStream DownLoad(string url)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Accept = "image/png,image/*;q=0.8,*/*;q=0.5";
                httpWebRequest.Referer = url;
                httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1700.107 Safari/537.36";
                httpWebRequest.Method = "Get";
                httpWebRequest.AllowAutoRedirect = false;
                httpWebRequest.KeepAlive = true;
                httpWebRequest.Headers["Accept-Encoding"] = "gzip, deflate";
                httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip;
                httpWebRequest.ServicePoint.Expect100Continue = false;
                httpWebRequest.Timeout = 5000;
                httpWebRequest.ReadWriteTimeout = 5000;

                WebProxy proxyObject = new WebProxy("127.0.0.1", 10809);
                httpWebRequest.Proxy = proxyObject;

                var webResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var getStream = webResponse.GetResponseStream())
                {
                    int num;
                    var ms = new MemoryStream();

                    byte[] buffer = new byte[0x400];

                    do
                    {
                        num = getStream.Read(buffer, 0, buffer.Length);

                        if (num > 0)
                        {
                            ms.Write(buffer, 0, num);
                        }



                    } while (num > 0);

                    return ms;

                }
            }
            catch (Exception)
            {
                return null;
            }


        }


        private void ExplorerForm_Resize(object sender, EventArgs e)
        {

            MainForm ef = sender as MainForm;
            panelsearch.Width = ef.Width - 152;
            panelv.Width = ef.Width - 152;
            panelv.Height = ef.Height - 120;
            paneltv.Height = ef.Height - 120;

        }


        private void lvfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            //
        }
        private void lvfile_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvFile.SelectedIndices.Count == 0)
                return;

            try
            {
                ListItemTag tag = (ListItemTag)lvFile.SelectedItems[0].Tag;
                System.Diagnostics.Process.Start(tag.fullname);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }




        public string GetCid(string name)
        {
            name = Path.GetFileNameWithoutExtension(name).ToUpper();

            var na = name.Split('-');
            string result = null;

            if (na.Length < 2)
            {
                return result;
            }

            result = na[0] + na[1];

            switch (na[0])
            {
                case "DANDY":
                case "FSDSS":
                case "GS":
                case "KMHR":
                case "RCTD":
                case "SDDE":
                case "SDJS":
                case "SDMU":
                case "STAR":
                case "STARS":
                case "SW":
                case "FAA":
                case "FSET":
                case "HBAD":
                case "HD":
                case "IDOL":
                case "IENE":
                case "IESP":
                case "IFDVA":
                case "MIST":
                case "NHDT":
                case "OKP":
                case "PIYO":
                case "SDABP":
                case "SDAM":
                case "SDDM":
                case "SDEN":
                case "SDMT":
                case "SVDVD":

                    result = "1" + result;
                    break;
                case "DFE":
                case "EKW":
                case "GOD":
                    result = "2" + result;
                    break;
                case "PPS":
                case "WANZ":
                    result = "3" + result;
                    break;
                case "PMP":
                    result = "5" + result;
                    break;
                case "BUR":
                case "HDI":
                case "IGN":
                case "LOL":
                case "MMD":
                case "NIN":
                case "SCR":
                    result = "12" + result;
                    break;
                case "GVG":
                    result = "13" + result;
                    break;
                case "BNYD":
                    result = "15" + result;
                    break;
                case "CEMN":
                    result = "18" + result;
                    break;
                case "BUD":
                case "BLD":
                case "HFD":
                case "AVGP":
                case "CGD":
                case "LID":
                    result = "24" + result;
                    break;
                case "GXAZ":
                case "HXAD":
                    result = "29" + result;
                    break;
                case "HODV":
                case "HMPD":
                    result = "41" + result;
                    break;
                case "VSPDS":
                    result = "42" + result;
                    break;
                case "ADZ":
                    result = "47" + result;
                    break;
                case "ODVHJ":
                    result = "48" + result;
                    break;
                case "EKDV":
                case "GESU":
                case "MADM":
                case "NITR":
                case "SON":
                    result = "49" + result;
                    break;
                case "DV":
                    result = "53" + result;
                    break;


                case "ID13":
                case "ID14":
                case "ID15":
                case "ID16":
                case "ID17":
                case "ID18":
                case "ID19":
                case "ID20":
                case "ID21":
                case "ID22":
                case "ID23":
                case "ID24":
                case "25ID":
                case "26ID":
                case "27ID":
                case "28ID":
                case "CSCT":
                case "SAIT":
                case "T28":
                case "HITMA":
                case "AKB":
                case "DAPD":
                case "DAVK":
                case "GRO":
                case "NKNO":
                case "PTNOZ":
                case "TKRO":
                case "TPRO":

                    result = "55" + result;
                    break;
                case "JKSR":
                case "BDSR":
                case "EIKI":
                    result = "57" + result;
                    break;
                case "SRXV":
                case "XV":
                    result = "60" + result;
                    break;
                case "NOV":
                case "SAK":
                    result = "66" + result;
                    break;

                case "SMA":
                    result = "83" + result;
                    break;

                case "BAZX":
                case "MDBK":
                case "MDS":
                case "MDTM":
                case "MKMP":
                case "XRW":
                case "MDB":
                case "OKAX":
                case "ONGP":
                case "REAL":
                case "SCOP":
                case "SCPX":
                    result = "84" + result;
                    break;

                case "ABP":
                case "CPDE":
                case "DAC":
                case "DNW":
                case "DOCP":
                case "DTT":
                case "GIRO":
                case "GZAP":
                case "KFNE":
                case "ONEZ":
                case "SIM":
                case "SKSK":
                case "SRS":
                case "WAT":
                case "AKA":
                case "AVOP":
                case "DKN":
                case "GDW":
                case "HAR":
                case "KBI":
                case "MCT":
                case "ONET":
                case "PPA":
                case "RTP":
                case "SGA":
                case "ULT":


                    result = "118" + result;
                    break;
                case "DCO":
                case "DPB":
                    result = "189" + result;
                    break;
                case "IBW":
                    result = "504" + result;
                    break;
                case "MRXD":
                    result = "837" + result;
                    break;
                case "PTS":
                    result = "h_021" + result;
                    break;
                case "MXGS":
                case "MXPC":
                    result = "h_068" + result;
                    break;
                case "KTDS":
                case "KTR":
                case "KTRA":
                    result = "h_094" + result;
                    break;
                case "BBACOS":
                case "SO":
                    result = "h_113" + result;
                    break;
                case "YAL":
                case "YSN":
                    result = "h_127" + result;
                    break;
                case "DIC":
                    result = "h_139" + result;
                    break;
                case "UPSM":
                    result = "h_150" + result;
                    break;
                case "GHKO":
                    result = "h_173" + result;
                    break;
                case "NFDM":
                    result = "h_188" + result;
                    break;
                case "JUTN":
                    result = "h_227" + result;
                    break;
                case "EMOT":
                case "AMBI":
                case "NACR":
                    result = "h_237" + result;
                    break;
                case "SABA":
                case "SUPA":
                    result = "h_244" + result;
                    break;
                case "GHAT":
                    result = "h_254" + result;
                    break;
                case "TKI":
                    result = "h_286" + result;
                    break;
                case "FONE":
                case "FSKT":
                case "FSRE":
                case "FSTC":
                case "FSTE":
                case "LOVE":
                    result = "h_491" + result;
                    break;
                case "YJM":
                    result = "h_533" + result;
                    break;
                case "URSH":
                    result = "h_593" + result;
                    break;
                case "DLIS":
                case "SHM":
                    result = "h_687" + result;
                    break;
                case "ZEX":
                    result = "h_720" + result;
                    break;
                case "MOT":
                    result = "h_796" + result;
                    break;
                case "ZIZG":
                    result = "h_826" + result;
                    break;
                case "MLP":
                    result = "h_848" + result;
                    break;
                case "DIY":
                    result = "h_900" + result;
                    break;
                case "VRTM":
                    result = "h_910" + result;
                    break;
                case "MOC":
                    result = "h_924" + result;
                    break;
                case "ARBB":
                    result = "h_1092" + result;
                    break;
                case "OZUKE":
                    result = "h_1103" + result;
                    break;
                case "BSTC":
                    result = "h_1117" + result;
                    break;
                case "HONB":
                case "GDJU":
                case "POCO0":
                    result = "h_1133" + result;
                    break;
                case "MILK":
                    result = "h_1240" + result;
                    break;
                case "INTY":
                    result = "h_1257" + result;
                    break;
                case "SKMJ":
                    result = "h_1324" + result;
                    break;
                case "OMT":
                    result = "h_1359" + result;
                    break;
                case "ZMEN":
                    result = "h_1371" + result;
                    break;
                case "TDC":
                    result = "h_1418" + result;
                    break;
                case "SEMC":
                    result = "h_1422" + result;
                    break;
                case "FGAN":
                    result = "h_1440" + result;
                    break;
            }


            return result;

        }


        private void menuItem2_Click(object sender, EventArgs e)
        {
            //var md = GetMovieData("pppd00255");
            //MessageBox.Show(md.star);




            FileCache.Clear();
            SyncContext.Send(SetStatus, "正在扫描本地文件...");


            CacheDataTask = new Task(() =>
            {


                lock (FileCache)
                {
                    var cache = Everything.GetFiles("AV\\1\\");
                    FileCache = cache.Where(f => (f.FileName.ToUpper().Contains(".MP4") || f.FileName.ToUpper().Contains(".MKV") || f.FileName.ToUpper().Contains(".AVI") || f.FileName.ToUpper().Contains(".WMV"))).OrderBy(f => f.FileName).ToList();

                }

                SyncContext.Send(SetStatus, string.Format("扫描完成,已缓存{0}个文件", FileCache.Count));



            });


            CacheDataTask.Start();




            //CacheDataTask = new Task(() => CacheData(0, 1, ""), TaskCreationOptions.LongRunning);
            //CacheDataTask.Start();
        }

        private void btnback_Click(object sender, EventArgs e)
        {
            GetMoviesList(searchtype, searchpage - 1, searchword);
        }

        private void btnnext_Click(object sender, EventArgs e)
        {
            GetMoviesList(searchtype, searchpage + 1, searchword);
        }

        private void tbxpath_TextChanged(object sender, EventArgs e)
        {

        }

        private void lvFile_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {

            ToolTip tt = new ToolTip();
            ListItemTag tag = (ListItemTag)e.Item.Tag;
            var fname = Path.GetFileName(tag.fullname);
            var cover = imageList.Images[imageList.Images.IndexOfKey(fname)];
            string tip = "";


            foreach (var t in tag.tags)
            {
                tip += t.caption + " ";
            }

            tt.UseFading = false;
            tt.AutoPopDelay = 5000;
            tt.InitialDelay = 0;
            tt.ReshowDelay = 0;
            tt.SetToolTip((e.Item).ListView, tip);


        }

        private void tvtags_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            tbxsearch.Text = e.Node.Text;

            Keys k = new Keys();
            k = Keys.Enter;

            var ke = new KeyEventArgs(k);
            tbxsearch_KeyPress(sender, new KeyPressEventArgs('\r'));
        }

        private void tbxsearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                GetMoviesList(searchtype, 1, tbxsearch.Text);
            }
        }

        private void miopen_Click(object sender, EventArgs e)
        {
            if (lvFile.SelectedIndices.Count == 0)
                return;

            try
            {
                ListItemTag tag = (ListItemTag)lvFile.SelectedItems[0].Tag;
                System.Diagnostics.Process.Start(tag.fullname);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void miopenpath_Click(object sender, EventArgs e)
        {
            if (lvFile.SelectedIndices.Count == 0)
                return;

            try
            {
                ListItemTag tag = (ListItemTag)lvFile.SelectedItems[0].Tag;
                System.Diagnostics.Process.Start("Explorer", "/select," + tag.fullname);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void micopycid_Click(object sender, EventArgs e)
        {
            if (lvFile.SelectedIndices.Count == 0)
                return;

            try
            {
                ListItemTag tag = (ListItemTag)lvFile.SelectedItems[0].Tag;
                Clipboard.SetText(tag.code);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void midelete_Click(object sender, EventArgs e)
        {
            if (lvFile.SelectedIndices.Count == 0)
                return;

            try
            {

                if (MessageBox.Show(this, "确定要永久删除此文件和数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    ListItemTag tag = (ListItemTag)lvFile.SelectedItems[0].Tag;

                    if (File.Exists(tag.fullname))
                    {
                        File.Delete(tag.fullname);
                    }

                    DeleteMovie(tag.id);
                    GetMoviesList(searchtype, searchpage, searchword);
                }





            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void miedit_Click(object sender, EventArgs e)
        {
            if (lvFile.SelectedIndices.Count == 0)
                return;

            try
            {
                ListItemTag tag = (ListItemTag)lvFile.SelectedItems[0].Tag;
                var md = MovieList.Where(m => m.fname == tag.fullname).FirstOrDefault();
                var ef = new EditForm(this, md);
                ef.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {

            if (CacheDataTask != null && (CacheDataTask.Status == TaskStatus.Running))
            {
                MessageBox.Show(this, "请等待文件扫描完成后执行！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            SyncContext.Send(SetStatus, "开始缓存文件...");
            MovieList.Clear();
            imageList.Images.Clear();
            lvFile.Clear();



            SaveDataTask = new Task(() =>
            {
                int pcount = 0;

                foreach (var f in FileCache)
                {

                    if (CheckMovie(f.FileName))
                    {
                        var md = DownloadMovieData(f.FullPathName);
                        md.type = 1;
                        MovieList.Add(md);
                        SyncContext.Send(SetStatus, string.Format("正在处理文件{0}...", md.fname));
                    }


                    pcount += 1;
                    int percent = Convert.ToInt32(((double)pcount / (double)FileCache.Count) * 100);
                    SyncContext.Send(SetProgress, percent);


                }

                SyncContext.Send(SetStatus, "正在存储数据");
                SaveMovies();
                GetMoviesList(1, 1, "");
                FixMoviesData();
                SyncContext.Send(SetStatus, "缓存完成");
            });



            SaveDataTask.Start();



        }

        private void menuItem4_Click(object sender, EventArgs e)
        {

            var namelist = GetNullMovies();

            SyncContext.Send(SetStatus, "正在修复数据");

            UpdateDataTask = new Task(() =>
            {
                int pcount = 0;

                foreach (var n in namelist)
                {
                    SyncContext.Send(SetStatus, string.Format("正在处理数据{0}...", Path.GetFileName(n)));
                    var md = DownloadMovieData(n);
                    UpdateMovie(md);
                    pcount += 1;
                    int percent = Convert.ToInt32(((double)pcount / (double)namelist.Length) * 100);
                    SyncContext.Send(SetProgress, percent);
                }


                FixMoviesData();
                SyncContext.Send(SetStatus, "修复完成");
            });

            UpdateDataTask.Start();



        }




    }
}
