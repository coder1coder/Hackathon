using Microsoft.AspNetCore.Routing;

namespace Hackathon.API;

public class LowerCaseRouteTransformer: IOutboundParameterTransformer
{
    public string TransformOutbound(object value)
        => value?.ToString()?.ToLower();
}
