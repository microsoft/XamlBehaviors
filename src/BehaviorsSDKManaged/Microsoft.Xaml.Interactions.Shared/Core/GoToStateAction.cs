// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace Microsoft.Xaml.Interactions.Core
{
    using System;
    using System.Globalization;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Interactivity;

    /// <summary>
    /// An action that will transition a <see cref="Windows.UI.Xaml.FrameworkElement"/> to a specified <seealso cref="Windows.UI.Xaml.VisualState"/> when executed.
    /// </summary>
    /// <remarks>
    /// If the <seealso cref="TargetObject"/> property is set, this action will attempt to change the state of the targeted element. If it is not set, the action walks
    /// the element tree in an attempt to locate an alternative target that defines states. <see cref="Windows.UI.Xaml.Controls.ControlTemplate"/> and <see cref="Windows.UI.Xaml.Controls.UserControl"/> are 
    /// two common results.
    /// </remarks>
    public sealed class GoToStateAction : DependencyObject, IAction
    {
        /// <summary>
        /// Identifies the <seealso cref="UseTransitions"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty UseTransitionsProperty = DependencyProperty.Register(
            "UseTransitions",
            typeof(bool),
            typeof(GoToStateAction),
            new PropertyMetadata(true));

        /// <summary>
        /// Identifies the <seealso cref="StateName"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty StateNameProperty = DependencyProperty.Register(
            "StateName",
            typeof(string),
            typeof(GoToStateAction),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <seealso cref="TargetObject"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register(
            "TargetObject",
            typeof(FrameworkElement),
            typeof(GoToStateAction),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets whether or not to use a <see cref="Windows.UI.Xaml.VisualTransition"/> to transition between states. This is a dependency property.
        /// </summary>
        public bool UseTransitions
        {
            get
            {
                return (bool)this.GetValue(GoToStateAction.UseTransitionsProperty);
            }
            set
            {
                this.SetValue(GoToStateAction.UseTransitionsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the <see cref="Windows.UI.Xaml.VisualState"/>. This is a dependency property.
        /// </summary>
        public string StateName
        {
            get
            {
                return (string)this.GetValue(GoToStateAction.StateNameProperty);
            }
            set
            {
                this.SetValue(GoToStateAction.StateNameProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the target object. This is a dependency property.
        /// </summary>
        public FrameworkElement TargetObject
        {
            get
            {
                return (FrameworkElement)this.GetValue(GoToStateAction.TargetObjectProperty);
            }
            set
            {
                this.SetValue(GoToStateAction.TargetObjectProperty, value);
            }
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="sender">The <see cref="System.Object"/> that is passed to the action by the behavior. Generally this is <seealso cref="Microsoft.Xaml.Interactivity.IBehavior.AssociatedObject"/> or a target object.</param>
        /// <param name="parameter">The value of this parameter is determined by the caller.</param>
        /// <returns>True if the transition to the specified state succeeds; else false.</returns>
        public object Execute(object sender, object parameter)
        {
            if (string.IsNullOrEmpty(this.StateName))
            {
                return false;
            }

            if (this.ReadLocalValue(GoToStateAction.TargetObjectProperty) != DependencyProperty.UnsetValue)
            {
                Control control = this.TargetObject as Control;
                if (control == null)
                {
                    return false;
                }

                return VisualStateUtilities.GoToState(control, this.StateName, this.UseTransitions);
            }

            FrameworkElement element = sender as FrameworkElement;
            if (element == null || !EventTriggerBehavior.IsElementLoaded(element))
            {
                return false;
            }

            Control resolvedControl = VisualStateUtilities.FindNearestStatefulControl(element);
            if (resolvedControl == null)
            {
                throw new InvalidOperationException(string.Format(
                    CultureInfo.CurrentCulture,
                    ResourceHelper.GoToStateActionTargetHasNoStateGroups,
                    element.Name));
            }

            return VisualStateUtilities.GoToState(resolvedControl, this.StateName, this.UseTransitions);
        }
    }
}
