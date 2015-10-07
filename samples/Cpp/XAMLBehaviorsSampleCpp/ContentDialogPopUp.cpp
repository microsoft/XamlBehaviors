#pragma once

#include "pch.h"
#include "ContentDialogPopUp.h"

using namespace Platform;
using namespace XAMLBehaviorsSampleCpp;

Object^ ContentDialogPopUp::Execute(Platform::Object^ sender, Platform::Object^ parameter)
{
	samplecd = ref new ContentDialogSample();
	samplecd->ShowAsync();
	return nullptr;
}