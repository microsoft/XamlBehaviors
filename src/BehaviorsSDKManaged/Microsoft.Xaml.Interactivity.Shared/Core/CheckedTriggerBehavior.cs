// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if WinUI
using Microsoft.UI.Xaml.Controls.Primitives;
#else
using Windows.UI.Xaml.Controls.Primitives;
#endif

namespace Microsoft.Xaml.Interactivity;

/// <summary>
/// A behavior that listens to an <see cref="ToggleButton"/> and executes its actions when the button is checked.
/// </summary>
public class CheckedTriggerBehavior : EventTriggerBehaviorBase<ToggleButton>
{
    /// <inheritdoc/>
    protected override bool RegisterEventCore(ToggleButton source)
    {
        source.Checked += OnEvent;
        return true;
    }

    /// <inheritdoc/>
    protected override void UnregisterEventCore(ToggleButton source)
    {
        source.Checked -= OnEvent;
    }
}
