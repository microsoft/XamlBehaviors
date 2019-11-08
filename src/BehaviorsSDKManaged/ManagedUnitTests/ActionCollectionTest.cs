using Microsoft.Xaml.Interactions.Media;
using AppContainerUITestMethod = Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer.UITestMethodAttribute;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ManagedUnitTests
{
    [TestClass]
    public class ActionCollectionTest
    {
        [AppContainerUITestMethod]
        public void Constructor_DefaultConstructor_SetsVolumeCorrectly()
        {
            PlaySoundAction playSoundAction = new PlaySoundAction();
            Assert.AreEqual(0.5, playSoundAction.Volume, "Volume should be initialized to 0.5");
        }

        [AppContainerUITestMethod]
        public void Invoke_RelativeSource_Invokes()
        {
            PlaySoundAction playSoundAction = new PlaySoundAction();

            playSoundAction.Source = "foo.wav";
            bool result = (bool)playSoundAction.Execute(null, null);
            Assert.IsTrue(result);
        }

        [AppContainerUITestMethod]
        public void Invoke_AbsoluteSource_Invokes()
        {
            PlaySoundAction playSoundAction = new PlaySoundAction();

            playSoundAction.Source = "ms-appx:///foo.wav";
            bool result = (bool)playSoundAction.Execute(null, null);
            Assert.IsTrue(result);
        }

        [AppContainerUITestMethod]
        public void Invoke_InvalidSource_ReturnsFalse()
        {
            PlaySoundAction playSoundAction = new PlaySoundAction();

            Assert.IsFalse((bool)playSoundAction.Execute(null, null),
                "PlaySoundAction.Execute should return false with a null source path.");
        }
    }
}
