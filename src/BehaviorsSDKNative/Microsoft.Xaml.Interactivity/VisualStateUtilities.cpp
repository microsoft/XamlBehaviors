#include "pch.h"
#include "VisualStateUtilities.h"

namespace winrt
{
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::Foundation::Collections;
} // namespace winrt

namespace winrt::Microsoft::Xaml::Interactivity::implementation
{
bool VisualStateUtilities::GoToState(Control const &control, hstring const &stateName, bool useTransitions)
{
    if (control == nullptr)
    {
        throw hresult_invalid_argument(L"control");
    }

    if (stateName.empty())
    {
        throw hresult_invalid_argument(L"stateName");
    }

    control.ApplyTemplate();
    return VisualStateManager::GoToState(control, stateName, useTransitions);
}

IVector<VisualStateGroup> VisualStateUtilities::GetVisualStateGroups(FrameworkElement const &element)
{
    if (element == nullptr)
    {
        throw hresult_invalid_argument(L"element");
    }

    auto visualStateGroups = VisualStateManager::GetVisualStateGroups(element);

    if (visualStateGroups == nullptr || visualStateGroups.Size() == 0)
    {
        int childrenCount = VisualTreeHelper::GetChildrenCount(element);
        if (childrenCount > 0)
        {
            auto childElement = VisualTreeHelper::GetChild(element, 0).try_as<FrameworkElement>();
            if (childElement)
            {
                visualStateGroups = VisualStateManager::GetVisualStateGroups(childElement);
            }
        }
    }

    return visualStateGroups;
}

Control VisualStateUtilities::FindNearestStatefulControl(FrameworkElement const &element)
{
    if (element == nullptr)
    {
        throw hresult_invalid_argument(L"element");
    }

    FrameworkElement localElement = element;
    // Try to find an element which is the immediate child of a UserControl, ControlTemplate or other such "boundary" element
    auto parent = localElement.Parent().as<FrameworkElement>();

    // bubble up looking for a place to stop
    while (!VisualStateUtilities::HasVisualStateGroupsDefined(localElement) && VisualStateUtilities::ShouldContinueTreeWalk(parent))
    {
        localElement = parent;
        parent = localElement.Parent().as<FrameworkElement>();
    }

    if (VisualStateUtilities::HasVisualStateGroupsDefined(localElement))
    {
        // Once we've found such an element, use the VisualTreeHelper to get it's parent. For most elements the two are the
        // same, but for children of a ControlElement this will give the control that contains the template.
        auto templatedParent = VisualTreeHelper::GetParent(localElement).try_as<Control>();

        if (templatedParent)
        {
            return templatedParent;
        }
        else
        {
            return localElement.as<Control>();
        }
    }

    return nullptr;
}

bool VisualStateUtilities::HasVisualStateGroupsDefined(FrameworkElement const &frameworkElement)
{
    return frameworkElement != nullptr && VisualStateManager::GetVisualStateGroups(frameworkElement).Size() != 0;
}

bool VisualStateUtilities::ShouldContinueTreeWalk(FrameworkElement const &element)
{
    if (element == nullptr || element.try_as<UserControl>() != nullptr)
    {
        return false;
    }
    else if (element.Parent() == nullptr)
    {
        // Stop if parent's parent is null AND parent isn't the template root of a ControlTemplate or DataTemplate
        auto templatedParent = VisualTreeHelper::GetParent(element).try_as<FrameworkElement>();
        if (templatedParent == nullptr || (!(element.try_as<Control>() != nullptr) && !(element.try_as<ContentPresenter>() != nullptr)))
        {
            return false;
        }
    }

    return true;
}

} // namespace winrt::Microsoft::Xaml::Interactivity::implementation
