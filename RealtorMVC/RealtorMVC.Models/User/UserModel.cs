﻿namespace RealtorMVC.Models.User;

public class UserModel
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string AvatarUrl { get; set; }
    public string PhoneNumber { get; set; }
    public string Role { get; set; }
}