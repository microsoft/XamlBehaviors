// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace Microsoft.Xaml.Interactions.Media
{
	/// <summary>
	/// Represents operations that can be applied to <seealso cref="Windows.UI.Xaml.Media.Animation.Storyboard"/>.
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
}
