using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly TodoContext _context;

        public EmployeesController(TodoContext context)
        {
            _context = context;

            if (!_context.Employee.Any())
            {
                List<Employee> employees = new List<Employee>();

                using (StreamReader r = new StreamReader(GetFilePath()))
                {
                    string json = r.ReadToEnd();
                    employees = JsonConvert.DeserializeObject<List<Employee>>(json);
                }

                _context.Employee.AddRange(employees);
                _context.SaveChanges();
            }
        }

        // GET: api/Employees?location=UK&name=
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee(string location = "", string name ="")
        {
            if(!string.IsNullOrEmpty(location))
            {
                return await _context.Employee.Where(e => e.Location.Equals(location)).ToListAsync();

            }

            return await _context.Employee.ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employee.FindAsync(id);

            if(employee == null)
            {
                return NotFound($"Employee with {id} does not exist");
            }
            
            return employee;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            _context.Employee.Add(employee);
            await _context.SaveChangesAsync();                     

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            if (!Request.Headers.Contains(new KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>("Authorization", "admin")))
            {
                return Unauthorized();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok($"Employee with {id} was deleted");
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }

        private string GetFilePath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Employees.json");
        }
    }
}
