//
// IncrementalUpdateControl.xaml.h
// Declaration of the IncrementalUpdateControl class
//

#pragma once

#include "IncrementalUpdateControl.g.h"
#include "IncrementalSample.h"

namespace XAMLBehaviorsSampleCpp
{
	[Windows::Foundation::Metadata::WebHostHidden]
	public ref class IncrementalUpdateControl sealed
	{
		Windows::UI::Xaml::Interop::IBindableObservableVector^ Items;
	public:
		IncrementalUpdateControl();

	private:
		
	};
}
