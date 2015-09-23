using System.Runtime.Serialization;

namespace ActivityStreams.Persistence.Cassandra.Tests.Models
{
    [DataContract(Name = "92e17681-1468-4297-9818-6e70cd97d20a")]
    public class ActivityBody
    {
        [DataMember(Order = 1)]
        public string Content { get; set; }
    }
}
