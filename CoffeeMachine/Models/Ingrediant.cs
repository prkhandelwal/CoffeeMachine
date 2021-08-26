using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMachine.Models
{
    public class Ingredient
    {
        public Ingredient(string name, uint quantity)
        {
            Name = name;
            Quantity = quantity;
        }
        public string Name { get; set; }
        public uint Quantity { get; set; }
    }

    public class InventoryItem : Ingredient
    {
        public bool IsLowInStock { get; set; }

        private readonly object QuantityLock;
        public InventoryItem(string name, uint quantity)  : base(name, quantity)
        {
            IsLowInStock = (quantity == 0);
            QuantityLock = new object();
        }

        public void Add(uint additionalQuantity)
        {
            lock (QuantityLock)
            {
                Quantity += additionalQuantity;
                IsLowInStock = false;
            }
        }

        public bool Consume(uint consumeQuantity)
        {
            lock (QuantityLock)
            {
                if (consumeQuantity > Quantity)
                {
                    IsLowInStock = true;
                    return false;
                }
                Quantity -= consumeQuantity;
                if (Quantity == 0)
                    IsLowInStock = true;
            }
            return true;
        }
    }


}
