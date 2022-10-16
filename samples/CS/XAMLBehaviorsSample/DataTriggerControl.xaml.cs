using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace XAMLBehaviorsSample
{

    public sealed partial class DataTriggerControl : UserControl, INotifyPropertyChanged
    {
        private StateEnum _state;

        public event PropertyChangedEventHandler PropertyChanged;
        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public StateEnum State {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                NotifyPropertyChanged();
            }
        }

        public DataTriggerControl()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        private void B1_Click(object sender, RoutedEventArgs e)
        {
            this.State = StateEnum.StateOne;
        }

        private void B2_Click(object sender, RoutedEventArgs e)
        {
            this.State = StateEnum.StateTwo;
        }

        private void B3_Click(object sender, RoutedEventArgs e)
        {
            this.State = StateEnum.StateThree;
        }
    }
}
