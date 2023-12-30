using System;
using Hackathon.Common.Models.Tags;
using Mapster;

namespace Hackathon.DAL.Mappings;

public class CommonMappings: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<IHasArrayTags, IHasStringTags>()
            .Map(x => x.Tags, s => 
                string.Join(IHasStringTags.Separator, s.Tags), s => s.Tags != null && s.Tags.Length > 0);

        config.ForType<IHasStringTags, IHasArrayTags>()
            .Map(x => x.Tags, s => string.IsNullOrWhiteSpace(s.Tags)
                ? Array.Empty<string>()
                : s.Tags.Split(IHasStringTags.Separator, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries));
    }
}
