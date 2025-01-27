using System.Collections.Generic;
using System.Linq;
using ProjectCore;

namespace ProjectCore.Interfaces
{
    public interface IBakeryService
    {
        List<Bakery> GetAll();

        Bakery ?Get(int id);

        void Add(Bakery bakery);

        void Delete(int id);

        void Update(Bakery bakery);

        int Count { get;}
    }
}
