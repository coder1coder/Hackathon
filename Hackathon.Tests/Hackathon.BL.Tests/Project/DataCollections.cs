using System.Collections.Generic;
using Hackathon.Common.Models.Project;

namespace Hackathon.BL.Tests.Project;

public static class DataCollections
{
    public static IEnumerable<object[]> ValidateSuiteValidBranchLinks()
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

    public static IEnumerable<object[]> ValidateSuiteUnvalidBranchLinks()
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
