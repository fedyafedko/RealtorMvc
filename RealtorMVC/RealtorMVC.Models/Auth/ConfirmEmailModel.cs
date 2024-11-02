namespace RealtorMVC.Models.Auth;

public class ConfirmEmailModel
{
    public Guid UserId { get; set; }
    public int Code { get; set; }
}