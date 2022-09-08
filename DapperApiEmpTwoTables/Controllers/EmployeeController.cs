using DapperApiEmpTwoTables.Model;
using DapperApiEmpTwoTables.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DapperApiEmpTwoTables.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepo;
        public EmployeeController(IEmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var Employee = await _employeeRepo.GetEmployees();
                return Ok(Employee);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("/id")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            try
            {
                var Employee = await _employeeRepo.GetEmployeeById(id);
                return Ok(Employee);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Employee emp)
        {
            try
            {
                var Employee = await _employeeRepo.CreateEmployee(emp);
                if (Employee == null)
                {
                    return StatusCode(409, "The request could not be processed because of conflict in the request");
                }
                else
                {
                    return StatusCode(200, string.Format("Record Inserted Successfuly", Employee));
                }
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee(Employee emp)
        {
            try
            {
                var Employee = await _employeeRepo.UpdateEmployee(emp);
                return Ok(Employee);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var Employee = await _employeeRepo.DeleteEmployee(id);
                return Ok(Employee);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

    }
}
