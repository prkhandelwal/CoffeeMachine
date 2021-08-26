using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMachine.Models
{
    public class Beverage
    {
        public string Name { get; private set; }
        public List<Ingredient> Ingredients { get; set; }

        public Beverage(String name, List<Ingredient> ingredients)
        {
            Name = name;
            Ingredients = ingredients;
        }
    }
}
