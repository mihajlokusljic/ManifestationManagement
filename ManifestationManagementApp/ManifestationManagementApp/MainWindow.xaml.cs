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
using ManifestationManagementApp.model;

namespace ManifestationManagementApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            model.Repository.GetInstance().ReadData();
            InitializeComponent();
        }

        private void ShowHelp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.MainContent.Content = new HelpView("StartHelpPage");
        }

        private void ShowNoviSad_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Map noviSadMap = Repository.GetInstance().GetMap((int)MapIds.NoviSad);
            MainContent.Content = new MapView(noviSadMap, this);
        }

        private void ShowStariGrad_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Map stariGradMap = Repository.GetInstance().GetMap((int)MapIds.StariGrad);
            MainContent.Content = new MapView(stariGradMap, this);
        }

        private void ShowDetelinara_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Map detelinaraMap = Repository.GetInstance().GetMap((int)MapIds.Detelinara);
            MainContent.Content = new MapView(detelinaraMap, this);
        }

        private void ShowLiman_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Map limanMap = Repository.GetInstance().GetMap((int)MapIds.Liman);
            MainContent.Content = new MapView(limanMap, this);
        }

        private void AddManifestation_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MainContent.Content = new AddManifestationView(this, false);
        }

        private void ShowManifestation_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.MainContent.Content = new ManifestationsView(this);
        }

        private void AddManifestationType_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.MainContent.Content = new AddManifTypeView(this, false);
        }

        private void ShowManifestationType_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.MainContent.Content = new ManifestationTypesView(this);
        }

        private void AddLabel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.MainContent.Content = new AddLabelView(this, false);
        }

        private void ShowLabel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.MainContent.Content = new LabelsView(this);
        }

        private void showAddLabelView(object sender, RoutedEventArgs e)
        {
            this.MainContent.Content = new AddLabelView(this, false);
        }

        private void showAddManifTypeView(object sender, RoutedEventArgs e)
        {
            this.MainContent.Content = new AddManifTypeView(this, false);
        }

        private void showLabelsView(object sender, RoutedEventArgs e)
        {
            this.MainContent.Content = new LabelsView(this);
        }

        private void showAddManifestationView(object sender, RoutedEventArgs e)
        {
            this.MainContent.Content = new AddManifestationView(this, false);
        }

        private void showManifestationTypesView(object sender, RoutedEventArgs e)
        {
            this.MainContent.Content = new ManifestationTypesView(this);
        }

        private void showManifestationsView(object sender, RoutedEventArgs e)
        {
            this.MainContent.Content = new ManifestationsView(this);
        }

        private void showDocumentation(object sender, RoutedEventArgs e)
        {
            this.MainContent.Content = new HelpView("StartHelpPage");
        }

        private void showDemo(object sender, RoutedEventArgs e)
        {
            this.MainContent.Content = new DemoView();
        }

        public void showLabelEditView(string labelId)
        {
            model.Label target = model.Repository.GetInstance().FindLabel(labelId);
            if(target == null)
            {
                return;
            }
            AddLabelView view = new AddLabelView(this, true);
            view.idInput.Text = target.Id;
            view.idInput.IsEnabled = false;
            view.colorPicker.SelectedColor = (Color)ColorConverter.ConvertFromString(target.Color);
            view.descriptionInput.Text = target.Description;
            view.autoGenerateId.Visibility = Visibility.Collapsed;
            view.autoGenerateIdLabel.Visibility = Visibility.Collapsed;
            view.AddOrEditBtn.Content = "Confirm changes";
            MainContent.Content = view;
        }

        public void showManifestationTypeEditView(string typeId)
        {
            model.ManifestationType target = model.Repository.GetInstance().FindManifestationType(typeId);
            if (target == null)
            {
                return;
            }
            AddManifTypeView view = new AddManifTypeView(this, true);
            view.idInput.Text = target.Id;
            view.idInput.IsEnabled = false;
            view.nameInput.Text = target.Name;
            view.descriptionInput.Text = target.Description;
            view.textBoxIconPath.Text = target.IconPath;
            view.autoGenerateId.Visibility = Visibility.Collapsed;
            view.autoGenerateIdLabel.Visibility = Visibility.Collapsed;
            view.AddOrEditBtn.Content = "Confirm changes";
            MainContent.Content = view;
        }

        public void showManifestationEditView(string manifId)
        {
            model.Manifestation target = model.Repository.GetInstance().FindManifestation(manifId);
            if (target == null)
            {
                return;
            }
            AddManifestationView view = new AddManifestationView(this, true);


            view.comboBoxTypes.SelectedIndex = Repository.GetInstance().ManifestationTypes.IndexOf(Repository.GetInstance().FindManifestationType(target.Type.Id));
            view.label.SelectedIndex = Repository.GetInstance().Labels.IndexOf(Repository.GetInstance().FindLabel(target.Labels.FirstOrDefault().Id));
            view.idInput.Text = target.Id;
            view.idInput.IsEnabled = false;
            view.nameInput.Text = target.Name;
            view.descriptionInput.Text = target.Description;

            view.priceCategory.SelectedIndex = (int) target.Prices;
            view.isItOutside.SelectedIndex = target.IsOutside ? 0 : 1;
            view.alcoholConsumption.SelectedIndex = (int) target.Alcohol;
            view.textBoxIconPath.Text = target.IconPath;
            view.datePicker1.Text = target.Date.ToString();
            view.smokingAllowed.IsChecked = target.SmokingAllowed;
            view.peopleWithSpecialSupport.IsChecked = target.SupportHandicaped;

            view.autoGenerateId.Visibility = Visibility.Collapsed;
            view.autoGenerateIdLabel.Visibility = Visibility.Collapsed;
            view.AddOrEditBtn.Content = "Confirm changes";
            MainContent.Content = view;
        }

        private void showNoviSadMap(object sender, RoutedEventArgs e)
        {
            Map noviSadMap = Repository.GetInstance().GetMap((int)MapIds.NoviSad);
            MainContent.Content = new MapView(noviSadMap, this);
        }

        private void showStariGradMap(object sender, RoutedEventArgs e)
        {
            Map stariGradMap = Repository.GetInstance().GetMap((int)MapIds.StariGrad);
            MainContent.Content = new MapView(stariGradMap, this);
        }

        private void showDetelinaraMap(object sender, RoutedEventArgs e)
        {
            Map detelinaraMap = Repository.GetInstance().GetMap((int)MapIds.Detelinara);
            MainContent.Content = new MapView(detelinaraMap, this);
        }

        private void showLimanMap(object sender, RoutedEventArgs e)
        {
            Map limanMap = Repository.GetInstance().GetMap((int)MapIds.Liman);
            MainContent.Content = new MapView(limanMap, this);
        }
    }
}