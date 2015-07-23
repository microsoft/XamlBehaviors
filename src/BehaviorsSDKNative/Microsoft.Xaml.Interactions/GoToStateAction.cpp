// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
#include "pch.h"
#include "GoToStateAction.h"
#include "ResourceHelper.h"
#include "EventTriggerBehavior.h"

using namespace Platform;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Microsoft::Xaml::Interactivity;
using namespace Microsoft::Xaml::Interactions::Core;

namespace DependencyProperties
{
	static DependencyProperty^ TargetObject = DependencyProperty::Register(
		"TargetObject",
		FrameworkElement::typeid,
		GoToStateAction::typeid,
		ref new PropertyMetadata(nullptr));

	static DependencyProperty^ StateName = DependencyProperty::Register(
		"StateName",
		String::typeid,
		GoToStateAction::typeid,
		ref new PropertyMetadata(nullptr));

	static DependencyProperty^ UseTransitions = DependencyProperty::Register(
		"UseTransitions",
		Boolean::typeid,
		GoToStateAction::typeid,
		ref new PropertyMetadata(true));
}

DependencyProperty^ GoToStateAction::TargetObjectProperty::get()
{
	return DependencyProperties::TargetObject;
}

DependencyProperty^ GoToStateAction::StateNameProperty::get()
{
	return DependencyProperties::StateName;
}

DependencyProperty^ GoToStateAction::UseTransitionsProperty::get()
{
	return DependencyProperties::UseTransitions;
}

Object^ GoToStateAction::Execute(Object^ sender, Object^ parameter)
{
	if (this->StateName == nullptr)
	{
		return false;
	}

	
	if (this->ReadLocalValue(GoToStateAction::TargetObjectProperty) != DependencyProperty::UnsetValue)
	{
		Control^ control = dynamic_cast<Control^>(this->TargetObject);
		if (control == nullptr)
		{
			return false;
		}

		return VisualStateUtilities::GoToState(control, this->StateName, this->UseTransitions);
	}

	FrameworkElement^ element = dynamic_cast<FrameworkElement^>(sender);
	if (element == nullptr || !EventTriggerBehavior::IsElementLoaded(element))
	{
		return false;
	}

	Control^ resolvedControl = VisualStateUtilities::FindNearestStatefulControl(element);
	if (resolvedControl == nullptr)
	{
		throw ref new FailureException(ResourceHelper::GetString("GoToStateActionTargetHasNoStateGroups", element->Name));
	}

	return VisualStateUtilities::GoToState(resolvedControl, this->StateName, this->UseTransitions);
}
