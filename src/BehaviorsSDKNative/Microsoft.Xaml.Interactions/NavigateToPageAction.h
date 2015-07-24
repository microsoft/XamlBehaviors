// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
#pragma once

namespace Microsoft { namespace Xaml { namespace Interactions { namespace Core
{
	/// <summary>
	/// An action that switches the current visual to the specified <seealso cref="Windows::UI::Xaml::Controls::Page"/>.
	/// </summary>
	[::Windows::UI::Xaml::Data::Bindable]
	[::Windows::Foundation::Metadata::WebHostHidden]
	public ref class NavigateToPageAction sealed : ::Windows::UI::Xaml::DependencyObject, ::Microsoft::Xaml::Interactivity::IAction
	{
	public:
		/// <summary>
		/// Identifies the <see cref="TargetPage"/> dependency property.
		/// </summary>
		static property ::Windows::UI::Xaml::DependencyProperty^ TargetPageProperty
		{
			::Windows::UI::Xaml::DependencyProperty^ get();
		}

		/// <summary>
		/// Identifies the <see cref="Parameter"/> dependency property.
		/// </summary>
		static property ::Windows::UI::Xaml::DependencyProperty^ ParameterProperty
		{
			::Windows::UI::Xaml::DependencyProperty^ get();
		}

		/// <summary>
		/// Gets or sets the parameter which will be passed to the Windows::UI::Xaml::Controls::Frame::Navigate(Windows::UI::Xaml::Interop::TypeName, Platfrom::Object^) method.
		/// </summary>
		property ::Platform::Object^ Parameter
		{
			::Platform::Object^ get()
			{
				return safe_cast<::Platform::Object^>(this->GetValue(NavigateToPageAction::ParameterProperty));
			}

			void set(::Platform::Object^ value)
			{
				this->SetValue(NavigateToPageAction::ParameterProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the fully qualified name of the <seealso cref="Windows::UI::Xaml::Controls::Page"/> to navigate to. This is a dependency property.
		/// </summary>
		property::Platform::String^ TargetPage
		{
			::Platform::String^ get()
			{
				return safe_cast<::Platform::String^>(this->GetValue(NavigateToPageAction::TargetPageProperty));
			}

			void set(::Platform::String^ value)
			{
				this->SetValue(NavigateToPageAction::TargetPageProperty, value);
			}
		}

		/// <summary>
		/// Executes the action.
		/// </summary>
		/// <param name="sender">The <see cref="Platform::Object"/> that is passed to the action by the behavior. Generally this is <seealso cref="Microsoft::Xaml::Interactivity::IBehavior::AssociatedObject"/> or a target object.</param>
		/// <param name="parameter">The value of this parameter is determined by the caller.</param>
		/// <returns>True if the navigation to the specified page is successful; else false.</returns>
		virtual ::Platform::Object^ Execute(::Platform::Object^ sender, ::Platform::Object^ parameter);
	};

}}}}