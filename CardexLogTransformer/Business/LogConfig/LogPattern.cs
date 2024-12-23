using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CardexLogTransformer.Business.LogConfig
{
    public class LogPattern
    {
        public required string Pattern { get; set; }

        public string? RegexOptionValue { get; set; }

        public RegexOptions RegexOption =>
            Enum.TryParse(typeof(RegexOptions), RegexOptionValue, out var result)
            ? (RegexOptions)result
            : RegexOptions.None;

    }
}
