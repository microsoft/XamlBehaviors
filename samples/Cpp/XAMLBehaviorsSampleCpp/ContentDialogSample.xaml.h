//
// ContentDialogSample.xaml.h
// Declaration of the ContentDialogSample class
//

#pragma once

#include "ContentDialogSample.g.h"

namespace XAMLBehaviorsSampleCpp
{
	[Windows::Foundation::Metadata::WebHostHidden]
	public ref class ContentDialogSample sealed
	{
	public:
		ContentDialogSample();
	private:
		void ContentDialog_PrimaryButtonClick(Windows::UI::Xaml::Controls::ContentDialog^ sender, Windows::UI::Xaml::Controls::ContentDialogButtonClickEventArgs^ args);
		void ContentDialog_SecondaryButtonClick(Windows::UI::Xaml::Controls::ContentDialog^ sender, Windows::UI::Xaml::Controls::ContentDialogButtonClickEventArgs^ args);
	};
}
