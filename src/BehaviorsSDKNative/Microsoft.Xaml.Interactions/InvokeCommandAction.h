// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
#pragma once

namespace Microsoft { namespace Xaml { namespace Interactions { namespace Core
{
	/// <summary>
	/// Executes a specified <seealso cref="Windows::UI::Xaml::Input::ICommand"/> when invoked.
	/// </summary>
	[::Windows::UI::Xaml::Data::Bindable]
	[::Windows::Foundation::Metadata::WebHostHidden]
	public ref class InvokeCommandAction sealed : ::Windows::UI::Xaml::DependencyObject, ::Microsoft::Xaml::Interactivity::IAction
	{
	public:
		/// <summary>
		/// Identifies the <see cref="Command"/> dependency property.
		/// </summary>
		static property ::Windows::UI::Xaml::DependencyProperty^ CommandProperty
		{
			::Windows::UI::Xaml::DependencyProperty^ get();
		}

		/// <summary>
		/// Identifies the <see cref="CommandParameter"/> dependency property.
		/// </summary>
		static property ::Windows::UI::Xaml::DependencyProperty^ CommandParameterProperty
		{
			::Windows::UI::Xaml::DependencyProperty^ get();
		}

		/// <summary>
		/// Identifies the <see cref="InputConverter"/> dependency property.
		/// </summary>
		static property::Windows::UI::Xaml::DependencyProperty^ InputConverterProperty
		{
			::Windows::UI::Xaml::DependencyProperty^ get();
		}

		/// <summary>
		/// Identifies the <see cref="InputConverterParameter"/> dependency property.
		/// </summary>
		static property::Windows::UI::Xaml::DependencyProperty^ InputConverterParameterProperty
		{
			::Windows::UI::Xaml::DependencyProperty^ get();
		}

		/// <summary>
		/// Identifies the <see cref="InputConverterLanguage"/> dependency property.
		/// </summary>
		static property::Windows::UI::Xaml::DependencyProperty^ InputConverterLanguageProperty
		{
			::Windows::UI::Xaml::DependencyProperty^ get();
		}

		/// <summary>
		/// Gets or sets the command this action should invoke. This is a dependency property.
		/// </summary>
		property ::Windows::UI::Xaml::Input::ICommand^ Command
		{
			::Windows::UI::Xaml::Input::ICommand^ get()
			{
				return safe_cast<::Windows::UI::Xaml::Input::ICommand^>(this->GetValue(InvokeCommandAction::CommandProperty));
			}

			void set(::Windows::UI::Xaml::Input::ICommand^ value)
			{
				this->SetValue(InvokeCommandAction::CommandProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the parameter that is passed to <see cref="Windows::UI::Xaml::Input::ICommand::Execute"/>.
		/// If this is not set, the parameter from the <see cref="Execute(sender, parameter)"/> method will be used.
		/// This is an optional dependency property.
		/// </summary>
		property ::Platform::Object^ CommandParameter
		{
			::Platform::Object^ get()
			{
				return safe_cast<::Platform::Object^>(this->GetValue(InvokeCommandAction::CommandParameterProperty));
			}

			void set(::Platform::Object^ value)
			{
				this->SetValue(InvokeCommandAction::CommandParameterProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the converter that is run on the parameter from the <see cref="Execute(sender, parameter)"/> method.
		/// This is an optional dependency property.
		/// </summary>
		property ::Windows::UI::Xaml::Data::IValueConverter^ InputConverter
		{
			::Windows::UI::Xaml::Data::IValueConverter^ get()
			{
				return safe_cast<::Windows::UI::Xaml::Data::IValueConverter^>(this->GetValue(InvokeCommandAction::InputConverterProperty));
			}

			void set(::Windows::UI::Xaml::Data::IValueConverter^ value)
			{
				this->SetValue(InvokeCommandAction::InputConverterProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the parameter that is passed to the <see cref="Windows::UI::Xaml::Data::IValueConverter::Convert"/>
		/// method of <see cref="InputConverter"/>.
		/// This is an optional dependency property.
		/// </summary>
		property ::Platform::Object^ InputConverterParameter
		{
			::Platform::Object^ get()
			{
				return safe_cast<::Platform::Object^>(this->GetValue(InvokeCommandAction::InputConverterParameterProperty));
			}

			void set(::Platform::Object^ value)
			{
				this->SetValue(InvokeCommandAction::InputConverterParameterProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the language that is passed to the <see cref="Windows::UI::Xaml::Data::IValueConverter::Convert"/>
		/// method of <see cref="InputConverter"/>.
		/// This is an optional dependency property.
		/// </summary>
		property ::Platform::String^ InputConverterLanguage
		{
			::Platform::String^ get()
			{
				return safe_cast<::Platform::String^>(this->GetValue(InvokeCommandAction::InputConverterLanguageProperty));
			}

			void set(::Platform::String^ value)
			{
				this->SetValue(InvokeCommandAction::InputConverterLanguageProperty, value);
			}
		}

		/// <summary>
		/// Executes the action.
		/// </summary>
		/// <param name="sender">The <see cref="Platform::Object"/> that is passed to the action by the behavior. Generally this is <seealso cref="Microsoft::Xaml::Interactivity::IBehavior::AssociatedObject"/> or a target object.</param>
		/// <param name="parameter">The value of this parameter is determined by the caller.</param>
		/// <returns>True if the command is successfully executed; else false.</returns>
		virtual ::Platform::Object^ Execute(::Platform::Object^ sender, ::Platform::Object^ parameter);
	};

}}}}