// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;

namespace Microsoft.Xaml.Interactivity.Design
{
    partial class MetadataTableProvider
    {
        private void AddAttribute(Type type, Attribute attribute)
        {
            _attributeTableBuilder.AddCustomAttributes(type, attribute);
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
            _attributeTableBuilder.AddCustomAttributes(type, propertyName, attribute);
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
            internal static readonly Type ActionCollection = typeof(ActionCollection);
            internal static readonly Type BehaviorCollection = typeof(BehaviorCollection);
        }
    }
}
