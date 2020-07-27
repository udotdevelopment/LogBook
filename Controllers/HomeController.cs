using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using LogBook.Business;
using LogBook.Models.MaxView;
using LogBook.ViewModels.CyberLock;
using LogBook.ViewModels.AIMS;
using LogBook.ViewModels.LogBook;
using LogBook.ViewModels.MaxView;
using Microsoft.AspNet.Identity;

namespace LogBook.Controllers
{

    [OpenIdAuthorize(Roles = "User")]
    public class HomeController : Controller
    {
        public string WebAPI {
            get
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings["WebAPIURL"];
            }
        }
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult SaveLogBookEntry(LogView log)
        {
           
            List<string> errors = new List<string>();
            if (log.Timestamp == null || log.Timestamp== DateTime.MinValue)
            {
                errors.Add("Timestamp Required");
            }
            if (log.ReasonForResponses==null || !log.ReasonForResponses.Any())
            {
                errors.Add("Reason For Response Required");
            }
            if (String.IsNullOrEmpty(log.Comment))
            {
                errors.Add("Comment Required");
            }
            if (errors.Any())
            {
                return PartialView("_Errors", errors);
            }

            if (User.Identity != null)
            {
                var claim = ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "name");
                log.User = claim.Value;
            }
            else
            {
                log.User = "NA";
            }

