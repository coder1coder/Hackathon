using System.Runtime.Serialization;

namespace Hackathon.Common.Models
{
    public enum SortOrder
    {
        [EnumMember(Value = "0")]
        Asc = 0, 
        [EnumMember(Value = "1")]
        Desc = 1
    }
}