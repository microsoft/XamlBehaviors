// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
#pragma once
#include "BehaviorCollection.h"
#include "ActionCollection.h"

namespace Microsoft { namespace Xaml { namespace Interactivity
{
	/// <summary>
	/// Defines a <see cref="BehaviorCollection"/> attached property and provides a method for executing an <seealso cref="ActionCollection"/>.
	/// </summary>
	[::Windows::UI::Xaml::Data::Bindable]
	[::Windows::Foundation::Metadata::WebHostHidden]
	public ref class Interaction sealed
	{
	private:
		Interaction() {};

	public:
		/// <summary>
		/// Gets or sets the <see cref="BehaviorCollection"/> associated with a specified object.
		/// </summary>
		static property ::Windows::UI::Xaml::DependencyProperty^ BehaviorsProperty
		{
			::Windows::UI::Xaml::DependencyProperty^ get();
		}

		/// <summary>
		/// Gets the <see cref="BehaviorCollection"/> associated with a specified object.
		/// </summary>
		/// <param name="obj">The <see cref="Windows::UI::Xaml::DependencyObject"/> from which to retrieve the <see cref="BehaviorCollection"/>.</param>
		/// <returns>A <see cref="BehaviorCollection"/> containing the behaviors associated with the specified object.</returns>
		static BehaviorCollection^ GetBehaviors(::Windows::UI::Xaml::DependencyObject^ obj);

		/// <summary>
		/// Sets the <see cref="BehaviorCollection"/> associated with a specified object.
		/// </summary>
		/// <param name="obj">The <see cref="Windows::UI::Xaml::DependencyObject"/> on which to set the <see cref="BehaviorCollection"/>.</param>
		/// <param name="value">The <see cref="BehaviorCollection"/> associated with the object.</param>
		static void SetBehaviors(::Windows::UI::Xaml::DependencyObject^ obj, BehaviorCollection^ value);

		/// <summary>
		/// Executes all actions in the <see cref="ActionCollection"/> and returns their results.
		/// </summary>
		/// <param name="sender">The <see cref="Platform::Object"/> which will be passed on to the action.</param>
		/// <param name="actions">The set of actions to execute.</param>
		/// <param name="parameter">The value of this parameter is determined by the calling behavior.</param>
		/// <returns>Returns the results of the actions.</returns>
		static ::Windows::Foundation::Collections::IIterable<::Platform::Object^>^ ExecuteActions(::Platform::Object^ sender, ActionCollection^ actions, ::Platform::Object^ parameter);

	internal:
		static void OnBehaviorsChanged(::Windows::UI::Xaml::DependencyObject^ sender, ::Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ args);
	};

}}}