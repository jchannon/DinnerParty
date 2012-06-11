using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Authentication.Forms;
using Nancy.Security;
using DinnerParty.Models;
using Raven.Client;
using DinnerParty.Models.RavenDB;
using System.Security.Cryptography;
using System.Text;
using Raven.Client.Document;

namespace DinnerParty
{
    public class UserMapper : IUserMapper
    {
        private IDocumentSession DocumentSession;

        public UserMapper(IDocumentSession DocumentSession)
        {
            this.DocumentSession = DocumentSession;

        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier)
        {
            var userRecord = DocumentSession.Query<UserModel, IndexUserLogin>().Where(x => x.UserId == identifier).FirstOrDefault();

            return userRecord == null ? null : new UserIdentity() { UserName = userRecord.Username, FriendlyName = userRecord.FriendlyName };
        }

        public Guid? ValidateUser(string username, string password)
        {
            var userRecord = DocumentSession.Query<UserModel, IndexUserLogin>().Where(x => x.Username == username && x.Password == EncodePassword(password)).FirstOrDefault();

            if (userRecord == null)
            {
                return null;
            }

            return userRecord.UserId;
        }

        public Guid? ValidateRegisterNewUser(RegisterModel newUser)
        {
            var userRecord = new UserModel()
            {
                UserId = Guid.NewGuid(),
                LoginType = "DinnerParty",
                EMailAddress = newUser.Email,
                FriendlyName = newUser.Name,
                Username = newUser.UserName,
                Password = EncodePassword(newUser.Password)
            };

            var existingUser = DocumentSession.Query<UserModel, IndexUserLogin>().Where(x => x.EMailAddress == userRecord.EMailAddress && x.LoginType == "DinnerParty").FirstOrDefault();
            if (existingUser != null)
                return null;

            DocumentSession.Store(userRecord);
            DocumentSession.SaveChanges();

            return userRecord.UserId;
        }

        private string EncodePassword(string originalPassword)
        {
            if (originalPassword == null)
                return String.Empty;

            //Declarations
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);

            //Convert encoded bytes back to a 'readable' string
            return BitConverter.ToString(encodedBytes);
        }


    }
}