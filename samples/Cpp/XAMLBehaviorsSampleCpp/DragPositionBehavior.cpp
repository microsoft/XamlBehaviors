#pragma once

#include "pch.h"
#include "DragPositionBehavior.h"

using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Input;
using namespace Platform;
using namespace XAMLBehaviorsSampleCpp;

void DragPositionBehavior::Attach(DependencyObject^ associatedObject)
{
	if ((associatedObject != AssociatedObject) && !Windows::ApplicationModel::DesignMode::DesignModeEnabled)
	{
		this->associatedObject = associatedObject;
		FrameworkElement^ fe = dynamic_cast<FrameworkElement^>(AssociatedObject);
		if (fe != nullptr)
		{
			fe->PointerPressed += ref new PointerEventHandler(this, &DragPositionBehavior::fe_PointerPressed);
			fe->PointerReleased += ref new PointerEventHandler(this, &DragPositionBehavior::fe_PointerReleased);
		}
	}
}

void DragPositionBehavior::fe_PointerPressed(Object^ sender, PointerRoutedEventArgs^ e)
{
	FrameworkElement^ fe = dynamic_cast<FrameworkElement^>(AssociatedObject);
	parent = dynamic_cast<UIElement^>(fe->Parent);

	if (!(dynamic_cast<TranslateTransform^>(fe->RenderTransform) != nullptr))
		fe->RenderTransform = ref new TranslateTransform();
	prevPoint = e->GetCurrentPoint(parent)->Position;

	parent->PointerMoved += ref new PointerEventHandler(this, &DragPositionBehavior::move);

	pointerId = (int)e->Pointer->PointerId;
}

void DragPositionBehavior::move(Object^ o, PointerRoutedEventArgs^ args)
{
	if (args->Pointer->PointerId != pointerId)
		return;

	FrameworkElement^ fe = dynamic_cast<FrameworkElement^>(AssociatedObject);
	auto pos = args->GetCurrentPoint(parent)->Position;
	auto tr = dynamic_cast<TranslateTransform^>(fe->RenderTransform);
	tr->X += pos.X - prevPoint.X;
	tr->Y += pos.Y - prevPoint.Y;
	prevPoint = pos;
}

void DragPositionBehavior::fe_PointerReleased(Object^ sender, PointerRoutedEventArgs^ e)
{
	FrameworkElement^ fe = dynamic_cast<FrameworkElement^>(AssociatedObject);
	if (e->Pointer->PointerId != pointerId)
		return;
	//Remove Event Handler
	//parent->PointerMoved -= &DragPositionBehavior::move;
	pointerId = -1;
}
void DragPositionBehavior::Detach()
{
	FrameworkElement^ fe = dynamic_cast<FrameworkElement^>(AssociatedObject);
	if (fe != nullptr)
	{
		//Remove Event Handlers
		//fe->PointerPressed -= &DragPositionBehavior::fe_PointerPressed;
		//fe->PointerReleased -= &DragPositionBehavior::fe_PointerReleased;
	}
	parent = nullptr;
	this->associatedObject = nullptr;
}