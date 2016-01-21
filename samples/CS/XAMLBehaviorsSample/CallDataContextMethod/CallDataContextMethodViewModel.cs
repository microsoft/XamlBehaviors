using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XAMLBehaviorsSample
{
    public class CallDataContextMethodViewModel
    {
        private Random _random = new Random();

        public ObservableCollection<int> RandomNumbers { get; } = new ObservableCollection<int>();

        public void AddRandomNumber()
        {
            RandomNumbers.Add(_random.Next(1000, 10000));
        }

        public void DeleteNumber(int number)
        {
            RandomNumbers.Remove(number);
        }
    }
}
