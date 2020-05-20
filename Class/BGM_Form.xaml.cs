using System.Windows;
using System.IO;
using System.Threading.Tasks;
using System;
using WMPLib;
using System.Net;
using System.ComponentModel;
using System.Diagnostics;

namespace WoTB_Mod_installer_Remake.Class
{
    public partial class BGM_Form : Window
    {
        string Path = Directory.GetCurrentDirectory();
        string TempPath = System.IO.Path.GetTempPath();
        string WoTB_Path = "";
        bool List_Visible = false;
        string Select_BGM = "";
        bool Exit = false;
        bool FSB_Create_Exist = false;
        WindowsMediaPlayer Player = new WindowsMediaPlayer();
        public BGM_Form()
        {
            InitializeComponent();
            //もしWoTBのフォルダが変わっていたら強制的に終了させる
            try
            {
                StreamReader str = new StreamReader(TempPath + "/SRTTbacon/Path.dat");
                WoTB_Path = str.ReadLine();
                str.Close();
                if (!Directory.Exists(WoTB_Path))
                {
                    MessageBox.Show("WoTBフォルダが取得できませんでした。プログラムを終了します。");
                    Close();
                }
            }
            catch
            {
                MessageBox.Show("エラーが発生しました。プログラムを終了します。");
                File.Delete(TempPath + "/SRTTbacon/Path.dat");
                Close();
            }
            MouseLeftButtonDown += (sender, e) => { DragMove(); };
            Player.settings.volume = 50;
            Volume_S.Value = 50;
            List_Add();
            Form_Show();
        }
        async void Form_Show()
        {
            Opacity = 0;
            while (true)
            {
                Opacity = Opacity + 0.05;
                if (Opacity >= 1)
                {
                    break;
                }
                await Task.Delay(30);
            }
        }
        void List_Add()
        {
            //BGM一覧を追加
            BGM_List.Items.Add("Two Steps From Hell");
            BGM_List.Items.Add("GuP_BGM");
            BGM_List.Items.Add("艦これBGM");
        }
        private void BGM_List_B_Click(object sender, RoutedEventArgs e)
        {
            //リストを表示/非表示
            if (List_Visible == false)
            {
                List_Visible = true;
                Sample_BGM_B.Visibility = Visibility.Hidden;
                BGM_Download_B.Visibility = Visibility.Hidden;
                BGM_List.Visibility = Visibility.Visible;
            }
            else
            {
                List_Visible = false;
                Sample_BGM_B.Visibility = Visibility.Visible;
                BGM_Download_B.Visibility = Visibility.Visible;
                BGM_List.Visibility = Visibility.Hidden;
            }
        }
        private void BGM_List_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //リストが選択されたら
            List_Visible = false;
            BGM_List.Visibility = Visibility.Hidden;
            Sample_BGM_B.Visibility = Visibility.Visible;
            BGM_Download_B.Visibility = Visibility.Visible;
            Select_BGM = BGM_List.SelectedItem.ToString();
            BGM_Name.Text = "選択されているBGMMod:" + Select_BGM;
        }
        private async void Back_B_Click(object sender, RoutedEventArgs e)
        {
            //フェードアウト
            if (Exit == false)
            {
                Exit = true;
                Opacity = 1;
                while (true)
                {
                    Opacity = Opacity - 0.05;
                    if (Opacity <= 0)
                    {
                        MainWindow f = new MainWindow();
                        f.Show();
                        Close();
                        break;
                    }
                    await Task.Delay(30);
                }
            }
        }
        private async void Exit_B_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("終了しますか？", "確認", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                while (true)
                {
                    Opacity = Opacity - 0.05;
                    if (Opacity <= 0)
                    {
                        Close();
                        break;
                    }
                    await Task.Delay(30);
                }
            }
        }
        private void Sample_BGM_B_Click(object sender, RoutedEventArgs e)
        {
            //選択された項目を取得(保存)
            Random r = new Random();
            int r2 = r.Next(1, 6);
            if (Select_BGM == "Two Steps From Hell")
            {
                Sample_Voice_Play(TempPath + "/SRTTbacon/DVPL_Extract/Resources/Two_Steps_From_Hell_0" + r2 + ".mp3");
            }
            if (Select_BGM == "GuP_BGM")
            {
                Sample_Voice_Play(TempPath + "/SRTTbacon/DVPL_Extract/Resources/GuP_BGM_0" + r2 + ".mp3");
            }
            if (Select_BGM == "艦これBGM")
            {
                Sample_Voice_Play(TempPath + "/SRTTbacon/DVPL_Extract/Resources/Kankore_BGM_0" + r2 + ".mp3");
            }
        }
        void Sample_Voice_Play(string Name)
        {
            //サンプルBGMを再生
            try
            {
                Player.controls.stop();
                Player.URL = Name;
                Player.controls.play();
            }
            catch
            {
                MessageBox.Show("音声ファイルを読み込めませんでした。");
            }
        }
        private void BGM_Download_B_Click(object sender, RoutedEventArgs e)
        {
            //サンプルBGMをダウンロード
            if (Select_BGM != "")
            {
                Download_P.Visibility = Visibility.Visible;
                Sample_BGM_B.Visibility = Visibility.Hidden;
                Back_B.Visibility = Visibility.Hidden;
                Exit_B.Visibility = Visibility.Hidden;
                BGM_List_B.Visibility = Visibility.Hidden;
                BGM_Download_B.Visibility = Visibility.Hidden;
                Volume_S.Visibility = Visibility.Hidden;
                string URL = "";
                if (Select_BGM == "Two Steps From Hell")
                {
                    URL = "https://www.dropbox.com/s/eb7cdzt92eke0uc/Two_Steps_From_Hell.zip?dl=1";
                }
                else if (Select_BGM == "GuP_BGM")
                {
                    URL = "https://www.dropbox.com/s/hokil3o63ikud48/GuP_BGM.zip?dl=1";
                }
                else if (Select_BGM == "艦これBGM")
                {
                    URL = "https://www.dropbox.com/s/6x7paczm5iqjgcm/Kankore_BGM.zip?dl=1";
                }
                Uri uri = new Uri(URL);
                if (wc == null)
                {
                    wc = new WebClient();
                    wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged);
                    wc.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_DownloadFileCompleted);
                }
                wc.DownloadFileAsync(uri, TempPath + "/SRTTbacon/BGM_Mod.dat");
            }
            else
            {
                MessageBox.Show("Modが選択されていません。");
            }
        }
        private WebClient wc = null;
        void wc_DownloadProgressChanged(Object sender, DownloadProgressChangedEventArgs e)
        {
            Download_P.Value = e.ProgressPercentage;
        }
        void wc_DownloadFileCompleted(Object sender, AsyncCompletedEventArgs e)
        {
            if ((e.Error != null) && (!e.Cancelled))
            {
                System.Windows.Forms.MessageBox.Show("ダウンロード中にエラーが発生しました。内容:" + e.Error.Message, "エラー", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            else if (e.Cancelled)
            {
                System.Windows.Forms.MessageBox.Show("ダウンロードがキャンセルされました。", "完了", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            else
            {
                BGM_Mod_Extract();
            }
            Download_P.Visibility = Visibility.Hidden;
            Download_P.Value = 0;
        }
        void BGM_Mod_Extract()
        {
            //BGMModを展開
            if (Directory.Exists(TempPath + "/SRTTbacon/BGM_Mod"))
            {
                Directory.Delete(TempPath + "/SRTTbacon/BGM_Mod", true);
            }
            Task task = Task.Run(() =>
            {
                System.IO.Compression.ZipFile.ExtractToDirectory(TempPath + "/SRTTbacon/BGM_Mod.dat", TempPath + "/SRTTbacon/BGM_Mod", System.Text.Encoding.UTF8);
            });
            task.Wait();
            BGM_Mod_Exist();
        }
        void BGM_Mod_Exist()
        {
            //WoTB内にdvpl形式の.yamlファイルがあればdvpl化を解除して保存する
            if (File.Exists(WoTB_Path + "/Configs/Sfx/sfx_high.yaml.dvpl") || File.Exists(WoTB_Path + "/Configs/Sfx/sfx_low.yaml.dvpl") || File.Exists(WoTB_Path + "/sounds.yaml.dvpl"))
            {
                if (File.Exists(WoTB_Path + "/Configs/Sfx/sfx_high.yaml.dvpl"))
                {
                    File.Copy(WoTB_Path + "/Configs/Sfx/sfx_high.yaml.dvpl", TempPath + "/SRTTbacon/DVPL_Extract/Resources_Out/sfx_high.yaml", true);
                }
                if (File.Exists(WoTB_Path + "/Configs/Sfx/sfx_low.yaml.dvpl"))
                {
                    File.Copy(WoTB_Path + "/Configs/Sfx/sfx_low.yaml.dvpl", TempPath + "/SRTTbacon/DVPL_Extract/Resources_Out/sfx_low.yaml", true);
                }
                if (File.Exists(WoTB_Path + "/sounds.yaml.dvpl"))
                {
                    File.Copy(WoTB_Path + "/sounds.yaml.dvpl", TempPath + "/SRTTbacon/DVPL_Extract/Resources_Out/sounds.yaml", true);
                }
                var startInfo = new ProcessStartInfo();
                startInfo.FileName = TempPath + "/SRTTbacon/DVPL_Extract/Resources_UnPack.bat";
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;
                startInfo.WorkingDirectory = System.IO.Path.GetTempPath() + "/SRTTbacon/DVPL_Extract";
                var process = Process.Start(startInfo);
                process.WaitForExit();
                string[] Files = Directory.GetFiles(TempPath + "/SRTTbacon/DVPL_Extract/Resources", "*", SearchOption.TopDirectoryOnly);
                foreach(string File in Files)
                {
                    if (System.IO.Path.GetFileName(File).Contains("sfx_high.yaml") || System.IO.Path.GetFileName(File).Contains("sfx_low.yaml"))
                    {
                        System.IO.File.Copy(File, WoTB_Path + "/Configs/Sfx/" + System.IO.Path.GetFileName(File), true);
                        System.IO.File.Delete(WoTB_Path + "/Configs/Sfx/" + System.IO.Path.GetFileName(File) + ".dvpl");
                    }
                    else if (System.IO.Path.GetFileName(File).Contains("sounds.yaml"))
                    {
                        System.IO.File.Copy(File, WoTB_Path + "/" + System.IO.Path.GetFileName(File), true);
                        System.IO.File.Delete(WoTB_Path + "/" + System.IO.Path.GetFileName(File) + ".dvpl");
                    }
                }
                if (FSB_Create_Exist == false)
                {
                    BGM_Mod_Install();
                }
            }
            else
            {
                if (FSB_Create_Exist == false)
                {
                    BGM_Mod_Install();
                }
            }
        }
        void BGM_Mod_Install()
        {
            //BGMModをWoTBフォルダへコピーする
            string[] Files_List = Directory.GetFiles(TempPath + "/SRTTbacon/BGM_Mod", "*", SearchOption.AllDirectories);
            foreach(string File_Name in Files_List)
            {
                string Ex = System.IO.Path.GetFileName(File_Name);
                if (Ex.Contains(".fsb") || Ex.Contains(".fev"))
                {
                    File.Copy(File_Name, WoTB_Path + "/Mods/" + Ex, true);
                    StreamReader str = new StreamReader(WoTB_Path + "/Configs/Sfx/sfx_high.yaml");
                    string Read = str.ReadToEnd();
                    str.Close();
                    if (!Read.Contains(Ex) && !Ex.Contains(".fsb"))
                    {
                        StreamWriter stw = new StreamWriter(WoTB_Path + "/Configs/Sfx/sfx_high.yaml", true);
                        stw.WriteLine("\n -");
                        stw.WriteLine("  \"~res:/Mods/" + Ex + "\"");
                        stw.Close();
                        StreamWriter stw2 = new StreamWriter(WoTB_Path + "/Configs/Sfx/sfx_low.yaml", true);
                        stw2.WriteLine("\n -");
                        stw2.WriteLine("  \"~res:/Mods/" + Ex + "\"");
                        stw2.Close();
                        System.Text.Encoding enc = System.Text.Encoding.UTF8;
                        StreamReader sr = new StreamReader(WoTB_Path + "/Configs/Sfx/sfx_high.yaml", enc);
                        string[] ss = Microsoft.VisualBasic.Strings.Split(sr.ReadToEnd(), "\r\n", -1, Microsoft.VisualBasic.CompareMethod.Binary);
                        sr.Close();
                        StreamWriter sw = new StreamWriter(WoTB_Path + "/Configs/Sfx/sfx_high.yaml", false, enc);
                        sw.Write(string.Join("\r\n", ss, 0, ss.Length - 1));
                        sw.Close();
                        StreamReader sr2 = new StreamReader(WoTB_Path + "/Configs/Sfx/sfx_low.yaml", enc);
                        string[] ss2 = Microsoft.VisualBasic.Strings.Split(sr2.ReadToEnd(), "\r\n", -1, Microsoft.VisualBasic.CompareMethod.Binary);
                        sr2.Close();
                        StreamWriter sw2 = new StreamWriter(WoTB_Path + "/Configs/Sfx/sfx_low.yaml", false, enc);
                        sw2.Write(string.Join("\r\n", ss2, 0, ss2.Length - 1));
                        sw2.Close();
                    }
                }
            }
            File.Move(WoTB_Path + "/sounds.yaml", WoTB_Path + "/sounds1.yaml");
            string Line_Temp = "";
            StreamWriter stw3 = File.CreateText(WoTB_Path + "/sounds.yaml");
            StreamReader sr3 = new StreamReader(WoTB_Path + "/sounds1.yaml", System.Text.Encoding.GetEncoding("UTF-8"));
            while (sr3.EndOfStream == false)
            {
                string line = sr3.ReadLine();
                if (line.Contains("VOICE_START_BATTLE") && !line.Contains("Music/Music/Music"))
                {
                    Line_Temp = line.Replace("    VOICE_START_BATTLE: ", "");
                    line = "    VOICE_START_BATTLE: \"Music/Music/Music\"";
                }
                else if (line.Contains("PREBATTLE_TIMER") && Line_Temp != "")
                {
                    line = "    PREBATTLE_TIMER: " + Line_Temp;
                }
                stw3.WriteLine(line);
            }
            sr3.Close();
            stw3.Close();
            File.Delete(WoTB_Path + "/sounds1.yaml");
            Sample_BGM_B.Visibility = Visibility.Visible;
            Back_B.Visibility = Visibility.Visible;
            Exit_B.Visibility = Visibility.Visible;
            BGM_List_B.Visibility = Visibility.Visible;
            BGM_Download_B.Visibility = Visibility.Visible;
            Volume_S.Visibility = Visibility.Visible;
            Directory.Delete(TempPath + "/SRTTbacon/BGM_Mod", true);
            File.Delete(TempPath + "/SRTTbacon/BGM_Mod.dat");
            MessageBox.Show("完了しました。");
        }
        private void Volume_S_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Player.settings.volume = (int)Volume_S.Value;
        }
        void wc_DownloadFileCompleted_02(Object sender, AsyncCompletedEventArgs e)
        {
            if ((e.Error != null) && (!e.Cancelled))
            {
                System.Windows.Forms.MessageBox.Show("ダウンロード中にエラーが発生しました。内容:" + e.Error.Message, "エラー", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            else if (e.Cancelled)
            {
                System.Windows.Forms.MessageBox.Show("ダウンロードがキャンセルされました。", "完了", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            else
            {
                Directory.CreateDirectory(TempPath + "/SRTTbacon/fsbank");
                System.IO.Compression.ZipFile.ExtractToDirectory(TempPath + "/SRTTbacon/fsbank.dat", TempPath + "/SRTTbacon/fsbank");
                MessageBox.Show("ダウンロードが完了しました。もう一度ボタンを押すと開始できます。");
            }
            Download_P.Visibility = Visibility.Hidden;
            Download_P.Value = 0;
        }
        private void BGM_Create_B_Click(object sender, RoutedEventArgs e)
        {
            //fsbankがなければダウンロード
            if (!File.Exists(TempPath + "/SRTTbacon/fsbank/fsbank.exe"))
            {
                MessageBoxResult result = MessageBox.Show("FMOD Studio API Windowsをダウンロードします。続行しますか？", "確認", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Download_P.Visibility = Visibility.Visible;
                    Uri uri = new Uri("https://www.dropbox.com/s/s6jwwlg7nmj4g8l/fsbank.zip?dl=1");
                    wc = new WebClient();
                    wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged);
                    wc.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_DownloadFileCompleted_02);
                    wc.DownloadFileAsync(uri, TempPath + "/SRTTbacon/fsbank.dat");
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("/Resources/Music内のmp3ファイルをBGMModに変換します。この作業には時間がかかる場合があります。続行しますか？", "確認", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        //BGMファイルの数を取得し、fsbankのプロジェクトを生成させる
                        int Number = 0;
                        if (Directory.GetFiles(Path + "/Resources/Music", "*", SearchOption.TopDirectoryOnly).Length > 9 || Directory.GetFiles(Path + "/Resources/Music", "*", SearchOption.TopDirectoryOnly).Length < 1)
                        {
                            if (Directory.GetFiles(Path + "/Resources/Music", "*", SearchOption.TopDirectoryOnly).Length > 9)
                            {
                                MessageBox.Show("読み込みが可能な最大ファイル数は9です。");
                            }
                            if (Directory.GetFiles(Path + "/Resources/Music", "*", SearchOption.TopDirectoryOnly).Length < 1)
                            {
                                MessageBox.Show("フォルダ内に有効なファイルが含まれていません。");
                            }
                        }
                        else
                        {
                            FSB_Create_Exist = true;
                            BGM_Mod_Exist();
                            FSB_Create_Exist = false;
                            string[] Files = Directory.GetFiles(Path + "/Resources/Music", "*", SearchOption.TopDirectoryOnly);
                            foreach (string File in Files)
                            {
                                if (Number <= 9)
                                {
                                    Number += 1;
                                    if (System.IO.Path.GetExtension(File) != ".mp3")
                                    {
                                        //MP3ファイル以外の場合はMP3にエンコードする。(1回1回エラーが出るため何度もボタンを押す必要がある。)
                                        StreamWriter stw5 = System.IO.File.CreateText(TempPath + "/SRTTbacon/lame.bat");
                                        stw5.WriteLine("lame.exe \"" + File + "\" \"" + Path + "/Resources/Music/" + System.IO.Path.GetFileName(File).Replace(System.IO.Path.GetExtension(File), ".mp3") + "\"");
                                        stw5.Close();
                                        var startInfo = new ProcessStartInfo();
                                        startInfo.FileName = TempPath + "/SRTTbacon/lame.bat";
                                        startInfo.UseShellExecute = false;
                                        startInfo.CreateNoWindow = true;
                                        startInfo.WorkingDirectory = System.IO.Path.GetTempPath() + "/SRTTbacon";
                                        var process = Process.Start(startInfo);
                                        process.WaitForExit();
                                        System.IO.File.Delete(File);
                                    }
                                    int Number_99 = 1;
                                    if (!System.IO.Path.GetFileName(File).Contains("Music_0"))
                                    {
                                        while (true)
                                        {
                                            if (System.IO.File.Exists(Path + "/Resources/Music/Music_0" + Number_99 + ".mp3"))
                                            {
                                                Number_99 += 1;
                                            }
                                            else
                                            {
                                                System.IO.File.Move(File, Path + "/Resources/Music/Music_0" + Number_99 + ".mp3");
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            int Number_01 = 1;
                            Directory.CreateDirectory(Path + "/Resources/Projects");
                            //fsbankのプロジェクトデータを作成する。
                            StreamWriter stw1 = File.CreateText(Path + "/Resources/Projects/BGM_Project.fsproj");
                            stw1.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<soundbank>\n    <file_name>" + Path + "/Resources/Music.fsb" + "</file_name>\n    <profile>\n        <format>MP3</format>\n        <quality>50</quality>\n    </profile>\n    <content>\n");
                            stw1.Close();
                            System.Text.Encoding enc = System.Text.Encoding.GetEncoding("UTF-8");
                            StreamWriter writer = new StreamWriter(Path + "/Resources/Projects/BGM_Project.fsproj", true, enc);
                            while (true)
                            {
                                if (File.Exists(Path + "/Resources/Music/Music_0" + Number_01 + ".mp3"))
                                {
                                    writer.WriteLine("        <subsound>");
                                    writer.WriteLine("            <file>" + Path + "/Resources/Music/Music_0" + Number_01 + ".mp3" + "</file>");
                                    writer.WriteLine("        </subsound>");
                                    Number_01 += 1;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            writer.WriteLine("    </content>");
                            writer.WriteLine("</soundbank>");
                            writer.Close();
                            //fsbankを起動させる。
                            StreamWriter stw2 = File.CreateText(TempPath + "/SRTTbacon/fsbank/BGM_Run.bat");
                            stw2.WriteLine("fsbank.exe");
                            stw2.Close();
                            MessageBox.Show("プロジェクトデータの作成に成功しました。\n場所:" + Path + "/Resources/Projects/BGM_Project.fsproj");
                            var startInfo2 = new ProcessStartInfo();
                            startInfo2.FileName = TempPath + "/SRTTbacon/fsbank/BGM_Run.bat";
                            startInfo2.UseShellExecute = false;
                            startInfo2.CreateNoWindow = true;
                            startInfo2.WorkingDirectory = TempPath + "/SRTTbacon/fsbank";
                            var process2 = Process.Start(startInfo2);
                            //BGMの数によって専用にfevファイルを出力させる(exe内に入っているのでそれをコピーする)
                            //Byte形式で取得し、ファイルに書き込む
                            if (Number_01 == 1)
                            {
                                byte[] strm = Properties.Resources.Music_01;
                                File.WriteAllBytes(Path + "/Resources/Music.fev", strm);
                            }
                            else if (Number_01 == 2)
                            {
                                byte[] strm = Properties.Resources.Music_02;
                                File.WriteAllBytes(Path + "/Resources/Music.fev", strm);
                            }
                            else if (Number_01 == 3)
                            {
                                byte[] strm = Properties.Resources.Music_03;
                                File.WriteAllBytes(Path + "/Resources/Music.fev", strm);
                            }
                            else if (Number_01 == 4)
                            {
                                byte[] strm = Properties.Resources.Music_04;
                                File.WriteAllBytes(Path + "/Resources/Music.fev", strm);
                            }
                            else if (Number_01 == 5)
                            {
                                byte[] strm = Properties.Resources.Music_05;
                                File.WriteAllBytes(Path + "/Resources/Music.fev", strm);
                            }
                            else if (Number_01 == 6)
                            {
                                byte[] strm = Properties.Resources.Music_06;
                                File.WriteAllBytes(Path + "/Resources/Music.fev", strm);
                            }
                            else if (Number_01 == 7)
                            {
                                byte[] strm = Properties.Resources.Music_07;
                                File.WriteAllBytes(Path + "/Resources/Music.fev", strm);
                            }
                            else if (Number_01 == 8)
                            {
                                byte[] strm = Properties.Resources.Music_08;
                                File.WriteAllBytes(Path + "/Resources/Music.fev", strm);
                            }
                            else if (Number_01 == 9)
                            {
                                byte[] strm = Properties.Resources.Music_09;
                                File.WriteAllBytes(Path + "/Resources/Music.fev", strm);
                            }
                            StreamReader str99 = new StreamReader(WoTB_Path + "/Configs/Sfx/sfx_high.yaml");
                            string str999 = str99.ReadToEnd();
                            str99.Close();
                            if (!str999.Contains("Music.fev"))
                            {
                                StreamWriter stw = new StreamWriter(WoTB_Path + "/Configs/Sfx/sfx_high.yaml", true);
                                stw.WriteLine("\n -");
                                stw.WriteLine("  \"~res:/Mods/Music.fev\"");
                                stw.Close();
                                StreamWriter stw4 = new StreamWriter(WoTB_Path + "/Configs/Sfx/sfx_low.yaml", true);
                                stw4.WriteLine("\n -");
                                stw4.WriteLine("  \"~res:/Mods/Music.fev\"");
                                stw4.Close();
                                StreamReader sr = new StreamReader(WoTB_Path + "/Configs/Sfx/sfx_high.yaml", enc);
                                string[] ss = Microsoft.VisualBasic.Strings.Split(sr.ReadToEnd(), "\r\n", -1, Microsoft.VisualBasic.CompareMethod.Binary);
                                sr.Close();
                                StreamWriter sw = new StreamWriter(WoTB_Path + "/Configs/Sfx/sfx_high.yaml", false, enc);
                                sw.Write(string.Join("\r\n", ss, 0, ss.Length - 1));
                                sw.Close();
                                StreamReader sr2 = new StreamReader(WoTB_Path + "/Configs/Sfx/sfx_low.yaml", enc);
                                string[] ss2 = Microsoft.VisualBasic.Strings.Split(sr2.ReadToEnd(), "\r\n", -1, Microsoft.VisualBasic.CompareMethod.Binary);
                                sr2.Close();
                                StreamWriter sw2 = new StreamWriter(WoTB_Path + "/Configs/Sfx/sfx_low.yaml", false, enc);
                                sw2.Write(string.Join("\r\n", ss2, 0, ss2.Length - 1));
                                sw2.Close();
                            }
                            File.Move(WoTB_Path + "/sounds.yaml", WoTB_Path + "/sounds1.yaml");
                            string Line_Temp = "";
                            StreamWriter stw3 = File.CreateText(WoTB_Path + "/sounds.yaml");
                            StreamReader sr3 = new StreamReader(WoTB_Path + "/sounds1.yaml", System.Text.Encoding.GetEncoding("UTF-8"));
                            while (sr3.EndOfStream == false)
                            {
                                string line = sr3.ReadLine();
                                if (line.Contains("VOICE_START_BATTLE") && !line.Contains("Music/Music/Music"))
                                {
                                    Line_Temp = line.Replace("    VOICE_START_BATTLE: ", "");
                                    line = "    VOICE_START_BATTLE: \"Music/Music/Music\"";
                                }
                                else if (line.Contains("PREBATTLE_TIMER") && Line_Temp != "")
                                {
                                    line = "    PREBATTLE_TIMER: " + Line_Temp;
                                }
                                stw3.WriteLine(line);
                            }
                            sr3.Close();
                            stw3.Close();
                            File.Delete(WoTB_Path + "/sounds1.yaml");
                        }
                    }
                    catch
                    {
                        MessageBox.Show("エラーが発生しました。開発者へご連絡ください。");
                    }
                }
            }
        }
        private async void Omake_B_Click(object sender, RoutedEventArgs e)
        {
            if (Exit == false)
            {
                Exit = true;
                Opacity = 1;
                while (true)
                {
                    Opacity = Opacity - 0.05;
                    if (Opacity <= 0)
                    {
                        Other f = new Other();
                        f.Show();
                        Close();
                        break;
                    }
                    await Task.Delay(30);
                }
            }
        }
    }
}