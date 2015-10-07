//
// MainPage.xaml.h
// Declaration of the MainPage class.
//

#pragma once

#include "MainPage.g.h"
#include "EventTriggerControl.xaml.h"
#include "IncrementalUpdateControl.xaml.h"
#include "ControlStoryboardControl.xaml.h"
#include "PlaySoundControl.xaml.h"
#include "GoToStateControl.xaml.h"
#include "NavigateToPageControl.xaml.h"
#include "InvokeCommandControl.xaml.h"
#include "CustomBehaviorControl.xaml.h"
#include "CustomActionControl.xaml.h"

namespace XAMLBehaviorsSampleCpp
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public ref class MainPage sealed
	{
	public:
		MainPage();

	protected:
		virtual void OnNavigatedTo(Windows::UI::Xaml::Navigation::NavigationEventArgs^ e) override;

	private:
		EventTriggerControl^ eventtriggercontrol;
		IncrementalUpdateControl^ incrementalupdatecontrol;
		ControlStoryboardControl^ controlstoryboardcontrol;
		PlaySoundControl^ playsoundcontrol;
		GoToStateControl^ gotostatecontrol;
		NavigateToPageControl^ navigatetopagecontrol;
		InvokeCommandControl^ invokecommandcontrol;
		CustomBehaviorControl^ custombehaviorcontrol;
		CustomActionControl^ customactioncontrol;

		bool CheckLastPageIsNavigateSample();

		void EventTriggerButton_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e);
		void IncrementalUpdateButton_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e);
		void ControlStoryboardButton_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e);
		void PlaySoundButton_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e);
		void NavigateToPageButton_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e);
		void GoToStateButton_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e);
		void InvokeCommandButton_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e);
		void CustomBehavior_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e);
		void CustomAction_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e);
	};
}
