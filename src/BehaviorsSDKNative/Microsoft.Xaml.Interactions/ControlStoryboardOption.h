// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
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