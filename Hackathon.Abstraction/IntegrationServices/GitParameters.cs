namespace Hackathon.Abstraction.IntegrationServices;

public sealed class GitParameters
{
    public string Url { get; set; }
    public string UserName { get; set; }
    public string Repository { get; set; }
    public string Branch { get; set; }

    public bool IsFull =>
        !string.IsNullOrEmpty(Url)
        && !string.IsNullOrEmpty(UserName)
        && !string.IsNullOrEmpty(Repository)
        && !string.IsNullOrEmpty(Branch);

    public string ToZipLink()
        => $"{Url}/{UserName}/{Repository}/archive/refs/heads/{Branch}.zip";
}
