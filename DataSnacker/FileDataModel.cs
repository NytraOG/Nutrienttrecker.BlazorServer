namespace DataSnacker;

public class FileDataModel
{
    public Guid   Oid      { get; set; }
    public int    Size     { get; set; }
    public string FileName { get; set; }
    public byte[] Content  { get; set; }
}