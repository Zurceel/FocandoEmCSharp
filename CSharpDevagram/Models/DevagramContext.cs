using Microsoft.EntityFrameworkCore;

namespace CSharpDevagram.Models
{
	public class DevagramContext : DbContext
	{
		public DevagramContext(DbContextOptions<DevagramContext> option) : base(option)
		{
		}

		public DbSet<Usuario> Usuarios { get; set; }
	}
}
