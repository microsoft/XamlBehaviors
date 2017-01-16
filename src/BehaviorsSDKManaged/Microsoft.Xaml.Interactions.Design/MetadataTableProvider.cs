// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

extern alias WindowsRuntime;
using System;
using System.ComponentModel;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.PropertyEditing;
using Microsoft.Windows.Design.PropertyEditing.Editors;
using Microsoft.Xaml.Interactions.Core;
using Microsoft.Xaml.Interactions.Design.Properties;
using Microsoft.Xaml.Interactions.Media;

namespace Microsoft.Xaml.Interactivity.Design
{
    internal class MetadataTableProvider : IProvideAttributeTable
    {
        private AttributeTableBuilder attributeTableBuilder;

        public AttributeTable AttributeTable
        {
            get
            {
                if (attributeTableBuilder == null)
                {
                    attributeTableBuilder = new AttributeTableBuilder();
                }

                #region IncrementalUpdateBehavior
                AddAttributes<IncrementalUpdateBehavior>(
                    new DescriptionAttribute(Resources.Description_IncrementalUpdateBehavior));

                AddAttributes<IncrementalUpdateBehavior>(
                    nameof(IncrementalUpdateBehavior.Phase),
                    new DescriptionAttribute(Resources.Description_IncrementalUpdateBehavior_Phase),
                    new CategoryAttribute(Resources.Category_Common_Properties));
                #endregion

                #region EventTriggerBehavior
                AddAttributes<EventTriggerBehavior>(
                    new DescriptionAttribute(Resources.Description_EventTriggerBehavior));

                AddAttributes<EventTriggerBehavior>(
                    nameof(EventTriggerBehavior.EventName),
                    new DescriptionAttribute(Resources.Description_EventTriggerBehavior_EventName),
                    CreateEditorAttribute<EventPickerPropertyValueEditor>(),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes<EventTriggerBehavior>(
                    nameof(EventTriggerBehavior.SourceObject),
                    new DescriptionAttribute(Resources.Description_EventTriggerBehavior_SourceObject),
                    CreateEditorAttribute<ElementBindingPickerPropertyValueEditor>(),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes<EventTriggerBehavior>(
                    "Actions",
                    new DescriptionAttribute(Resources.Description_EventTriggerBehavior_Actions));
                #endregion

                #region DataTriggerBehavior
                AddAttributes<DataTriggerBehavior>(
                    new DefaultBindingPropertyAttribute(nameof(DataTriggerBehavior.Binding)));

                AddAttributes<DataTriggerBehavior>(
                    nameof(DataTriggerBehavior.Binding),
                    new DescriptionAttribute(Resources.Description_DataTriggerBehavior_Binding),
                    CreateEditorAttribute<PropertyBindingPickerPropertyValueEditor>(),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes<DataTriggerBehavior>(
                    nameof(DataTriggerBehavior.ComparisonCondition),
                    new DescriptionAttribute(Resources.Description_DataTriggerBehavior_ComparisonCondition),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes<DataTriggerBehavior>(
                    nameof(DataTriggerBehavior.Value),
                    new DescriptionAttribute(Resources.Description_DataTriggerBehavior_Value),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new TypeConverterAttribute(typeof(StringConverter)));

                AddAttributes<DataTriggerBehavior>(
                    "Actions",
                    new DescriptionAttribute(Resources.Description_DataTriggerBehavior_Actions));
                #endregion

                #region ChangePropertyAction
                AddAttributes<ChangePropertyAction>(
                    new DescriptionAttribute(Resources.Description_ChangePropertyAction));

                AddAttributes<ChangePropertyAction>(
                    nameof(ChangePropertyAction.PropertyName),
                    CreateEditorAttribute<PropertyPickerPropertyValueEditor>(),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new DescriptionAttribute(Resources.Description_ChangePropertyAction_PropertyName));

                AddAttributes<ChangePropertyAction>(
                    nameof(ChangePropertyAction.TargetObject),
                    new DescriptionAttribute(Resources.Description_ChangePropertyAction_TargetObject),
                    CreateEditorAttribute<ElementBindingPickerPropertyValueEditor>(),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes<ChangePropertyAction>(
                    nameof(ChangePropertyAction.Value),
                    new DescriptionAttribute(Resources.Description_ChangePropertyAction_Value),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new BrowsableAttribute(false));

                #endregion ChangePropertyAction

                #region InvokeCommandAction
                AddAttributes<InvokeCommandAction>(
                    new DescriptionAttribute(Resources.Description_InvokeCommandAction),
                    new DefaultBindingPropertyAttribute(nameof(InvokeCommandAction.Command)));

                AddAttributes<InvokeCommandAction>(
                    nameof(InvokeCommandAction.Command),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    CreateEditorAttribute<PropertyBindingPickerPropertyValueEditor>(),
                    new DescriptionAttribute(Resources.Description_InvokeCommandAction_Command));

                AddAttributes<InvokeCommandAction>(
                    nameof(InvokeCommandAction.CommandParameter),
                    new TypeConverterAttribute(typeof(StringConverter)),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new DescriptionAttribute(Resources.Description_InvokeCommandAction_CommandParameter),
                    new EditorBrowsableAttribute(EditorBrowsableState.Advanced));

                AddAttributes<InvokeCommandAction>(
                    nameof(InvokeCommandAction.InputConverter),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new DescriptionAttribute(Resources.Description_InvokeCommandAction_InputConverter),
                    new EditorBrowsableAttribute(EditorBrowsableState.Advanced));

                AddAttributes<InvokeCommandAction>(
                    nameof(InvokeCommandAction.InputConverterParameter),
                    new TypeConverterAttribute(typeof(StringConverter)),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new DescriptionAttribute(Resources.Description_InvokeCommandAction_InputConverterParameter),
                    new EditorBrowsableAttribute(EditorBrowsableState.Advanced));

                AddAttributes<InvokeCommandAction>(
                    nameof(InvokeCommandAction.InputConverterLanguage),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new DescriptionAttribute(Resources.Description_InvokeCommandAction_InputConverterLanguage),
                    new TypeConverterAttribute(typeof(CultureInfoNamesConverter)),
                    new EditorBrowsableAttribute(EditorBrowsableState.Advanced));
                #endregion

                #region ControlStoryboardAction
                AddAttributes<ControlStoryboardAction>(
                    new DescriptionAttribute(Resources.Description_ControlStoryboardAction));

                AddAttributes<ControlStoryboardAction>(
                    nameof(ControlStoryboardAction.ControlStoryboardOption),
                    new DescriptionAttribute(Resources.Description_ControlStoryboardAction_ControlStoryboardOption),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes<ControlStoryboardAction>(
                    nameof(ControlStoryboardAction.Storyboard),
                    new DescriptionAttribute(Resources.Description_ControlStoryboardAction_Storyboard),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    CreateEditorAttribute<StoryboardPickerPropertyValueEditor>(),
                    new TypeConverterAttribute(typeof(TypeConverter)));
                #endregion

                #region GotoStateAction
                AddAttributes<GoToStateAction>(
                    new DescriptionAttribute(Resources.Description_GoToStateAction));

                AddAttributes<GoToStateAction>(
                    nameof(GoToStateAction.StateName),
                    CreateEditorAttribute<StatePickerPropertyValueEditor>(),
                    new DescriptionAttribute(Resources.Description_GoToStateAction_StateName),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes<GoToStateAction>(
                    nameof(GoToStateAction.UseTransitions),
                    new DescriptionAttribute(Resources.Description_GoToStateAction_UseTransitions),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes<GoToStateAction>(
                    nameof(GoToStateAction.TargetObject),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new DescriptionAttribute(Resources.Description_GoToStateAction_TargetObject),
                    CreateEditorAttribute<ElementBindingPickerPropertyValueEditor>());
                #endregion

                #region NavigateToPageAction
                AddAttributes<NavigateToPageAction>(
                    new DescriptionAttribute(Resources.Description_NavigateToPageAction));

                AddAttributes<NavigateToPageAction>(
                    nameof(NavigateToPageAction.TargetPage),
                    CreateEditorAttribute<PagePickerPropertyValueEditor>(),
                    new DescriptionAttribute(Resources.Description_NavigateToPageAction_TargetPage),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes<NavigateToPageAction>(
                    nameof(NavigateToPageAction.Parameter),
                    new DescriptionAttribute(Resources.Description_NavigateToPageAction_Parameter),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new TypeConverterAttribute(typeof(StringConverter)));
                #endregion

                #region PlaySoundAction
                AddAttributes<PlaySoundAction>(
                    new DescriptionAttribute(Resources.Description_PlaySoundAction));

                AddAttributes<PlaySoundAction>(
                    nameof(PlaySoundAction.Source),
                    CreateEditorAttribute<UriPropertyValueEditor>(),
                    new DescriptionAttribute(Resources.Description_PlaySoundAction_Source),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes<PlaySoundAction>(
                    nameof(PlaySoundAction.Volume),
                    new NumberRangesAttribute(0, 0, 1, 1, canBeAuto: false),
                    new NumberIncrementsAttribute(0.001, 0.01, 0.1),
                    new DescriptionAttribute(Resources.Description_PlaySoundAction_Volume),
                    new CategoryAttribute(Resources.Category_Common_Properties));
                #endregion

                #region CallMethodAction
                PropertyOrder order = PropertyOrder.Default;

                AddAttributes<CallMethodAction>(
                    new DescriptionAttribute(Resources.Description_CallMethodAction),
                    new DefaultBindingPropertyAttribute(nameof(CallMethodAction.TargetObject)));

                AddAttributes<CallMethodAction>(
                    nameof(CallMethodAction.TargetObject),
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)),
                    new DescriptionAttribute(Resources.Description_CallMethodAction_TargetObject),
                    CreateEditorAttribute<ElementBindingPickerPropertyValueEditor>(),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes<CallMethodAction>(
                    nameof(CallMethodAction.MethodName),
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)),
                    new DescriptionAttribute(Resources.Description_CallMethodAction_MethodName),
                    new CategoryAttribute(Resources.Category_Common_Properties));
                #endregion

                #region
                AddAttributes<FadeAction>(
                    new DescriptionAttribute(Resources.Description_FadeAction),
                    new CategoryAttribute(Resources.Category_Common_Properties)
                    );

                AddAttributes<FadeAction>(
                    "Duration",
                    new DescriptionAttribute(Resources.Description_FadeAction_Duration),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes<FadeAction>(
                    "FadeType",
                    new DescriptionAttribute(Resources.Description_FadeAction_FadeType),
                    new CategoryAttribute(Resources.Category_Common_Properties));
                #endregion

                return attributeTableBuilder.CreateTable();
            }
        }

        private void AddAttribute<T>(Attribute attribute)
        {
            attributeTableBuilder.AddCustomAttributes(typeof(T), attribute);
        }
        
        private void AddAttributes<T>(params Attribute[] attributes)
        {
            foreach (Attribute attribute in attributes)
            {
                AddAttribute<T>(attribute);
            }
        }

        private void AddAttribute<T>(string propertyName, Attribute attribute)
        {
            attributeTableBuilder.AddCustomAttributes(typeof(T), propertyName, attribute);
        }

        private void AddAttributes<T>(string propertyName, params Attribute[] attributes)
        {
            foreach (Attribute attribute in attributes)
            {
                AddAttribute<T>(propertyName, attribute);
            }
        }
        private static EditorAttribute CreateEditorAttribute<T>() where T : PropertyValueEditor
        {
            return PropertyValueEditor.CreateEditorAttribute(typeof(T));
        }

    }
}