using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManifestationManagementApp.command
{
    public static class Commands
    {
        public static readonly RoutedUICommand showHelp = new RoutedUICommand(
            "Show Help",
            "ShowHelp",
            typeof(RoutedCommand),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F1, ModifierKeys.None)
            }
            );

        public static readonly RoutedUICommand showNoviSad = new RoutedUICommand(
            "Show Novi Sad",
            "ShowNoviSad",
            typeof(RoutedCommand),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F1, ModifierKeys.Control)
            }
            );

        public static readonly RoutedUICommand showStariGrad = new RoutedUICommand(
            "Show Stari Grad",
            "ShowStariGrad",
            typeof(RoutedCommand),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F2, ModifierKeys.Control)
            }
            );

        public static readonly RoutedUICommand showDetelinara = new RoutedUICommand(
            "Show Detelinara",
            "ShowDetelinara",
            typeof(RoutedCommand),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F3, ModifierKeys.Control)
            }
            );

        public static readonly RoutedUICommand showLiman = new RoutedUICommand(
            "Show Liman",
            "ShowLiman",
            typeof(RoutedCommand),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F4, ModifierKeys.Control)
            }
            );

        public static readonly RoutedUICommand addManifestation = new RoutedUICommand(
            "Add manifestation",
            "AddManifestation",
            typeof(RoutedCommand),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Q, ModifierKeys.Control)
            }
            );

        public static readonly RoutedUICommand showManifestation = new RoutedUICommand(
            "Show manifestation",
            "ShowManifestation",
            typeof(RoutedCommand),
            new InputGestureCollection()
            {
                new KeyGesture(Key.W, ModifierKeys.Control)
            }
            );

        public static readonly RoutedUICommand addManifestationType = new RoutedUICommand(
            "Add manifestation type",
            "AddManifestationType",
            typeof(RoutedCommand),
            new InputGestureCollection()
            {
                new KeyGesture(Key.E, ModifierKeys.Control)
            }
            );

        public static readonly RoutedUICommand showManifestationType = new RoutedUICommand(
            "Show manifestation type",
            "ShowManifestationType",
            typeof(RoutedCommand),
            new InputGestureCollection()
            {
                new KeyGesture(Key.R, ModifierKeys.Control)
            }
            );

        public static readonly RoutedUICommand addLabel = new RoutedUICommand(
            "Add label",
            "AddLabel",
            typeof(RoutedCommand),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D, ModifierKeys.Control)
            }
            );

        public static readonly RoutedUICommand showLabel = new RoutedUICommand(
            "Show label",
            "ShowLabel",
            typeof(RoutedCommand),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F, ModifierKeys.Control)
            }
            );
    }
}
