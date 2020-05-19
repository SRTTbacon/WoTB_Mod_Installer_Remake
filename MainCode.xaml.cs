using System.Windows;
using System.IO;
using System.Threading.Tasks;
using WMPLib;
using System.Diagnostics;
using System.Net;
using System.ComponentModel;
using System;

namespace WoTB_Mod_installer_Remake
{
    public partial class MainWindow : Window
    {
        string Path_Temp = System.IO.Path.GetTempPath();
        string Path = Directory.GetCurrentDirectory();
        string TempPath = System.IO.Path.GetTempPath();
        bool Mod_List_Visible = false;
        string Select_Mod = "";
        string WoTB_Path = "";
        bool OK = false;
        string List = "";
        int Number = 0;
        bool FSB_File_OK = true;
        bool Exit = false;
        bool IsFSB_Extract = false;
        bool WoTB_Mods_Directory = false;
        WindowsMediaPlayer Player = new WindowsMediaPlayer();
        public MainWindow()
        {
            //起動直後
            InitializeComponent();
            MouseLeftButtonDown += (sender, e) => { DragMove(); };
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show("ネットワークに接続されていません。ソフトを強制終了します。");
                Close();
            }
            byte[] strm = Properties.Resources.SRTTbacon;
            List_Add();
            Directory.CreateDirectory(TempPath + "/SRTTbacon");
            Directory.CreateDirectory(TempPath + "/SRTTbacon/Number");
            Directory.CreateDirectory(Path + "/Resources");
            Directory.CreateDirectory(Path + "/Resources/Music");
            Directory.CreateDirectory(Path + "/Resources/Backup");
            if (!File.Exists(Path_Temp + "/SRTTbacon/DVPL_Extract/Resources/ASMR_01.mp3"))
            {
                if (Directory.Exists(Path_Temp + "/SRTTbacon"))
                {
                    Directory.Delete(Path_Temp + "/SRTTbacon", true);
                }
                byte[] stream = Properties.Resources.SRTTbacon;
                Task task = Task.Run(() =>
                {
                    File.WriteAllBytes(Path_Temp + "/SRTTbacon.dat", stream);
                });
                task.Wait();
                System.IO.Compression.ZipFile.ExtractToDirectory(Path_Temp + "/SRTTbacon.dat", Path_Temp + "/SRTTbacon");
                string[] Voices = Directory.GetFiles(Path + "/Resources/Voice_Sample", "*.mp3", SearchOption.TopDirectoryOnly);
                foreach(string Voice in Voices)
                {
                    string File_Name = System.IO.Path.GetFileName(Voice);
                    File.Copy(Voice, Path_Temp + "/SRTTbacon/DVPL_Extract/Resources_Out/" + File_Name, true);
                }
                var startInfo = new ProcessStartInfo();
                startInfo.FileName = Path_Temp + "/SRTTbacon/DVPL_Extract/Resources_UnPack.bat";
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;
                startInfo.WorkingDirectory = Path_Temp + "/SRTTbacon/DVPL_Extract";
                var process = Process.Start(startInfo);
                process.WaitForExit();
                File.Delete(Path_Temp + "/SRTTbacon.dat");
            }
            WoTB_Folder_Select();
            Player.settings.volume = 50;
            Volume_S.Value = 50;
            Form_Show();
            Main_Loop();
            if (!File.Exists(Path + "/Resources/Backup/sounds.yaml") && !File.Exists(Path + "/Resources/Backup/sounds.yaml.dvpl"))
            {
                if (File.Exists(WoTB_Path + "/sounds.yaml"))
                {
                    File.Copy(WoTB_Path + "/sounds.yaml", Path + "/Resources/Backup/sounds.yaml", true);
                }
                else if (File.Exists(WoTB_Path + "/sounds.yaml.dvpl"))
                {
                    File.Copy(WoTB_Path + "/sounds.yaml.dvpl", Path + "/Resources/Backup/sounds.yaml.dvpl", true);
                }
                if (File.Exists(WoTB_Path + "/Configs/Sfx/sfx_high.yaml"))
                {
                    Directory.CreateDirectory(Path + "/Resources/Backup/Configs/SFx");
                    File.Copy(WoTB_Path + "/Configs/Sfx/sfx_high.yaml", Path + "/Resources/Backup/Configs/Sfx/sfx_high.yaml", true);
                }
                else if (File.Exists(WoTB_Path + "/Configs/Sfx/sfx_high.yaml.dvpl"))
                {
                    Directory.CreateDirectory(Path + "/Resources/Backup/Configs/SFx");
                    File.Copy(WoTB_Path + "/Configs/Sfx/sfx_high.yaml.dvpl", Path + "/Resources/Backup/Configs/Sfx/sfx_high.yaml.dvpl", true);
                }
                if (File.Exists(WoTB_Path + "/Configs/Sfx/sfx_low.yaml"))
                {
                    Directory.CreateDirectory(Path + "/Resources/Backup/Configs/SFx");
                    File.Copy(WoTB_Path + "/Configs/Sfx/sfx_low.yaml", Path + "/Resources/Backup/Configs/Sfx/sfx_low.yaml", true);
                }
                else if (File.Exists(WoTB_Path + "/Configs/Sfx/sfx_low.yaml.dvpl"))
                {
                    Directory.CreateDirectory(Path + "/Resources/Backup/Configs/SFx");
                    File.Copy(WoTB_Path + "/Configs/Sfx/sfx_low.yaml.dvpl", Path + "/Resources/Backup/Configs/Sfx/sfx_low.yaml.dvpl", true);
                }
                if (Directory.Exists(WoTB_Path + "/Mods"))
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(WoTB_Path + "/Mods", Path + "/Resources/Backup/Mods");
                    WoTB_Mods_Directory = true;
                }
                else
                {
                    WoTB_Mods_Directory = false;
                }
            }
        }
        async void Main_Loop()
        {
            while (true)
            {
                //fsbファイルの展開が終わったかを取得
                if (FSB_File_OK == false)
                {
                    string[] Files = Directory.GetFiles(Path_Temp + "/SRTTbacon", "*.wav", SearchOption.TopDirectoryOnly);
                    foreach (string File in Files)
                    {
                        if (!List.Contains(File))
                        {
                            StreamWriter stw = System.IO.File.CreateText(Path_Temp + "/SRTTbacon/Number/" + Number + ".dat");
                            stw.WriteLine(Path + "/Resources/Extract/" + System.IO.Path.GetFileName(File).Replace(".wav", ".mp3"));
                            stw.Close();
                            List += File;
                            Number += 1;
                        }
                    }
                }
                await Task.Delay(5);
            }
        }
        async void Form_Show()
        {
            //フェードイン
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
        private async void Exit_B_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("終了しますか？", "確認", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                Opacity = 1;
                int Down_Speed = (int)Volume_S.Value / 18;
                while (true)
                {
                    Opacity = Opacity - 0.05;
                    if (Player.playState == WMPPlayState.wmppsPlaying)
                    {
                        Volume_S.Value -= Down_Speed;
                    }
                    if (Opacity <= 0)
                    {
                        Close();
                        break;
                    }
                    await Task.Delay(30);
                }
            }
        }
        private void Mod_Install_B_Click(object sender, RoutedEventArgs e)
        {
            //音声Mod一覧のリストを表示/非表示
            if (Mod_List_Visible == false)
            {
                Mod_List_Visible = true;
                Mod_Download_B.Visibility = Visibility.Hidden;
                Sample_Voice_B.Visibility = Visibility.Hidden;
                BGM_Form_B.Visibility = Visibility.Hidden;
                FSB_Convert.Visibility = Visibility.Hidden;
                FSB_Extract_B.Visibility = Visibility.Hidden;
                Volume_S.Visibility = Visibility.Hidden;
                Mod_List.Visibility = Visibility.Visible;
            }
            else
            {
                if (IsFSB_Extract == true)
                {
                    Mod_Install_B.Visibility = Visibility.Visible;
                    Sample_Voice_B.Visibility = Visibility.Visible;
                    Volume_S.Visibility = Visibility.Visible;
                    Mod_List.Visibility = Visibility.Hidden;
                }
                else
                {
                    Mod_Download_B.Visibility = Visibility.Visible;
                    Sample_Voice_B.Visibility = Visibility.Visible;
                    Volume_S.Visibility = Visibility.Visible;
                    BGM_Form_B.Visibility = Visibility.Visible;
                    FSB_Convert.Visibility = Visibility.Visible;
                    FSB_Extract_B.Visibility = Visibility.Visible;
                    Sample_Voice_B.Visibility = Visibility.Visible;
                    Mod_List.Visibility = Visibility.Hidden;
                }
                Mod_List_Visible = false;
            }
        }
        private void Mod_List_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //リストの項目がクリックされたら
            if (IsFSB_Extract == true)
            {
                Mod_Install_B.Visibility = Visibility.Visible;
                Sample_Voice_B.Visibility = Visibility.Visible;
                Volume_S.Visibility = Visibility.Visible;
                Mod_List.Visibility = Visibility.Hidden;
            }
            else
            {
                Mod_List.Visibility = Visibility.Hidden;
                Volume_S.Visibility = Visibility.Visible;
                Sample_Voice_B.Visibility = Visibility.Visible;
                Mod_Download_B.Visibility = Visibility.Visible;
                BGM_Form_B.Visibility = Visibility.Visible;
                FSB_Convert.Visibility = Visibility.Visible;
                FSB_Extract_B.Visibility = Visibility.Visible;
            }
            Mod_List_Visible = false;
            Select_Mod_Name.Text = "選択されている音声Mod:" + Mod_List.SelectedItem;
            Select_Mod = Mod_List.SelectedItem.ToString();
        }
        private void Mod_Download_B_Click(object sender, RoutedEventArgs e)
        {
            if (Select_Mod != "")
            {
                Select_Mod_Download();
            }
            else
            {
                MessageBox.Show("音声Modが選択されていません。");
            }
        }
        async void WoTB_Folder_Select()
        {
            //WoTBのフォルダを設定する
            if (!File.Exists(TempPath + "/SRTTbacon/Path.dat"))
            {
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                fbd.Description = "World of Tanks Blitzフォルダを選択してください。";
                fbd.ShowNewFolderButton = false;
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        string[] Files = Directory.GetFiles(fbd.SelectedPath, "*", SearchOption.AllDirectories);
                        foreach (string FileName in Files)
                        {
                            if (System.IO.Path.GetFileName(FileName).Contains("license-image001.png.dvpl") || System.IO.Path.GetFileName(FileName).Contains("license-image001.png"))
                            {
                                WoTB_Path = System.IO.Path.GetDirectoryName(FileName);
                                StreamWriter stw = File.CreateText(TempPath + "/SRTTbacon/Path.dat");
                                stw.WriteLine(WoTB_Path);
                                stw.Close();
                                OK = true;
                                break;
                            }
                            else
                            {
                                OK = false;
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("そのフォルダはアクセスが許可されていません。");
                    }
                    if (OK == true)
                    {
                        MessageBox.Show("WoTBのフォルダを取得しました。\n" + WoTB_Path);
                    }
                    else
                    {
                        MessageBoxResult result = MessageBox.Show("フォルダを確認できませんでした。\n選択をやり直しますか？", "確認", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            WoTB_Folder_Select();
                        }
                        else
                        {
                            while (true)
                            {
                                Opacity = Opacity - 0.05;
                                if (Opacity <= 0)
                                {
                                    Close();
                                }
                                await Task.Delay(30);
                            }
                        }
                    }
                }
                else
                {
                    Close();
                }
            }
            else
            {
                StreamReader str = new StreamReader(TempPath + "/SRTTbacon/Path.dat");
                WoTB_Path = str.ReadLine();
                str.Close();
            }
        }
        private async void BGM_Form_B_Click(object sender, RoutedEventArgs e)
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
                        Class.BGM_Form f = new Class.BGM_Form();
                        f.Show();
                        Close();
                        break;
                    }
                    await Task.Delay(30);
                }
            }
        }
        private void FSB_Extract_B_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("FSBファイルの内容によっては時間がかかる場合があります。また、続行しますか？", "確認", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                //fsbファイルを展開する
                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                ofd.Title = "FSBファイルを選択してください。";
                ofd.Multiselect = false;
                ofd.Filter = "FSBファイル(*.fsb;*.bank)|*.fsb;*.bank";
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        if (Directory.Exists(Path_Temp + "/SRTTbacon/Number"))
                        {
                            Directory.Delete(Path_Temp + "/SRTTbacon/Number", true);
                        }
                        Directory.CreateDirectory(Path_Temp + "/SRTTbacon/Number");
                        List = "";
                        Number = 0;
                        FSB_File_OK = false;
                        Task task = Task.Run(() =>
                        {
                            File.Copy(ofd.FileName, Path_Temp + "/SRTTbacon/FSB_Extract.fsb", true);
                        });
                        task.Wait();
                        var startInfo = new ProcessStartInfo();
                        startInfo.FileName = Path_Temp + "/SRTTbacon/Extract.bat";
                        startInfo.UseShellExecute = false;
                        startInfo.CreateNoWindow = true;
                        startInfo.WorkingDirectory = System.IO.Path.GetTempPath() + "/SRTTbacon";
                        var process = Process.Start(startInfo);
                        IsFSB_Extract = true;
                        Mod_Download_B.Visibility = Visibility.Hidden;
                        BGM_Form_B.Visibility = Visibility.Hidden;
                        FSB_Extract_B.Visibility = Visibility.Hidden;
                        FSB_Convert.Visibility = Visibility.Hidden;
                        Cache_B.Visibility = Visibility.Hidden;
                        Task task2 = Task.Run(() =>
                        {
                            process.WaitForExit();
                            FSB_File_OK = true;
                            FSB_Extract();
                        });
                    }
                    catch
                    {
                        MessageBox.Show("エラー：もう一度お試しください。繰り返し表示された場合は開発者へご連絡ください。");
                    }
                }
            }
        }
        void FSB_Extract()
        {
            //上と同じ
            Directory.CreateDirectory(Path_Temp + "/SRTTbacon/Extract");
            if (Directory.Exists(Path + "/Resources/Extract"))
            {
                Directory.Delete(Path + "/Resources/Extract", true);
            }
            Directory.CreateDirectory(Path + "/Resources/Extract");
            string[] Files = Directory.GetFiles(Path_Temp + "/SRTTbacon", "*.wav", SearchOption.TopDirectoryOnly);
            foreach (string File in Files)
            {
                //展開されたwavファイルをmp3へ変換する
                StreamWriter stw = System.IO.File.CreateText(Path_Temp + "/SRTTbacon/lame.bat");
                stw.WriteLine("lame.exe \"" + File + "\" \"Extract/" + System.IO.Path.GetFileName(File).Replace(".wav", ".mp3") + "\"");
                stw.Close();
                var startInfo = new ProcessStartInfo();
                startInfo.FileName = Path_Temp + "/SRTTbacon/lame.bat";
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;
                startInfo.WorkingDirectory = System.IO.Path.GetTempPath() + "/SRTTbacon";
                var process = Process.Start(startInfo);
                process.WaitForExit();
                System.IO.File.Delete(File);
            }
            //変換されたmp3ファイルを実行フォルダの/Resources/Extractへコピーして元のファイルを削除
            string[] Files2 = Directory.GetFiles(Path_Temp + "/SRTTbacon/Extract", "*.mp3", SearchOption.TopDirectoryOnly);
            foreach (string File2 in Files2)
            {
                File.Copy(File2, Path + "/Resources/Extract/" + System.IO.Path.GetFileName(File2), true);
            }
            Directory.Delete(Path_Temp + "/SRTTbacon/Extract", true);
            IsFSB_Extract = false;
            MessageBoxResult result = MessageBox.Show("抽出が完了しました。フォルダを開きますか？", "確認", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                Process.Start(Path + "/Resources/Extract");
            }
            if (Mod_List_Visible == false)
            {
                Mod_Download_B.Visibility = Visibility.Visible;
                BGM_Form_B.Visibility = Visibility.Visible;
                FSB_Extract_B.Visibility = Visibility.Visible;
                FSB_Convert.Visibility = Visibility.Visible;
                Cache_B.Visibility = Visibility.Visible;
            }
        }
        WebClient webc;
        private void FSB_Convert_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(Path_Temp + "/SRTTbacon/fsbank/fsbank.exe"))
            {
                MessageBoxResult result = MessageBox.Show("FMOD Studio API Windowsをダウンロードします。続行しますか？", "確認", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    //fsbankをダウンロード
                    Download_Progress.Visibility = Visibility.Visible;
                    Uri uri = new Uri("https://www.dropbox.com/s/s6jwwlg7nmj4g8l/fsbank.zip?dl=1");
                    webc = new WebClient();
                    webc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged_02);
                    webc.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_DownloadFileCompleted_02);
                    webc.DownloadFileAsync(uri, Path_Temp + "/SRTTbacon/fsbank.dat");
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("\"FSBファイルから音声を抽出\"から抽出したデータのみが適応されます。続行しますか？", "確認", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    if (File.Exists(Path_Temp + "/SRTTbacon/Number/0.dat"))
                    {
                        FMOD_Save(Path_Temp + "/SRTTbacon/Number");
                    }
                    else
                    {
                        MessageBox.Show("先に\"FSBファイルから音声を抽出\"ボタンから抽出する必要があります。");
                    }
                }
            }
        }
        private void Cache_B_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("生成されたファイル(ダウンロードを含む)を削除します。続行しますか？", "確認", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    //生成されたファイルやフォルダを削除し、最低限のファイルを残す(再び生成させる)
                    File.Copy(Path_Temp + "/SRTTbacon/Path.dat", Path_Temp + "/SRTTbacon_WoTB_Path.dat", true);
                    Directory.Delete(Path_Temp + "/SRTTbacon", true);
                    if (Directory.Exists(Path + "/Resources/Extract"))
                    {
                        Directory.Delete(Path + "/Resources/Extract", true);
                    }
                    if (Directory.Exists(Path + "/Resources/Projects"))
                    {
                        Directory.Delete(Path + "/Resources/Projects", true);
                    }
                    if (Directory.Exists(Path + "/Resources/Music"))
                    {
                        Directory.Delete(Path + "/Resources/Music", true);
                    }
                    string[] Files = Directory.GetFiles(Path + "/Resources", "*", SearchOption.TopDirectoryOnly);
                    foreach (string File in Files)
                    {
                        System.IO.File.Delete(File);
                    }
                    Directory.CreateDirectory(TempPath + "/SRTTbacon");
                    Directory.CreateDirectory(Path + "/Resources/Music");
                    byte[] strm = Properties.Resources.SRTTbacon;
                    File.WriteAllBytes(Path_Temp + "/Resources.dat", strm);
                    System.IO.Compression.ZipFile.ExtractToDirectory(Path_Temp + "/Resources.dat", Path_Temp + "/SRTTbacon");
                    string[] Voices = Directory.GetFiles(Path + "/Resources/Voice_Sample", "*.mp3", SearchOption.TopDirectoryOnly);
                    foreach (string Voice in Voices)
                    {
                        string File_Name = System.IO.Path.GetFileName(Voice);
                        File.Copy(Voice, Path_Temp + "/SRTTbacon/DVPL_Extract/Resources_Out/" + File_Name, true);
                    }
                    File.Copy(Path_Temp + "/SRTTbacon_WoTB_Path.dat", Path_Temp + "/SRTTbacon/Path.dat");
                    File.Delete(Path_Temp + "/SRTTbacon_WoTB_Path.dat");
                    var startInfo = new ProcessStartInfo();
                    startInfo.FileName = Path_Temp + "/SRTTbacon/DVPL_Extract/Resources_UnPack.bat";
                    startInfo.UseShellExecute = false;
                    startInfo.CreateNoWindow = true;
                    startInfo.WorkingDirectory = Path_Temp + "/SRTTbacon/DVPL_Extract";
                    var process = Process.Start(startInfo);
                    process.WaitForExit();
                    File.Delete(Path_Temp + "/SRTTbacon.dat");
                    MessageBox.Show("キャッシュを削除しました。");
                }
                catch
                {
                    MessageBox.Show("エラーが発生しました。開発者へご連絡ください。");
                }
            }
        }
        void wc_DownloadProgressChanged_02(object sender, DownloadProgressChangedEventArgs e)
        {
            Download_Progress.Value = e.ProgressPercentage;
        }
        void wc_DownloadFileCompleted_02(object sender, AsyncCompletedEventArgs e)
        {
            if ((e.Error != null) && (!e.Cancelled))
            {
                MessageBox.Show("ダウンロード中にエラーが発生しました。内容:" + e.Error.Message);
            }
            else if (e.Cancelled)
            {
                MessageBox.Show("ダウンロードがキャンセルされました。");
            }
            else
            {
                //ダウンロードが完了したら実行
                Task task = Task.Run(() =>
                {
                    Directory.CreateDirectory(Path_Temp + "/SRTTbacon/fsbank");
                    System.IO.Compression.ZipFile.ExtractToDirectory(Path_Temp + "/SRTTbacon/fsbank.dat", Path_Temp + "/SRTTbacon/fsbank");
                });
                task.Wait();
                Download_Progress.Visibility = Visibility.Hidden;
                File.Delete(Path_Temp + "/SRTTbacon/fsbank.dat");
                MessageBox.Show("ダウンロードが完了しました。");
            }
            Download_Progress.Visibility = Visibility.Hidden;
        }
        void FMOD_Save(string File)
        {
            try
            {
                //fsbファイルから抽出されたファイル群をもとにfsbankのプロジェクトファイルを生成する
                Directory.CreateDirectory(Path + "/Resources/Projects");
                int Number_01 = 0;
                StreamWriter stw1 = System.IO.File.CreateText(Path + "/Resources/Projects/Voice_Mod.fsproj");
                stw1.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<soundbank>\n    <file_name>" + Path + "/Resources/ingame_voice_ja.fsb" + "</file_name>\n    <profile>\n        <format>MP3</format>\n        <quality>75</quality>\n    </profile>\n    <content>\n");
                stw1.Close();
                System.Text.Encoding enc = System.Text.Encoding.GetEncoding("UTF-8");
                StreamWriter writer = new StreamWriter(Path + "/Resources/Projects/Voice_Mod.fsproj", true, enc);
                while (true)
                {
                    if (System.IO.File.Exists(Path_Temp + "/SRTTbacon/Number/" + Number_01 + ".dat"))
                    {
                        StreamReader str2 = new StreamReader(Path_Temp + "/SRTTbacon/Number/" + Number_01 + ".dat");
                        writer.WriteLine("        <subsound>");
                        writer.WriteLine("            <file>" + str2.ReadLine() + "</file>");
                        writer.WriteLine("        </subsound>");
                        str2.Close();
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
                StreamWriter stw = System.IO.File.CreateText(Path_Temp + "/SRTTbacon/fsbank/Run.bat");
                stw.WriteLine("fsbank.exe");
                stw.Close();
                //コマンド(.bat)を実行
                MessageBox.Show("プロジェクトデータの作成に成功しました。\n場所:" + Path + "/Resources/Projects/Voice_Mod.fsproj");
                var startInfo = new ProcessStartInfo();
                startInfo.FileName = Path_Temp + "/SRTTbacon/fsbank/Run.bat";
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;
                startInfo.WorkingDirectory = System.IO.Path.GetTempPath() + "/SRTTbacon/fsbank";
                var process = Process.Start(startInfo);
            }
            catch
            {
                MessageBox.Show("エラー:もう一度お試しください。繰り返し表示された場合は開発者へご連絡ください。");
            }
        }
        private void Volume_S_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //サンプル音声の音量調整
            Player.settings.volume = (int)Volume_S.Value;
        }
        private void Backup_B_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("バックアップから音声に関するファイルを復元します。", "確認", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    string[] Files = Directory.GetFiles(Path + "/Resources/Backup");
                    foreach (string File in Files)
                    {
                        string File_Name = System.IO.Path.GetFileName(File);
                        if (File_Name == ("sounds.yaml"))
                        {
                            System.IO.File.Copy(File, WoTB_Path + "/sounds.yaml", true);
                            System.IO.File.Delete(WoTB_Path + "/sounds.yaml.dvpl");
                        }
                        else if (File_Name == "sounds.yaml.dvpl")
                        {
                            System.IO.File.Copy(File, WoTB_Path + "/sounds.yaml.yaml", true);
                            System.IO.File.Delete(WoTB_Path + "/sounds.yaml");
                        }
                        else if (File_Name == "sfx_high.yaml")
                        {
                            System.IO.File.Copy(File, WoTB_Path + "/Configs/Sfx/sfx_high.yaml", true);
                            System.IO.File.Delete(WoTB_Path + "/Configs/Sfx/sfx_high.yaml.dvpl");
                        }
                        else if (File_Name == "sfx_high.yaml.dvpl")
                        {
                            System.IO.File.Copy(File, WoTB_Path + "/Configs/Sfx/sfx_high.yaml.dvpl", true);
                            System.IO.File.Delete(WoTB_Path + "/Configs/Sfx/sfx_high.yaml");
                        }
                        else if (File_Name == "sfx_low.yaml")
                        {
                            System.IO.File.Copy(File, WoTB_Path + "/Configs/Sfx/sfx_low.yaml", true);
                            System.IO.File.Delete(WoTB_Path + "/Configs/Sfx/sfx_low.yaml.dvpl");
                        }
                        else if (File_Name == "sfx_low.yaml.dvpl")
                        {
                            System.IO.File.Copy(File, WoTB_Path + "/Configs/Sfx/sfx_low.yaml.dvpl", true);
                            System.IO.File.Delete(WoTB_Path + "/Configs/Sfx/sfx_low.yaml");
                        }
                    }
                    if (WoTB_Mods_Directory == true && Directory.Exists(Path + "/Resources/Mods"))
                    {
                        if (Directory.Exists(WoTB_Path + "/Mods"))
                        {
                            Directory.Delete(WoTB_Path + "/Mods", true);
                        }
                        Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(Path + "/Resources/Backup/Mods", WoTB_Path + "/Mods");
                    }
                    else if (WoTB_Mods_Directory == true)
                    {
                        MessageBox.Show("一部のファイルが復元されませんでした。\nバックアップファイルが操作された可能性があります。");
                    }
                    MessageBox.Show("完了しました。");
                }
                catch
                {
                    MessageBox.Show("エラー:バックアップファイルが存在しない可能性があります。");
                }
            }
        }
        private void Zip_Mod_B_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Title = "音声Modのzipファイルを選択してください。";
            ofd.Multiselect = false;
            ofd.Filter = "Zipファイル(*.zip)|*.zip";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (Directory.Exists(Path_Temp + "/SRTTbacon/Select_Voice_Mod"))
                {
                    Directory.Delete(Path_Temp + "/SRTTbacon/Select_Voice_Mod", true);
                }
                Directory.CreateDirectory(Path_Temp + "/SRTTbacon/Select_Voice_Mod");
                Task task = Task.Run(() =>
                {
                    System.IO.Compression.ZipFile.ExtractToDirectory(ofd.FileName, Path_Temp + "/SRTTbacon/Select_Voice_Mod");
                });
                task.Wait();
                Choice_Voice_Mod();
            }
        }
        private void Directory_Mod_B_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.Description = "音声Modがあるフォルダを選んでください。";
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (Directory.Exists(Path_Temp + "/SRTTbacon/Select_Voice_Mod"))
                {
                    Directory.Delete(Path_Temp + "/SRTTbacon/Select_Voice_Mod", true);
                }
                Directory.CreateDirectory(Path_Temp + "/SRTTbacon/Select_Voice_Mod");
                Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(fbd.SelectedPath, Path_Temp + "/SRTTbacon/Select_Voice_Mod");
                Choice_Voice_Mod();
            }
        }
        void Choice_Voice_Mod()
        {
            try
            {
                bool IsSounds_File = false;
                bool IsConfigs_File = false;
                bool IsIngame_Voice_File = false;
                string[] Files = Directory.GetFiles(Path_Temp + "/SRTTbacon/Select_Voice_Mod", "*", SearchOption.AllDirectories);
                Sfx_High_Low_Exist();
                foreach (string File in Files)
                {
                    string File_Name = System.IO.Path.GetFileName(File);
                    if (File_Name == "sounds.yaml")
                    {
                        IsSounds_File = true;
                        System.IO.File.Copy(File, WoTB_Path + "/sounds.yaml", true);
                        System.IO.File.Delete(WoTB_Path + "/sounds.yaml.dvpl");
                    }
                    else if (File_Name == "sounds.yaml.dvpl")
                    {
                        IsSounds_File = true;
                        System.IO.File.Copy(File, WoTB_Path + "/sounds.yaml.dvpl", true);
                        System.IO.File.Delete(WoTB_Path + "/sounds.yaml");
                    }
                    else if (File_Name == "sfx_high.yaml")
                    {
                        IsConfigs_File = true;
                        System.IO.File.Copy(File, WoTB_Path + "/Configs/Sfx/sfx_high.yaml", true);
                        System.IO.File.Delete(WoTB_Path + "/Configs/Sfx/sfx_high.yaml.dvpl");
                    }
                    else if (File_Name == "sfx_high.yaml.dvpl")
                    {
                        IsConfigs_File = true;
                        System.IO.File.Copy(File, WoTB_Path + "/Configs/Sfx/sfx_high.yaml.dvpl", true);
                        System.IO.File.Delete(WoTB_Path + "/Configs/Sfx/sfx_high.yaml");
                    }
                    else if (File_Name == "sfx_low.yaml")
                    {
                        IsConfigs_File = true;
                        System.IO.File.Copy(File, WoTB_Path + "/Configs/Sfx/sfx_low.yaml", true);
                        System.IO.File.Delete(WoTB_Path + "/Configs/Sfx/sfx_low.yaml.dvpl");
                    }
                    else if (File_Name == "sfx_low.yaml.dvpl")
                    {
                        IsConfigs_File = true;
                        System.IO.File.Copy(File, WoTB_Path + "/Configs/Sfx/sfx_low.yaml.dvpl", true);
                        System.IO.File.Delete(WoTB_Path + "/Configs/Sfx/sfx_low.yaml");
                    }
                    else if (File_Name.Contains(".fsb") || File_Name.Contains(".fev"))
                    {
                        Directory.CreateDirectory(WoTB_Path + "/Mods");
                        System.IO.File.Copy(File, WoTB_Path + "/Mods/" + File_Name, true);
                    }
                    if (File_Name.Contains("ingame_voice") && !File_Name.Contains(".fev"))
                    {
                        IsIngame_Voice_File = true;
                        StreamReader str = new StreamReader(WoTB_Path + "/Configs/Sfx/sfx_high.yaml");
                        string str2 = str.ReadToEnd().Replace("~res:/Sfx/ingame_voice.fev", "~res:/Mods/ingame_voice.fev");
                        str.Close();
                        StreamReader str3 = new StreamReader(WoTB_Path + "/Configs/Sfx/sfx_high.yaml");
                        string str4 = str3.ReadToEnd().Replace("~res:/Sfx/ingame_voice_low.fev", "~res:/Mods/ingame_voice_low.fev");
                        str3.Close();
                        StreamWriter stw = System.IO.File.CreateText(WoTB_Path + "/Configs/Sfx/sfx_high.yaml");
                        stw.Write(str2);
                        stw.Close();
                        StreamWriter stw2 = new StreamWriter(WoTB_Path + "/Configs/Sfx/sfx_low.yaml");
                        stw2.Write(str4);
                        stw2.Close();
                    }
                }
                if (IsIngame_Voice_File == true)
                {
                    if (File.Exists(WoTB_Path + "/Sfx/ingame_voice.fev.dvpl"))
                    {
                        File.Copy(WoTB_Path + "/Sfx/ingame_voice.fev.dvpl", WoTB_Path + "/Mods/ingame_voice.fev.dvpl", true);
                        File.Copy(WoTB_Path + "/Sfx/ingame_voice_low.fev.dvpl", WoTB_Path + "/Mods/ingame_voice_low.fev.dvpl", true);
                    }
                    else if (File.Exists(WoTB_Path + "/Sfx/ingame_voice.fev"))
                    {
                        File.Copy(WoTB_Path + "/Sfx/ingame_voice.fev", WoTB_Path + "/Mods/ingame_voice.fev", true);
                        File.Copy(WoTB_Path + "/Sfx/ingame_voice_low.fev", WoTB_Path + "/Mods/ingame_voice_low.fev", true);
                    }
                    else
                    {
                        MessageBox.Show("WoTBフォルダ内にingame_voice.fevファイルが存在しません。\nMod内にfevファイルがあればこのエラーは出ません。");
                    }
                }
                if (IsSounds_File == false)
                {
                    Uri uri = new Uri("https://www.dropbox.com/s/0sw6kivbmguyn5m/sounds.yaml.dvpl?dl=1");
                    if (wc == null)
                    {
                        wc = new WebClient();
                        wc.DownloadFileCompleted += delegate
                        {
                            File.Copy(Path_Temp + "/SRTTbacon/sounds.yaml.dvpl", WoTB_Path + "/sounds.yaml.dvpl", true);
                            File.Delete(Path_Temp + "/SRTTbacon/sounds.yaml.dvpl");
                            File.Delete(WoTB_Path + "/sounds.yaml");
                        };
                    }
                    wc.DownloadFileAsync(uri, TempPath + "/SRTTbacon/sounds.yaml.dvpl");
                }
                MessageBox.Show("導入されました。");
            }
            catch
            {
                MessageBox.Show("エラーが発生しました。開発者へご連絡ください。");
            }
        }
        void Sfx_High_Low_Exist()
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
                foreach (string File in Files)
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
            }
        }
    }
}