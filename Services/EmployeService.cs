using Microsoft.EntityFrameworkCore;
using myfirstrestapi.Data;
using myfirstrestapi.Dto;
using myfirstrestapi.Entities;
using myfirstrestapi.IServices;

namespace myfirstrestapi.Services
{
  

    public class EmployeService : IEmployeService
    {
        private readonly AppDBcontext _db;

        public EmployeService(AppDBcontext appDBcontext)
        {
            _db = appDBcontext;
        }

       public async Task<Tuple<int,List<Employedto>>> getallemployees()
        {
            return new Tuple<int,List<Employedto>>(1,await _db.Employees.AsNoTracking().Select(x=>new Employedto
            {
                id = x.id,
                name = x.name,
                CreatedDate = x.CreatedDate,
                EmailAddress = x.EmailAddress,
                modifieddate = x.modifieddate,
                position    = x.position,
                department = x.department,  
                DOB = x.DOB
            }).ToListAsync());

        }

        public async Task<Tuple<int, string,Employe?>> createEmploye(Employedto e)
        {
            var user = await _db.Employees.AnyAsync(x => x.EmailAddress == e.EmailAddress);
            if (user)
            {
                return new Tuple<int, string,Employe?>(0, "Email Address Already Exist",null); 

            }
            else
            {
                var emp = new Entities.Employe
                {
                    id = Guid.NewGuid(),
                    name = e.name,
                    CreatedDate = DateTime.Now,
                    EmailAddress = e.EmailAddress,
                    modifieddate = null,
                    position = e.position,
                    department = e.department,
                    DOB = e.DOB
                };
                await _db.Employees.AddAsync(emp);
                await _db.SaveChangesAsync();
                return new Tuple<int, string,Employe?>(1, "Employee Created Successfully",emp);
            }
        }


        public async Task<Tuple<int, string,Employe?>> UpdateEmployee(Employedto employee)
        {
            var existing = await _db.Employees.FirstOrDefaultAsync(x => x.EmailAddress == employee.EmailAddress);

            if (existing == null)
            {
                return new Tuple<int, string,Employe?>(0, "Employee Not Exist With This Email ID",null);
            }

            existing.position = string.IsNullOrWhiteSpace(employee.position) ? existing.position : employee.position;
            existing.DOB = employee.DOB ?? existing.DOB;
            existing.name = string.IsNullOrWhiteSpace(employee.name) ? existing.name : employee.name;
            existing.EmailAddress = string.IsNullOrWhiteSpace(employee.EmailAddress) ? existing.EmailAddress : employee.EmailAddress;
            existing.department = string.IsNullOrWhiteSpace(employee.department) ? existing.department : employee.department;
            existing.modifieddate = DateTime.UtcNow; // Update the modified timestamp

            _db.Employees.Update(existing);
            await _db.SaveChangesAsync();

            return new Tuple<int, string,Employe?>(1, "Employee Updated Successfully!",existing);
        }



        public async Task<Tuple<int, string>> DeleteEmployee(Guid id)
        {
            var existing = await _db.Employees.FirstOrDefaultAsync(x => x.id == id);
            if (existing == null)
            {
                return new Tuple<int, string>(0, "Employee Not Found");
            }

            _db.Employees.Remove(existing);
            await _db.SaveChangesAsync();

            return new Tuple<int, string>(1, "Employee Deleted Successfully!");


        }

        public async Task<Tuple<int, Employedto?>> GetEmployeeById(Guid id)
        {
            var data = await _db.Employees
                .AsNoTracking()
                .Select(x => new Employedto
                {
                    id = x.id,
                    CreatedDate = x.CreatedDate,
                    modifieddate = x.modifieddate,
                    department = x.department,
                    DOB = x.DOB,
                    name = x.name,
                    EmailAddress = x.EmailAddress,
                    position = x.position
                })
                .FirstOrDefaultAsync(x => x.id == id);

            if (data == null)
            {
                return new Tuple<int, Employedto?>(0, null);
            }

            return new Tuple<int, Employedto?>(1, data);
        }
    }
}
