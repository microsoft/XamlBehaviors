//
// InvokeCommandControl.xaml.cpp
// Implementation of the InvokeCommandControl class
//

#include "pch.h"
#include "InvokeCommandControl.xaml.h"
#include "SampleCommand.h"

using namespace XAMLBehaviorsSampleCpp;

using namespace Platform;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::UI;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Controls::Primitives;
using namespace Windows::UI::Xaml::Data;
using namespace Windows::UI::Xaml::Input;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

InvokeCommandControl::InvokeCommandControl()
{
	UpdateGreyCommand = ref new SampleCommand(ref new ExecuteDelegate(this, &InvokeCommandControl::updateGrey), nullptr);
	UpdatePinkCommand = ref new SampleCommand(ref new ExecuteDelegate(this, &InvokeCommandControl::updatePink), nullptr);

	InitializeComponent();
	this->DataContext = this;

	darkgreybrush = ref new SolidColorBrush();
	Color grey = Color();
	grey.A = 255;
	grey.R = 51;
	grey.G = 51;
	grey.B = 50;
	darkgreybrush->Color = grey;

	pinkbrush = ref new SolidColorBrush();
	Color pink = Color();
	pink.A = 255;
	pink.R = 233;
	pink.G = 95;
	pink.B = 91;
	pinkbrush->Color = pink;


	
}

ICommand^ InvokeCommandControl::GreyCommand::get()
{
	return UpdateGreyCommand;
}

ICommand^ InvokeCommandControl::PinkCommand::get()
{
	return UpdatePinkCommand;
}

void InvokeCommandControl::GreyCommand::set(Windows::UI::Xaml::Input::ICommand^ value)
{
	UpdateGreyCommand = value;
}

void InvokeCommandControl::PinkCommand::set(Windows::UI::Xaml::Input::ICommand^ value)
{
	UpdateGreyCommand = value;
}

void InvokeCommandControl::updateGrey(Platform::Object^ parameter)
{
	border->Background = darkgreybrush;
}

void InvokeCommandControl::updatePink(Platform::Object^ parameter)
{
	border->Background = pinkbrush;
}
