using System;
using System.Collections.Generic;

namespace LogBook.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsPrimary { get; set; }

        public bool HasBeenPrompted { get; set; }

        public string UdotUsername { get; set; }

        public int IntId { get; set; }

        public int ExtId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateCreated { get; set; }

        public string LastFirstName { get; set; }

        public DateTime SessionExpirationDate { get; set; }

        public string Token { get; set; }

        public string Title { get; set; }

        public List<string> Roles { get; set; }

        public IEnumerable<Role> RoleDetails { get; set; }

        public string WebUserName { get; set; }

        public string UdotUsernameInEmail { get; set; }

        public IEnumerable<int> WorkGroupIds { get; set; }
    }
    public class Role
    {
        public string Name { get; set; }

        public IEnumerable<string> ActionSets { get; set; }

        public IEnumerable<string> ActionTypes { get; set; }
    }
}