using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

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
