using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using System.IO;
using DinnerParty.Properties;

namespace DinnerParty
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        byte[] favicon;

        protected override void ApplicationStartup(TinyIoC.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
        }

        protected override byte[] DefaultFavIcon
        {
            get
            {
                //if (favicon == null)
                //{
                //    //TODO: remember to replace 'AssemblyName' with the prefix of the resource
                //    using (var resourceStream = GetType().Assembly.GetManifestResourceStream("DinnerParty.favicon.ico"))
                //    {
                //        var tempFavicon = new byte[resourceStream.Length];
                //        resourceStream.Read(tempFavicon, 0, (int)resourceStream.Length);
                //        favicon = tempFavicon;
                //    }
                //}
                //return favicon;


                if (favicon == null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Resources.favicon.Save(ms);
                        favicon = ms.ToArray();
                    }
                }
                return favicon;


            }
        }
    }
}