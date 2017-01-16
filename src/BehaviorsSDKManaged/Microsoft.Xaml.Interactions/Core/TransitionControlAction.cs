using Microsoft.Xaml.Interactivity;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Xaml.Interactions.Core
{
    public enum AnimationKind
    {
        Right,
        Left,
        Up,
        Down
    }
    public sealed class TransitionControlAction : DependencyObject, IAction
    {


        public AnimationKind AnimationKind
        {
            get { return (AnimationKind)GetValue(AnimationKindProperty); }
            set { SetValue(AnimationKindProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AnimationKind.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnimationKindProperty =
            DependencyProperty.Register("AnimationKind", typeof(AnimationKind), typeof(TransitionControlAction), new PropertyMetadata(AnimationKind.Right));


        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Duration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(TransitionControlAction), new PropertyMetadata(TimeSpan.FromMilliseconds(500)));


        private void AnimateControl(FrameworkElement control, TimeSpan duration, AnimationKind kind)
        {
            double xFinal = 0;
            double yFinal = 0;
            if (kind == AnimationKind.Left)
                xFinal = -control.ActualWidth;
            else if (kind == AnimationKind.Right)
                xFinal = control.ActualWidth;
            else if (kind == AnimationKind.Up)
                yFinal = -control.ActualHeight;
            else if (kind == AnimationKind.Down)
                yFinal = control.ActualHeight;
            var translate = new TranslateTransform() { X = 0, Y=0};
            control.RenderTransform = translate;
            if (kind == AnimationKind.Left || kind == AnimationKind.Right)
            {
                var da = new DoubleAnimation() { From = 0, To = xFinal, Duration = duration };
                Storyboard.SetTarget(da, control);
                Storyboard.SetTargetProperty(da, "(UIElement.RenderTransform).(TranslateTransform.X)");
                var sb = new Storyboard();
                sb.Children.Add(da);
                sb.Begin();
            }
            else
            {
                var da = new DoubleAnimation() { From = 0, To = yFinal, Duration = duration };
                Storyboard.SetTarget(da, control);
                Storyboard.SetTargetProperty(da, "(UIElement.RenderTransform).(TranslateTransform.Y)");
                var sb = new Storyboard();
                sb.Children.Add(da);
                sb.Begin();
            }
        }

        public object Execute(object sender, object parameter)
        {
            AnimateControl((FrameworkElement)sender, Duration, AnimationKind);
            return true;
        }
    }
}
