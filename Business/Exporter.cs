using System.Collections.Generic;
using System.IO;
using CsvHelper;
using LogBook.Models.LogBook;
using LogBook.ViewModels.LogBook;

namespace LogBook.Business
{
    public class Exporter
    {

        //public static byte[] GetCsvFile(IEnumerable records)
        //{
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        using (var streamWriter = new StreamWriter(memoryStream))
        //        using (var csv = new CsvWriter(streamWriter))
        //        {

        //            csv.Configuration.RegisterClassMap<Log.LogClassMap>();
        //        }
        //        return memoryStream.ToArray();
        //    }
        //}

        public static byte[] GetLogFile(List<LogView> records)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                using (var csv = new CsvWriter(streamWriter))
                {
                    csv.Configuration.RegisterClassMap<Log.LogClassMap>();
                    csv.WriteHeader<Log>();
                    csv.NextRecord();
                    foreach (var record in records)
                    {
                        //csv.WriteField(record.Id);
                        csv.WriteField(record.Timestamp.ToString("MM/dd/yyyy HH:mm:ss"));
                        csv.WriteField(record.LocationId);
                        csv.WriteField(record.Intersection);
                        //csv.WriteField(record.DateCreated.ToString("MM/dd/yyyy HH:mm:ss"));
                        csv.WriteField(record.OnsiteOrRemote);
                        csv.WriteField(record.Comment);
                        csv.WriteField(record.User);
                        csv.WriteField(record.ReasonForResponseCommaSeparated);
                        csv.NextRecord();
                    }
                }
                return memoryStream.ToArray();
            }
        }
    }
}