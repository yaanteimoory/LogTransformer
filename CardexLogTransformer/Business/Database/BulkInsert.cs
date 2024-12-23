using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardexLogTransformer.Business.Database
{
    public class BulkInsert : IDatabase
    {
        private readonly DataTable Table = new DataTable();
        private List<string>? Columns {  get; set; }

        public void AddColumns(List<string> columns)
        {
            Columns = columns;
            foreach (var column in columns)
            {
                Table.Columns.Add(column, typeof(string));  // Assuming all columns are string, adjust if necessary
            }
        }

        public void AddRows(IEnumerable<Dictionary<string, string>> rows)
        {
            if (Columns.IsNullOrEmpty() || rows.IsNullOrEmpty()) return;
            
            foreach (var record in rows)
            {
                var row = Table.NewRow();
                foreach (var column in Columns!)
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


                Table.Rows.Add(row);
            }
        }

        public void InsertToDatabase()
        {
            
        }
    }
}
