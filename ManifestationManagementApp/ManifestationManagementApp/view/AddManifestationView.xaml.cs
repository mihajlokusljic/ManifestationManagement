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
using System.Collections.ObjectModel;
using ManifestationManagementApp.model;
using Xceed.Wpf.Toolkit;

namespace ManifestationManagementApp.view
{
    /// <summary>
    /// Interaction logic for AddManifestationView.xaml
    /// </summary>
    public partial class AddManifestationView : Page
    {
        private MainWindow mainWindow;
        public bool Editing { get; set; }

        
        public ObservableCollection<model.Label> Labels { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private string labelText;
        public string LabelText
        {
            get { return labelText; }
            set
            {
                if (value != labelText)
                {
                    labelText = value;
                    OnPropertyChanged("LabelText");
                }
            }
        }

        private string isOut;
        public string IsOut
        {
            get { return isOut; }
            set
            {
                if (value != isOut)
                {
                    isOut = value;
                    OnPropertyChanged("IsOut");
                }
            }
        }

        private string alcoholCons;
        public string AlcoholCons
        {
            get { return alcoholCons; }
            set
            {
                if (value != alcoholCons)
                {
                    alcoholCons = value;
                    OnPropertyChanged("AlcoholCons");
                }
            }
        }

        private string priceCat;
        public string PriceCat
        {
            get { return priceCat; }
            set
            {
                if (value != priceCat)
                {
                    priceCat = value;
                    OnPropertyChanged("PriceCat");
                }
            }
        }

        public ObservableCollection<model.Label> selectedLabels;

        public ObservableCollection<model.Label> SelectedLabels
        {
            get { return selectedLabels; }
            set
            {
                if (value != selectedLabels)
                {
                    selectedLabels = value;
                    OnPropertyChanged("Labels");
                }
            }
        }

        private void OnPropertyChanged(string v)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(v));
            }
        }
        

        public AddManifestationView(MainWindow parent, bool editMode)
        {
            InitializeComponent();
            comboBoxTypes.DataContext = Repository.GetInstance();
            Labels = Repository.GetInstance().Labels;
            DataContext = new Manifestation();
            label.DataContext = this;
            isItOutside.DataContext = this;
            alcoholConsumption.DataContext = this;
            priceCategory.DataContext = this;
            mainWindow = parent;
            labelText = "";
            isOut = "";
            selectedLabels = new ObservableCollection<model.Label>();
            Editing = editMode;
            descriptionInput.Focus();
        }

        public AddManifestationView()
        {
            InitializeComponent();
            comboBoxTypes.DataContext = Repository.GetInstance();
            Labels = Repository.GetInstance().Labels;
            DataContext = new Manifestation();
            label.DataContext = this;
            isItOutside.DataContext = this;
            alcoholConsumption.DataContext = this;
            priceCategory.DataContext = this; label.DataContext = this;
            Editing = false;
            labelText = "";
            isOut = "";
            selectedLabels = new ObservableCollection<model.Label>();
            descriptionInput.Focus();
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

        private void ShowHelp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.mainWindow.MainContent.Content = new HelpView("AddManifestationHelp");
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
            if (!Editing)
            {
                idInput.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
            comboBoxTypes.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
            nameInput.GetBindingExpression(TextBox.TextProperty).UpdateSource();
           // label.GetBindingExpression(CheckComboBox.SelectedValueProperty).UpdateSource();
            isItOutside.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
            alcoholConsumption.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
            priceCategory.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();

            descriptionInput.GetBindingExpression(TextBox.TextProperty).UpdateSource();
     //       datePicker1.GetBindingExpression(DatePicker.TextProperty).UpdateSource();
            

            if (idInput.Text == "" || nameInput.Text == "" || comboBoxTypes.Text == "" || label.Text=="" ||
                descriptionInput.Text == "" || priceCategory.Text == "" || alcoholConsumption.Text == "" ||
                datePicker1.Text == "" || isItOutside.Text == "" || (Repository.GetInstance().FindManifestation(idInput.Text) != null && !isAutoChecked && !Editing))
            {
                AddedLabelMessage.Content = "Manifestation has not been added successfully";
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
                if (alcohol.Equals("No alcohol"))
                {
                    retVal.Alcohol = model.AlcoholConusmption.Forbidden;
                }
                else if (alcohol.Equals("Allowed to bring alcohol"))
                {
                    retVal.Alcohol = model.AlcoholConusmption.BringAlcohol;
                }
                else if (alcohol.Equals("Allowed to buy alcohol"))
                {
                    retVal.Alcohol = model.AlcoholConusmption.BuyAlcohol;
                }
                else
                {
                    retVal.Alcohol = model.AlcoholConusmption.Forbidden;
                }

                String outside = isItOutside.Text;
                if (outside.Equals("Outside"))
                {
                    retVal.IsOutside = true;
                }
                else if (outside.Equals("Inside"))
                {
                    retVal.IsOutside = false;
                }
                else
                {
                    retVal.IsOutside = true;
                }

                retVal.Date = DateTime.Parse(datePicker1.Text);
                retVal.Description = descriptionInput.Text;
                retVal.Type = Repository.GetInstance().FindManifestationType(comboBoxTypes.Text);
                

                if (textBoxIconPath.Text == "")
                {
                    retVal.IconPath = retVal.Type.IconPath;
                }
                else
                {
                    retVal.IconPath = textBoxIconPath.Text;
                }

                foreach (model.Label lab in SelectedLabels)
                {
                    model.Label labela = Repository.GetInstance().FindLabel(lab.Id);
                    if (labela != null)
                    {
                        retVal.Addlabel(Repository.GetInstance().FindLabel(lab.Id));
                    }
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

        private void IdInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        
    }
}
