using System;

namespace LogBook.Repositories
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
