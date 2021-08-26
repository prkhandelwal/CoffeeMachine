using CoffeeMachine.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

Console.WriteLine("Welcome to coffee machine");
string fileName = "data.json";
string jsonString = File.ReadAllText(fileName);
var jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonString);
Console.WriteLine(jsonObject.machine.outlets.count_n);
await RunMachine(jsonObject);

static async Task RunMachine(dynamic input)
{

    var machineObject = input.machine;
    var outletsObject = machineObject.outlets;
    var numOutlets = (int)outletsObject.count_n;

    var beverages = new List<Beverage>();
    var items = new Dictionary<string, InventoryItem>();

    var beveragesObject = machineObject.beverages.ToObject<Dictionary<string, Dictionary<string, uint>>>();
    foreach (var bev in beveragesObject)
    {
        string bevName = bev.Key;
        var ingredients = new List<Ingredient>();
        foreach (var ingrediant in bev.Value)
        {
            ingredients.Add(new Ingredient(ingrediant.Key, ingrediant.Value));
        }
        beverages.Add(new Beverage(bevName, ingredients));
    }

    var itemsQuantityObject = machineObject.total_items_quantity.ToObject<Dictionary<string, uint>>();
    foreach (var item in itemsQuantityObject)
    {
        items.Add(item.Key, new InventoryItem(item.Key, item.Value));
    }

    var machine = new Machine(numOutlets, beverages, new Inventory(items));
    var output = await machine.StartPreparing();
    foreach (var item in output)
    {
        Console.WriteLine(item);
    }
}

