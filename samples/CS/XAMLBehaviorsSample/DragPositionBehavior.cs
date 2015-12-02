using Microsoft.Xaml.Interactivity;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace XAMLBehaviorsSample
{
    public class DragPositionBehavior : DependencyObject, IBehavior
    {
        private UIElement _parent = null;
        private Point _prevPoint;
        private int _pointerId = -1;

        private void _initialise(FrameworkElement element)
        {
            element.RenderTransform = new CompositeTransform();
        }

        private void _cleanup()
        {
            _parent = null;
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
                    _initialise(fe);
                }
            }
        }

        void fe_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var fe = AssociatedObject as FrameworkElement;
            _parent = (UIElement)fe.Parent;

            _prevPoint = e.GetCurrentPoint(_parent).Position;
            _parent.PointerMoved += move;
            _pointerId = (int)e.Pointer.PointerId;
        }

        private void move(object o, PointerRoutedEventArgs args)
        {
            if (args.Pointer.PointerId != _pointerId)
                return;

            var fe = AssociatedObject as FrameworkElement;
            var pos = args.GetCurrentPoint(_parent).Position;
            var tr = (CompositeTransform)fe.RenderTransform;
            tr.TranslateX += pos.X - _prevPoint.X;
            tr.TranslateY += pos.Y - _prevPoint.Y;
            _prevPoint = pos;
        }

        void fe_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var fe = AssociatedObject as FrameworkElement;
            if (e.Pointer.PointerId != _pointerId)
                return;
            _parent.PointerMoved -= move;
            _pointerId = -1;
        }

        public void Detach()
        {
            var fe = AssociatedObject as FrameworkElement;
            if (fe != null)
            {
                fe.PointerPressed -= fe_PointerPressed;
                fe.PointerReleased -= fe_PointerReleased;
            }

            _cleanup();
        }
    }
}
