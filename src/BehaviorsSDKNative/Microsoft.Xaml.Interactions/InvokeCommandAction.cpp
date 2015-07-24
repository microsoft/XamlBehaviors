// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
#include "pch.h"
#include "InvokeCommandAction.h"

using namespace Platform;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Data;
using namespace Windows::UI::Xaml::Input;
using namespace Microsoft::Xaml::Interactivity;
using namespace Microsoft::Xaml::Interactions::Core;

namespace DependencyProperties
{
	static DependencyProperty^ Command = DependencyProperty::Register(
		"Command",
		ICommand::typeid,
		InvokeCommandAction::typeid,
		ref new PropertyMetadata(nullptr));

	static DependencyProperty^ CommandParameter = DependencyProperty::Register(
		"CommandParameter",
		Object::typeid,
		InvokeCommandAction::typeid,
		ref new PropertyMetadata(nullptr));

	static DependencyProperty^ InputConverter = DependencyProperty::Register(
		"InputConverter",
		IValueConverter::typeid,
		InvokeCommandAction::typeid,
		ref new PropertyMetadata(nullptr));

	static DependencyProperty^ InputConverterParameter = DependencyProperty::Register(
		"InputConverterParameter",
		Object::typeid,
		InvokeCommandAction::typeid,
		ref new PropertyMetadata(nullptr));

	static DependencyProperty^ InputConverterLanguage = DependencyProperty::Register(
		"InputConverterLanguage",
		String::typeid,
		InvokeCommandAction::typeid,
		ref new PropertyMetadata(""));
}

DependencyProperty^ InvokeCommandAction::CommandProperty::get()
{
	return DependencyProperties::Command;
}

DependencyProperty^ InvokeCommandAction::CommandParameterProperty::get()
{
	return DependencyProperties::CommandParameter;
}

DependencyProperty^ InvokeCommandAction::InputConverterProperty::get()
{
	return DependencyProperties::InputConverter;
}

DependencyProperty^ InvokeCommandAction::InputConverterParameterProperty::get()
{
	return DependencyProperties::InputConverterParameter;
}

DependencyProperty^ InvokeCommandAction::InputConverterLanguageProperty::get()
{
	return DependencyProperties::InputConverterLanguage;
}

Object^ InvokeCommandAction::Execute(Object^ sender, Object^ parameter)
{
	if (this->Command == nullptr)
	{
		return false;
	}

	Object^ resolvedParamter;
	if (this->CommandParameter != nullptr)
	{
		resolvedParamter = this->CommandParameter;
	}
	else if (this->InputConverter != nullptr)
	{
		resolvedParamter = this->InputConverter->Convert(
			parameter,
			Object::GetType(),
			this->InputConverterParameter,
			this->InputConverterLanguage);
	}
	else
	{
		resolvedParamter = parameter;
	}

	if (!this->Command->CanExecute(resolvedParamter))
	{
		return false;
	}

	this->Command->Execute(resolvedParamter);
	return true;
}
