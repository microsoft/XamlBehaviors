using Microsoft.Xaml.Interactivity;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace XAMLBehaviorsSample
{
    /// <summary>
    /// Implements a custom Behavior for enabling dragging of a generic FrameworkElement.
    /// Allows controls to be moved around a Canvas using a Mouse Drag.
    /// </summary>
    public class DragPositionBehavior : DependencyObject, IBehavior
    {
        /// <summary>
        /// The parent element for the associated object.
        /// </summary>
        private UIElement parent = null;

        /// <summary>
        /// The previous point coordinates.
        /// </summary>
        private Point prevPoint;

        /// <summary>
        /// The Pointer Id value.
        /// </summary>
        private int pointerId = -1;

        /// <summary>
        /// Initialises the private fields used by the Behavior.
        /// Called when Behavior is attached.
        /// </summary>
        /// <param name="element"></param>
        private void initialise(FrameworkElement element)
        {
            element.RenderTransform = new CompositeTransform();
        }

        /// <summary>
        /// Cleanup fields. Called when object is detached.
        /// </summary>
        private void cleanup()
        {
            parent = null;
            AssociatedObject = null;
        }

        /// <summary>
        /// Event handler for the PointerPressed event of the Associated object.
        /// </summary>
        /// <param name="o">The sender</param>
        /// <param name="args">The event arguments</param>
        private void fe_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var fe = AssociatedObject as FrameworkElement;
            parent = (UIElement)fe.Parent;

            prevPoint = e.GetCurrentPoint(parent).Position;
            parent.PointerMoved += move;
            pointerId = (int)e.Pointer.PointerId;
        }

        /// <summary>
        /// Event handler for the PointerMoved event of the Associated object.
        /// </summary>
        /// <param name="o">The sender</param>
        /// <param name="args">The event arguments</param>
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

        /// <summary>
        /// Event handler for the PointerReleased event of the Associated object.
        /// </summary>
        /// <param name="o">The sender</param>
        /// <param name="args">The event arguments</param>
        private void fe_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var fe = AssociatedObject as FrameworkElement;
            if (e.Pointer.PointerId != pointerId)
                return;
            parent.PointerMoved -= move;
            pointerId = -1;
        }

        /// <summary>
        /// Exposes the Behavior associated object.
        /// </summary>
        public DependencyObject AssociatedObject
        {
            get;
            set;
        }

        /// <summary>
        /// Attaches to the specified object.
        /// Called after the behavior is attached to the <see cref="Microsoft.Xaml.Interactivity.Behavior.AssociatedObject"/>.
        /// </summary>
        /// <param name="associatedObject">The <seealso cref="Windows.UI.Xaml.DependencyObject"/> to which the <seealso cref="IBehavior"/> will be attached.</param>
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

        /// <summary>
        /// Detaches this instance from its associated object.
        /// Called when the behavior is being detached from its <see cref="Microsoft.Xaml.Interactivity.Behavior.AssociatedObject"/>.
        /// </summary>
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
