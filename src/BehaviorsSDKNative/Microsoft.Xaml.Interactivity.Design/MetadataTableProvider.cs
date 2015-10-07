// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.ComponentModel;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Xaml.Interactivity.Design.Properties;

namespace Microsoft.Xaml.Interactivity.Design
{
    internal class MetadataTableProvider : IProvideAttributeTable, IRequireAttributeTableTypeResolver
    {
        private AttributeTableBuilder attributeTableBuilder;

        public IAttributeTableTypeResolver TypeResolver { set; private get; }

        public AttributeTable AttributeTable
        {
            get
            {
                if (attributeTableBuilder == null)
                {
                    attributeTableBuilder = new AttributeTableBuilder();
                }

                AddAttribute<ActionCollection>(new CategoryAttribute(Resources.Category_Name_Actions));
                AddAttribute<BehaviorCollection>(new ToolboxBrowsableAttribute(false));

                return attributeTableBuilder.CreateTable();
            }
        }

        private void AddAttribute<T>(Attribute attribute)
        {
            attributeTableBuilder.AddCustomAttributes(typeof(T), attribute);
        }

        private void AddAttribute(string typeName, Attribute attribute)
        {
            Type type = this.TypeResolver.ResolveType(typeName);
            if (type != null)
            {
                attributeTableBuilder.AddCustomAttributes(type, attribute);
            }
        }
    }
}
