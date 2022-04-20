using Heartbeat_Api.Data;
using Heartbeat_Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Heartbeat_Api.Services
{
    public class CalculationService : ICalculationService
    { 
        private IDictionary<int, int> _heartbeatsPerAgents { get; set; } = new Dictionary<int, int>();
        private int  _totalUnRegisteredAgents { get; set; } = 0;
        private readonly DataContext _context;        
        private readonly IConfiguration _config;

        public CalculationService(IConfiguration config, DataContext context)
        {
            _config = config;
            _context = context;
           
        }

        public void AddRegisteredAgents(int id)
        {
            _heartbeatsPerAgents.Add(id, 0);
        }

        public void RemoveRegisteredAgents(int id)
        {
            if (_heartbeatsPerAgents.ContainsKey(id))
            {
                _heartbeatsPerAgents.Remove(id);
                //_totalUnRegisteredAgents++;
            }
                
        }

        public void AddUnRegisteredAgents()
        {
            _totalUnRegisteredAgents++;
        }

        public int TotalRegisteredAgents()
        {
            return _heartbeatsPerAgents.Count();
        }
      
        public int TotalUnRegisteredAgents()
        {
            return _totalUnRegisteredAgents;
        }

        public int TotalHeartbeatsPerAgents(int id)
        {
            if (_heartbeatsPerAgents.ContainsKey(id))
                return _heartbeatsPerAgents[id];

            return -1;
        }

        public void SetHeartbeatsForAgents(int id)
        {
            if (_heartbeatsPerAgents.ContainsKey(id))
                _heartbeatsPerAgents[id]++;
             
        }

        

    }
}
