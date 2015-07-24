// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
#pragma once

namespace Microsoft { namespace Xaml { namespace Interactions { namespace Core
{
	private delegate void Action(::Platform::Object^ eventArgs);

	// A method that when given an arbitrary object and action, hooks an event handler to the object
	// such that when the event is fired, the action is invoked.
	typedef ::Windows::Foundation::EventRegistrationToken(*RegisterHandler)(::Platform::Object^, Action^);

	typedef void (*UnregisterHandler)(::Platform::Object^, ::Windows::Foundation::EventRegistrationToken);

	// A static class that maps event names to delegates which are able to attach to that event when
	// given a object and a method to invoke as the result of event firing. Currently we assume that
	// event names are unique.
	class EventManager sealed
	{
	public:
		static void AddDecription(
			const wchar_t* eventName,
			RegisterHandler handler,
			UnregisterHandler unregisterHandler);

		static ::Windows::Foundation::EventRegistrationToken Register(
			::Platform::String^ eventName,
			::Platform::Object^ sender,
			Action^ action);

		static void Unregister(
			::Platform::String^ eventName,
			::Platform::Object^ sender,
			::Windows::Foundation::EventRegistrationToken token);

	private:
		static void AddDefaultEvents();
	};

}}}}