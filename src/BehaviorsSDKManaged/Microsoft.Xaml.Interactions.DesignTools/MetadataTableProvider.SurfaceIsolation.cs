// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;

namespace Microsoft.Xaml.Interactions.Design
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
            private const string mediaNS = "Microsoft.Xaml.Interactivity.";

            internal const string CallMethodAction          = coreNS  + "CallMethodAction";
            internal const string ChangePropertyAction      = coreNS  + "ChangePropertyAction";
            internal const string ControlStoryboardAction   = mediaNS + "ControlStoryboardAction";
            internal const string DataTriggerBehavior       = coreNS  + "DataTriggerBehavior";
            internal const string IncrementalUpdateBehavior = coreNS  + "IncrementalUpdateBehavior";
            internal const string EventTriggerBehavior      = coreNS  + "EventTriggerBehavior";
            internal const string GoToStateAction           = coreNS  + "GoToStateAction";
            internal const string InvokeCommandAction       = coreNS  + "InvokeCommandAction";
            internal const string NavigateToPageAction      = coreNS  + "NavigateToPageAction";
            internal const string PlaySoundAction           = mediaNS + "PlaySoundAction";
        }
    }
}
