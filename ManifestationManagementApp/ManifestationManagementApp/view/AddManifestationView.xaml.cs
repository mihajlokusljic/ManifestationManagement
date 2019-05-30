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
    /// Interaction logic for AddManifestationView.xaml
    /// </summary>
    public partial class AddManifestationView : Page
    {
        public AddManifestationView()
        {
            InitializeComponent();
        }

        private void loadIcon_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Image files (*.png;*.jpg,*.ico)|*.ico;*.png;*.jpg";
            if (dialog.ShowDialog() == true)
            {
                textBoxIconPath.Text = dialog.FileName;
            }
        }
        private void AutoGenerateIdClicked(object sender, RoutedEventArgs e)
        {
            bool isAutoChecked = autoGenerateId.IsChecked.Value;
            if (isAutoChecked)
            {
                idInput.IsEnabled = false;
                idInput.Text = $"manifestation{model.Label.counter + 1}";
            }
            else
            {
                idInput.IsEnabled = true;
                idInput.Text = "";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool isAutoChecked = autoGenerateId.IsChecked.Value;
      
            if (idInput.Text == "" && !isAutoChecked)
            {
                AddedLabelMessage.Content = "Please, insert id for this manifestation";
            }
            else if (nameInput.Text == "")
            {
                AddedLabelMessage.Content = "Please, insert name for this manifestation";
            }
            else if (comboBoxTypes.Text == "")
            {
                AddedLabelMessage.Content = "Please, add type for this manifestation";
            }
            else if (label.Text == "")
            {
                AddedLabelMessage.Content = "Please, add label for this manifestation";
            }
            else
            {
                model.Manifestation retVal = new model.Manifestation();
                if (isAutoChecked)
                {
                    model.Manifestation.counter = model.Label.counter + 1;
                    retVal.Id = "manifestation" + model.Label.counter;
                    idInput.Text = $"manifestation{model.Manifestation.counter}";

                }
                else
                {
                    retVal.Id = idInput.Text;
                }
                retVal.Name = nameInput.Text;
                
                if (smokingAllowed.IsChecked.Value)
                {
                    retVal.SmokingAllowed = true;
                }
                else
                {
                    retVal.SmokingAllowed = false;
                }

                if (peopleWithSpecialSupport.IsChecked.Value)
                {
                    retVal.SupportHandicaped = true;
                }
                else
                {
                    retVal.SupportHandicaped = false;
                }

                String price = priceCategory.Text;
                if (price.Equals("Free"))
                {
                    retVal.Prices = model.PriceCategory.Free;
                }
                else if (price.Equals("Low price"))
                {
                    retVal.Prices = model.PriceCategory.LowPrices;
                }
                else if (price.Equals("Medium price"))
                {
                    retVal.Prices = model.PriceCategory.MediumPrices;
                }
                else if (price.Equals("High price"))
                {
                    retVal.Prices = model.PriceCategory.HighPrices;
                }
                retVal.IconPath = textBoxIconPath.Text;
                retVal.Description = descriptionInput.Text;
                model.Repository rep = model.Repository.GetInstance();
                rep.AddManifestation(retVal);
                AddedLabelMessage.Content = "Manifestation " + retVal.Id + " has been added successfully.";
                if (autoGenerateId.IsChecked.Value)
                {
                    //ako je izabrano automatsko inkrementiranje, azurira se vrijednost za id
                    idInput.Text = $"manifestation{model.Manifestation.counter + 1}";
                }
            }

        }
    }
}
