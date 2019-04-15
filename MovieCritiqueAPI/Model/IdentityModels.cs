using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
	public class User : IdentityUser
	{
		public string userName { get; set; }
		public string password { get; set; }
	}

	public class MovieCritiqueEntity : IdentityDbContext<User>
	{

		public MovieCritiqueEntity()
			: base("name=MovieCritiqueEntities")
		{
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			//AspNetUsers -> User
			modelBuilder.Entity<User>()
				.ToTable("User");
			
		}
	}
}