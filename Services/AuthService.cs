using myfirstrestapi.Data;
using myfirstrestapi.Dto;
using myfirstrestapi.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace myfirstrestapi.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDBcontext _context;

        public AuthService(AppDBcontext context)
        {
            _context = context;
        }

        private string GetJwtToken(UserDto dto)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, dto.name ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, dto.id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("QtGin2leKfRhbQlvv9JzF9krMMSYIIy13PaRhN0tBc8"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "http://localhost:5115",
                audience: "http://localhost:5115",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Tuple<int, TokenDto>> LoginUser(UserDto user)
        {
            try
            {
                var exist = await _context.AccountUser.FirstOrDefaultAsync(x => x.email == user.email);
                if (exist == null)
                {
                    return Tuple.Create(0, new TokenDto { Token = null, Message = "this user not exist" });
                }

                var passwordhasher = new PasswordHasher<string>();
                var verify = passwordhasher.VerifyHashedPassword(user.email, exist.password, user.password);

                if (verify == PasswordVerificationResult.Success)
                {
                    var dto = new UserDto { email = user.email, id = exist.id };
                    var token = GetJwtToken(dto);
                    return Tuple.Create(2, new TokenDto { Token = token, Message = "login success" });
                }
                else if (verify == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    var dto = new UserDto { email = user.email, id = exist.id };
                    var token = GetJwtToken(dto);
                    // rehash and update stored password
                    exist.password = hash(user);
                    _context.AccountUser.Update(exist);
                    await _context.SaveChangesAsync();
                    return Tuple.Create(2, new TokenDto { Token = token, Message = "login success" });
                }
                else // Failed
                {
                    return Tuple.Create(0, new TokenDto { Token = null, Message = "password is not correct" });
                }
            }
            catch (Exception)
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
                    return Tuple.Create(0, "this user already exist");
                }

                _context.AccountUser.Add(new Entities.User
                {
                    id = Guid.NewGuid(),
                    name = user.name,
                    email = user.email,
                    password = hash(user)
                });

                await _context.SaveChangesAsync();
                return Tuple.Create(1, "register success");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string hash(UserDto user)
        {
            var passwordhasher = new PasswordHasher<string>();
            var hash = passwordhasher.HashPassword(user.email, user.password);
            return hash;
        }
    }
}
