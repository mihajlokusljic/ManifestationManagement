using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ManifestationManagementApp.model
{
    public class Repository: INotifyPropertyChanged
    {
        private static string dataFilepath = "..\\..\\resources\\data\\data.xml";
        public int LabelCounter { get; set; }
        public int ManifestationTypeCounter { get; set; }
        public int ManifestationCounter { get; set; }

        private ObservableCollection<Label> labels;
        public ObservableCollection<Label> Labels
        {
            get { return labels; }
            set
            {
                if(labels != value)
                {
                    labels = value;
                    OnPropertyChanged("Labels");
                }
            }
        }

        private ObservableCollection<ManifestationType> manifestationTypes;
        public ObservableCollection<ManifestationType> ManifestationTypes
        {
            get { return manifestationTypes; }
            set
            {
                if (manifestationTypes != value)
                {
                    manifestationTypes = value;
                    OnPropertyChanged("ManifestationTypes");
                }
            }
        }

        private ObservableCollection<Manifestation> manifestations;
        public ObservableCollection<Manifestation> Manifestations
        {
            get { return manifestations; }
            set
            {
                if (manifestations != value)
                {
                    manifestations = value;
                    OnPropertyChanged("Manifestations");
                }
            }
        }

        private ObservableCollection<Map> maps;
        [XmlIgnore]
        public ObservableCollection<Map> Maps
        {
            get { return maps; }
            set
            {
                if (maps != value)
                {
                    maps = value;
                    OnPropertyChanged("Maps");
                }
            }
        }

        private static Repository instance = null;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        
        private Repository()
        {
            Labels = new ObservableCollection<Label>();
            ManifestationTypes = new ObservableCollection<ManifestationType>();
            Manifestations = new ObservableCollection<Manifestation>();
            Maps = new ObservableCollection<Map>();
            Maps = new ObservableCollection<Map>();
            LabelCounter = 0;
            ManifestationTypeCounter = 0;
            ManifestationCounter = 0;
        }

        public static Repository GetInstance()
        {
            if(instance == null)
            {
                instance = new Repository();
            }
            return instance;
        }

        public Label FindLabel(string id)
        {
            foreach(Label lab in Labels)
            {
                if(lab.Id == id)
                {
                    return lab;
                }
            }
            return null;
        }

        public bool Addlabel(Label newLabel)
        {
            if(FindLabel(newLabel.Id) != null)
            {
                return false;
            }
            Labels.Add(newLabel);
            SaveData();
            return true;
        }

        public bool UpdateLabel(Label newLabelData)
        {
            Label target = FindLabel(newLabelData.Id);
            if (target == null)
            {
                return false;
            }
            target.Color = newLabelData.Color;
            target.Description = newLabelData.Description;
            SaveData();
            return true;
        }

        public bool LabelIsReferenced(string labelId)
        {
            foreach(Manifestation manif in Manifestations)
            {
                foreach(Label lab in manif.Labels)
                {
                    if(lab.Id == labelId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ManifestationTypeIsReferenced(string typeId)
        {
            foreach (Manifestation manif in Manifestations)
            {
                    if (manif.Type.Id == typeId)
                    {
                        return true;
                    }
            }
            return false;
        }

        public bool DeleteLabel(string id)
        {
            Label target = FindLabel(id);
            if(target == null)
            {
                return false;
            }

            Labels.Remove(target);
            SaveData();
            return true;
        }

        public ManifestationType FindManifestationType(string id)
        {
            foreach(ManifestationType type in ManifestationTypes)
            {
                if(type.Id == id)
                {
                    return type;
                }
            }
            return null;
        }

        public bool AddManifestationType(ManifestationType newType)
        {
            if(FindLabel(newType.Id) != null)
            {
                return false;
            }
            ManifestationTypes.Add(newType);
            SaveData();
            return true;
        }

        public bool UpdateManifestationType(ManifestationType newTypeData)
        {
            ManifestationType target = FindManifestationType(newTypeData.Id);
            if (target == null)
            {
                return false;
            }
            target.Name = newTypeData.Name;
            target.IconPath = newTypeData.IconPath;
            target.Description = newTypeData.Description;
            SaveData();
            return true;
        }

        public bool DeleteManifestationType(string id)
        {
            ManifestationType target = FindManifestationType(id);
            if(target == null)
            {
                return false;
            }
            ManifestationTypes.Remove(target);
            SaveData();
            return true;
        }

        public Manifestation FindManifestation(string id)
        {
            foreach (Manifestation manif in Manifestations)
            {
                if (manif.Id == id)
                {
                    return manif;
                }
            }
            return null;
        }

        public bool AddManifestation(Manifestation newManif)
        {
            if (FindManifestation(newManif.Id) != null)
            {
                return false;
            }
            Manifestations.Add(newManif);
            SaveData();
            return true;
        }

        public bool UpdateManifestation(Manifestation newManifestationData)
        {
            Manifestation target = FindManifestation(newManifestationData.Id);
            if (target == null)
            {
                return false;
            }
            target.Name = newManifestationData.Name;
            target.Type = newManifestationData.Type;
            target.Labels = newManifestationData.Labels;
            target.SmokingAllowed = newManifestationData.SmokingAllowed;
            target.SupportHandicaped = newManifestationData.SupportHandicaped;
            target.Prices = newManifestationData.Prices;
            target.Alcohol = newManifestationData.Alcohol;
            target.IconPath = newManifestationData.IconPath;
            target.Description = newManifestationData.Description;
            SaveData();
            return true;
        }

        public bool DeleteManifestation(string id)
        {
            Manifestation target = FindManifestation(id);
            if (target == null)
            {
                return false;
            }
            Manifestations.Remove(target);
            SaveData();
            return true;
        }

        public Map GetMap(int mapId)
        {
            foreach(Map map in Maps)
            {
                if(map.Id == mapId)
                {
                    return map;
                }
            }
            return null;
        }

        public void SaveData()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Repository));
            using(StreamWriter sw = File.CreateText(dataFilepath))
            {
                serializer.Serialize(sw, this);
            }
        }

        public void InitializeMaps()
        {
            Map NoviSadMap = new Map()
            {
                Id = (int)MapIds.NoviSad,
                ImagePath = "../resources/maps/mapaNoviSad.png",
                Width = 1679,
                Heigth = 1015,
                ParentMapOffset = new Coordinates() { X = 0, Y = 0, ParentMap = null }
            };
            Maps.Add(NoviSadMap);

            Map StariGradMap = new Map()
            {
                Id = (int)MapIds.StariGrad,
                ImagePath = "../resources/maps/mapaStariGrad.png",
                Width = 757,
                Heigth = 312,
                ParentMapOffset = new Coordinates() { X = 810, Y = 330, ParentMap = NoviSadMap }
            };
            Maps.Add(StariGradMap);

            Map DetelinaraMap = new Map()
            {
                Id = (int)MapIds.Detelinara,
                ImagePath = "../resources/maps/mapaDetelinara.png",
                Width = 757,
                Heigth = 457,
                ParentMapOffset = new Coordinates() { X = 125, Y = 105, ParentMap = NoviSadMap }
            };
            Maps.Add(DetelinaraMap);

            Map LimanMap = new Map()
            {
                Id = (int)MapIds.Liman,
                ImagePath = "../resources/maps/mapaLiman.png",
                Width = 760,
                Heigth = 465,
                ParentMapOffset = new Coordinates() { X = 565, Y = 545, ParentMap = NoviSadMap }
            };
            Maps.Add(LimanMap);
        }

        public void ReadData()
        {
            XmlSerializer reader = new XmlSerializer(typeof(Repository));
            using (StreamReader file = new StreamReader(dataFilepath))
            {
                Repository rep = (Repository)reader.Deserialize(file);
                LabelCounter = rep.LabelCounter;
                ManifestationTypeCounter = rep.ManifestationTypeCounter;
                Labels = rep.Labels;
                ManifestationTypes = rep.ManifestationTypes;
                Manifestations = rep.Manifestations;
            }
            InitializeMaps();
        }

    }
}
