using Windows.UI.Xaml.Controls;

namespace XAMLBehaviorsSample;

public sealed partial class ContentDialogSample : ContentDialog
{
    public ContentDialogSample()
    {
        this.InitializeComponent();
    }

    private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        this.Hide();
    }
}
