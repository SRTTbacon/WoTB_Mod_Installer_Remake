using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;

namespace WoTB_Mod_installer_Remake
{
    public partial class MainWindow : Window
    {
        //サンプル音声の指定
        void Select_Mod_Download()
        {
            if (Directory.Exists(TempPath + "/SRTTbacon/Voice_Mod"))
            {
                Directory.Delete(TempPath + "/SRTTbacon/Voice_Mod", true);
            }
            Download_P.Visibility = Visibility.Visible;
            BGM_Form_B.Visibility = Visibility.Hidden;
            Mod_List.Visibility = Visibility.Hidden;
            Mod_Install_B.Visibility = Visibility.Hidden;
            Exit_B.Visibility = Visibility.Hidden;
            Mod_Download_B.Visibility = Visibility.Hidden;
            Sample_Voice_B.Visibility = Visibility.Hidden;
            string URL = "";
            if (Select_Mod == "野良と皇女と野良猫ハート(黒木未知)")
            {
                URL = "https://www.dropbox.com/s/b5vrm887z4qv2ip/NoraNekoHeart_Mod.zip?dl=1";
            }
            if (Select_Mod == "野良と皇女と野良猫ハート(ユウラシア・オブ・エンド)")
            {
                URL = "https://www.dropbox.com/s/njdgvehda98aix3/Voices_Furacia.dat?dl=1";
            }
            if (Select_Mod == "野良と皇女と野良猫ハート(パトリシア・オブ・エンド)")
            {
                URL = "https://www.dropbox.com/s/gcuvbrupubkknkw/Patricia_Mod.zip?dl=1";
            }
            if (Select_Mod == "Summer_Pockets")
            {
                URL = "https://www.dropbox.com/s/sof6x16ikkqk4ql/Summer_Pockets_Mod.zip?dl=1";
            }
            if (Select_Mod == "琴葉姉妹")
            {
                URL = "https://www.dropbox.com/s/fi61ebwjscz4jsg/kotonoha_aoi.zip?dl=1";
            }
            if (Select_Mod == "WoT_1.4")
            {
                URL = "https://www.dropbox.com/s/gk43ggrpztd5kz7/WoT_1.4_Mod.zip?dl=1";
            }
            if (Select_Mod == "セレン・ヘイズ")
            {
                URL = "https://www.dropbox.com/s/zwnru3s3gcf0rmt/seren_Mod.zip?dl=1";
            }
            if (Select_Mod == "WoT(1.4以前)")
            {
                URL = "https://www.dropbox.com/s/wneeeimhq1hh0vy/PC_Voice_MOD.zip?dl=1";
            }
            if (Select_Mod == "ASMR")
            {
                URL = "https://www.dropbox.com/s/ay5714v365vrhl6/Slave_Voice_Mod.zip?dl=1";
            }
            if (Select_Mod == "Riddle_Joker(あやせ)")
            {
                URL = "https://www.dropbox.com/s/2b76zb9pghf1get/Ayase_Voice_Mod.zip?dl=1";
            }
            if (Select_Mod == "Riddle_Joker(七海)")
            {
                URL = "https://www.dropbox.com/s/l2dilxqi2bpl3ga/Nanami_Mod.zip?dl=1";
            }
            if (Select_Mod == "このすば！(ウィズ)")
            {
                URL = "https://www.dropbox.com/s/qu3ox0bclvf9sit/Wiss_Voice_Mod.zip?dl=1";
            }
            if (Select_Mod == "このすば！(クリス)")
            {
                URL = "https://www.dropbox.com/s/tuzto4kojfr2aeh/Criss_Mod.zip?dl=1";
            }
            if (Select_Mod == "艦これ(電、雷)")
            {
                URL = "https://www.dropbox.com/s/2d16p0m7zkg6y51/KankoreVoice_V2.zip?dl=1";
            }
            if (Select_Mod == "ガルパン(BC自由学園)")
            {
                URL = "https://www.dropbox.com/s/3nvykszgtkugzwp/GuP_BC.zip?dl=1";
            }
            if (Select_Mod == "名取さな")
            {
                URL = "https://www.dropbox.com/s/o3adovfm5t2hc8l/Natori_Voice_Mod.zip?dl=1";
            }
            if (Select_Mod == "Angel_Beats")
            {
                URL = "https://www.dropbox.com/s/smsqz5z9krq1cpx/Angel_Beats.zip?dl=1";
            }
            Uri uri = new Uri(URL);
            if (wc == null)
            {
                wc = new WebClient();
                wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged);
                wc.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_DownloadFileCompleted);
            }
            wc.DownloadFileAsync(uri, TempPath + "/SRTTbacon/Voice_Mod.dat");
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
                System.Windows.Forms.MessageBox.Show("ダウンロード中にエラーが発生しました。内容:" + e.Error.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (e.Cancelled)
            {
                System.Windows.Forms.MessageBox.Show("ダウンロードがキャンセルされました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("データのダウンロードが完了しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Voice_Mod_Extract();
            }
            Download_P.Visibility = Visibility.Hidden;
            BGM_Form_B.Visibility = Visibility.Visible;
            Mod_Install_B.Visibility = Visibility.Visible;
            Exit_B.Visibility = Visibility.Visible;
            Mod_Download_B.Visibility = Visibility.Visible;
            Sample_Voice_B.Visibility = Visibility.Visible;
        }
        void Voice_Mod_Extract()
        {
            Task task = Task.Run(() =>
            {
                System.IO.Compression.ZipFile.ExtractToDirectory(TempPath + "/SRTTbacon/Voice_Mod.dat", TempPath + "/SRTTbacon/Voice_Mod", System.Text.Encoding.UTF8);
            });
            task.Wait();
            File.Delete(TempPath + "/SRTTbacon/Voice_Mod.dat");
            Voice_Mod_Install();
        }
        void Voice_Mod_Install()
        {
            string[] Files = Directory.GetFiles(TempPath + "/SRTTbacon/Voice_Mod", "*", SearchOption.AllDirectories);
            foreach (string FileName in Files)
            {
                string Name = System.IO.Path.GetFileName(FileName);
                if (FileName.Contains(".fev") || FileName.Contains(".fsb"))
                {//.yamlがコピーされない。。。
                    //第三因数のtrueは上書きをするか
                    Directory.CreateDirectory(WoTB_Path + "/Mods");
                    File.Copy(FileName, WoTB_Path + "/Mods/" + Name, true);
                }
                else if (FileName.Contains("sfx_high.yaml") || FileName.Contains("sfx_low.yaml"))
                {
                    File.Copy(FileName, WoTB_Path + "/Configs/Sfx/" + Name, true);
                    if (!FileName.Contains(".dvpl"))
                    {
                        File.Delete(WoTB_Path + "/Configs/Sfx/sfx_high.yaml.dvpl");
                        File.Delete(WoTB_Path + "/Configs/Sfx/sfx_low.yaml.dvpl");
                    }
                }
                else if (FileName.Contains("sounds.yaml"))
                {
                    File.Copy(FileName, WoTB_Path + "/" + Name, true);
                    if (!FileName.Contains(".dvpl"))
                    {
                        File.Delete(WoTB_Path + "/sounds.yaml.dvpl");
                    }
                }
            }
            Directory.Delete(TempPath + "/SRTTbacon/Voice_Mod", true);
            System.Windows.MessageBox.Show("正常に完了しました。");
        }
    }
}
