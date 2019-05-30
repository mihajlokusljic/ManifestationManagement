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
using ManifestationManagementApp.model;

namespace ManifestationManagementApp.view
{
    /// <summary>
    /// Interaction logic for AddManifTypeView.xaml
    /// </summary>
    public partial class AddManifTypeView : Page
    {
        public AddManifTypeView()
        {
            InitializeComponent();
        }

        private void autoGenerateIdsBtnClicked(object sender, RoutedEventArgs e)
        {
            bool isAutoChecked = autoGenerateId.IsChecked.Value;
            if (isAutoChecked)
            {
                idInput.IsEnabled = false;
                idInput.Text = $"type{ManifestationType.counter + 1}";
            }
            else
            {
                idInput.IsEnabled = true;
                idInput.Text = "";
            }
        }
    }
}
