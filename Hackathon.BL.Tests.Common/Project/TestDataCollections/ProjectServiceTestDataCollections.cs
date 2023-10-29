using Hackathon.Common.Models.Project;

namespace Hackathon.BL.Tests.Common.Project.TestDataCollections;

public static class ProjectServiceTestDataCollections
{
    public static IEnumerable<object[]> ValidateSuiteValidBranchLinks { get; }
        = ProjectServoceTestDataCollectionsInit.ValidateSuiteValidBranchLinksInit();

    public static IEnumerable<object[]> ValidateSuiteUnvalidBranchLinks { get; }
        = ProjectServoceTestDataCollectionsInit.ValidateSuiteUnvalidBranchLinksInit();
}

internal static class ProjectServoceTestDataCollectionsInit
{
    public static IEnumerable<object[]> ValidateSuiteValidBranchLinksInit()
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

    public static IEnumerable<object[]> ValidateSuiteUnvalidBranchLinksInit()
    {
        string[] branchLinks =
        {
            "https://github.com/as/_as-BR.0912/project_01.ASD-A/tree/main",
            "https://github.com/ASDasd0099/ASDasd__0001--2s/tree/main/directory",
            "https://github.com/Proger0014/url-shortenertree/main"
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
