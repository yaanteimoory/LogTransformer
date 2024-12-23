using Microsoft.Data.SqlClient;
using System.CodeDom;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace CardexLogTransformer.Business;

public interface IDatabase
{
    protected void AddColumns(List<string> columns);
    protected void AddRows(IEnumerable<Dictionary<string,string>> rows);
    protected void InsertToDatabase(string connectionString);

    private static void CheckConnection(string connectionString)
    {
        using SqlConnection connection = new(connectionString);
        connection.Open();
    }

    
    public void InsertDataToSqlServer(List<string> columns, IEnumerable<Dictionary<string, string>> rows, string connectionString)
    {
        CheckConnection(connectionString);

        AddColumns(columns);
        AddRows(rows);
        InsertToDatabase(connectionString);


    }


    static Dictionary<string, string> ExtractLogValues(string log)
    {
        var result = new Dictionary<string, string>();

        // Single regex pattern to capture LogTime, ThreadId, Time, and Action
        string pattern = @"^(?<LogTime>\d{2}:\d{2}:\d{2}\.\d+).*?""ThreadId"":(?<ThreadId>\d+).*?""Time"":""(?<Time>[^""]+)"".*?""Action"":(?<Action>\d+)";

        var match = Regex.Match(log, pattern, RegexOptions.Singleline);
        if (match.Success)
        {
            result["LogTime"] = match.Groups["LogTime"].Value;
            result["ThreadId"] = match.Groups["ThreadId"].Value;
            result["Time"] = match.Groups["Time"].Value;
            result["Action"] = match.Groups["Action"].Value;
        }

        return result;
    }
}