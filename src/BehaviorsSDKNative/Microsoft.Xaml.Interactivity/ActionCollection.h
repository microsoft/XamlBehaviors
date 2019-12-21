// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
#pragma once
#include "ActionCollection.g.h"

namespace winrt::Microsoft::Xaml::Interactivity::implementation
{
/// <summary>
/// Represents a collection of IActions.
/// </summary>
struct ActionCollection : ActionCollectionT<ActionCollection>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ActionCollection"/> class.
    /// </summary>
    ActionCollection();

private:
    VectorChanged_revoker _vectorChanged;

    void OnVectorChanged(
        Windows::Foundation::Collections::IObservableVector<Windows::UI::Xaml::DependencyObject> const &sender,
        Windows::Foundation::Collections::IVectorChangedEventArgs const &event);

    static void VerifyType(Windows::UI::Xaml::DependencyObject const &item);
};
} // namespace winrt::Microsoft::Xaml::Interactivity::implementation
namespace winrt::Microsoft::Xaml::Interactivity::factory_implementation
{
struct ActionCollection : ActionCollectionT<ActionCollection, implementation::ActionCollection>
{
};
} // namespace winrt::Microsoft::Xaml::Interactivity::factory_implementation
