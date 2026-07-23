using myfirstrestapi.Data;
using myfirstrestapi.Dto;
using myfirstrestapi.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace myfirstrestapi.Services
{
    public class AuthService(AppDBcontext _context):IAuthService
    {

        public async Task<Tuple<int, string>> LoginUser(UserDto user)
        {
            try
            {
                var exsist = await _context.AccountUser.FirstOrDefaultAsync(x => x.email == user.email);
                if (exsist == null)
                {
                    return new Tuple<int, string>(0, "this user not exsist");
                }
                var passwordhasher = new PasswordHasher<string>();

                var verify= passwordhasher.VerifyHashedPassword(user.email, exsist.password, user.password);
                if(verify==PasswordVerificationResult.Success)
                {
                    return new Tuple<int, string>(2, "login success");    
                }
                else if (verify==PasswordVerificationResult.SuccessRehashNeeded)
                {
                    user.password = hash(user);
                    _context.AccountUser.Update(exsist);
                    _context.SaveChanges();
                }
                else if(verify== PasswordVerificationResult.Failed)
                {
                    return new Tuple<int, string>(0, "password is not correct");
                }


                return new Tuple<int, string>(2, "login success");
            }
            catch (Exception ex)
            {
                throw;


            }
        }

        public async Task<Tuple<int, string>> register(UserDto user)
        {

            try
            {
                var exsist = await _context.AccountUser.AnyAsync(x => x.email == user.email);
                if (exsist)
                {
                    return new Tuple<int, string>(0, "this user already exsist");
                }


                _context.AccountUser.Add(new Entities.User
                {
                    id = Guid.NewGuid(),
                    name = user.name,
                    email = user.email,
                    password = hash(user)
                });

                await _context.SaveChangesAsync();
                return new Tuple<int, string>(1, "register success");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private string hash(UserDto user)
        {
            var passwordhasher=new PasswordHasher<string>();
            var hash=passwordhasher.HashPassword(user.email, user.password);
            return hash;

        }
    }
}
