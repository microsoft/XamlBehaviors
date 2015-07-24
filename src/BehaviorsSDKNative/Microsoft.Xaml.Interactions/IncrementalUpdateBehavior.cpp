// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
#pragma warning(disable: 6298)
#include "pch.h"
#include "IncrementalUpdateBehavior.h"
#include "ResourceHelper.h"

using namespace std;
using namespace Platform;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Controls::Primitives;
using namespace Windows::UI::Xaml::Media;
using namespace Microsoft::Xaml::Interactivity;
using namespace Microsoft::Xaml::Interactions::Core;

namespace DependencyProperties
{
	static DependencyProperty^ Phase = DependencyProperty::Register(
		"Phase",
		int::typeid,
		IncrementalUpdateBehavior::typeid,
		ref new PropertyMetadata(1, ref new PropertyChangedCallback(&IncrementalUpdateBehavior::OnPhaseChanged)));

	static DependencyProperty^ IncrementalUpdater = DependencyProperty::RegisterAttached(
		"IncrementalUpdater",
		IncrementalUpdateBehavior::IncrementalUpdater::typeid,
		IncrementalUpdateBehavior::typeid,
		ref new PropertyMetadata(nullptr, ref new PropertyChangedCallback(&IncrementalUpdateBehavior::OnIncrementalUpdaterChanged)));
}

void IncrementalUpdateBehavior::IncrementalUpdater::PhasedElementRecord::FreezeAndHide()
{
	if (_isFrozen)
	{
		return;
	}

	_isFrozen = true;
	_localOpacity = _frameworkElement->ReadLocalValue(FrameworkElement::OpacityProperty);
	_localDataContext = _frameworkElement->ReadLocalValue(FrameworkElement::DataContextProperty);
	_frameworkElement->Opacity = 0.0;
	_frameworkElement->DataContext = _frameworkElement->DataContext;
}

void IncrementalUpdateBehavior::IncrementalUpdater::PhasedElementRecord::ThawAndShow()
{
	if (!_isFrozen)
	{
		return;
	}

	if (_localOpacity != DependencyProperty::UnsetValue)
	{
		_frameworkElement->SetValue(FrameworkElement::OpacityProperty, _localOpacity);
	}
	else
	{
		_frameworkElement->ClearValue(FrameworkElement::OpacityProperty);
	}

	if (_localDataContext != DependencyProperty::UnsetValue)
	{
		_frameworkElement->SetValue(FrameworkElement::DataContextProperty, _localDataContext);
	}
	else
	{
		_frameworkElement->ClearValue(FrameworkElement::DataContextProperty);
	}

	_isFrozen = false;
}

UIElement^ IncrementalUpdateBehavior::IncrementalUpdater::FindContentTemplateRoot(FrameworkElement^ phaseElement)
{
	DependencyObject^ ancestor = static_cast<DependencyObject^>(phaseElement) ;
	while (ancestor != nullptr)
	{
		DependencyObject^ parent = VisualTreeHelper::GetParent(ancestor);
		SelectorItem^ item = dynamic_cast<SelectorItem^>(parent);

		if (item != nullptr)
		{
			return item->ContentTemplateRoot;
		}
		ancestor = parent;
	}

	return nullptr;
}

void IncrementalUpdateBehavior::IncrementalUpdater::CachePhaseElement(FrameworkElement^ phaseElement, int phase)
{
	if (phase < 0)
	{
		throw ref new OutOfBoundsException("phase");
	}

	if (phase <= 0)
	{
		return;
	}

	UIElement^ contentTemplateRoot = IncrementalUpdateBehavior::IncrementalUpdater::FindContentTemplateRoot(phaseElement);
	if (contentTemplateRoot != nullptr)
	{
		// get the cache for this element
		auto& elementCacheRecord = _elementCache[contentTemplateRoot];

		// get the cache for this phase
		auto& elementPhaseRecord = elementCacheRecord[phase];

		// insert the element
		PhasedElementRecord phasedElementRecord(phaseElement);
		elementPhaseRecord.push_back(phasedElementRecord);
	}
}