            using (var client = new HttpClient())
            {
                string uri = WebAPI+"LogBook";
                //HTTP GET
                JavaScriptSerializer JSserializer = new JavaScriptSerializer();
                var jsonString = JSserializer.Serialize(log);
                var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync(uri, stringContent);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return Content("Log Saved " + DateTime.Now);
                }
                else //web api sent error response 
                {
                    return Content("Server error. Please contact administrator.");
                }
            }
        }

        public ActionResult LogBook(string id)
        {
            string locationId = Request.QueryString["LocationId"];
            string signalId = Request.QueryString["signalId"];
            LogView logbookViewModel = new LogView()
            {
                Id = 0,
                Timestamp = DateTime.Now
            };
            if (!String.IsNullOrEmpty(signalId))
            {
                logbookViewModel.SearchId = signalId;
            }
            if (!String.IsNullOrEmpty(locationId))
            {
                logbookViewModel.SearchId = locationId;
            }

            //var myObject = System.Security.Claims.ClaimsPrincipal.Current;
            //string logOnName = Request.LogonUserIdentity.Name;
            //ViewData["Name"] = logOnName;

            //var name = System.Security.Claims.ClaimsPrincipal.Current.FindFirst("name").Value;
            //var lastName = System.Security.Claims.ClaimsPrincipal.Current.FindFirst("lastname").Value;
            //var firstName = System.Security.Claims.ClaimsPrincipal.Current.FindFirst("firstname").Value;
            //var userEmail = System.Security.Claims.ClaimsPrincipal.Current.FindFirst("phone").Value;
            //var phone = System.Security.Claims.ClaimsPrincipal.Current.FindFirst("email").Value;
            var test = User.Identity.Name;
            var role = User.IsInRole("User");
            //var myName = "Hello Andre";

            //var LastName = Request.Headers["lastname"];
            //var FirstName = Request.Headers["firstname"];
            //var UserEmail = Request.Headers["email"];
            //var PhoneNumber = Request.Headers["phone"];
            //var name = "Hello from HomeController:Logbook";
            var path = @"C:\Temp\ResponseHeaders.txt";
            using (var RH = new StreamWriter(path, false))
            {
                RH.WriteLine("Start of reading the responses");
                RH.WriteLine();
                RH.WriteLine("Last Name is: >" + test + "<");
                RH.Close();
            }

            //IdentityUserRole IdentityUserRole role = new IdentityUserRole(FirstName);
            return View(logbookViewModel);
        }

        public async Task<ActionResult> GetAIMSWorkOrders(string id)
        {
            int aIMSCount = 0;
            try
            {
                using (var client = new HttpClient())
                {
                    string uri = WebAPI + "AIMSTotal?";
                    uri += "location=" + id;
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
                        aIMSCount = JSserializer.Deserialize<int>(data);
                    }
                    else //web api sent error response 
                    {
                        return Content("Server error. Please contact administrator.");
                    }
                }
               
            }
            catch (Exception ex)
            { }
            return PartialView("_AimsWorkOrders", aIMSCount);
        }

        public async Task<ActionResult> GetAIMSResult(string id, int pageNumber)
        {
                WorkorderLogViewModel workorderLogViewModel = new WorkorderLogViewModel();
            try
            {
                
                    using (var client = new HttpClient())
                    {
                        string uri = WebAPI + "WorkorderViewEntries?";
                        uri += "location=" + id;
                        uri += "&pageSize=5";
                        uri += "&pageNumber=" +pageNumber;
                        var responseTask = client.GetAsync(uri);
                        responseTask.Wait();
                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            string data = await result.Content.ReadAsStringAsync();
                            //use JavaScriptSerializer from System.Web.Script.Serialization
                            JavaScriptSerializer JSserializer = new JavaScriptSerializer();
                            //deserialize to your class
                            workorderLogViewModel = JSserializer.Deserialize<WorkorderLogViewModel>(data);
                        }
                        else //web api sent error response 
                        {
                            return Content("Server error. Please contact administrator.");
                        }
                    }
                
            }
            catch (Exception ex)
            {
            }
            if(workorderLogViewModel.WorkorderLogs ==null)
                workorderLogViewModel.WorkorderLogs = new List<WorkorderView>();

            return PartialView("_AimsResult", workorderLogViewModel);
        }

        public async Task<ActionResult> GetCyberLockLogs(string id)
        {
            int cyberLockLogTotal = 0;
            try
            {
                using (var client = new HttpClient())
                {
                    string uri = WebAPI + "CyberlockTotalFromSignal?";
                    uri += "signalId=" + id;
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
                        cyberLockLogTotal = JSserializer.Deserialize<int>(data);
                    }
                    else //web api sent error response 
                    {
                        return Content("Server error. Please contact administrator.");
                    }
                }

            }
            catch (Exception ex)
            { }
            CyberLockLogViewModel cyberLockLogViewModel = new CyberLockLogViewModel();
            cyberLockLogViewModel.TotalNumber = cyberLockLogTotal;
            cyberLockLogViewModel.CyberlockLogs = new List<CyberLockLog>();
            return PartialView("_CyberLockLogs", cyberLockLogViewModel);
        }

        public async Task<ActionResult> GetCyberLockLogsResult(string id, int? pageNumber)
        {
            if (pageNumber == null)
                pageNumber = 1;
            CyberLockLogViewModel cyberLockLogViewModel = new CyberLockLogViewModel();
            using (var client = new HttpClient())
            {
                string uri = WebAPI + "CyberlockEntries?";
                uri += "signalId=" + id;
                uri += "&pageSize=5";
                uri += "&pageNumber=" + pageNumber;
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
                    cyberLockLogViewModel = JSserializer.Deserialize<CyberLockLogViewModel>(data);
                }
                else //web api sent error response 
                {
                    return Content("Server error. Please contact administrator.");
                }
            }

            return PartialView("_CyberLockLogsResult", cyberLockLogViewModel);
        }


        public async Task<ActionResult> GetMaxViewLogs(string id)
        {
            int number;
            if (!int.TryParse(id, out number))
            {
                return Content("");
            }
            MaxViewLogViewModel maxViewLogViewModel = new MaxViewLogViewModel();
            using (var client = new HttpClient())
            {
                string uri = WebAPI + "MaxviewTotal?";
                uri += "signalId=" + id;
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
                    maxViewLogViewModel.TotalNumber = JSserializer.Deserialize<int>(data);
                }
                else //web api sent error response 
                {
                    return Content("Server error. Please contact administrator.");
                }
            }
           return PartialView("_MaxViewLog", maxViewLogViewModel);
        }

        public async Task<ActionResult> GetMaxViewLogsResult(string id, int? pageNumber)
        {
            MaxViewLogViewModel maxViewLogViewModel = new MaxViewLogViewModel();
            using (var client = new HttpClient())
            {
                if (pageNumber == null)
                    pageNumber = 1;
                string uri = WebAPI + "MaxviewEntries?";
                uri += "signalId=" + id;
                uri += "&pageSize=5";
                uri += "&pageNumber="+pageNumber;
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
                    maxViewLogViewModel = JSserializer.Deserialize<MaxViewLogViewModel>(data);
                }
                else //web api sent error response 
                {
                    return Content("Server error. Please contact administrator.");
                }
            }
            return PartialView("_MaxViewLogResult", maxViewLogViewModel);
        }

        public async Task<ActionResult> GetMaxViewLogsInitialLoad(string id)
        {
            MaxViewLogViewModel maxViewLogViewModel = new MaxViewLogViewModel();
            maxViewLogViewModel.MaxViewLogs = new List<MaxViewLog>();
            using (var client = new HttpClient())
            {
                string uri = WebAPI + "MaxviewTotal?";
                uri += "signalId=" + id;
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
                    maxViewLogViewModel.TotalNumber = Convert.ToInt32(data);
                }
                else //web api sent error response 
                {
                    return Content("Server error. Please contact administrator.");
                }
            }
            return PartialView("_MaxViewLog", maxViewLogViewModel);
        }

        public async Task<ActionResult> GetComments(string id, string searchText)
        {
            LogBookView logViewModel = new LogBookView();

            var apiController = new LogApiController();
            logViewModel.TotalNumber = apiController.LogBookCount(id, searchText);
            //using (var client = new HttpClient())
            //{
            //    string uri = WebAPI + "LogBookCount?";
            //    uri += "signalId=" + id;
            //    uri += "&searchText=" + searchText;
            //    //HTTP GET
            //    var responseTask = client.GetAsync(uri);
            //    responseTask.Wait();

            //    var result = responseTask.Result;
            //    if (result.IsSuccessStatusCode)
            //    {
            //        string data = await result.Content.ReadAsStringAsync();
            //        //use JavaScriptSerializer from System.Web.Script.Serialization
            //        JavaScriptSerializer JSserializer = new JavaScriptSerializer();
            //        //deserialize to your class
            //        logViewModel.TotalNumber = JSserializer.Deserialize<int>(data);
            //    }
            //    else //web api sent error response 
            //    {
            //        return Content("Server error. Please contact administrator.");
            //    }
            //}

            return PartialView("_Comments", logViewModel);
        }

        public async Task<ActionResult> GetCommentsResult(string id, int? pageNumber, 
            string searchText)
        {
            if (pageNumber == null)
                pageNumber = 1;
            LogBookView logViewModel = new LogBookView();
            using (var client = new HttpClient())
            {
                string uri = WebAPI + "LogBookGet?";
                uri += "locationId=" + id;  
                uri += "&pageSize=5";
                uri += "&pageNumber=" + pageNumber;
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

        public async Task<ActionResult> GetLogEntry(string id)
        {
            if (id == null)
            {
                return Content("<h1>Location Not Found</h1>");
            }
            ViewModels.LogBook.LogView logbookViewModel = new LogView()
            {
                Id = 0,
                Timestamp = DateTime.Now

            };
            using (var client = new HttpClient())
            {
                string uri = WebAPI + "AIMSLocation?";
                uri += "LocationToSearch=" + id;
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
                    var locationView = JSserializer.Deserialize<LocationView>(data);
                    logbookViewModel.LocationId = locationView.LocationIdentifier;
                    logbookViewModel.LocationDescription = locationView.Name;
                    logbookViewModel.LocationTypeDescription = locationView.LocationType;
                    if (String.IsNullOrEmpty(logbookViewModel.LocationId))
                    {
                        return Content("Location not found.");
                    }
                }
                else //web api sent error response 
                {
                    return Content("Server error. Please contact administrator.");
                }
            }
            using (var client = new HttpClient())
            {
                string uri = WebAPI + "ReasonsForResponse";
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
                    var reasonsForResponse = JSserializer.Deserialize<List<ReasonForResponseView>>(data);
                    logbookViewModel.ReasonForResponses = reasonsForResponse;
                }
                else //web api sent error response 
                {
                    return Content("Server error. Please contact administrator.");
                }
            }

            return PartialView("_LogEntry",logbookViewModel);
        }
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

       

    }

}