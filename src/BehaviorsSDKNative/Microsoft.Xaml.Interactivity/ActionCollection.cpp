// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
#include "pch.h"
#include "ActionCollection.h"

namespace winrt
{
using namespace Windows::Foundation::Collections;
using namespace Windows::UI::Xaml;
} // namespace winrt

namespace winrt::Microsoft::Xaml::Interactivity::implementation
{
ActionCollection::ActionCollection()
{
    _vectorChanged = VectorChanged(auto_revoke, {get_weak(), &ActionCollection::OnVectorChanged});
}

void ActionCollection::OnVectorChanged(IObservableVector<DependencyObject> const &, IVectorChangedEventArgs const &event)
{
    auto collectionChange = event.CollectionChange();

    if (collectionChange == CollectionChange::Reset)
    {
        for (auto const &item : *this)
        {
            ActionCollection::VerifyType(item);
        }
    }
    else if (collectionChange == CollectionChange::ItemInserted || collectionChange == CollectionChange::ItemChanged)
    {
        auto changedItem = GetAt(event.Index());
        ActionCollection::VerifyType(changedItem);
    }
}

void ActionCollection::VerifyType(DependencyObject const &item)
{
    auto type = item.try_as<IAction>();
    if (type == nullptr)
    {
        // ResourceHelper::GetString("NonActionAddedToActionCollectionExceptionMessage")
        throw winrt::hresult_invalid_argument(L"NonActionAddedToActionCollectionExceptionMessage");
    }
}
} // namespace winrt::Microsoft::Xaml::Interactivity::implementation
