using Microsoft.UI.Xaml.Controls;

namespace ManagedUnitTests
{
    /// <summary>
    /// This page serves two purposes:
    /// 
    /// 1. Its existence causes the XAML compiler to implement IXamlMetadataProvider
    /// on the App class.  IXamlMetadataProvider is used by NavigateToPageAction and will
    /// only be implemented on App if there are XAML types defined in the project.
    /// 
    /// 2. It provides a target for the NavigateToPageAction to navigate to in tests.
    /// </summary>
    public sealed partial class BlankPage : Page
    {
        public BlankPage()
        {
            this.InitializeComponent();
        }
    }
}
