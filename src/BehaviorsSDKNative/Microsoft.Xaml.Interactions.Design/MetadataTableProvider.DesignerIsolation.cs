// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using Microsoft.Xaml.Interactions.Core;
using Microsoft.Xaml.Interactions.Media;

namespace Microsoft.Xaml.Interactivity.Design
{
    partial class MetadataTableProvider
    {
        private void AddAttribute(Type type, Attribute attribute)
        {
            attributeTableBuilder.AddCustomAttributes(type, attribute);
        }

        private void AddAttributes(Type type, params Attribute[] attributes)
        {
            foreach (Attribute attribute in attributes)
            {
                AddAttribute(type, attribute);
            }
        }

        private void AddAttribute(Type type, string propertyName, Attribute attribute)
        {
            attributeTableBuilder.AddCustomAttributes(type, propertyName, attribute);
        }

        private void AddAttributes(Type type, string propertyName, params Attribute[] attributes)
        {
            foreach (Attribute attribute in attributes)
            {
                AddAttribute(type, propertyName, attribute);
            }
        }

        /// <summary>
        /// This class contains the types used by the older Extensibility APIs.
        /// </summary>
        private static class Targets
        {
            internal static readonly Type IncrementalUpdateBehavior = typeof(IncrementalUpdateBehavior);
            internal static readonly Type EventTriggerBehavior = typeof(EventTriggerBehavior);
            //internal static readonly Type DataTriggerBehavior = typeof(DataTriggerBehavior);
            //internal static readonly Type ChangePropertyAction = typeof(ChangePropertyAction);
            internal static readonly Type InvokeCommandAction = typeof(InvokeCommandAction);
            internal static readonly Type ControlStoryboardAction = typeof(ControlStoryboardAction);
            internal static readonly Type GoToStateAction = typeof(GoToStateAction);
            internal static readonly Type NavigateToPageAction = typeof(NavigateToPageAction);
            internal static readonly Type PlaySoundAction = typeof(PlaySoundAction);
            //internal static readonly Type CallMethodAction = typeof(CallMethodAction);
        }
    }
}
