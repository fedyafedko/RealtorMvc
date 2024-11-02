﻿using RealtorMVC.FluentEmail.MessageBase;

namespace RealtorMVC.Abstraction.FluentEmail;

public interface IEmailService
{
    Task<bool> SendAsync<T>(T message, string? from = null)
        where T : EmailMessageBase;

    Task<bool> SendManyAsync<T>(List<T> message)
        where T : EmailMessageBase;
}
