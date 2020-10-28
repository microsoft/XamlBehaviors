// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml.Controls;

namespace ManagedUnitTests
{
    [TestClass]
    public class InteractionTest
    {
        [UITestMethod]
        public void SetBehaviors_MultipleBehaviors_AllAttached()
        {
            BehaviorCollection behaviorCollection = new BehaviorCollection();
            behaviorCollection.Add(new StubBehavior());
            behaviorCollection.Add(new StubBehavior());
            behaviorCollection.Add(new StubBehavior());

            Button button = new Button();
            Interaction.SetBehaviors(button, behaviorCollection);

            foreach (StubBehavior behavior in behaviorCollection)
            {
                Assert.AreEqual(1, behavior.AttachCount, "Should only have called Attach once.");
                Assert.AreEqual(0, behavior.DetachCount, "Should not have called Detach.");
                Assert.AreEqual(button, behavior.AssociatedObject, "Should be attached to the host of the BehaviorCollection.");
            }
        }

        [UITestMethod]
        public void SetBehaviors_MultipleSets_DoesNotReattach()
        {
            BehaviorCollection behaviorCollection = new BehaviorCollection() { new StubBehavior() };

            Button button = new Button();
            Interaction.SetBehaviors(button, behaviorCollection);
            Interaction.SetBehaviors(button, behaviorCollection);

            foreach (StubBehavior behavior in behaviorCollection)
            {
                Assert.AreEqual(1, behavior.AttachCount, "Should only have called Attach once.");
            }
        }

        [UITestMethod]
        public void SetBehaviors_CollectionThenNull_DeatchCollection()
        {
            BehaviorCollection behaviorCollection = new BehaviorCollection() { new StubBehavior() };

            Button button = new Button();
            Interaction.SetBehaviors(button, behaviorCollection);
            Interaction.SetBehaviors(button, null);

            foreach (StubBehavior behavior in behaviorCollection)
            {
                Assert.AreEqual(1, behavior.DetachCount, "Should only have called Detach once.");
                Assert.IsNull(behavior.AssociatedObject, "AssociatedObject should be null after Detach.");
            }
        }

        [UITestMethod]
        public void SetBehaviors_NullThenNull_NoOp()
        {
            // As long as this doesn't crash/assert, we're good.

            Button button = new Button();
            Interaction.SetBehaviors(button, null);
            Interaction.SetBehaviors(button, null);
            Interaction.SetBehaviors(button, null);
        }

        [UITestMethod]

        public void SetBehaviors_ManualDetachThenNull_DoesNotDoubleDetach()
        {
            BehaviorCollection behaviorCollection = new BehaviorCollection();
            behaviorCollection.Add(new StubBehavior());
            behaviorCollection.Add(new StubBehavior());
            behaviorCollection.Add(new StubBehavior());

            Button button = new Button();
            Interaction.SetBehaviors(button, behaviorCollection);

            foreach (StubBehavior behavior in behaviorCollection)
            {
                behavior.Detach();
            }

            Interaction.SetBehaviors(button, null);

            foreach (StubBehavior behavior in behaviorCollection)
            {
                Assert.AreEqual(1, behavior.DetachCount, "Setting BehaviorCollection to null should not call Detach on already Detached Behaviors.");
                Assert.IsNull(behavior.AssociatedObject, "AssociatedObject should be null after Detach.");
            }
        }

        [UITestMethod]
        public void ExecuteActions_NullParameters_ReturnsEmptyEnumerable()
        {
            // Mostly just want to test that this doesn't throw any exceptions.
            IEnumerable<object> result = Interaction.ExecuteActions(null, null, null);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count(), "Calling ExecuteActions with a null ActionCollection should return an empty enumerable.");
        }

        [UITestMethod]
        public void ExecuteActions_MultipleActions_AllActionsExecuted()
        {
            ActionCollection actions = new ActionCollection();
            actions.Add(new StubAction());
            actions.Add(new StubAction());
            actions.Add(new StubAction());

            Button sender = new Button();
            string parameterString = "TestString";

            Interaction.ExecuteActions(sender, actions, parameterString);

            foreach (StubAction action in actions)
            {
                Assert.AreEqual(1, action.ExecuteCount, "Each IAction should be executed once.");
                Assert.AreEqual(sender, action.Sender, "Sender is passed to the actions.");
                Assert.AreEqual(parameterString, action.Parameter, "Parameter is passed to the actions.");
            }
        }

        [UITestMethod]
        public void ExecuteActions_ActionsWithResults_ResultsInActionOrder()
        {
            string[] expectedReturnValues = { "A", "B", "C" };

            ActionCollection actions = new ActionCollection();

            foreach (string returnValue in expectedReturnValues)
            {
                actions.Add(new StubAction(returnValue));
            }

            List<object> results = Interaction.ExecuteActions(null, actions, null).ToList();

            Assert.AreEqual(expectedReturnValues.Length, results.Count, "Should have the same number of results as IActions.");

            for (int resultIndex = 0; resultIndex < results.Count; resultIndex++)
            {
                Assert.AreEqual(expectedReturnValues[resultIndex], results[resultIndex], "Results should be returned in the order of the actions in the ActionCollection.");
            }
        }
    }
}
