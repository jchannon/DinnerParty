using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client.Indexes;

namespace DinnerParty.Models.RavenDB
{
    public class IndexUserLogin : AbstractIndexCreationTask<UserModel>
    {
        public IndexUserLogin()
        {
            this.Map = users =>
                                     from user in users
                                     select new
                                     {
                                         LoginType = user.LoginType,
                                         UserId = user.UserId,
                                         Username = user.Username,
                                         Password = user.Password,
                                         EMailAddress = user.EMailAddress
                                     };
        }
    }
}