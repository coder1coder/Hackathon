using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.ExceptionHandling
{
    public class CustomDetails: ProblemDetails
    {
        public string AdditionalInfo { get; set; }
    }
}