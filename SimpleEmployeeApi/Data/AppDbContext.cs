using Microsoft.EntityFrameworkCore;
using SimpleEmployeeApi.Models;

namespace SimpleEmployeeApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Employee> Employees => Set<Employee>();
    }
}
