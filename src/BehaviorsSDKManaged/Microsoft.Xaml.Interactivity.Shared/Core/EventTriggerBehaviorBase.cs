// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Xaml.Interactivity;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Microsoft.Xaml.Interactivity;

/// <summary>
/// A base behavior that listens for an event on its source and executes its actions when that event is fired.
/// </summary>
public abstract class EventTriggerBehaviorBase<T> : Trigger<T> where T : DependencyObject
{
    /// <summary>
    /// Identifies the <seealso cref="SourceObject"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SourceObjectProperty = DependencyProperty.Register(
        "SourceObject",
        typeof(T),
        typeof(EventTriggerBehaviorBase<T>),
        new PropertyMetadata(null, new PropertyChangedCallback(OnSourceObjectChanged)));

    private T _resolvedSource;
    private bool _isEventRegistered;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventTriggerBehaviorBase{T}"/> class.
    /// </summary>
    public EventTriggerBehaviorBase()
    {
    }

    /// <summary>
    /// Gets or sets the source object from which this behavior listens for events.
    /// If <seealso cref="SourceObject"/> is not set, the source will default to <seealso cref="Trigger{T}.AssociatedObject"/>. This is a dependency property.
    /// </summary>
    public T SourceObject
    {
        get => (T)GetValue(SourceObjectProperty);
        set => SetValue(SourceObjectProperty, value);
    }

    /// <inheritdoc/>
    protected override void OnAttached()
    {
        base.OnAttached();
        SetResolvedSource(ComputeResolvedSource());
    }

    /// <inheritdoc/>
    protected override void OnDetaching()
    {
        base.OnDetaching();
        SetResolvedSource(null);
    }

    /// <summary>
    /// Contains the core logic to register an event handler
    /// </summary>
    /// <param name="source">The object for which to register the event handler</param>
    /// <returns>true if the event handler got registered, false if it did not</returns>
    protected abstract bool RegisterEventCore(T source);

    /// <summary>
    /// Contains the core logic to unregister an event handler
    /// </summary>
    /// <param name="source">The object for which to unregister the event handler</param>
    protected abstract void UnregisterEventCore(T source);

    /// <summary>
    /// The method that should be called when the event is triggered
    /// </summary>
    /// <param name="sender">The event's sender</param>
    /// <param name="eventArgs">The event arguments</param>
    protected void OnEvent(object sender, object eventArgs)
    {
        Interaction.ExecuteActions(_resolvedSource, Actions, eventArgs);
    }

    /// <summary>
    /// Registers the event handler
    /// </summary>
    protected void RegisterEvent()
    {
        if (!_isEventRegistered)
        {
            _isEventRegistered = RegisterEventCore(_resolvedSource);
        }
    }

    /// <summary>
    /// Unregisters the event handler
    /// </summary>
    protected void UnregisterEvent()
    {
        if (_isEventRegistered)
        {
            _isEventRegistered = false;
            UnregisterEventCore(_resolvedSource);
        }
    }

    private void SetResolvedSource(T newSource)
    {
        if (AssociatedObject == null || _resolvedSource == newSource)
        {
            return;
        }

        if (_resolvedSource != null)
        {
            UnregisterEvent();
        }

        _resolvedSource = newSource;

        if (_resolvedSource != null)
        {
            RegisterEvent();
        }
    }

    private T ComputeResolvedSource()
    {
        // If the SourceObject property is set at all, we want to use it. It is possible that it is data
        // bound and bindings haven't been evaluated yet. Plus, this makes the API more predictable.
        if (ReadLocalValue(SourceObjectProperty) != DependencyProperty.UnsetValue)
        {
            return SourceObject;
        }

        return AssociatedObject;
    }

    private static void OnSourceObjectChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
    {
        EventTriggerBehaviorBase<T> behavior = (EventTriggerBehaviorBase<T>)dependencyObject;
        behavior.SetResolvedSource(behavior.ComputeResolvedSource());
    }
}
