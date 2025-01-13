using OurBakeryStore.Models;
using OurBakeryStore.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace OurBakeryStore.Services
{
    public class BakeryService :IBakeryService
    {
        List<Bakery> Bakeries { get; }
        int nextId = 3;
        public BakeryService()
        {
            Bakeries = new List<Bakery>
            {
                new Bakery { Id = 1, Name = "Classic Italian", IsItWithChocolate = false },
                new Bakery { Id = 2, Name = "Veggie", IsItWithChocolate = true }
            };
        }

        public List<Bakery> GetAll() => Bakeries;
        public Bakery Get(int id) => Bakeries.FirstOrDefault(p => p.Id == id);

        public void Add(Bakery bakery)
        {
            bakery.Id = nextId++;
            Bakeries.Add(bakery); // חשוב לוודא שזה בדיוק השם הנכון
            Console.WriteLine($"Bakery added: {bakery.Name}, Total bakeries: {Bakeries.Count}");
        }


        public void Delete(int id)
        {
            var bakery = Get(id);
            if (bakery != null)
            {
                Bakeries.Remove(bakery);
                Console.WriteLine($"Bakery with ID {id} deleted.");
            }
        }


        public void Update(Bakery bakery)
        {
            var existing = Get(bakery.Id);
            if (existing != null)
            {
                existing.Name = bakery.Name;
                existing.IsItWithChocolate = bakery.IsItWithChocolate;
                Console.WriteLine($"Bakery with ID {bakery.Id} updated.");
            }
        }



        public int Count { get =>  Bakeries.Count();}
    }
}