//
// InvokeCommandControl.xaml.h
// Declaration of the InvokeCommandControl class
//

#pragma once

#include "InvokeCommandControl.g.h"

namespace XAMLBehaviorsSampleCpp
{
	[Windows::UI::Xaml::Data::Bindable]
	public ref class InvokeCommandControl sealed
	{
	public:
		InvokeCommandControl();

		property Windows::UI::Xaml::Input::ICommand^ GreyCommand
		{
			Windows::UI::Xaml::Input::ICommand^ get();
			void set(Windows::UI::Xaml::Input::ICommand^ value);
		}

		property Windows::UI::Xaml::Input::ICommand^ PinkCommand
		{
			Windows::UI::Xaml::Input::ICommand^ get();
			void set(Windows::UI::Xaml::Input::ICommand^ value);
		}

	private:
		Windows::UI::Xaml::Input::ICommand^ UpdateGreyCommand;
		Windows::UI::Xaml::Input::ICommand^ UpdatePinkCommand;

		Windows::UI::Xaml::Media::SolidColorBrush^ darkgreybrush;
		Windows::UI::Xaml::Media::SolidColorBrush^ pinkbrush;

		void updateGrey(Platform::Object^ parameter);
		void updatePink(Platform::Object^ parameter);
	};
}
