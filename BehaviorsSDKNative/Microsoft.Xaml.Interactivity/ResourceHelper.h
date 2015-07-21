// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
#pragma once
#pragma warning(disable: 6298)
namespace Microsoft { namespace Xaml { namespace Interactivity
{
	namespace ResourceHelper
	{
		inline Platform::String^ GetString(Platform::String^ resourceName)
		{
			Windows::ApplicationModel::Resources::ResourceLoader^ strings = Windows::ApplicationModel::Resources::ResourceLoader::GetForCurrentView("Microsoft.Xaml.Interactivity/Strings");
			return strings->GetString(resourceName);
		}
	}

}}}