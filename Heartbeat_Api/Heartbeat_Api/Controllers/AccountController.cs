using Heartbeat_Api.Data;
using Heartbeat_Api.Dtos;
using Heartbeat_Api.Entities;
using Heartbeat_Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Heartbeat_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;
        private readonly ICalculationService _calculationService;

        public AccountController(IConfiguration config, DataContext context, ITokenService tokenService,
            ICalculationService calculationService)
        {
            _config = config;
            _context = context;
            _tokenService = tokenService;
            _calculationService = calculationService;
        }

        public IActionResult Index()
        {
            return View("Users Account");
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            try
            { 
                if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

                using var hmac = new HMACSHA512();
                var user = new AgentUser
                {
                    UserName = registerDto.Username.ToLower(),
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                    PasswordSalt = hmac.Key
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();


                //Add new registered agent
                _calculationService.AddRegisteredAgents(user.Id);

                return new UserDto
                {
                    Username = user.UserName,
                    Token = _tokenService.CreateToken(user)
                };
                //return Ok(user.UserName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }             
        }
         
        [HttpDelete("unregister")]       
        public async Task<ActionResult> Unregister(UserParamDto userParam)
        {
            var entity = await GetUser(userParam.Id); 
            if (entity == null) 
                return Unauthorized("Invalid username");


            _context.Set<AgentUser>().Remove(entity);

            _context.SaveChanges();

            _calculationService.RemoveRegisteredAgents(userParam.Id);


            return Ok("user removed");
        }
         
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto logindto)
        {
            try
            {
                var maxAgentsAllowed = int.Parse(_config["MaxAgentsAllowed"]);

                //if (TotalRegisteredAgents() >= maxAgentsAllowed)
                //    return BadRequest("Error - reached maximum Agents");

                if (_calculationService.TotalRegisteredAgents() >= maxAgentsAllowed)
                    return BadRequest("Error - reached maximum Agents");

                var user = await _context.Users
                    .SingleOrDefaultAsync(x => x.UserName == logindto.Username);

                if (user == null) return Unauthorized("Invalid username");

                using var hmac = new HMACSHA512(user.PasswordSalt);

                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(logindto.Password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
                }

                return new UserDto
                {
                    Username = user.UserName,
                    Token = _tokenService.CreateToken(user)
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
         
        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

        private async Task<AgentUser> GetUser(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);

            return user;
        }

       
    }
}
