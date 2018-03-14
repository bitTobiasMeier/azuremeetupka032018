using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace StatefulSessionService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class StatefulSessionService : StatefulService, ISessionService
    {
        public StatefulSessionService(StatefulServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return this.CreateServiceRemotingReplicaListeners();
        }


        public async Task<SessionData> AddAsync(SessionData data)
        {
            var myDictionary =
                await this.StateManager.GetOrAddAsync<IReliableDictionary<string, SessionContract>>("sessions");
            using (var tx = this.StateManager.CreateTransaction())
            {
                await myDictionary.AddAsync(tx, data.Id, new SessionContract()
                {
                    Id = data.Id,
                    Speaker = data.Speaker,
                    Title = data.Title
                });
                await tx.CommitAsync();
            }

            return data;
        }

        public async Task<SessionData[]> GetAllAsync()
        {
            var list = new List<SessionData>();
            var myDictionary =
                await this.StateManager.GetOrAddAsync<IReliableDictionary<string, SessionContract>>("sessions");
            using (var tx = this.StateManager.CreateTransaction())
            {
                var enumerable = await myDictionary.CreateEnumerableAsync(tx);
                using (var enumerator = enumerable.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        var contract = enumerator.Current.Value;
                        list.Add(new SessionData()
                        {
                            Id = contract.Id,
                            Speaker = contract.Speaker,
                            Title = contract.Title
                        });
                    }
                }
            }

            return list.ToArray();
        }

	

    }
}
