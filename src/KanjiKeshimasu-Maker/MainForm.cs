using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Reflection;

namespace KanjiKeshimasu_Maker
{
    public partial class MainForm : Form
    {
        public static string Appli_Title = "漢字ケシマス発生器";
        public static List<Moji> KeshiMasu = new List<Moji>();
        public static List<Moji> MojiDabese = new List<Moji>();
        public static int KeshiMasuCnt = 0;
        public static string KanjiCSV_Filename = Application.StartupPath + "\\KanjiDataMoji.csv";

        private Label[,] lblKeshiText = new Label[6, 4];
        private Label[,] lblAnsweText = new Label[6, 4];

        public MainForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            lblKeshiText[0, 0] = lbl00;
            lblKeshiText[0, 1] = lbl01;
            lblKeshiText[0, 2] = lbl02;
            lblKeshiText[0, 3] = lbl03;
            lblKeshiText[1, 0] = lbl10;
            lblKeshiText[1, 1] = lbl11;
            lblKeshiText[1, 2] = lbl12;
            lblKeshiText[1, 3] = lbl13;
            lblKeshiText[2, 0] = lbl20;
            lblKeshiText[2, 1] = lbl21;
            lblKeshiText[2, 2] = lbl22;
            lblKeshiText[2, 3] = lbl23;
            lblKeshiText[3, 0] = lbl30;
            lblKeshiText[3, 1] = lbl31;
            lblKeshiText[3, 2] = lbl32;
            lblKeshiText[3, 3] = lbl33;
            lblKeshiText[4, 0] = lbl40;
            lblKeshiText[4, 1] = lbl41;
            lblKeshiText[4, 2] = lbl42;
            lblKeshiText[4, 3] = lbl43;
            lblKeshiText[5, 0] = lbl50;
            lblKeshiText[5, 1] = lbl51;
            lblKeshiText[5, 2] = lbl52;
            lblKeshiText[5, 3] = lbl53;

            lblAnsweText[0, 0] = lbla00;
            lblAnsweText[0, 1] = lbla01;
            lblAnsweText[0, 2] = lbla02;
            lblAnsweText[0, 3] = lbla03;
            lblAnsweText[1, 0] = lbla10;
            lblAnsweText[1, 1] = lbla11;
            lblAnsweText[1, 2] = lbla12;
            lblAnsweText[1, 3] = lbla13;
            lblAnsweText[2, 0] = lbla20;
            lblAnsweText[2, 1] = lbla21;
            lblAnsweText[2, 2] = lbla22;
            lblAnsweText[2, 3] = lbla23;
            lblAnsweText[3, 0] = lbla30;
            lblAnsweText[3, 1] = lbla31;
            lblAnsweText[3, 2] = lbla32;
            lblAnsweText[3, 3] = lbla33;
            lblAnsweText[4, 0] = lbla40;
            lblAnsweText[4, 1] = lbla41;
            lblAnsweText[4, 2] = lbla42;
            lblAnsweText[4, 3] = lbla43;
            lblAnsweText[5, 0] = lbla50;
            lblAnsweText[5, 1] = lbla51;
            lblAnsweText[5, 2] = lbla52;
            lblAnsweText[5, 3] = lbla53;

            this.AllowDrop = true;
            foreach (Label lb in lblKeshiText)
            {
                lb.AllowDrop = true;
                lb.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
                lb.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            }
            foreach (Label lb in lblAnsweText)
            {
                lb.AllowDrop = true;
                lb.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
                lb.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            }

