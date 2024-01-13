using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hackathon.Common.Models.Tags;

public class TagCollection: Collection<string>
{
    public TagCollection()
    {
    }
    
    public TagCollection(IList<string> list):base(list)
    {
    }
}
