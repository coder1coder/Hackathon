namespace Hackathon.IntegrationServices.Github.Abstraction;

public sealed class GitParameters
{
    public string Url { get; set; }
    public string UserName { get; set; }
    public string Repository { get; set; }
    public string Branch { get; set; }

    public bool IsFull =>
        !string.IsNullOrWhiteSpace(Url)
        && !string.IsNullOrWhiteSpace(UserName)
        && !string.IsNullOrWhiteSpace(Repository)
        && !string.IsNullOrWhiteSpace(Branch);

    public string ToZipLink()
        => $"{Url}/{UserName}/{Repository}/archive/refs/heads/{Branch}.zip";
}
