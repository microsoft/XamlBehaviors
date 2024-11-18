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
            private const string rootNamespace = "Microsoft.Xaml.Interactivity.";

            internal const string CallMethodAction          = rootNamespace + "CallMethodAction";
            internal const string ChangePropertyAction      = rootNamespace + "ChangePropertyAction";
            internal const string ControlStoryboardAction   = rootNamespace + "ControlStoryboardAction";
            internal const string DataTriggerBehavior       = rootNamespace + "DataTriggerBehavior";
            internal const string IncrementalUpdateBehavior = rootNamespace + "IncrementalUpdateBehavior";
            internal const string EventTriggerBehavior      = rootNamespace + "EventTriggerBehavior";
            internal const string GoToStateAction           = rootNamespace + "GoToStateAction";
            internal const string InvokeCommandAction       = rootNamespace + "InvokeCommandAction";
            internal const string NavigateToPageAction      = rootNamespace + "NavigateToPageAction";
            internal const string PlaySoundAction           = rootNamespace + "PlaySoundAction";
            internal const string ActionCollection          = rootNamespace + "ActionCollection";
            internal const string BehaviorCollection        = rootNamespace + "BehaviorCollection";
        }
    }
}
