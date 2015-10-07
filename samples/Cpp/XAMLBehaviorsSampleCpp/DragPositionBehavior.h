#pragma once

namespace XAMLBehaviorsSampleCpp
{
	using namespace Microsoft::Xaml::Interactivity;

	public ref class DragPositionBehavior sealed : Windows::UI::Xaml::DependencyObject, IBehavior
	{
		private:
			DependencyObject^ associatedObject;
			Windows::UI::Xaml::UIElement^ parent = nullptr;
			Windows::Foundation::Point prevPoint;
			int pointerId = -1;

			void fe_PointerPressed(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e);
			void fe_PointerReleased(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e);

		public:
			virtual property DependencyObject^ AssociatedObject
			{
				DependencyObject^ get()
				{
					return this->associatedObject;
				}
			}
			virtual void Attach(DependencyObject^ associatedObject);
			virtual void Detach();

			void move(Platform::Object^ o, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ args);
	};
}