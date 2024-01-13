namespace Hackathon.FileStorage.Configuration;

/// <summary>
/// Настройки хранилища S3
/// </summary>
public class S3Options
{
    public string ServiceUrl {get;set;}
    public bool ForcePathStyle {get;set;}
    public bool UseHttp {get;set;}
    public string AccessKey {get;set;}
    public string SecretKey {get;set;}
}
