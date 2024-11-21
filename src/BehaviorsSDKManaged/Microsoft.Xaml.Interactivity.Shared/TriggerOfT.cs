// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Microsoft.Xaml.Interactivity;

/// <summary>
/// A base class for behaviors, implementing the basic plumbing of ITrigger
/// </summary>
/// <typeparam name="T">The object type to attach to</typeparam>
public abstract class Trigger<T> : Trigger where T : DependencyObject
{
    /// <summary>
    /// Gets the object to which this behavior is attached.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new T AssociatedObject
    {
        get { return base.AssociatedObject as T; }
    }

    /// <summary>
    /// Called after the behavior is attached to the <see cref="Microsoft.Xaml.Interactivity.Behavior.AssociatedObject"/>.
    /// </summary>
    /// <remarks>
    /// Override this to hook up functionality to the <see cref="Microsoft.Xaml.Interactivity.Behavior.AssociatedObject"/>
    /// </remarks>
    protected override void OnAttached()
    {
        base.OnAttached();

        if (this.AssociatedObject == null)
        {
            string actualType = base.AssociatedObject.GetType().FullName;
            string expectedType = typeof(T).FullName;
            string message = string.Format(ResourceHelper.GetString("InvalidAssociatedObjectExceptionMessage"), actualType, expectedType);
            throw new InvalidOperationException(message);
        }
    }
}
