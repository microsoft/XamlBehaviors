// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
#include "pch.h"
#include "NavigateToPageAction.h"

using namespace Platform;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Markup;
using namespace Windows::UI::Xaml::Media;
using namespace Microsoft::Xaml::Interactivity;
using namespace Microsoft::Xaml::Interactions::Core;

namespace DependencyProperties
{
	static DependencyProperty^ Parameter = DependencyProperty::Register(
		"Parameter",
		String::typeid,
		NavigateToPageAction::typeid,
		ref new PropertyMetadata(nullptr));

	static DependencyProperty^ TargetPage = DependencyProperty::Register(
		"TargetPage",
		Object::typeid,
		NavigateToPageAction::typeid,
		ref new PropertyMetadata(nullptr));
}

DependencyProperty^ NavigateToPageAction::TargetPageProperty::get()
{
	return DependencyProperties::TargetPage;
}

DependencyProperty^ NavigateToPageAction::ParameterProperty::get()
{
	return DependencyProperties::Parameter;
}

Object^ NavigateToPageAction::Execute(Object^ sender, Object^ parameter)
{
	if (this->TargetPage == nullptr)
	{
		return false;
	}

	IXamlMetadataProvider^ metadataProvider = safe_cast<IXamlMetadataProvider^>(Application::Current);
	IXamlType^ xamlType = metadataProvider->GetXamlType(this->TargetPage);
	if (xamlType == nullptr)
	{
		return false;
	}

	INavigate^ navigateElement = dynamic_cast<INavigate^>(Window::Current->Content);
	DependencyObject^ senderObject = dynamic_cast<DependencyObject^>(sender);

	// If the sender wasn't an INavigate, then keep looking up the tree from the
	// root we were given for another INavigate.
	while (senderObject != nullptr && navigateElement == nullptr)
	{
		navigateElement = dynamic_cast<INavigate^>(senderObject);
		if (navigateElement == nullptr)
		{
			senderObject = VisualTreeHelper::GetParent(senderObject);
		}
	}

	if (navigateElement == nullptr)
	{
		return false;
	}

	Frame^ frame = dynamic_cast<Frame^>(navigateElement);

	if (frame != nullptr)
	{
		Object^ frameParameter = this->Parameter != nullptr
			? this->Parameter
			: parameter;

		return frame->Navigate(xamlType->UnderlyingType, frameParameter);
	}
	else
	{
		return navigateElement->Navigate(xamlType->UnderlyingType);
	}
}
