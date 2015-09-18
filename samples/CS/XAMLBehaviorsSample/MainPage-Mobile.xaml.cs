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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace XAMLBehaviorsSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage_Mobile : Page
    {
        DataTriggerControl_Mobile _datatrigger;
        EventTriggerControl_Mobile _eventtrigger;
        IncrementalUpdateControl_Mobile _incrementaltrigger;

        CallMethodControl_Mobile _callmethodaction;
        GoToStateControl_Mobile _gotostateaction;
        ChangePropertyControl_Mobile _changepropertyaction;
        ControlStoryboardControl_Mobile _controlstoryboardaction;
        PlaySoundControl_Mobile _playsoundaction;
        NavigateToPageControl_Mobile _navigatetopageaction;
        InvokeCommandControl_Mobile _invokecommandaction;

        CustomBehaviorControl_Mobile _custombehavioraction;
        CustomActionControl_Mobile _customactionaction;

        public MainPage_Mobile()
        {
            this.InitializeComponent();

            _datatrigger = new DataTriggerControl_Mobile();
            _eventtrigger = new EventTriggerControl_Mobile();
            _incrementaltrigger = new IncrementalUpdateControl_Mobile();

            _callmethodaction = new CallMethodControl_Mobile();
            _gotostateaction = new GoToStateControl_Mobile();
            _changepropertyaction = new ChangePropertyControl_Mobile();
            _controlstoryboardaction = new ControlStoryboardControl_Mobile();
            _playsoundaction = new PlaySoundControl_Mobile();
            _navigatetopageaction = new NavigateToPageControl_Mobile();
            _invokecommandaction = new InvokeCommandControl_Mobile();

            _custombehavioraction = new CustomBehaviorControl_Mobile();
            _customactionaction = new CustomActionControl_Mobile();
        }

        private bool CheckLastPage(Type desiredPage)
        {
            var lastPage = Frame.BackStack.LastOrDefault();
            return (lastPage != null && lastPage.SourcePageType.Equals(desiredPage)) ? true : false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (CheckLastPage(typeof(NavigatePageSample)))
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
