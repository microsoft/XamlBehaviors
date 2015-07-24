// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
#pragma once

#include "IBehavior.h"

namespace Microsoft { namespace Xaml { namespace Interactivity
{
	/// <summary>
	/// Interface implemented by all custom actions.
	/// </summary>
	[::Windows::Foundation::Metadata::WebHostHidden]
	public interface class IAction
	{
		/// <summary>
		/// Executes the action.
		/// </summary>
		/// <param name="sender">The <see cref="Platform::Object"/> that is passed to the action by the behavior. Generally this is <seealso cref="Microsoft::Xaml::Interactivity::IBehavior::AssociatedObject"/> or a target object.</param>
		/// <param name="parameter">The value of this parameter is determined by the caller.</param>
		/// <remarks> An example of parameter usage is EventTriggerBehavior, which passes the EventArgs as a parameter to its actions.</remarks>
		/// <returns>Returns the result of the action.</returns>
		::Platform::Object^ Execute(::Platform::Object^ sender, ::Platform::Object^ parameter);
	};

}}}