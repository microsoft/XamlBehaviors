using Microsoft.Xaml.Interactivity;
using System;
using Windows.UI.Xaml;

namespace XAMLBehaviorsSample;

class ContentDialogPopUp : DependencyObject, IAction
{
    ContentDialogSample samplecd;

    public object Execute(object sender, object parameter)
    {
        samplecd = new ContentDialogSample();
        ShowCD();
        return null;
    }

    public async void ShowCD()
    {
        await samplecd.ShowAsync();
    }
}
