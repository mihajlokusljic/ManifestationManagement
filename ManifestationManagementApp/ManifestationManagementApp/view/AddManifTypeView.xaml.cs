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
            Repository rep = Repository.GetInstance();
            string id;
            if(isAutoChecked)
            {
                id = $"type{ManifestationType.counter + 1}";
            } else
            {
                id = idInput.Text;
            }

            if(id == "")
            {
                AddedTypeMessage.Content = "Please enter an Id.";
            }
            else if (rep.FindManifestationType(id) != null)
            {
                AddedTypeMessage.Content = "Entered id already exists. Pease enter a different value.";
            }
            else if (typeName == "")
            {
                AddedTypeMessage.Content = "Please enter a name for manifestation type.";
            }
            else if (desc == "")
            {
                AddedTypeMessage.Content = "Please enter some description.";
            }
            else
            {
                ManifestationType newType = new ManifestationType()
                {
                    Id = id,
                    Name = typeName,
                    Description = desc,
                    IconPath = "",
                };
                rep.AddManifestationType(newType);
                string nextId = $"type{ManifestationType.counter + 1}";
                AddedTypeMessage.Content = $"Successfully added manifestation type: {newType.Id}";
                nameInput.Text = "";
                descriptionInput.Text = "";
                if(isAutoChecked)
                {
                    idInput.Text = nextId;
                    nameInput.Focus();
                }
                else
                {
                    idInput.Text = "";
                    idInput.Focus();
                }

            }
        }
    }
}
