using RealtorMVC.Models.Auth;

namespace RealtorMVC.BLL.Interfaces;

public interface IEmailConfirmationService
{
    Task<AuthSuccessModel> ConfirmEmailAsync(ConfirmEmailModel model);
    Task<int> GenerateEmailCodeAsync(Guid userId);
    Task ResendConfirmationCodeAsync(Guid userId);
}
