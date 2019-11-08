// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Xaml.Interactions.Core
{
    using Interactivity;

    /// <summary>
    /// A behavior that listens for a specified event on its source and executes its actions when that event is fired.
    /// </summary>
    public sealed class EventTriggerBehavior : Trigger
    {
        /// <summary>
        /// Identifies the <seealso cref="EventName"/> dependency property.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty EventNameProperty = DependencyProperty.Register(
            "EventName",
            typeof(string),
            typeof(EventTriggerBehavior),
            new PropertyMetadata("Loaded", new PropertyChangedCallback(EventTriggerBehavior.OnEventNameChanged)));

        /// <summary>
        /// Identifies the <seealso cref="SourceObject"/> dependency property.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty SourceObjectProperty = DependencyProperty.Register(
            "SourceObject",
            typeof(object),
            typeof(EventTriggerBehavior),
            new PropertyMetadata(null, new PropertyChangedCallback(EventTriggerBehavior.OnSourceObjectChanged)));

        private object _resolvedSource;
        private Delegate _eventHandler;
        private bool _isLoadedEventRegistered;
        private bool _isWindowsRuntimeEvent;
        private Func<Delegate, EventRegistrationToken> _addEventHandlerMethod;
        private Action<EventRegistrationToken> _removeEventHandlerMethod;

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
            get
            {
                return (string)this.GetValue(EventTriggerBehavior.EventNameProperty);
            }

            set
            {
                this.SetValue(EventTriggerBehavior.EventNameProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the source object from which this behavior listens for events.
        /// If <seealso cref="SourceObject"/> is not set, the source will default to <seealso cref="Behavior.AssociatedObject"/>. This is a dependency property.
        /// </summary>
        public object SourceObject
        {
            get
            {
                return (object)this.GetValue(EventTriggerBehavior.SourceObjectProperty);
            }

            set
            {
                this.SetValue(EventTriggerBehavior.SourceObjectProperty, value);
            }
        }

        /// <summary>
        /// Called after the behavior is attached to the <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.SetResolvedSource(this.ComputeResolvedSource());
        }

        /// <summary>
        /// Called when the behavior is being detached from its <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.SetResolvedSource(null);
        }

        private void SetResolvedSource(object newSource)
        {
            if (this.AssociatedObject == null || this._resolvedSource == newSource)
            {
                return;
            }

            if (this._resolvedSource != null)
            {
                this.UnregisterEvent(this.EventName);
            }

            this._resolvedSource = newSource;

            if (this._resolvedSource != null)
            {
                this.RegisterEvent(this.EventName);
            }
        }

        private object ComputeResolvedSource()
        {
            // If the SourceObject property is set at all, we want to use it. It is possible that it is data
            // bound and bindings haven't been evaluated yet. Plus, this makes the API more predictable.
            if (this.ReadLocalValue(EventTriggerBehavior.SourceObjectProperty) != DependencyProperty.UnsetValue)
            {
                return this.SourceObject;
            }

            return this.AssociatedObject;
        }

        private void RegisterEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                return;
            }

            if (eventName != "Loaded")
            {
                Type sourceObjectType = this._resolvedSource.GetType();
                EventInfo info = sourceObjectType.GetRuntimeEvent(eventName);
                if (info == null)
                {
                    return;
                }

                MethodInfo methodInfo = typeof(EventTriggerBehavior).GetTypeInfo().GetDeclaredMethod("OnEvent");
                this._eventHandler = methodInfo.CreateDelegate(info.EventHandlerType, this);

                this._isWindowsRuntimeEvent = EventTriggerBehavior.IsWindowsRuntimeEvent(info);
                if (this._isWindowsRuntimeEvent)
                {
                    this._addEventHandlerMethod = add => (EventRegistrationToken)info.AddMethod.Invoke(this._resolvedSource, new object[] { add });
                    this._removeEventHandlerMethod = token => info.RemoveMethod.Invoke(this._resolvedSource, new object[] { token });

                    WindowsRuntimeMarshal.AddEventHandler(this._addEventHandlerMethod, this._removeEventHandlerMethod, this._eventHandler);
                }
                else
                {
                    info.AddEventHandler(this._resolvedSource, this._eventHandler);
                }
            }
            else if (!this._isLoadedEventRegistered)
            {
                FrameworkElement element = this._resolvedSource as FrameworkElement;
                if (element != null && !EventTriggerBehavior.IsElementLoaded(element))
                {
                    this._isLoadedEventRegistered = true;
                    element.Loaded += this.OnEvent;
                }
            }
        }

        private void UnregisterEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                return;
            }

            if (eventName != "Loaded")
            {
                if (this._eventHandler == null)
                {
                    return;
                }

                EventInfo info = this._resolvedSource.GetType().GetRuntimeEvent(eventName);
                if (this._isWindowsRuntimeEvent)
                {
                    WindowsRuntimeMarshal.RemoveEventHandler(this._removeEventHandlerMethod, this._eventHandler);
                }
                else
                {
                    info.RemoveEventHandler(this._resolvedSource, this._eventHandler);
                }

                this._eventHandler = null;
            }
            else if (this._isLoadedEventRegistered)
            {
                this._isLoadedEventRegistered = false;
                FrameworkElement element = (FrameworkElement)this._resolvedSource;
                element.Loaded -= this.OnEvent;
            }
        }

        private void OnEvent(object sender, object eventArgs)
        {
            Interaction.ExecuteActions(this._resolvedSource, this.Actions, eventArgs);
        }

        private static void OnSourceObjectChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            EventTriggerBehavior behavior = (EventTriggerBehavior)dependencyObject;
            behavior.SetResolvedSource(behavior.ComputeResolvedSource());
        }

        private static void OnEventNameChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            EventTriggerBehavior behavior = (EventTriggerBehavior)dependencyObject;
            if (behavior.AssociatedObject == null || behavior._resolvedSource == null)
            {
                return;
            }

            string oldEventName = (string)args.OldValue;
            string newEventName = (string)args.NewValue;

            behavior.UnregisterEvent(oldEventName);
            behavior.RegisterEvent(newEventName);
        }

        internal static bool IsElementLoaded(FrameworkElement element)
        {
            if (element == null)
            {
                return false;
            }

            UIElement rootVisual = Window.Current.Content;
            DependencyObject parent = element.Parent;
            if (parent == null)
            {
                // If the element is the child of a ControlTemplate it will have a null parent even when it is loaded.
                // To catch that scenario, also check it's parent in the visual tree.
                parent = VisualTreeHelper.GetParent(element);
            }

            return (parent != null || (rootVisual != null && element == rootVisual));
        }

        private static bool IsWindowsRuntimeEvent(EventInfo eventInfo)
        {
            return eventInfo != null &&
                EventTriggerBehavior.IsWindowsRuntimeType(eventInfo.EventHandlerType) &&
                EventTriggerBehavior.IsWindowsRuntimeType(eventInfo.DeclaringType);
        }

        private static bool IsWindowsRuntimeType(Type type)
        {
            if (type != null)
            {
                return type.AssemblyQualifiedName.EndsWith("ContentType=WindowsRuntime", StringComparison.Ordinal);
            }

            return false;
        }
    }
}
