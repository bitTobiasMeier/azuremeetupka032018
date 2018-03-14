using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace StatefulSessionService
{
    public interface ISessionService : IService
    {
        Task<SessionData> AddAsync(SessionData data);
        Task<SessionData[]> GetAllAsync();
    }

    public class SessionData
    {
        public string Title { get; set; }
        public string Speaker { get; set; }
        public string Id { get; set; }
    }

    [DataContract]
    public class SessionContract
    {
        [DataMember] public string Title { get; set; }
        [DataMember] public string Speaker { get; set; }
        [DataMember] public string Id { get; set; }
    }
}
