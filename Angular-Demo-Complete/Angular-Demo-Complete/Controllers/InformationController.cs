using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Angular_Demo_Complete.Controllers
{


    [RoutePrefix("api/Information")]
    public class InformationController : ApiController
    {
        [Route("Time")]
        public object GetTime() {
            return DateTime.Now;
        }


    }
}
