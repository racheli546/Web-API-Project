using ProjectCore;
using ProjectCore.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;



namespace ProjectCore.Services
{
    public class BakeryService : IBakeryService
    {
        List<Bakery> ListBakeries { get; }
        int nextId = 4;
        public BakeryService()
        {
            ListBakeries = new List<Bakery>
            {
            new Bakery { Id = 1, Name = "Cheeses cake" ,Category=CategoryBakeries.Cheesecake},
            new Bakery { Id = 2, Name = "Chocolates Cake", Category = CategoryBakeries.ChocolateCake},
            new Bakery { Id = 3, Name = " Carrots Cake", Category = CategoryBakeries.CarrotCake},
            new Bakery { Id = 4, Name = " Vanillas Cake", Category = CategoryBakeries.VanillaCake}
            };
        }

        public List<Bakery> GetAll() => ListBakeries;
        // public Bakery? Get(int id) => ListBakeries.FirstOrDefault(b => b.Id == id);
        public Bakery Get(int id)
        {
            var Bakery = ListBakeries.FirstOrDefault(b => b.Id == id);
            if (Bakery == null)
            {
                throw new IndexOutOfRangeException($"Bakery with ID {id} does not exist.");
            }
            return Bakery;
        }

        public void Add(Bakery bakery)
        {
            bakery.Id = nextId;
            ListBakeries.Add(bakery);
            nextId++;   
        }

        // public void Delete(int id)
        // {
        //     var bakery = Get(id);
        //     if (bakery is null)
        //         return;

        //     Listbakeries.Remove(bakery);
        // }
        public void Delete(int id)
        {
          var bakery = Get(id); // השיטה Get כבר תזרוק שגיאה אם הספר לא נמצא
          ListBakeries.Remove(bakery);
        }

        public void Update(Bakery bakery)
        {
            var index = ListBakeries.FindIndex(b => b.Id == bakery.Id);
            if (index == -1)
                return;

            ListBakeries[index] = bakery;
        }

        public int Count { get => ListBakeries.Count(); }
    }

    public static class BakeryServiceHelper
    {
        public static void AddBakeryService(this IServiceCollection services)
        {
            services.AddSingleton<IBakeryService , BakeryService>();    
        }
    }
}