﻿namespace Hackathon.API.Contracts.Users;

public class SignUpRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
}