using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace XAMLBehaviorsSample
{
    public sealed partial class IncrementalUpdateControl : UserControl
    {
        public List<ItemSample> Items;
        public IncrementalUpdateControl()
        {
            this.InitializeComponent();
            Items = new List<ItemSample>();
            gridViewSample.ItemsSource = Items;
            for(int i = 1; i <= 100; i++)
            {
                Items.Add(new ItemSample(i));
            }
        }
    }

    public class ItemSample
    {
        public int Count { get; set; }
        public ItemSample()
        {
            Count = 0;
        }
        public ItemSample(int i)
        {
            Count = i;
        }
    }
}
