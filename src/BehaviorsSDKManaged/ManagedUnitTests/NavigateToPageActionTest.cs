// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;

namespace ManagedUnitTests;

[TestClass]
public class NavigateToPageActionTest
{
    private static readonly string TestPageName = typeof(BlankPage).FullName;

    [UITestMethod]
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

    [UITestMethod]
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

    [UITestMethod]
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
