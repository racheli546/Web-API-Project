using ProjectCore;
using ProjectCore.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;



namespace ProjectCore.Services;

public class BakeryService : IBakeryService
{
    List<Bakery> ListBakeries { get; set; }
    private string fileName = "Bakery.json";

    int nextId = 0;
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

    public Bakery Get(int id) => ListBakeries.FirstOrDefault(t => t.Id == id);

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
    public void Update(Bakery bakery)
    {
        var index = ListBakeries.FindIndex(b => b.Id == bakery.Id);
        if (index == -1)
            return;

        ListBakeries[index] = bakery;
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
