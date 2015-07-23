// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
#pragma once
#include "EventManager.h"

namespace Microsoft { namespace Xaml { namespace Interactions { namespace Core
{
	/// <summary>
	/// A behavior that listens for a specified event on its source and executes its actions when that event is fired.
	/// </summary>
	/// <remarks>
	/// The following events are supported:
	/// <see cref="Windows::UI::Xaml::UIElement::Tapped"/>
	/// <see cref="Windows::UI::Xaml::UIElement::PointerPressed"/>
	/// <see cref="Windows::UI::Xaml::FrameworkElement::Loaded"/>
	/// <see cref="Windows::UI::Xaml::FrameworkElement::DataContextChanged"/>
	/// <see cref="Windows::UI::Xaml::Controls::Primitives::ButtonBase::Click"/>
	/// <see cref="Windows::UI::Xaml::Controls::Primitives::ToggleButton::Checked"/>
	/// <see cref="Windows::UI::Xaml::Controls::Primitives::ToggleButton::Unchecked"/>
	/// <see cref="Windows::UI::Xaml::Controls::Primitives::Selector::SelectionChanged"/>
	/// <see cref="Windows::UI::Xaml::Controls::TextBox::TextChanged"/>
	/// <see cref="Windows::UI::Xaml::Controls::ToggleSwitch::Toggled"/>
	/// <see cref="Windows::UI::Xaml::Controls::WebView::NavigationCompleted"/>
	/// Consider implementing a custom behavior to respond to other events.
	/// </remarks>
	[::Windows::UI::Xaml::Data::Bindable]
	[::Windows::Foundation::Metadata::WebHostHidden]
	[::Windows::UI::Xaml::Markup::ContentPropertyAttribute(Name = "Actions")]
	public ref class EventTriggerBehavior sealed : ::Windows::UI::Xaml::DependencyObject, ::Microsoft::Xaml::Interactivity::IBehavior
	{
	private:
		::Windows::UI::Xaml::DependencyObject^ associatedObject;
		void InvokeActions(::Platform::Object^ eventArgs);

		// Resolved source
		::Platform::Object^ resolvedSource;
		::Platform::Object^ ComputeResolvedSource();
		void SetResolvedSource(::Platform::Object^ newSource);

		// Event registration
		::Windows::Foundation::EventRegistrationToken registeredToken;
		void RegisterEvent(::Platform::String^ eventName);
		void UnregisterEvent(::Platform::String^ eventName);

		// Loaded registration
		bool isLoadedRegistered;
		::Windows::Foundation::EventRegistrationToken loadedToken;
		void OnLoaded(::Platform::Object^ sender, ::Windows::UI::Xaml::RoutedEventArgs^ e);

	internal:
		// Dependency property change handlers
		static void OnEventNameChanged(::Windows::UI::Xaml::DependencyObject^ sender, ::Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ args);
		static void OnSourceObjectChanged(::Windows::UI::Xaml::DependencyObject^ sender, ::Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ args);
		static bool IsElementLoaded(::Windows::UI::Xaml::FrameworkElement^ frameworkElement);

	public:
		/// <summary>
		/// Identifies the <see cref="SourceObject"/> dependency property.
		/// </summary>
		static property ::Windows::UI::Xaml::DependencyProperty^ SourceObjectProperty
		{
			::Windows::UI::Xaml::DependencyProperty^ get();
		}

		/// <summary>
		/// Identifies the <see cref="EventName"/> dependency property.
		/// </summary>
		static property ::Windows::UI::Xaml::DependencyProperty^ EventNameProperty
		{
			::Windows::UI::Xaml::DependencyProperty^ get();
		}

		/// <summary>
		/// Identifies the <see cref="Actions"/> dependency property.
		/// </summary>
		static property ::Windows::UI::Xaml::DependencyProperty^ ActionsProperty
		{
			::Windows::UI::Xaml::DependencyProperty^ get();
		}

		/// <summary>
		/// Gets the <see cref="Windows::UI::Xaml::DependencyObject"/> to which the <seealso cref="::Microsoft::Xaml::Interactivity::IBehavior"/> is attached.
		/// </summary>
		virtual property ::Windows::UI::Xaml::DependencyObject^ AssociatedObject
		{
			::Windows::UI::Xaml::DependencyObject^ get()
			{
				return this->associatedObject;
			}
		}

		/// <summary>
		/// Gets or sets the source object from which this behavior listens for events.
		/// If <see cref="SourceObject"/> is not set, the source will default to <see cref="AssociatedObject"/>. This is a dependency property.
		/// </summary>
		property ::Platform::Object^ SourceObject
		{
			::Platform::Object^ get()
			{
				return safe_cast<::Platform::Object^>(this->GetValue(EventTriggerBehavior::SourceObjectProperty));
			}

			void set(::Platform::Object^ value)
			{
				this->SetValue(EventTriggerBehavior::SourceObjectProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the name of the event to listen for. This is a dependency property.
		/// </summary>
		property::Platform::String^ EventName
		{
			::Platform::String^ get()
			{
				return safe_cast<::Platform::String^>(this->GetValue(EventTriggerBehavior::EventNameProperty));
			}

			void set(::Platform::String^ value)
			{
				this->SetValue(EventTriggerBehavior::EventNameProperty, value);
			}
		}

		/// <summary>
		/// Gets the collection of actions associated with the behavior. This is a dependency property.
		/// </summary>
		property ::Microsoft::Xaml::Interactivity::ActionCollection^ Actions
		{
			::Microsoft::Xaml::Interactivity::ActionCollection^ get()
			{
				::Microsoft::Xaml::Interactivity::ActionCollection^ actionCollection = safe_cast<::Microsoft::Xaml::Interactivity::ActionCollection^>(this->GetValue(EventTriggerBehavior::ActionsProperty));
				if (actionCollection == nullptr)
				{
					actionCollection = ref new ::Microsoft::Xaml::Interactivity::ActionCollection();
					this->SetValue(EventTriggerBehavior::ActionsProperty, actionCollection);
				}

				return actionCollection;
			}
		}

		/// <summary>
		/// Attaches to the specified object.
		/// </summary>
		/// <param name="associatedObject">The <see cref="Windows::UI::Xaml::DependencyObject"/> to which the <seealso cref="::Microsoft::Xaml::Interactivity::IBehavior"/> will be attached.</param>
		virtual void Attach(::Windows::UI::Xaml::DependencyObject^ associatedObject);

		/// <summary>
		/// Detaches this instance from its associated object.
		/// </summary>
		virtual void Detach();
	};

}}}}