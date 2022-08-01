using ErenYeager.Models;
using Microsoft.EntityFrameworkCore;

namespace ErenYeager.Data {
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {

        }

        public DbSet<Teacher> Teachers {
            get; set;
        }

        public DbSet<Department> Departments {
            get; set;
        }



    }
}
