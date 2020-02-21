// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.ComponentModel;
using Microsoft.Xaml.Interactions.Design.Properties;

#if SurfaceIsolation
using Microsoft.VisualStudio.DesignTools.Extensibility.Metadata;
using Microsoft.VisualStudio.DesignTools.Extensibility.PropertyEditing;
using Editors = Microsoft.VisualStudio.DesignTools.Extensibility.PropertyEditing.Editors;
#else
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.PropertyEditing;
using Editors = Microsoft.Windows.Design.PropertyEditing.Editors;
#endif

[assembly: ProvideMetadata(typeof(Microsoft.Xaml.Interactivity.Design.MetadataTableProvider))]

namespace Microsoft.Xaml.Interactivity.Design
{
    partial class MetadataTableProvider : IProvideAttributeTable
    {
        private AttributeTableBuilder _attributeTableBuilder;

        public AttributeTable AttributeTable
        {
            get
            {
                if (_attributeTableBuilder == null)
                {
                    _attributeTableBuilder = new AttributeTableBuilder();
                }

                #region IncrementalUpdateBehavior
                AddAttributes(Targets.IncrementalUpdateBehavior, new DescriptionAttribute(Resources.Description_IncrementalUpdateBehavior));

                AddAttributes(Targets.IncrementalUpdateBehavior, "Phase",
                    new DescriptionAttribute(Resources.Description_IncrementalUpdateBehavior_Phase),
                    new CategoryAttribute(Resources.Category_Common_Properties));
                #endregion

                #region EventTriggerBehavior
                AddAttributes(Targets.EventTriggerBehavior, new DescriptionAttribute(Resources.Description_EventTriggerBehavior));

                AddAttributes(Targets.EventTriggerBehavior, "EventName",
                    new DescriptionAttribute(Resources.Description_EventTriggerBehavior_EventName),
                    CreateEditorAttribute<Editors.EventPickerPropertyValueEditor>(),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes(Targets.EventTriggerBehavior, "SourceObject",
                    new DescriptionAttribute(Resources.Description_EventTriggerBehavior_SourceObject),
                    CreateEditorAttribute<Editors.ElementBindingPickerPropertyValueEditor>(),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes(Targets.EventTriggerBehavior, "Actions",
                    new DescriptionAttribute(Resources.Description_EventTriggerBehavior_Actions));
                #endregion

                #region DataTriggerBehavior
                AddAttributes(Targets.DataTriggerBehavior, new DefaultBindingPropertyAttribute("Binding"));

                AddAttributes(Targets.DataTriggerBehavior, "Binding",
                    new DescriptionAttribute(Resources.Description_DataTriggerBehavior_Binding),
                    CreateEditorAttribute<Editors.PropertyBindingPickerPropertyValueEditor>(),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes(Targets.DataTriggerBehavior, "ComparisonCondition",
                    new DescriptionAttribute(Resources.Description_DataTriggerBehavior_ComparisonCondition),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes(Targets.DataTriggerBehavior, "Value",
                    new DescriptionAttribute(Resources.Description_DataTriggerBehavior_Value),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new TypeConverterAttribute(typeof(StringConverter)));

                AddAttributes(Targets.DataTriggerBehavior, "Actions",
                    new DescriptionAttribute(Resources.Description_DataTriggerBehavior_Actions));
                #endregion

                #region ChangePropertyAction
                AddAttributes(Targets.ChangePropertyAction, new DescriptionAttribute(Resources.Description_ChangePropertyAction));

                AddAttributes(Targets.ChangePropertyAction, "PropertyName",
                    CreateEditorAttribute<Editors.PropertyPickerPropertyValueEditor>(),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new DescriptionAttribute(Resources.Description_ChangePropertyAction_PropertyName));

                AddAttributes(Targets.ChangePropertyAction, "TargetObject",
                    new DescriptionAttribute(Resources.Description_ChangePropertyAction_TargetObject),
                    CreateEditorAttribute<Editors.ElementBindingPickerPropertyValueEditor>(),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes(Targets.ChangePropertyAction, "Value",
                    new DescriptionAttribute(Resources.Description_ChangePropertyAction_Value),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new BrowsableAttribute(false));

                #endregion ChangePropertyAction

                #region InvokeCommandAction
                AddAttributes(Targets.InvokeCommandAction,
                    new DescriptionAttribute(Resources.Description_InvokeCommandAction),
                    new DefaultBindingPropertyAttribute("Command"));

                AddAttributes(Targets.InvokeCommandAction, "Command",
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    CreateEditorAttribute<Editors.PropertyBindingPickerPropertyValueEditor>(),
                    new DescriptionAttribute(Resources.Description_InvokeCommandAction_Command));

                AddAttributes(Targets.InvokeCommandAction, "CommandParameter",
                    new TypeConverterAttribute(typeof(StringConverter)),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new DescriptionAttribute(Resources.Description_InvokeCommandAction_CommandParameter),
                    new EditorBrowsableAttribute(EditorBrowsableState.Advanced));

                AddAttributes(Targets.InvokeCommandAction, "InputConverter",
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new DescriptionAttribute(Resources.Description_InvokeCommandAction_InputConverter),
                    new EditorBrowsableAttribute(EditorBrowsableState.Advanced));

                AddAttributes(Targets.InvokeCommandAction, "InputConverterParameter",
                    new TypeConverterAttribute(typeof(StringConverter)),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new DescriptionAttribute(Resources.Description_InvokeCommandAction_InputConverterParameter),
                    new EditorBrowsableAttribute(EditorBrowsableState.Advanced));

                AddAttributes(Targets.InvokeCommandAction, "InputConverterLanguage",
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new DescriptionAttribute(Resources.Description_InvokeCommandAction_InputConverterLanguage),
                    new TypeConverterAttribute(typeof(CultureInfoNamesConverter)),
                    new EditorBrowsableAttribute(EditorBrowsableState.Advanced));
                #endregion

                #region ControlStoryboardAction
                AddAttributes(Targets.ControlStoryboardAction, new DescriptionAttribute(Resources.Description_ControlStoryboardAction));

                AddAttributes(Targets.ControlStoryboardAction, "ControlStoryboardOption",
                    new DescriptionAttribute(Resources.Description_ControlStoryboardAction_ControlStoryboardOption),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes(Targets.ControlStoryboardAction, "Storyboard",
                    new DescriptionAttribute(Resources.Description_ControlStoryboardAction_Storyboard),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    CreateEditorAttribute<Editors.StoryboardPickerPropertyValueEditor>(),
                    new TypeConverterAttribute(typeof(TypeConverter)));
                #endregion

                #region GotoStateAction
                AddAttributes(Targets.GoToStateAction, new DescriptionAttribute(Resources.Description_GoToStateAction));

                AddAttributes(Targets.GoToStateAction, "StateName",
                    CreateEditorAttribute<Editors.StatePickerPropertyValueEditor>(),
                    new DescriptionAttribute(Resources.Description_GoToStateAction_StateName),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes(Targets.GoToStateAction, "UseTransitions",
                    new DescriptionAttribute(Resources.Description_GoToStateAction_UseTransitions),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes(Targets.GoToStateAction, "TargetObject",
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new DescriptionAttribute(Resources.Description_GoToStateAction_TargetObject),
                    CreateEditorAttribute<Editors.ElementBindingPickerPropertyValueEditor>());
                #endregion

                #region NavigateToPageAction
                AddAttributes(Targets.NavigateToPageAction, new DescriptionAttribute(Resources.Description_NavigateToPageAction));

                AddAttributes(Targets.NavigateToPageAction, "TargetPage",
                    CreateEditorAttribute<Editors.PagePickerPropertyValueEditor>(),
                    new DescriptionAttribute(Resources.Description_NavigateToPageAction_TargetPage),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes(Targets.NavigateToPageAction, "Parameter",
                    new DescriptionAttribute(Resources.Description_NavigateToPageAction_Parameter),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new TypeConverterAttribute(typeof(StringConverter)));
                #endregion

                #region PlaySoundAction
                AddAttributes(Targets.PlaySoundAction, new DescriptionAttribute(Resources.Description_PlaySoundAction));

                AddAttributes(Targets.PlaySoundAction, "Source",
                    CreateEditorAttribute<Editors.UriPropertyValueEditor>(),
                    new DescriptionAttribute(Resources.Description_PlaySoundAction_Source),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes(Targets.PlaySoundAction, "Volume",
                    new NumberRangesAttribute(0, 0, 1, 1, canBeAuto: false),
                    new NumberIncrementsAttribute(0.001, 0.01, 0.1),
                    new DescriptionAttribute(Resources.Description_PlaySoundAction_Volume),
                    new CategoryAttribute(Resources.Category_Common_Properties));
                #endregion

                #region CallMethodAction
                PropertyOrder order = PropertyOrder.Default;

                AddAttributes(Targets.CallMethodAction, new DescriptionAttribute(Resources.Description_CallMethodAction),
                    new DefaultBindingPropertyAttribute("TargetObject"));

                AddAttributes(Targets.CallMethodAction, "TargetObject",
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)),
                    new DescriptionAttribute(Resources.Description_CallMethodAction_TargetObject),
                    CreateEditorAttribute<Editors.ElementBindingPickerPropertyValueEditor>(),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes(Targets.CallMethodAction, "MethodName",
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)),
                    new DescriptionAttribute(Resources.Description_CallMethodAction_MethodName),
                    new CategoryAttribute(Resources.Category_Common_Properties));
                #endregion

                return _attributeTableBuilder.CreateTable();
            }
        }

        private static EditorAttribute CreateEditorAttribute<T>() where T : PropertyValueEditor
        {
            return PropertyValueEditor.CreateEditorAttribute(typeof(T));
        }
    }
}