using ProjectCore;
using ProjectCore.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;


namespace ProjectCore.Services;

public class UserService : IUserService
{
    private List<User> ListUsers;
    private string FilePath = "Users.json";
    private int nextId;
    public UserService(IWebHostEnvironment webHost)
    {
        this.FilePath = Path.Combine(webHost.ContentRootPath, "Data", "Users.json");
        using (var jsonFile = File.OpenText(FilePath))
        {
            ListUsers = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        Console.WriteLine("✅ משתמשים נטענו מהקובץ:");
        foreach (var user in ListUsers)
        {
            Console.WriteLine($"🔹 {user.Id} - {user.Username} - {user.Password}");
        }
    }

    private readonly List<User> _users = new()
        {
            new User { Id = 1, Username = "admin", Password = "1234", Role = "Admin" },
            new User { Id = 2, Username = "user", Password = "1234", Role = "User" }
        };

    public User Authenticate(string username, string password)
    {
        Console.WriteLine($"🔍 מנסים למצוא משתמש עם שם: '{username}' וסיסמה: '{password}'");

        var user = ListUsers.FirstOrDefault(u =>
            u.Username.Trim().ToLower() == username.Trim().ToLower() &&
            u.Password.Trim() == password.Trim()
        );

        if (user == null)
        {
            Console.WriteLine("❌ משתמש לא נמצא או סיסמה שגויה");
        }
        else
        {
            Console.WriteLine($"✅ משתמש נמצא: {user.Id} - {user.Username}");
        }

        return user;
    }
    private void saveToFile()
    {
        try
        {
            File.WriteAllText(FilePath, JsonSerializer.Serialize(ListUsers, new JsonSerializerOptions { WriteIndented = true }));
            Console.WriteLine("✅ שמירת משתמשים בוצעה בהצלחה");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ שגיאה בשמירת משתמשים לקובץ: {ex.Message}");
        }
    }

    public List<User> GetAll() => ListUsers;

    public User Get(int id)
    {
        if (ListUsers == null)
        {
            Console.WriteLine("❌ Get(int id): הרשימה ריקה!");
            return null;
        }

        var user = ListUsers.FirstOrDefault(p => p.Id == id);
        if (user == null)
        {
            Console.WriteLine($"❌ Get(int id): לא נמצא משתמש עם ID = {id}");
        }

        return user;
    }

    public void Add(User user)
    {
        if (ListUsers == null)
        {
            Console.WriteLine("⚠ הרשימה ריקה - יוצרים רשימה חדשה");
            ListUsers = new List<User>();
        }

        user.Id = ListUsers.Any() ? ListUsers.Max(u => u.Id) + 1 : 1;
        ListUsers.Add(user);

        saveToFile();
        Console.WriteLine($"✅ משתמש נוסף בהצלחה: {user.Username} (ID = {user.Id})");
    }

    public void Delete(int id)
    {
        var User = Get(id);
        if (User is null)
            return;
        ListUsers.Remove(User);
        saveToFile();
    }

    public void Update(User user)
    {
        var index = ListUsers.FindIndex(p => p.Id == user.Id);
        if (index == -1)
            return;
        ListUsers[index].Username = user.Username;
        ListUsers[index].Password = user.Password;
        saveToFile();
    }
    public void BuyBakery(int userId, Bakery bakery)
    {
        var user = Get(userId);
        if (user != null)
        {
            user.PurchasedBakeries.Add(bakery);
            Update(user);
        }
    }

    public void RemoveBakery(int userId, int bakeryId)
    {
        var user = Get(userId);
        if (user != null)
        {
            var bakery = user.PurchasedBakeries.FirstOrDefault(b => b.Id == bakeryId);
            if (bakery != null)
            {
                user.PurchasedBakeries.Remove(bakery);
                Update(user);
            }
        }
    }

}

