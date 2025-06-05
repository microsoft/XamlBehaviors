// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Microsoft.Xaml.Interactivity;

/// <summary>
/// A behavior that listens to an <see cref="FrameworkElement"/> and executes its actions when the element is unloaded.
/// </summary>
public class UnloadedTriggerBehavior : EventTriggerBehaviorBase<FrameworkElement>
{
    /// <inheritdoc/>
    protected override bool RegisterEventCore(FrameworkElement source)
    {
        source.Unloaded += OnEvent;
        return true;
    }

    /// <inheritdoc/>
    protected override void UnregisterEventCore(FrameworkElement source)
    {
        source.Unloaded -= OnEvent;
    }
}
