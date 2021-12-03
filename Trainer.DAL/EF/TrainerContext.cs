using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Trainer.DAL.Entities;

namespace Trainer.DAL.EF
{
    public class TrainerContext : IdentityDbContext<User>
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Examination> Examinations { get; set; }
        public DbSet<Results> Results { get; set; }

        public TrainerContext(DbContextOptions<TrainerContext> options)
        : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
 