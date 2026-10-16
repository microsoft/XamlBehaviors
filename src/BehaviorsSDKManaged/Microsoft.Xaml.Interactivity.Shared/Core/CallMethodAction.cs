// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Microsoft.Xaml.Interactivity;

/// <summary>
/// An action that calls a method on a specified object when invoked.
/// </summary>
#if NET5_0_OR_GREATER
[RequiresUnreferencedCode("This action is not trim-safe.")]
#endif
public sealed class CallMethodAction : DependencyObject, IAction
{
    /// <summary>
    /// Identifies the <seealso cref="MethodName"/> dependency property.
    /// </summary>
    [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
    public static readonly DependencyProperty MethodNameProperty = DependencyProperty.Register(
        "MethodName",
        typeof(string),
        typeof(CallMethodAction),
        new PropertyMetadata(null, new PropertyChangedCallback(CallMethodAction.OnMethodNameChanged)));

    /// <summary>
    /// Identifies the <seealso cref="TargetObject"/> dependency property.
    /// </summary>
    [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
    public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register(
        "TargetObject",
        typeof(object),
        typeof(CallMethodAction),
        new PropertyMetadata(null, new PropertyChangedCallback(CallMethodAction.OnTargetObjectChanged)));

    private Type _targetObjectType;
    private List<MethodDescriptor> _methodDescriptors = new List<MethodDescriptor>();
    private MethodDescriptor _cachedMethodDescriptor;

    /// <summary>
    /// Gets or sets the name of the method to invoke. This is a dependency property.
    /// </summary>
    public string MethodName
    {
        get
        {
            return (string)this.GetValue(CallMethodAction.MethodNameProperty);
        }

        set
        {
            this.SetValue(CallMethodAction.MethodNameProperty, value);
        }
    }

    /// <summary>
    /// Gets or sets the object that exposes the method of interest. This is a dependency property.
    /// </summary>
    public object TargetObject
    {
        get
        {
            return this.GetValue(CallMethodAction.TargetObjectProperty);
        }

        set
        {
            this.SetValue(CallMethodAction.TargetObjectProperty, value);
        }
    }

    /// <summary>
    /// Executes the action.
    /// </summary>
    /// <param name="sender">The <see cref="object"/> that is passed to the action by the behavior. Generally this is <seealso cref="Microsoft.Xaml.Interactivity.IBehavior.AssociatedObject"/> or a target object.</param>
    /// <param name="parameter">The value of this parameter is determined by the caller.</param>
    /// <returns>True if the method is called; else false.</returns>
    public object Execute(object sender, object parameter)
    {
        object target;
        if (this.ReadLocalValue(CallMethodAction.TargetObjectProperty) != DependencyProperty.UnsetValue)
        {
            target = this.TargetObject;
        }
        else
        {
            target = sender;
        }

        if (target == null || string.IsNullOrEmpty(this.MethodName))
        {
            return false;
        }

        this.UpdateTargetType(target.GetType());

        MethodDescriptor methodDescriptor = this.FindBestMethod(parameter);
        if (methodDescriptor == null)
        {
            if (this.TargetObject != null)
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.CurrentCulture,
                    ResourceHelper.CallMethodActionValidMethodNotFoundExceptionMessage,
                    this.MethodName,
                    this._targetObjectType));
            }

            return false;
        }

        ParameterInfo[] parameters = methodDescriptor.Parameters;
        if (parameters.Length == 0)
        {
            methodDescriptor.MethodInfo.Invoke(target, parameters: null);
            return true;
        }
        else if (parameters.Length == 2)
        {
            methodDescriptor.MethodInfo.Invoke(target, new object[] { target, parameter });
            return true;
        }

