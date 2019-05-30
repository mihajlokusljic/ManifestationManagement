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
using ManifestationManagementApp.view;

namespace ManifestationManagementApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void showAddLabelView(object sender, RoutedEventArgs e)
        {
            this.MainContent.Content = new AddLabelView();
        }

        private void showAddManifTypeView(object sender, RoutedEventArgs e)
        {
            this.MainContent.Content = new AddManifTypeView();
        }

        private void showLabelsView(object sender, RoutedEventArgs e)
        {
            this.MainContent.Content = new LabelsView();
        }
    }
}
