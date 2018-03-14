using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using StatefulSessionService;

namespace Platform.Controllers
{
    [Route("api/[controller]")]
    public class SessionController : Controller
    {

        [HttpGet]
        public async Task<IEnumerable<SessionData>> Get()
        {
            try
            {
                var proxy = GetProxy();
                return await proxy.GetAllAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }

        }




        [HttpPost]
        public async Task<string> PostAsync([FromBody] SessionData value)
        {
            try
            {
                var proxy = GetProxy();
                var result = await proxy.AddAsync(value);
                ServiceEventSource.Current.Message("Session \"" + result.Speaker + ": " + result.Title + "\" added");
                return result.Speaker + ": " + result.Title;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return ex.Message;
            }

        }

        private static ISessionService GetProxy()
        {
            var proxy = ServiceProxy.Create<ISessionService>(new Uri("fabric:/AzureMeetupKA/StatefulSessionService"),
                new ServicePartitionKey(0));
            return proxy;
        }

	


    }
}
