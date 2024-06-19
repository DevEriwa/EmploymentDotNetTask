using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using EmploymentDotNetTask.Models;

namespace EmploymentDotNetTask.Data
{
	public class EmploymentDbContext : DbContext
	{
		public EmploymentDbContext(DbContextOptions<EmploymentDbContext> options)
		  : base(options)
		{
		}
		public DbSet<Application>? Applicants { get; set; }
		public DbSet<QuestionType>? QuestionTypes { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Application>().ToContainer("Applicants").HasPartitionKey(partitionkey => partitionkey.Id);
			modelBuilder.Entity<QuestionType>().ToContainer("QuestionTypes").HasPartitionKey(partitionkey => partitionkey.Id);

			modelBuilder.Entity<Application>().OwnsMany(x => x.DateAnswers);
			modelBuilder.Entity<Application>().OwnsMany(x => x.DropdownAnswers);
			modelBuilder.Entity<Application>().OwnsMany(x => x.MultipleChoiceAnswers);
			modelBuilder.Entity<Application>().OwnsMany(x => x.NumericAnswers);
			modelBuilder.Entity<Application>().OwnsMany(x => x.ParagraphAnswers);
			modelBuilder.Entity<Application>().OwnsMany(x => x.YesOrNoAnswers);
			modelBuilder.Entity<Application>().OwnsOne(x => x.Gender);

		}
	}


}
