// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace Microsoft.Xaml.Interactions.Core
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;
    using Interactivity;

    internal static class DataBindingHelper
    {
        private static readonly Dictionary<Type, List<DependencyProperty>> DependenciesPropertyCache = new Dictionary<Type, List<DependencyProperty>>();

        /// <summary>
        /// Ensures that all binding expression on actions are up to date.
        /// </summary>
        /// <remarks>
        /// DataTriggerBehavior fires during data binding phase. Since the ActionCollection is a child of the behavior,
        /// bindings on the action  may not be up-to-date. This routine is called before the action
        /// is executed in order to guarantee that all bindings are refreshed with the most current data.
        /// </remarks>
        public static void RefreshDataBindingsOnActions(ActionCollection actions)
        {
            foreach (DependencyObject action in actions)
            {
                foreach (DependencyProperty property in DataBindingHelper.GetDependencyProperties(action.GetType()))
                {
                    DataBindingHelper.RefreshBinding(action, property);
                }
            }
        }

        private static IEnumerable<DependencyProperty> GetDependencyProperties(Type type)
        {
            List<DependencyProperty> propertyList = null;

            if (!DataBindingHelper.DependenciesPropertyCache.TryGetValue(type, out propertyList))
            {
                propertyList = new List<DependencyProperty>();

                while (type != null && type != typeof(Object))
                {
                    foreach (FieldInfo fieldInfo in type.GetRuntimeFields())
                    {
                        if (fieldInfo.IsPublic && typeof(DependencyProperty).IsAssignableFrom(fieldInfo.FieldType))
                        {
                            DependencyProperty property = fieldInfo.GetValue(null) as DependencyProperty;
                            if (property != null)
                            {
                                propertyList.Add(property);
                            }
                        }
                    }

                    type = type.GetTypeInfo().BaseType;
                }

                DataBindingHelper.DependenciesPropertyCache[type] = propertyList;
            }

            return propertyList;
        }

        private static void RefreshBinding(DependencyObject target, DependencyProperty property)
        {
			BindingExpression binding = target.ReadLocalValue(property) as BindingExpression;
            if (binding != null && binding.ParentBinding != null)
            {
                BindingOperations.SetBinding(target, property, binding.ParentBinding);
            }
		}
    }
}
