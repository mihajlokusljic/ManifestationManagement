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
    /// Interaction logic for AddLabelView.xaml
    /// </summary>
    public partial class AddLabelView : Page
    {
        public AddLabelView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool isAutoChecked = autoGenerateId.IsChecked.Value;
            if (colorPicker.SelectedColor.ToString() == "")
            {
                AddedLabelMessage.Content = "Please, pick some color.";
            } else if (descriptionInput.Text == "")
            {
                AddedLabelMessage.Content = "Please, add some description.";
            } else if (idInput.Text == "" && !isAutoChecked)
            {
                AddedLabelMessage.Content = "Please, insert id for this label.";
            }
            else
            {
                model.Label retVal = new model.Label();
                if (isAutoChecked)
                {
                    model.Label.counter = model.Label.counter + 1;
                    retVal.Id = "lab" + model.Label.counter;
                }
                else
                {
                    retVal.Id = idInput.Text;
                }
                retVal.Color = colorPicker.SelectedColor.ToString();
                retVal.Description = descriptionInput.Text;
                model.Repository rep = model.Repository.GetInstance();
                rep.Addlabel(retVal);
                AddedLabelMessage.Content = "Label " + retVal.Id + " has been added successfully.";
                if(autoGenerateId.IsChecked.Value)
                {
                    //ako je izabrano automatsko inkrementiranje, azurira se vrijednost za id
                    idInput.Text = $"lab{model.Label.counter + 1}";
                }
            }
        }

        private void CancelBtnClicked(object sender, RoutedEventArgs e)
        {

        }

        private void AutoGenerateIdClicked(object sender, RoutedEventArgs e)
        {
            bool isAutoChecked = autoGenerateId.IsChecked.Value;
            if (isAutoChecked)
            {
                idInput.IsEnabled = false;
                idInput.Text = $"lab{model.Label.counter + 1}";
            }
            else
            {
                idInput.IsEnabled = true;
                idInput.Text = "";
            }
        }
    }
}
