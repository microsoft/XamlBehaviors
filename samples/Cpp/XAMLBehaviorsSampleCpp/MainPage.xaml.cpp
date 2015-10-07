//
// MainPage.xaml.cpp
// Implementation of the MainPage class.
//

#include "pch.h"
#include "MainPage.xaml.h"
#include "NavigatePageSample.xaml.h"

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
using namespace Windows::UI::Xaml::Interop;
using namespace Windows::UI::Xaml::Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

MainPage::MainPage()
{
	InitializeComponent();
	eventtriggercontrol = ref new EventTriggerControl();
	incrementalupdatecontrol = ref new IncrementalUpdateControl();
	controlstoryboardcontrol = ref new ControlStoryboardControl();
	playsoundcontrol = ref new PlaySoundControl();
	navigatetopagecontrol = ref new NavigateToPageControl();
	gotostatecontrol = ref new GoToStateControl();
	invokecommandcontrol = ref new InvokeCommandControl();
	custombehaviorcontrol = ref new CustomBehaviorControl();
	customactioncontrol = ref new CustomActionControl();
}

bool XAMLBehaviorsSampleCpp::MainPage::CheckLastPageIsNavigateSample()
{
	int lastIndex = (Frame->BackStack->Size) - 1;
	if (lastIndex >= 0)
	{
		auto lastPage = Frame->BackStack->GetAt(lastIndex);
		auto lastPageName = lastPage->SourcePageType.Name;
		if (lastPage != nullptr && lastPageName == TypeName(XAMLBehaviorsSampleCpp::NavigatePageSample::typeid).Name)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	else
	{
		return false;
	}
	
}

void XAMLBehaviorsSampleCpp::MainPage::OnNavigatedTo(NavigationEventArgs^ e)
{
	if (CheckLastPageIsNavigateSample())
	{
		pivot->SelectedIndex = 1;
		ActionsContent->Children->Clear();
		ActionsContent->Children->Append(navigatetopagecontrol);
	}
}

void XAMLBehaviorsSampleCpp::MainPage::EventTriggerButton_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
	BehaviorsContent->Children->Clear();
	BehaviorsContent->Children->Append(eventtriggercontrol);
}


void XAMLBehaviorsSampleCpp::MainPage::IncrementalUpdateButton_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
	BehaviorsContent->Children->Clear();
	BehaviorsContent->Children->Append(incrementalupdatecontrol);
}


void XAMLBehaviorsSampleCpp::MainPage::ControlStoryboardButton_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
	ActionsContent->Children->Clear();
	ActionsContent->Children->Append(controlstoryboardcontrol);
}


void XAMLBehaviorsSampleCpp::MainPage::PlaySoundButton_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
	ActionsContent->Children->Clear();
	ActionsContent->Children->Append(playsoundcontrol);
}


void XAMLBehaviorsSampleCpp::MainPage::NavigateToPageButton_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
	ActionsContent->Children->Clear();
	ActionsContent->Children->Append(navigatetopagecontrol);
}


void XAMLBehaviorsSampleCpp::MainPage::GoToStateButton_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
	ActionsContent->Children->Clear();
	ActionsContent->Children->Append(gotostatecontrol);
}


void XAMLBehaviorsSampleCpp::MainPage::InvokeCommandButton_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
	ActionsContent->Children->Clear();
	ActionsContent->Children->Append(invokecommandcontrol);
}


void XAMLBehaviorsSampleCpp::MainPage::CustomBehavior_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
	CustomContent->Children->Clear();
	CustomContent->Children->Append(custombehaviorcontrol);
}


void XAMLBehaviorsSampleCpp::MainPage::CustomAction_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
	CustomContent->Children->Clear();
	CustomContent->Children->Append(customactioncontrol);
}
