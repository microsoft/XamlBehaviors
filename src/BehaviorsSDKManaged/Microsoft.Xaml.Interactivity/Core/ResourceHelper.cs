// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Xaml.Interactivity;

using Windows.ApplicationModel.Resources;

internal static partial class ResourceHelper
{
    private static ResourceLoader Strings => ResourceLoader.GetForCurrentView("Microsoft.Xaml.Interactivity/Strings");
}