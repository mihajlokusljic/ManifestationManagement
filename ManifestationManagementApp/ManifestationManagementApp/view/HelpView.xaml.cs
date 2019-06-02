using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for HelpView.xaml
    /// </summary>
    public partial class HelpView : Page
    {
        public HelpView(string key)
        {
            InitializeComponent();

            string path = string.Format("..\\..\\resources\\documentation\\{0}.html", key);

            if (!File.Exists(path))
                key = "Error";

            Console.WriteLine(key);

            string absolutePath = System.IO.Path.GetFullPath(string.Format("..\\..\\resources\\documentation\\{0}.html", key));
            Uri url = new Uri(string.Format("file:///{0}", absolutePath));

            HelpBrowser.Navigate(url);
        }
    }
}
