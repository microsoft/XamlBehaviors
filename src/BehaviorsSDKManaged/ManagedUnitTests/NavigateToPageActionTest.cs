using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xaml.Interactions.Core;
using Microsoft.UI.Xaml;
using AppContainerUITestMethod = Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer.UITestMethodAttribute;

namespace ManagedUnitTests
{
    [TestClass]
    public class NavigateToPageActionTest
    {
        private static readonly string TestPageName = typeof(BlankPage).FullName;

        [AppContainerUITestMethod]
        public void Execute_SenderImplementsINavigate_NavigatesToSender()
        {
            // Arrange
            TestVisualTreeHelper visualTreeHelper = new TestVisualTreeHelper();
            NavigateToPageAction action = new NavigateToPageAction(visualTreeHelper);
            action.TargetPage = NavigateToPageActionTest.TestPageName;
            NavigableStub navigateTarget = new NavigableStub();

            // Act
            bool success = (bool)action.Execute(navigateTarget, null);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(NavigateToPageActionTest.TestPageName, navigateTarget.NavigatedTypeFullName);
        }

        [AppContainerUITestMethod]
        public void Execute_SenderDoesNotImplementINavigate_NavigatesToAncestor()
        {
            // Arrange
            TestVisualTreeHelper visualTreeHelper = new TestVisualTreeHelper();
            NavigateToPageAction action = new NavigateToPageAction(visualTreeHelper);
            action.TargetPage = NavigateToPageActionTest.TestPageName;
            DependencyObject sender = new SimpleDependencyObject();
            NavigableStub navigateTarget = new NavigableStub();
            visualTreeHelper.AddChild(navigateTarget, sender);

            // Act
            bool success = (bool)action.Execute(sender, null);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(NavigateToPageActionTest.TestPageName, navigateTarget.NavigatedTypeFullName);
        }

        [AppContainerUITestMethod]
        public void Execute_NoAncestorImplementsINavigate_Fails()
        {
            // Arrange
            TestVisualTreeHelper visualTreeHelper = new TestVisualTreeHelper();
            NavigateToPageAction action = new NavigateToPageAction(visualTreeHelper);
            action.TargetPage = NavigateToPageActionTest.TestPageName;
            DependencyObject sender = new SimpleDependencyObject();
            DependencyObject parent = new SimpleDependencyObject();
            visualTreeHelper.AddChild(parent, sender);

            // Act
            bool success = (bool)action.Execute(sender, null);

            // Assert
            Assert.IsFalse(success);
        }

        private class SimpleDependencyObject : DependencyObject
        {
        }
    }
}
