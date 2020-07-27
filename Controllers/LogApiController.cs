using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml.Linq;
using LogBook.Business;
using LogBook.Models.AIMS;
using LogBook.Models.LogBook;
using LogBook.Models.MaxView;
using LogBook.ViewModels.AIMS;
using LogBook.ViewModels.CyberLock;
using LogBook.ViewModels.LogBook;
using Newtonsoft.Json;
using PagedList;
using LogBook.Repositories;
using LogBook.Repositories.AIMS;
using LogBook.Repositories.MaxView;
using MaxViewLogViewModel = LogBook.ViewModels.MaxView.MaxViewLogViewModel;
using WorkorderLogViewModel = LogBook.ViewModels.AIMS.WorkorderLogViewModel;

namespace LogBook.Controllers
{
    [OpenIdAuthorize]
    [RoutePrefix("api/LogApi")]
    public class LogApiController : ApiController
    {
        public class DataResult<T>
        {
            public int Total { get; set; }
            public T[] Data { get; set; }
        }

        public string AimsApiUrl
        {
            get
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings["AimsApiUrl"];
            }
        }

        public string CyberAuditUrl
        {
            get
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings["CyberAuditUrl"];
            }
        }

        public string CyberAuditUser
        {
            get
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings["CyberAuditUser"];
            }
        }
        
        public string CyberAuditPwd
        {
            get
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings["CyberAuditPwd"];
            }
        }
        private IDatabaseStatusRepository dbStatusRepository = DatabaseStatusRepositoryFactory.Create();
        private IPhysicalLocationRepository locRepository = PhysicalLocationRepositoryFactory.Create();
        private IMaxviewUserRepository userRepository = MaxviewUserRepositoryFactory.Create();
        private ISystemDatabaseTypeRepository sysDbTypeRepository = SystemDatabaseTypeRepositoryFactory.Create();
        private ISystemDatabaseRepository sysDbRepository = SystemDatabaseRepositoryFactory.Create();

        [HttpGet]
        [Route("ReasonsForResponse")]
        //http://localhost:52920/api/LogApi/ReasonsForResponse
        public List<ReasonForResponseAPIView> ReasonsForResponse()
        {
                var db = Repositories.LogBook.ReasonForResponseRepositoryFactory.Create();
                var reasons = db.GetReasons();
                return reasons;
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        [Route("LogBookCount")]
        //http://localhost:52920/api/LogApi/LogBookCount?signalId=7220&searchText=Flash
        public int LogBookCount(string signalId, string searchText)
        {
            var db = Repositories.LogBook.LogBookRepositoryFactory.Create();
            return db.GetLogCount(signalId, searchText);
        }

        private string CyberLockAudit(string reportPartialUrl)
        {
            string result = "";
            using (WebClient client = new WebClient())
            {
                string cyberlockUser = CyberAuditUser;
                string cyberlockPwd = CyberAuditPwd;
                string credentials =
                    Convert.ToBase64String(Encoding.ASCII.GetBytes(cyberlockUser + ":" + cyberlockPwd));
                client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
                string url = CyberAuditUrl + reportPartialUrl; //"/dynamic/audit/lock/xml?lockId=";
                //lockId: L601417D8 - signalId: 6142
                try
                {
                    result = client.DownloadString(url);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }

            return result;
        }

        [HttpGet]
        [Route("CyberlockTotal")]
        //http://localhost:52920/api/LogApi/CyberlockTotal
        public int CyberlockTotal(string lockId)
        {
            int total = 0;
            total = GetTotalEventsFromLockid(lockId);
            return total;
        }

        [HttpGet]
        [Route("CyberlockTotalFromSignal")]
        //http://localhost:52920/api/LogApi/CyberlockTotalFromSignal
        public int CyberlockTotalFromSignal(string signalId)
        {
            int total = 0;
            string lockId = GetLockidFromSignalid(signalId);
            total = GetTotalEventsFromLockid(lockId);
            return total;
        }

        private int GetTotalEventsFromLockid(string lockId)
        {
            int total = 0;
            string partialUrl = "/dynamic/audit/lock/xml?lockId=" + lockId;
            string result = CyberLockAudit(partialUrl);
            if (String.IsNullOrEmpty(result))
                return total;
            try
            {
                var xDoc = XDocument.Parse(result);
                foreach (XElement element in xDoc.Descendants("event"))
                {
                    total++;
                }
            }
            catch (Exception ex)
            {

            }

            return total;

        }

        [HttpGet]
        [Route("CyberlockLogs")]
        public List<CyberLockLog> CyberlockLogs(string lockId)
        {
            List<CyberLockLog> LogList = new List<CyberLockLog>();
            LogList = GetLocklogsFromLockid(lockId);
            return LogList;
        }

        private List<CyberLockLog> GetLocklogsFromLockid(string lockId)
        {
            List<CyberLockLog> LogList = new List<CyberLockLog>();
            string partialUrl = "dynamic/audit/lock/xml?lockId=" + lockId;
            string result = CyberLockAudit(partialUrl);
            if (String.IsNullOrEmpty(result))
                return LogList;
            try
            {
                var xDoc = XDocument.Parse(result);
                var report = xDoc.Descendants("report").Single();
                var lockName = report.Element("name").Value;
                foreach (XElement element in xDoc.Descendants("event"))
                {
                    CyberLockLog newlog = new CyberLockLog();
                    newlog.Name = lockName;
                    newlog.Person_name = element.Element("person_name").Value;
                    newlog.Key_name = element.Element("key_name").Value;
                    newlog.Date = Convert.ToDateTime(element.Element("date").Value);
                    newlog.Description = element.Element("description").Value;
                    LogList.Add(newlog);
                }
            }
            catch (Exception ex)
            {

            }

            return LogList;
        }

        private string GetLockidFromSignalid(string signalId)
        {
            string lockId = "";
            string partialUrl = "lock/list";
            string result1 = CyberLockAudit(partialUrl);
            if (String.IsNullOrEmpty(result1))
                return lockId;
            try
            {
                var xDoc1 = XDocument.Parse(result1);
                foreach (XElement element in xDoc1.Descendants("lock"))
                {
                    if (element.Attribute("name").Value == signalId)
                    {
                        lockId = element.Attribute("id").Value;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return lockId;
        }

        [HttpGet]
        [Route("CyberlockLogsFromSignal")]
        public List<CyberLockLog> CyberlockLogsFromSignal(string signalId)
        {
            List<CyberLockLog> LogList = new List<CyberLockLog>();
            string lockId = GetLockidFromSignalid(signalId);
            LogList = GetLocklogsFromLockid(lockId);
            return LogList;
        }

        [HttpGet]
        [Route("CyberlockEntries")]
        //http://localhost:52920/api/LogApi/CyberlockEntries?signalId=7220&pageSize=5&pageNumber=1
        public CyberLockLogViewModel CyberlockEntries(string signalId, int pageSize, int pageNumber)
        {
            var vm = new CyberLockLogViewModel();
            vm.CyberlockLogs = new List<CyberLockLog>();
            vm.PageNumber = pageNumber;
            vm.PageSize = pageSize;
            vm.TotalNumber = 0;

            string lockId = GetLockidFromSignalid(signalId);
            if (string.IsNullOrEmpty(lockId))
            {
                return vm;
            }
            var LogList = GetLocklogsFromLockid(lockId);
            if (LogList.Count==0)
            {
                return vm;
            }

            vm.TotalNumber = LogList.Count();
            vm.CyberlockLogs = LogList.ToPagedList(pageNumber, pageSize).ToList();

            return vm;
        }

        [HttpGet]
        [Route("LogBookGet")]
        //http://localhost:52920/api/LogApi/LogBookGet?locationId=7220&pageSize=5&pageNumber=1
        public LogBookView LogBookEntries(string locationId, int pageSize, int pageNumber, string searchText)
        {
            LogBookView logViewModel = new LogBookView();
            var db = Repositories.LogBook.LogBookRepositoryFactory.Create();
            var logs = db.GetLogs(locationId, searchText);
            logViewModel.TotalNumber = logs.Count;
            logViewModel.Logs = logs.ToPagedList(pageNumber, pageSize).ToList();
            logViewModel.PageSize = pageSize;
            logViewModel.PageNumber = pageNumber;
            return logViewModel;
        }

        [HttpGet]
        [Route("LogBookReport")]
        public LogBookView LogBookReport(string locationId, string commentSearch, DateTime startDate, DateTime endDate,
            string userSearch, int onsiteRemote, int reasons, int pageRows, int pageNumber)
        {
            LogBookView logViewModel = new LogBookView();
            var db = Repositories.LogBook.LogBookRepositoryFactory.Create();
            var logs = db.SearchLogs(locationId, commentSearch, startDate, endDate, userSearch, onsiteRemote, reasons);
            logViewModel.TotalNumber = logs.Count;
            logViewModel.Logs = logs.ToPagedList(pageNumber, pageRows).ToList();
            logViewModel.PageSize = pageRows;
            logViewModel.PageNumber = pageNumber;
            return logViewModel;
        }

        [HttpPost]
        [Route("LogBook")]
        //http://localhost:52920/api/LogApi/LogBookEntries
        public void LogBookEntries(Log log)
        {
            log.Timestamp = log.Timestamp.ToLocalTime();
            var db = Repositories.LogBook.LogBookRepositoryFactory.Create();
            db.Update(log);
        }

        [HttpGet]
        [Route("MaxviewTotal")]
        //http://localhost:52920/api/LogApi/MaxviewTotal?signalId=7220

        public int MaxviewTotal(int signalId)
        {
            var systemDatabaseList = sysDbRepository.GetDownloadListFromSignalId(signalId);
            return (systemDatabaseList.Count());

        }

        [HttpGet]
        [Route("MaxviewEntries")]
        //http://localhost:52920/api/LogApi/MaxviewEntries?locationId=7220&pageSize=5&pageNumber=1

        public MaxViewLogViewModel MaxviewEntries(int signalId, int pageSize, int pageNumber)
        {
            var vm = new MaxViewLogViewModel();
            vm.PageNumber = pageNumber;
            vm.PageSize = pageSize;

            var systemDatabaseList = sysDbRepository.GetDownloadListFromSignalId(signalId);
            vm.TotalNumber = systemDatabaseList.Count();
            vm.MaxViewLogs = new List<MaxViewLog>();
            var maxViewLogs =systemDatabaseList.ToPagedList(pageNumber, pageSize).ToList();

            int listId = 0;
            foreach(var maxViewLog in maxViewLogs)
            { 
                var log = new MaxViewLog();
                log.Id = ++listId;
                log.Time = maxViewLog.StartTime;
                log.Type = sysDbTypeRepository.GetNameFromID(maxViewLog.SystemDatabaseTypeID);
                log.Status = dbStatusRepository.GetStatusName(maxViewLog.DatabaseStatusId);
                DateTime end = DateTime.Now;
                if (maxViewLog.EndTime != null)
                    end = (DateTime)maxViewLog.EndTime;
                log.Duration = end - maxViewLog.StartTime;
                if (maxViewLog.UserId is null)
                {
                    log.User = string.Empty;
                }
                else
                {
                    log.User = userRepository.GetUserName((int)maxViewLog.UserId);
                }

                log.Comment = maxViewLog.Comment;
                vm.MaxViewLogs.Add(log);
            }
            return vm;
        }

        private string GetDataResult(int SignalId, string UriBase)
        {
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Add((
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")));
                string fullUri = UriBase + SignalId.ToString();

                var response = client.GetAsync(fullUri).Result;
                var content = response.Content.ReadAsByteArrayAsync().Result;
                var decompressedData = Decompress(content);
                string jsonString = System.Text.Encoding.UTF8.GetString(decompressedData, 0, decompressedData.Length);
                return jsonString;
            }
        }

        private string GetDataResultNoDecompression(int SignalId, string UriBase)
        {
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Add((
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")));
                string fullUri = UriBase + SignalId.ToString();

                var response = client.GetAsync(fullUri).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                return content;
            }
        }

        [HttpGet]
        [Route("AIMSLocation")]
        //http://localhost:52920/api/LogApi/AIMSLocation?LocationToSearch=4850
        public LocationView AIMSLocation(string LocationToSearch)
        {
            var locationFound = new LocationView();

            int SignalId = 0;
            bool isNumeric = int.TryParse(LocationToSearch, out SignalId);

            if (isNumeric)
            {
                string uriBase = AimsApiUrl +
                    @"locations/ReadByFilter?Page=1&PageSize=200&SignalId=";

                var myResult = new DataResult<PhysicalLocation>();
                var jsonString = GetDataResultNoDecompression(SignalId, uriBase);
                myResult = JsonConvert.DeserializeObject<DataResult<PhysicalLocation>>(jsonString);
                if (myResult.Total > 0)
                {
                    locationFound.LocationIdentifier = LocationToSearch;
                    locationFound.LocationType = "Signal";
                    locationFound.Name = myResult.Data[0].Name;
                }

            }
            else
            {
                PhysicalLocation locFound = locRepository.GetLocationForRWIS(LocationToSearch);

                if (locFound != null)
                {
                    locationFound.LocationIdentifier = LocationToSearch;
                    locationFound.LocationType = "RWIS";
                    locationFound.Name = locFound.Name;
                }
            }

            return locationFound;
        }

        [HttpGet]
        [Route("SignalLocation")]
        //http://localhost:52920/api/LogApi/SignalLocation?locationId=4850
        public LocationView SignalLocation(int SignalId)
        {
            string uriBase = AimsApiUrl + 
                @"locations/ReadByFilter?Page=1&PageSize=200&SignalId=";

            var myResult = new DataResult<PhysicalLocation>();
            var jsonString = GetDataResultNoDecompression(SignalId, uriBase);
            myResult = JsonConvert.DeserializeObject<DataResult<PhysicalLocation>>(jsonString);
            var locationFound = new LocationView();
            if (myResult.Total > 0)
            {
                locationFound.LocationIdentifier = SignalId.ToString();
                locationFound.LocationType = "Signal";
                locationFound.Name = myResult.Data[0].Name;
            }

            return locationFound;
        }

        [HttpGet]
        [Route("AIMSTotal")]
        //http://localhost:52920/api/LogApi/AIMSTotal?location=UT3
        public int AIMSTotal(string location)
        {
            string jsonString = "";
            var myResult = new DataResult<AIMSWorkOrders>();
            int signalId = 0;
            bool isNumeric = int.TryParse(location.Trim(), out signalId);

            if (isNumeric) //Signal
            {
                string uriBase = AimsApiUrl + @"workOrder/read?signalId=";
                jsonString = GetDataResult(signalId, uriBase);
            }
            else //RWIS
            {
                PhysicalLocation locFound = locRepository.GetLocationForRWIS(location);
                if (locFound != null)
                {
                    string uriBase = AimsApiUrl + @"workOrder/read?locationId=";
                    jsonString = GetDataResult(locFound.IntId, uriBase);
                }
                else return 0;  // no location found
            }
            myResult = JsonConvert.DeserializeObject<DataResult<AIMSWorkOrders>>(jsonString);

            return myResult.Total;
        }

        //[HttpGet]
        //[Route("AIMSEntries")] //http://localhost:52920/api/LogApi/AIMSEntries?location=7220&pageSize=5&pageNumber=1

        //public AIMSLogViewModel AIMSEntries(string location, int pageSize, int pageNumber)
        //{
        //    var vm = new AIMSLogViewModel();
        //    vm.PageNumber = pageNumber;
        //    vm.PageSize = pageSize;
        //    var logs = new List<AIMSWorkOrders>();
        //    var myResult = new DataResult<AIMSWorkOrders>();
        //    string jsonString = "";

        //    int locationId = 0;
        //    bool isNumeric = int.TryParse(location.Trim(), out locationId);

        //    if (isNumeric) //Signal
        //    {
        //        string uriBase = AimsApiUrl + @"workOrder/read?locationId=";
        //        jsonString = GetDataResult(locationId, uriBase);
        //    }
        //    else //RWIS
        //    {
        //        PhysicalLocation locFound = locRepository.GetLocationForRWIS(location);
        //        string uriBase = AimsApiUrl + @"workOrder/read?locationId=";
        //        jsonString = GetDataResult(locFound.IntId, uriBase);
        //    }

        //    myResult = JsonConvert.DeserializeObject<DataResult<AIMSWorkOrders>>(jsonString);
        //    var allLogs = myResult.Data.ToList();
        //    vm.TotalNumber = myResult.Total;

        //    if (pageNumber < 1 || pageSize == 0 || pageNumber > Math.Ceiling((float)vm.TotalNumber / pageSize))
        //    {
        //        return vm;
        //    }

        //    int ListId = 0;
        //    int pageLimit = pageNumber * pageSize;
        //    for (int i = (pageNumber - 1) * pageSize; (i < vm.TotalNumber && i < pageLimit); i++)
        //    {
        //        var log = allLogs[i];
        //        logs.Add(log);
        //    }

        //    vm.AIMSLogs = logs;
        //    return vm;
        //}

        [HttpGet]
        [Route("WorkorderViewEntries")] //http://localhost:52920/api/LogApi/WorkorderViewEntries?location=7220&pageSize=5&pageNumber=1

        public WorkorderLogViewModel WorkorderViewEntries(string location, int pageSize, int pageNumber)
        {
            var vm = new WorkorderLogViewModel();
            vm.PageNumber = pageNumber;
            vm.PageSize = pageSize;
            var logs = new List<AIMSWorkOrders>();
            var viewLogs = new List<WorkorderView>();
            var myResult = new DataResult<AIMSWorkOrders>();

            string jsonString = "";

            int signalId = 0;
            bool isNumeric = int.TryParse(location.Trim(), out signalId);

            if (isNumeric) //Signal
            {
                string uriBase = AimsApiUrl + @"workOrder/read?signalId=";
                jsonString = GetDataResult(signalId, uriBase);
            }
            else //RWIS
            {
                PhysicalLocation locFound = locRepository.GetLocationForRWIS(location);
                string uriBase = AimsApiUrl + @"workOrder/read?locationId=";
                jsonString = GetDataResult(locFound.IntId, uriBase);
            }

            myResult = JsonConvert.DeserializeObject<DataResult<AIMSWorkOrders>>(jsonString);
            var allLogs = myResult.Data.ToList();
            vm.TotalNumber = myResult.Total;

            if (pageNumber < 1 || pageSize == 0 || pageNumber > Math.Ceiling((float)vm.TotalNumber / pageSize))
            {
                return vm;
            }

            int ListId = 0;
            int pageLimit = pageNumber * pageSize;
            for (int i = (pageNumber - 1) * pageSize; (i < vm.TotalNumber && i < pageLimit); i++)
            {
                var log = allLogs[i];
                var viewLog = new WorkorderView();
                viewLog.ExtId = log.ExtId;
                viewLog.IntId = log.IntId;
                viewLog.ShortDescription = log.ShortDescription;
                viewLog.DateCreated = log.DateCreated;
                viewLog.CreatedByUserName = log.CreatedBy.Name;
                viewLog.Status = log.Status.Abbreviation;
                viewLogs.Add(viewLog);
            }

            vm.WorkorderLogs = viewLogs;
            return vm;
        }


        static byte[] Decompress(byte[] gzip)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }
    }
}
