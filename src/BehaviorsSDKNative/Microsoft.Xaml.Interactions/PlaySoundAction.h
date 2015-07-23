// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
#pragma once

namespace Microsoft { namespace Xaml { namespace Interactions { namespace Media
{
	/// <summary>
	/// An action that will play a sound to completion.
	/// </summary>
	/// <remarks>
	/// This action is intended for use with short sound effects that don't need to be stopped or controlled. If you are trying 
	/// to create a music player or game, it may not meet your needs.
	/// </remarks>
	[::Windows::UI::Xaml::Data::Bindable]
	[::Windows::Foundation::Metadata::WebHostHidden]
	public ref class PlaySoundAction sealed : ::Windows::UI::Xaml::DependencyObject, ::Microsoft::Xaml::Interactivity::IAction
	{
	public:
		/// <summary>
		/// Identifies the <see cref="Source"/> dependency property.
		/// </summary>
		static property::Windows::UI::Xaml::DependencyProperty^ SourceProperty
		{
			::Windows::UI::Xaml::DependencyProperty^ get();
		}

		/// <summary>
		/// Identifies the <see cref="Volume"/> dependency property.
		/// </summary>
		static property::Windows::UI::Xaml::DependencyProperty^ VolumeProperty
		{
			::Windows::UI::Xaml::DependencyProperty^ get();
		}

		/// <summary>
		/// Gets or sets the location of the sound file. This is used to set the source property of a <see cref="Windows::UI::Xaml::Controls::MediaElement"/>. This is a dependency property.
		/// </summary>
		/// <remarks>
		/// The sound can be any file format supported by <see cref="Windows::UI::Xaml::Controls::MediaElement"/>. In the case of a video, it will play only the
		/// audio portion.
		/// </remarks>
		property ::Platform::String^ Source
		{
			::Platform::String^ get()
			{
				return safe_cast<::Platform::String^>(this->GetValue(PlaySoundAction::SourceProperty));
			}

			void set(::Platform::String^ value)
			{
				this->SetValue(PlaySoundAction::SourceProperty, value);
			}
		}

		/// <summary>
		/// Gets or set the volume of the sound. This is used to set the <see cref="Windows::UI::Xaml::Controls::MediaElement::Volume"/> property of the <see cref="Windows::UI::Xaml::Controls::MediaElement"/>. This is a dependency property.
		/// </summary>
		/// <remarks>
		/// By default this is set to 0.5.
		/// </remarks>
		property double Volume
		{
			double get()
			{
				return static_cast<double>(this->GetValue(PlaySoundAction::VolumeProperty));
			}

			void set(double value)
			{
				this->SetValue(PlaySoundAction::VolumeProperty, value);
			}
		}

		/// <summary>
		/// Executes the action.
		/// </summary>
		/// <param name="sender">The <see cref="Platform::Object"/> that is passed to the action by the behavior. Generally this is <seealso cref="Microsoft::Xaml::Interactivity::IBehavior::AssociatedObject"/> or a target object.</param>
		/// <param name="parameter">The value of this parameter is determined by the caller.</param>
		/// <returns>True if <see cref="Windows::UI::Xaml::Controls::MediaElement::Source"/> is set successfully; else false.</returns>
		virtual ::Platform::Object^ Execute(::Platform::Object^ sender, ::Platform::Object^ parameter);

	private:
		Windows::UI::Xaml::Controls::Primitives::Popup^ popup;

		void OnMediaEnded(::Platform::Object^ sender, ::Windows::UI::Xaml::RoutedEventArgs ^e);
		void OnMediaFailed(::Platform::Object ^sender, ::Windows::UI::Xaml::ExceptionRoutedEventArgs ^e);
	};

}}}}