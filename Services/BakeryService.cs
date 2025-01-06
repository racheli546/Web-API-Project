using OurBakeryStore.Models;
using OurBakeryStore.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace OurBakeryStore.Services
{
    public class BakeryService :IBakeryService
    {
        List<Bakery> Bakerys { get; }
        int nextId = 3;
        public BakeryService()
        {
            Bakerys = new List<Bakery>
            {
                new Bakery { Id = 1, Name = "Classic Italian", IsItWithChocolate = false },
                new Bakery { Id = 2, Name = "Veggie", IsItWithChocolate = true }
            };
        }

        public List<Bakery> GetAll() => Bakerys;
        public Bakery Get(int id) => Bakerys.FirstOrDefault(p => p.Id == id);

        public void Add(Bakery Bakery)
        {
            Bakery.Id = nextId++;
            Bakerys.Add(Bakery);
        }

        public void Delete(int id)
        {
            var Bakery = Get(id);
            if(Bakery is null)
                return;

            Bakerys.Remove(Bakery);
        }

        public void Update(Bakery Bakery)
        {
            var index = Bakerys.FindIndex(p => p.Id == Bakery.Id);
            if(index == -1)
                return;

            Bakerys[index] = Bakery;
        }

        public int Count { get =>  Bakerys.Count();}
    }
}