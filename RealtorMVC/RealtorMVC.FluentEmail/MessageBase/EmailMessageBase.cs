﻿namespace RealtorMVC.FluentEmail.MessageBase;

public abstract class EmailMessageBase
{
    public abstract string Subject { get; }
    public abstract string TemplateName { get; }
    public string Recipient { get; set; } = null!;
}
