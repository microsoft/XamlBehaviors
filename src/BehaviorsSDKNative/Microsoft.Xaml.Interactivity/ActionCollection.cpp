// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
#include "pch.h"
#include "ActionCollection.h"
#include "IAction.h"
#include "ResourceHelper.h"

using namespace Platform;
using namespace Windows::UI::Xaml;
using namespace Windows::Foundation::Collections;
using namespace Microsoft::Xaml::Interactivity;

ActionCollection::ActionCollection()
{
	this->VectorChanged += ref new VectorChangedEventHandler<DependencyObject^>(this, &ActionCollection::OnVectorChanged);
}

void ActionCollection::OnVectorChanged(IObservableVector<DependencyObject^>^ sender, IVectorChangedEventArgs^ eventArgs)
{
	CollectionChange collectionChange = eventArgs->CollectionChange;

	if (collectionChange == CollectionChange::Reset)
	{
		for (const auto& item : this)
		{
			ActionCollection::VerifyType(item);
		}
	}
	else if (collectionChange == CollectionChange::ItemInserted || collectionChange == CollectionChange::ItemChanged)
	{
		auto changedItem = this->GetAt(eventArgs->Index);
		ActionCollection::VerifyType(changedItem);
	}
}

void ActionCollection::VerifyType(DependencyObject^ item)
{
	IAction^ action = dynamic_cast<IAction^>(item);

	if (action == nullptr)
	{
		throw ref new FailureException(ResourceHelper::GetString("NonActionAddedToActionCollectionExceptionMessage"));
	}
}