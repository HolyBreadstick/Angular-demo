using Angular_Demo_Complete.Entities;
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

        [Route("Demo")]
        public object GetDemo() {

            var temp = new Artist() {
                firstName = "Bailey",
                lastName = "Miller",
                birthdate= new DateTime(1996,11,06)
            };

            var tempAl = new Album() {
                title = "Hot Rap Shit",
                views = 0
            };

            var tempS = new Song() {
                title="Burnout",
                onSale=true,
                discount=.25,
                price = 1.99
            };

            tempAl.Songs.Add(tempS);

            temp.Albums.Add(tempAl);


            return temp;

        }

    }
}
