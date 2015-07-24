// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
#pragma once

namespace Microsoft { namespace Xaml { namespace Interactions { namespace Media
{
	/// <summary>
	/// Represents operations that can be applied to <seealso cref="Windows::UI::Xaml::Media::Animation::Storyboard"/>.
	/// </summary>
	public enum class ControlStoryboardOption
	{
		Play,
		Stop,
		TogglePlayPause,
		Pause,
		Resume,
		SkipToFill
	};

}}}}