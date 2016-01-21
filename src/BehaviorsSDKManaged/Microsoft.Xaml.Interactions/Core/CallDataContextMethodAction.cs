using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

using Microsoft.Xaml.Interactivity;

namespace Microsoft.Xaml.Interactions.Core
{
    /// <summary>
    /// Calls the specified public method on the DataContext of the associated object, 
    /// or on the DataContext of the closest visual ancestor having a function with a matching name.
    /// </summary>
    /// <remarks>
    /// 
    /// <list type="bullet">
    /// <listheader>Method signatures supported: </listheader>
    /// <item>
    /// <term>()</term>
    /// <description>method with no parameters</description>
    /// <term>(dataContext)</term>
    /// <description>method with a single parameter that has the type of the DataContext. Useful for calling from outside lists, e.g. Delete(Book book)</description>
    /// <term>(dataContext, sender)</term>
    /// <description>dataContext is set to the DataContext of the associated object. Sender is the <see cref="System.Object"/> that is passed to the action by the behavior. Generally this is <seealso cref="Microsoft.Xaml.Interactivity.IBehavior.AssociatedObject"/> or a target object.</description>
    /// <term>(dataContext, sender, parameters)</term>
    /// <description>dataContext is set to the DataContext of the associated object. Sender is the <see cref="System.Object"/> that is passed to the action by the behavior. Generally this is <seealso cref="Microsoft.Xaml.Interactivity.IBehavior.AssociatedObject"/> or a target object. Parameters is the <see cref="System.Object"/> that is passed to the action by the behavior.</description>
    /// </item>
    /// </list>
    /// </remarks>
    public class CallDataContextMethodAction : DependencyObject, IAction
    {
        /// <summary>
        /// Invokes the specified method on the DataContext.
        /// </summary>
        public object Execute(object sender, object parameter)
        {
            var fwe = sender as FrameworkElement;
            if (fwe == null)
                return null;

            var associatedObjectDataContext = fwe.DataContext;
            var successful = false;
            object lastDataContext = null;

            while (fwe != null)
            {
                var fweDataContext = fwe.DataContext;
                if (fweDataContext == null || fweDataContext.Equals(lastDataContext))
                {
                    fwe = VisualTreeHelper.GetParent(fwe) as FrameworkElement;
                    continue;
                }

                lastDataContext = fweDataContext;

                var mi = fweDataContext.GetType().GetRuntimeMethods().FirstOrDefault(m => m.IsPublic && m.Name == MethodName);
                if (mi == null)
                {
                    fwe = VisualTreeHelper.GetParent(fwe) as FrameworkElement;
                    continue;
                }
                switch (mi.GetParameters().Length)
                {
                    case 1:
                        mi.Invoke(fweDataContext, new[] { associatedObjectDataContext });
                        break;
                    case 2:
                        mi.Invoke(fweDataContext, new[] { associatedObjectDataContext, sender });
                        break;
                    case 3:
                        mi.Invoke(fweDataContext, new[] { associatedObjectDataContext, sender, parameter });
                        break;
                    default:
                        mi.Invoke(fweDataContext, null);
                        break;
                }
                successful = true;
                break;
            }

            if (!successful)
                throw new ArgumentException(string.Format(
                    CultureInfo.CurrentCulture, 
                    ResourceHelper.CallDataContextMethodActionMethodNotFoundExceptionMessage, 
                    this.MethodName));

            return null;
        }

        /// <summary>
        /// Gets or sets the name of the method to invoke. This is a dependency property.
        /// </summary>
        public string MethodName
        {
            get { return (string)this.GetValue(MethodNameProperty); }

            set { this.SetValue(MethodNameProperty, value); }
        }

        /// <summary>
        /// The MethodName property's descriptor
        /// </summary>
        public static readonly DependencyProperty MethodNameProperty = DependencyProperty.Register(
            "MethodName",
            typeof(string),
            typeof(CallDataContextMethodAction),
            new PropertyMetadata(null));
    }
}