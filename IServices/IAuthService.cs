using System;
using System.Threading.Tasks;
using myfirstrestapi.Dto;

namespace myfirstrestapi.IServices
{
    public interface IAuthService
    {
        Task<Tuple<int, string>> LoginUser(UserDto user);
        Task<Tuple<int, string>> register(UserDto user);
    }
}
