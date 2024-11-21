using System.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace XAMLBehaviorsSample;

public sealed partial class CallMethodControl : UserControl, INotifyPropertyChanged
{
    public int Count { get; set; }

    public CallMethodControl()
    {
        this.InitializeComponent();
        this.DataContext = this;
        Count = 0;
    }

    public event PropertyChangedEventHandler PropertyChanged;

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
