using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardexLogTransformer.Models
{
    public class LogItem
    {
        public required bool IsEnable { get; set; }
        public required string ColumnName { get; set; }
        public required ColumnType ColumnType { get; set; }
    }
}
