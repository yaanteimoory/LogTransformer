using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardexLogTransformer.Models
{
    public class LogItem
    {
        public  bool IsEnable { get; set;  }
        public  string ColumnName { get; set; }
        public ColumnType ColumnType { get; set; }
    }
}
