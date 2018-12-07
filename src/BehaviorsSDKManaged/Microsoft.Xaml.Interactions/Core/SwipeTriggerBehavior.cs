using Microsoft.Xaml.Interactions.Utility;
using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Microsoft.Xaml.Interactions.Core
{
    public class SwipeTriggerBehavior : Trigger<UIElement>
    {

        /// <summary>
        /// Gets or sets the Direction of swipe. This is a dependency property.
        /// </summary>
        public SwipeDirections Direction {
            get => (SwipeDirections)GetValue(DirectionProperty);
            set => SetValue(DirectionProperty, value);
        }

        /// <summary>
        /// Identifies the <seealso cref="Direction"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(nameof(Direction), typeof(SwipeDirections), typeof(SwipeTriggerBehavior), new PropertyMetadata(SwipeDirections.All));

        /// <summary>
        /// Gets or sets the Treshold of swipe. This is a dependency property.
        /// </summary>
        public double Treshold {
            get => (double)GetValue(TresholdProperty);
            set => SetValue(TresholdProperty, value);
        }

        /// <summary>
        /// Identifies the <seealso cref="Treshold"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TresholdProperty = DependencyProperty.Register(nameof(Treshold), typeof(double), typeof(SwipeTriggerBehavior), new PropertyMetadata(100D));

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.ManipulationMode = this.AssociatedObject.ManipulationMode | ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            this.AssociatedObject.ManipulationCompleted += OnManipulationCompleted;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.ManipulationCompleted -= OnManipulationCompleted;
        }

        protected void Execute(object sender, SwipeDirections direction)
        {
            Interaction.ExecuteActions(sender, this.Actions, direction);
        }

        private void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var isRight = e.Velocities.Linear.X.Between(0.3, Treshold);
            var isLeft = e.Velocities.Linear.X.Between(-Treshold, -0.3);
            var isUp = e.Velocities.Linear.Y.Between(-Treshold, -0.3);
            var isDown = e.Velocities.Linear.Y.Between(0.3, Treshold);

            if (isLeft && !(isUp || isDown) && Direction.HasFlag(SwipeDirections.Left))
            {
                this.Execute(this.AssociatedObject, SwipeDirections.Left);
            }
            if (isRight && !(isUp || isDown) && Direction.HasFlag(SwipeDirections.Right))
            {
                this.Execute(this.AssociatedObject, SwipeDirections.Right);
            }
            if (isUp && !(isRight || isLeft) && Direction.HasFlag(SwipeDirections.Up))
            {
                this.Execute(this.AssociatedObject, SwipeDirections.Up);
            }
            if (isDown && !(isRight || isLeft) && Direction.HasFlag(SwipeDirections.Down))
            {
                this.Execute(this.AssociatedObject, SwipeDirections.Down);
            }
            if (isLeft && isDown && Direction.HasFlag(SwipeDirections.LeftDown))
            {
                this.Execute(this.AssociatedObject, SwipeDirections.LeftDown);
            }
            if (isLeft && isUp && Direction.HasFlag(SwipeDirections.LeftUp))
            {
                this.Execute(this.AssociatedObject, SwipeDirections.LeftUp);
            }
            if (isRight && isDown && Direction.HasFlag(SwipeDirections.RightDown))
            {
                this.Execute(this.AssociatedObject, SwipeDirections.RightDown);
            }
            if (isRight && isUp && Direction.HasFlag(SwipeDirections.RightUp))
            {
                this.Execute(this.AssociatedObject, SwipeDirections.RightUp);
            }
        }
    }

    [Flags]
    public enum SwipeDirections
    {
        None = 0,
        Left = 1,
        Right = 2,
        Up = 4,
        Down = 8,
        LeftRight = Left | Right,
        UpDown = Up | Down,
        LeftDown = Left | Down,
        LeftUp = Left | Up,
        RightDown = Right | Down,
        RightUp = Right | Up,
        All = Left | Right | Up | Down,
    }
}
