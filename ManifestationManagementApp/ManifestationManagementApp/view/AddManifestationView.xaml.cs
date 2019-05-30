using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for AddManifestationView.xaml
    /// </summary>
    public partial class AddManifestationView : Page
    {

        private ManifestationType selectedType;
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string v)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(v));
            }
        }

        public ManifestationType SelectedType
        {
            get { return selectedType; }
            set
            {
                if (value != selectedType)
                {
                    selectedType = value;
                    OnPropertyChanged("Type");
                }
            }
        }
        
        public AddManifestationView()
        {
            InitializeComponent();
            comboBoxTypes.DataContext = Repository.GetInstance();
            label.DataContext = Repository.GetInstance();
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
                idInput.Text = $"manifestation{Repository.GetInstance().ManifestationCounter + 1}";
                while (Repository.GetInstance().FindManifestation(idInput.Text) != null)
                {
                    Repository.GetInstance().ManifestationCounter = Repository.GetInstance().ManifestationCounter + 1;
                    idInput.Text = $"manifestation{Repository.GetInstance().LabelCounter + 1}";
                }
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
                AddedLabelMessage.Foreground = Brushes.Red;

            }
            else if (nameInput.Text == "")
            {
                AddedLabelMessage.Content = "Please, insert name for this manifestation";
                AddedLabelMessage.Foreground = Brushes.Red;

            }
            else if (comboBoxTypes.Text == "")
            {
                AddedLabelMessage.Content = "Please, add type for this manifestation";
                AddedLabelMessage.Foreground = Brushes.Red;

            }
            else if (label.Text == "")
            {
                AddedLabelMessage.Content = "Please, add label for this manifestation";
                AddedLabelMessage.Foreground = Brushes.Red;

            }
            else
            {
                Manifestation retVal = new Manifestation();
                if (isAutoChecked)
                {
                    Repository.GetInstance().ManifestationCounter = Repository.GetInstance().ManifestationCounter + 1;
                    retVal.Id = "manifestation" + Repository.GetInstance().ManifestationCounter;
                    idInput.Text = retVal.Id;
                    while (Repository.GetInstance().FindManifestation(retVal.Id) != null)
                    {
                        Repository.GetInstance().ManifestationCounter = Repository.GetInstance().ManifestationCounter + 1;
                        retVal.Id = "manifestation" + Repository.GetInstance().ManifestationCounter;
                        idInput.Text = retVal.Id;
                    }

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
                AddedLabelMessage.Foreground = Brushes.Green;
                if (autoGenerateId.IsChecked.Value)
                {
                    // ako je izabrano automatsko inkrementiranje, azurira se vrijednost za id
                    idInput.Text = $"manifestation{Repository.GetInstance().ManifestationCounter + 1}";
                    while (Repository.GetInstance().FindManifestation(idInput.Text) != null)
                    {
                        Repository.GetInstance().ManifestationCounter = Repository.GetInstance().ManifestationCounter + 1;
                        idInput.Text = $"manifestation{Repository.GetInstance().ManifestationCounter + 1}";
                    }
                }
            }

        }
    }
}
