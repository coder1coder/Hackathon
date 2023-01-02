namespace Hackathon.Abstraction.FileStorage;

public enum Bucket
{
    Avatars = 0,
    Events = 1
}

public static class BucketExtensions {
    public static string ToBucketName(this Bucket bucket)
    {
        return bucket switch
        {
            Bucket.Avatars => nameof(Bucket.Avatars).ToLower(),
            Bucket.Events => nameof(Bucket.Events).ToLower(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
