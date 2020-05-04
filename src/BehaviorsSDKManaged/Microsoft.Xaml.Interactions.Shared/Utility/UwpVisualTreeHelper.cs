// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace Microsoft.Xaml.Interactions.Utility
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media;

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