            //文字データを読み込みます
            if (!ReadKanjiData(KanjiCSV_Filename))
            {
                MessageBox.Show(Path.GetFileName(KanjiCSV_Filename) + " ファイルを読み込めませんでした", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnMake.Enabled = false;
            }
            else
            {
                SetMainFormTitle(Path.GetFileName(KanjiCSV_Filename), true);
            }

            //ケシマスに漢字を書きます
            DispKeshimasu(null, 0);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// メインフォームのタイトルをセットします
        /// </summary>
        /// <param name="filename">表示するファイル名</param>
        /// <param name="buildpartFlg">ビルド情報を表示するかどうか</param>
        private void SetMainFormTitle(string filename, bool buildpartFlg, int done = -1, int notdone = -1)
        {
            //自分自身のバージョン情報を取得する
            FileVersionInfo ver = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

            if (filename != "")
            {
                filename = " | " + filename;
            }

            // タスクの表示
            string tasks = "";
            if (done != -1 && notdone != -1)
            {
                tasks = string.Concat(done.ToString(), " / ", notdone.ToString());
            }

            //タイトルの表示
            //this.Text = string.Concat(ValiableList.MainFormTtle, " Ver ", ver.FileMajorPart, ".", ver.FileMinorPart, ".", ver.FileBuildPart, " - ", filename);
            if (buildpartFlg == true)
            {
                this.Text = string.Concat(Appli_Title, " Ver ", ver.FileMajorPart, ".", ver.FileMinorPart.ToString(), ".", ver.FileBuildPart.ToString(), filename, tasks);
            }
            else
            {
                this.Text = string.Concat(Appli_Title, " Ver ", ver.FileMajorPart, ".", ver.FileMinorPart.ToString(), filename, tasks);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (Label lb in lblKeshiText)
            {
                lb.DragDrop -= new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
                lb.DragEnter -= new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            }
            foreach (Label lb in lblAnsweText)
            {
                lb.DragDrop -= new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
                lb.DragEnter -= new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            }
        }

        /// <summary>
        /// ケシマス漢字を発生します
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMake_Click(object sender, EventArgs e)
        {
            MakeKeshimasu();
        }

        private void DispKeshimasu(Moji[,] screen, int total)
        {
            string yomi = "";
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (screen == null)
                    {
                        this.lblKeshiText[y, x].Text = "";
                        this.lblAnsweText[y, x].Text = "";
                        this.lblYomi.Text = "";
                    }
                    else
                    {
                        this.lblKeshiText[y, x].Text = screen[y, x].Kanji;
                        this.lblKeshiText[y, x].BackColor = screen[y, x].BkColor;

                        if (screen[y, x].Kanji == "")
                        {
                            this.lblAnsweText[y, x].Text = "";
                        }
                        else
                        {
                            string ans = "";
                            int cnt = total - screen[y, x].AnsNum;

                            if (cnt >= 10)
                            {
                                char bar = (char)(65 + cnt - 10);
                                ans = bar.ToString();
                            }
                            else
                            {
                                ans = cnt.ToString("0");
                            }
                            this.lblAnsweText[y, x].Text = ans;

                            if (ans == "1")
                            {
                                yomi = screen[y, x].Yomi;
                            }
                        }
                        this.lblAnsweText[y, x].BackColor = screen[y, x].BkColor;
                    }
                }
            }

            if (screen == null)
            {
                this.lblYomi.Text = "";
            }
            else
            {
                this.lblYomi.Text = yomi;
            }

            if (chkColorOFF.Checked)
            {
                foreach (Label lb in lblKeshiText)
                {
                    lb.BackColor = Color.Transparent;
                }
            }          
        }

