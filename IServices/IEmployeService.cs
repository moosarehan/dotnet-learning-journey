using myfirstrestapi.Dto;
using myfirstrestapi.Entities;

namespace myfirstrestapi.IServices
{
    public interface IEmployeService
    {
        Task<Tuple<int, List<Employedto>>> getallemployees();
        Task<Tuple<int, string, Employe?>> createEmploye(Employedto e);
        Task<Tuple<int, string, Employe?>> UpdateEmployee(Employedto employee);

        Task<Tuple<int, string>> DeleteEmployee(Guid id);

        Task<Tuple<int, Employedto?>> GetEmployeeById(Guid id);
    }
}
