//
// ContentDialogSample.xaml.cpp
// Implementation of the ContentDialogSample class
//

#include "pch.h"
#include "ContentDialogSample.xaml.h"

using namespace XAMLBehaviorsSampleCpp;

using namespace Platform;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Controls::Primitives;
using namespace Windows::UI::Xaml::Data;
using namespace Windows::UI::Xaml::Input;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

XAMLBehaviorsSampleCpp::ContentDialogSample::ContentDialogSample()
{
	InitializeComponent();
}

void XAMLBehaviorsSampleCpp::ContentDialogSample::ContentDialog_PrimaryButtonClick(Windows::UI::Xaml::Controls::ContentDialog^ sender, Windows::UI::Xaml::Controls::ContentDialogButtonClickEventArgs^ args)
{
	this->Hide();
}

void XAMLBehaviorsSampleCpp::ContentDialogSample::ContentDialog_SecondaryButtonClick(Windows::UI::Xaml::Controls::ContentDialog^ sender, Windows::UI::Xaml::Controls::ContentDialogButtonClickEventArgs^ args)
{
}
