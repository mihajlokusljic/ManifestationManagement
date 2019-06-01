﻿using System;
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
            view.autoGenerateId.Visibility = Visibility.Collapsed;
            view.autoGenerateIdLabel.Visibility = Visibility.Collapsed;
            view.AddOrEditBtn.Content = "Confirm changes";
            MainContent.Content = view;
        }

        public void showManifestationEditView(string manifId) //prepraviti
        {
            model.Manifestation target = model.Repository.GetInstance().FindManifestation(manifId);
            if (target == null)
            {
                return;
            }

            AddManifestationView view = new AddManifestationView(this, true);
            view.idInput.Text = target.Id;
            view.idInput.IsEnabled = false;
            view.nameInput.Text = target.Name;
            view.descriptionInput.Text = target.Description;
            view.comboBoxTypes.SelectedItem = target.Labels[0].Id;


            view.autoGenerateId.Visibility = Visibility.Collapsed;
            view.autoGenerateIdLabel.Visibility = Visibility.Collapsed;
            view.AddOrEditBtn.Content = "Confirm changes";
            MainContent.Content = view;
        }

        private void showNoviSadMap(object sender, RoutedEventArgs e)
        {
            Map noviSadMap = Repository.GetInstance().GetMap((int)MapIds.NoviSad);
            MainContent.Content = new MapView(noviSadMap);
        }
    }
}