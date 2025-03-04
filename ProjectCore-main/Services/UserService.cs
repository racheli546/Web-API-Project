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
        private  string FilePath = "Users.json";
        private int nextId;
        public UserService(IWebHostEnvironment webHost)
        {
            this.FilePath = Path.Combine(webHost.ContentRootPath, "Data", "Users.json");
            using (var jsonFile = File.OpenText(FilePath))
            {
                ListUsers = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        //  public UserService()
        // {
        //     ListUsers = LoadUsersFromFile();
        //     nextId = ListUsers.Any() ? ListUsers.Max(u => u.Id) + 1 : 1;
        // }
    private void saveToFile()
    {
        File.WriteAllText(FilePath, JsonSerializer.Serialize(ListUsers));
    }
        // private List<User> LoadUsersFromFile()
        // {
        //     if (File.Exists(FilePath))
        //     {
        //         var json = File.ReadAllText(FilePath);
        //         return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        //     }
        //     return new List<User>();
        // }
        // private void SaveUsersToFile()
        // {
        //     var json = JsonSerializer.Serialize(ListUsers);
        //     File.WriteAllText(FilePath, json);
        // }
        // public UserService()
        // {
        //     ListUsers = new List<User>
        //     {
        //     new User { Id = 0, Username = "admin", Password = "1111", Role = "Admin" },
        //     new User { Id = 1, Username = "Nechama", Password = "1111", Role = "User" },
        //     new User { Id = 2, Username = "Hodaya", Password = "2222", Role = "User" },
        //     new User { Id = 3, Username = "Anjel", Password = "3333", Role = "User" },
        //     new User { Id = 4, Username = "Berman", Password = "4444", Role = "User" }
        //     };
        // }

        public List<User> GetAll() => ListUsers;
        // public User? Get(int id) => ListUsers.FirstOrDefault(b => b.Id == id);
        public User Get(int id)
        {
            return ListUsers.FirstOrDefault(p => p.Id == id);
        }
        public void Add(User user)
        {
            user.Id = ListUsers.Max(u => u.Id) + 1;
            ListUsers.Add(user);
            saveToFile();
        }
       

        // public void Delete(int id)
        // {
        //     var user = Get(id);
        //     if (user is null)
        //         return;

        //     ListUsers.Remove(user);
        // }
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
        // public bool VerifyPassword(string password, string storedPassword)
        // {
        //     return password == storedPassword;
        // }

        // internal static ActionResult<List<User>> GetAllUsers()
        // {
        //     throw new NotImplementedException();
        // }


        // public int Count => ListUsers.Count;
        // public int Count { get => ListUsers.Count(); }
    }

    // public static class UserServiceHelper
    // {
    //     public static void AddUserService(this IServiceCollection services)
    //     {
    //         services.AddSingleton<IUserService , UserService>();    
    //     }
    // }
