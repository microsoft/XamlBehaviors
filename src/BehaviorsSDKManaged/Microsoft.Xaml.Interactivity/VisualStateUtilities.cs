// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Xaml.Interactivity
{
    /// <summary>
    /// Provides various standard operations for working with <seealso cref="Microsoft.UI.Xaml.VisualStateManager"/>.
    /// </summary>
    public static class VisualStateUtilities
    {
        /// <summary>
        /// Transitions the control between two states.
        /// </summary>
        /// <param name="control">The <see cref="Microsoft.UI.Xaml.Controls.Control"/> to transition between states.</param>
        /// <param name="stateName">The state to transition to.</param>
        /// <param name="useTransitions">True to use a <see cref="Microsoft.UI.Xaml.VisualTransition"/> to transition between states; otherwise, false.</param>
        /// <returns>True if the <paramref name="control"/> is successfully transitioned to the new state; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="control"/> or <paramref name="stateName"/> is null.</exception>
        public static bool GoToState(Control control, string stateName, bool useTransitions)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            if (string.IsNullOrEmpty(stateName))
            {
                throw new ArgumentNullException(nameof(stateName));
            }

            control.ApplyTemplate();
            return VisualStateManager.GoToState(control, stateName, useTransitions);
        }

        /// <summary>
        /// Gets the value of the VisualStateManager.VisualStateGroups attached property.
        /// </summary>
        /// <param name="element">The <see cref="Microsoft.UI.Xaml.FrameworkElement"/> from which to get the VisualStateManager.VisualStateGroups.</param>
        /// <returns>The list of VisualStateGroups in the given element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> is null.</exception>
        public static IList<VisualStateGroup> GetVisualStateGroups(FrameworkElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            IList<VisualStateGroup> visualStateGroups = VisualStateManager.GetVisualStateGroups(element);

            if (visualStateGroups == null || visualStateGroups.Count == 0)
            {
                int childrenCount = VisualTreeHelper.GetChildrenCount(element);
                if (childrenCount > 0)
                {
                    FrameworkElement childElement = VisualTreeHelper.GetChild(element, 0) as FrameworkElement;
                    if (childElement != null)
                    {
                        visualStateGroups = VisualStateManager.GetVisualStateGroups(childElement);
                    }
                }
            }

            return visualStateGroups;
        }

        /// <summary>
        /// Find the nearest parent which contains visual states.
        /// </summary>
        /// <param name="element">The <see cref="Microsoft.UI.Xaml.FrameworkElement"/> from which to find the nearest stateful control.</param>
        /// <returns>The nearest <see cref="Microsoft.UI.Xaml.Controls.Control"/> that contains visual states; else null.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> is null.</exception>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Stateful")]
        public static Control FindNearestStatefulControl(FrameworkElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            // Try to find an element which is the immediate child of a UserControl, ControlTemplate or other such "boundary" element
            FrameworkElement parent = element.Parent as FrameworkElement;

            // bubble up looking for a place to stop
            while (!VisualStateUtilities.HasVisualStateGroupsDefined(element) && VisualStateUtilities.ShouldContinueTreeWalk(parent))
            {
                element = parent;
                parent = parent.Parent as FrameworkElement;
            }

            if (VisualStateUtilities.HasVisualStateGroupsDefined(element))
            {
                // Once we've found such an element, use the VisualTreeHelper to get it's parent. For most elements the two are the 
                // same, but for children of a ControlElement this will give the control that contains the template.
                Control templatedParent = VisualTreeHelper.GetParent(element) as Control;

                if (templatedParent != null)
                {
                    return templatedParent;
                }
                else
                {
                    return element as Control;
                }
            }

            return null;
        }

        private static bool HasVisualStateGroupsDefined(FrameworkElement element)
        {
            return element != null && VisualStateManager.GetVisualStateGroups(element).Count != 0;
        }

        private static bool ShouldContinueTreeWalk(FrameworkElement element)
        {
            if (element == null)
            {
                return false;
            }
            else if (element is UserControl)
            {
                return false;
            }
            else if (element.Parent == null)
            {
                // Stop if parent's parent is null AND parent isn't the template root of a ControlTemplate or DataTemplate.
                FrameworkElement templatedParent = VisualTreeHelper.GetParent(element) as FrameworkElement;
                if (templatedParent == null || (!(templatedParent is Control) && !(templatedParent is ContentPresenter)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
