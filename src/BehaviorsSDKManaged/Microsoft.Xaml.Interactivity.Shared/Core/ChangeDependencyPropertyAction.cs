// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
#endif

namespace Microsoft.Xaml.Interactivity;

/// <summary>
/// An action that will change a specified dependency property to a specified value when invoked.
/// </summary>
[ContentProperty(Name = "Value")]
public sealed class ChangeDependencyPropertyAction : DependencyObject, IAction
{
    /// <summary>
    /// Identifies the <seealso cref="TargetObject"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register(
        "TargetObject",
        typeof(DependencyObject),
        typeof(ChangeDependencyPropertyAction),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <seealso cref="Value"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        "Value",
        typeof(object),
        typeof(ChangeDependencyPropertyAction),
        new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets the dependency property to change. This is not a dependency property, due to framework restrictions.
    /// </summary>
    public DependencyProperty DependencyProperty { get; set; }

    /// <summary>
    /// Gets or sets the value to set. This is a dependency property.
    /// </summary>
    public object Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>
    /// Gets or sets the object whose property will be changed.
    /// If <seealso cref="TargetObject"/> is not set or cannot be resolved, the sender of <seealso cref="Execute"/> will be used. This is a dependency property.
    /// </summary>
    public DependencyObject TargetObject
    {
        get => (DependencyObject)GetValue(TargetObjectProperty);
        set => SetValue(TargetObjectProperty, value);
    }

    /// <summary>
    /// Executes the action.
    /// </summary>
    /// <param name="sender">The <see cref="object"/> that is passed to the action by the behavior. Generally this is <seealso cref="IBehavior.AssociatedObject"/> or a target object.</param>
    /// <param name="parameter">Ignored</param>
    /// <returns>True if updating the dependency property value succeeds; else false.</returns>
    public object Execute(object sender, object parameter)
    {
        DependencyObject targetObject;
        if (ReadLocalValue(TargetObjectProperty) != DependencyProperty.UnsetValue)
        {
            targetObject = TargetObject;
        }
        else
        {
            targetObject = sender as DependencyObject;
        }

        if (targetObject == null || DependencyProperty == null)
        {
            return false;
        }

        targetObject.SetValue(DependencyProperty, Value);
        return true;
    }
}