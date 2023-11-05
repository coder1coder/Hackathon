using System;
using Hackathon.FileStorage.Abstraction.Models;

namespace Hackathon.FileStorage.BL.Extensions;

public static class BucketExtensions 
{
    public static string ToBucketName(this Bucket bucket)
    {
        return bucket switch
        {
            Bucket.Avatars => nameof(Bucket.Avatars).ToLower(),
            Bucket.Events => nameof(Bucket.Events).ToLower(),
            Bucket.Projects => nameof(Bucket.Projects).ToLower(),
            _ => throw new ArgumentOutOfRangeException(nameof(bucket), "Бакет не определен")
        };
    }
}
