using Microsoft.EntityFrameworkCore;
using PROG_POE2.Models;

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*START*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//

namespace PROG_POE2.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<ClaimModel> Claims { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ClaimModel>()
				.HasKey(c => c.ClaimID);

			modelBuilder.Entity<ClaimModel>()
			   .Property(c => c.SupportingDocument)
			   .HasColumnType("varbinary(max)")
			   .IsRequired(false);

			modelBuilder.Entity<ClaimModel>()
				.Property(c => c.HourlyRate)
				.HasColumnType("decimal(18,2)");

			modelBuilder.Entity<ClaimModel>()
			.Property(c => c.TotalAmount)
			.HasColumnType("decimal(18,2)")
			.HasComputedColumnSql("[HoursWorked] * [HourlyRate]", stored: true);

            modelBuilder.Entity<ClaimModel>()
                .Property(c => c.AdditionalNotes)
				.HasColumnType("nvarchar(max)")
				.IsRequired(false);
        }
	}
}

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*END*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//
