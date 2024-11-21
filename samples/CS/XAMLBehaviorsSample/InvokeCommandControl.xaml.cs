using System;
using System.ComponentModel;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace XAMLBehaviorsSample;

public sealed partial class InvokeCommandControl : UserControl, INotifyPropertyChanged
{
    public int Count { get; set; }
    public event PropertyChangedEventHandler PropertyChanged;
    public SolidColorBrush darkgreybrush;
    public SolidColorBrush pinkbrush;

    public class SampleCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            CanExecuteChanged(null, null);
        }
    }

    public ICommand UpdateCountCommand { get; set; }
    public ICommand UpdateGreyCommand { get; set; }
    public ICommand UpdatePinkCommand { get; set; }

    public InvokeCommandControl()
    {
        this.InitializeComponent();
        UpdateCountCommand = new SampleCommand();
        UpdateGreyCommand = new SampleCommand();
        UpdatePinkCommand = new SampleCommand();

        this.UpdateCountCommand.CanExecuteChanged += UpdateCountCommand_CanExecuteChanged;
        this.UpdateGreyCommand.CanExecuteChanged += UpdateGreyCommand_CanExecuteChanged;
        this.UpdatePinkCommand.CanExecuteChanged += UpdatePinkCommand_CanExecuteChanged;

        this.DataContext = this;
        darkgreybrush = new SolidColorBrush();
        darkgreybrush.Color = Color.FromArgb(255, 51, 51, 50);
        pinkbrush = new SolidColorBrush();
        pinkbrush.Color = Color.FromArgb(255, 233, 95, 91);
    }

    private void UpdateCountCommand_CanExecuteChanged(object sender, EventArgs e)
    {
        Count++;
        OnPropertyChanged(nameof(Count));
    }

    private void UpdateGreyCommand_CanExecuteChanged(object sender, EventArgs e)
    {
        border.Background = darkgreybrush;
    }

    private void UpdatePinkCommand_CanExecuteChanged(object sender, EventArgs e)
    {
        border.Background = pinkbrush;
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
