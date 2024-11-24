using StarWars.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace StarWars.Template
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CharacterModelControll : UserControl
    {
        public CharacterModelControll()
        {
            this.InitializeComponent();
        }

        private void ShowVehiclesButton_Click(object sender, RoutedEventArgs e)
        {
            var character = this.DataContext as Character; // Sostituisci
            if (character != null)
            {
               

            }
        }
    }
}
