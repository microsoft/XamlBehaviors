// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#endif

namespace Microsoft.Xaml.Interactivity;

/// <summary>
/// A behavior that listens to an <see cref="FrameworkElement"/> and executes its actions when the element is loaded.
/// </summary>
public sealed class LoadedTriggerBehavior : EventTriggerBehaviorBase<FrameworkElement>
{
    /// <inheritdoc/>
    protected override bool RegisterEventCore(FrameworkElement source)
    {
        if (!IsElementLoaded(source))
        {
            source.Loaded += OnEvent;
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <inheritdoc/>
    protected override void UnregisterEventCore(FrameworkElement source)
    {
        source.Loaded -= OnEvent;
    }

    internal static bool IsElementLoaded(FrameworkElement element)
    {
        if (element == null)
        {
            return false;
        }

        UIElement rootVisual = default;
        if (element.XamlRoot != null)
        {
            rootVisual = element.XamlRoot.Content;
        }
        else if (Window.Current != null)
        {
            rootVisual = Window.Current.Content;
        }

        DependencyObject parent = element.Parent;
        if (parent == null)
        {
            // If the element is the child of a ControlTemplate it will have a null parent even when it is loaded.
            // To catch that scenario, also check it's parent in the visual tree.
            parent = VisualTreeHelper.GetParent(element);
        }

        return (parent != null || (rootVisual != null && element == rootVisual));
    }
}
