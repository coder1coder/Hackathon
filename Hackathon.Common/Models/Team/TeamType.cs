namespace Hackathon.Common.Models.Team
{
    //Тип команды
    public enum TeamType
    {
        /// <summary>
        /// Закрытый
        /// </summary>
        /// <remarks> 
        /// В команду можно попасть только по приглашению
        /// </remarks>
        Closed = 0,

        /// <summary>
        /// Открытый
        /// </summary>
        /// <remarks>
        /// Вступить может любой желающий
        /// </remarks>
        Open = 1,
    }
}