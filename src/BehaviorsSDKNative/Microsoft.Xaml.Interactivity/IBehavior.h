// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
#pragma once

namespace Microsoft { namespace Xaml { namespace Interactivity
{
	/// <summary>
	/// Interface implemented by all custom behaviors.
	/// </summary>
	[::Windows::Foundation::Metadata::WebHostHidden]
	public interface class IBehavior
	{
		/// <summary>
		/// Gets the <see cref="Windows::UI::Xaml::DependencyObject"/> to which the <seealso cref="IBehavior"/> is attached.
		/// </summary>
		property ::Windows::UI::Xaml::DependencyObject^ AssociatedObject
		{
			::Windows::UI::Xaml::DependencyObject^ get();
		}

		/// <summary>
		/// Attaches to the specified object.
		/// </summary>
		/// <param name="associatedObject">The <see cref="Windows::UI::Xaml::DependencyObject"/> to which the <seealso cref="IBehavior"/> will be attached.</param>
		void Attach(::Windows::UI::Xaml::DependencyObject^ associatedObject);

		/// <summary>
		/// Detaches this instance from its associated object.
		/// </summary>
		void Detach();
	};

}}}