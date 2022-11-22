// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#endif

namespace Microsoft.Xaml.Interactions.Utility
{
    /// <summary>
    /// IVisualTreeHelper implementation that calls the real VisualTreeHelper.
    /// </summary>
    internal class UwpVisualTreeHelper : IVisualTreeHelper
    {
        #region IVisualTreeHelper implementation

        public DependencyObject GetParent(DependencyObject reference)
        {
            return VisualTreeHelper.GetParent(reference);
        }

        #endregion
    }
}
