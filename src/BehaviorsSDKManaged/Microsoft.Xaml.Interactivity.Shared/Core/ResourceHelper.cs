// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Xaml.Interactivity;

internal static partial class ResourceHelper
{
    public static string CallMethodActionValidMethodNotFoundExceptionMessage
        => Strings.GetString(nameof(CallMethodActionValidMethodNotFoundExceptionMessage));

    public static string ChangePropertyActionCannotFindPropertyNameExceptionMessage
        => Strings.GetString(nameof(ChangePropertyActionCannotFindPropertyNameExceptionMessage));

    public static string ChangePropertyActionCannotSetValueExceptionMessage
        => Strings.GetString(nameof(ChangePropertyActionCannotSetValueExceptionMessage));

    public static string ChangePropertyActionPropertyIsReadOnlyExceptionMessage
        => Strings.GetString(nameof(ChangePropertyActionPropertyIsReadOnlyExceptionMessage));

    public static string GoToStateActionTargetHasNoStateGroups
        => Strings.GetString(nameof(GoToStateActionTargetHasNoStateGroups));

    public static string CannotAttachBehaviorMultipleTimesExceptionMessage
        => Strings.GetString(nameof(CannotAttachBehaviorMultipleTimesExceptionMessage));

    public static string CannotFindEventNameExceptionMessage
        => Strings.GetString(nameof(CannotFindEventNameExceptionMessage));

    public static string InvalidAssociatedObjectExceptionMessage
        => Strings.GetString(nameof(InvalidAssociatedObjectExceptionMessage));

    public static string NonActionAddedToActionCollectionExceptionMessage
        => Strings.GetString(nameof(NonActionAddedToActionCollectionExceptionMessage));

    public static string NonBehaviorAddedToBehaviorCollectionExceptionMessage
        => Strings.GetString(nameof(NonBehaviorAddedToBehaviorCollectionExceptionMessage));

    public static string DuplicateBehaviorInCollectionExceptionMessage
        => Strings.GetString(nameof(DuplicateBehaviorInCollectionExceptionMessage));

    public static string InvalidLeftOperand
        => Strings.GetString(nameof(InvalidLeftOperand));

    public static string InvalidRightOperand
        => Strings.GetString(nameof(InvalidRightOperand));

    public static string InvalidOperands
        => Strings.GetString(nameof(InvalidOperands));
}
