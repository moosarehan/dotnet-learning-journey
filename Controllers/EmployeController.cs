using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myfirstrestapi.Dto;
using myfirstrestapi.Entities;
using myfirstrestapi.GenericResponse;
using myfirstrestapi.IServices;
using myfirstrestapi.Services;

namespace myfirstrestapi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeController(IEmployeService employeService) : ControllerBase
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await employeService.getallemployees();
            if(!employees.Item2.Any())
            {
                return  NotFound(Response<List<Employedto>>.failure(null, "No employees found"));
            }
            return Ok(Response<List<Employedto>>.success(employees.Item2, "Employees retrieved successfully"));
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateEmployee([FromBody] Employedto employeeDto)
        {
            var result = await employeService.createEmploye(employeeDto);
            if (result.Item1 == 0)
            {
                return BadRequest(Response<string>.failure(null, result.Item2));
            }
            return Ok(Response<Employe>.success(result.Item3  , result.Item2));
        }

        [HttpPut("update")]
        public async Task<IActionResult> updateEmployee([FromBody] Employedto employeeDto)
        {
            var result = await employeService.UpdateEmployee(employeeDto);
            if (result.Item1 == 0)
            {
                return BadRequest(Response<string>.failure(null, result.Item2));
            }
            return Ok(Response<Employe>.success(result.Item3, result.Item2));
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var result = await employeService.DeleteEmployee(id);
            if (result.Item1 == 0)
            {
                return BadRequest(Response<string>.failure(null, result.Item2));
            }
            return Ok(Response<string>.success(null, result.Item2));
        }

        [HttpGet("GetEmployeeById/{id:guid}")]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            try
            {
                var result = await employeService.GetEmployeeById(id);

                if (result.Item1 == 0)
                {
                    return Ok(Response<string>.failure(null, "Data Not Found"));
                }

                return Ok(Response<Employedto>.success(result.Item2, "Data Found Successfully!"));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
