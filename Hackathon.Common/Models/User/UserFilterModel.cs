using Hackathon.Common.Models.Base;

namespace Hackathon.Common.Models.User
{
    public class UserFilterModel: IFilterModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }
}