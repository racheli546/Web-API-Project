using System.Collections.Generic;
using System.Linq;
using ProjectCore;

namespace ProjectCore.Interfaces
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        List<User> GetAll();

        User ?Get(int id);

        void Add(User user);

        void Delete(int id);

        void Update(User user);
         // ✅ הוספת קניית מאפה
        void BuyBakery(int userId, Bakery bakery);

        // ✅ הסרת מאפה מרשימת המשתמש
        void RemoveBakery(int userId, int bakeryId);

        // int Count { get;}
    }
}
