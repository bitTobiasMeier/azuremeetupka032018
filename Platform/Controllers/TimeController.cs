using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using TimeService;

namespace Platform.Controllers
{
   
        //Achtung: Release-Configuration für x64 vor CheckIn für die Projekte anpassen!

        [Route("api/[controller]")]
        public class TimeController : Controller
        {
            [HttpGet]
            public async Task<string> GetAsync()
            {

                try
                {
                    var proxy = GetProxy();
                    return await proxy.GetTimeAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return ex.Message;
                }
            }

            private static ITimeService GetProxy()
            {
                var proxy = ServiceProxy.Create<ITimeService>(new Uri("fabric:/AzureMeetupKA/TimeService"));
                return proxy;
            }
        }
    
}
