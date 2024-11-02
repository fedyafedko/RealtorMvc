using System.ComponentModel.DataAnnotations;

namespace RealtorMVC.DAL.Entities;

public class BaseEntity
{
    [Key]
    public Guid Id { get; set; }
}