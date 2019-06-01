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
        private MainWindow mainWindow;
        public bool Editing { get; set; }

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

        public AddManifestationView(MainWindow parent, bool editMode)
        {
            InitializeComponent();
            comboBoxTypes.DataContext = Repository.GetInstance();
            label.DataContext = Repository.GetInstance();
            mainWindow = parent;
            Editing = editMode;
        }

        public AddManifestationView()
        {
            InitializeComponent();
            comboBoxTypes.DataContext = Repository.GetInstance();
            label.DataContext = Repository.GetInstance();
            Editing = false;
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
                    idInput.Text = $"manifestation{Repository.GetInstance().ManifestationCounter + 1}";
                }
            }
            else
            {
                idInput.IsEnabled = true;
                idInput.Text = "";
            }
        }

        private void addManifBtnClicked(object sender, RoutedEventArgs e) //ispraviti sa editom
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
            else if (textBoxIconPath.Text == "")
            {
                AddedLabelMessage.Content = "Please, add icon for this manifestation";
                AddedLabelMessage.Foreground = Brushes.Red;

            }
            else if (descriptionInput.Text == "")
            {
                AddedLabelMessage.Content = "Please, add description for this manifestation";
                AddedLabelMessage.Foreground = Brushes.Red;

            }
            else if (priceCategory.Text == "")
            {
                AddedLabelMessage.Content = "Please, pick price category for this manifestation";
                AddedLabelMessage.Foreground = Brushes.Red;

            }
            else if (alcoholConsumption.Text == "")
            {
                AddedLabelMessage.Content = "Please, pick alcohol consumption type for this manifestation";
                AddedLabelMessage.Foreground = Brushes.Red;
            }
            else if (datePicker1.Text == "")
            {
                AddedLabelMessage.Content = "Please, pick date for this manifestation";
                AddedLabelMessage.Foreground = Brushes.Red;
            }
            else if (isItOutside.Text == "")
            {
                AddedLabelMessage.Content = "Please, pick is it outside or not for this manifestation";
                AddedLabelMessage.Foreground = Brushes.Red;
            }
            else if (Repository.GetInstance().FindManifestation(idInput.Text) != null && !isAutoChecked && !Editing)
            {
                AddedLabelMessage.Content = "This id is already taken, please insert another one";
                AddedLabelMessage.Foreground = Brushes.Red;
            }
            else
            {
                Manifestation retVal = new Manifestation();
                retVal.MapCoordinates = new System.Collections.ObjectModel.ObservableCollection<Coordinates>();
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
                else
                {
                    retVal.Prices = model.PriceCategory.Free;
                }

                String alcohol = alcoholConsumption.Text;
                if (price.Equals("No alcohol"))
                {
                    retVal.Alcohol = model.AlcoholConusmption.Forbidden;
                }
                else if (price.Equals("Allowed to bring alcohol"))
                {
                    retVal.Alcohol = model.AlcoholConusmption.BringAlcohol;
                }
                else if (price.Equals("Allowed to buy alcohol"))
                {
                    retVal.Alcohol = model.AlcoholConusmption.BuyAlcohol;
                }
                else
                {
                    retVal.Alcohol = model.AlcoholConusmption.Forbidden;
                }

                String outside = isItOutside.Text;
                if (price.Equals("Outside"))
                {
                    retVal.IsOutside = true;
                }
                else if (price.Equals("Inside"))
                {
                    retVal.IsOutside = false;
                }
                else
                {
                    retVal.IsOutside = true;
                }

                retVal.Date = DateTime.Parse(datePicker1.Text);
                retVal.IconPath = textBoxIconPath.Text;
                retVal.Description = descriptionInput.Text;
                retVal.Type = Repository.GetInstance().FindManifestationType(comboBoxTypes.Text);
                model.Label lab = Repository.GetInstance().FindLabel(label.Text);
                if (lab != null)
                {
                    retVal.Addlabel(Repository.GetInstance().FindLabel(label.Text));

                }
                model.Repository rep = model.Repository.GetInstance();
                if (!Editing)
                {
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
                else
                {
                    rep.UpdateManifestation(retVal);
                    ManifestationsView manifs = new ManifestationsView(mainWindow);
                    manifs.scrollTo(retVal.Id);
                    mainWindow.MainContent.Content = manifs;
                }
            }

        }

        private void CancelBtnClicked(object sender, RoutedEventArgs e)
        {
            ManifestationsView manifestations = new ManifestationsView(mainWindow);
            if (Editing)
            {
                manifestations.scrollTo(idInput.Text);
            }
            mainWindow.MainContent.Content = manifestations;
        }
    }
}
