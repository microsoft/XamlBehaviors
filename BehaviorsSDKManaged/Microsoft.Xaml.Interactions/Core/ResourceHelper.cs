// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
namespace Microsoft.Xaml.Interactions
{
    using Windows.ApplicationModel.Resources;
    using Interactivity;

    internal static class ResourceHelper
    {
        public static string GetString(string resourceName)
        {
            ResourceLoader strings = ResourceLoader.GetForCurrentView("Microsoft.Xaml.Interactions/Strings");
            return strings.GetString(resourceName);
        }

        public static string CallMethodActionValidMethodNotFoundExceptionMessage
        {
            get
            {
                return ResourceHelper.GetString("CallMethodActionValidMethodNotFoundExceptionMessage");
            }
        }

        public static string ChangePropertyActionCannotFindPropertyNameExceptionMessage
        {
            get
            {
                return ResourceHelper.GetString("ChangePropertyActionCannotFindPropertyNameExceptionMessage");
            }
        }

        public static string ChangePropertyActionCannotSetValueExceptionMessage
        {
            get
            {
                return ResourceHelper.GetString("ChangePropertyActionCannotSetValueExceptionMessage");
            }
        }

        public static string ChangePropertyActionPropertyIsReadOnlyExceptionMessage
        {
            get
            {
                return ResourceHelper.GetString("ChangePropertyActionPropertyIsReadOnlyExceptionMessage");
            }
        }

        public static string GoToStateActionTargetHasNoStateGroups
        {
            get
            {
                return ResourceHelper.GetString("GoToStateActionTargetHasNoStateGroups");
            }
        }

        public static string CannotAttachBehaviorMultipleTimesExceptionMessage
        {
            get
            {
                return ResourceHelper.GetString("CannotAttachBehaviorMultipleTimesExceptionMessage");
            }
        }

        public static string CannotFindEventNameExceptionMessage
        {
            get
            {
                return ResourceHelper.GetString("CannotFindEventNameExceptionMessage");
            }
        }

        public static string InvalidLeftOperand
        {
            get
            {
                return ResourceHelper.GetString("InvalidLeftOperand");
            }
        }

        public static string InvalidRightOperand
        {
            get
            {
                return ResourceHelper.GetString("InvalidRightOperand");
            }
        }

        public static string InvalidOperands
        {
            get
            {
                return ResourceHelper.GetString("InvalidOperands");
            }
        }
    }
}
