using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using LogBook.Business;
using LogBook.Models;
using LogBook.Models.LogBook;
using LogBook.ViewModels.LogBook;
using LogBook.ViewModels.LogsReport;

namespace LogBook.Controllers
{
    [OpenIdAuthorize(Roles = "Reports")]
    public class LogsReportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public string WebAPI
        {
            get
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings["WebAPIURL"];
            }
        }
        public string PageRows
        {
            get
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings["ReportPageRows"];
            }
        }

        public string FileMaxRows
        {
            get
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings["FileMaxRows"];
            }
        }
        

        public async Task<ActionResult> GetCommentsResult(string id, string searchText)
        {
            LogBookView logViewModel = new LogBookView();
            using (var client = new HttpClient())
            {
                string uri = WebAPI + "LogBookGet?";
                uri += "signalId=" + id;
                uri += "&pageSize=5000";
                uri += "&pageNumber=1";
                uri += "&searchText=" + searchText;
                //HTTP GET
                var responseTask = client.GetAsync(uri);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    string data = await result.Content.ReadAsStringAsync();
                    //use JavaScriptSerializer from System.Web.Script.Serialization
                    JavaScriptSerializer JSserializer = new JavaScriptSerializer();
                    //deserialize to your class
                    logViewModel = JSserializer.Deserialize<LogBookView>(data);
                }
                else //web api sent error response 
                {
                    return Content("Server error. Please contact administrator.");
                }
            }

            return PartialView("_CommentsResult", logViewModel);
        }

        // GET: LogsReport
        public ActionResult Index()
        {
            var vm = new LogsReportViewModel();
            using (var client = new HttpClient())
            {
                string uri = WebAPI + "ReasonsForResponse";
                var response = client.GetAsync(uri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    JavaScriptSerializer JSserializer = new JavaScriptSerializer();
                    //deserialize to your class
                    var reasonsForResponse = JSserializer.Deserialize<List<ReasonForResponseView>>(content);
                    vm.ReasonForResponses = reasonsForResponse;
                }
                else //web api sent error response 
                {
                    return Content("Server error getting the Reasons For Responses filter. Please contact administrator.");
                }
            }
            return View(vm);
        }

        private int SetOnsiteRemote(LogsReportViewModel vm)
        {
            int OnsiteRemote = 0;
            if (vm.OnSite)
                OnsiteRemote += 1; //binary 0001
            if (vm.Remote)
                OnsiteRemote += 2; //binary 0010
            return OnsiteRemote;
        }

        private int SetReasons(LogsReportViewModel vm)
        {
            int Reasons = 0;
            foreach (var reason in vm.ReasonForResponses)
            {
                if (reason.Id == 1)
                {
                    Reasons += reason.Id;
                }
                else
                {
                    Reasons += 2 << (reason.Id - 2); //set bits
                }
            }

            return Reasons;
        }
        public ActionResult RunReport(LogsReportViewModel vm)
        {
            var logBookView = new LogBookView();
            if (vm.LogsView.PageNumber == 0)
            {
                vm.LogsView.PageNumber = 1;
            }
            if (vm.ReportPageNumber == 0)
            {
                vm.ReportPageNumber = 1;
            }

            string uri = BuildUriWithoutPage(vm);
            uri += "&pageRows=" + PageRows;
            uri += "&pageNumber=" + vm.ReportPageNumber;

            //if (ModelState.IsValid)  //don't do  ModelState.IsValid ==> ReasonForResponses will always be invalid
            if ( (vm.EndDate.Ticks - vm.StartDate.Ticks) > 0)  
            {
                logBookView = GetLogBookView(uri);

                vm.LogsView = logBookView; //logViewModel.Logs;
                if (logBookView != null)
                {
                    return PartialView("ReportResult", vm);            
                }
                else
                {
                    return Content("Server error for Reports. Please contact administrator.");

                }
            }
            else
            {
                return Content("Please make sure End Date is before Start Date");
            }
        }

        private LogBookView GetLogBookView(string uri)
        {
            var logBookView = new LogBookView();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add((
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")));

                var response = client.GetAsync(uri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    JavaScriptSerializer JSserializer = new JavaScriptSerializer();
                    logBookView = JSserializer.Deserialize<LogBookView>(content);
                }
                else //web api sent error response 
                {
                    return null; //logBookView;
                }
            }

            logBookView = FindIntersection(logBookView);

            return logBookView;
        }

        private string BuildUriWithoutPage(LogsReportViewModel vm)
        {
            string StartDateString = vm.StartDate.ToShortDateString();
            string EndDateString = vm.EndDate.AddDays(1).ToShortDateString();
            int OnsiteRemote = SetOnsiteRemote(vm);
            int Reasons = SetReasons(vm);

            string uri = WebAPI + "LogBookReport?";
            uri += "locationId=" + vm.LocationId;
            uri += "&commentSearch=" + vm.CommentSearch;
            uri += "&startDate=" + StartDateString;
            uri += "&endDate=" + EndDateString;
            uri += "&userSearch=" + vm.UserSearch;
            uri += "&OnsiteRemote=" + OnsiteRemote;
            uri += "&reasons=" + Reasons;
            return uri;
        }

        private LogBookView FindIntersection(LogBookView logBookView)
        {
            string currentLocation = "";
            string currentIntersection = "";
            foreach (LogView log in logBookView.Logs)
            {
                if (log.Onsite)
                {
                    log.OnsiteOrRemote = "On Site";
                }
                else
                {
                    log.OnsiteOrRemote = "Remote";
                }

                if (!String.Equals(currentLocation, "") && String.Equals(currentLocation, log.LocationId))
                {
                    log.Intersection = currentIntersection;
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add((
                            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")));
                        string uri = WebAPI + "AIMSLocation?";
                        uri += "LocationToSearch=" + log.LocationId;
                        var response = client.GetAsync(uri).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var content = response.Content.ReadAsStringAsync().Result;
                            JavaScriptSerializer JSserializer = new JavaScriptSerializer();
                            var locationView =
                                JSserializer.Deserialize<ViewModels.AIMS.LocationView>(content);
                            currentLocation = locationView.LocationIdentifier;
                            currentIntersection = locationView.Name;
                            log.Intersection = currentIntersection;
                        }
                        else //web api sent error response 
                        {
                            log.Intersection = "Server error, cannot get intersection info";
                        }
                    }
                }
            }

            return logBookView;
        }


        [HttpPost]
        public FileResult FileDownload(LogsReportViewModel vm)
        {
            string startDateFileName
                = vm.StartDate.ToString("yyyyMMdd");
            string endDateFileName = vm.EndDate.ToString("yyyyMMdd");
            string fileZipName = "ELogBook" + vm.LocationId + "_" + startDateFileName + "to" + endDateFileName + ".csv";

            byte[] fileToSave = new byte[]{};

            var logBookView = new LogBookView();
            vm.LogsView.PageNumber = 1;
            string uri = BuildUriWithoutPage(vm);
            uri += "&pageRows=" + FileMaxRows;
            uri += "&pageNumber=" + vm.LogsView.PageNumber;

            if ((vm.EndDate.Ticks - vm.StartDate.Ticks) > 0)
            {
                logBookView = GetLogBookView(uri);
                var records = logBookView.Logs;

                fileToSave = Business.Exporter.GetLogFile(records);
            }

            return File(fileToSave, "csv", fileZipName);

            //// javascript download prefers text; haven't tested binary yet.
            //// works for chrome so far. not IE
            //using (var memStream = new MemoryStream())
            //{
            //    using (var ziparchive = new ZipArchive(memStream, ZipArchiveMode.Create, true))
            //    {
            //        var entry1 = ziparchive.CreateEntry(fileZipName, CompressionLevel.Optimal);
            //        using (var entryStream = entry1.Open())
            //        {
            //            entryStream.Write(fileToSave, 0, fileToSave.Length);
            //        }
            //    }

            //    return File(memStream.ToArray(), "downloaded/csv", fileZipName);
            //}
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
