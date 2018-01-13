using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.Items
{
    public class Inventory
    {
        private Item[] _items;

        public Inventory(params Item[] items)
        {
            _items = items;
        }

        public int CurrentIndex { get; private set; }

        public Item CurrentItem => _items[CurrentIndex];

        public int MaxItems => 10;

        public Item this[int key]
        {
            get
            {
                if (key >= _items.Length || key < 0)
                {
                    return null;
                }
                return _items[key];
            }
        }

        public void Next()
        {
            if (++CurrentIndex >= _items.Length)
            {
                CurrentIndex = 0;
            }
        }

        public void Previous()
        {
            if (--CurrentIndex < 0)
            {
                CurrentIndex = _items.Length - 1;
            }
        }
    }
}
