using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoffeeMachine.Models
{
    public class Machine
    {
        public int NumOutlets { get; private set; }
        public Inventory Inventory { get; private set; }
        public List<Beverage> Beverages { get; private set; }

        public Machine(int numOutlets, List<Beverage> beverages, Inventory inventory)
        {
            NumOutlets = numOutlets;
            Inventory = inventory;
            Beverages = beverages;
        }

        public async Task<HashSet<string>> StartPreparing()
        {
            int bevarageCounter = 0;
            var output = new HashSet<string>();
            int numBeveragesToPrepare = Math.Min(NumOutlets, Beverages.Count());
            var tasks = new List<Task<string>>();
            //Added parallel tasks in batches
            while (bevarageCounter < Beverages.Count())
            {
                // Batch size
                var beverage = Beverages[bevarageCounter];
                string status = CheckRequirements(beverage);
                if (numBeveragesToPrepare == 0)
                    break;
                if (String.IsNullOrEmpty(status))
                {
                    tasks.Add(Prepare(beverage)); // Adding tasks in parallel
                    bevarageCounter++;
                }
                else
                {
                    Console.WriteLine(status);
                    output.Add(status);
                    bevarageCounter++;
                }

                if(tasks.Count() == numBeveragesToPrepare)
                {
                    await Task.WhenAll(tasks);
                    foreach (var task in tasks)
                    {
                        status = task.Result;
                        Console.WriteLine(status);
                        output.Add(status);
                    }
                    tasks.Clear();
                }
            }

            if (tasks.Count() > 0)
            {
                await Task.WhenAll(tasks);
                foreach (var task in tasks)
                {
                    var status = task.Result;
                    Console.WriteLine(status);
                    output.Add(status);
                }
                tasks.Clear();
            }

            return output;
        }

        public string CheckRequirements(Beverage beverage)
        {
            foreach (var item in beverage.Ingredients)
            {
                if (Inventory.ItemUnavailable(item.Name))
                {
                    return beverage.Name + " cannot be prepared because " + item.Name + " is not available";
                }
                if (Inventory.ItemLowInStock(item.Name))
                {
                    return beverage.Name + " cannot be prepared because item " + item.Name + " is not sufficient";
                }
            }
            return String.Empty;
        }

        public void AddNewBeverage(Beverage beverage)
        {
            if(Beverages.Where(item => item.Name == beverage.Name).Count() == 0)
            {
                Beverages.Add(beverage);
            }
        }

        public Dictionary<string, uint> GetItemsToRefill()
        {
            return Inventory.GetItemsToRefill();
        }

        public void AddInventoryItem(String name, uint amount)
        {
            Inventory.AddItem(name, amount);
        }

        public async Task<string> Prepare(Beverage beverage)
        {
            await Task.Delay(1000); // Delay for simulating machine time
            foreach (var ingredient in beverage.Ingredients)
            {
                if (!Inventory.ConsumeItem(ingredient.Name, ingredient.Quantity))
                {
                    if (Inventory.ItemUnavailable(ingredient.Name))
                        return beverage.Name + " cannot be prepared because " + ingredient.Name + " is not available";

                    return beverage.Name + " cannot be prepared because item " + ingredient.Name + " is not sufficient";
                }
            }
            return beverage.Name + " is prepared";
        }

    }
}
