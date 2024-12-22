using CardexLogTransformer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CardexLogTransformer.Business
{
    public class LogConvertor
    {

        public class LogType
        {
            public required int Proiery { get; set; }
            public string? Pattern {  get; set; }

        }

        public required LogType[] LogTypes { get; init; }

        public LogItem Convert(string log)
        {
            var logType = DetermineLogType(log);


            return null;
        }

        private LogType DetermineLogType(string log)
        {
            LogType? defaultLogType = null;
            foreach (var item in LogTypes)
            {
                if(item.Pattern == null)
                {
                    defaultLogType = item;
                }
                else
                {
                    if (Regex.IsMatch(log, item.Pattern!)) return item;
                }
                
            }

            
            return defaultLogType ?? throw new Exception("Need Defualt LogTyp");
        }

    }
}
