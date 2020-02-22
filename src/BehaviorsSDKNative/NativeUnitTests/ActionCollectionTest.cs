// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------

using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml.Controls;

namespace BehaviorsXamlSdkUnitTests
{
    [TestClass]
    public class ActionCollectionTest
    {
        [UITestMethod]
        public void VectorChanged_NonActionAdded_ExceptionThrown()
        {
            ActionCollection actionCollection = new ActionCollection();
            actionCollection.Add(new StubAction());

            Assert.ThrowsException<COMException>(() => actionCollection.Add(new Button()));
        }

        [UITestMethod]
        public void VectorChanged_ActionChangedToNonAction_ExceptionThrown()
        {
            ActionCollection actionCollection = new ActionCollection();
            actionCollection.Add(new StubAction());

            Assert.ThrowsException<COMException>(() => actionCollection[0] = new Button());
        }
    }
}
