using System.Collections.Generic;
using Microsoft.Xaml.Interactions.Utility;
using Windows.UI.Xaml;

namespace ManagedUnitTests
{
    /// <summary>
    /// Test impementation of VisualTreeHelper.  Enables unit testing of code that
    /// uses VisualTreeHelper without needing a real visual tree.
    /// </summary>
    internal class TestVisualTreeHelper : IVisualTreeHelper
    {
        private Dictionary<DependencyObject, DependencyObject> parents = new Dictionary<DependencyObject, DependencyObject>();

        public void AddChild(DependencyObject parent, DependencyObject child)
        {
            this.parents[child] = parent;
        }

        #region IVisualTreeHelper implementation

        public DependencyObject GetParent(DependencyObject child)
        {
            DependencyObject parent;
            this.parents.TryGetValue(child, out parent);
            return parent;
        }

        #endregion
    }
}
