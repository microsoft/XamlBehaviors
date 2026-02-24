// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Microsoft.Xaml.Interactivity;

/// <summary>
/// A behavior that listens to an <see cref="UIElement"/> and executes its actions when the UI element completes manipulation.
/// </summary>
public sealed class ManipulationCompletedTriggerBehavior : EventTriggerBehaviorBase<UIElement>
{
    /// <inheritdoc/>
    protected override bool RegisterEventCore(UIElement source)
    {
        source.ManipulationCompleted += OnEvent;
        return true;
    }

    /// <inheritdoc/>
    protected override void UnregisterEventCore(UIElement source)
    {
        source.ManipulationCompleted -= OnEvent;
    }
}
