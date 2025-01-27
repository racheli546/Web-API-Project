namespace ProjectCore;
public enum CategoryBakeries
{
    Cheesecake,
    ChocolateCake,
    CarrotCake,
    VanillaCake 
}
public class Bakery
{
    public int Id { get; set; }
    public string Name { get; set; }
    public CategoryBakeries Category { get; set; }
}