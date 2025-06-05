// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if WinUI
using Microsoft.UI.Xaml.Input;
#else
using Windows.UI.Xaml.Input;
#endif

namespace Microsoft.Xaml.Interactivity;

/// <summary>
/// A behavior that listens to an <see cref="KeyboardAccelerator"/> and executes its actions when the accelerator is invoked.
/// </summary>
public sealed class KeyboardAcceleratorInvokedTriggerBehavior : EventTriggerBehaviorBase<KeyboardAccelerator>
{
    /// <inheritdoc/>
    protected override bool RegisterEventCore(KeyboardAccelerator source)
    {
        source.Invoked += OnEvent;
        return true;
    }

    /// <inheritdoc/>
    protected override void UnregisterEventCore(KeyboardAccelerator source)
    {
        source.Invoked -= OnEvent;
    }
}
