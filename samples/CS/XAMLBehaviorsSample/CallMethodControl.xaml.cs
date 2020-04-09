using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace XAMLBehaviorsSample
{
    public sealed partial class CallMethodControl : UserControl, INotifyPropertyChanged
    {
        public int Count { get; set; }

        public CallMethodControl()
        {
            this.InitializeComponent();
            this.DataContext = this;
            Count = 0;
        }

        public event Microsoft.UI.Xaml.Data.PropertyChangedEventHandler PropertyChanged;

        public void IncrementCount()
        {
            Count++;
            OnPropertyChanged(nameof(Count));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
