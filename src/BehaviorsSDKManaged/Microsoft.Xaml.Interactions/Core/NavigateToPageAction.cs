// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace Microsoft.Xaml.Interactions.Core
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Markup;
    using Windows.UI.Xaml.Media;
    using Interactivity;

    /// <summary>
    /// An action that switches the current visual to the specified <see cref="Windows.UI.Xaml.Controls.Page"/>.
    /// </summary>
    public sealed class NavigateToPageAction : DependencyObject, IAction
    {
        /// <summary>
        /// Identifies the <seealso cref="TargetPage"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty TargetPageProperty = DependencyProperty.Register(
            "TargetPage",
            typeof(string),
            typeof(NavigateToPageAction),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <seealso cref="Parameter"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty ParameterProperty = DependencyProperty.Register(
            "Parameter",
            typeof(object),
            typeof(NavigateToPageAction),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the fully qualified name of the <see cref="Windows.UI.Xaml.Controls.Page"/> to navigate to. This is a dependency property.
        /// </summary>
        public string TargetPage
        {
            get
            {
                return (string)this.GetValue(NavigateToPageAction.TargetPageProperty);
            }
            set
            {
                this.SetValue(NavigateToPageAction.TargetPageProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the parameter which will be passed to the <see cref="Windows.UI.Xaml.Controls.Frame.Navigate(System.Type,object)"/> method.
        /// </summary>
        public object Parameter
        {
            get
            {
                return (object)this.GetValue(NavigateToPageAction.ParameterProperty);
            }
            set
            {
                this.SetValue(NavigateToPageAction.ParameterProperty, value);
            }
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="sender">The <see cref="System.Object"/> that is passed to the action by the behavior. Generally this is <seealso cref="Microsoft.Xaml.Interactivity.IBehavior.AssociatedObject"/> or a target object.</param>
        /// <param name="parameter">The value of this parameter is determined by the caller.</param>
        /// <returns>True if the navigation to the specified page is successful; else false.</returns>
        public object Execute(object sender, object parameter)
        {
            if (string.IsNullOrEmpty(this.TargetPage))
            {
                return false;
            }

            IXamlMetadataProvider metadataProvider = (IXamlMetadataProvider)Application.Current;
            IXamlType xamlType = metadataProvider.GetXamlType(this.TargetPage);
            if (xamlType == null)
            {
                return false;
            }

            INavigate navigateElement = Window.Current.Content as INavigate;
            DependencyObject senderObject = sender as DependencyObject;

            // If the sender wasn't an INavigate, then keep looking up the tree from the
            // root we were given for another INavigate.
            while (senderObject != null && navigateElement == null)
            {
                navigateElement = sender as INavigate;
                if (navigateElement == null)
                {
                    senderObject = VisualTreeHelper.GetParent(senderObject);
                }
            }

            if (navigateElement == null)
            {
                return false;
            }

            Frame frame = navigateElement as Frame;

            if (frame != null)
            {
                return frame.Navigate(xamlType.UnderlyingType, this.Parameter ?? parameter);
            }
            else
            {
                return navigateElement.Navigate(xamlType.UnderlyingType);
            }
        }
    }
}
