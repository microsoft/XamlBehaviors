// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
#pragma once

namespace Microsoft { namespace Xaml { namespace Interactions { namespace Core
{
	/// <summary>
	/// A behavior that allows incremental updating of <seealso cref="Windows::UI::Xaml::Controls::ListView"/> and <seealso cref="Windows::UI::Xaml::Controls::GridView"/> contents to support faster updating.
	/// By attaching this behavior to elements in the <seealso cref="Windows::UI::Xaml::Controls::ItemsControl::ItemTemplate"/> used by these views, some of the updates can be deferred until there is render time available, resulting in a smoother experience.
	/// </summary>
	[::Windows::UI::Xaml::Data::Bindable]
	[::Windows::Foundation::Metadata::WebHostHidden]
	public ref class IncrementalUpdateBehavior sealed : ::Windows::UI::Xaml::DependencyObject, ::Microsoft::Xaml::Interactivity::IBehavior
	{
	internal:
		[::Windows::Foundation::Metadata::WebHostHidden]
		ref class IncrementalUpdater sealed
		{
		public:
			void CachePhaseElement(::Windows::UI::Xaml::FrameworkElement^ phaseElement, int phase);
			void UncachePhaseElement(::Windows::UI::Xaml::FrameworkElement^ phaseElement, int phase);
			void Attach(::Windows::UI::Xaml::DependencyObject^ dependencyObject);
			void Detach();

		private:
			class UIElementComparator
			{
			public:
				bool operator()(const ::Windows::UI::Xaml::UIElement^ x, const ::Windows::UI::Xaml::UIElement^ y) { return reinterpret_cast<const void*>(x) < reinterpret_cast<const void*>(y); }
			};

			class PhasedElementRecord
			{
			private:
				::Platform::Object^ _localOpacity;
				::Platform::Object^ _localDataContext;
				bool _isFrozen;

			public:
				::Windows::UI::Xaml::FrameworkElement^ _frameworkElement;

				PhasedElementRecord(::Windows::UI::Xaml::FrameworkElement^ frameworkElement) { this->_isFrozen = false; this->_frameworkElement = frameworkElement; }
				void FreezeAndHide();
				void ThawAndShow();
			};

			std::map<::Windows::UI::Xaml::UIElement^, std::map<int, std::vector<PhasedElementRecord>>, IncrementalUpdater::UIElementComparator> _elementCache;

			static ::Windows::UI::Xaml::UIElement^ FindContentTemplateRoot(::Windows::UI::Xaml::FrameworkElement^ phaseElement);

			::Windows::Foundation::EventRegistrationToken _containerContentChangingToken;
			::Windows::UI::Xaml::Controls::ListViewBase^ _listViewBase;

			void OnContainerContentChanging(::Windows::UI::Xaml::Controls::ListViewBase^ sender, ::Windows::UI::Xaml::Controls::ContainerContentChangingEventArgs^ e);
			void OnContainerContentChangingCallback(Windows::UI::Xaml::Controls::ListViewBase^ sender, ::Windows::UI::Xaml::Controls::ContainerContentChangingEventArgs^ e);
			::Windows::Foundation::TypedEventHandler<::Platform::Object^, ::Windows::UI::Xaml::RoutedEventArgs^>^ _unloadedHandler;
			::Windows::Foundation::TypedEventHandler<::Windows::UI::Xaml::Controls::ListViewBase^, ::Windows::UI::Xaml::Controls::ContainerContentChangingEventArgs^>^ _cccHandler;
			::Windows::Foundation::TypedEventHandler<::Windows::UI::Xaml::Controls::ListViewBase^, ::Windows::UI::Xaml::Controls::ContainerContentChangingEventArgs^>^ _cccCallbackHandler;
		};

		static void OnPhaseChanged(::Windows::UI::Xaml::DependencyObject^ sender, ::Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ args);

		static void OnIncrementalUpdaterChanged(::Windows::UI::Xaml::DependencyObject^ sender, ::Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ args);

	private:
		::Windows::UI::Xaml::DependencyObject^ _associatedObject;
		::Windows::Foundation::EventRegistrationToken _loadedToken;
		::Windows::Foundation::EventRegistrationToken _unloadedToken;

		static property::Windows::UI::Xaml::DependencyProperty^ IncrementalUpdaterProperty
		{
			::Windows::UI::Xaml::DependencyProperty^ get();
		}

		static IncrementalUpdater^ GetIncrementalUpdater(::Windows::UI::Xaml::DependencyObject^ obj)
		{
			return safe_cast<IncrementalUpdater^>(obj->GetValue(IncrementalUpdateBehavior::IncrementalUpdaterProperty));
		}

		static void SetIncrementalUpdater(::Windows::UI::Xaml::DependencyObject^ obj, IncrementalUpdater^ value)
		{
			obj->SetValue(IncrementalUpdateBehavior::IncrementalUpdaterProperty, value);
		}

		void OnAssociatedObjectLoaded(::Platform::Object^ sender, ::Windows::UI::Xaml::RoutedEventArgs^ e);
		void OnAssociatedObjectUnloaded(::Platform::Object^ sender, ::Windows::UI::Xaml::RoutedEventArgs^ e);

		IncrementalUpdater^ _updater;
		IncrementalUpdater^ FindUpdater();

	public:
		/// <summary>
		/// Gets the <see cref="Windows::UI::Xaml::DependencyObject"/> to which the <seealso cref="::Microsoft::Xaml::Interactivity::IBehavior"/> is attached.
		/// </summary>
		virtual property ::Windows::UI::Xaml::DependencyObject^ AssociatedObject
		{
			::Windows::UI::Xaml::DependencyObject^ get()
			{
				return this->_associatedObject;
			}
		}

		/// <summary>
		/// Attaches to the specified object.
		/// </summary>
		/// <param name="associatedObject">The <see cref="Windows::UI::Xaml::DependencyObject"/> to which the <seealso cref="::Microsoft::Xaml::Interactivity::IBehavior"/> will be attached.</param>
		virtual void Attach(::Windows::UI::Xaml::DependencyObject^ associatedObject);

		/// <summary>
		/// Detaches this instance from its associated object.
		/// </summary>
		virtual void Detach();

		/// <summary>
		/// Identifies the <see cref="Phase"/> dependency property.
		/// </summary>
		static property ::Windows::UI::Xaml::DependencyProperty^ PhaseProperty
		{
			::Windows::UI::Xaml::DependencyProperty^ get();
		}

		/// <summary>
		/// Gets or sets the relative priority of this incremental update. Lower Phase values are addressed first.
		/// </summary>
		property int Phase
		{
			int get()
			{
				return safe_cast<int>(this->GetValue(IncrementalUpdateBehavior::PhaseProperty));
			}

			void set(int value)
			{
				this->SetValue(IncrementalUpdateBehavior::PhaseProperty, value);
			}
		}
	};

}}}}