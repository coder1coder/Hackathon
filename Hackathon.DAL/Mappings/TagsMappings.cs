using System;
using Hackathon.Common.Models.Tags;
using Mapster;

namespace Hackathon.DAL.Mappings;

public class TagsMappings: IRegister
{
    public const string TagsSeparator = " ";
    
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<TagCollection, string>()
            .MapWith(x=> string.Join(TagsSeparator, x));

        config.ForType<string, TagCollection>()
            .MapWith(x => 
                new TagCollection(x.Split(TagsSeparator, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)));
    }
}
