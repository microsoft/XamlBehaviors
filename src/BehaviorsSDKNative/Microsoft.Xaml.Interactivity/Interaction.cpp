// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
#include "pch.h"
#include "Interaction.h"
#include "BehaviorCollection.h"
#include "IAction.h"

using namespace Platform;
using namespace Platform::Collections;
using namespace Windows::Foundation::Collections;
using namespace Windows::UI::Xaml;
using namespace Microsoft::Xaml::Interactivity;

namespace DependencyProperties
{
	static DependencyProperty^ Behaviors = DependencyProperty::RegisterAttached(
		"Behaviors",
		BehaviorCollection::typeid,
		Interaction::typeid,
		ref new PropertyMetadata(nullptr, ref new PropertyChangedCallback(&Interaction::OnBehaviorsChanged)));
}

void Interaction::OnBehaviorsChanged(DependencyObject^ sender, DependencyPropertyChangedEventArgs^ args)
{
	BehaviorCollection^ oldCollection = safe_cast<BehaviorCollection^>(args->OldValue);
	BehaviorCollection^ newCollection = safe_cast<BehaviorCollection^>(args->NewValue);

	if (oldCollection == newCollection)
	{
		return;
	}

	if (oldCollection != nullptr && oldCollection->AssociatedObject != nullptr)
	{
		oldCollection->Detach();
	}

	if (newCollection != nullptr && sender != nullptr)
	{
		newCollection->Attach(sender);
	}
}

DependencyProperty^ Interaction::BehaviorsProperty::get()
{
	return DependencyProperties::Behaviors;
}

IIterable<Object^>^ Interaction::ExecuteActions(Object^ sender, ActionCollection^ actions, Object^ parameter)
{
	if (actions == nullptr || Windows::ApplicationModel::DesignMode::DesignModeEnabled)
	{
		return ref new Vector<Object^>();
	}

	std::vector<Object^> results;
	for (auto& dependencyObject : actions)
	{
		IAction^ action = safe_cast<IAction^>(static_cast<Object^>(dependencyObject));
		results.push_back(action->Execute(sender, parameter));
	}

	return ref new Vector<Object^>(std::move(results));
}

BehaviorCollection^ Interaction::GetBehaviors(DependencyObject^ obj)
{
	if (obj == nullptr)
	{
		#pragma warning(suppress: 6298)
		throw ref new InvalidArgumentException("obj");
	}

	BehaviorCollection^ behaviors = safe_cast<BehaviorCollection^>(obj->GetValue(Interaction::BehaviorsProperty));
	if (behaviors == nullptr)
	{
		behaviors = ref new BehaviorCollection();
		Interaction::SetBehaviors(obj, behaviors);
	}

	return behaviors;
}

void Interaction::SetBehaviors(DependencyObject^ obj, BehaviorCollection^ value)
{
	if (obj == nullptr)
	{
		#pragma warning(suppress: 6298)
		throw ref new InvalidArgumentException("obj");
	}

	obj->SetValue(Interaction::BehaviorsProperty, value);
}
