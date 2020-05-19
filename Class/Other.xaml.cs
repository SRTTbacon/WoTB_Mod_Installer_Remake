using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace WoTB_Mod_installer_Remake.Class
{
    public partial class Other : Window
    {
        bool Exit = false;
        public Other()
        {
            InitializeComponent();
            MouseLeftButtonDown += (sender, e) => { DragMove(); };
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
        private async void Back_B_Click(object sender, RoutedEventArgs e)
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
                        BGM_Form f = new BGM_Form();
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
        private async void Music_Player_B_Click(object sender, RoutedEventArgs e)
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
                        Music_Player f = new Music_Player();
                        f.Show();
                        Close();
                        break;
                    }
                    await Task.Delay(30);
                }
            }
        }
        private async void Video_Player_B_Click(object sender, RoutedEventArgs e)
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
                        Video_Player f = new Video_Player();
                        f.Show();
                        Close();
                        break;
                    }
                    await Task.Delay(30);
                }
            }
        }
        private void Source_Code_B_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("SRTTbaconのGithubのページを開きます。", "確認", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                var process = Process.Start("https://github.com/SRTTbacon");
            }
        }
    }
}
