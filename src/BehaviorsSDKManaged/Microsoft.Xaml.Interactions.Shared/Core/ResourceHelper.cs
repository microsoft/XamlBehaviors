// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace Microsoft.Xaml.Interactions.Core
{
    using Windows.ApplicationModel.Resources;
    using Interactivity;

    internal static class ResourceHelper
    {
#if NET5_0
        private static ResourceLoader strings = new ResourceLoader(ResourceLoader.GetDefaultResourceFilePath(), "Microsoft.Xaml.Interactions/Strings");
#endif

        public static string GetString(string resourceName)
        {
#if !NET5_0 
            var strings = ResourceLoader.GetForCurrentView("Microsoft.Xaml.Interactions/Strings");
#endif
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
