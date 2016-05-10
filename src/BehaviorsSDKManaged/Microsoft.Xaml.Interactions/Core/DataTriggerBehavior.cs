﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace Microsoft.Xaml.Interactions.Core
{
    using System;
    using System.Globalization;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Markup;
    using Interactivity;

    /// <summary>
    /// A behavior that performs actions when the bound data meets a specified condition.
    /// </summary>
    [ContentPropertyAttribute(Name = "Actions")]
    public sealed class DataTriggerBehavior : Behavior
    {
        /// <summary> q
        /// Identifies the <seealso cref="Actions"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty ActionsProperty = DependencyProperty.Register(
            "Actions",
            typeof(ActionCollection),
            typeof(DataTriggerBehavior),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <seealso cref="Binding"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty BindingProperty = DependencyProperty.Register(
            "Binding",
            typeof(object),
            typeof(DataTriggerBehavior),
            new PropertyMetadata(null, new PropertyChangedCallback(DataTriggerBehavior.OnValueChanged)));

        /// <summary>
        /// Identifies the <seealso cref="ComparisonCondition"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty ComparisonConditionProperty = DependencyProperty.Register(
            "ComparisonCondition",
            typeof(ComparisonConditionType),
            typeof(DataTriggerBehavior),
            new PropertyMetadata(ComparisonConditionType.Equal,
                new PropertyChangedCallback(DataTriggerBehavior.OnValueChanged)));
                
        /// <summary>
        /// Identifies the <seealso cref="Otherwise"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty OtherwiseProperty = DependencyProperty.Register(
            "Otherwise",
            typeof(ActionCollection),
            typeof(DataTriggerBehavior),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <seealso cref="Value"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(object),
            typeof(DataTriggerBehavior),
            new PropertyMetadata(null, new PropertyChangedCallback(DataTriggerBehavior.OnValueChanged)));

        /// <summary>
        /// Gets the collection of actions associated with the behavior. This is a dependency property.
        /// </summary>
        public ActionCollection Actions
        {
            get
            {
                ActionCollection actionCollection = (ActionCollection)this.GetValue(DataTriggerBehavior.ActionsProperty);
                if (actionCollection == null)
                {
                    actionCollection = new ActionCollection();
                    this.SetValue(DataTriggerBehavior.ActionsProperty, actionCollection);
                }

                return actionCollection;
            }
        }

        /// <summary>
        /// Gets or sets the bound object that the <see cref="DataTriggerBehavior"/> will listen to. This is a dependency property.
        /// </summary>
        public object Binding
        {
            get
            {
                return (object)this.GetValue(DataTriggerBehavior.BindingProperty);
            }
            set
            {
                this.SetValue(DataTriggerBehavior.BindingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the type of comparison to be performed between <see cref="DataTriggerBehavior.Binding"/> and <see cref="DataTriggerBehavior.Value"/>. This is a dependency property.
        /// </summary>
        public ComparisonConditionType ComparisonCondition
        {
            get
            {
                return (ComparisonConditionType)this.GetValue(DataTriggerBehavior.ComparisonConditionProperty);
            }
            set
            {
                this.SetValue(DataTriggerBehavior.ComparisonConditionProperty, value);
            }
        }

        /// <summary>
        /// Gets the collection of actions associated with the behavior to be executed when the expression evaluates to false. This is a dependency property.
        /// </summary>
        public ActionCollection Otherwise
        {
            get
            {
                ActionCollection actionCollection = (ActionCollection)this.GetValue(DataTriggerBehavior.OtherwiseProperty);
                if (actionCollection == null)
                {
                    actionCollection = new ActionCollection();
                    this.SetValue(DataTriggerBehavior.OtherwiseProperty, actionCollection);
                }

                return actionCollection;
            }
        }

        /// <summary>
        /// Gets or sets the value to be compared with the value of <see cref="DataTriggerBehavior.Binding"/>. This is a dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        public object Value
        {
            get
            {
                return (object)this.GetValue(DataTriggerBehavior.ValueProperty);
            }
            set
            {
                this.SetValue(DataTriggerBehavior.ValueProperty, value);
            }
        }

        private static bool Compare(object leftOperand, ComparisonConditionType operatorType, object rightOperand)
        {
            if (leftOperand != null && rightOperand != null)
            {
                rightOperand = TypeConverterHelper.Convert(rightOperand.ToString(), leftOperand.GetType().FullName);
            }

            IComparable leftComparableOperand = leftOperand as IComparable;
            IComparable rightComparableOperand = rightOperand as IComparable;
            if ((leftComparableOperand != null) && (rightComparableOperand != null))
            {
                return DataTriggerBehavior.EvaluateComparable(leftComparableOperand, operatorType, rightComparableOperand);
            }

            switch (operatorType)
            {
                case ComparisonConditionType.Equal:
                    return object.Equals(leftOperand, rightOperand);

                case ComparisonConditionType.NotEqual:
                    return !object.Equals(leftOperand, rightOperand);

                case ComparisonConditionType.LessThan:
                case ComparisonConditionType.LessThanOrEqual:
                case ComparisonConditionType.GreaterThan:
                case ComparisonConditionType.GreaterThanOrEqual:
                    {
                        if (leftComparableOperand == null && rightComparableOperand == null)
                        {
                            throw new ArgumentException(string.Format(
                                CultureInfo.CurrentCulture,
                                ResourceHelper.InvalidOperands,
                                leftOperand != null ? leftOperand.GetType().Name : "null",
                                rightOperand != null ? rightOperand.GetType().Name : "null",
                                operatorType.ToString()));
                        }
                        else if (leftComparableOperand == null)
                        {
                            throw new ArgumentException(string.Format(
                                CultureInfo.CurrentCulture,
                                ResourceHelper.InvalidLeftOperand,
                                leftOperand != null ? leftOperand.GetType().Name : "null",
                                operatorType.ToString()));
                        }
                        else
                        {
                            throw new ArgumentException(string.Format(
                                CultureInfo.CurrentCulture,
                                ResourceHelper.InvalidRightOperand,
                                rightOperand != null ? rightOperand.GetType().Name : "null",
                                operatorType.ToString()));
                        }
                    }
            }

            return false;
        }

        /// <summary>
        /// Evaluates both operands that implement the IComparable interface.
        /// </summary>
        private static bool EvaluateComparable(IComparable leftOperand, ComparisonConditionType operatorType, IComparable rightOperand)
        {
            object convertedOperand = null;
            try
            {
                convertedOperand = Convert.ChangeType(rightOperand, leftOperand.GetType(), CultureInfo.CurrentCulture);
            }
            catch (FormatException)
            {
                // FormatException: Convert.ChangeType("hello", typeof(double), ...);
            }
            catch (InvalidCastException)
            {
                // InvalidCastException: Convert.ChangeType(4.0d, typeof(Rectangle), ...);
            }

            if (convertedOperand == null)
            {
                return operatorType == ComparisonConditionType.NotEqual;
            }

            int comparison = leftOperand.CompareTo((IComparable)convertedOperand);
            switch (operatorType)
            {
                case ComparisonConditionType.Equal:
                    return comparison == 0;

                case ComparisonConditionType.NotEqual:
                    return comparison != 0;

                case ComparisonConditionType.LessThan:
                    return comparison < 0;

                case ComparisonConditionType.LessThanOrEqual:
                    return comparison <= 0;

                case ComparisonConditionType.GreaterThan:
                    return comparison > 0;

                case ComparisonConditionType.GreaterThanOrEqual:
                    return comparison >= 0;
            }

            return false;
        }

        private static void OnValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            DataTriggerBehavior dataTriggerBehavior = (DataTriggerBehavior)dependencyObject;
            if (dataTriggerBehavior.AssociatedObject == null)
            {
                return;
            }

            DataBindingHelper.RefreshDataBindingsOnActions(dataTriggerBehavior.Actions);

            // Some value has changed--either the binding value, reference value, or the comparison condition. Re-evaluate the equation.
            if (DataTriggerBehavior.Compare(dataTriggerBehavior.Binding, dataTriggerBehavior.ComparisonCondition, dataTriggerBehavior.Value))
            {
                Interaction.ExecuteActions(dataTriggerBehavior.AssociatedObject, dataTriggerBehavior.Actions, args);
            }
            else
            {
                Interaction.ExecuteActions(dataTriggerBehavior.AssociatedObject, dataTriggerBehavior.Otherwise, args);
            }
        }
    }
}
