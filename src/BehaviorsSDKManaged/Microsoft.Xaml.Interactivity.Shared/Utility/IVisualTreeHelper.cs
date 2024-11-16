// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Microsoft.Xaml.Interactions.Utility
{
    /// <summary>
    /// Abstraction layer over the UWP's VisualTreeHelper class so we can 
    /// mock it for unit testing purposes where an actual view won't be available.
    /// </summary>
    internal interface IVisualTreeHelper
    {
        /// <summary>
        /// Returns an object's parent object in the visual tree.
        /// </summary>
        /// <param name="reference">
        /// The object for which to get the parent object.
        /// </param>
        /// <returns>
        /// The parent object of the reference object in the visual tree. 
        /// </returns>
        /// <remarks>
        /// THREAD SAFETY: This method should be called on the object's Dispatcher thread.
        /// </remarks>
        DependencyObject GetParent(DependencyObject reference);
    }
}
