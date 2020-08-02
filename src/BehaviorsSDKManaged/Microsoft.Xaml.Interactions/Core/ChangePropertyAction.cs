// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace Microsoft.Xaml.Interactions.Core
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Interactivity;
    using Windows.UI.Xaml;

    /// <summary>
    /// An action that will change a specified property to a specified value when invoked.
    /// </summary>
    public sealed partial class ChangePropertyAction : DependencyObject, IAction
    {
        /// <summary>
        /// Identifies the <seealso cref="PropertyName"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(
            "PropertyName",
            typeof(PropertyPath),
            typeof(ChangePropertyAction),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <seealso cref="TargetObject"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register(
            "TargetObject",
            typeof(object),
            typeof(ChangePropertyAction),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <seealso cref="Value"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(object),
            typeof(ChangePropertyAction),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the name of the property to change. This is a dependency property.
        /// </summary>
        public PropertyPath PropertyName
        {
            get
            {
                return (PropertyPath)this.GetValue(ChangePropertyAction.PropertyNameProperty);
            }
            set
            {
                this.SetValue(ChangePropertyAction.PropertyNameProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the value to set. This is a dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        public object Value
        {
            get
            {
                return this.GetValue(ChangePropertyAction.ValueProperty);
            }
            set
            {
                this.SetValue(ChangePropertyAction.ValueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the object whose property will be changed.
        /// If <seealso cref="TargetObject"/> is not set or cannot be resolved, the sender of <seealso cref="Execute"/> will be used. This is a dependency property.
        /// </summary>
        public object TargetObject
        {
            get
            {
                return (object)this.GetValue(ChangePropertyAction.TargetObjectProperty);
            }
            set
            {
                this.SetValue(ChangePropertyAction.TargetObjectProperty, value);
            }
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="sender">The <see cref="System.Object"/> that is passed to the action by the behavior. Generally this is <seealso cref="Microsoft.Xaml.Interactivity.IBehavior.AssociatedObject"/> or a target object.</param>
        /// <param name="parameter">The value of this parameter is determined by the caller.</param>
        /// <returns>True if updating the property value succeeds; else false.</returns>
        public object Execute(object sender, object parameter)
        {
            object targetObject;
            if (this.ReadLocalValue(ChangePropertyAction.TargetObjectProperty) != DependencyProperty.UnsetValue)
            {
                targetObject = this.TargetObject;
            }
            else
            {
                targetObject = sender;
            }

            if (targetObject == null || this.PropertyName == null)
            {
                return false;
            }

            this.UpdatePropertyValue(targetObject);
            return true;
        }

        private void UpdatePropertyValue(object targetObject)
        {
            int GetTypeHierarchyDepth(TypeInfo type)
            {
                int depth = 1;
                while (type.BaseType != null)
                {
                    depth++;
                    type = type.BaseType.GetTypeInfo();
                }
                return depth;
            }

            Type targetType = targetObject.GetType();
            var properties = targetType.GetRuntimeProperties()
                                       .Where(p => p.Name == this.PropertyName.Path)
                                       .OrderByDescending(p => GetTypeHierarchyDepth(p.DeclaringType.GetTypeInfo()));
            PropertyInfo propertyInfo = properties.First();
            this.ValidateProperty(targetType.Name, propertyInfo);

            Exception innerException = null;
            try
            {
                object result = null;
                string valueAsString = null;
                Type propertyType = propertyInfo.PropertyType;
                TypeInfo propertyTypeInfo = propertyType.GetTypeInfo();
                if (this.Value == null)
                {
                    // The result can be null if the type is generic (nullable), or the default value of the type in question
                    result = propertyTypeInfo.IsValueType ? Activator.CreateInstance(propertyType) : null;
                }
                else if (propertyTypeInfo.IsAssignableFrom(this.Value.GetType().GetTypeInfo()))
                {
                    result = this.Value;
                }
                else
                {
                    valueAsString = this.Value.ToString();
                    result = propertyTypeInfo.IsEnum ? Enum.Parse(propertyType, valueAsString, false) :
                        TypeConverterHelper.Convert(valueAsString, propertyType.FullName);
                }

                propertyInfo.SetValue(targetObject, result, new object[0]);
            }
            catch (FormatException e)
            {
                innerException = e;
            }
            catch (ArgumentException e)
            {
                innerException = e;
            }

            if (innerException != null)
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.CurrentCulture,
                    ResourceHelper.ChangePropertyActionCannotSetValueExceptionMessage,
                    this.Value != null ? this.Value.GetType().Name : "null",
                    this.PropertyName,
                    propertyInfo.PropertyType.Name),
                    innerException);
            }
        }

        /// <summary>
        /// Ensures the property is not null and can be written to.
        /// </summary>
        private void ValidateProperty(string targetTypeName, PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.CurrentCulture,
                    ResourceHelper.ChangePropertyActionCannotFindPropertyNameExceptionMessage,
                    this.PropertyName,
                    targetTypeName));
            }
            else if (!propertyInfo.CanWrite)
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.CurrentCulture,
                    ResourceHelper.ChangePropertyActionPropertyIsReadOnlyExceptionMessage,
                    this.PropertyName,
                    targetTypeName));
            }
        }
    }
}
