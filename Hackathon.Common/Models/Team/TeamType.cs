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
        Private = 0,

        /// <summary>
        /// Открытый
        /// </summary>
        /// <remarks>
        /// Вступить может любой желающий
        /// </remarks>
        Public = 1,
    }
}
