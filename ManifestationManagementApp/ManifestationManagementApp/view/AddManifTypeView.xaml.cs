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
        private MainWindow mainWindow;
        public bool Editing { get; set; }

        public AddManifTypeView()
        {
            InitializeComponent();
            Editing = false;
            DataContext = new ManifestationType();
        }

        public AddManifTypeView(MainWindow parent, bool editMode)
        {
            InitializeComponent();
            mainWindow = parent;
            Editing = editMode;
            DataContext = new ManifestationType();
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

        private void autoGenerateIdsBtnClicked(object sender, RoutedEventArgs e)
        {
            bool isAutoChecked = autoGenerateId.IsChecked.Value;
            if (isAutoChecked)
            {
                idInput.IsEnabled = false;
                idInput.Text = $"type{Repository.GetInstance().ManifestationTypeCounter + 1}";
                while (Repository.GetInstance().FindManifestationType(idInput.Text) != null)
                {
                    Repository.GetInstance().ManifestationTypeCounter = Repository.GetInstance().ManifestationTypeCounter + 1;
                    idInput.Text = $"type{Repository.GetInstance().ManifestationTypeCounter + 1}";
                }
                nameInput.Focus();
            }
            else
            {
                idInput.IsEnabled = true;
                idInput.Text = "";
                idInput.Focus();
            }
        }

        private void addTypeBtnClicked(object sender, RoutedEventArgs e)
        {
            bool isAutoChecked = autoGenerateId.IsChecked.Value;
            string typeName = nameInput.Text;
            string desc = descriptionInput.Text;
            string iconPath = textBoxIconPath.Text;
            Repository rep = Repository.GetInstance();
            string id;
            idInput.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            nameInput.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            descriptionInput.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            textBoxIconPath.GetBindingExpression(TextBox.TextProperty).UpdateSource();

            if (isAutoChecked && !Editing)
            {
                id = $"type{Repository.GetInstance().ManifestationTypeCounter + 1}";
                while (rep.FindManifestationType(id) != null)
                {
                    Repository.GetInstance().ManifestationTypeCounter = Repository.GetInstance().ManifestationTypeCounter + 1;
                    id = $"type{Repository.GetInstance().ManifestationTypeCounter + 1}";
                }
            } else
            {
                id = idInput.Text;
            }

            if(id == "" || (rep.FindManifestationType(id) != null && !Editing) || typeName == "" ||
               desc == "" || iconPath == "")
            {
                AddedTypeMessage.Content = "Manifestation type has not been added successfully.";
                AddedTypeMessage.Foreground = Brushes.Red;
            }
           
            else
            {
                ManifestationType newType = new ManifestationType()
                {
                    Id = id,
                    Name = typeName,
                    Description = desc,
                    IconPath = iconPath,
                };
                if (!Editing)
                {
                    rep.AddManifestationType(newType);
                    AddedTypeMessage.Content = $"Successfully added manifestation type: {newType.Id}";
                    AddedTypeMessage.Foreground = Brushes.Green;
                    nameInput.Text = "";
                    descriptionInput.Text = "";
                    textBoxIconPath.Text = "";
                    if (isAutoChecked)
                    {
                        Repository.GetInstance().ManifestationTypeCounter = Repository.GetInstance().ManifestationTypeCounter + 1;
                        idInput.Text = $"type{Repository.GetInstance().ManifestationTypeCounter + 1}";
                        while (Repository.GetInstance().FindManifestationType(idInput.Text) != null)
                        {
                            Repository.GetInstance().ManifestationTypeCounter = Repository.GetInstance().ManifestationTypeCounter + 1;
                            idInput.Text = $"type{Repository.GetInstance().ManifestationTypeCounter + 1}";
                        }
                        nameInput.Focus();
                    }
                    else
                    {
                        idInput.Text = "";
                        idInput.Focus();
                    }
                }
                else
                {
                    rep.UpdateManifestationType(newType);
                    ManifestationTypesView types = new ManifestationTypesView(mainWindow);
                    types.scrollTo(newType.Id);
                    mainWindow.MainContent.Content = types;
                }


            }
        }

        private void CancelBtnClicked(object sender, RoutedEventArgs e)
        {
            ManifestationTypesView manifestationTypes = new ManifestationTypesView(mainWindow);
            if (Editing)
            {
                manifestationTypes.scrollTo(idInput.Text);
            }
            mainWindow.MainContent.Content = manifestationTypes;
        }
    }
}