        return false;
    }

    private MethodDescriptor FindBestMethod(object parameter)
    {
        TypeInfo parameterTypeInfo = parameter == null ? null : parameter.GetType().GetTypeInfo();

        if (parameterTypeInfo == null)
        {
            return this._cachedMethodDescriptor;
        }

        MethodDescriptor mostDerivedMethod = null;

        // Loop over the methods looking for the one whose type is closest to the type of the given parameter.
        foreach (MethodDescriptor currentMethod in this._methodDescriptors)
        {
            TypeInfo currentTypeInfo = currentMethod.SecondParameterTypeInfo;

            if (currentTypeInfo.IsAssignableFrom(parameterTypeInfo))
            {
                if (mostDerivedMethod == null || !currentTypeInfo.IsAssignableFrom(mostDerivedMethod.SecondParameterTypeInfo))
                {
                    mostDerivedMethod = currentMethod;
                }
            }
        }

        return mostDerivedMethod ?? this._cachedMethodDescriptor;
    }

    private void UpdateTargetType(Type newTargetType)
    {
        if (newTargetType == this._targetObjectType)
        {
            return;
        }

        this._targetObjectType = newTargetType;

        this.UpdateMethodDescriptors();
    }

    private void UpdateMethodDescriptors()
    {
        this._methodDescriptors.Clear();
        this._cachedMethodDescriptor = null;

        if (string.IsNullOrEmpty(this.MethodName) || this._targetObjectType == null)
        {
            return;
        }

        // Find all public methods that match the given name  and have either no parameters,
        // or two parameters where the first is of type Object.
        foreach (MethodInfo method in this._targetObjectType.GetRuntimeMethods())
        {
            if (string.Equals(method.Name, this.MethodName, StringComparison.Ordinal)
                && method.ReturnType == typeof(void)
                && method.IsPublic)
            {
                ParameterInfo[] parameters = method.GetParameters();
                if (parameters.Length == 0)
                {
                    // There can be only one parameterless method of the given name.
                    this._cachedMethodDescriptor = new MethodDescriptor(method, parameters);
                }
                else if (parameters.Length == 2 && parameters[0].ParameterType == typeof(object))
                {
                    this._methodDescriptors.Add(new MethodDescriptor(method, parameters));
                }
            }
        }

        // We didn't find a parameterless method, so we want to find a method that accepts null
        // as a second parameter, but if we have more than one of these it is ambigious which
        // we should call, so we do nothing.
        if (this._cachedMethodDescriptor == null)
        {
            foreach (MethodDescriptor method in this._methodDescriptors)
            {
                TypeInfo typeInfo = method.SecondParameterTypeInfo;
                if (!typeInfo.IsValueType || (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    if (this._cachedMethodDescriptor != null)
                    {
                        this._cachedMethodDescriptor = null;
                        return;
                    }
                    else
                    {
                        this._cachedMethodDescriptor = method;
                    }
                }
            }
        }
    }

    private static void OnMethodNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
    {
        CallMethodAction callMethodAction = (CallMethodAction)sender;
        callMethodAction.UpdateMethodDescriptors();
    }

    private static void OnTargetObjectChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
    {
        CallMethodAction callMethodAction = (CallMethodAction)sender;

        Type newType = args.NewValue != null ? args.NewValue.GetType() : null;
        callMethodAction.UpdateTargetType(newType);
    }

    [DebuggerDisplay("{MethodInfo}")]
    private class MethodDescriptor
    {
        public MethodDescriptor(MethodInfo methodInfo, ParameterInfo[] methodParameters)
        {
            this.MethodInfo = methodInfo;
            this.Parameters = methodParameters;
        }

        public MethodInfo MethodInfo
        {
            get;
            private set;
        }

        public ParameterInfo[] Parameters
        {
            get;
            private set;
        }

        public int ParameterCount
        {
            get
            {
                return this.Parameters.Length;
            }
        }

        public TypeInfo SecondParameterTypeInfo
        {
            get
            {
                if (this.ParameterCount < 2)
                {
                    return null;
                }

                return this.Parameters[1].ParameterType.GetTypeInfo();
            }
        }
    }
}
