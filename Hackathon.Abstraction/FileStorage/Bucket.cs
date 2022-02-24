namespace Hackathon.Abstraction.FileStorage;

public enum Bucket
{
    Avatars = 0
}

public static class BucketExtensions {
    public static string ToBucketName(this Bucket bucket)
    {
        return bucket switch
        {
            Bucket.Avatars => nameof(Bucket.Avatars).ToLower(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}