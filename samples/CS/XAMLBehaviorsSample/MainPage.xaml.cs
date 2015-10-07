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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace XAMLBehaviorsSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DataTriggerControl _datatrigger;
        EventTriggerControl _eventtrigger;
        IncrementalUpdateControl _incrementaltrigger;

        CallMethodControl _callmethodaction;
        GoToStateControl _gotostateaction;
        ChangePropertyControl _changepropertyaction;
        ControlStoryboardControl _controlstoryboardaction;
        PlaySoundControl _playsoundaction;
        NavigateToPageControl _navigatetopageaction;
        InvokeCommandControl _invokecommandaction;

        CustomBehaviorControl _custombehavioraction;
        CustomActionControl _customactionaction;

        public MainPage()
        {
            this.InitializeComponent();

            _datatrigger = new DataTriggerControl();
            _eventtrigger = new EventTriggerControl();
            _incrementaltrigger = new IncrementalUpdateControl();

            _callmethodaction = new CallMethodControl();
            _gotostateaction = new GoToStateControl();
            _changepropertyaction = new ChangePropertyControl();
            _controlstoryboardaction = new ControlStoryboardControl();
            _playsoundaction = new PlaySoundControl();
            _navigatetopageaction = new NavigateToPageControl();
            _invokecommandaction = new InvokeCommandControl();

            _custombehavioraction = new CustomBehaviorControl();
            _customactionaction = new CustomActionControl();
        }

        private bool CheckLastPage(Type desiredPage)
        {
            var lastPage = Frame.BackStack.LastOrDefault();
            return (lastPage != null && lastPage.SourcePageType.Equals(desiredPage)) ? true : false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if(CheckLastPage(typeof (NavigatePageSample)))
            {
                pivot.SelectedIndex = 1;
                ActionsContent.Children.Clear();
                ActionsContent.Children.Add(_navigatetopageaction);
            }
        }

        private void DataTriggerButton_Click(object sender, RoutedEventArgs e)
        {
            BehaviorsContent.Children.Clear();
            BehaviorsContent.Children.Add(_datatrigger);
        }

        private void EventTriggerButton_Click(object sender, RoutedEventArgs e)
        {
            _eventtrigger = new EventTriggerControl();
            BehaviorsContent.Children.Clear();
            BehaviorsContent.Children.Add(_eventtrigger);
        }

        private void IncrementalUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            BehaviorsContent.Children.Clear();
            BehaviorsContent.Children.Add(_incrementaltrigger);
        }

        private void CallMethodButton_Click(object sender, RoutedEventArgs e)
        {
            ActionsContent.Children.Clear();
            ActionsContent.Children.Add(_callmethodaction);
        }

        private void ChangePropertyButton_Click(object sender, RoutedEventArgs e)
        {
            ActionsContent.Children.Clear();
            ActionsContent.Children.Add(_changepropertyaction);
        }

        private void ControlStoryboardButton_Click(object sender, RoutedEventArgs e)
        {
            ActionsContent.Children.Clear();
            ActionsContent.Children.Add(_controlstoryboardaction);
        }

        private void PlaySoundButton_Click(object sender, RoutedEventArgs e)
        {
            ActionsContent.Children.Clear();
            ActionsContent.Children.Add(_playsoundaction);
        }

        private void GoToStateButton_Click(object sender, RoutedEventArgs e)
        {
            ActionsContent.Children.Clear();
            ActionsContent.Children.Add(_gotostateaction);
        }

        private void InvokeCommandButton_Click(object sender, RoutedEventArgs e)
        {
            ActionsContent.Children.Clear();
            ActionsContent.Children.Add(_invokecommandaction);
        }

        private void NavigateToPageButton_Click(object sender, RoutedEventArgs e)
        {
            ActionsContent.Children.Clear();
            ActionsContent.Children.Add(_navigatetopageaction);
        }

        private void CustomBehavior_Click(object sender, RoutedEventArgs e)
        {
            CustomContent.Children.Clear();
            CustomContent.Children.Add(_custombehavioraction);
        }

        private void CustomAction_Click(object sender, RoutedEventArgs e)
        {
            CustomContent.Children.Clear();
            CustomContent.Children.Add(_customactionaction);
        }
    }
}
