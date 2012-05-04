using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client.Indexes;

namespace DinnerParty.Models.RavenDB
{
    public class IndexUserLoginType : AbstractIndexCreationTask<UserModel>
    {
        public IndexUserLoginType()
        {
            this.Map = users =>
                                     from user in users
                                     select new
                                     {
                                         LoginType = user.LoginType
                                     };
        }
    }
}