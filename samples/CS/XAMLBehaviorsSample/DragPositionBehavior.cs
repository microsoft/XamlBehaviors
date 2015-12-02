using Microsoft.Xaml.Interactivity;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace XAMLBehaviorsSample
{
    public class DragPositionBehavior : DependencyObject, IBehavior
    {
        private UIElement parent = null;
        private Point prevPoint;
        private int pointerId = -1;

        private void initialise(FrameworkElement element)
        {
            element.RenderTransform = new CompositeTransform();
        }

        private void cleanup()
        {
            parent = null;
            AssociatedObject = null;
        }

        public DependencyObject AssociatedObject
        {
            get;
            set;
        }

        public void Attach(DependencyObject associatedObject)
        {
            if ((associatedObject != AssociatedObject) && !Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                AssociatedObject = associatedObject;
                var fe = AssociatedObject as FrameworkElement;
                if (fe != null)
                {
                    fe.PointerPressed += fe_PointerPressed;
                    fe.PointerReleased += fe_PointerReleased;
                    initialise(fe);
                }
            }
        }

        void fe_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var fe = AssociatedObject as FrameworkElement;
            parent = (UIElement)fe.Parent;

            prevPoint = e.GetCurrentPoint(parent).Position;
            parent.PointerMoved += move;
            pointerId = (int)e.Pointer.PointerId;
        }

        private void move(object o, PointerRoutedEventArgs args)
        {
            if (args.Pointer.PointerId != pointerId)
                return;

            var fe = AssociatedObject as FrameworkElement;
            var pos = args.GetCurrentPoint(parent).Position;
            var tr = (CompositeTransform)fe.RenderTransform;
            tr.TranslateX += pos.X - prevPoint.X;
            tr.TranslateY += pos.Y - prevPoint.Y;
            prevPoint = pos;
        }

        void fe_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var fe = AssociatedObject as FrameworkElement;
            if (e.Pointer.PointerId != pointerId)
                return;
            parent.PointerMoved -= move;
            pointerId = -1;
        }

        public void Detach()
        {
            var fe = AssociatedObject as FrameworkElement;
            if (fe != null)
            {
                fe.PointerPressed -= fe_PointerPressed;
                fe.PointerReleased -= fe_PointerReleased;
            }

            cleanup();
        }
    }
}
