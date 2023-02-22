

namespace MF.Users.Database.Contexts;

public class MFUserContext : IdentityDbContext<MFUser>
{
	public MFUserContext(DbContextOptions<MFUserContext> options) : base(options)
	{

	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
	}
}
