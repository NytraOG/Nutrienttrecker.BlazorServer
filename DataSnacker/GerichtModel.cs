namespace DataSnacker;

public class GerichtModel
{
    public Guid   Oid     { get; set; }
    public string Name    { get; set; }
    public double Kcal    { get; set; }
    public double Protein { get; set; }
    public double Fat     { get; set; }
    public double Carbs   { get; set; }
}