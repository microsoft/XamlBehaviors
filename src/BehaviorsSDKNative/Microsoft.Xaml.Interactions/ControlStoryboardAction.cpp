// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#include "pch.h"
#include "ControlStoryboardAction.h"
#include "ControlStoryboardOption.h"

using namespace Platform;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Media::Animation;
using namespace Microsoft::Xaml::Interactivity;
using namespace Microsoft::Xaml::Interactions::Media;

namespace DependencyProperties
{
	static DependencyProperty^ Storyboard = DependencyProperty::Register(
		"Storyboard",
		Storyboard::typeid,
		ControlStoryboardAction::typeid,
		ref new PropertyMetadata(nullptr, ref new PropertyChangedCallback(&ControlStoryboardAction::OnStoryboardChanged)));

	static DependencyProperty^ ControlStoryboardOption = DependencyProperty::Register(
		"ControlStoryboardOption",
		Microsoft::Xaml::Interactions::Media::ControlStoryboardOption::typeid,
		ControlStoryboardAction::typeid,
		ref new PropertyMetadata(Microsoft::Xaml::Interactions::Media::ControlStoryboardOption::Play));
}

void ControlStoryboardAction::OnStoryboardChanged(DependencyObject^ sender, DependencyPropertyChangedEventArgs^ args)
{
	ControlStoryboardAction^ action = dynamic_cast<ControlStoryboardAction^>(sender);
	if (action != nullptr)
	{
		action->isPaused = false;
	}
}

DependencyProperty^ ControlStoryboardAction::StoryboardProperty::get()
{
	return DependencyProperties::Storyboard;
}

DependencyProperty^ ControlStoryboardAction::ControlStoryboardOptionProperty::get()
{
	return DependencyProperties::ControlStoryboardOption;
}

Object^ ControlStoryboardAction::Execute(Object^ sender, Object^ parameter)
{
	if (this->Storyboard == nullptr)
	{
		return false;
	}

	switch (this->ControlStoryboardOption)
	{
	case Media::ControlStoryboardOption::Play:
		this->Storyboard->Begin();
		break;

	case Media::ControlStoryboardOption::Stop:
		this->Storyboard->Stop();
		break;

	case Media::ControlStoryboardOption::TogglePlayPause:
		{
			ClockState currentState = this->Storyboard->GetCurrentState();

			if (currentState == ClockState::Stopped)
			{
				this->isPaused = false;
				this->Storyboard->Begin();
			}
			else if (this->isPaused)
			{
				this->isPaused = false;
				this->Storyboard->Resume();
			}
			else
			{
				this->isPaused = true;
				this->Storyboard->Pause();
			}
		}

		break;

	case  Media::ControlStoryboardOption::Pause:
		this->Storyboard->Pause();
		break;

	case  Media::ControlStoryboardOption::Resume:
		this->Storyboard->Resume();
		break;

	case  Media::ControlStoryboardOption::SkipToFill:
		this->Storyboard->SkipToFill();
		break;

	default:
		return false;
	}

	return true;
}