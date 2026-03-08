using Microsoft.EntityFrameworkCore;
using SimpleEmployeeApi.Data;
using SimpleEmployeeApi.Models;

namespace SimpleEmployeeApi.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _db;
        public EmployeeService(AppDbContext db) => _db = db;

        public async Task<List<Employee>> GetAllAsync()
            => await _db.Employees.ToListAsync();

        public async Task<Employee?> GetByIdAsync(int id)
            => await _db.Employees.FindAsync(id);

        public async Task<Employee> CreateAsync(Employee employee)
        {
            _db.Employees.Add(employee);
            await _db.SaveChangesAsync();
            return employee;
        }

        public async Task<bool> UpdateAsync(Employee employee)
        {
            var exists = await _db.Employees.AnyAsync(e => e.Id == employee.Id);
            if (!exists) return false;
            _db.Employees.Update(employee);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var e = await _db.Employees.FindAsync(id);
            if (e == null) return false;
            _db.Employees.Remove(e);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
