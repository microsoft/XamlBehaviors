// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Globalization;
using Windows.UI.Xaml;

namespace Microsoft.Xaml.Interactivity
{
    /// <summary>
    /// A base class for behaviors, implementing the basic plumbing of IBehavior
    /// </summary>
    public abstract partial class Behavior : DependencyObject, IBehavior
#if HAS_UNO
        , IBehavior2
#endif
    {
        /// <summary>
        /// Gets the <see cref="Windows.UI.Xaml.DependencyObject"/> to which the behavior is attached.
        /// </summary>
        public DependencyObject AssociatedObject { get; private set; }

#if HAS_UNO
        private WeakReference _associatedObjectWeak;

        DependencyObject IBehavior2.AssociatedObjectWeak {
            get => _associatedObjectWeak?.Target as DependencyObject;
            set
            {
                if (value != null)
                {
                    _associatedObjectWeak = new WeakReference(value);
                }
                else
                {
                    _associatedObjectWeak = null;
                }
            }
        }
#endif

        /// <summary>
        /// Attaches the behavior to the specified <see cref="Windows.UI.Xaml.DependencyObject"/>.
        /// </summary>
        /// <param name="associatedObject">The <see cref="Windows.UI.Xaml.DependencyObject"/> to which to attach.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="associatedObject"/> is null.</exception>
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

#if HAS_UNO
            (this as IBehavior2).AssociatedObjectWeak = associatedObject;
#endif
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
