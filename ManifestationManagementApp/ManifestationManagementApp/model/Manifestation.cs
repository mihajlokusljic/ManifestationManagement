using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

namespace ManifestationManagementApp.model
{
    public class Manifestation : INotifyPropertyChanged
    {
        private string id;
        public string Id
        {
            get { return id; }
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                if (value != description)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        private ManifestationType type;
        public ManifestationType Type
        {
            get { return type; }
            set
            {
                if (value != type)
                {
                    type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        private AlcoholConusmption alcohol;
        public AlcoholConusmption Alcohol
        {
            get { return alcohol; }
            set
            {
                if (value != alcohol)
                {
                    alcohol = value;
                    OnPropertyChanged("Alcohol");
                }
            }
        }

        private string iconPath;
        public string IconPath
        {
            get { return iconPath; }
            set
            {
                if (value != iconPath)
                {
                    iconPath = value;

                    var exeDirectory = Directory.GetCurrentDirectory();
                    var exeDirectoryInfo = new DirectoryInfo(exeDirectory);
                    var projectDirectoryInfo = exeDirectoryInfo.Parent.Parent; // bin/debug to project

                    var dataPath = Path.Combine(projectDirectoryInfo.FullName, "resources\\images");
                    var newPath = Path.Combine(dataPath, @iconPath.Split('\\').Last());
                    if (!File.Exists(newPath) && newPath != null && !string.IsNullOrEmpty(newPath) && !string.IsNullOrWhiteSpace(newPath))
                    {
                        File.Copy(@iconPath, @newPath, true);
                    }
                    iconPath = newPath;

                    OnPropertyChanged("IconPath");
                }
            }
        }

        private bool supportHandicaped;
        public bool SupportHandicaped
        {
            get { return supportHandicaped; }
            set
            {
                if (value != supportHandicaped)
                {
                    supportHandicaped = value;
                    OnPropertyChanged("SupportHandicaped");
                }
            }
        }

        private bool smokingAllowed;
        public bool SmokingAllowed
        {
            get { return smokingAllowed; }
            set
            {
                if (value != smokingAllowed)
                {
                    smokingAllowed = value;
                    OnPropertyChanged("SmokingAllowed");
                }
            }
        }

        private bool isOutside;
        public bool IsOutside
        {
            get { return isOutside; }
            set
            {
                if (value != isOutside)
                {
                    isOutside = value;
                    OnPropertyChanged("IsOutside");
                }
            }
        }

        private PriceCategory prices;
        public PriceCategory Prices
        {
            get { return prices; }
            set
            {
                if (value != prices)
                {
                    prices = value;
                    OnPropertyChanged("Prices");
                }
            }
        }

        private string expectedAudience;
        public string ExpectedAudience
        {
            get { return expectedAudience; }
            set
            {
                if (value != expectedAudience)
                {
                    expectedAudience = value;
                    OnPropertyChanged("ExpectedAudience");
                }
            }
        }

        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set
            {
                if (value != date)
                {
                    date = value;
                    OnPropertyChanged("Date");
                }
            }
        }

        private ObservableCollection<Label> labels;
        public ObservableCollection<Label> Labels
        {
            get { return labels; }
            set
            {
                if (value != labels)
                {
                    labels = value;
                    OnPropertyChanged("Labels");
                }
            }
        }

        private ObservableCollection<Coordinates> mapCoords;
        public ObservableCollection<Coordinates> MapCoordinates
        {
            get { return mapCoords; }
            set
            {
                if (value != mapCoords)
                {
                    mapCoords = value;
                    OnPropertyChanged("MapCoordinates");
                }
            }
        }

        public void RemoveCoordinatesForMap(int mapId)
        {
            Coordinates target = null;
            foreach(Coordinates coords in MapCoordinates)
            {
                if(coords.ParentMap.Id == mapId)
                {
                    target = coords;
                    break;
                }
            }
            if(target != null)
            {
                MapCoordinates.Remove(target);
            }
        }

        public bool Addlabel(Label newLabel)
        {
            
            Labels.Add(newLabel);
            return true;
        }

        public Manifestation()
        {
            labels = new ObservableCollection<Label>();
            MapCoordinates = new ObservableCollection<Coordinates>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
