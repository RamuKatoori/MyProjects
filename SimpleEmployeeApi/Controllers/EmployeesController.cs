using Microsoft.AspNetCore.Mvc;
using SimpleEmployeeApi.Dtos;
using SimpleEmployeeApi.Models;
using SimpleEmployeeApi.Services;

namespace SimpleEmployeeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _svc;
        private readonly IConfiguration _config;

        public EmployeesController(IEmployeeService svc, IConfiguration config)
        {
            _svc = svc;
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetAll()
        {
            var list = await _svc.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employee>> Get(int id)
        {
            var emp = await _svc.GetByIdAsync(id);
            if (emp == null) return NotFound();
            return emp;
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> Create(EmployeeDto dto)
        {
            var employee = new Employee { Name = dto.Name, Email = dto.Email, Role = dto.Role };
            var created = await _svc.CreateAsync(employee);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, EmployeeDto dto)
        {
            var existing = await _svc.GetByIdAsync(id);
            if (existing == null) return NotFound();
            existing.Name = dto.Name;
            existing.Email = dto.Email;
            existing.Role = dto.Role;
            var ok = await _svc.UpdateAsync(existing);
            return ok ? NoContent() : StatusCode(500);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _svc.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
