// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
#include "pch.h"
#include "BehaviorCollection.h"

namespace winrt
{
using namespace Windows::ApplicationModel;
using namespace Windows::Foundation::Collections;
using namespace Windows::UI::Xaml;
} // namespace winrt

namespace winrt::Microsoft::Xaml::Interactivity::implementation
{
BehaviorCollection::BehaviorCollection() : associatedObject(nullptr)
{
    _vectorChanged = VectorChanged(auto_revoke, {get_weak(), &BehaviorCollection::OnVectorChanged});
}

BehaviorCollection::~BehaviorCollection()
{
    Detach();
}

void BehaviorCollection::OnVectorChanged(IObservableVector<DependencyObject> const &sender, IVectorChangedEventArgs const &eventArgs)
{
    if (eventArgs.CollectionChange() == CollectionChange::Reset)
    {
        for (auto &behavior : this->oldCollection)
        {
            if (behavior.AssociatedObject() != nullptr)
            {
                behavior.Detach();
            }
        }

        this->oldCollection.clear();
        this->oldCollection.reserve(this->Size());

        for (const auto &newItem : *this)
        {
            this->oldCollection.push_back(VerifiedAttach(newItem));
        }

#if _DEBUG
        this->VerifyOldCollectionIntegrity();
#endif

        return;
    }

    unsigned int eventIndex = eventArgs.Index();
    auto changedItem = this->GetAt(eventIndex);

    switch (eventArgs.CollectionChange())
    {
    case CollectionChange::ItemInserted:
    {
        this->oldCollection.insert(oldCollection.begin() + eventIndex, this->VerifiedAttach(changedItem));
    }
    break;

    case CollectionChange::ItemChanged:
    {
        auto oldItem = this->oldCollection[eventIndex];
        if (oldItem.AssociatedObject() != nullptr)
        {
            oldItem.Detach();
        }

        this->oldCollection[eventIndex] = this->VerifiedAttach(changedItem);
    }
    break;

    case CollectionChange::ItemRemoved:
    {
        auto oldItem = this->oldCollection[eventIndex];
        if (oldItem.AssociatedObject() != nullptr)
        {
            oldItem.Detach();
        }

        this->oldCollection.erase(oldCollection.begin() + eventIndex);
    }
    break;

    default:
        _ASSERT(false);
        break;
    }

#if _DEBUG
    this->VerifyOldCollectionIntegrity();
#endif
}

DependencyObject BehaviorCollection::AssociatedObject()
{
    return associatedObject.get();
}

void BehaviorCollection::Attach(DependencyObject const &associatedObject)
{
    if (associatedObject == AssociatedObject())
    {
        return;
    }

    if (DesignMode::DesignModeEnabled())
    {
        return;
    }

    if (AssociatedObject() != nullptr)
    {
        //ResourceHelper::GetString("CannotAttachBehaviorMultipleTimesExceptionMessage")
        throw winrt::hresult_invalid_argument(L"CannotAttachBehaviorMultipleTimesExceptionMessage");
    }

    _ASSERT(associatedObject != nullptr);
    this->associatedObject = associatedObject;

    for (auto const &item : *this)
    {
        auto behaviorItem = item.as<IBehavior>();
        behaviorItem.Attach(associatedObject);
    }
}
void BehaviorCollection::Detach()
{
    for (auto& item : this->oldCollection)
    {
        if (item.AssociatedObject() != nullptr)
        {
            item.Detach();
        }
    }

    this->oldCollection.clear();

    this->associatedObject = nullptr;
}

IBehavior BehaviorCollection::VerifiedAttach(DependencyObject const& item)
{
    auto behavior = item.as<IBehavior>();
    if (behavior == nullptr)
    {
      // ResourceHelper::GetString("NonBehaviorAddedToBehaviorCollectionExceptionMessage")
      throw winrt::hresult_invalid_argument(L"NonBehaviorAddedToBehaviorCollectionExceptionMessage");
    }

    auto found = std::find(begin(this->oldCollection), end(this->oldCollection), behavior);
    if (found != this->oldCollection.end())
    {
      // ResourceHelper::GetString("DuplicateBehaviorInCollectionExceptionMessage")
      throw winrt::hresult_invalid_argument(L"DuplicateBehaviorInCollectionExceptionMessage");
    }

    if (this->AssociatedObject() != nullptr)
    {
        behavior.Attach(this->AssociatedObject());
    }

    return behavior;
}

#if _DEBUG
void BehaviorCollection::VerifyOldCollectionIntegrity()
{
    bool isValid = (this->Size() == this->oldCollection.size());
    if (isValid)
    {
        for (unsigned int i = 0; i < this->Size(); i++)
        {
            if (GetAt(i).as<IBehavior>() != this->oldCollection[i])
            {
                isValid = false;
                break;
            }
        }
    }

    _ASSERT(isValid);
}
#endif

} // namespace winrt::Microsoft::Xaml::Interactivity::implementation
