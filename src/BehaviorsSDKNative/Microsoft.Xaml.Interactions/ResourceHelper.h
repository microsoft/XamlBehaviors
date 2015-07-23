// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
#pragma once
#pragma warning(disable: 6298)

namespace Microsoft { namespace Xaml { namespace Interactions { namespace Core
{
	namespace ResourceHelper
	{
		inline Platform::String^ GetString(Platform::String^ resourceName)
		{
			::Windows::ApplicationModel::Resources::ResourceLoader^ strings = ::Windows::ApplicationModel::Resources::ResourceLoader::GetForCurrentView("Microsoft.Xaml.Interactions/Strings");
			return strings->GetString(resourceName);
		}

		/// <summary>
		/// The string table is shared between managed and native implementations of the Behaviors SDK.
		/// This method is a quick, one parameter only version of string.Format() that uses the {0} syntax for replacement.
		/// </summary>
		inline Platform::String^ GetString(Platform::String^ resourceName, Platform::String^ parameter)
		{
			Platform::String^ resourceString = ResourceHelper::GetString(resourceName);

			std::wstring message(resourceString->Data());
			size_t firstParen = message.find(L"{");
			if (firstParen < 0)
			{
				return resourceString;
			}

			if (parameter == nullptr || parameter->IsEmpty())
			{
				message.erase(firstParen, 3);
			}
			else
			{
				message.replace(firstParen, 3, parameter->Data());
			}

			return ref new Platform::String(message.data());
		}
	}

}}}}
