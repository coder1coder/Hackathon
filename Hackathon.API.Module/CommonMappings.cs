using System;
using Hackathon.Common.Models.Tags;
using Mapster;

namespace Hackathon.API.Module;

public class CommonMappings: IRegister
{
    private const char TagsSeparator = ' ';
    
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<IHasArrayTags, IHasStringTags>()
            .Map(x => x.Tags, s => string.Join(TagsSeparator, s.Tags ?? Array.Empty<string>()));

        config.ForType<IHasStringTags, IHasArrayTags>()
            .Map(x => x.Tags, s => string.IsNullOrWhiteSpace(s.Tags)
                ? Array.Empty<string>()
                : s.Tags.Split(TagsSeparator, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries));
    }
}
