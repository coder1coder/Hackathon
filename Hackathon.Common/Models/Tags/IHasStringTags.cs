namespace Hackathon.Common.Models.Tags;

public interface IHasStringTags
{
    /// <summary>
    /// Теги в строковом представлении разделенные специальным разделителем
    /// </summary>
    string Tags { get; set; }
}
