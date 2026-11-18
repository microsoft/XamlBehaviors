// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

#if NET5_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#else
using System.Runtime.InteropServices.WindowsRuntime;
#endif

namespace Microsoft.Xaml.Interactivity;

/// <summary>
/// A behavior that listens for a specified event on its source and executes its actions when that event is fired.
/// </summary>
#if NET5_0_OR_GREATER
[RequiresUnreferencedCode("This behavior is not trim-safe, as it uses reflection to register the event handler. Use the provided event-specific trigger behaviors, or make your own by deriving from EventTriggerBehaviorBase<T>.")]
#endif
public sealed class EventTriggerBehavior : EventTriggerBehaviorBase<DependencyObject>
{
    /// <summary>
    /// Identifies the <seealso cref="EventName"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EventNameProperty = DependencyProperty.Register(
        "EventName",
        typeof(string),
        typeof(EventTriggerBehavior),
        new PropertyMetadata("Loaded", new PropertyChangedCallback(EventTriggerBehavior.OnEventNameChanged)));

    private Delegate _eventHandler;
    private EventInfo _event; // null if EventName == Loaded
#if !NET5_0_OR_GREATER
    private bool _isWindowsRuntimeEvent;
    private Func<Delegate, EventRegistrationToken> _addEventHandlerMethod;
    private Action<EventRegistrationToken> _removeEventHandlerMethod;
#endif

    /// <summary>
    /// Initializes a new instance of the <see cref="EventTriggerBehavior"/> class.
    /// </summary>
    public EventTriggerBehavior()
    {
    }

    /// <summary>
    /// Gets or sets the name of the event to listen for. This is a dependency property.
    /// </summary>
    public string EventName
    {
        get => (string)GetValue(EventNameProperty);
        set => SetValue(EventNameProperty, value);
    }

    /// <inheritdoc/>
    protected override bool RegisterEventCore(DependencyObject source)
    {
        var eventName = EventName;
        if (string.IsNullOrEmpty(eventName))
        {
            return false;
        }

        if (eventName != "Loaded")
        {
            Type sourceObjectType = source.GetType();
            var info = sourceObjectType.GetRuntimeEvent(eventName);
            if (info == null)
            {
                return false;
            }

            MethodInfo methodInfo = typeof(EventTriggerBehavior).GetTypeInfo().GetDeclaredMethod("OnEvent");
            _eventHandler = methodInfo.CreateDelegate(info.EventHandlerType, this);

#if !NET5_0_OR_GREATER
            _isWindowsRuntimeEvent = IsWindowsRuntimeEvent(info);
            if (_isWindowsRuntimeEvent)
            {
                _addEventHandlerMethod = add => (EventRegistrationToken)info.AddMethod.Invoke(source, new object[] { add });
                _removeEventHandlerMethod = token => info.RemoveMethod.Invoke(source, new object[] { token });

                WindowsRuntimeMarshal.AddEventHandler(_addEventHandlerMethod, _removeEventHandlerMethod, _eventHandler);

                return true;
            }
#endif

            info.AddEventHandler(source, _eventHandler);
            _event = info;

            return true;
        }
        else if (source is FrameworkElement element && !LoadedTriggerBehavior.IsElementLoaded(element))
        {
            element.Loaded += OnEvent;
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <inheritdoc/>
    protected override void UnregisterEventCore(DependencyObject source)
    {
        // Don't use the EventName property in this function. By the point this is called, it might have changed.
#if !NET5_0_OR_GREATER
        if (_isWindowsRuntimeEvent)
        {
            WindowsRuntimeMarshal.RemoveEventHandler(_removeEventHandlerMethod, _eventHandler);
            _isWindowsRuntimeEvent = false;
            _addEventHandlerMethod = null;
            _removeEventHandlerMethod = null;
            _eventHandler = null;

            return;
        }
#endif

        if (_event != null)
        {
            _event.RemoveEventHandler(source, _eventHandler);
            _event = null;
            _eventHandler = null;
        }
        else
        {
            ((FrameworkElement)source).Loaded -= OnEvent;
        }
    }

    private static void OnEventNameChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
    {
        EventTriggerBehavior behavior = (EventTriggerBehavior)dependencyObject;

        behavior.UnregisterEvent();
        behavior.RegisterEvent();
    }

#if !NET5_0_OR_GREATER
    private static bool IsWindowsRuntimeEvent(EventInfo eventInfo)
    {
        return eventInfo != null &&
            IsWindowsRuntimeType(eventInfo.EventHandlerType) &&
            IsWindowsRuntimeType(eventInfo.DeclaringType);
    }

    private static bool IsWindowsRuntimeType(Type type)
    {
        if (type != null)
        {
            // This will only work when using built-in WinRT interop, ie. where .winmd files are directly
            // referenced instead of generated projections. That is, this would not work on .NET 5 or higher,
            // where CsWinRT is used instead.
            return type.AssemblyQualifiedName.EndsWith("ContentType=WindowsRuntime", StringComparison.Ordinal);
        }

        return false;
    }
#endif
}
