using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace XAMLBehaviorsSample;

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
