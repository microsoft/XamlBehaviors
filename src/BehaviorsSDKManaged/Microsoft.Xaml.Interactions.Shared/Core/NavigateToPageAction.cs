// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

using Microsoft.Xaml.Interactivity;
using Microsoft.Xaml.Interactions.Utility;
using System;
using System.Linq;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
#endif

namespace Microsoft.Xaml.Interactions.Core
{
    /// <summary>
    /// An action that switches the current visual to the specified <see cref="Page"/>.
    /// </summary>
    public sealed partial class NavigateToPageAction : DependencyObject, IAction
    {
        private readonly IVisualTreeHelper _visualTreeHelper;

        /// <summary>
        /// Identifies the <seealso cref="TargetPage"/> dependency property.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty TargetPageProperty = DependencyProperty.Register(
            "TargetPage",
            typeof(string),
            typeof(NavigateToPageAction),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <seealso cref="Parameter"/> dependency property.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty ParameterProperty = DependencyProperty.Register(
            "Parameter",
            typeof(object),
            typeof(NavigateToPageAction),
            new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the NavigateToPageAction class.
        /// </summary>
        public NavigateToPageAction()
            : this(new UwpVisualTreeHelper())
        {
        }

        /// <summary>
        /// Initializes a new instance of the NavigateToPageAction class.
        /// </summary>
        /// <param name="visualTreeHelper">
        /// IVisualTreeHelper implementation to use when searching the tree for an
        /// INavigate target.
        /// </param>
        internal NavigateToPageAction(IVisualTreeHelper visualTreeHelper)
        {
            this._visualTreeHelper = visualTreeHelper;
        }

        /// <summary>
        /// Gets or sets the fully qualified name of the <see cref="Page"/> to navigate to. This is a dependency property.
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
        /// Gets or sets the parameter which will be passed to the <see cref="Frame.Navigate(global::System.Type,object)"/> method.
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
        /// <param name="sender">The <see cref="object"/> that is passed to the action by the behavior. Generally this is <seealso cref="Microsoft.Xaml.Interactivity.IBehavior.AssociatedObject"/> or a target object.</param>
        /// <param name="parameter">The value of this parameter is determined by the caller.</param>
        /// <returns>True if the navigation to the specified page is successful; else false.</returns>
        public object Execute(object sender, object parameter)
        {
            if (string.IsNullOrEmpty(this.TargetPage))
            {
                return false;
            }

            // Temporary work around for Uno until it implements IXamlMetadataProvider
#if !HAS_UNO
            IXamlMetadataProvider metadataProvider = Application.Current as IXamlMetadataProvider;
            if (metadataProvider == null)
            {
                // This will happen if there are no XAML files in the project other than App.xaml.
                // The markup compiler doesn't bother implementing IXamlMetadataProvider on the app
                // in that case.
                return false;
            }

            IXamlType xamlType = metadataProvider.GetXamlType(this.TargetPage);
            if (xamlType == null)
            {
                return false;
            }
#endif

            INavigate navigateElement;
            if (sender is UIElement element && element.XamlRoot != null)
            {
                navigateElement = element.XamlRoot.Content as INavigate;
            }
            else
            {
                navigateElement = Window.Current?.Content as INavigate;
            }

            DependencyObject senderObject = sender as DependencyObject;

            // If the sender wasn't an INavigate, then keep looking up the tree from the
            // root we were given for another INavigate.
            while (senderObject != null && navigateElement == null)
            {
                navigateElement = senderObject as INavigate;
                if (navigateElement == null)
                {
                    senderObject = this._visualTreeHelper.GetParent(senderObject);
                }
            }

            if (navigateElement == null)
            {
                return false;
            }

            Frame frame = navigateElement as Frame;

            // Temporary work around for Uno until it implements IXamlMetadataProvider
#if !HAS_UNO
            var pageType = xamlType.UnderlyingType;
#else
            var pageType = AppDomain.CurrentDomain.GetAssemblies()
                .Select(a => a.GetType(this.TargetPage, false))
                .FirstOrDefault(t => t != null);
            if (pageType == null)
            {
                return false;
            }
#endif
            if (frame != null)
            {
                return frame.Navigate(pageType, this.Parameter ?? parameter);
            }
            else
            {
                return navigateElement.Navigate(pageType);
            }
        }
    }
}
