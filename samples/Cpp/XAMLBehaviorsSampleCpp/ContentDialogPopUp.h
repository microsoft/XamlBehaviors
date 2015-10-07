#pragma once
#include "ContentDialogSample.xaml.h"


namespace XAMLBehaviorsSampleCpp
{
	using namespace Microsoft::Xaml::Interactivity;
	public ref class ContentDialogPopUp sealed : public Windows::UI::Xaml::DependencyObject, public IAction 
	{
		public:
			virtual Platform::Object^ Execute(Platform::Object^ sender, Platform::Object^ parameter);

		private:
			ContentDialogSample^ samplecd;
	};
}