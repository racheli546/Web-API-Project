using OurBakeryStore.Models;
using System.Collections.Generic;
using System.Linq;

namespace OurBakeryStore.Interfaces
{
    public interface IBakeryService
    {
        List<Bakery> GetAll();

        Bakery Get(int id);

        void Add(Bakery pizza);

        void Delete(int id);

        void Update(Bakery pizza);

        int Count { get;}
    }
}