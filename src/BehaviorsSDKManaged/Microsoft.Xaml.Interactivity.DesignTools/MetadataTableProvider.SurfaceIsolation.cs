// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;

namespace Microsoft.Xaml.Interactivity.Design
{
    partial class MetadataTableProvider
    {
        private void AddAttributes(string typeIdentifier, params Attribute[] attributes)
        {
            _attributeTableBuilder.AddCustomAttributes(typeIdentifier, attributes);
        }

        private void AddAttributes(string typeIdentifier, string propertyName, params Attribute[] attributes)
        {
            _attributeTableBuilder.AddCustomAttributes(typeIdentifier, propertyName, attributes);
        }

        /// <summary>
        /// This class contains the type names required by the new Extensibility APIs.
        /// </summary>
        private static class Targets
        {
            private const string coreNS = "Microsoft.Xaml.Interactivity.";

            internal const string ActionCollection = coreNS  + "ActionCollection";
            internal const string BehaviorCollection = coreNS  + "BehaviorCollection";
        }
    }
}
