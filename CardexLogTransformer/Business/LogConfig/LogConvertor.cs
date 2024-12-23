using CardexLogTransformer.Business.LogConfig;
using CardexLogTransformer.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CardexLogTransformer.Business
{
    public class LogConvertor
    {

        public required LogPattern[] LogTypes { get; init; }


        public GroupCollection ExtractData(string log)
        {
            foreach (var logType in LogTypes)
            {
                //11:43:08.8284188 - ⌂{ "ThreadId":291,"User":"s.taghigil","Time":"2024-12-14T11:43:08.8284188+03:30","Action":13,"CardexInfo":{ "ItemType":2,"TypeOfEffectOnStock":1,"PartID":681,"StoreID":20,"PlantID":26,"TrackingFactor1Value":"250000","TrackingFactor2Value":null,"TrackingFactor3Value":null,"TrackingFactor4Value":null,"TrackingFactor5Value":null,"HasSecondUnit":false},"FiscalYearRef":1403,"Description":null,"MajorUnitQuantity":null,"SecondUnitQuantity":null,"Date":null,"CreationDate":null,"CacheMajorUnitQuantity":1320.000000,"CacheSecondUnitQuantity":0.0,"CacheDate":"2024-12-14T00:00:00+03:30","CacheCreationDate":"2024-12-14T11:18:48.513","ParticipantMajorUnitQuantity":null,"ParticipantSecondUnitQuantity":null,"ParticipantDate":null,"ParticipantCreationDate":null,"DatabaseMajorUnitQuantity":null,"DatabaseSecondUnitQuantity":null,"DatabaseDate":null,"DatabaseCreationDate":null,"TotalUncommittedChangeMajorUnitQuantity":12.000000,"TotalUncommittedChangeSecondUnitQuantity":0.0,"TotalUncommittedChangeDate":"2024-12-12T00:00:00","TotalUncommittedChangeCreationDate":"2024-12-14T11:43:08.703","AppliedChanges":null,"UncommittedConsumers":{ },"UncommittedRequests":["04a424fe-901f-41f4-9e53-d9c79d0f2bc9"]}⌂ [User: s.taghigil(172.17.56.7); Trace: 'a207d82d-d8ce-4fc9-b685-a765a0d17116'; Priority: Medium; ThreadId: 291; ThreadName: '']
                //^(?<LogTime>\d{2}:\d{2}:\d{2}\.\d+).*?"ThreadId":(?<ThreadId>\d+).*?"User":"(?<User>[^"]+)".*?"Time":"(?<Time>[^"]+)".*?"Action":(?<Action>\d+).*?"ItemType":(?<ItemType>\d+).*?"TypeOfEffectOnStock":(?<TypeOfEffectOnStock>\d+).*?"PartID":(?<PartID>\d+).*?"StoreID:(?<StoreID>\d+).*?"PlantID":(?<PlantID>\d+)
                //.*?"TrackingFactor1Value":"?(?<TrackingFactor1Value>null|[^"]+).*?"TrackingFactor2Value":"?(?<TrackingFactor2Value>null|[^"]+).*?"TrackingFactor3Value":"?(?<TrackingFactor3Value>null|[^"]+).*?"TrackingFactor4Value":"?(?<TrackingFactor4Value>null|[^"]+).*?"TrackingFactor5Value":"?(?<TrackingFactor5Value>null|[^"]+).*?"HasSecondUnit":(?<HasSecondUnit>true|false).*?"FiscalYearRef":(?<FiscalYearRef>\d+).*?"Description":(?<Description>.+?),"MajorUnitQuantity":(?<MajorUnitQuantity.+?),"SecondUnitQuantity":(?<SecondUnitQuantity.+?)
                var pattern = logType.Pattern;
                var match = Regex.Match(log, pattern, logType.RegexOption);
                if (match.Success)
                {
                    return match.Groups;
                }

            }

            throw new NoNullAllowedException($"No pattern found for: {log}");
        }

        static void Test()
        {
            var data = new Dictionary<string, string>();
            var logs = new string[] {
                @"11:43:08.8284188 - ⌂{ ""ThreadId"":null,""User"":""s.taghigil"",""Time"":""2024-12-14T11:43:08.8284188+03:30""",
                @"11:43:08.8284188 - ⌂{ ""ThreadId"":""Ali"",""User"":""s.taghigil"",""Time"":""2024-12-14T11:43:08.8284188+03:30"""
            };
            var pattern = @"^(?<LogTime>\d{2}:\d{2}:\d{2}\.\d+).*?""ThreadId"":(?<ThreadId>[^""])";

            foreach (var log in logs)
            {
                
                
                var match = Regex.Match(log, pattern, RegexOptions.Singleline);
                if (match.Success)
                {
                    data["LogTime"] = match.Groups["LogTime"].Value;
                    data["ThreadId"] = match.Groups["ThreadId"].Value;
                }
                if (!data.Any())
                {
                    Console.WriteLine("NOTHING FOUND");
                }
                else
                {
                    foreach (var item in data)
                    {
                        Console.WriteLine($"{item.Key} is {item.Value}");
                    }
                }
                Console.WriteLine();
            }
        }

        public IEnumerable<GroupCollection> ExtractData(IEnumerable<string> logs) => logs.Select(selector: ExtractData);

    }
}
