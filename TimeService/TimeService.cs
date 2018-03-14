using System;
using System.Collections.Generic;
using System.Fabric;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

//Vor Namespace Stellen
[assembly:
    FabricTransportServiceRemotingProvider(RemotingListener = RemotingListener.V2Listener,
        RemotingClient = RemotingClient.V2Client)]
namespace TimeService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class TimeService : StatelessService, ITimeService
    {
        public TimeService(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return this.CreateServiceRemotingInstanceListeners();
        }

        public Task<string> GetTimeAsync()
        {
            return Task.FromResult(DateTime.Now.ToString(CultureInfo.InvariantCulture));
        }


    }

    public interface ITimeService : IService
    {
        Task<string> GetTimeAsync();
    }
}
