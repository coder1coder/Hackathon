namespace Hackathon.Common.Models.ApprovalApplications;

/// <summary>
/// Статус согласования
/// </summary>
public enum ApprovalApplicationStatus: byte
{
    /// <summary>
    /// На согласовании
    /// </summary>
    Requested = 0,

    /// <summary>
    /// Согласовано
    /// </summary>
    Approved = 1,

    /// <summary>
    /// Отклонено
    /// </summary>
    Rejected = 2
}
