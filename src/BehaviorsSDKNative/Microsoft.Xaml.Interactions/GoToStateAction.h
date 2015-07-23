// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
#pragma once

namespace Microsoft { namespace Xaml { namespace Interactions { namespace Core
{
	/// <summary>
	/// An action that will transition a <see cref="Windows::UI::Xaml::FrameworkElement"/> to a specified <seealso cref="Windows::UI::Xaml::VisualState"/> when executed.
	/// </summary>
	/// <remarks>
	/// If the <see cref="TargetObject"/> property is set, this action will attempt to change the state of the targeted element. If it is not set, the action walks
	/// the element tree in an attempt to locate an alternative target that defines states. <see cref="Windows::UI::Xaml::Controls::ControlTemplate"/> and <see cref="Windows::UI::Xaml::Controls::UserControl"/> are 
	/// two common results.
	/// </remarks>
	[::Windows::UI::Xaml::Data::Bindable]
	[::Windows::Foundation::Metadata::WebHostHidden]
	public ref class GoToStateAction sealed : ::Windows::UI::Xaml::DependencyObject, ::Microsoft::Xaml::Interactivity::IAction
	{
	public:
		/// <summary>
		/// Identifies the <see cref="TargetObject"/> dependency property.
		/// </summary>
		static property ::Windows::UI::Xaml::DependencyProperty^ TargetObjectProperty
		{
			::Windows::UI::Xaml::DependencyProperty^ get();
		}

		/// <summary>
		/// Identifies the <see cref="StateName"/> dependency property.
		/// </summary>
		static property ::Windows::UI::Xaml::DependencyProperty^ StateNameProperty
		{
			::Windows::UI::Xaml::DependencyProperty^ get();
		}

		/// <summary>
		/// Identifies the <see cref="UseTransitions"/> dependency property.
		/// </summary>
		static property ::Windows::UI::Xaml::DependencyProperty^ UseTransitionsProperty
		{
			::Windows::UI::Xaml::DependencyProperty^ get();
		}

		/// <summary>
		/// Gets or sets the target object. This is a dependency property.
		/// </summary>
		property ::Windows::UI::Xaml::FrameworkElement^ TargetObject
		{
			::Windows::UI::Xaml::FrameworkElement^ get()
			{
				return safe_cast<::Windows::UI::Xaml::FrameworkElement^>(this->GetValue(GoToStateAction::TargetObjectProperty));
			}

			void set(::Windows::UI::Xaml::FrameworkElement^ value)
			{
				this->SetValue(GoToStateAction::TargetObjectProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the name of the <seealso cref="Windows::UI::Xaml::VisualState"/>. This is a dependency property.
		/// </summary>
		property ::Platform::String^ StateName
		{
			::Platform::String^ get()
			{
				return safe_cast<::Platform::String^>(this->GetValue(GoToStateAction::StateNameProperty));
			}

			void set(::Platform::String^ value)
			{
				this->SetValue(GoToStateAction::StateNameProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets whether or not to use a <seealso cref="Windows::UI::Xaml::VisualTransition"/> to transition between states. This is a dependency property.
		/// </summary>
		property bool UseTransitions
		{
			bool get()
			{
				return safe_cast<bool>(this->GetValue(GoToStateAction::UseTransitionsProperty));
			}

			void set(bool value)
			{
				this->SetValue(GoToStateAction::UseTransitionsProperty, value);
			}
		}

		/// <summary>
		/// Executes the action.
		/// </summary>
		/// <param name="sender">The <see cref="Platform::Object"/> that is passed to the action by the behavior. Generally this is <seealso cref="Microsoft::Xaml::Interactivity::IBehavior::AssociatedObject"/> or a target object.</param>
		/// <param name="parameter">The value of this parameter is determined by the caller.</param>
		/// <returns>True if the transition to the specified state succeeds; else false.</returns>
		virtual ::Platform::Object^ Execute(::Platform::Object^ sender, ::Platform::Object^ parameter);
	};

}}}}