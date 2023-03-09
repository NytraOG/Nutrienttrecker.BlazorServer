// See https://aka.ms/new-console-template for more information

using System.Data.SqlClient;
using System.Reflection;
using Dapper;
using DataSnacker;
using Newtonsoft.Json;

var connectionString = "Integrated Security=SSPI;Pooling=false;Data Source=DESKTOP-7FA7F9C\\MSSQLSERVER2019;Initial Catalog=NyTEC.NutrientTrecker_Local";

using var connection = new SqlConnection(connectionString);

var  nahrungsmittelJson = QuetschDasJsonRaus<NahrungsmittelModel>(connection, "Select *, n.Fett as 'Fat' from Nahrungsmittel n left join FileData f on f.Oid = n.Datei");
// var  gerichtJson = QuetschDasJsonRaus<GerichtModel>(connection, "Select *, Fett as 'Fat' from Gericht");
// var  fileJson    = QuetschDasJsonRaus<FileDataModel>(connection, "Select * from FileData");

MachDasJsonInDieFile(nahrungsmittelJson, "Nahrungsmittel.json");
// MachDasJsonInDieFile(gerichtJson, "Gerichte.json");
// MachDasJsonInDieFile(fileJson, "Files.json");

Console.WriteLine("Feddich wie'n rettich");
Console.ReadKey();

string QuetschDasJsonRaus<T>(SqlConnection sqlConnection, string query)
{
    var queryResult = sqlConnection.Query<T>(query);
    return JsonConvert.SerializeObject(queryResult);
}

void MachDasJsonInDieFile(string content, string fileName)
{
    var dataPath = GetPath();
    Console.WriteLine($"Path: {dataPath}");

    var filePath = Path.Combine(dataPath, fileName);
    Console.WriteLine($"FilePath: {filePath}");
    Console.WriteLine($"Schreibe in die File rein...{Environment.NewLine}");

    File.WriteAllText(filePath, content);
}

string GetPath()
{
    var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);

    if (basePath is null)
        throw new DirectoryNotFoundException("Directory weg :C");

    var localPath = new Uri(basePath).LocalPath;

    return Path.Combine(localPath, "Data");
}