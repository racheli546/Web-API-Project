using System.Collections.Generic;
using System.Linq;
using ProjectCore;

namespace ProjectCore.Interfaces
{
    public interface IUserService
    {
        List<User> GetAll();

        User ?Get(int id);

        void Add(User user);

        void Delete(int id);

        void Update(User user);

        // int Count { get;}
    }
}
