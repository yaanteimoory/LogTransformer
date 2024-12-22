using System.Data;

namespace CardexLogTransformer.Business;

public class QueryBuilder : EventPublisher<QueryBuilder>
{
    public DataTable ConvertToDataTable(List<string> columns,List<Dictionary<string, string>> dataList)
    {
        TriggerBefore(this,new ());
        var dataTable = new DataTable();

        

        // Add columns to DataTable
        foreach (var column in columns)
        {
            dataTable.Columns.Add(column, typeof(string));  // Assuming all columns are string, adjust if necessary
        }

        // Add rows to DataTable
        foreach (var record in dataList)
        {
            var row = dataTable.NewRow();
            foreach (var column in columns)
            {
                record.TryGetValue(column, out var value);
                if (string.IsNullOrEmpty(value) || string.Equals("null", value, StringComparison.OrdinalIgnoreCase))
                {
                    row[column] = DBNull.Value;
                }
                else
                {
                    row[column] = value;
                }
                
            }
            dataTable.Rows.Add(row);
        }

        TriggerAfter(this,new ());
        return dataTable;
    }
}