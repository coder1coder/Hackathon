using System;
using System.Text.Json.Serialization;

namespace Hackathon.Contracts.Requests.Event
{
    public class SetStatusRequest<T>
    {
        public long Id { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public T Status { get; set; }
    }
}