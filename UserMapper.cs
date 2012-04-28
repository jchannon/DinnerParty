using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Authentication.Forms;
using Nancy.Security;
using DinnerParty.Models;

namespace DinnerParty
{
    public class UserMapper : IUserMapper
    {
        private static List<Tuple<string, string, string, Guid>> users = new List<Tuple<string, string, string, Guid>>();

        static UserMapper()
        {
            users.Add(new Tuple<string, string, string, Guid>("admin", "John Hamm", "password", new Guid("55E1E49E-B7E8-4EEA-8459-7A906AC4D4C0")));
            users.Add(new Tuple<string, string, string, Guid>("user", "Jack Black", "password", new Guid("56E1E49E-B7E8-4EEA-8459-7A906AC4D4C0")));
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier)
        {
            var userRecord = users.Where(u => u.Item4 == identifier).FirstOrDefault();

            return userRecord == null
                       ? null
                       : new UserIdentity { UserName = userRecord.Item1, FriendlyName = userRecord.Item2 };
        }

        public static Guid? ValidateUser(string username, string password)
        {
            var userRecord = users.Where(u => u.Item1 == username && u.Item3 == password).FirstOrDefault();

            if (userRecord == null)
            {
                return null;
            }

            return userRecord.Item4;
        }

        public static Guid? ValidateRegisterNewUser(RegisterModel newUser)
        {
            return new Guid("56E1E49E-B7E8-4EEA-8459-7A906AC4D4C0");
        }

    }
}