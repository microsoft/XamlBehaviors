// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
#pragma once
#include "IBehavior.h"

namespace Microsoft { namespace Xaml { namespace Interactivity
{
	/// <summary>
	/// Represents a collection of IBehaviors with a shared <see cref="Microsoft::Xaml::Interactivity::BehaviorCollection::AssociatedObject"/>.
	/// </summary>
	[::Windows::Foundation::Metadata::WebHostHidden]
	public ref class BehaviorCollection sealed : ::Windows::UI::Xaml::DependencyObjectCollection
	{
	public:
		/// <summary>
		/// Initializes a new instance of the <see cref="BehaviorCollection"/> class.
		/// </summary>
		BehaviorCollection();

		/// <summary>
		/// Gets the <see cref="Windows::UI::Xaml::DependencyObject"/> to which the <see cref="BehaviorCollection"/> is attached.
		/// </summary>
		property ::Windows::UI::Xaml::DependencyObject^ AssociatedObject
		{
			::Windows::UI::Xaml::DependencyObject^ get()
			{
				return this->associatedObject.Resolve<Windows::UI::Xaml::DependencyObject>();
			}
		}

		/// <summary>
		/// Attaches the collection of behaviors to the specified <see cref="Windows::UI::Xaml::DependencyObject"/>.
		/// </summary>
		/// <param name="associatedObject">The <see cref="Windows::UI::Xaml::DependencyObject"/> to which to attach.</param>
		/// <exception cref="Platform::FailureException">The <see cref="BehaviorCollection"/> is already attached to a different <see cref="Windows::UI::Xaml::DependencyObject"/>.</exception>
		void Attach(::Windows::UI::Xaml::DependencyObject^ associatedObject);

		/// <summary>
		/// Detaches the collection of behaviors from the <see cref="Microsoft::Xaml::Interactivity::BehaviorCollection::AssociatedObject"/>.
		/// </summary>
		void Detach();

	private:
		~BehaviorCollection();

		// After a VectorChanged event we need to compare the current state of the collection
		// with the old collection so that we can call Detach on all removed items.
		std::vector<IBehavior^> oldCollection;
		::Platform::WeakReference associatedObject;

		void OnVectorChanged(
			::Windows::Foundation::Collections::IObservableVector<::Windows::UI::Xaml::DependencyObject^>^ sender,
			::Windows::Foundation::Collections::IVectorChangedEventArgs^ eventArgs);

		IBehavior^ VerifiedAttach(::Windows::UI::Xaml::DependencyObject^ item);

#if _DEBUG
		void VerifyOldCollectionIntegrity();
#endif
	};

}}}