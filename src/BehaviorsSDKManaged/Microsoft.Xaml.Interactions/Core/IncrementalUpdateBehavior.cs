// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace Microsoft.Xaml.Interactions.Core
{
    using System;
    using System.Collections.Generic;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Controls.Primitives;
    using Windows.UI.Xaml.Media;
    using Interactivity;

    /// <summary>
    /// A behavior that allows incremental updating of <seealso cref="Windows.UI.Xaml.Controls.ListView"/> and <seealso cref="Windows.UI.Xaml.Controls.GridView"/> contents to support faster updating.
    /// By attaching this behavior to elements in the <seealso cref="Windows.UI.Xaml.Controls.ItemsControl.ItemTemplate"/> used by these views, some of the updates can be deferred until there is render time available, resulting in a smoother experience.
    /// </summary>
    public sealed partial class IncrementalUpdateBehavior : Behavior<FrameworkElement>
    {
        /// <summary>
        /// Identifies the <seealso cref="Phase"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty PhaseProperty = DependencyProperty.Register(
            "Phase",
            typeof(int),
            typeof(IncrementalUpdateBehavior),
            new PropertyMetadata((int)1, new PropertyChangedCallback(IncrementalUpdateBehavior.OnPhaseChanged)));

        /// <summary>
        /// Identifies the <seealso cref="IncrementalUpdater"/> dependency property.
        /// </summary>
        private static readonly DependencyProperty IncrementalUpdaterProperty = DependencyProperty.RegisterAttached(
            "IncrementalUpdater",
            typeof(IncrementalUpdater),
            typeof(IncrementalUpdateBehavior),
            new PropertyMetadata(null, new PropertyChangedCallback(IncrementalUpdateBehavior.OnIncrementalUpdaterChanged)));

        private IncrementalUpdater updater = null;

        /// <summary>
        /// Gets or sets the relative priority of this incremental update. Lower Phase values are addressed first.
        /// </summary>
        public int Phase
        {
            get { return (int)this.GetValue(IncrementalUpdateBehavior.PhaseProperty); }
            set { this.SetValue(IncrementalUpdateBehavior.PhaseProperty, value); }
        }

        private static void OnPhaseChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            IncrementalUpdateBehavior behavior = (IncrementalUpdateBehavior)sender;
            IncrementalUpdater incrementalUpdater = behavior.FindUpdater();
            FrameworkElement frameworkElement = behavior.AssociatedObject;

            if (incrementalUpdater != null && frameworkElement != null)
            {
                incrementalUpdater.UncachePhaseElement(frameworkElement, (int)args.OldValue);
                incrementalUpdater.CachePhaseElement(frameworkElement, (int)args.NewValue);
            }
        }

        private static IncrementalUpdater GetIncrementalUpdater(DependencyObject dependencyObject)
        {
            return (IncrementalUpdater)dependencyObject.GetValue(IncrementalUpdateBehavior.IncrementalUpdaterProperty);
        }

        private static void SetIncrementalUpdater(DependencyObject dependencyObject, IncrementalUpdater value)
        {
            dependencyObject.SetValue(IncrementalUpdateBehavior.IncrementalUpdaterProperty, value);
        }

        private static void OnIncrementalUpdaterChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (args.OldValue != null)
            {
                IncrementalUpdater incrementalUpdater = (IncrementalUpdater)args.OldValue;
                incrementalUpdater.Detach();
            }
            if (args.NewValue != null)
            {
                IncrementalUpdater incrementalUpdater = (IncrementalUpdater)args.NewValue;
                incrementalUpdater.Attach(sender);
            }
        }

        private void OnAssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            IncrementalUpdater incrementalUpdater = this.FindUpdater();

            if (incrementalUpdater != null)
            {
                incrementalUpdater.CachePhaseElement(this.AssociatedObject, this.Phase);
            }
        }

        private void OnAssociatedObjectUnloaded(object sender, RoutedEventArgs e)
        {
            if (this.updater != null)
            {
                this.updater.UncachePhaseElement(this.AssociatedObject, this.Phase);
            }

            this.updater = null;
        }

        private IncrementalUpdater FindUpdater()
        {
            if (this.updater != null)
            {
                return this.updater;
            }

            DependencyObject ancestor = this.AssociatedObject;
            while (ancestor != null)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(ancestor);
                ListViewBase listView = parent as ListViewBase;

                if (listView != null)
                {
                    IncrementalUpdater currentUpdater = IncrementalUpdateBehavior.GetIncrementalUpdater(listView);
                    if (currentUpdater == null)
                    {
                        currentUpdater = new IncrementalUpdater();

                        IncrementalUpdateBehavior.SetIncrementalUpdater(listView, currentUpdater);
                    }
                    return currentUpdater;
                }
                ancestor = parent;
            }

            return null;
        }

        /// <summary>
        /// Called after the behavior is attached to the <see cref="Microsoft.Xaml.Interactivity.Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.Loaded += this.OnAssociatedObjectLoaded;
            this.AssociatedObject.Unloaded += this.OnAssociatedObjectUnloaded;
        }

        /// <summary>
        /// Called when the behavior is being detached from its <see cref="Microsoft.Xaml.Interactivity.Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.Loaded -= this.OnAssociatedObjectLoaded;
            this.AssociatedObject.Unloaded -= this.OnAssociatedObjectUnloaded;
            // no need to perform the work that Unloaded would have done - that's just housekeeping on the cache, which is now going away
        }

        private class IncrementalUpdater
        {
            private ListViewBase associatedListViewBase = null;
            private Dictionary<UIElement, ElementCacheRecord> elementCache = new Dictionary<UIElement, ElementCacheRecord>();

            private class PhasedElementRecord
            {
                private readonly FrameworkElement frameworkElement;
                private object localOpacity;
                private object localDataContext;
                private bool isFrozen;

                public PhasedElementRecord(FrameworkElement frameworkElement)
                {
                    this.frameworkElement = frameworkElement;
                }

                public FrameworkElement FrameworkElement { get { return this.frameworkElement; } }

                public void FreezeAndHide()
                {
                    if (this.isFrozen)
                    {
                        return;
                    }

                    this.isFrozen = true;
                    this.localOpacity = this.frameworkElement.ReadLocalValue(FrameworkElement.OpacityProperty);
                    this.localDataContext = this.frameworkElement.ReadLocalValue(FrameworkElement.DataContextProperty);
                    this.frameworkElement.Opacity = 0.0;
                    this.frameworkElement.DataContext = this.frameworkElement.DataContext;
                }

                public void ThawAndShow()
                {
                    if (!this.isFrozen)
                    {
                        return;
                    }

                    if (this.localOpacity != DependencyProperty.UnsetValue)
                    {
                        this.frameworkElement.SetValue(FrameworkElement.OpacityProperty, this.localOpacity);
                    }
                    else
                    {
                        this.frameworkElement.ClearValue(FrameworkElement.OpacityProperty);
                    }

                    if (this.localDataContext != DependencyProperty.UnsetValue)
                    {
                        this.frameworkElement.SetValue(FrameworkElement.DataContextProperty, this.localDataContext);
                    }
                    else
                    {
                        this.frameworkElement.ClearValue(FrameworkElement.DataContextProperty);
                    }

                    this.isFrozen = false;
                }
            }

            private class ElementCacheRecord
            {
                private List<int> phases = new List<int>();
                private List<List<PhasedElementRecord>> elementsByPhase = new List<List<PhasedElementRecord>>();

                public List<int> Phases
                {
                    get { return this.phases; }
                }

                public List<List<PhasedElementRecord>> ElementsByPhase
                {
                    get { return this.elementsByPhase; }
                }
            }

            private void OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs e)
            {
#if HAS_UNO
				UIElement contentTemplateRoot = e.ItemContainer.ContentTemplateRoot as UIElement /* UNO TODO */;
#else
				UIElement contentTemplateRoot = e.ItemContainer.ContentTemplateRoot;
#endif

				ElementCacheRecord elementCacheRecord;
                if (this.elementCache.TryGetValue(contentTemplateRoot, out elementCacheRecord))
                {
                    if (!e.InRecycleQueue)
                    {
                        foreach (List<PhasedElementRecord> phaseRecord in elementCacheRecord.ElementsByPhase)
                        {
                            foreach (PhasedElementRecord phasedElementRecord in phaseRecord)
                            {
                                phasedElementRecord.FreezeAndHide();
                            }
                        }

                        if (elementCacheRecord.Phases.Count > 0)
                        {
                            e.RegisterUpdateCallback((uint)elementCacheRecord.Phases[0], this.OnContainerContentChangingCallback);
                        }

                        // set the DataContext manually since we inhibit default operation by setting e.Handled=true
                        ((FrameworkElement)contentTemplateRoot).DataContext = e.Item;
                    }
                    else
                    {
                        // clear the DataContext manually since we inhibit default operation by setting e.Handled=true
                        contentTemplateRoot.ClearValue(FrameworkElement.DataContextProperty);

                        foreach (List<PhasedElementRecord> phaseRecord in elementCacheRecord.ElementsByPhase)
                        {
                            foreach (PhasedElementRecord phasedElementRecord in phaseRecord)
                            {
                                phasedElementRecord.ThawAndShow();
                            }
                        }
                    }
                }
            }

            private void OnContainerContentChangingCallback(ListViewBase sender, ContainerContentChangingEventArgs e)
            {
#if HAS_UNO
                UIElement contentTemplateRoot = e.ItemContainer.ContentTemplateRoot as UIElement /* UNO TODO */;
#else
                UIElement contentTemplateRoot = e.ItemContainer.ContentTemplateRoot;
#endif
				ElementCacheRecord elementCacheRecord;
                if (this.elementCache.TryGetValue(contentTemplateRoot, out elementCacheRecord))
                {
                    int phaseIndex = elementCacheRecord.Phases.BinarySearch((int)e.Phase);

                    if (phaseIndex >= 0)
                    {
                        foreach (PhasedElementRecord phasedElementRecord in elementCacheRecord.ElementsByPhase[phaseIndex])
                        {
                            phasedElementRecord.ThawAndShow();
                        }

                        phaseIndex++;
                    }
                    else
                    {
                        // don't know why this phase was not found, but by BinarySearch rules, ~phaseIndex is the place
                        // where it would be inserted, thus the item there has the next higher number.
                        phaseIndex = ~phaseIndex;
                    }

                    if (phaseIndex < elementCacheRecord.Phases.Count)
                    {
                        e.RegisterUpdateCallback((uint)elementCacheRecord.Phases[phaseIndex], this.OnContainerContentChangingCallback);
                    }
                }
            }

            private static UIElement FindContentTemplateRoot(FrameworkElement phaseElement)
            {
                DependencyObject ancestor = phaseElement;
                while (ancestor != null)
                {
                    DependencyObject parent = VisualTreeHelper.GetParent(ancestor);
                    SelectorItem item = parent as SelectorItem;

                    if (item != null)
                    {
#if HAS_UNO
						return item.ContentTemplateRoot as UIElement /* UNO TODO */;
#else
						return item.ContentTemplateRoot;
#endif
					}
					ancestor = parent;
                }

                return null;
            }

            public void CachePhaseElement(FrameworkElement phaseElement, int phase)
            {
                if (phase < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(phase));
                }

                if (phase <= 0)
                {
                    return;
                }

                UIElement contentTemplateRoot = IncrementalUpdater.FindContentTemplateRoot(phaseElement);
                if (contentTemplateRoot != null)
                {
                    // get the cache for this element
                    ElementCacheRecord elementCacheRecord;
                    if (!this.elementCache.TryGetValue(contentTemplateRoot, out elementCacheRecord))
                    {
                        elementCacheRecord = new ElementCacheRecord();
                        this.elementCache.Add(contentTemplateRoot, elementCacheRecord);
                    }

                    // get the cache for this phase
                    int phaseIndex = elementCacheRecord.Phases.BinarySearch(phase);

                    if (phaseIndex < 0)
                    {
                        // not found - insert
                        phaseIndex = ~phaseIndex;
                        elementCacheRecord.Phases.Insert(phaseIndex, phase);
                        elementCacheRecord.ElementsByPhase.Insert(phaseIndex, new List<PhasedElementRecord>());
                    }

                    List<PhasedElementRecord> phasedElementRecords = elementCacheRecord.ElementsByPhase[phaseIndex];

                    // first see if the element is already there
                    for (int i = 0; i < phasedElementRecords.Count; i++)
                    {
                        if (phasedElementRecords[i].FrameworkElement == phaseElement)
                        {
                            return;
                        }
                    }

                    // insert the element
                    phasedElementRecords.Add(new PhasedElementRecord(phaseElement));
                }
            }

            public void UncachePhaseElement(FrameworkElement phaseElement, int phase)
            {
                if (phase <= 0)
                {
                    return;
                }

                UIElement contentTemplateRoot = IncrementalUpdater.FindContentTemplateRoot(phaseElement);
                if (contentTemplateRoot != null)
                {
                    // get the cache for this element
                    ElementCacheRecord elementCacheRecord;
                    if (this.elementCache.TryGetValue(contentTemplateRoot, out elementCacheRecord))
                    {
                        // get the cache for this phase
                        int phaseIndex = elementCacheRecord.Phases.BinarySearch(phase);

                        if (phaseIndex >= 0)
                        {
                            // remove the element: the linear search here is not spectacular but the list should be very short
                            List<PhasedElementRecord> phasedElementRecords = elementCacheRecord.ElementsByPhase[phaseIndex];

                            for (int i = 0; i < phasedElementRecords.Count; i++)
                            {
                                if (phasedElementRecords[i].FrameworkElement == phaseElement)
                                {
                                    phasedElementRecords[i].ThawAndShow();

                                    phasedElementRecords.RemoveAt(i);

                                    if (phasedElementRecords.Count == 0)
                                    {
                                        elementCacheRecord.Phases.RemoveAt(phaseIndex);
                                        elementCacheRecord.ElementsByPhase.RemoveAt(phaseIndex);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            public void Attach(DependencyObject dependencyObject)
            {
                this.associatedListViewBase = dependencyObject as ListViewBase;

                if (this.associatedListViewBase != null)
                {
                    this.associatedListViewBase.ContainerContentChanging += this.OnContainerContentChanging;
                }
            }

            public void Detach()
            {
                if (this.associatedListViewBase != null)
                {
                    this.associatedListViewBase.ContainerContentChanging -= this.OnContainerContentChanging;
                }
                this.associatedListViewBase = null;
            }
        }
    }
}
