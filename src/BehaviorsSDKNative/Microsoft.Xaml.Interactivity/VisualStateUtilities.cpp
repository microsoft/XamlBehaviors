// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
#pragma warning(disable: 6298)
#include "pch.h"
#include "VisualStateUtilities.h"

using namespace Platform;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::Foundation::Collections;
using namespace Microsoft::Xaml::Interactivity;

bool VisualStateUtilities::GoToState(Control^ control, String^ stateName, bool useTransitions)
{
	if (control == nullptr)
	{
		#pragma warning(suppress: 6298)
		throw ref new InvalidArgumentException("control");
	}

	if (stateName == nullptr)
	{
		#pragma warning(suppress: 6298)
		throw ref new InvalidArgumentException("stateName");
	}

	control->ApplyTemplate();
	return VisualStateManager::GoToState(control, stateName, useTransitions);
}

IVector<VisualStateGroup^>^ VisualStateUtilities::GetVisualStateGroups(FrameworkElement^ element)
{
	if (element == nullptr)
	{
		#pragma warning(suppress: 6298)
		throw ref new InvalidArgumentException("element");
	}

	IVector<VisualStateGroup^>^ visualStateGroups = VisualStateManager::GetVisualStateGroups(element);

	if (visualStateGroups == nullptr || visualStateGroups->Size == 0)
	{
		int childrenCount = VisualTreeHelper::GetChildrenCount(element);
		if (childrenCount > 0)
		{
			FrameworkElement^ childElement = dynamic_cast<FrameworkElement^>(VisualTreeHelper::GetChild(element, 0));
			if (childElement != nullptr)
			{
				visualStateGroups = VisualStateManager::GetVisualStateGroups(childElement);
			}
		}
	}

	return visualStateGroups;
}

Control^ VisualStateUtilities::FindNearestStatefulControl(FrameworkElement^ element)
{
	if (element == nullptr)
	{
		#pragma warning(suppress: 6298)
		throw ref new InvalidArgumentException("element");
	}

	// Try to find an element which is the immediate child of a UserControl, ControlTemplate or other such "boundary" element
	FrameworkElement^ parent = dynamic_cast<FrameworkElement^>(element->Parent);

	// bubble up looking for a place to stop
	while (!VisualStateUtilities::HasVisualStateGroupsDefined(element) && VisualStateUtilities::ShouldContinueTreeWalk(parent))
	{
		element = parent;
		parent = dynamic_cast<FrameworkElement^>(element->Parent);
	}

	if (VisualStateUtilities::HasVisualStateGroupsDefined(element))
	{
		// Once we've found such an element, use the VisualTreeHelper to get it's parent. For most elements the two are the 
		// same, but for children of a ControlElement this will give the control that contains the template.
		Control^ templatedParent = dynamic_cast<Control^>(VisualTreeHelper::GetParent(element));

		if (templatedParent != nullptr)
		{
			return templatedParent;
		}
		else
		{
			return  dynamic_cast<Control^>(element);
		}
	}

	return nullptr;
}

bool VisualStateUtilities::HasVisualStateGroupsDefined(FrameworkElement^ frameworkElement)
{
	return frameworkElement != nullptr && VisualStateManager::GetVisualStateGroups(frameworkElement)->Size != 0;
}

bool VisualStateUtilities::ShouldContinueTreeWalk(FrameworkElement^ element)
{
	if (element == nullptr || dynamic_cast<UserControl^>(element) != nullptr)
	{
		return false;
	}
	else if (element->Parent == nullptr)
	{
		// Stop if parent's parent is null AND parent isn't the template root of a ControlTemplate or DataTemplate
		FrameworkElement^ templatedParent = dynamic_cast<FrameworkElement^>(VisualTreeHelper::GetParent(element));
		if (templatedParent == nullptr || (!(dynamic_cast<Control^>(element) != nullptr) && !(dynamic_cast<ContentPresenter^>(element) != nullptr)))
		{
			return false;
		}
	}

	return true;
}
