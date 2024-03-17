using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PopularSports.SurveyApp.WebAPI.Models;
using System;

namespace PopularSports.SurveyApp.WebAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> context) : base(context)
        {
        }

        public DbSet<Sport> Sports { get; set; }
        public DbSet<Survey> Survey { get; set; }
        public DbSet<Roles> Roles { get; set; }
    }
}
