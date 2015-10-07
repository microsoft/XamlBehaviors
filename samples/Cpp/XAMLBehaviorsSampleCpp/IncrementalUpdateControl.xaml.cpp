//
// IncrementalUpdateControl.xaml.cpp
// Implementation of the IncrementalUpdateControl class
//

#include "pch.h"
#include "IncrementalUpdateControl.xaml.h"

using namespace XAMLBehaviorsSampleCpp;

using namespace Platform;
using namespace Platform::Collections;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Controls::Primitives;
using namespace Windows::UI::Xaml::Data;
using namespace Windows::UI::Xaml::Input;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

IncrementalUpdateControl::IncrementalUpdateControl()
{
	InitializeComponent();

	Items = ref new Platform::Collections::Vector<ItemSample^>();
	gridViewSample->ItemsSource = Items;
	for (int i = 1; i <= 100; i++)
	{
		Items->Append(ref new ItemSample(i));
	}
}

