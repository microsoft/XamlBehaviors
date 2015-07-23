// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
#pragma once
#include "ControlStoryboardOption.h"

namespace Microsoft { namespace Xaml { namespace Interactions { namespace Media
{
	/// <summary>
	/// An action that will change the state of the specified <seealso cref="Windows::UI::Xaml::Media::Animation::Storyboard"/> when executed.
	/// </summary>
	[::Windows::UI::Xaml::Data::Bindable]
	[::Windows::Foundation::Metadata::WebHostHidden]
	public ref class ControlStoryboardAction sealed : ::Windows::UI::Xaml::DependencyObject, ::Microsoft::Xaml::Interactivity::IAction
	{
	private:
		bool isPaused;

	internal:
		static void OnStoryboardChanged(::Windows::UI::Xaml::DependencyObject^ sender, ::Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ args);

	public:
		/// <summary>
		/// Identifies the <see cref="Storyboard"/> dependency property.
		/// </summary>
		static property::Windows::UI::Xaml::DependencyProperty^ StoryboardProperty
		{
			::Windows::UI::Xaml::DependencyProperty^ get();
		}

		/// <summary>
		/// Identifies the <see cref="ControlStoryboardOption"/> dependency property.
		/// </summary>
		static property::Windows::UI::Xaml::DependencyProperty^ ControlStoryboardOptionProperty
		{
			::Windows::UI::Xaml::DependencyProperty^ get();
		}

		/// <summary>
		/// Gets or sets the targeted <seealso cref="Windows::UI::Xaml::Media::Animation::Storyboard"/>. This is a dependency property.
		/// </summary>
		property ::Windows::UI::Xaml::Media::Animation::Storyboard^ Storyboard
		{
			::Windows::UI::Xaml::Media::Animation::Storyboard^ get()
			{
				return safe_cast<::Windows::UI::Xaml::Media::Animation::Storyboard^>(this->GetValue(ControlStoryboardAction::StoryboardProperty));
			}

			void set(::Windows::UI::Xaml::Media::Animation::Storyboard^ value)
			{
				this->SetValue(ControlStoryboardAction::StoryboardProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the action to execute on the <seealso cref="Windows::UI::Xaml::Media::Animation::Storyboard"/>. This is a dependency property.
		/// </summary>
		property Media::ControlStoryboardOption ControlStoryboardOption
		{
			Media::ControlStoryboardOption get()
			{
				return static_cast<Media::ControlStoryboardOption>(this->GetValue(ControlStoryboardAction::ControlStoryboardOptionProperty));
			}

			void set(Media::ControlStoryboardOption value)
			{
				this->SetValue(ControlStoryboardAction::ControlStoryboardOptionProperty, value);
			}
		}

		/// <summary>
		/// Executes the action.
		/// </summary>
		/// <param name="sender">The <see cref="Platform::Object"/> that is passed to the action by the behavior. Generally this is <seealso cref="Microsoft::Xaml::Interactivity::IBehavior::AssociatedObject"/> or a target object.</param>
		/// <param name="parameter">The value of this parameter is determined by the caller.</param>
		/// <returns>True if the specified operation is invoked successfully; else false.</returns>
		virtual ::Platform::Object^ Execute(::Platform::Object^ sender, ::Platform::Object^ parameter);
	};

}}}}