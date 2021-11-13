using System;

namespace Hackathon.Contracts.Requests
{
    public class SetStatusRequest<T> where T: Enum
    {
        public long Id { get; set; }
        public T Status { get; set; }
    }
}