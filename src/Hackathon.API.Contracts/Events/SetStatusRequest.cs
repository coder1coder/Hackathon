using System.Text.Json.Serialization;

namespace Hackathon.API.Contracts.Events;

public class SetStatusRequest<T>
{
    public long Id { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public T Status { get; set; }
}
