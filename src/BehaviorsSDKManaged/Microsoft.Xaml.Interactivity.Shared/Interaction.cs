// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Microsoft.Xaml.Interactivity
{
    /// <summary>
    /// Defines a <see cref="BehaviorCollection"/> attached property and provides a method for executing an <seealso cref="ActionCollection"/>.
    /// </summary>
    public sealed class Interaction
    {
        /// <remarks>
        /// CA1053: Static holder types should not have public constructors
        /// </remarks>
        private Interaction()
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="BehaviorCollection"/> associated with a specified object.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty BehaviorsProperty = DependencyProperty.RegisterAttached(
            "Behaviors",
            typeof(BehaviorCollection),
            typeof(Interaction),
            new PropertyMetadata(null, new PropertyChangedCallback(Interaction.OnBehaviorsChanged)));

        /// <summary>
        /// Gets the <see cref="BehaviorCollection"/> associated with a specified object.
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.DependencyObject"/> from which to retrieve the <see cref="BehaviorCollection"/>.</param>
        /// <returns>A <see cref="BehaviorCollection"/> containing the behaviors associated with the specified object.</returns>
        public static BehaviorCollection GetBehaviors(DependencyObject obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            BehaviorCollection behaviorCollection = (BehaviorCollection)obj.GetValue(Interaction.BehaviorsProperty);
            if (behaviorCollection == null)
            {
                behaviorCollection = new BehaviorCollection();
                obj.SetValue(Interaction.BehaviorsProperty, behaviorCollection);

                var frameworkElement = obj as FrameworkElement;

                if (frameworkElement != null)
                {
                    frameworkElement.Loaded -= FrameworkElement_Loaded;
                    frameworkElement.Loaded += FrameworkElement_Loaded;
                    frameworkElement.Unloaded -= FrameworkElement_Unloaded;
                    frameworkElement.Unloaded += FrameworkElement_Unloaded;
                }
            }

            return behaviorCollection;
        }

        /// <summary>
        /// Sets the <see cref="BehaviorCollection"/> associated with a specified object.
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.DependencyObject"/> on which to set the <see cref="BehaviorCollection"/>.</param>
        /// <param name="value">The <see cref="BehaviorCollection"/> associated with the object.</param>
        public static void SetBehaviors(DependencyObject obj, BehaviorCollection value)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            obj.SetValue(Interaction.BehaviorsProperty, value);
        }

        /// <summary>
        /// Executes all actions in the <see cref="ActionCollection"/> and returns their results.
        /// </summary>
        /// <param name="sender">The <see cref="System.Object"/> which will be passed on to the action.</param>
        /// <param name="actions">The set of actions to execute.</param>
        /// <param name="parameter">The value of this parameter is determined by the calling behavior.</param>
        /// <returns>Returns the results of the actions.</returns>
        public static IEnumerable<object> ExecuteActions(object sender, ActionCollection actions, object parameter)
        {
            List<object> results = new List<object>();

            if (actions == null || Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                return results;
            }

            foreach (DependencyObject dependencyObject in actions)
            {
                IAction action = (IAction)dependencyObject;
                results.Add(action.Execute(sender, parameter));
            }

            return results;
        }

        private static void OnBehaviorsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            BehaviorCollection oldCollection = (BehaviorCollection)args.OldValue;
            BehaviorCollection newCollection = (BehaviorCollection)args.NewValue;

            if (oldCollection == newCollection)
            {
                return;
            }

            if (oldCollection != null && oldCollection.AssociatedObject != null)
            {
                oldCollection.Detach();
            }

            if (newCollection != null && sender != null)
            {
                newCollection.Attach(sender);
            }
        }

        private static void FrameworkElement_Loaded(object sender, RoutedEventArgs e)
        {
            var d = sender as DependencyObject;

            if (d != null)
            {
                GetBehaviors(d).Attach(d);
            }
        }

        private static void FrameworkElement_Unloaded(object sender, RoutedEventArgs e)
        {
            var d = sender as DependencyObject;

            if (d != null)
            {
                GetBehaviors(d).Detach();
            }
        }
    }
}