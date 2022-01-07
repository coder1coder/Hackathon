using System;

namespace Hackathon.Contracts.Requests.Event
{
    public class SetStatusRequest<T>
    {
        public long Id { get; set; }

        public T Status { get; set; }
    }
}