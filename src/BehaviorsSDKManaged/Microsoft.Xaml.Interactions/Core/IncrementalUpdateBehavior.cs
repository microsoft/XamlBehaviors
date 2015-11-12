// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace Microsoft.Xaml.Interactions.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Controls.Primitives;
    using Windows.UI.Xaml.Media;
    using Interactivity;

    /// <summary>
    /// A behavior that allows incremental updating of <seealso cref="Windows.UI.Xaml.Controls.ListView"/> and <seealso cref="Windows.UI.Xaml.Controls.GridView"/> contents to support faster updating.
    /// By attaching this behavior to elements in the <seealso cref="Windows.UI.Xaml.Controls.ItemsControl.ItemTemplate"/> used by these views, some of the updates can be deferred until there is render time available, resulting in a smoother experience.
    /// </summary>
    public sealed class IncrementalUpdateBehavior : DependencyObject, IBehavior
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

        private DependencyObject associatedObject = null;
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
            FrameworkElement frameworkElement = behavior.associatedObject as FrameworkElement;

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
            FrameworkElement frameworkElement = (FrameworkElement)this.associatedObject;

            if (incrementalUpdater != null && frameworkElement != null)
            {
                incrementalUpdater.CachePhaseElement(frameworkElement, this.Phase);
            }
        }

        private void OnAssociatedObjectUnloaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement frameworkElement = (FrameworkElement)this.associatedObject;

            if (this.updater != null && frameworkElement != null)
            {
                this.updater.UncachePhaseElement(frameworkElement, this.Phase);
            }

            this.updater = null;
        }

        private IncrementalUpdater FindUpdater()
        {
            if (this.updater != null)
            {
                return this.updater;
            }

            DependencyObject ancestor = this.associatedObject;
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
        /// Gets the <seealso cref="Windows.UI.Xaml.DependencyObject"/> to which the <seealso cref="IBehavior"/> is attached.
        /// </summary>
        public DependencyObject AssociatedObject
        {
            get { return this.associatedObject; }
        }

        /// <summary>
        /// Attaches to the specified object.
        /// </summary>
        /// <param name="associatedObject">The <seealso cref="Windows.UI.Xaml.DependencyObject"/> to which the <seealso cref="IBehavior"/> will be attached.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1500:VariableNamesShouldNotMatchFieldNames", MessageId = "associatedObject")]
        public void Attach(DependencyObject associatedObject)
        {
            if (associatedObject == this.associatedObject || Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                return;
            }

            if (this.associatedObject != null)
            {
                throw new InvalidOperationException(string.Format(
                    CultureInfo.CurrentCulture,
                    ResourceHelper.CannotAttachBehaviorMultipleTimesExceptionMessage,
                    associatedObject,
                    this.associatedObject));
            }

            Debug.Assert(associatedObject != null, "Cannot attach the behavior to a null object.");

            this.associatedObject = associatedObject;
            FrameworkElement frameworkElement = associatedObject as FrameworkElement;
            if (frameworkElement != null)
            {
                frameworkElement.Loaded += this.OnAssociatedObjectLoaded;
                frameworkElement.Unloaded += this.OnAssociatedObjectUnloaded;
            }
        }

        /// <summary>
        /// Detaches this instance from its associated object.
        /// </summary>
        public void Detach()
        {
            FrameworkElement frameworkElement = this.associatedObject as FrameworkElement;
            if (frameworkElement != null)
            {
                frameworkElement.Loaded -= this.OnAssociatedObjectLoaded;
                frameworkElement.Unloaded -= this.OnAssociatedObjectUnloaded;
                // no need to perform the work that Unloaded would have done - that's just housekeeping on the cache, which is now going away
            }
            this.associatedObject = null;
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
                UIElement contentTemplateRoot = e.ItemContainer.ContentTemplateRoot;

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
                UIElement contentTemplateRoot = e.ItemContainer.ContentTemplateRoot;

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
                        return item.ContentTemplateRoot;
                    }
                    ancestor = parent;
                }

                return null;
            }

            public void CachePhaseElement(FrameworkElement phaseElement, int phase)
            {
                if (phase < 0)
                {
                    throw new ArgumentOutOfRangeException("phase");
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
