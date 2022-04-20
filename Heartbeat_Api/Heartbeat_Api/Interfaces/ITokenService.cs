using Heartbeat_Api.Entities;

namespace Heartbeat_Api.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AgentUser user);
    }
}
