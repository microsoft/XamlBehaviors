// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
#include "pch.h"
#include "PlaySoundAction.h"

using namespace Platform;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Controls::Primitives;
using namespace Microsoft::Xaml::Interactivity;
using namespace Microsoft::Xaml::Interactions::Media;

namespace DependencyProperties
{
	static DependencyProperty^ Source = DependencyProperty::Register(
		"Source",
		String::typeid,
		PlaySoundAction::typeid,
		ref new PropertyMetadata(nullptr));

	static DependencyProperty^ Volume = DependencyProperty::Register(
		"ControlStoryboardOption",
		double::typeid,
		PlaySoundAction::typeid,
		ref new PropertyMetadata(0.5));
}

DependencyProperty^ PlaySoundAction::SourceProperty::get()
{
	return DependencyProperties::Source;
}

DependencyProperty^ PlaySoundAction::VolumeProperty::get()
{
	return DependencyProperties::Volume;
}

Object^ PlaySoundAction::Execute(Object^ sender, Object^ parameter)
{
	if (this->Source == nullptr)
	{
		return false;
	}
	
	Uri^ sourceUri = nullptr;
	try
	{
		sourceUri = ref new Uri(this->Source);
	}
	catch (Platform::InvalidArgumentException^)
	{
	}
	if (sourceUri == nullptr || sourceUri->Suspicious || wcscmp(sourceUri->SchemeName->Data(), L"ms-appx") != 0)
	{
		// Impose ms-appx:// scheme if user has specified a relative URI
		std::wstring str(L"ms-appx:///");
		str.append(this->Source->Data());
		sourceUri = ref new Uri(StringReference(str.c_str()));
		if (sourceUri->Suspicious)
		{
			return false;
		}
	}

	this->popup = ref new Popup();
	MediaElement^ mediaElement = ref new MediaElement();
	popup->Child = mediaElement;

	// It is legal (although not advisable) to provide a video file. By setting visibility to collapsed, only the sound track should play.
	mediaElement->Visibility = Visibility::Collapsed;

	mediaElement->Source = sourceUri;
	mediaElement->Volume = this->Volume;

	mediaElement->MediaEnded += ref new RoutedEventHandler(this, &PlaySoundAction::OnMediaEnded);
	mediaElement->MediaFailed += ref new ExceptionRoutedEventHandler(this, &PlaySoundAction::OnMediaFailed);

	this->popup->IsOpen = true;

	return true;
}

void PlaySoundAction::OnMediaEnded(Object^ sender, RoutedEventArgs^ e)
{
	if (this->popup != nullptr)
	{
		this->popup->Child = nullptr;
		this->popup->IsOpen = false;
		this->popup = nullptr;
	}
}

void PlaySoundAction::OnMediaFailed(Object^ sender, ExceptionRoutedEventArgs^ e)
{
	if (this->popup != nullptr)
	{
		this->popup->Child = nullptr;
		this->popup->IsOpen = false;
		this->popup = nullptr;
	}
}
