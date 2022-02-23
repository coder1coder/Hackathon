namespace Hackathon.Common.Configuration;

public class S3Options
{
    public string ServiceUrl {get;set;}
    public bool ForcePathStyle {get;set;}
    public bool UseHttp {get;set;}
    public string AccessKey {get;set;}
    public string SecretKey {get;set;}
}