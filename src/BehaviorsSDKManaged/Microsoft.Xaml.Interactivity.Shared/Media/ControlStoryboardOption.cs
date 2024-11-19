#if WinUI
using Microsoft.UI.Xaml.Media.Animation;
#else
using Windows.UI.Xaml.Media.Animation;
#endif

// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace Microsoft.Xaml.Interactivity;

/// <summary>
/// Represents operations that can be applied to <seealso cref="Storyboard"/>.
/// </summary>
public enum ControlStoryboardOption
{
    /// <summary>
    /// Specifies the play operation.
    /// </summary>
    Play,
    /// <summary>
    /// Specifies the stop operation.
    /// </summary>
    Stop,
    /// <summary>
    /// Specifies the TogglePlayPause operation.
    /// </summary>
    TogglePlayPause,
    /// <summary>
    /// Specifies the pause operation.
    /// </summary>
    Pause,
    /// <summary>
    /// Specifies the resume operation.
    /// </summary>
    Resume,
    /// <summary>
    /// Specifies the SkipToFill operation.
    /// </summary>
    SkipToFill
}
