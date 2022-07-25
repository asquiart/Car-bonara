
using CarbonaraWebAPI.Data;
using CarbonaraWebAPI.Infrastructure.Jwt;
using CarbonaraWebAPI.Model.DAO;
using CarbonaraWebAPI.Model.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CarbonaraWebAPI.Services
{
    public class AuthService
    {
        public readonly UserService _userService;
        private readonly TokenManagement _tokenManagement;

        public AuthService(AppDbContext appContext, TokenManagement tokenManagement)
        {
            _userService = new UserService(appContext);
            _tokenManagement = tokenManagement;
        }

        public async Task<LoginDTO> Login(string email, string password)
        {
            AuthData authData = await _userService.FindByEmailAsync(email);
            await _userService.SetLastLogin(email);
            if (authData == null || !ValidatePasswordHash(password, authData))
                return null;
            var claims = await BuildClaimsAsync(authData.Id);
            if (claims == null)
                return null;
            var token = CreateJWTtoken(claims);
            return await CreateLoginDataAsync(token, authData.Person);
        }

        public async Task<LoginDTO> Register(string email, string password)
        {
            if (await _userService.FindByEmailAsync(email) != null)
                return null;
            AuthData authData = CreatePasswordHash(password);
            if (!await _userService.AddAuthDataAsync(authData, email))
                return null;
            var claims = await BuildClaimsAsync(authData.Id);
            if (claims == null)
                return null;
            var token = CreateJWTtoken(claims);
            return await CreateLoginDataAsync(token, authData.Person);
        }

        

        public static AuthData CreatePasswordHash(string password)
        {
            AuthData auth = new();
            using (var hmac = new HMACSHA512())
            {
                auth.PasswordSalt = hmac.Key;
                auth.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
            return auth;
        }

        public bool ValidatePasswordHash(string password, AuthData authData)
        {
            using (var hmac = new HMACSHA512(authData.PasswordSalt))
            {
                return hmac.ComputeHash(Encoding.UTF8.GetBytes(password)).SequenceEqual(authData.PasswordHash);
            }
        }
        /*
         *      user     actor     0                         1                        2
         * 5. employee, employee gleiche IDs person   -> user null     | actor -> employee not null
         * 3. user,     user     gleiche IDs person   -> user not null | actor -> employee null
         * 2. user,     employee unterschiedliche IDs -> user not null | actor -> employee not null
         *                   ERLAUBT
         * 3. employee, employee unterschiedliche IDs -> user null     | actor -> employee not null 
         *                   VERBOTEN:(
         * 4. user,     employee unterschiedliche IDs -> user not null | actor -> employee not null
         * 5. user,     user     unterschiedliche IDs -> user not null | actor -> employee null
         *                  wird abgefangen
         *                       gleiche IDs person   -> user not null | actor -> employee not null
         *                       gleiche IDs person   -> user null | actor -> employee null
         *                  ist übring
         *  
         *  Rollen:
         *  Anonymous (Nicht angemeldet) -> kann sich registrieren und das Buchungssystem nutzen aber nicht buchen
         *   -> kann Benutzerdaten ändern und das Buchungssystem nutzen aber nicht buchen
         *  Authorized -> kann buchen, ist auch angemeldet
         *  Employee -> kann die Benutzer verwalten und sich als benutzer ausgeben
         *  Admin -> kann die Employees verwalten, ist auch Employee
         *  
         */
        // TODO: Test schreiben und in doku übernehmen
        public async Task<List<Claim>?> BuildClaimsAsync(int personID, int actorID = -1)
        {
            bool differentIDs = actorID != -1;
            actorID = differentIDs ? actorID : personID;
            

            User? user = await _userService.GetUserById(personID);
            Employee? employee = await _userService.GetEmployeeById(actorID);

            if (!(
                 (!differentIDs && user == null && employee != null) ||
                 (!differentIDs && user != null && employee == null) ||
                 ( differentIDs && user != null && employee != null) ))
                return null;


            List<Claim> claims = new()
            {
                new Claim("User", personID.ToString()),
                new Claim("Actor", actorID.ToString())
            };

            if (user != null && user.UserState_ == User.UserState.Authorized)
                claims.Add(new Claim(ClaimTypes.Role, "Authorized"));
            if (employee != null)
                claims.Add(new Claim(ClaimTypes.Role, "Employee"));
            if (employee != null && employee.IsAdmin)
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));

            return claims;
        }

        public string CreateJWTtoken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                _tokenManagement.Issuer,
                _tokenManagement.Audience,
                claims.ToArray(),
                expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        public async Task<LoginDTO> CreateLoginDataAsync(string token, Person person)
        {
            User user = await _userService.GetUserById(person.Id);
            bool isUser = user != null;
            return new LoginDTO(token, isUser, new PersonDTO(person));
        }

        public class UserService
        {
            private readonly AppDbContext _dbContext;

            public UserService(AppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<bool> AddAuthDataAsync(AuthData authData, string email)
            {
                Person? result = await _dbContext.Person.SingleOrDefaultAsync(p => p.Email == email);
                if (result == null) return false;
                authData.Person = result;
                await _dbContext.AuthData.AddAsync(authData);
                return await _dbContext.SaveChangesAsync() == 1;
            }

            public async Task<bool> ChangeAuthDataAsync(AuthData authData, int id)
            {
                AuthData result = await _dbContext.AuthData.SingleAsync(p => p.Id == id);
                result.PasswordSalt = authData.PasswordSalt;
                result.PasswordHash = authData.PasswordHash;
                return await _dbContext.SaveChangesAsync() == 1;
            }

            public async Task<bool> SetLastLogin(string email)
            {
                Person? result = await _dbContext.Person.SingleOrDefaultAsync(p => p.Email == email);
                if (result == null) return false;
                result.LastLogin = DateTime.UtcNow;
                return await _dbContext.SaveChangesAsync() == 1;
            }

            public async Task<Employee?> GetEmployeeById(int id)
            {
                return await _dbContext.Employee.SingleOrDefaultAsync(e => e.Id == id);
            }

            public async Task<User?> GetUserById(int id)
            {
                return await _dbContext.User.Where(u => u.Id == id).Include(u => u.Person).FirstOrDefaultAsync();
            }

            public async Task<AuthData?> FindByEmailAsync(string email)
            {
                return await _dbContext.AuthData.SingleOrDefaultAsync(a => a.Person.Email == email);
            }

            public async Task<AuthData?> FindByIDAsync(int ID)
            {
                return await _dbContext.AuthData.SingleAsync(a => a.Person.Id == ID);
            }
        }
    }
}
