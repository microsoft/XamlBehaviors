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
    public sealed partial class IncrementalUpdateControl_Mobile : UserControl
    {
        public List<ItemSample_Mobile> Items;
        public IncrementalUpdateControl_Mobile()
        {
            this.InitializeComponent();
            Items = new List<ItemSample_Mobile>();
            gridViewSample.ItemsSource = Items;
            for (int i = 1; i <= 100; i++)
            {
                Items.Add(new ItemSample_Mobile(i));
            }
        }
    }

    public class ItemSample_Mobile
    {
        public int Count { get; set; }
        public ItemSample_Mobile()
        {
            Count = 0;
        }
        public ItemSample_Mobile(int i)
        {
            Count = i;
        }
    }
}
