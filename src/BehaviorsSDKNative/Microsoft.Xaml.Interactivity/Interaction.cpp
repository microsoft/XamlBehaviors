#include "pch.h"
#include "Interaction.h"
#include "BehaviorCollection.h"

namespace winrt
{
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::UI::Xaml;
using namespace Microsoft::Xaml::Interactivity;
} // namespace winrt

namespace winrt::Microsoft::Xaml::Interactivity::implementation
{
namespace DependencyProperties
{
static DependencyProperty Behaviors = DependencyProperty::RegisterAttached(
    L"Behaviors",
    xaml_typename<Microsoft::Xaml::Interactivity::BehaviorCollection>(),
    xaml_typename<Microsoft::Xaml::Interactivity::Interaction>(),
    PropertyMetadata(nullptr, {&Interaction::OnBehaviorsChanged}));
}

void Interaction::OnBehaviorsChanged(DependencyObject const &sender, DependencyPropertyChangedEventArgs const &args)
{
  auto oldCollection = args.OldValue().as<Microsoft::Xaml::Interactivity::BehaviorCollection>();
  auto newCollection = args.NewValue().as<Microsoft::Xaml::Interactivity::BehaviorCollection>();

    if (oldCollection == newCollection)
    {
        return;
    }

    if (oldCollection != nullptr && oldCollection.AssociatedObject() != nullptr)
    {
        oldCollection.Detach();
    }

    if (newCollection != nullptr && sender != nullptr)
    {
        newCollection.Attach(sender);
    }
}

DependencyProperty Interaction::BehaviorsProperty()
{
    return DependencyProperties::Behaviors;
}

IIterable<IInspectable> Interaction::ExecuteActions(IInspectable const &sender, ActionCollection const &actions, IInspectable const &parameter)
{
    IVector<IInspectable> results = winrt::single_threaded_vector<IInspectable>();

    if (actions == nullptr || Windows::ApplicationModel::DesignMode::DesignModeEnabled())
    {
        return results;
    }

    for (auto &dependencyObject : actions)
    {
        auto action = dependencyObject.as<IAction>();
        results.Append(action.Execute(sender, parameter));
    }

    return results;
}

Microsoft::Xaml::Interactivity::BehaviorCollection Interaction::GetBehaviors(DependencyObject const &obj)
{
    if (obj == nullptr)
    {
        throw winrt::hresult_invalid_argument(L"obj");
    }

    auto behaviors = obj.GetValue(Interaction::BehaviorsProperty()).as<Microsoft::Xaml::Interactivity::BehaviorCollection>();
    if (behaviors == nullptr)
    {
        behaviors = make<BehaviorCollection>();
        Interaction::SetBehaviors(obj, behaviors);
    }

    return behaviors;
}

void Interaction::SetBehaviors(DependencyObject const &obj, Microsoft::Xaml::Interactivity::BehaviorCollection const &value)
{
    if (obj == nullptr)
    {
        throw winrt::hresult_invalid_argument(L"obj");
    }

    obj.SetValue(Interaction::BehaviorsProperty(), value);
}

} // namespace winrt::Microsoft::Xaml::Interactivity::implementation