void IncrementalUpdateBehavior::IncrementalUpdater::UncachePhaseElement(FrameworkElement^ phaseElement, int phase)
{
	if (phase <= 0)
	{
		return;
	}

	UIElement^ contentTemplateRoot = IncrementalUpdateBehavior::IncrementalUpdater::FindContentTemplateRoot(phaseElement);
	if (contentTemplateRoot != nullptr)
	{
		// get the cache for this element
		auto& elementCacheRecord = _elementCache[contentTemplateRoot];

		// get the cache for this phase
		auto& elementPhaseRecord = elementCacheRecord[phase];

		// remove the element
		for (auto it = elementPhaseRecord.begin(); it < elementPhaseRecord.end(); it++)
		{
			if (it->_frameworkElement == phaseElement)
			{
				elementPhaseRecord.erase(it);
				break;
			}
		}
	}
}

void IncrementalUpdateBehavior::IncrementalUpdater::OnContainerContentChanging(Windows::UI::Xaml::Controls::ListViewBase^ sender, ContainerContentChangingEventArgs^ e)
{
	UIElement^ contentTemplateRoot = e->ItemContainer->ContentTemplateRoot;

	if (_elementCache.find(contentTemplateRoot) == _elementCache.end())
	{
		return;
	}

	auto& elementCacheRecord = _elementCache[contentTemplateRoot];

	if (!e->InRecycleQueue)
	{
		for (auto& phaseRecordPair : elementCacheRecord)
		{
			for (auto& phaseElementRecord : phaseRecordPair.second)
			{
				phaseElementRecord.FreezeAndHide();
			}
		}

		if (elementCacheRecord.size() > 0)
		{
			e->RegisterUpdateCallback(elementCacheRecord.cbegin()->first, _cccCallbackHandler);
		}

		// set the DataContext manually since we inhibit default operation by setting e.Handled=true
		FrameworkElement^ frameworkElement = dynamic_cast<FrameworkElement^>(contentTemplateRoot);
		if (frameworkElement != nullptr)
		{
			frameworkElement->DataContext = e->Item;
		}
	}
	else
	{
		// clear the DataContext manually since we inhibit default operation by setting e.Handled=true
		contentTemplateRoot->ClearValue(FrameworkElement::DataContextProperty);

		for (auto& phaseRecordPair : elementCacheRecord)
		{
			for (auto& phaseElementRecord : phaseRecordPair.second)
			{
				phaseElementRecord.ThawAndShow();
			}
		}
	}

	e->Handled = true;
}

void IncrementalUpdateBehavior::IncrementalUpdater::OnContainerContentChangingCallback(Windows::UI::Xaml::Controls::ListViewBase^ sender, ContainerContentChangingEventArgs^ e)
{
	map<int, vector<PhasedElementRecord>> *elementCacheRecord = &_elementCache[e->ItemContainer->ContentTemplateRoot];

	auto it = elementCacheRecord->find(e->Phase);

	for (auto& phaseElementRecord : it->second)
	{
		phaseElementRecord.ThawAndShow();
	}

	it++;
	if (it != elementCacheRecord->end())
	{
		e->RegisterUpdateCallback(it->first, _cccCallbackHandler);
	}
}

void IncrementalUpdateBehavior::IncrementalUpdater::Attach(DependencyObject^ dependencyObject)
{
	_listViewBase = safe_cast<ListViewBase^>(dependencyObject);

	if (_cccHandler == nullptr)
	{
		_cccHandler = ref new TypedEventHandler<ListViewBase^, ContainerContentChangingEventArgs^>(this, &IncrementalUpdateBehavior::IncrementalUpdater::OnContainerContentChanging);
	}

	if (_cccCallbackHandler == nullptr)
	{
		_cccCallbackHandler = ref new TypedEventHandler<ListViewBase^, ContainerContentChangingEventArgs^>(this, &IncrementalUpdateBehavior::IncrementalUpdater::OnContainerContentChangingCallback);
	}

	_containerContentChangingToken = _listViewBase->ContainerContentChanging += _cccHandler;
}

void IncrementalUpdateBehavior::IncrementalUpdater::Detach()
{
	_listViewBase->ContainerContentChanging -= _containerContentChangingToken;
	_listViewBase = nullptr;
}

void IncrementalUpdateBehavior::OnPhaseChanged(DependencyObject^ sender, DependencyPropertyChangedEventArgs^ args)
{
	IncrementalUpdateBehavior^ behavior = safe_cast<IncrementalUpdateBehavior^>(sender);
	IncrementalUpdater^ incrementalUpdater = behavior->FindUpdater();
	FrameworkElement^ frameworkElement = dynamic_cast<FrameworkElement^>(behavior->_associatedObject);

	if (incrementalUpdater != nullptr && frameworkElement != nullptr)
	{
		incrementalUpdater->UncachePhaseElement(frameworkElement, safe_cast<int>(args->OldValue));
		incrementalUpdater->CachePhaseElement(frameworkElement, safe_cast<int>(args->NewValue));
	}
}

