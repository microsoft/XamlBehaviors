// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Microsoft.Xaml.Interactions.Media;

namespace ManagedUnitTests
{
    [TestClass]
    public class ActionCollectionTest
    {
        [UITestMethod]
        public void Constructor_DefaultConstructor_SetsVolumeCorrectly()
        {
            PlaySoundAction playSoundAction = new PlaySoundAction();
            Assert.AreEqual(0.5, playSoundAction.Volume, "Volume should be initialized to 0.5");
        }

        [UITestMethod]
        public void Invoke_RelativeSource_Invokes()
        {
            PlaySoundAction playSoundAction = new PlaySoundAction();

            playSoundAction.Source = "foo.wav";
            bool result = (bool)playSoundAction.Execute(null, null);
            Assert.IsTrue(result);
        }

        [UITestMethod]
        public void Invoke_AbsoluteSource_Invokes()
        {
            PlaySoundAction playSoundAction = new PlaySoundAction();

            playSoundAction.Source = "ms-appx:///foo.wav";
            bool result = (bool)playSoundAction.Execute(null, null);
            Assert.IsTrue(result);
        }

        [UITestMethod]
        public void Invoke_InvalidSource_ReturnsFalse()
        {
            PlaySoundAction playSoundAction = new PlaySoundAction();

            Assert.IsFalse((bool)playSoundAction.Execute(null, null),
                "PlaySoundAction.Execute should return false with a null source path.");
        }
    }
}
