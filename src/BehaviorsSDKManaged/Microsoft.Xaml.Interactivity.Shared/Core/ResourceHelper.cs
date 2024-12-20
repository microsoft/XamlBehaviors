﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Xaml.Interactivity;

using Windows.ApplicationModel.Resources;

internal static class ResourceHelper
{
#if NET8_0_OR_GREATER && !MODERN_WINDOWS_UWP
    private static ResourceLoader strings = new ResourceLoader(ResourceLoader.GetDefaultResourceFilePath(), "Microsoft.Xaml.Interactivity/Strings");
#endif

    public static string GetString(string resourceName)
    {
#if !NET8_0_OR_GREATER || MODERN_WINDOWS_UWP
        ResourceLoader strings = ResourceLoader.GetForCurrentView("Microsoft.Xaml.Interactivity/Strings");
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
