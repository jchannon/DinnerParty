using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using System.IO;

namespace DinnerParty.Helpers
{
    public class FileNotFoundResult : Response
    {
        public string Message
        {
            get;
            set;
        }

        public FileNotFoundResult()
        {

            StatusCode = HttpStatusCode.NotFound;
          
            //Contents = stream =>
            //    {
            //        var writer = new StreamWriter(stream) { AutoFlush = true };
            //        writer.Write(Message);
            //    };
        }
    }
}
