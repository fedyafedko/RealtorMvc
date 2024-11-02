using System.ComponentModel.DataAnnotations.Schema;

namespace RealtorMVC.DAL.Entities;

public class UserRegister : BaseEntity
{
    public int Code { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public bool IsCodeRegenerated { get; set; }

    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}