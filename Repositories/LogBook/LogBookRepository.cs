using System;
using System.Collections.Generic;
using System.Linq;
using LogBook.Models.LogBook;
using LogBook.ViewModels.LogBook;

namespace LogBook.Repositories.LogBook
{
    public class LogBookRepository : ILogBookRepository
    {
        private readonly Models.LogBook.LogBook db = new Models.LogBook.LogBook();

        public List<LogView> GetLogs(string locationId,string searchText)
        {
            IQueryable<LogView> result = db.Logs.Select(l => new LogView{Id = l.Id, LocationId = l.LocationId, Timestamp = l.Timestamp, DateCreated = l.DateCreated, Onsite = l.Onsite, Comment = l.Comment, User = l.User,
                ReasonForResponses = (l.ReasonForResponses
                    .Select(r => new ReasonForResponseView{Id = r.Id, Abbreviation = r.Abbreviation, Description = r.Description, DisplayOrder = r.DisplayOrder})).ToList()})
                .Where(l => l.LocationId.Trim() == locationId.Trim())
                .OrderByDescending(l => l.Timestamp);
            if (!String.IsNullOrEmpty(searchText))
            {
                result = result.Where(r => r.Comment.Contains(searchText));
            }
            return result.ToList();
        }

        public List<LogView> SearchLogs(string locationId, string commentSearch, DateTime startDate, DateTime endDate,
            string userSearch, int onsiteRemote, int reasons)

        {
            IQueryable<LogView> result = db.Logs.Select(l => new LogView
                {
                    Id = l.Id,
                    LocationId = l.LocationId,
                    Timestamp = l.Timestamp,
                    DateCreated = l.DateCreated,
                    Onsite = l.Onsite,
                    Comment = l.Comment,
                    User = l.User,
                    ReasonForResponses = (l.ReasonForResponses
                        .Select(r => new ReasonForResponseView { Id = r.Id, Abbreviation = r.Abbreviation, Description = r.Description, DisplayOrder = r.DisplayOrder })).ToList()
                })
                .Where(l => (l.Timestamp >= startDate && l.Timestamp < endDate))
                .OrderByDescending(l => l.Timestamp);
            if (!String.IsNullOrEmpty(locationId) && !String.Equals(locationId, "0"))
            {
                result = result.Where(l => l.LocationId.Trim() == locationId.Trim());
            }
            if (!String.IsNullOrEmpty(commentSearch))
            {
                result = result.Where(r => r.Comment.Contains(commentSearch));
            }
            if (!String.IsNullOrEmpty(userSearch))
            {
                result = result.Where(u => u.User.ToUpper().Contains(userSearch.ToUpper()));
            }
            switch (onsiteRemote)
            {
                case 0:
                    return new List<LogView>();  //did not choose any result set, since Onsite & Remote had to be checked one
                case 1:
                    result = result.Where(l => l.Onsite == true);
                    break;
                case 2:
                    result = result.Where(l => l.Onsite == false);
                    break;

                case 3:  //all result sets are in
                    break;
            }

            if (reasons == 0)
            {
                return null;
            }
            else if (reasons < 127) //127: all options are checked, 2^0+2^1+2^2+...+2^6=127.
            {
                List<int> ChoosenReasons = new List<int>();
                ChoosenReasons = ShiftInput(reasons);
                result = result.Where(l => l.ReasonForResponses.Any(x => ChoosenReasons.Contains(x.Id)));
            }

            return result.ToList();
        }
        private static List<int> ShiftInput(int numberIn)
        {
            List<int> ChoosenReasons = new List<int>();
            int remain;
            for (int i = 1; i < 8 && i >= 1; i++)
            {
                numberIn = Math.DivRem(numberIn, 2, out remain);
                if (remain > 0)
                {
                    ChoosenReasons.Add(i);
                }
            }

            return ChoosenReasons;
        }

        public void Update(Log log)
        {
            DateTime startDate = log.Timestamp;
            DateTime endDate = log.Timestamp.AddSeconds(5);
            if (!db.Logs.Any(l => l.Timestamp >= startDate && log.Timestamp <= endDate && l.Comment == log.Comment && l.LocationId == log.LocationId))
            {
                List<ReasonForResponse> reasonsForResponse = new List<ReasonForResponse>();
                foreach (var logReasonForResponse in log.ReasonForResponses)
                {
                    reasonsForResponse.Add(
                        db.ReasonForResponses.FirstOrDefault(r => r.Id == logReasonForResponse.Id));
                }
                log.ReasonForResponses = reasonsForResponse;
                log.LocationType = db.LocationTypes.FirstOrDefault(l => l.Description == log.LocationTypeDescription);
                log.DateCreated = DateTime.Now;
                db.Logs.Add(log);
                db.SaveChanges();
            }
        }

        public int GetLogCount(string signalId, string searchText)
        {
            IQueryable<Log> result = db.Logs//.Include(l => l.ReasonForResponses)
                .OrderByDescending(l => l.Timestamp)
                .Where(l => l.LocationId.Trim() == signalId.Trim());
            if (!String.IsNullOrEmpty(searchText))
            {
                result = result.Where(r => r.Comment.Contains(searchText));
            }
            return result.Count();
        }
    }
}
