using System;
using System.Linq;
using LogBook.Models.MaxView;

namespace LogBook.Repositories.MaxView
{
    public class MaxviewUserRepository : IMaxviewUserRepository
    {
        private readonly Maxview db = new Maxview();

        public string GetUserName(int UserId)
        {
            var result = (from c in db.MaxviewUsers
                where c.Id == UserId
                select c).FirstOrDefault();
            if (result != null)
            {
                return result.UserName;
            }
            else
            {
                return String.Empty;
            }
        }

    }
}
