// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
#include "pch.h"
#include "EventManager.h"
#include "ResourceHelper.h"
#include <unordered_map>
#include <algorithm>

#define UNREGISTER_EVENT(event_name, type) \
	[](Object^ sender, EventRegistrationToken token) { safe_cast<type^>(sender)->event_name -= token; }

#define ADD_DESCRIPTION(event_name, type, handler_type, args_type)																	\
	EventManager::AddDecription(L#event_name, [](Object^ sender, Action^ action)													\
		{ return safe_cast<type^>(sender)->event_name += ref new handler_type([action](Object^ s, args_type^ e) { action(e); }); }, \
		UNREGISTER_EVENT(event_name, type))

#define ADD_TYPED_DESCRIPTION(event_name, type, args_type)																									\
	EventManager::AddDecription(L#event_name, [](Object^ sender, Action^ action)																			\
		{ return safe_cast<type^>(sender)->event_name += ref new TypedEventHandler<type^, args_type^>([action](type^ s, args_type^ e) { action(e); }); },	\
		UNREGISTER_EVENT(event_name, type))

using namespace Platform;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Input;
using namespace Windows::UI::Xaml::Controls;
using namespace Microsoft::Xaml::Interactions::Core;
using namespace Windows::UI::Xaml::Controls::Primitives;

struct CompareStringX
{
	bool operator() (const wchar_t* lhs, const wchar_t* rhs) const
	{
		return wcscmp(lhs, rhs) < 0;
	}
};

typedef std::map<const wchar_t*, RegisterHandler, CompareStringX> EventNameToRegisterHandlerMap;
typedef std::map<const wchar_t*, UnregisterHandler, CompareStringX> EventNameToUnregisterHandlerMap;

namespace Global
{
	static EventNameToRegisterHandlerMap RegisterHandlers;
	static EventNameToUnregisterHandlerMap UnregisterHandlers;
}

void EventManager::AddDecription(const wchar_t* eventName, RegisterHandler registerHandler, UnregisterHandler unregisterHandler)
{
	Global::RegisterHandlers.insert(EventNameToRegisterHandlerMap::value_type(eventName, registerHandler));
	Global::UnregisterHandlers.insert(EventNameToUnregisterHandlerMap::value_type(eventName, unregisterHandler));
}

EventRegistrationToken EventManager::Register(String^ eventName, Object^ sender, Action^ action)
{
	if (Global::RegisterHandlers.empty())
	{
		EventManager::AddDefaultEvents();
	}

	EventNameToRegisterHandlerMap::const_iterator iterator = Global::RegisterHandlers.find(eventName->Data());
	if (iterator != Global::RegisterHandlers.end())
	{
		RegisterHandler handler = iterator->second;
		return handler(sender, action);
	}
	else
	{
		throw ref new FailureException(ResourceHelper::GetString("EventNotSupportedExceptionMessage", eventName));
	}
}

void EventManager::Unregister(String^ eventName, Object^ sender, EventRegistrationToken token)
{
	if (Global::UnregisterHandlers.empty())
	{
		EventManager::AddDefaultEvents();
	}

	EventNameToUnregisterHandlerMap::const_iterator iterator = Global::UnregisterHandlers.find(eventName->Data());
	if (iterator != Global::UnregisterHandlers.end())
	{
		UnregisterHandler handler = iterator->second;
		handler(sender, token);
	}
	else
	{
		throw ref new FailureException(ResourceHelper::GetString("EventNotSupportedExceptionMessage", eventName));
	}
}

void EventManager::AddDefaultEvents()
{
	// When adding to this list, also add the full event name to EventPickerEditor.xaml.cs in
	// order to get designtime support.
	ADD_DESCRIPTION(PointerPressed, UIElement, PointerEventHandler, PointerRoutedEventArgs);
	ADD_DESCRIPTION(PointerReleased, UIElement, PointerEventHandler, PointerRoutedEventArgs);
	ADD_DESCRIPTION(Tapped, UIElement, TappedEventHandler, TappedRoutedEventArgs);
	ADD_DESCRIPTION(Click, ButtonBase, RoutedEventHandler, RoutedEventArgs);
	ADD_DESCRIPTION(Checked, ToggleButton, RoutedEventHandler, RoutedEventArgs);
	ADD_DESCRIPTION(Unchecked, ToggleButton, RoutedEventHandler, RoutedEventArgs);
	ADD_DESCRIPTION(TextChanged, TextBox, TextChangedEventHandler, TextChangedEventArgs);
	ADD_DESCRIPTION(Toggled, ToggleSwitch, RoutedEventHandler, RoutedEventArgs);
	ADD_DESCRIPTION(SelectionChanged, Selector, SelectionChangedEventHandler, SelectionChangedEventArgs);

	ADD_TYPED_DESCRIPTION(NavigationCompleted, WebView, WebViewNavigationCompletedEventArgs);
	ADD_TYPED_DESCRIPTION(DataContextChanged, FrameworkElement, DataContextChangedEventArgs);
}
