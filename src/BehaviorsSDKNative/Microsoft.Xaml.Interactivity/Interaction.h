#pragma once
#include "Interaction.g.h"

namespace winrt::Microsoft::Xaml::Interactivity::implementation
{
/// <summary>
/// Defines a <see cref="BehaviorCollection"/> attached property and provides a method for executing an <seealso cref="ActionCollection"/>.
/// </summary>
struct Interaction : InteractionT<Interaction>
{
    /// <summary>
    /// Gets or sets the <see cref="BehaviorCollection"/> associated with a specified object.
    /// </summary>
    static Windows::UI::Xaml::DependencyProperty BehaviorsProperty();

    /// <summary>
    /// Gets the <see cref="BehaviorCollection"/> associated with a specified object.
    /// </summary>
    /// <param name="obj">The <see cref="Windows::UI::Xaml::DependencyObject"/> from which to retrieve the <see cref="BehaviorCollection"/>.</param>
    /// <returns>A <see cref="BehaviorCollection"/> containing the behaviors associated with the specified object.</returns>
    static Microsoft::Xaml::Interactivity::BehaviorCollection GetBehaviors(Windows::UI::Xaml::DependencyObject const &obj);

    /// <summary>
    /// Sets the <see cref="BehaviorCollection"/> associated with a specified object.
    /// </summary>
    /// <param name="obj">The <see cref="Windows::UI::Xaml::DependencyObject"/> on which to set the <see cref="BehaviorCollection"/>.</param>
    /// <param name="value">The <see cref="BehaviorCollection"/> associated with the object.</param>
    static void SetBehaviors(Windows::UI::Xaml::DependencyObject const &obj, Microsoft::Xaml::Interactivity::BehaviorCollection const &value);

    /// <summary>
    /// Executes all actions in the <see cref="ActionCollection"/> and returns their results.
    /// </summary>
    /// <param name="sender">The <see cref="Platform::Object"/> which will be passed on to the action.</param>
    /// <param name="actions">The set of actions to execute.</param>
    /// <param name="parameter">The value of this parameter is determined by the calling behavior.</param>
    /// <returns>Returns the results of the actions.</returns>
    static Windows::Foundation::Collections::IIterable<Windows::Foundation::IInspectable> ExecuteActions(Windows::Foundation::IInspectable const &sender, Microsoft::Xaml::Interactivity::ActionCollection const &actions, Windows::Foundation::IInspectable const &parameter);

    static void OnBehaviorsChanged(Windows::UI::Xaml::DependencyObject const& sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs const& args);
};
} // namespace winrt::Microsoft::Xaml::Interactivity::implementation
namespace winrt::Microsoft::Xaml::Interactivity::factory_implementation
{
struct Interaction : InteractionT<Interaction, implementation::Interaction>
{
};
} // namespace winrt::Microsoft::Xaml::Interactivity::factory_implementation
