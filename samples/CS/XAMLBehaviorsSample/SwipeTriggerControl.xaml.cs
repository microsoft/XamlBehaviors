using Microsoft.Xaml.Interactions.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace XAMLBehaviorsSample
{
    public sealed partial class SwipeTriggerControl : UserControl
    {
        public ICommand SwipeCommand { get; }

        public SwipeTriggerControl()
        {
            this.InitializeComponent();
            this.SwipeCommand = new MoveCommand(rectangleMove);
            this.DataContext = this;
        }

        private class MoveCommand : ICommand {
            public FrameworkElement Element { get; }

            public event EventHandler CanExecuteChanged;

            protected virtual void OnCanExecuteChanged() {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }

            public MoveCommand(FrameworkElement element) {
                this.Element = element;
            }

            public bool CanExecute(object parameter) {
                return true;
            }

            public void Execute(object parameter) {
                var r = Grid.GetRow(this.Element);
                var c = Grid.GetColumn(this.Element);

                var direction = (SwipeDirections)parameter;
                if (direction.HasFlag(SwipeDirections.Up) && r > 0) {
                    Grid.SetRow(this.Element, r - 1);
                }
                if (direction.HasFlag(SwipeDirections.Down) && r < 2) {
                    Grid.SetRow(this.Element, r + 1);
                }
                if (direction.HasFlag(SwipeDirections.Left) && c > 0) {
                    Grid.SetColumn(this.Element, c - 1);
                }
                if (direction.HasFlag(SwipeDirections.Right) && c < 2) {
                    Grid.SetColumn(this.Element, c + 1);
                }
            }
        }

    }
}