DependencyProperty^ IncrementalUpdateBehavior::PhaseProperty::get()
{
	return DependencyProperties::Phase;
}

void IncrementalUpdateBehavior::OnIncrementalUpdaterChanged(DependencyObject^ sender, DependencyPropertyChangedEventArgs^ args)
{
	if (args->OldValue != nullptr)
	{
		IncrementalUpdateBehavior::IncrementalUpdater^ incrementalUpdater = static_cast<IncrementalUpdateBehavior::IncrementalUpdater^>(args->OldValue);
		incrementalUpdater->Detach();
	}
	if (args->NewValue != nullptr)
	{
		IncrementalUpdateBehavior::IncrementalUpdater^ incrementalUpdater = static_cast<IncrementalUpdateBehavior::IncrementalUpdater^>(args->NewValue);
		incrementalUpdater->Attach(sender);
	}
}

DependencyProperty^ IncrementalUpdateBehavior::IncrementalUpdaterProperty::get()
{
	return DependencyProperties::IncrementalUpdater;
}

void IncrementalUpdateBehavior::OnAssociatedObjectLoaded(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
	IncrementalUpdater^ incrementalUpdater = this->FindUpdater();
	FrameworkElement^ frameworkElement = static_cast<FrameworkElement^>(_associatedObject);

	if (incrementalUpdater != nullptr && frameworkElement != nullptr)
	{
		incrementalUpdater->CachePhaseElement(frameworkElement, this->Phase);
	}
}

void IncrementalUpdateBehavior::OnAssociatedObjectUnloaded(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
	FrameworkElement^ frameworkElement = static_cast<FrameworkElement^>(_associatedObject);

	if (_updater != nullptr && frameworkElement != nullptr)
	{
		_updater->UncachePhaseElement(frameworkElement, this->Phase);
	}

	_updater = nullptr;
}

IncrementalUpdateBehavior::IncrementalUpdater^ IncrementalUpdateBehavior::FindUpdater()
{
	if (_updater != nullptr)
	{
		return _updater;
	}

	DependencyObject^ ancestor = _associatedObject;
	while (ancestor != nullptr)
	{
		DependencyObject^ parent = VisualTreeHelper::GetParent(ancestor);
		ListViewBase^ listView = dynamic_cast<ListViewBase^>(parent);

		if (listView != nullptr)
		{
			IncrementalUpdater^ currentUpdater = IncrementalUpdateBehavior::GetIncrementalUpdater(listView);
			if (currentUpdater == nullptr)
			{
				currentUpdater = ref new IncrementalUpdater();

				IncrementalUpdateBehavior::SetIncrementalUpdater(listView, currentUpdater);
			}
			return currentUpdater;
		}
		ancestor = parent;
	}

	return nullptr;
}

void IncrementalUpdateBehavior::Attach(DependencyObject^ associatedObject)
{
	if (associatedObject == _associatedObject)
	{
		return;
	}

	if (Windows::ApplicationModel::DesignMode::DesignModeEnabled)
	{
		return;
	}

	if (_associatedObject != nullptr)
	{
		throw ref new FailureException(ResourceHelper::GetString("CannotAttachBehaviorMultipleTimesExceptionMessage"));
	}

	_associatedObject = associatedObject;

	FrameworkElement^ frameworkElement = dynamic_cast<FrameworkElement^>(associatedObject);
	if (frameworkElement != nullptr)
	{
		_loadedToken = frameworkElement->Loaded += ref new Windows::UI::Xaml::RoutedEventHandler(this, &IncrementalUpdateBehavior::OnAssociatedObjectLoaded);
		_unloadedToken = frameworkElement->Unloaded += ref new Windows::UI::Xaml::RoutedEventHandler(this, &IncrementalUpdateBehavior::OnAssociatedObjectUnloaded);
	}
}

void IncrementalUpdateBehavior::Detach()
{
	FrameworkElement^ frameworkElement = dynamic_cast<FrameworkElement^>(_associatedObject);
	if (frameworkElement != nullptr)
	{
		frameworkElement->Loaded -= _loadedToken;
		frameworkElement->Unloaded -= _unloadedToken;
		// no need to perform the work that Unloaded would have done - that's just housekeeping on the cache, which is now going away
	}

	_associatedObject = nullptr;
}
