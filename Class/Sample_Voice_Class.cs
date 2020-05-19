using System.Windows;
using System;

namespace WoTB_Mod_installer_Remake
{
    public partial class MainWindow : Window
    {
        //どの音声Modが選択されたかを取得
        private void Sample_Voice_B_Click(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            //サンプル音声の5個の中からランダムで再生させる
            int r2 = r.Next(1, 6);
            if (Select_Mod == "野良と皇女と野良猫ハート(黒木未知)")
            {
                Sample_Voice_Play(Path_Temp + "/SRTTbacon/DVPL_Extract/Resources/Kuroki_0" + r2 + ".mp3");
            }
            if (Select_Mod == "野良と皇女と野良猫ハート(ユウラシア・オブ・エンド)")
            {
                Sample_Voice_Play(Path_Temp + "/SRTTbacon/DVPL_Extract/Resources/Furasia_0" + r2 + ".mp3");
            }
            if (Select_Mod == "野良と皇女と野良猫ハート(パトリシア・オブ・エンド)")
            {
                Sample_Voice_Play(Path_Temp + "/SRTTbacon/DVPL_Extract/Resources/Patricia_0" + r2 + ".mp3");
            }
            if (Select_Mod == "Summer_Pockets")
            {
                Sample_Voice_Play(Path_Temp + "/SRTTbacon/DVPL_Extract/Resources/Summer_Pokets_0" + r2 + ".mp3");
            }
            if (Select_Mod == "琴葉姉妹")
            {
                Sample_Voice_Play(Path_Temp + "/SRTTbacon/DVPL_Extract/Resources/Kotonoha_0" + r2 + ".mp3");
            }
            if (Select_Mod == "WoT_1.4")
            {
                Sample_Voice_Play(Path_Temp + "/SRTTbacon/DVPL_Extract/Resources/PC_1_4_0" + r2 + ".mp3");
            }
            if (Select_Mod == "セレン・ヘイズ")
            {
                Sample_Voice_Play(Path_Temp + "/SRTTbacon/DVPL_Extract/Resources/Seren_0" + r2 + ".mp3");
            }
            if (Select_Mod == "WoT(1.4以前)")
            {
                Sample_Voice_Play(Path_Temp + "/SRTTbacon/DVPL_Extract/Resources/WoT_PC_0" + r2 + ".mp3");
            }
            if (Select_Mod == "ASMR")
            {
                Sample_Voice_Play(Path_Temp + "/SRTTbacon/DVPL_Extract/Resources/ASMR_0" + r2 + ".mp3");
            }
            if (Select_Mod == "Riddle_Joker(あやせ)")
            {
                Sample_Voice_Play(Path_Temp + "/SRTTbacon/DVPL_Extract/Resources/Ayase_0" + r2 + ".mp3");
            }
            if (Select_Mod == "Riddle_Joker(七海)")
            {
                Sample_Voice_Play(Path_Temp + "/SRTTbacon/DVPL_Extract/Resources/Nanami_0" + r2 + ".mp3");
            }
            if (Select_Mod == "このすば！(ウィズ)")
            {
                Sample_Voice_Play(Path_Temp + "/SRTTbacon/DVPL_Extract/Resources/Wiss_0" + r2 + ".mp3");
            }
            if (Select_Mod == "このすば！(クリス)")
            {
                Sample_Voice_Play(Path_Temp + "/SRTTbacon/DVPL_Extract/Resources/Criss_0" + r2 + ".mp3");
            }
            if (Select_Mod == "艦これ(電、雷)")
            {
                Sample_Voice_Play(Path_Temp + "/SRTTbacon/DVPL_Extract/Resources/Kankore_0" + r2 + ".mp3");
            }
            if (Select_Mod == "ガルパン(BC自由学園)")
            {
                Sample_Voice_Play(Path_Temp + "/SRTTbacon/DVPL_Extract/Resources/GuP_BC_0" + r2 + ".mp3");
            }
            if (Select_Mod == "名取さな")
            {
                Sample_Voice_Play(Path_Temp + "/SRTTbacon/DVPL_Extract/Resources/Natori_0" + r2 + ".mp3");
            }
            if (Select_Mod == "Angel_Beats")
            {
                Sample_Voice_Play(Path_Temp + "/SRTTbacon/DVPL_Extract/Resources/Angel_Beats_0" + r2 + ".mp3");
            }
        }
        void Sample_Voice_Play(string Name)
        {
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
    }
}
