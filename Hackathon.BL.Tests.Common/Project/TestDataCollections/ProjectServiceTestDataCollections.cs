using Hackathon.Common.Models.Project;

namespace Hackathon.BL.Tests.Common.Project.TestDataCollections;

public static class ProjectServiceTestDataCollections
{
    public static IEnumerable<object[]> UpdateSuiteBranchLinks { get; }
        = ProjectServoceTestDataCollectionsInit.UpdateSuiteBranchLinksInit();
}

internal static class ProjectServoceTestDataCollectionsInit
{
    public static IEnumerable<object[]> UpdateSuiteBranchLinksInit()
    {
        string[] branchLinks =
        {
            "https://github.com/as_as-BR.0912/project_01.ASD-A/tree/main",
            "https://github.com/ASDasd0099/ASDasd__0001--2s/tree/main/",
            "https://github.com/Proger0014/url-shortener/tree/main",
            "https://github.com/Asd.a9/Asd.a9_a/tree/main/"
        };
        
        List<object[]> list = new List<object[]>();

        foreach (string branchLink in branchLinks)
        {
            list.Add(new [] { new UpdateProjectFromGitBranchParameters()
            {
                LinkToGitBranch = branchLink,
                EventId = 1,
                TeamId = 1
            } });
        }
        
        return list;
    }
}
