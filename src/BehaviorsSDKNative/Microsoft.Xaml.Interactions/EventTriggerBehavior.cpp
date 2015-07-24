// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
#pragma warning(disable: 6298)
#include "pch.h"
#include "EventTriggerBehavior.h"
#include "EventManager.h"
#include "ResourceHelper.h"

using namespace Platform;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::Foundation::Collections;
using namespace Microsoft::Xaml::Interactivity;
using namespace Microsoft::Xaml::Interactions::Core;

namespace DependencyProperties
{
	static DependencyProperty^ SourceObject = DependencyProperty::Register(
		"SourceObject",
		Object::typeid,
		EventTriggerBehavior::typeid,
		ref new PropertyMetadata(nullptr, ref new PropertyChangedCallback(&EventTriggerBehavior::OnSourceObjectChanged)));

	static DependencyProperty^ EventName = DependencyProperty::Register(
		"EventName",
		String::typeid,
		EventTriggerBehavior::typeid,
		ref new PropertyMetadata("Loaded", ref new PropertyChangedCallback(&EventTriggerBehavior::OnEventNameChanged)));

	static DependencyProperty^ Actions = DependencyProperty::Register(
		"Actions",
		ActionCollection::typeid,
		EventTriggerBehavior::typeid,
		ref new PropertyMetadata(nullptr));
}

void EventTriggerBehavior::OnEventNameChanged(DependencyObject^ sender, DependencyPropertyChangedEventArgs^ args)
{
	EventTriggerBehavior^ eventTriggerBehavior = safe_cast<EventTriggerBehavior^>(sender);
	if (eventTriggerBehavior->AssociatedObject == nullptr || eventTriggerBehavior->resolvedSource == nullptr)
	{
		return;
	}

	String^ oldEventName = safe_cast<String^>(args->OldValue);
	String^ newEventName = safe_cast<String^>(args->NewValue);

	eventTriggerBehavior->UnregisterEvent(oldEventName);
	eventTriggerBehavior->RegisterEvent(newEventName);
}

void EventTriggerBehavior::OnSourceObjectChanged(DependencyObject^ sender, DependencyPropertyChangedEventArgs^ args)
{
	EventTriggerBehavior^ eventTriggerBehavior = safe_cast<EventTriggerBehavior^>(sender);
	eventTriggerBehavior->SetResolvedSource(eventTriggerBehavior->ComputeResolvedSource());
}

DependencyProperty^ EventTriggerBehavior::SourceObjectProperty::get()
{
	return DependencyProperties::SourceObject;
}

DependencyProperty^ EventTriggerBehavior::EventNameProperty::get()
{
	return DependencyProperties::EventName;
}

DependencyProperty^ EventTriggerBehavior::ActionsProperty::get()
{
	return DependencyProperties::Actions;
}

void EventTriggerBehavior::Attach(DependencyObject^ associatedObject)
{
	if (associatedObject == this->AssociatedObject)
	{
		return;
	}

	if (Windows::ApplicationModel::DesignMode::DesignModeEnabled)
	{
		return;
	}

	if (this->AssociatedObject != nullptr)
	{
		throw ref new FailureException(ResourceHelper::GetString("CannotAttachBehaviorMultipleTimesExceptionMessage"));
	}

	this->associatedObject = associatedObject;
	this->SetResolvedSource(this->ComputeResolvedSource());
}

Object^ EventTriggerBehavior::ComputeResolvedSource()
{
	if (this->ReadLocalValue(EventTriggerBehavior::SourceObjectProperty) != DependencyProperty::UnsetValue)
	{
		return this->SourceObject;
	}
	
	return this->AssociatedObject;
}

void EventTriggerBehavior::SetResolvedSource(Object^ newSource)
{
	if (this->AssociatedObject == nullptr || this->resolvedSource == newSource)
	{
		return;
	}

	if (this->resolvedSource != nullptr)
	{
		this->UnregisterEvent(this->EventName);
	}

	this->resolvedSource = newSource;

	if (this->resolvedSource != nullptr)
	{
		this->RegisterEvent(this->EventName);
	}
}

void EventTriggerBehavior::RegisterEvent(String^ eventName)
{
	if (eventName == nullptr)
	{
		return;
	}

	if (eventName != "Loaded")
	{
		this->registeredToken = EventManager::Register(eventName, this->resolvedSource, ref new Action(this, &EventTriggerBehavior::InvokeActions));
	}
	else if (!this->isLoadedRegistered)
	{
		FrameworkElement^ element = dynamic_cast<FrameworkElement^>(this->resolvedSource);
		if (element != nullptr && !EventTriggerBehavior::IsElementLoaded(element))
		{
			this->isLoadedRegistered = true;
			this->loadedToken = element->Loaded += ref new RoutedEventHandler(this, &EventTriggerBehavior::OnLoaded);
		}
	}
}

void EventTriggerBehavior::UnregisterEvent(String^ eventName)
{
	if (eventName == nullptr)
	{
		return;
	}

	if (eventName == "Loaded")
	{
		if (this->isLoadedRegistered)
		{
			this->isLoadedRegistered = false;
			FrameworkElement^ element = safe_cast<FrameworkElement^>(this->resolvedSource);
			element->Loaded -= this->loadedToken;
		}
	}
	else
	{
		EventManager::Unregister(eventName, this->resolvedSource, this->registeredToken);
	}
}

void EventTriggerBehavior::InvokeActions(Object^ eventArgs)
{
	Interaction::ExecuteActions(this->resolvedSource, this->Actions, eventArgs);
}

void EventTriggerBehavior::Detach()
{
	this->SetResolvedSource(nullptr);
	this->associatedObject = nullptr;
}

void EventTriggerBehavior::OnLoaded(Object^ sender, RoutedEventArgs^ e)
{
	this->InvokeActions(e);
}

bool EventTriggerBehavior::IsElementLoaded(FrameworkElement^ element)
{
	UIElement^ rootVisual = Window::Current->Content;

	DependencyObject^ parent = element->Parent;

	if (parent == nullptr)
	{
		parent = VisualTreeHelper::GetParent(element);
	}

	return (parent != nullptr || (rootVisual != nullptr && element == rootVisual));
}
