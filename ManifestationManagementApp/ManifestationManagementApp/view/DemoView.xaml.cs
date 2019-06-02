using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ManifestationManagementApp.view
{
    /// <summary>
    /// Interaction logic for DemoView.xaml
    /// </summary>
    public partial class DemoView : Page
    {

        public DemoView()
        {
            InitializeComponent();
            string absolutePath = System.IO.Path.GetFullPath(string.Format("..\\..\\resources\\video\\Clock_Face_2Videvo.mov"));
            player.Source = new Uri(absolutePath);
            player.Play();
            player.Pause();
            string absolutePath2 = System.IO.Path.GetFullPath(string.Format("..\\..\\resources\\icons\\play.png"));
            startImage.Source = BitmapFrame.Create(new Uri(absolutePath2));
            string absolutePath3 = System.IO.Path.GetFullPath(string.Format("..\\..\\resources\\icons\\pause.png"));
            pauseImage.Source = BitmapFrame.Create(new Uri(absolutePath3));
            string absolutePath4 = System.IO.Path.GetFullPath(string.Format("..\\..\\resources\\icons\\stop.png"));
            stopImage.Source = BitmapFrame.Create(new Uri(absolutePath4));
        }

        private void buttonPlay_Click(object sender, RoutedEventArgs e)
        {
            player.Play();
        }

        private void buttonPause_Click(object sender, RoutedEventArgs e)
        {
            player.Pause();
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            player.Stop();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            player.Play();
        }
    }
}
