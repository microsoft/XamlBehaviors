// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
#pragma once
#include "BehaviorCollection.g.h"

namespace winrt::Microsoft::Xaml::Interactivity::implementation
{
/// <summary>
/// Represents a collection of IBehaviors with a shared <see cref="Microsoft::Xaml::Interactivity::BehaviorCollection::AssociatedObject"/>.
/// </summary>
struct BehaviorCollection : BehaviorCollectionT<BehaviorCollection>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BehaviorCollection"/> class.
    /// </summary>
    BehaviorCollection();
    ~BehaviorCollection();

    /// <summary>
    /// Gets the <see cref="Windows::UI::Xaml::DependencyObject"/> to which the <see cref="BehaviorCollection"/> is attached.
    /// </summary>
    Windows::UI::Xaml::DependencyObject AssociatedObject();

    /// <summary>
    /// Attaches the collection of behaviors to the specified <see cref="Windows::UI::Xaml::DependencyObject"/>.
    /// </summary>
    /// <param name="associatedObject">The <see cref="Windows::UI::Xaml::DependencyObject"/> to which to attach.</param>
    /// <exception cref="Platform::FailureException">The <see cref="BehaviorCollection"/> is already attached to a different <see cref="Windows::UI::Xaml::DependencyObject"/>.</exception>
    void Attach(Windows::UI::Xaml::DependencyObject const &associatedObject);

    /// <summary>
    /// Detaches the collection of behaviors from the <see cref="Microsoft::Xaml::Interactivity::BehaviorCollection::AssociatedObject"/>.
    /// </summary>
    void Detach();

private:
    // After a VectorChanged event we need to compare the current state of the collection
    // with the old collection so that we can call Detach on all removed items.
    std::vector<IBehavior> oldCollection;
    winrt::weak_ref<Windows::UI::Xaml::DependencyObject> associatedObject;

    VectorChanged_revoker _vectorChanged;

    void OnVectorChanged(
        Windows::Foundation::Collections::IObservableVector<Windows::UI::Xaml::DependencyObject> const &sender,
        Windows::Foundation::Collections::IVectorChangedEventArgs const &event);

    Microsoft::Xaml::Interactivity::IBehavior VerifiedAttach(Windows::UI::Xaml::DependencyObject const& item);

#if _DEBUG
    void VerifyOldCollectionIntegrity();
#endif
};
} // namespace winrt::Microsoft::Xaml::Interactivity::implementation
namespace winrt::Microsoft::Xaml::Interactivity::factory_implementation
{
struct BehaviorCollection : BehaviorCollectionT<BehaviorCollection, implementation::BehaviorCollection>
{
};
} // namespace winrt::Microsoft::Xaml::Interactivity::factory_implementation
