namespace Hackathon.Common;

public static class RegexPatterns
{
    public const string GithubBranchLink = @"(https://github.com/)([0-9a-zA-Z-_.]+)/([0-9a-zA-Z-_.]+)/tree/([0-9a-zA-Z-_.]+)/?$";
}