        /// <summary>
        /// 一様乱数を取得する
        /// </summary>
        /// <returns>乱数:-1～+1</returns>
        public double GetRandom()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] bs = new byte[sizeof(Int32)];
            rng.GetBytes(bs);
            int iR = System.BitConverter.ToInt32(bs, 0);
            return ((double)iR / Int32.MaxValue);
        }
        public UInt32 GetRandom(UInt32 val)
        {
            return (UInt32)((GetRandom() + 1.0) / 2.0 * (double)val + 0.5);
        }

        /// <summary>
        /// 漢字データを読み込みます
        /// </summary>
        /// <returns></returns>
        private bool ReadKanjiData(string filename)
        {
            try
            {
                string[] lines = File.ReadAllLines(filename);

                MojiDabese.Clear();

                for (int i = 0; i < lines.Length; i++)
                {
                    string[] strs = lines[i].Split(',');
                    Moji moji = new Moji(strs[0], strs[1]);
                    moji.KanjiNum = i;
                    MojiDabese.Add(moji);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return false;
            }
            return true;
        }

        public Moji[,] SetKanji(List<Moji> dat, int step)
        {
            //漢字を書きます
            Moji[,] screen = new Moji[6, 4];
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    screen[y, x] = new Moji(1, 0, 0, 0);
                }
            }

            Moji kanji = null;
            Moji moji = null;
            for (int i = 0; i < step; i++)
            {
                if (dat == null)
                {
                    moji = KeshiMasu[i];
                }
                else
                {
                    moji = dat[i];
                    int tcn = 100000;
                    while (tcn > 0) //10万回繰り返して、ダメだったらあきらめて重複漢字を使用します
                    {
                        kanji = getKanji(moji.n);
                        //熟語として未使用かどうか確かめます
                        bool ok = true;
                        for (int j = 0; j < i; j++)
                        {
                            if (dat[j].KanjiNum == kanji.KanjiNum)
                            {
                                ok = false;
                                break;
                            }
                        }

                        //使用されている漢字に同じ感じが無いか調べます
                        for (int ky = 0; ky < 6; ky++)
                        {
                            for (int kx = 0; kx < 4; kx++)
                            {
                                Debug.WriteLine(screen[ky, kx].Kanji);

                                if (screen[ky, kx].Kanji != "" && kanji.Kanji.IndexOf(screen[ky, kx].Kanji) >= 0)
                                {
                                    ok = false;
                                    break;
                                }
                            }
                            if (ok == false) { break; }
                        }

                        if (ok == true) { break; }
                        tcn--;
                    }
                    moji.Kanji = kanji.Kanji;
                    moji.Yomi = kanji.Yomi;
                    moji.KanjiNum = kanji.KanjiNum;
                    moji.BkColor = Color.FromArgb((int)GetRandom(255), (int)GetRandom(255), (int)GetRandom(255));
                }

                //keshiデータに追加します
                if (moji.TateYoko == 0)
                {
                    for (int x = moji.X; x < moji.X + moji.n; x++)
                    {
                        for (int y = 1; y < moji.Y + 1; y++)
                        {
                            screen[y - 1, x] = screen[y, x].Clone();
                        }
                        screen[moji.Y, x] = moji.Clone();
                        screen[moji.Y, x].n = 1;
                        screen[moji.Y, x].Kanji = moji.Kanji.Substring(x - moji.X, 1);
                    }
                }
                else
                {
                    for (int y = moji.n; y < moji.Y + moji.n; y++)
                    {
                        screen[y - moji.n, moji.X] = screen[y, moji.X].Clone();
                    }

                    for (int y = 0; y < moji.n; y++)
                    {
                        screen[moji.Y + y, moji.X] = moji.Clone();
                        screen[moji.Y + y, moji.X].n = 1;
                        screen[moji.Y + y, moji.X].Kanji = moji.Kanji.Substring(y, 1);
                    }
                }
            }
            return screen;
        }

        public async void MakeKeshimasu()
        {
            //Kazu[0]=1文字漢字の数、Kazu[1]=2文字漢字の数、Kazu[2]=3文字漢字の数
            int[] kazu = new int[3];
            kazu[0] = (int)nudOneNum.Value;
            kazu[2] = (int)nudThreeNum.Value;
            int k = 24 - kazu[2] * 3 - kazu[0];
            kazu[1] = k / 2;

            if ((k % 2) == 1)
            {
                MessageBox.Show("設定文字数が合いません\r\n三文字の文字数か、一文字の文字数を変えてください", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.btnMake.Enabled = false;
            this.gbxCntrl.Enabled = false;
            this.gbxSetNum.Enabled = false;

            List<Moji> dat = new List<Moji>();
            //スレッド処理します
            await Task.Run(() =>
            {
                dat = Calculate(kazu);
            });

            KeshiMasu = dat;
            KeshiMasuCnt = dat.Count;

            //漢字を書きます
            Moji[,] screen = SetKanji(dat, dat.Count);

            DispKeshimasu(screen, dat.Count);

            this.btnMake.Enabled = true;
            this.gbxCntrl.Enabled = true;
            this.gbxSetNum.Enabled = true;
            this.btnMake.Focus();
        }

        ///// <summary>
        ///// n 個以上の連続する空白の場所を求める
        ///// </summary>
        ///// <param name="dat"></param>
        ///// <param name="n"></param>
        ///// <returns></returns>
        //public List<Moji> getEmptyNum(List<Moji> dat, int num)
        //{
        //    string[,] keshi = new string[6, 4];

        //    //現在のデータをケシマスにめ込みます
        //    for (int i = 0; i < dat.Count; i++)
        //    {
        //        int x, y;
        //        Moji moji = dat[i];
        //        if (moji.TateYoko == 0)
        //        {
        //            for (int j = 0; j < moji.n; j++)
        //            {
        //                x = moji.X + j > 3 ? 3 : moji.X + j;
        //                y = moji.Y > 5 ? 5 : moji.Y;
        //                keshi[y, x] = "1";
        //            }
        //        }
        //        else
        //        {
        //            for (int j = 0; j < moji.n; j++)
        //            {
        //                x = moji.X > 3 ? 3 : moji.X;
        //                y = moji.Y + j > 5 ? 5 : moji.Y + j;
        //                keshi[y, x] = "1";
        //            }
        //        }
        //    }

        //    //n個以上連続する空欄を数えます
        //    List<Moji> empdat = new List<Moji>();
        //    bool ok = true;
        //    //先ずは横方向から
        //    for (int y = 0; y < 6; y++)
        //    {
        //        for (int x = 0; x < 4; x++)
        //        {
        //            ok = true;
        //            for (int i = 0; i < num; i++)
        //            {
        //                if (x + i >= 4)
        //                {
        //                    ok = false;
        //                    break;
        //                }

        //                if (keshi[y, x + i] != null)
        //                {
        //                    ok = false;
        //                    break;
        //                }
        //            }

        //            if (ok == true)
        //            {
        //                empdat.Add(new Moji(num, x, y, 0));
        //            }
        //        }
        //    }

        //    if (num > 1)
        //    {
        //        //次は縦方向を調べます
        //        for (int y = 0; y < 6; y++)
        //        {
        //            for (int x = 0; x < 4; x++)
        //            {
        //                ok = true;
        //                for (int i = 0; i < num; i++)
        //                {
        //                    if (y + i >= 6)
        //                    {
        //                        ok = false;
        //                        break;
        //                    }

        //                    if (keshi[y + i, x] != null)
        //                    {
        //                        ok = false;
        //                        break;
        //                    }
        //                }

        //                if (ok == true)
        //                {
        //                    empdat.Add(new Moji(num, x, y, 1));
        //                }
        //            }
        //        }
        //    }
        //    return empdat;
        //}

        /// <summary>
        /// n 個以上の連続する空白の場所を求める
        /// </summary>
        /// <param name="dat"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public List<Moji> getEmptyPos(int[,] keshi, int num)
        {
            //Debug.WriteLine("-- 文字数: " + num.ToString());

            int sta = 2; //壁で隠されている部分 0-1

            //n個以上連続する空欄を数えます
            List<Moji> empdat = new List<Moji>();
            bool ok = true;
            //先ずは横方向から
            for (int y = sta; y < 6; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    //空中または地面の下かどうか
                    if (y + 1 != 6 && keshi[y + 1, x] == 0) { continue; }

                    //右方向にnum個置けるかどうか
                    ok = true;
                    for (int i = 0; i < num; i++)
                    {
                        int ix = x + i;
                        int iy = y + 1;

                        if (ix >= 4)
                        {
                            ok = false;
                            break;
                        }

                        if (iy == 6)
                        {
                            continue;
                        }
                        else if (keshi[iy, ix] == 0)
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (ok == false) { continue; }

                    //横に置いた後、上の空間があふれていないかどうか
                    ok = true;
                    for (int i = 0; i < num; i++)
                    {
                        int ix = x + i;
                        //X座標の一番上が埋まっていないか
                        if (keshi[0, ix] != 0)
                        {
                            ok = false;
                            break;
                        }
                    }

                    if (ok == true)
                    {
                        //Debug.WriteLine("横: " + x.ToString() + ", " + y.ToString());
                        empdat.Add(new Moji(num, x, y, 0));
                    }
                }
            }

            if (num > 1)
            {
                //次は縦方向を調べます(sta + num - 1は、挿入する場所がY座標2以上にするためです)                
                for (int y = sta + num - 1; y < 6; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        ok = true;
                        if (y < 5 && keshi[y + 1, x] == 0)
                        {
                            ok = false;
                        }

                        if (ok == false) { continue; }

                        //縦に置いた後、上の空間があふれていないかどうか
                        ok = true;
                        for (int i = 0; i < num; i++)
                        {
                            if (keshi[i, x] != 0)
                            {
                                ok = false;
                                break;
                            }
                        }

                        if (ok == true)
                        {
                            //Debug.WriteLine("縦: " + x.ToString() + ", " + (y - num + 1).ToString());
                            empdat.Add(new Moji(num, x, y - num + 1, 1));
                        }
                    }
                }
            }
            return empdat;
        }

        /// <summary>
        /// 漢字を選びます
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public Moji getKanji(int n)
        {
            List<Moji> moji = new List<Moji>();

            //n個の漢字を選び出します
            for (int i = 0; i < MojiDabese.Count; i++)
            {
                if (MojiDabese[i].n == n)
                {
                    moji.Add(MojiDabese[i].Clone());
                }
            }
            return moji[(int)GetRandom((uint)moji.Count - 1)];
        }

        /// <summary>
        /// 24文字の位置を計算します
        /// </summary>
        /// <param name="kazu">[0]-[2]に使うべき1文字漢字の数と2文字漢字の数と3文字漢字の数が入っています</param>
        /// <returns></returns>
        public List<Moji> Calculate(int[] kazus)
        {
            List<Moji> dat = new List<Moji>();
            List<Moji>[] emptyDat = new List<Moji>[3];
            int[,] keshi = new int[6, 4];
            int[] kazu = new int[kazus.Length];

            bool ok = false;
            bool allOk = false;
            while (!allOk)
            {
                ok = false;
                dat.Clear();

                for (int i = 0; i < kazus.Length; i++)
                {
                    kazu[i] = kazus[i];
                }
                for (int y = 0; y < 6; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        keshi[y, x] = 0;
                    }
                }

                while (!ok)
                {
                    ok = false;

                    //1～3文字漢字が置くことができる場所を探します
                    for (int i = 0; i < 3; i++)
                    {
                        emptyDat[i] = getEmptyPos(keshi, i + 1);
                    }

                    //1文字、2文字、3文字のどれかを選びます。
                    int sel = -1;
                    while (true)
                    {
                        int k = (int)GetRandom(2);
                        if (kazu[k] - 1 >= 0)
                        {
                            if (emptyDat[k].Count - 1 >= 0)
                            {
                                kazu[k]--;
                                sel = k;
                                break;
                            }
                            else
                            {
                                ok = true;
                                break;
                            }
                        }
                    }

                    //ok==trueだったら、もう一度、初めから作り直す
                    if (ok == true)
                    {
                        break;
                    }

                    //sel文字から1つデータを選びます
                    int a = (int)GetRandom((uint)emptyDat[sel].Count - 1);
                    Moji moji = emptyDat[sel][a];
                    moji.AnsNum = dat.Count;
                    dat.Add(moji);

                    //keshiデータに追加します
                    if (moji.TateYoko == 0)
                    {
                        for (int x = moji.X; x < moji.X + moji.n; x++)
                        {
                            for (int y = 1; y < moji.Y + 1; y++)
                            {
                                keshi[y - 1, x] = keshi[y, x];
                            }
                            keshi[moji.Y, x] = (int)GetRandom(999) + 1;
                        }
                    }
                    else
                    {
                        for (int y = moji.n; y < moji.Y + moji.n; y++)
                        {
                            keshi[y - moji.n, moji.X] = keshi[y, moji.X];
                        }

                        for (int y = 0; y < moji.n; y++)
                        {
                            keshi[moji.Y + y, moji.X] = (int)GetRandom(999) + 1;
                        }
                    }

                    Debug.WriteLine("-------");
                    for (int y = 0; y < 6; y++)
                    {
                        for (int x = 0; x < 4; x++)
                        {
                            Debug.Write(keshi[y, x].ToString("000") + ", ");
                        }
                        Debug.WriteLine("");
                    }

                    if (kazu[0] == 0 && kazu[1] == 0 && kazu[2] == 0)
                    {
                        ok = true;
                        allOk = true;
                    }
                }
            }
            return dat;
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            KeshiMasuCnt--;
            if (KeshiMasuCnt < 0)
            {
                KeshiMasuCnt = 0;
            }

            //漢字を書きます
            Moji[,] screen = SetKanji(null, KeshiMasuCnt);
            DispKeshimasu(screen, KeshiMasuCnt);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            KeshiMasuCnt++;
            if (KeshiMasuCnt > KeshiMasu.Count)
            {
                KeshiMasuCnt = KeshiMasu.Count;
            }

            //漢字を書きます
            Moji[,] screen = SetKanji(null, KeshiMasuCnt);
            DispKeshimasu(screen, KeshiMasuCnt);
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop);

            //文字データを読み込みます
            if (!ReadKanjiData(filenames[0]))
            {
                MessageBox.Show(Path.GetFileName(filenames[0]) + " ファイルを読み込めませんでした", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnMake.Enabled = false;
                return;
            }
            SetMainFormTitle(Path.GetFileName(filenames[0]), true);
            btnMake.Enabled = true;
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void btnCSVRead_Click(object sender, EventArgs e)
        {
            string filename;
            string[] filenames;
            if (!InputFilename.InputOpenFilename(out filenames, out filename, "CSV(*.csv)|*.csv|全てのファイル|*.*", ""))
            {
                return;
            }

            //文字データを読み込みます
            if (!ReadKanjiData(filename))
            {
                MessageBox.Show(Path.GetFileName(filename) + " ファイルを読み込めませんでした", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnMake.Enabled = false;
                return;
            }

            SetMainFormTitle(Path.GetFileName(filename), true);
            btnMake.Enabled = true;
        }

        private void chkColorOFF_CheckedChanged(object sender, EventArgs e)
        {
            //漢字を書きます
            Moji[,] screen = SetKanji(null, KeshiMasuCnt);
            DispKeshimasu(screen, KeshiMasuCnt);
        }

        private void chkHidden_CheckedChanged(object sender, EventArgs e)
        {
            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < lblKeshiText.GetLength(1); x++)
                {
                    lblKeshiText[y, x].Visible = !chkHidden.Checked;
                }
            }
        }
    }
}
