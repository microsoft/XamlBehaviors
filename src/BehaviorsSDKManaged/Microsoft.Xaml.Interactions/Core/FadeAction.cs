using Microsoft.Xaml.Interactivity;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Xaml.Interactions.Core
{
    public enum FadeTypes
    {
        FadeIn = 0,
        FadeOut
    }

    public class FadeAction : DependencyObject, IAction
    {
        public FadeTypes FadeType
        {
            get { return (FadeTypes)GetValue(FadeTypeProperty); }
            set { SetValue(FadeTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FadeType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FadeTypeProperty =
            DependencyProperty.Register("FadeType", typeof(FadeTypes), typeof(FadeAction), new PropertyMetadata(FadeTypes.FadeOut));

        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Duration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(FadeAction), new PropertyMetadata(TimeSpan.FromSeconds(0.5)));

        public object Execute(object sender, object parameter)
        {
            FadeControl((FrameworkElement)sender);
            return true;
        }

        private void FadeControl(FrameworkElement control)
        {
            var da = new DoubleAnimation() { Duration = Duration };
            var sb = new Storyboard();
            sb.Children.Add(da);
            Storyboard.SetTarget(da, control);
            Storyboard.SetTargetProperty(da, "Opacity");
            switch(FadeType)
            {
                case FadeTypes.FadeIn:
                    da.From = 0;
                    da.To = 1;
                    break;
                case FadeTypes.FadeOut:
                    da.From = 1;
                    da.To = 0;
                    break;
            }
            sb.Begin();
        }
    }
}
