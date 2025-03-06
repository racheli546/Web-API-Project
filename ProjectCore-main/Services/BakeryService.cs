using ProjectCore;
using ProjectCore.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;



namespace ProjectCore.Services;

public class BakeryService : IBakeryService
{
    List<Bakery> ListBakeries { get; set;}
    private string fileName = "Bakery.json";

    int nextId = 0;
    // public BakeryService(IWebHostEnvironment webHost)
    // {
    //     this.fileName = Path.Combine(webHost.ContentRootPath, "Data", "Bakery.json");
    //     if (File.Exists(fileName))
    //     {
    //         using (var jsonFile = File.OpenText(fileName))
    //         {
    //             var content = jsonFile.ReadToEnd();
    //             ListBakeries = JsonSerializer.Deserialize<List<Bakery>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Bakery>();
    //         }
    //     }
    //     else
    //     {
    //         ListBakeries = new List<Bakery>();
    //     }
    //     // ListBakeries = new List<Bakery>
    //     // {
    //     // new Bakery { Id = 1, Name = "Cheeses cake" ,Category=CategoryBakeries.Cheesecake},
    //     // new Bakery { Id = 2, Name = "Chocolates Cake", Category = CategoryBakeries.ChocolateCake},
    //     // new Bakery { Id = 3, Name = " Carrots Cake", Category = CategoryBakeries.CarrotCake},
    //     // new Bakery { Id = 4, Name = " Vanillas Cake", Category = CategoryBakeries.VanillaCake}
    //     // };
    // }
    public BakeryService(IWebHostEnvironment webHost)
    {
        this.fileName = Path.Combine(webHost.ContentRootPath, "Data", "Bakery.json");

        if (!File.Exists(fileName))
        {
            Console.WriteLine("⚠ Bakery.json לא קיים - יוצרים רשימה ריקה");
            ListBakeries = new List<Bakery>();
            saveToFile(); // ניצור קובץ חדש
        }
        else
        {
            try
            {
                var json = File.ReadAllText(fileName);
                ListBakeries = JsonSerializer.Deserialize<List<Bakery>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Bakery>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ שגיאה בטעינת מאפיות: {ex.Message}");
                ListBakeries = new List<Bakery>(); // נימנע מקריסה
            }
        }
    }


    // private void saveToFile()
    // {
    //     File.WriteAllText(fileName, JsonSerializer.Serialize(ListBakeries));
    // }

    private void saveToFile()
    {
        try
        {
            File.WriteAllText(fileName, JsonSerializer.Serialize(ListBakeries, new JsonSerializerOptions { WriteIndented = true }));
            Console.WriteLine("✅ שמירת מאפיות בוצעה בהצלחה");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ שגיאה בשמירת מאפיות לקובץ: {ex.Message}");
        }
    }

    // public List<Bakery> GetAll()
    // {
    //     if (ListBakeries == null || !ListBakeries.Any())
    //     {
    //         Console.WriteLine("❌ Bakery list is empty or null!");
    //     }
    //     return ListBakeries ?? new List<Bakery>();
    // }

    public List<Bakery> GetAll()
    {
        if (ListBakeries == null)
        {
            Console.WriteLine("⚠ הרשימה הייתה NULL - יוצרים רשימה ריקה");
            ListBakeries = new List<Bakery>();
        }

        if (!ListBakeries.Any())
        {
            Console.WriteLine("⚠ אין מאפיות זמינות - הרשימה ריקה.");
        }
        
        return ListBakeries;
    }

    // public Bakery? Get(int id) => ListBakeries.FirstOrDefault(b => b.Id == id);
    public Bakery Get(int id) => ListBakeries.FirstOrDefault(t => t.Id == id);

    // public Bakery Get(int id)
    // {
    //     var Bakery = ListBakeries.FirstOrDefault(b => b.Id == id);
    //     if (Bakery == null)
    //     {
    //         throw new IndexOutOfRangeException($"Bakery with ID {id} does not exist.");
    //     }
    //     return Bakery;
    // }
//  public void Add(Bakery bakery)
//     {
//         bakery.Id = nextId;
//         ListBakeries.Add(bakery);
//         nextId++;
//     }

    // public void Add(Bakery bakery)
    // {
    //     bakery.Id = ListBakeries.Max(t => t.Id) + 1;
    //     ListBakeries.Add(bakery);
    //     // console.log("racheli ", bakery.Id);
    //     saveToFile();
    // }
    public void Add(Bakery bakery)
    {
        if (ListBakeries == null)
        {
            Console.WriteLine("⚠ הרשימה הייתה NULL - יוצרים רשימה חדשה");
            ListBakeries = new List<Bakery>();
        }

        bakery.Id = ListBakeries.Any() ? ListBakeries.Max(t => t.Id) + 1 : 1;
        ListBakeries.Add(bakery);
        saveToFile();
        Console.WriteLine($"✅ מאפייה נוספה בהצלחה: {bakery.Name} (ID = {bakery.Id})");
    }


    // public void Update(Bakery bakery)
    // {
    //     var index = ListBakeries.FindIndex(b => b.Id == bakery.Id);
    //     if (index == -1)
    //         return;

    //     ListBakeries[index] = bakery;
    // }
    public void Update(Bakery bakery)
    {
        var index = ListBakeries.FindIndex(b => b.Id == bakery.Id);
        if (index == -1)
            return;

        ListBakeries[index]= bakery;
        saveToFile();
    }
    public void Delete(int id)
    {
        var bakery = Get(id);
        if (bakery is null)
            return;
        ListBakeries.Remove(bakery);
        saveToFile();
    }
    public int Count { get => ListBakeries.Count(); }

}
// public void Delete(int id)
// {
//     var bakery = Get(id); // השיטה Get כבר תזרוק שגיאה אם הספר לא נמצא
//     ListBakeries.Remove(bakery);
// }





// public static class BakeryServiceHelper
// {
//     public static void AddBakeryService(this IServiceCollection services)
//     {
//         services.AddSingleton<IBakeryService, BakeryService>();
//     }
// }
