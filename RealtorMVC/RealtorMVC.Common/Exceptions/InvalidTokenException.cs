﻿namespace RealtorMVC.Common.Exceptions;

public class InvalidTokenException : Exception
{
    public InvalidTokenException(string? message)
    : base(message) { }
}
