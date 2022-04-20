using Heartbeat_Api.Data;
using Heartbeat_Api.Entities;
using Heartbeat_Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Heartbeat_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        public DataContext _context { get; }
        private readonly ICalculationService _calculationService;


        public UsersController(DataContext context, ICalculationService calculationService)
        {
            _context = context;
            _calculationService = calculationService;
        }

        [HttpGet("users")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AgentUser>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            return users;
        }

        [HttpGet("registered_agents")]
        public int TotalRegisteredAgents()
        {
            return _calculationService.TotalRegisteredAgents();
        }

        [HttpGet("unregistered_agents")]
        public int TotalUnRegisteredAgents()
        {
            return _calculationService.TotalUnRegisteredAgents();   
        }

        [Authorize]
        [HttpGet("heartbeats")]
        public void SetHeartbeatsPerUser (int id)
        {
            _calculationService.SetHeartbeatsForAgents(id);
        }
         
        [HttpGet("user_heartbeatcount")]
        public int TotalHeartbeatsPerUser(int id)
        { 
           return  _calculationService.TotalHeartbeatsPerAgents(id);
        }
    }
}
