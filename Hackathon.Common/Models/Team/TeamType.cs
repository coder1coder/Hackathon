using System.Runtime.Serialization;

namespace Hackathon.Common.Models.Team
{
    //Тип команды
    public enum TeamType : byte
    {
        /// <summary>
        /// Закрытый
        /// </summary>
        /// <remarks>
        /// В команду можно попасть только по приглашению
        /// </remarks>
        [EnumMember(Value = "0")]
        Private = 0,

        /// <summary>
        /// Открытый
        /// </summary>
        /// <remarks>
        /// Вступить может любой желающий
        /// </remarks>
        [EnumMember(Value = "1")]
        Public = 1,
    }
}
