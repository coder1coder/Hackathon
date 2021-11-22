using System;
namespace Hackathon.API.ExceptionHandling
{
    public class CustomException : Exception
    {
        public string AdditionalInfo { get; set; }
        public string Type { get; set; }
        public string Detail { get; set; }
        public string Title { get; set; }
        public string Instance { get; set; }
        
        public CustomException(string instance)
        {
            Type = "hackathon-exception";
            Detail = "There was an unexpected error.";
            Title = "Hackathon Exception";
            AdditionalInfo = "Maybe you can try again in a bit?";
            Instance = instance;
        }
    }
}