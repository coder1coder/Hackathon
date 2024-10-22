using System;
using System.Linq;

namespace Hackathon.API;

public static class OriginHelper
{
    private const string WildcardSubdomain = "*.";

    public static bool IsAllowed(string[] allowedOrigins, string currentOrigin)
    {
        if (string.IsNullOrEmpty(currentOrigin)
            || allowedOrigins is not {Length: > 0}
            || !Uri.TryCreate(currentOrigin, UriKind.RelativeOrAbsolute, out var originUri))
        {
            return false;
        }

        return originUri.Host == "localhost" || allowedOrigins.IsOriginAnAllowedSubdomain(originUri.OriginalString);
    }

    private static bool IsOriginAnAllowedSubdomain(this string[] allowedOrigins, string origin)
    {
        if (allowedOrigins.Contains(origin))
        {
            return true;
        }

        if (Uri.TryCreate(origin, UriKind.Absolute, out var originUri))
        {
            return allowedOrigins
                .Where(o => o.Contains($"://{WildcardSubdomain}"))
                .Select(CreateDomainUri)
                .Any(domain => IsSubdomainOf(originUri, domain));
        }

        return false;
    }

    private static Uri CreateDomainUri(string origin) =>
        new(origin.Replace(WildcardSubdomain, string.Empty), UriKind.Absolute);

    private static bool IsSubdomainOf(Uri subdomain, Uri domain)
        => subdomain.IsAbsoluteUri
               && domain.IsAbsoluteUri
               && subdomain.Scheme == domain.Scheme
               && subdomain.Port == domain.Port
               && subdomain.Host.EndsWith($".{domain.Host}", StringComparison.Ordinal);
}
