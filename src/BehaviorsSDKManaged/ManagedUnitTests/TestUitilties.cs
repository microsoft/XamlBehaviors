// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using Windows.UI.Xaml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ManagedUnitTests
{
    public static class TestUtilities
    {
        /// <summary>
        /// Handles the difference between InvalidOperationException in managed and native.
        /// </summary>
        public static void AssertThrowsInvalidOperationException(Action action)
        {
            Assert.ThrowsException<InvalidOperationException>(action);
        }

        public static void AssertThrowsArgumentException(Action action)
        {
            Assert.ThrowsException<ArgumentException>(action);
        }

        public static void AssertDetached(StubBehavior behavior)
        {
            Assert.AreEqual(1, behavior.DetachCount, "The Behavior should be detached.");
            Assert.IsNull(behavior.AssociatedObject, "A Detached Behavior should have a null AssociatedObject.");
        }

        public static void AssertNotDetached(StubBehavior behavior)
        {
            Assert.AreEqual(0, behavior.DetachCount, "The Behavior should not be detached.");
        }

        public static void AssertAttached(StubBehavior behavior, DependencyObject associatedObject)
        {
            Assert.AreEqual(1, behavior.AttachCount, "The behavior should be attached.");
            Assert.AreEqual(associatedObject, behavior.AssociatedObject, "The AssociatedObject of the Behavior should be what it was attached to.");
        }

        public static void AssertNotAttached(StubBehavior behavior)
        {
            Assert.AreEqual(0, behavior.AttachCount, "The behavior should not be attached.");
            Assert.IsNull(behavior.AssociatedObject, "The AssociatedObject should be null for a non-attached Behavior.");
        }
    }
}
