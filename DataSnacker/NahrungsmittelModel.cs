namespace DataSnacker;

public class NahrungsmittelModel
{
    public Guid   Oid      { get; set; }
    public string Name     { get; set; }
    public double Kcal     { get; set; }
    public double Protein  { get; set; }
    public double Carbs    { get; set; }
    public double Fat      { get; set; }
    public Guid   File     { get; set; }
    public int    Size     { get; set; }
    public string FileName { get; set; }
    public byte[] Content  { get; set; }
}