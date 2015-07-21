// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
#include "pch.h"
#include "BehaviorCollection.h"
#include "IBehavior.h"
#include "ResourceHelper.h"

using namespace Platform;
using namespace Platform::Collections;
using namespace Windows::UI::Xaml;
using namespace Microsoft::Xaml::Interactivity;
using namespace Windows::Foundation::Collections;

BehaviorCollection::BehaviorCollection()
{
	this->associatedObject = nullptr;
	this->VectorChanged += ref new VectorChangedEventHandler<DependencyObject^>(this, &BehaviorCollection::OnVectorChanged);
}

BehaviorCollection::~BehaviorCollection()
{
	this->Detach();
}

void BehaviorCollection::OnVectorChanged(IObservableVector<DependencyObject^>^ sender, IVectorChangedEventArgs^ eventArgs)
{
	if (eventArgs->CollectionChange == CollectionChange::Reset)
	{
		for (auto& behavior : this->oldCollection)
		{
			if (behavior->AssociatedObject != nullptr)
			{
				behavior->Detach();
			}
		}

		this->oldCollection.clear();
		this->oldCollection.reserve(this->Size);

		for (const auto& newItem : this)
		{
			this->oldCollection.push_back(this->VerifiedAttach(newItem));
		}

#if _DEBUG
		this->VerifyOldCollectionIntegrity();
#endif

		return;
	}

	unsigned int eventIndex = eventArgs->Index;
	DependencyObject^ changedItem = this->GetAt(eventIndex);

	switch (eventArgs->CollectionChange)
	{
	case CollectionChange::ItemInserted:
		{
			this->oldCollection.insert(oldCollection.begin() + eventIndex, this->VerifiedAttach(changedItem));
		}
		break;

	case CollectionChange::ItemChanged:
		{
			IBehavior^ oldItem = this->oldCollection[eventIndex];
			if (oldItem->AssociatedObject != nullptr)
			{
				oldItem->Detach();
			}

			this->oldCollection[eventIndex] = this->VerifiedAttach(changedItem);
		}
		break;

	case CollectionChange::ItemRemoved:
		{
			IBehavior^ oldItem = this->oldCollection[eventIndex];
			if (oldItem->AssociatedObject != nullptr)
			{
				oldItem->Detach();
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

void BehaviorCollection::Attach(DependencyObject^ associatedObject)
{
	if (associatedObject == this->AssociatedObject)
	{
		return;
	}

	if (Windows::ApplicationModel::DesignMode::DesignModeEnabled)
	{
		return;
	}

	if (this->AssociatedObject != nullptr)
	{
		throw ref new FailureException(ResourceHelper::GetString("CannotAttachBehaviorMultipleTimesExceptionMessage"));
	}

	_ASSERT(associatedObject != nullptr);
	this->associatedObject = associatedObject;

	for (DependencyObject^ item : this)
	{
		IBehavior^ behaviorItem = safe_cast<IBehavior^>(item);
		behaviorItem->Attach(this->AssociatedObject);
	}
}

void BehaviorCollection::Detach()
{
	for (auto& item : this->oldCollection)
	{
		if (item->AssociatedObject != nullptr)
		{
			item->Detach();
		}
	}

	this->oldCollection.clear();

	this->associatedObject = nullptr;
}

IBehavior^ BehaviorCollection::VerifiedAttach(DependencyObject^ item)
{
	IBehavior^ behavior = dynamic_cast<IBehavior^>(item);
	if (behavior == nullptr)
	{
		throw ref new FailureException(ResourceHelper::GetString("NonBehaviorAddedToBehaviorCollectionExceptionMessage"));
	}

	auto found = std::find(begin(this->oldCollection), end(this->oldCollection), behavior);
	if (found != this->oldCollection.end())
	{
		throw ref new FailureException(ResourceHelper::GetString("DuplicateBehaviorInCollectionExceptionMessage"));
	}

	if (this->AssociatedObject != nullptr)
	{
		behavior->Attach(this->AssociatedObject);
	}

	return behavior;
}

#if _DEBUG
void BehaviorCollection::VerifyOldCollectionIntegrity()
{
	bool isValid = (this->Size == this->oldCollection.size());
	if (isValid)
	{
		for (unsigned int i = 0; i < this->Size; i++)
		{
			if (safe_cast<IBehavior^>(this->GetAt(i)) != this->oldCollection[i])
			{
				isValid = false;
				break;
			}
		}
	}

	_ASSERT(isValid);
}
#endif