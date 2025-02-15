﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Xaml.Interactivity;

/// <summary>
/// Provides design tools information about which EventName to set for EventTriggerBehavior when instantiating an <see cref="Microsoft.Xaml.Interactivity.IAction"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class DefaultEventAttribute : Attribute
{
	private readonly Type _targetType;
	private readonly string _eventName;

	/// <summary>
	/// Initializes a new instance of the <see cref="DefaultEventAttribute"/> class.
	/// </summary>
	/// <param name="targetType">The type this attribute applies to.</param>
	/// <param name="eventName">The event name for the EventTriggerBehavior.</param>
	public DefaultEventAttribute(Type targetType, string eventName)
	{
		this._targetType = targetType;
		this._eventName = eventName;
	}

	/// <summary>
	/// Gets the type that the <see cref="DefaultEventAttribute"/> applies to.
	/// </summary>
	public Type TargetType
	{
		get
		{
			return this._targetType;
		}
	}

	/// <summary>
	/// Gets the event name to pass to the EventTriggerBehavior constructor.
	/// </summary>
	public string EventName
	{
		get
		{
			return this._eventName;
		}
	}
}