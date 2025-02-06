using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KanjiKeshimasu_Maker
{
    public class InputFilename
    {
        /// <summary>
        /// 保存するファイル名を取得します
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string InputSaveFilename(string basefilename, string filter, string title, string foldername = "")
        {
            string filename = "";
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                try
                {
                    if (foldername != "")
                    {
                        sfd.InitialDirectory = foldername;
                    }
                    else
                    {
                        sfd.InitialDirectory = Path.GetDirectoryName(basefilename);
                    }
                    sfd.FileName = Path.GetFileNameWithoutExtension(basefilename);
                }
                catch { }

                if (sfd.InitialDirectory == "")
                {
                    //sfd.InitialDirectory = Directory.GetCurrentDirectory();
                    sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);    //ダイアログの開く場所
                }

                sfd.Title = title;
                sfd.Filter = filter;
                //sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                if (sfd.ShowDialog() == DialogResult.Cancel)
                {
                    return "";
                }
                filename = sfd.FileName;
            }
            return filename;
        }

        public static bool InputOpenFilename(out string[] fnames, out string fname, string filter, string title, string foldername = "", string basefilename = "", bool multiselect = false)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            fnames = null;
            fname = "";

            //[ファイルの種類]に表示される選択肢を指定する
            //指定しないとすべてのファイルが表示される
            ofd.Filter = filter;
            if (filter == "")
            {
                ofd.Filter = "すべてのファイル(*.*)|*.*";
            }

            //[ファイルの種類]で1番目のフィルタが選択されているようにする
            ofd.FilterIndex = 1;

            //タイトルを設定する
            ofd.Title = title;

            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            ofd.RestoreDirectory = true;

            if (foldername != "")
            {
                ofd.InitialDirectory = foldername;
            }
            else
            {
                if (basefilename == "")
                {
                    ofd.InitialDirectory = "";
                }
                else
                {
                    ofd.InitialDirectory = Path.GetDirectoryName(basefilename);
                }
            }

            if (basefilename == "")
            {
                ofd.FileName = Path.GetFileNameWithoutExtension(basefilename);
            }
            else
            {
                ofd.FileName = "";
            }

            //存在しないファイルの名前が指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            ofd.CheckFileExists = true;

            //存在しないパスが指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            ofd.CheckPathExists = true;

            //複数のファイルを選択できるようにする
            ofd.Multiselect = multiselect;

            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //OKボタンがクリックされたとき
                fnames = ofd.FileNames;
                fname = ofd.FileName;
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}
