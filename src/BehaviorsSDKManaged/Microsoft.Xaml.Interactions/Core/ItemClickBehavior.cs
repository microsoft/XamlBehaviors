// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace Microsoft.Xaml.Interactions.Core
{
    using System.Windows.Input;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Interactivity;

    /// <summary>
    /// A behavior that listens for the <see cref="ListViewBase.ItemClick"/> event on its source and executes a specified command when that event is fired
    /// </summary>
    public sealed class ItemClickBehavior : Behavior<ListViewBase>
    {
        /// <summary>
        /// Gets or sets the <see cref="ICommand"/> instance to invoke when the current behavior is triggered
        /// </summary>
        public ICommand Command {
            get => (ICommand)this.GetValue(CommandProperty);
            set => this.SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Identifies the <seealso cref="Command"/> property
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            nameof(Command),
            typeof(ICommand),
            typeof(ItemClickBehavior),
            new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// Handles a clicked item and invokes the associated command
        /// </summary>
        /// <param name="sender">The current <see cref="ListViewBase"/> instance</param>
        /// <param name="e">The <see cref="ItemClickEventArgs"/> instance with the clicked item</param>
        private void HandleItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(this.Command is ICommand command) ||
                !command.CanExecute(e.ClickedItem))
            {
                return;
            }

            command.Execute(e.ClickedItem);
        }

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();

            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.ItemClick += this.HandleItemClick;
            }
        }

        /// <inheritdoc/>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.ItemClick -= this.HandleItemClick;
            }
        }
    }
}
