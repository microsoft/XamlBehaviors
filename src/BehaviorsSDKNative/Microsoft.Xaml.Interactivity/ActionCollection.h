// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
#pragma once

namespace Microsoft { namespace Xaml { namespace Interactivity
{
	/// <summary>
	/// Represents a collection of IActions.
	/// </summary>
	public ref class ActionCollection sealed : ::Windows::UI::Xaml::DependencyObjectCollection
	{
	public:
		/// <summary>
		/// Initializes a new instance of the <see cref="ActionCollection"/> class.
		/// </summary>
		ActionCollection();

	private:
		void OnVectorChanged(
			::Windows::Foundation::Collections::IObservableVector<::Windows::UI::Xaml::DependencyObject^>^ sender,
			::Windows::Foundation::Collections::IVectorChangedEventArgs^ eventArgs);

		static void VerifyType(::Windows::UI::Xaml::DependencyObject^ item);
	};

}}}