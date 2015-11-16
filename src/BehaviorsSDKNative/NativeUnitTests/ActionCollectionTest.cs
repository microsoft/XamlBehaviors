// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
namespace BehaviorsXamlSdkUnitTests
{
    using System;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Microsoft.Xaml.Interactivity;
    using Windows.UI.Xaml.Controls;
    using AppContainerUITestMethod = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethodAttribute;

    [TestClass]
    public class ActionCollectionTest
    {
        [AppContainerUITestMethod]
        public void VectorChanged_NonActionAdded_ExceptionThrown()
        {
            ActionCollection actionCollection = new ActionCollection();
            actionCollection.Add(new StubAction());

            TestUtilities.AssertThrowsException(() => actionCollection.Add(new Button()));
        }

        [AppContainerUITestMethod]
        public void VectorChanged_ActionChangedToNonAction_ExceptionThrown()
        {
            ActionCollection actionCollection = new ActionCollection();
            actionCollection.Add(new StubAction());

            TestUtilities.AssertThrowsException(() => actionCollection[0] = new Button());
        }
    }
}
