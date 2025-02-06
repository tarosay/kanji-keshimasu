using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanjiKeshimasu_Maker
{
    public class Moji
    {
        public string Kanji = "";
        public string Yomi = "";
        public int n = 1;
        public int X = 0;
        public int Y = 0;
        public int TateYoko = 0;    //0:横書き, 1:縦書き
        public Color BkColor = Color.Transparent;
        public int KanjiNum = 0;
        public int AnsNum = 0;

        public Moji()
        {
        }
        public Moji(int n, int x, int y, int hv)
        {
            this.n = n;
            this.X = x;
            this.Y = y;
            this.TateYoko = hv;
        }
        public Moji(string kanji, string yomi)
        {
            this.Kanji = kanji;
            this.Yomi = yomi;
            this.n = kanji.Length;
        }

        public Moji Clone()
        {
            Moji dat = new Moji(this.n, this.X, this.Y, this.TateYoko);
            dat.Kanji = this.Kanji;
            dat.Yomi = this.Yomi;
            dat.BkColor = this.BkColor;
            dat.KanjiNum = this.KanjiNum;
            dat.AnsNum = this.AnsNum;
            return dat;
        }
    }
}
