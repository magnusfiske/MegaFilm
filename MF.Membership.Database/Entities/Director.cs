
namespace MF.Membership.Database.Entities;

public class Director : IEntity
{
    public int Id { get; set; }
    [MaxLength(50)]
    public string? Name { get; set; }

    public virtual ICollection<Film>? Films { get; set; }
}
