using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMachine.Models
{
    public class Inventory
    {
        public Dictionary<string, InventoryItem> Items { get; private set; }


        public Inventory(Dictionary<string, InventoryItem> items)
        {
            Items = items;
            MarkItemsUnavailable();
        }

        public void MarkItemsUnavailable()
        {
            foreach (var item in Items)
            {
                if (item.Value.Quantity == 0)
                {
                    item.Value.IsLowInStock = true;
                }
            }
        }

        public uint GetItemQuantity(String name)
        {
            return Items.ContainsKey(name) ? Items[name].Quantity : 0;
        }

        public bool ItemUnavailable(String name)
        { 
            if (!Items.ContainsKey(name))
                AddItem(name, 0);
            return Items[name].Quantity == 0;
        }

        public bool ItemLowInStock(String name)
        {
            if (!Items.ContainsKey(name))
                AddItem(name, 0);
            return Items[name].IsLowInStock;
        }

        public Dictionary<string, uint> GetItemsToRefill()
        {
            var itemsToRefill = new Dictionary<string, uint>();
            foreach (var item in Items.Keys)
            {
                if(ItemUnavailable(item) || ItemLowInStock(item))
                {
                    itemsToRefill.Add(item, GetItemQuantity(item));
                }
            }
            return itemsToRefill;
        }

        public bool ConsumeItem(String name, uint amount)
        {
            return Items[name].Consume(amount);
        }

        public void AddItem(String name, uint amount)
        {
            if (Items.ContainsKey(name))
                Items[name].Add(amount);
            else
                Items.Add(name, new InventoryItem(name, amount));
        }
    }
}
