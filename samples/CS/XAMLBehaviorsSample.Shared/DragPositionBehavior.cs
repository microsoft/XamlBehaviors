using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace XAMLBehaviorsSample
{
    public partial class DragPositionBehavior : DependencyObject, IBehavior
    {
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
                }
            }
        }

        UIElement parent = null;
        Point prevPoint;
        int pointerId = -1;
        void fe_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var fe = AssociatedObject as FrameworkElement;
            parent = (UIElement)fe.Parent;

            if (!(fe.RenderTransform is TranslateTransform))
                fe.RenderTransform = new TranslateTransform();
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
			var tr = (TranslateTransform)fe.RenderTransform;
			tr.X += pos.X - prevPoint.X;
			tr.Y += pos.Y - prevPoint.Y;
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
            parent = null;
            AssociatedObject = null;
        }
    }
}
