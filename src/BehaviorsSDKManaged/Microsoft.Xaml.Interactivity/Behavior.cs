// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Globalization;
using Microsoft.UI.Xaml;

namespace Microsoft.Xaml.Interactivity
{
    /// <summary>
    /// A base class for behaviors, implementing the basic plumbing of IBehavior
    /// </summary>
    public abstract class Behavior : DependencyObject, IBehavior
    {
        /// <summary>
        /// Gets the <see cref="Microsoft.UI.Xaml.DependencyObject"/> to which the behavior is attached.
        /// </summary>
        public DependencyObject AssociatedObject { get; private set; }

        /// <summary>
        /// Attaches the behavior to the specified <see cref="Microsoft.UI.Xaml.DependencyObject"/>.
        /// </summary>
        /// <param name="associatedObject">The <see cref="Microsoft.UI.Xaml.DependencyObject"/> to which to attach.</param>
        /// <exception cref="ArgumentNullException"><paramref name="associatedObject"/> is null.</exception>
        public void Attach(DependencyObject associatedObject)
        {
            if (associatedObject == this.AssociatedObject || Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                return;
            }

            if (this.AssociatedObject != null)
            {
                throw new InvalidOperationException(string.Format(
                    CultureInfo.CurrentCulture,
                    ResourceHelper.CannotAttachBehaviorMultipleTimesExceptionMessage,
                    associatedObject,
                    this.AssociatedObject));
            }

            Debug.Assert(associatedObject != null, "Cannot attach the behavior to a null object.");

            if (associatedObject == null) throw new ArgumentNullException(nameof(associatedObject));

            AssociatedObject = associatedObject;
            OnAttached();
        }

        /// <summary>
        /// Detaches the behaviors from the <see cref="Microsoft.Xaml.Interactivity.Behavior.AssociatedObject"/>.
        /// </summary>
        public void Detach()
        {
            OnDetaching();
            AssociatedObject = null;
        }

        /// <summary>
        /// Called after the behavior is attached to the <see cref="Microsoft.Xaml.Interactivity.Behavior.AssociatedObject"/>.
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the <see cref="Microsoft.Xaml.Interactivity.Behavior.AssociatedObject"/>
        /// </remarks>
        protected virtual void OnAttached()
        {
        }

        /// <summary>
        /// Called when the behavior is being detached from its <see cref="Microsoft.Xaml.Interactivity.Behavior.AssociatedObject"/>.
        /// </summary>
        /// <remarks>
        /// Override this to unhook functionality from the <see cref="Microsoft.Xaml.Interactivity.Behavior.AssociatedObject"/>
        /// </remarks>
        protected virtual void OnDetaching()
        {
        }
    }
}
